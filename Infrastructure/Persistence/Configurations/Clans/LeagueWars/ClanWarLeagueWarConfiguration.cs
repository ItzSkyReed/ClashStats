using Domain.Constants;
using Domain.Models.ClanWarLeagues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Clans.LeagueWars;

public class ClanWarLeagueWarConfiguration : IEntityTypeConfiguration<ClanWarLeagueWar>
{
    public void Configure(EntityTypeBuilder<ClanWarLeagueWar> builder)
    {
        builder.ToTable("clan_war_league_wars");

        builder.HasKey(x => x.WarTag);

        builder.Property(x => x.StartTime).IsRequired().HasColumnType("timestamp without time zone");
        builder.Property(x => x.EndTime).IsRequired().HasColumnType("timestamp without time zone");
        builder.Property(x => x.WarStartTime).IsRequired().HasColumnType("timestamp without time zone");

        builder.Property(x => x.State).HasConversion(
            role => role.Value,
            value => ClanWarLeagueState.FromValue(value)
        ).UseCollation("C").HasMaxLength(64).IsRequired();

        builder.Property(x => x.Season).IsRequired().UseCollation("C");

        builder.Property(x => x.Round).IsRequired();

        builder.Property(x => x.OpponentClanTag).HasMaxLength(10).UseCollation("C").IsRequired();
        builder.Property(x => x.OpponentClanName).HasMaxLength(64).IsRequired();
        builder.Property(x => x.OpponentClanLevel).IsRequired();
        builder.Property(x => x.OpponentAttacks).IsRequired();
        builder.Property(x => x.OurAttacks).IsRequired();
        builder.Property(x => x.OurStars).IsRequired();
        builder.Property(x => x.OpponentStars).IsRequired();
        builder.Property(x => x.OurDestructionPercentage).IsRequired();
        builder.Property(x => x.OpponentDestructionPercentage).IsRequired();

        builder.HasOne(x => x.Group)
            .WithMany(cw => cw.Wars)
            .HasForeignKey(x => x.Season)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}