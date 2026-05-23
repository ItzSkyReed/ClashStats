namespace Infrastructure.Persistence.Configurations.Clans;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Application.AnalyticsDTOs; // Замени на свой namespace

public class ClanWarSummaryConfiguration : IEntityTypeConfiguration<ClanWarSummaryDto>
{
    public void Configure(EntityTypeBuilder<ClanWarSummaryDto> builder)
    {
        builder.ToView("mv_clan_war_summaries");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("Id");
        builder.Property(x => x.StartTime).HasColumnName("StartTime");
        builder.Property(x => x.EndTime).HasColumnName("EndTime");
        builder.Property(x => x.TeamSize).HasColumnName("TeamSize");
        builder.Property(x => x.OpponentClanName).HasColumnName("OpponentClanName");
        builder.Property(x => x.OpponentClanLevel).HasColumnName("OpponentClanLevel");
        builder.Property(x => x.OpponentAttacks).HasColumnName("OpponentAttacks");
        builder.Property(x => x.OurAttacks).HasColumnName("OurAttacks");
        builder.Property(x => x.OurStars).HasColumnName("OurStars");
        builder.Property(x => x.OpponentStars).HasColumnName("OpponentStars");
        builder.Property(x => x.OurDestructionPercentage).HasColumnName("OurDestructionPercentage");
        builder.Property(x => x.OpponentDestructionPercentage).HasColumnName("OpponentDestructionPercentage");
        builder.Property(x => x.ExpEarned).HasColumnName("ExpEarned");

        builder.Property(x => x.AverageOurTownHall).HasColumnName("AverageOurTownHall");
        builder.Property(x => x.AverageOpponentTownHall).HasColumnName("AverageOpponentTownHall");
        builder.Property(x => x.TownHallAdvantage).HasColumnName("TownHallAdvantage");
        builder.Property(x => x.ParticipationRate).HasColumnName("ParticipationRate");
        builder.Property(x => x.OurThreeStarsCount).HasColumnName("OurThreeStarsCount");
        builder.Property(x => x.StarsPerAttack).HasColumnName("StarsPerAttack");

        builder.Ignore(x => x.Result);
    }
}