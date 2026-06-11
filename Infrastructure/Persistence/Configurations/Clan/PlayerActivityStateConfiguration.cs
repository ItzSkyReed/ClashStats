using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Clan;

public class PlayerActivityStateConfiguration : IEntityTypeConfiguration<PlayerActivityState>
{
    public void Configure(EntityTypeBuilder<PlayerActivityState> builder)
    {
        builder.ToTable("player_activity_states");

        builder.HasKey(x => x.MemberInternalId);

        builder.Property(x => x.MemberInternalId);
        builder.Property(x => x.ActivityScore);

        builder.HasOne(x => x.Member)
            .WithOne() // Связь один-к-одному
            .HasForeignKey<PlayerActivityState>(x => x.MemberInternalId)
            .HasPrincipalKey<ClanMember>(x => x.InternalId);
    }
}