using Application.DTOs.Players;

namespace Application.Interfaces;

public interface IPlayerActivityService
{
    IReadOnlyList<(int InternalId, long NewScore)> DetermineActivityUpdates(
        IEnumerable<(int InternalId, PlayerDto Profile)> fetchedProfiles,
        IReadOnlyDictionary<int, long> lastKnownScores);
}