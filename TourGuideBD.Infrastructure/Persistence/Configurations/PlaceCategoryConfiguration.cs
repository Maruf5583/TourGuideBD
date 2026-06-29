using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.PlaceDetails;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class PlaceCategoryConfiguration : IEntityTypeConfiguration<PlaceCategory>
{
    public void Configure(EntityTypeBuilder<PlaceCategory> builder)
    {
        builder.ToTable("PlaceCategories");

        builder.Property(c => c.Name).IsRequired().HasMaxLength(50);
        builder.HasIndex(c => c.CategoryType).IsUnique();
    }
}