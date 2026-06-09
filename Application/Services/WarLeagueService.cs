using System.Collections.Concurrent;
using Application.DTOs.Clans.ClanWarLeagues;
using Application.DTOs.Clans.ClanWars;
using Application.Extensions;
using Application.Interfaces;
using Domain.Constants;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class WarLeagueService(IClashApiClient apiClient, ILogger<WarLeagueService> logger) : IWarLeagueService
{
    public async ValueTask<short> GetLeagueWarTeamSize(ClanWarLeagueGroupDto leagueGroupDto, CancellationToken ct)
    {
        var warTag = leagueGroupDto.Rounds[0].WarTags[0]; // Подойдет тэг любой войны, можно взять первую
        var leaguerWar = await apiClient.GetClanWarLeagueWarAsync(warTag, ct).UnwrapOrLogAsync(logger);
        return (short)(leaguerWar?.TeamSize ?? 0);
    }

    public async Task<List<IWarLeagueService.ClanLeagueRanking>?> CalculateClanWarLeagueRankings(ClanWarLeagueGroupDto leagueGroupDto,
        CancellationToken ct)
    {
        if (leagueGroupDto.State != ClanWarLeagueGroupState.Ended)
            return null;

        var warTags = leagueGroupDto.Rounds.SelectMany(r => r.WarTags);

        var wars = new ConcurrentBag<ClanWarLeaguerWarDto>(); // Замените на вашу DTO модели войны

        await Parallel.ForEachAsync(warTags, ct, async (tag, token) =>
        {
            var war = await apiClient.GetClanWarLeagueWarAsync(tag, token).UnwrapOrLogAsync(logger);

            if (war != null)
                wars.Add(war);
        });


        var rankings = wars
            .SelectMany(war => new[]
            {
                new
                {
                    war.Clan.Tag,
                    war.Clan.Name,
                    war.Clan.Stars,
                    war.Clan.DestructionPercentage,
                    IsWinner = IsWarWinner(war.Clan, war.Opponent)
                },
                new
                {
                    war.Opponent.Tag,
                    war.Opponent.Name,
                    war.Opponent.Stars,
                    war.Opponent.DestructionPercentage,
                    IsWinner = IsWarWinner(war.Opponent, war.Clan)
                }
            })
            .GroupBy(c => c.Tag)
            .Select(g => new
            {
                Tag = g.Key,
                g.First().Name,
                TotalStars = g.Sum(c => c.Stars + (c.IsWinner ? 10 : 0)),
                TotalDestruction = g.Sum(c => c.DestructionPercentage)
            })
            .OrderByDescending(c => c.TotalStars)
            .ThenByDescending(c => c.TotalDestruction)
            .Select((clan, index) => new IWarLeagueService.ClanLeagueRanking(
                Place: (short)(index + 1),
                Tag: clan.Tag!,
                Name: clan.Name!,
                TotalStars: clan.TotalStars,
                TotalDestruction: clan.TotalDestruction
            ))
            .ToList();

        return rankings;
    }

    public short? GetClanPlace(string clanTag, in List<IWarLeagueService.ClanLeagueRanking> rankings)
    {
        return rankings.SingleOrDefault(o => o.Tag == clanTag)?.Place;
    }

    private static bool IsWarWinner(in WarClanDto clan, in WarClanDto opponent)
    {
        if (clan.Stars > opponent.Stars) return true;
        return clan.Stars == opponent.Stars && clan.DestructionPercentage > opponent.DestructionPercentage;
    }
}