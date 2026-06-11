using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Clan;

public class PlayerActivitySnapshotConfiguration : IEntityTypeConfiguration<PlayerActivitySnapshot>
{
    public void Configure(EntityTypeBuilder<PlayerActivitySnapshot> builder)
    {
        builder.ToTable("player_activity_snapshots");

        builder.HasIndex(x => new { x.MemberInternalId, x.Id }).IsDescending(false, true);

        builder.Property(x => x.MemberInternalId);
        builder.Property(x => x.Id);
        builder.Property(x => x.SnapshotTime);

        builder.HasOne(x => x.Member)
            .WithMany(x => x.ActivitySnapshots)
            .HasForeignKey(x => x.MemberInternalId)
            .HasPrincipalKey(x => x.InternalId)
            .IsRequired();
    }
}