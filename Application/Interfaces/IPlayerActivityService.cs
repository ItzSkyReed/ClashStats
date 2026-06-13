using Application.ClashOfClansModels.Players;
using Shared.DTOs.Statistics;

namespace Application.Interfaces;

public interface IPlayerActivityService
{
    IReadOnlyList<(int InternalId, long NewScore)> DetermineActivityUpdates(
        IEnumerable<(int InternalId, PlayerDto Profile)> fetchedProfiles,
        IReadOnlyDictionary<int, long> lastKnownScores);

    Task<List<ChartPointDto>> GetNormalizedMemberChartAsync(string memberTag, DateTimeOffset start, DateTimeOffset end);
}