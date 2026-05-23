using Domain.Models.Clans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Clans;

public class ClanWarPlayerPerformanceConfiguration : IEntityTypeConfiguration<ClanWarPlayerPerformance>
{
    public void Configure(EntityTypeBuilder<ClanWarPlayerPerformance> builder)
    {
        builder.ToTable("clan_war_player_performances");

        builder.HasKey(x => new { x.WarId, x.PlayerTag });

        builder.Property(x => x.WarId).IsRequired();
        builder.Property(x => x.PlayerTag).HasMaxLength(10).UseCollation("C").IsRequired();
        builder.Property(x => x.TownHallLevel).IsRequired();
        builder.Property(x => x.MapPosition).IsRequired();

        builder.Property(x => x.Attack1Stars);
        builder.Property(x => x.Defender1Tag).HasMaxLength(10).UseCollation("C");
        builder.Property(x => x.Attack1Destruction);
        builder.Property(x => x.Attack1Duration);
        builder.Property(x => x.Opponent1Position);
        builder.Property(x => x.Opponent1TownHallLevel);

        builder.Property(x => x.Attack2Stars);
        builder.Property(x => x.Defender2Tag).HasMaxLength(10).UseCollation("C");
        builder.Property(x => x.Attack2Destruction);
        builder.Property(x => x.Attack2Duration);
        builder.Property(x => x.Opponent2Position);
        builder.Property(x => x.Opponent2TownHallLevel);

        builder.HasOne(x => x.War)
            .WithMany(cw => cw.PlayersPerformances)
            .HasForeignKey(x => x.WarId)
            .IsRequired();

    }
}