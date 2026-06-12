using Application.ClashOfClansModels.Players;
using Application.Interfaces;

namespace Application.Services;

public class PlayerActivityService : IPlayerActivityService
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