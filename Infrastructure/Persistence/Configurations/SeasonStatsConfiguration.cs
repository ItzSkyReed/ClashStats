using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class SeasonStatsConfiguration : IEntityTypeConfiguration<SeasonStats>
{
    public void Configure(EntityTypeBuilder<SeasonStats> builder)
    {
        builder.ToTable("season_stats");

        builder.HasKey(x => new { x.SeasonDate, x.PlayerTag });

        builder.Property(x => x.SeasonDate).IsRequired();
        builder.Property(x => x.PlayerTag).IsRequired();
        builder.Property(x => x.Donations).IsRequired();
        builder.Property(x => x.DonationsReceived).IsRequired();

        builder.HasOne(x => x.Player)
            .WithMany(cw => cw.SeasonStats)
            .HasForeignKey(x => x.PlayerTag);
    }
}