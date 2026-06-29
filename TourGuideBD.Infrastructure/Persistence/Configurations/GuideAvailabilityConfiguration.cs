using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Guide;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class GuideAvailabilityConfiguration : IEntityTypeConfiguration<GuideAvailability>
{
    public void Configure(EntityTypeBuilder<GuideAvailability> builder)
    {
        builder.ToTable("GuideAvailabilities");

        builder.HasOne(a => a.TourPackage)
            .WithMany(t => t.Availabilities)
            .HasForeignKey(a => a.TourPackageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(a => new { a.TourPackageId, a.AvailableDate }).IsUnique();
    }
}