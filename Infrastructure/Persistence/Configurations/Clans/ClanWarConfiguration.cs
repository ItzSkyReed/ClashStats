using System.ComponentModel.DataAnnotations.Schema;
using Domain.Constants;
using Domain.Models.Clans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Clans;

public class ClanWarConfiguration : IEntityTypeConfiguration<ClanWar>
{
    public void Configure(EntityTypeBuilder<ClanWar> builder)
    {
        builder.ToTable("clan_wars");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StartTime).IsRequired().HasColumnType("timestamp without time zone");

        builder.Property(x => x.State).HasConversion(
            role => role!.Value,
            value => ClanWarState.FromValue(value)
        ).UseCollation("C").HasMaxLength(64).IsRequired();
        builder.Property(x => x.EndTime).IsRequired().HasColumnType("timestamp without time zone");
        builder.Property(x => x.TeamSize).IsRequired();
        builder.Property(x => x.OpponentClanTag).HasMaxLength(10).UseCollation("C").IsRequired();
        builder.Property(x => x.OpponentClanName).HasMaxLength(64).IsRequired();
        builder.Property(x => x.OpponentClanLevel).IsRequired();
        builder.Property(x => x.OpponentAttacks).IsRequired();
        builder.Property(x => x.OurAttacks).IsRequired();
        builder.Property(x => x.OurStars).IsRequired();
        builder.Property(x => x.OpponentStars).IsRequired();
        builder.Property(x => x.OurDestructionPercentage).IsRequired();
        builder.Property(x => x.OpponentDestructionPercentage).IsRequired();
        builder.Property(x => x.ExpEarned);

        builder.HasIndex(x => x.OpponentClanTag);
        builder.HasIndex(x => new { x.StartTime, x.EndTime });
    }
}