using Domain.Constants;
using Domain.Models.Analytics.ClanWarLeagues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Analytics.LeagueWars;

public class ClanWarLeagueGroupSummaryConfiguration : IEntityTypeConfiguration<ClanWarLeagueGroupSummary>
{
    public void Configure(EntityTypeBuilder<ClanWarLeagueGroupSummary> builder)
    {
        builder.ToView("mv_clan_war_league_group_summaries");

        builder.HasKey(x => x.Season);

        builder.Property(x => x.Season);
        builder.Property(x => x.State).HasConversion(
            role => role.Value,
            value => ClanWarLeagueState.FromValue(value)
        );

        builder.Property(x => x.TeamSize);
        builder.Property(x => x.Place);

        builder.Property(x => x.TotalWins);
        builder.Property(x => x.TotalLosses);

        builder.Property(x => x.TotalOurAttacks);
        builder.Property(x => x.TotalOurStars);
        builder.Property(x => x.TotalOurThreeStars);

        builder.Property(x => x.TotalOpponentAttacks);
        builder.Property(x => x.TotalOpponentStars);

        builder.Property(x => x.ParticipationRate);
        builder.Property(x => x.AverageStarsPerAttack);
        builder.Property(x => x.AverageOurDestructionPercentage);

        builder.Property(x => x.AverageOurTownHall);
        builder.Property(x => x.AverageOpponentTownHall);
        builder.Property(x => x.TownHallAdvantage);
    }
}