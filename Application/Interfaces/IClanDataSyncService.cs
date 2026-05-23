namespace Application.Interfaces;

public interface IClanDataSyncService
{
    public Task UpdateClanMembers(CancellationToken ct);
    public Task UpdateSeasonStats(CancellationToken ct);
    public Task<bool> UpdateClanWar(CancellationToken ct);
    public Task RefreshMaterializedViews(CancellationToken ct);
    public Task CleanupStuckWars(CancellationToken ct);
}