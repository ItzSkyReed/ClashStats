using Domain.Constants;
using Domain.Models.Clans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Clans;

public class ClanMemberConfiguration : IEntityTypeConfiguration<ClanMember>
{
    public void Configure(EntityTypeBuilder<ClanMember> builder)
    {
        builder.ToTable("clan_members");

        builder.HasKey(x => x.Tag);
        builder.Property(x => x.Tag).HasMaxLength(10).UseCollation("C").IsRequired();
        builder.Property(x => x.Name).HasMaxLength(64).IsRequired();
        builder.Property(x => x.TownHallLevel).IsRequired();
        builder.Property(x => x.BuilderHallLevel).IsRequired();
        builder.Property(x => x.ExpLevel).IsRequired();
        builder.Property(x => x.Trophies).IsRequired();
        builder.Property(x => x.WarStars).IsRequired();
        builder.Property(x => x.BuilderBaseTrophies).IsRequired();
        builder.Property(x => x.BestBuilderBaseTrophies).IsRequired();
        builder.Property(x => x.Role).HasConversion(
            role => role!.Value,
            value => ClanRole.FromValue(value)
        ).UseCollation("C").HasMaxLength(64).IsRequired();
        builder.Property(x => x.WarPreference).HasConversion(
            preference => preference!.Value,
            value => WarPreference.FromValue(value)
        ).UseCollation("C").IsRequired();
        builder.Property(x => x.ClanCapitalContributions).IsRequired();
        builder.Property(x => x.IsNowInClan).IsRequired();
    }
}