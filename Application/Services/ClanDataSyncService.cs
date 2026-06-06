using System.Collections.Concurrent;
using System.Globalization;
using System.Net;
using Application.DTOs.Clans.ClanWarLeagues;
using Application.DTOs.Players;
using Application.Interfaces;
using Domain.Constants;
using Domain.Models;
using Domain.Models.ClanWarLeagues;
using Domain.Models.ClanWars;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class ClanDataSyncService(
    IAppDbContext dbContext,
    IClashApiClient apiClient,
    [FromKeyedServices("ClanTag")] string clanTag,
    ILogger<ClanDataSyncService> logger,
    IWarLeagueService warLeagueService) : IClanDataSyncService
{
    public async Task UpdateClanMembers(CancellationToken ct)
    {
        var clanMembers = await apiClient.GetClanMembersAsync(clanTag, cancellationToken: ct).UnwrapOrLogAsync(logger);

        if (clanMembers is null)
            return;

        var currentTags = clanMembers.Items.Select(m => m.Tag).ToHashSet();

        var dbMembersDict = await dbContext.ClanMembers.ToDictionaryAsync(m => m.Tag, ct);

        foreach (var dbMember in dbMembersDict.Values)
        {
            dbMember.IsNowInClan = currentTags.Contains(dbMember.Tag);
        }

        var playerResults = new ConcurrentBag<(string Tag, IApiResult<PlayerDto> Result)>();

        await Parallel.ForEachAsync(currentTags, ct, async (tag, _) =>
        {
            var result = await apiClient.GetPlayerAsync(tag, ct);
            playerResults.Add((tag, result));
        });

        foreach (var item in playerResults)
        {
            if (item.Result.Data is { } player)
            {
                if (dbMembersDict.TryGetValue(player.Tag, out var existingDbMember))
                {
                    existingDbMember.UpdateFromPlayerDto(player);
                    existingDbMember.IsNowInClan = true;
                }
                else
                {
                    var newMember = new ClanMember { Tag = player.Tag, IsNowInClan = true, Name = player.Name };
                    newMember.UpdateFromPlayerDto(player);
                    dbContext.ClanMembers.Add(newMember);
                }
            }
            else if (item.Result.Error is { } error)
            {
                logger.LogError("Api error while updating player {Tag}: {ErrorMessage}", item.Tag, error.Message);
            }
        }

        await dbContext.SaveChangesAsync(ct);
    }

    public async Task UpdateSeasonStats(CancellationToken ct)
    {
        var leagueData = await apiClient.GetLeagueSeasonsAsync(cancellationToken: ct).UnwrapOrLogAsync(logger);

        if (leagueData is null)
            return;

        var parsedDateTime = DateTimeOffset.ParseExact(
            leagueData.Items[^1].Id,
            "'v2-'yyyy-MM-dd'T'HH:mm:ss'Z'",
            CultureInfo.InvariantCulture
        );

        var latestSeason = DateOnly.FromDateTime(parsedDateTime.UtcDateTime);

        var clanMembers = await apiClient.GetClanMembersAsync(clanTag, cancellationToken: ct).UnwrapOrLogAsync(logger);
        if (clanMembers is null)
            return;

        var apiMemberTags = clanMembers.Items.Select(m => m.Tag).ToList();

        var allowedDbMemberTags = await dbContext.ClanMembers
            .Where(m => apiMemberTags.Contains(m.Tag))
            .Select(m => m.Tag)
            .ToHashSetAsync(ct);

        var existingStats = await dbContext.SeasonStats
            .Where(s => s.SeasonDate == latestSeason && allowedDbMemberTags.Contains(s.PlayerTag))
            .ToDictionaryAsync(s => s.PlayerTag, ct);

        foreach (var member in clanMembers.Items.Where(member => allowedDbMemberTags.Contains(member.Tag)))
        {
            if (existingStats.TryGetValue(member.Tag, out var dbStat))
            {
                dbStat.Donations = member.Donations;
                dbStat.DonationsReceived = member.DonationsReceived;
            }

            else
            {
                var newStat = new SeasonStats
                {
                    SeasonDate = latestSeason,
                    PlayerTag = member.Tag,
                    Donations = member.Donations,
                    DonationsReceived = member.DonationsReceived
                };

                dbContext.SeasonStats.Add(newStat);
            }
        }

        await dbContext.SaveChangesAsync(ct);
    }

    public async Task<bool> UpdateClanWar(CancellationToken ct)
    {
        var clanWarData = await apiClient.GetCurrentClanWarAsync(clanTag, ct).UnwrapOrLogAsync(logger);

        if (clanWarData is null)
            return false;

        if (clanWarData.State != ClanWarState.WarEnded && clanWarData.State != ClanWarState.InWar)
            return false;

        var currentWar = await dbContext.ClanWars
            .Include(cw => cw.PlayersPerformances)
            .SingleOrDefaultAsync(
                s => s.OpponentClanTag == clanWarData.Opponent!.Tag && s.StartTime == clanWarData.StartTime, ct);

        if (currentWar is null)
        {
            currentWar = new ClanWar
            {
                PlayersPerformances = []
            };
            dbContext.ClanWars.Add(currentWar);
        }

        currentWar.UpdateFromClanWarDto(clanWarData);

        currentWar.PlayersPerformances ??= [];


        var opponentsLookup = clanWarData.Opponent!.Members?
            .ToDictionary(m => m.Tag) ?? [];

        foreach (var memberDto in clanWarData.Clan.Members!)
        {
            var performance = currentWar.PlayersPerformances
                .SingleOrDefault(p => p.PlayerTag == memberDto.Tag);

            if (performance is null)
            {
                performance = new ClanWarPlayerPerformance
                {
                    PlayerTag = memberDto.Tag,
                    MapPosition = (short)memberDto.MapPosition,
                    TownHallLevel = (short)memberDto.TownhallLevel,
                    War = currentWar
                };
                currentWar.PlayersPerformances.Add(performance);
            }

            var attacks = memberDto.Attacks?.OrderBy(a => a.Order).ToList() ?? [];

            var attack1 = attacks.Count > 0 ? attacks[0] : null;
            performance.Attack1Stars = (short?)attack1?.Stars;
            performance.Attack1Destruction = (short?)attack1?.DestructionPercentage;
            performance.Attack1Duration = (short?)attack1?.Duration;
            performance.Defender1Tag = attack1?.DefenderTag;

            var defender1 = attack1?.DefenderTag is not null ? opponentsLookup.GetValueOrDefault(attack1.DefenderTag) : null;
            performance.Opponent1Position = (short?)defender1?.MapPosition;
            performance.Opponent1TownHallLevel = (short?)defender1?.TownhallLevel;

            var attack2 = attacks.Count > 1 ? attacks[1] : null;
            performance.Attack2Stars = (short?)attack2?.Stars;
            performance.Attack2Destruction = (short?)attack2?.DestructionPercentage;
            performance.Attack2Duration = (short?)attack2?.Duration;
            performance.Defender2Tag = attack2?.DefenderTag;

            var defender2 = attack2?.DefenderTag is not null ? opponentsLookup.GetValueOrDefault(attack2.DefenderTag) : null;
            performance.Opponent2Position = (short?)defender2?.MapPosition;
            performance.Opponent2TownHallLevel = (short?)defender2?.TownhallLevel;
        }

        var savedRows = await dbContext.SaveChangesAsync(ct);
        return savedRows > 0;
    }

    public async Task RefreshMaterializedViews(CancellationToken ct)
    {
        await dbContext.RefreshPlayerSummariesViewAsync(ct);
        await dbContext.RefreshClanWarSummariesViewAsync(ct);
    }

    public async Task CleanupStuckWars(CancellationToken ct)
    {
        var timeThreshold = DateTime.UtcNow.AddHours(-1);

        var thresholdUnspecified = DateTime.SpecifyKind(timeThreshold, DateTimeKind.Unspecified);

        var stuckWars = await dbContext.ClanWars
            .Where(cw => cw.State != ClanWarState.WarEnded && cw.EndTime < thresholdUnspecified)
            .ToListAsync(ct);

        if (stuckWars.Count == 0)
            return;

        var warLog = await apiClient.GetClanWarLogAsync(clanTag, cancellationToken: ct).UnwrapOrLogAsync(logger);

        if (warLog is null)
        {
            logger.LogError("Stuck wars were found, but server failed to get war log");
            return;
        }

        var warLogDict = warLog.Items.ToDictionary(m => m.EndTime.Ticks);


        foreach (var war in stuckWars)
        {
            if (warLogDict.TryGetValue(war.EndTime.Ticks, out var logEntry))
            {
                war.UpdateStuckWarFromWarLogEntryDto(logEntry);
                logger.LogWarning("War {WarId} against {Opponent} was forcibly closed due to downtime, updated from war log.",
                    war.Id, war.OpponentClanName);
            }
            else
            {
                war.State = ClanWarState.WarEnded;
                logger.LogError("War {WarId} against {Opponent} not found in WarLog! Forcibly set status to WarEnded without stats update.",
                    war.Id, war.OpponentClanName);
            }
        }

        await dbContext.SaveChangesAsync(ct);
    }

    private static void UpdateMemberData(ClanMember entity, PlayerDto playerApiData)
    {
        entity.Name = playerApiData.Name;
        entity.Trophies = (short)playerApiData.Trophies;
        entity.BestBuilderBaseTrophies = (short)playerApiData.BestBuilderBaseTrophies;
        entity.BuilderBaseTrophies = (short)playerApiData.BuilderBaseTrophies;
        entity.ExpLevel = (short)playerApiData.ExpLevel;
        entity.Role = playerApiData.Role;
        entity.TownHallLevel = (short)playerApiData.TownHallLevel;
        entity.BuilderHallLevel = (short)playerApiData.BuilderHallLevel;
        entity.WarPreference = playerApiData.WarPreference;
        entity.ClanCapitalContributions = playerApiData.ClanCapitalContributions;
        entity.WarStars = playerApiData.WarStars;
    }

    private static void UpdateClanWarData(ClanWar entity, ClanWarDto apiData)
    {
        entity.State = apiData.State;
        entity.EndTime = apiData.EndTime;
        entity.StartTime = apiData.StartTime;
        entity.ExpEarned = (short?)apiData.Clan.ExpEarned;
        entity.TeamSize = (short)apiData.TeamSize;

        entity.OpponentAttacks = (short)apiData.Opponent!.Attacks;
        entity.OpponentClanLevel = (short)apiData.Opponent.ClanLevel;
        entity.OpponentClanName = apiData.Opponent.Name;
        entity.OpponentClanTag = apiData.Opponent.Tag;
        entity.OpponentDestructionPercentage = apiData.Opponent.DestructionPercentage;
        entity.OpponentStars = (short)apiData.Opponent.Stars;

        entity.OurAttacks = (short)apiData.Clan.Attacks;
        entity.OurDestructionPercentage = apiData.Clan.DestructionPercentage;
        entity.OurStars = (short)apiData.Clan.Stars;
    }

    private static void UpdateClanWarData(ClanWar entity, ClanWarLogEntryDto apiData)
    {
        entity.State = ClanWarState.WarEnded;
        entity.EndTime = apiData.EndTime;
        if (entity.StartTime == DateTime.MinValue)
            entity.StartTime = apiData.EndTime - new TimeSpan(48, 0, 0); // Approximately

        entity.ExpEarned = (short?)apiData.Clan.ExpEarned;
        entity.TeamSize = (short)apiData.TeamSize;

        entity.OpponentAttacks = (short)apiData.Opponent.Attacks;
        entity.OpponentClanLevel = (short)apiData.Opponent.ClanLevel;
        entity.OpponentClanName = apiData.Opponent.Name;
        entity.OpponentClanTag = apiData.Opponent.Tag;
        entity.OpponentDestructionPercentage = apiData.Opponent.DestructionPercentage;
        entity.OpponentStars = (short)apiData.Opponent.Stars;

        entity.OurAttacks = (short)apiData.Clan.Attacks;
        entity.OurDestructionPercentage = apiData.Clan.DestructionPercentage;
        entity.OurStars = (short)apiData.Clan.Stars;
    }

    private async Task<ClanMemberListDto?> GetClanMembers(string tag, CancellationToken ct)
    {
        var clanResult = await apiClient.GetClanMembersAsync(tag, cancellationToken: ct);

        if (clanResult.Error is not { } clanError)
            return clanResult.Data;

        logger.LogError("Api error while getting clan members: {ErrorMessage}", clanError.Message);

        return null;
    }

    private async Task<ClanWarDto?> GetClanWar(string tag, CancellationToken ct)
    {
        var warResult = await apiClient.GetCurrentClanWarAsync(tag, ct);

        if (warResult.Error is not { } clanError)
            return warResult.Data;

        logger.LogError("Api error while getting current clan war: {ErrorMessage}", clanError.Message);

        return null;
    }

    private async Task<LeagueSeasonListDto?> GetLeagueSeasons(CancellationToken ct)
    {
        var leagueResult = await apiClient.GetLeagueSeasonsAsync(cancellationToken: ct);

        if (leagueResult.Error is not { } leagueResultError)
            return leagueResult.Data;

        logger.LogError("Api error while getting league season: {ErrorMessage}", leagueResultError.Message);

        return null;
    }

    private async Task<ClanWarLogDto?> GetClanWarLog(CancellationToken ct)
    {
        var logResult = await apiClient.GetClanWarLogAsync(clanTag, cancellationToken: ct);

        if (logResult.Error is not { } logResultError)
            return logResult.Data;

        logger.LogError("Api error while getting war log: {ErrorMessage}", logResultError.Message);

        return null;
    }
}