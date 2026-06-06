using Domain.Models.Analytics.ClanWars;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Analytics.Wars;

public class ClanWarSummaryConfiguration : IEntityTypeConfiguration<ClanWarSummary>
{
    public void Configure(EntityTypeBuilder<ClanWarSummary> builder)
    {
        builder.ToView("mv_clan_war_summaries");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id);
        builder.Property(x => x.StartTime);
        builder.Property(x => x.EndTime);
        builder.Property(x => x.TeamSize);
        builder.Property(x => x.OpponentClanName);
        builder.Property(x => x.OpponentClanLevel);
        builder.Property(x => x.OpponentAttacks);
        builder.Property(x => x.OurAttacks);
        builder.Property(x => x.OurStars);
        builder.Property(x => x.OpponentStars);
        builder.Property(x => x.OurDestructionPercentage);
        builder.Property(x => x.OpponentDestructionPercentage);
        builder.Property(x => x.ExpEarned);

        builder.Property(x => x.AverageOurTownHall);
        builder.Property(x => x.AverageOpponentTownHall);
        builder.Property(x => x.TownHallAdvantage);
        builder.Property(x => x.ParticipationRate);
        builder.Property(x => x.OurThreeStarsCount);
        builder.Property(x => x.StarsPerAttack);

        builder.Ignore(x => x.Result);
    }
}