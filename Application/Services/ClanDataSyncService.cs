using System.Collections.Concurrent;
using System.Globalization;
using System.Net;
using Application.DTOs.Clans.ClanWarLeagues;
using Application.DTOs.Players;
using Application.Extensions;
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

    public async Task RefreshCwMaterializedViews(CancellationToken ct)
    {
        await dbContext.RefreshCwPlayerSummariesViewAsync(ct);
        await dbContext.RefreshCwClanWarSummariesViewAsync(ct);
    }

    public async Task RefreshCwlMaterializedViews(CancellationToken ct)
    {
        await dbContext.RefreshCwlPlayerSummariesViewAsync(ct);
        await dbContext.RefreshCwlClanWarSummariesViewAsync(ct);
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

    public async Task<bool> UpdateClanLeagueWars(CancellationToken ct)
    {
        var leagueResult = await apiClient.GetCurrentClanWarLeagueGroupAsync(clanTag, ct);

        if (leagueResult.StatusCode == HttpStatusCode.NotFound)
            return false;

        if (leagueResult.Error != null)
        {
            logger.LogError("Api error while getting league group: {ErrorMessage}", leagueResult.Error.Message);
            return false; // Если ошибка, дальше идти нет смысла
        }

        var leagueGroup = leagueResult.Data;

        if (leagueGroup?.State != ClanWarLeagueState.WarEnded && leagueGroup?.State != ClanWarLeagueState.InWar)
            return false;

        await SyncLeagueGroupAsync(leagueGroup, ct);

        var (downloadedWars, warRoundInfos) = await DownloadLeagueWarsAsync(leagueGroup, ct);

        if (downloadedWars.IsEmpty)
            return false;

        // Синхронизируем войны и статистику игроков
        await SyncWarsAndPerformancesAsync(leagueGroup.Season, downloadedWars, warRoundInfos, ct);

        var savedRows = await dbContext.SaveChangesAsync(ct);
        return savedRows > 0;
    }

    private async Task SyncLeagueGroupAsync(ClanWarLeagueGroupDto leagueGroup, CancellationToken ct)
    {
        var groupEntity = await dbContext.ClanWarLeagueGroups
            .FirstOrDefaultAsync(t => t.Season == leagueGroup.Season, ct);

        var rankings = await warLeagueService.CalculateClanWarLeagueRankings(leagueGroup, ct);
        var place = rankings != null ? warLeagueService.GetClanPlace(clanTag, rankings) : null;

        if (groupEntity == null)
        {
            groupEntity = new ClanWarLeagueGroup
            {
                Season = leagueGroup.Season,
                State = leagueGroup.State,
                TeamSize = await warLeagueService.GetLeagueWarTeamSize(leagueGroup, ct),
                Place = place
            };

            await dbContext.ClanWarLeagueGroups.AddAsync(groupEntity, ct);
        }
        else
        {
            groupEntity.State = leagueGroup.State;
            groupEntity.Place = place;
        }
    }

    private async Task<(ConcurrentDictionary<string, ClanWarLeaguerWarDto> Wars, List<(string Tag, short Round)> RoundInfos)>
        DownloadLeagueWarsAsync(ClanWarLeagueGroupDto leagueGroup, CancellationToken ct)
    {
        var warRoundInfos = leagueGroup.Rounds
            .Select((round, index) => new { RoundNumber = (short)(index + 1), round.WarTags })
            .SelectMany(r => r.WarTags
                .Where(tag => tag != "#0")
                .Select(tag => (Tag: tag, Round: r.RoundNumber)))
            .ToList();

        var warTags = warRoundInfos.Select(x => x.Tag).Distinct().ToList();
        var downloadedWars = new ConcurrentDictionary<string, ClanWarLeaguerWarDto>();

        await Parallel.ForEachAsync(warTags, ct, async (tag, token) =>
        {
            var war = await apiClient.GetClanWarLeagueWarAsync(tag, token).UnwrapOrLogAsync(logger);
            if (war != null)
            {
                downloadedWars.TryAdd(tag, war);
            }
        });

        return (downloadedWars, warRoundInfos);
    }

    private async Task SyncWarsAndPerformancesAsync(
        string season,
        ConcurrentDictionary<string, ClanWarLeaguerWarDto> downloadedWars,
        List<(string Tag, short Round)> warRoundInfos,
        CancellationToken ct)
    {
        var fetchedTags = downloadedWars.Keys.ToList();

        var existingDbWars = await dbContext.ClanWarLeagueWars
            .Where(w => fetchedTags.Contains(w.WarTag))
            .ToDictionaryAsync(w => w.WarTag, ct);

        var existingPerformances = await dbContext.ClanWarLeaguePlayerPerformances
            .Where(p => fetchedTags.Contains(p.WarTag))
            .ToListAsync(ct);

        var perfLookup = existingPerformances.ToDictionary(p => (p.WarTag, p.PlayerTag));

        foreach (var (tag, incomingWar) in downloadedWars)
        {
            if (incomingWar.Clan.Tag != clanTag && incomingWar.Opponent.Tag != clanTag)
                continue;

            if (!existingDbWars.TryGetValue(tag, out var dbWar))
            {
                var roundNumber = warRoundInfos.FirstOrDefault(x => x.Tag == tag).Round;

                dbWar = new ClanWarLeagueWar
                {
                    WarTag = tag,
                    State = incomingWar.State!,
                    Season = season,
                    Round = roundNumber
                };
                await dbContext.ClanWarLeagueWars.AddAsync(dbWar, ct);
            }

            dbWar.UpdateFromClanWarLeaguerWarDto(incomingWar, clanTag);

            var ourSide = incomingWar.Clan.Tag == clanTag ? incomingWar.Clan : incomingWar.Opponent;
            var oppSide = incomingWar.Clan.Tag == clanTag ? incomingWar.Opponent : incomingWar.Clan;

            var opponentsLookup = oppSide.Members?.ToDictionary(m => m.Tag) ?? [];

            foreach (var memberDto in ourSide.Members ?? [])
            {
                if (!perfLookup.TryGetValue((tag, memberDto.Tag), out var playerPerf))
                {
                    playerPerf = new ClanWarLeaguePlayerPerformance
                    {
                        WarTag = tag,
                        PlayerTag = memberDto.Tag
                    };

                    await dbContext.ClanWarLeaguePlayerPerformances.AddAsync(playerPerf, ct);
                    perfLookup.TryAdd((tag, memberDto.Tag), playerPerf);
                }

                playerPerf.UpdateFromClanWarMemberDto(memberDto, opponentsLookup);
            }
        }
    }
}