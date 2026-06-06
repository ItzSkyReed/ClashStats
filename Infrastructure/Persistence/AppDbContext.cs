using Application.Interfaces;
using Ardalis.SmartEnum.SystemTextJson;
using Domain.Constants;
using Domain.Models;
using Domain.Models.Analytics.ClanWars;
using Domain.Models.ClanWarLeagues;
using Domain.Models.ClanWars;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    // DbSets
    public DbSet<ClanMember> ClanMembers { get; set; }
    public DbSet<ClanWar> ClanWars { get; set; }
    public DbSet<ClanWarPlayerPerformance> ClanWarPlayerPerformances { get; set; }
    public DbSet<SeasonStats> SeasonStats { get; set; }
    public DbSet<ClanWarSummary> ClanWarSummaries { get; set; }
    public DbSet<ClanWarPlayerSummary> ClanWarPlayerSummaries { get; set; }
    public DbSet<ClanWarLeagueGroup> ClanWarLeagueGroups { get; set; }
    public DbSet<ClanWarLeagueWar> ClanWarLeagueWars { get; set; }
    public DbSet<ClanWarLeaguePlayerPerformance> ClanWarLeaguePlayerPerformances { get; set; }

    public async Task RefreshCwPlayerSummariesViewAsync(CancellationToken ct = default)
    {
        await Database.ExecuteSqlRawAsync(
            "REFRESH MATERIALIZED VIEW CONCURRENTLY mv_clan_war_player_summaries;",
            ct);
    }

    public async Task RefreshCwClanWarSummariesViewAsync(CancellationToken ct = default)
    {
        await Database.ExecuteSqlRawAsync(
            "REFRESH MATERIALIZED VIEW CONCURRENTLY mv_clan_war_summaries;",
            ct);
    }

    public async Task RefreshCwlPlayerSummariesViewAsync(CancellationToken ct = default)
    {
        await Database.ExecuteSqlRawAsync(
            "REFRESH MATERIALIZED VIEW CONCURRENTLY mv_clan_war_league_player_summaries;",
            ct);
    }

    public async Task RefreshCwlClanWarSummariesViewAsync(CancellationToken ct = default)
    {
        await Database.ExecuteSqlRawAsync(
            "REFRESH MATERIALIZED VIEW CONCURRENTLY mv_clan_war_league_group_summaries;",
            ct);
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<WarFrequency>()
            .HaveConversion<SmartEnumValueConverter<WarFrequency, string>>();

        configurationBuilder.Properties<ClanWarState>()
            .HaveConversion<SmartEnumValueConverter<ClanWarState, string>>();


        configurationBuilder.Properties<ClanRole>()
            .HaveConversion<SmartEnumValueConverter<ClanRole, string>>();

        configurationBuilder.Properties<ClanWarLeagueState>()
            .HaveConversion<SmartEnumValueConverter<ClanWarLeagueState, string>>();
    }
}