using Domain.Models.Analytics.ClanWarLeagues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Analytics.LeagueWars;

public class ClanWarLeaguePlayerSummaryConfiguration : IEntityTypeConfiguration<ClanWarLeaguesPlayerSummary>
{
    public void Configure(EntityTypeBuilder<ClanWarLeaguesPlayerSummary> builder)
    {
        builder.ToView("mv_clan_war_leagues_player_summaries");

        builder.HasKey(x => new { x.Tag, x.Season });

        builder.Property(x => x.Season);
        builder.Property(x => x.Tag);
        builder.Property(x => x.Name);

        builder.Property(x => x.AverageMapPosition);
        builder.Property(x => x.AverageOpponentMapPosition);
        builder.Property(x => x.AverageTownHallLevel);
        builder.Property(x => x.AverageOpponentTownHallLevel);
        builder.Property(x => x.AttackParticipationRate);
        builder.Property(x => x.AverageDestructionPercentage);
        builder.Property(x => x.AverageStarsPerAttack);
        builder.Property(x => x.TotalRoundsOnMap);
        builder.Property(x => x.MirrorAttacksRate);
        builder.Property(x => x.ThreeStarRateAgainstSameTh);
        builder.Property(x => x.AverageThMismatches);
        // TODO: сделать нормальный фикс через миграцию
        builder.Property(x => x.CwlRoundsAttacks).HasColumnType("smallint[]").HasColumnName("RecentCwlRoundsAttacks");
    }
}