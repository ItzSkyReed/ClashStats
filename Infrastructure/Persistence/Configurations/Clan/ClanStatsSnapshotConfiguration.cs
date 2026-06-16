using Domain.Models.Statistics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Clan;

public class ClanStatsSnapshotConfiguration : IEntityTypeConfiguration<ClanStatsSnapshot>
{
    public void Configure(EntityTypeBuilder<ClanStatsSnapshot> builder)
    {
        builder.ToTable("clan_stats_snapshots");

        builder.HasKey(x => new { x.CapturedAt, x.ClanTag });

        builder.Property(x => x.CapturedAt);
        builder.Property(x => x.ClanTag);

        builder.Property(x => x.ClanLevel);
        builder.Property(x => x.ClanPoints);
        builder.Property(x => x.ClanBuilderBasePoints);
        builder.Property(x => x.ClanCapitalPoints);

        builder.Property(x => x.WarLeagueName).UseCollation("C").HasMaxLength(32);
        builder.Property(x => x.WarLeagueId);

        builder.Property(x => x.CapitalLeagueName).UseCollation("C").HasMaxLength(32);
        builder.Property(x => x.CapitalLeagueId);

        builder.Property(x => x.WarWinStreak);
        builder.Property(x => x.WarWins);
        builder.Property(x => x.WarTies);
        builder.Property(x => x.WarLosses);

        builder.Property(x => x.MembersCount);
        builder.Property(x => x.AverageTownHallLevel);

        builder.Property(x => x.RequiredTrophies);
        builder.Property(x => x.RequiredTownHallLevel);
    }
}