using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Trip;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class BusStandConfiguration : IEntityTypeConfiguration<BusStand>
{
    public void Configure(EntityTypeBuilder<BusStand> builder)
    {
        builder.ToTable("BusStands");

        builder.Property(b => b.Name).IsRequired().HasMaxLength(150);
        builder.Property(b => b.NameBn).IsRequired().HasMaxLength(150);
        builder.Property(b => b.Latitude).HasColumnType("float");
        builder.Property(b => b.Longitude).HasColumnType("float");

        builder.HasOne(b => b.District)
            .WithMany()
            .HasForeignKey(b => b.DistrictId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(b => b.DistrictId);
    }
}