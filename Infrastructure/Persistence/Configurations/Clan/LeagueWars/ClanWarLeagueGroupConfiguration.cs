using Domain.Constants;
using Domain.Models.ClanWarLeagues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Clan.LeagueWars;

public class ClanWarLeagueGroupConfiguration : IEntityTypeConfiguration<ClanWarLeagueGroup>
{
    public void Configure(EntityTypeBuilder<ClanWarLeagueGroup> builder)
    {
        builder.ToTable("clan_war_league_groups");

        builder.HasKey(x => x.Season);

        builder.Property(x => x.Season).IsRequired().UseCollation("C");

        builder.Property(x => x.State).HasConversion(
            role => role.Value,
            value => ClanWarLeagueGroupState.FromValue(value)
        ).UseCollation("C").HasMaxLength(64).IsRequired();

        builder.Property(x => x.TeamSize).IsRequired();

        builder.Property(x => x.Place).IsRequired(false);
    }
}