using Domain.Models.Analytics;

namespace Infrastructure.Persistence.Configurations.Clans;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ClanWarPlayerSummaryConfiguration : IEntityTypeConfiguration<ClanWarPlayerSummary>
{
    public void Configure(EntityTypeBuilder<ClanWarPlayerSummary> builder)
    {
        builder.ToView("mv_clan_war_player_summaries");

        builder.HasKey(x => x.Tag);
        builder.Property(x => x.Tag);
        builder.Property(x => x.Name);
        builder.Property(x => x.AverageMapPosition);
        builder.Property(x => x.AverageOpponentMapPosition);
        builder.Property(x => x.AverageTownHallLevel);
        builder.Property(x => x.AverageOpponentTownHallLevel);
        builder.Property(x => x.AttackParticipationRate);
        builder.Property(x => x.AverageDestructionPercentage);
        builder.Property(x => x.AverageStarsPerWar);
        builder.Property(x => x.FirstAttackParticipationRate);
        builder.Property(x => x.SecondAttackParticipationRate);
        builder.Property(x => x.MirrorAttacksRate);
        builder.Property(x => x.ThreeStarRateAgainstSameTh);
        builder.Property(x => x.AverageThMismatches);
        builder.Property(x => x.RecentWarsAttacks).HasColumnType("smallint[]");
    }
}