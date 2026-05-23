using Application.Interfaces;
using Ardalis.SmartEnum.SystemTextJson;
using Domain.Constants;
using Domain.Models;
using Domain.Models.Clans;

namespace Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    // DbSets
    public DbSet<ClanMember> ClanMembers { get; set; }
    public DbSet<ClanWar> ClanWars { get; set; }
    public DbSet<ClanWarPlayerPerformance> ClanWarPlayerPerformances { get; set; }
    public DbSet<SeasonStats> SeasonStats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public async Task RefreshPlayerSummariesViewAsync(CancellationToken ct = default)
    {
        await Database.ExecuteSqlRawAsync(
            "REFRESH MATERIALIZED VIEW CONCURRENTLY mv_clan_war_player_summaries;",
            ct);
    }

    public async Task RefreshClanWarSummariesViewAsync(CancellationToken ct = default)
    {
        await Database.ExecuteSqlRawAsync(
            "REFRESH MATERIALIZED VIEW CONCURRENTLY mv_clan_war_summaries;",
            ct);
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