using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Trip;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class DistrictBusRouteConfiguration : IEntityTypeConfiguration<DistrictBusRoute>
{
    public void Configure(EntityTypeBuilder<DistrictBusRoute> builder)
    {
        builder.ToTable("DistrictBusRoutes");

        builder.Property(r => r.BusCost).HasColumnType("decimal(10,2)");

        builder.HasOne(r => r.FromDistrict)
            .WithMany()
            .HasForeignKey(r => r.FromDistrictId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.ToDistrict)
            .WithMany()
            .HasForeignKey(r => r.ToDistrictId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(r => new { r.FromDistrictId, r.ToDistrictId }).IsUnique();
    }
}