using Domain.Models;
using Domain.Models.Clans;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IAppDbContext
{
    public DbSet<ClanMember> ClanMembers { get; set; }
    public DbSet<ClanWar> ClanWars { get; set; }
    public DbSet<ClanWarPlayerPerformance> ClanWarPlayerPerformances { get; set; }
    public DbSet<SeasonStats> SeasonStats { get; set; }

    public Task RefreshPlayerSummariesViewAsync(CancellationToken ct = default);
    public Task RefreshClanWarSummariesViewAsync(CancellationToken ct = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}