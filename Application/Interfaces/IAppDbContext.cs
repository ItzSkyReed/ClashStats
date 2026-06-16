using Domain.Models;
using Domain.Models.Analytics.ClanWarLeagues;
using Domain.Models.Analytics.ClanWars;
using Domain.Models.ClanWarLeagues;
using Domain.Models.ClanWars;
using Domain.Models.Statistics;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IAppDbContext
{
    public DbSet<ClanMember> ClanMembers { get; set; }
    public DbSet<ClanWar> ClanWars { get; set; }
    public DbSet<ClanWarPlayerPerformance> ClanWarPlayerPerformances { get; set; }
    public DbSet<PlayerSeasonStats> SeasonStats { get; set; }
    public DbSet<ClanWarLeagueGroup> ClanWarLeagueGroups { get; set; }
    public DbSet<ClanWarLeagueWar> ClanWarLeagueWars { get; set; }
    public DbSet<ClanWarLeaguePlayerPerformance> ClanWarLeaguePlayerPerformances { get; set; }
    public DbSet<PlayerActivitySnapshot> PlayerActivitySnapshots { get; set; }
    public DbSet<PlayerActivityState> PlayerActivityStates { get; set; }
    public DbSet<ClanStatsSnapshot> ClanStatsSnapshots { get; set; }

    public DbSet<ClanWarSummary> ClanWarSummaries { get; set; }
    public DbSet<ClanWarPlayerSummary> ClanWarPlayerSummaries { get; set; }

    public DbSet<ClanWarLeagueGroupSummary> ClanWarLeagueGroupSummaries { get; set; }
    public DbSet<ClanWarLeaguesPlayerSummary> ClanWarLeaguePlayerSummaries { get; set; }

    public Task RefreshCwPlayerSummariesViewAsync(CancellationToken ct = default);
    public Task RefreshCwClanWarSummariesViewAsync(CancellationToken ct = default);

    public Task RefreshCwlPlayerSummariesViewAsync(CancellationToken ct = default);
    public Task RefreshCwlClanWarSummariesViewAsync(CancellationToken ct = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}