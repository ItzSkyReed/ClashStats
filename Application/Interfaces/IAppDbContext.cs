using Domain.Models;
using Domain.Models.Analytics.ClanWars;
using Domain.Models.ClanWarLeagues;
using Domain.Models.ClanWars;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IAppDbContext
{
    public DbSet<ClanMember> ClanMembers { get; set; }
    public DbSet<ClanWar> ClanWars { get; set; }
    public DbSet<ClanWarPlayerPerformance> ClanWarPlayerPerformances { get; set; }
    public DbSet<SeasonStats> SeasonStats { get; set; }
    public DbSet<ClanWarSummary> ClanWarSummaries { get; set; }
    public DbSet<ClanWarPlayerSummary> ClanWarPlayerSummaries { get; set; }
    public DbSet<ClanWarLeagueGroup> ClanWarLeagueGroups { get; set; }
    public DbSet<ClanWarLeagueWar> ClanWarLeagueWars { get; set; }
    public DbSet<ClanWarLeaguePlayerPerformance> ClanWarLeaguePlayerPerformances { get; set; }

    public Task RefreshPlayerSummariesViewAsync(CancellationToken ct = default);
    public Task RefreshClanWarSummariesViewAsync(CancellationToken ct = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}