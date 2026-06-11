namespace Application.Interfaces;

public interface IClanDataSyncService
{
    public Task UpdateClanMembers(CancellationToken ct);
    public Task UpdateSeasonStats(CancellationToken ct);
    public Task<bool> UpdateClanWar(CancellationToken ct);
    public Task RefreshCwMaterializedViews(CancellationToken ct);
    public Task RefreshCwlMaterializedViews(CancellationToken ct);
    public Task CleanupStuckWars(CancellationToken ct);
    public Task<bool> UpdateClanLeagueWars(CancellationToken ct);
    public Task UpdateActivitySnapshots(CancellationToken ct);
}