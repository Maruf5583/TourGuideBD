using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.PlaceDetails;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class PlaceCategoryMapConfiguration : IEntityTypeConfiguration<PlaceCategoryMap>
{
    public void Configure(EntityTypeBuilder<PlaceCategoryMap> builder)
    {
        builder.ToTable("PlaceCategoryMaps");

        builder.HasKey(m => new { m.PlaceId, m.PlaceCategoryId });

        builder.HasOne(m => m.Place)
            .WithMany(p => p.CategoryMaps)
            .HasForeignKey(m => m.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(m => m.PlaceCategory)
            .WithMany(c => c.CategoryMaps)
            .HasForeignKey(m => m.PlaceCategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}