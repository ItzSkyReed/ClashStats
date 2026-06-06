using Domain.Models.ClanWarLeagues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Clan.LeagueWars;

public class ClanWarLeaguePlayerPerformanceConfiguration : IEntityTypeConfiguration<ClanWarLeaguePlayerPerformance>
{
    public void Configure(EntityTypeBuilder<ClanWarLeaguePlayerPerformance> builder)
    {
        builder.ToTable("clan_war_league_player_performances");

        builder.HasKey(x => new { x.WarTag, x.PlayerTag });

        builder.Property(x => x.WarTag).IsRequired().UseCollation("C");
        builder.Property(x => x.PlayerTag).HasMaxLength(10).UseCollation("C").IsRequired();
        builder.Property(x => x.TownHallLevel).IsRequired();
        builder.Property(x => x.MapPosition).IsRequired();

        builder.Property(x => x.AttackStars);
        builder.Property(x => x.DefenderTag).HasMaxLength(10).UseCollation("C");
        builder.Property(x => x.AttackDestruction);
        builder.Property(x => x.AttackDuration);
        builder.Property(x => x.OpponentPosition);
        builder.Property(x => x.OpponentTownHallLevel);

        builder.HasOne(x => x.LeagueWar)
            .WithMany(cw => cw.PlayerPerformances)
            .HasForeignKey(x => x.WarTag)
            .IsRequired();

        builder.HasOne(x => x.Member)
            .WithMany(cw => cw.ClanWarLeaguePerformances)
            .HasForeignKey(x => x.PlayerTag)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}