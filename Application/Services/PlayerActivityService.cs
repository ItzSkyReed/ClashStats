using Application.ClashOfClansModels.Players;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Statistics;

namespace Application.Services;

public class PlayerActivityService(IAppDbContext dbContext) : IPlayerActivityService
{
    public IReadOnlyList<(int InternalId, long NewScore)> DetermineActivityUpdates(
        IEnumerable<(int InternalId, PlayerDto Profile)> fetchedProfiles,
        IReadOnlyDictionary<int, long> lastKnownScores)
    {
        var updates = new List<(int InternalId, long NewScore)>();

        foreach (var (internalId, profile) in fetchedProfiles)
        {
            if (!TryCalculateActivityScore(profile, out var currentScore))
                continue;

            // Если игрока нет в словаре или счет изменился
            if (!lastKnownScores.TryGetValue(internalId, out var lastScore) || lastScore != currentScore)
            {
                updates.Add((internalId, currentScore));
            }
        }

        return updates;
    }

    public async Task<List<ChartPointDto>> GetNormalizedMemberChartAsync(string memberTag, DateTimeOffset start, DateTimeOffset end)
    {
        var memberInternalId = await dbContext.ClanMembers
            .Where(x => x.Tag == memberTag)
            .Select(x => (int?)x.InternalId)
            .SingleOrDefaultAsync();

        if (memberInternalId == null)
            return [];

        var dbData = await GetMemberActivityChartAsync(memberInternalId.Value, start, end);

        var dbDict = dbData.ToDictionary(x => x.Time, x => x.ActivityScore);

        DateTimeOffset currentGridPoint;
        TimeSpan step;

        switch ((end - start).TotalDays)
        {
            case > 14:
                currentGridPoint = new DateTimeOffset(start.Date, TimeSpan.Zero);
                step = TimeSpan.FromDays(1);
                break;
            case >= 2:
                currentGridPoint = new DateTimeOffset(start.Year, start.Month, start.Day, start.Hour, 0, 0, TimeSpan.Zero);
                step = TimeSpan.FromHours(1);
                break;
            default:
                currentGridPoint = new DateTimeOffset(start.Year, start.Month, start.Day, start.Hour, start.Minute, 0, TimeSpan.Zero);
                step = TimeSpan.FromMinutes(1);
                break;
        }

        var finalData = new List<ChartPointDto>();

        while (currentGridPoint <= end)
        {
            finalData.Add(new ChartPointDto
            {
                Time = currentGridPoint,
                ActivityScore = dbDict.GetValueOrDefault(currentGridPoint, 0)
            });

            currentGridPoint += step;
        }

        return finalData;
    }

    private async Task<List<ChartPointDto>> GetMemberActivityChartAsync(
        int memberInternalId,
        DateTimeOffset start,
        DateTimeOffset end)
    {
        var timeSpan = end - start;

        var query = dbContext.PlayerActivitySnapshots
            .Where(x => x.MemberInternalId == memberInternalId
                        && x.SnapshotTime >= start
                        && x.SnapshotTime <= end);

        return timeSpan.TotalDays switch
        {
            > 14 => await query.GroupBy(x => x.SnapshotTime.Date)
                .Select(g => new ChartPointDto
                {
                    Time = g.Key, ActivityScore = g.Count() // Сколько раз засветился в этот день
                })
                .OrderBy(x => x.Time)
                .ToListAsync(),

            >= 2 => await query.GroupBy(x => new
                    { x.SnapshotTime.Year, x.SnapshotTime.Month, x.SnapshotTime.Day, x.SnapshotTime.Hour })
                .Select(g => new ChartPointDto
                {
                    Time = new DateTimeOffset(g.Key.Year, g.Key.Month, g.Key.Day, g.Key.Hour, 0, 0, TimeSpan.Zero),
                    ActivityScore = g.Count() // Сколько раз засветился за час
                })
                .OrderBy(x => x.Time)
                .ToListAsync(),

            _ => await query.GroupBy(x => new
                {
                    x.SnapshotTime.Year, x.SnapshotTime.Month, x.SnapshotTime.Day, x.SnapshotTime.Hour, x.SnapshotTime.Minute
                })
                .Select(g => new ChartPointDto
                {
                    Time = new DateTimeOffset(g.Key.Year, g.Key.Month, g.Key.Day, g.Key.Hour, g.Key.Minute, 0, TimeSpan.Zero),
                    ActivityScore = g.Count()
                })
                .OrderBy(x => x.Time)
                .ToListAsync()
        };
    }

    private static bool TryCalculateActivityScore(PlayerDto profile, out long score)
    {
        score = 0;
        if (!TryGetAchievementValue(profile.Achievements, "Gold Grab", out var goldGrab) ||
            !TryGetAchievementValue(profile.Achievements, "Elixir Escapade", out var elixirGrab) ||
            !TryGetAchievementValue(profile.Achievements, "Wall Buster", out var wallsBroken) ||
            !TryGetAchievementValue(profile.Achievements, "Un-Build It", out var bulderThsBroken) ||
            !TryGetAchievementValue(profile.Achievements, "Friend in Need", out var donations) ||
            !TryGetAchievementValue(profile.Achievements, "Heroic Heist", out var darkElixirGrab) ||
            !TryGetAchievementValue(profile.Achievements, "Clan War Wealth", out var goldFromCcCollected) ||
            !TryGetAchievementValue(profile.Achievements, "Well Seasoned", out var seasonPointsCollected) ||
            !TryGetAchievementValue(profile.Achievements, "Games Champion", out var clanGamePoints))
        {
            return false;
        }

        score = (long)profile.ExpLevel +
                profile.WarStars +
                profile.BestBuilderBaseTrophies +
                profile.ClanCapitalContributions +
                goldGrab +
                elixirGrab +
                wallsBroken +
                bulderThsBroken +
                darkElixirGrab +
                goldFromCcCollected +
                clanGamePoints +
                seasonPointsCollected +
                donations;

        return true;
    }

    private static bool TryGetAchievementValue(
        IEnumerable<PlayerAchievementProgressDto>? achievements,
        string achievementName,
        out long value)
    {
        var achievement = achievements?.FirstOrDefault(x => x.Name == achievementName);

        if (achievement != null)
        {
            value = achievement.Value;
            return true;
        }

        value = 0;
        return false;
    }
}