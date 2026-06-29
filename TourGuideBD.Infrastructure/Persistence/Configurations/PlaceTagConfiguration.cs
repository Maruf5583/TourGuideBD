using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.PlaceDetails;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class PlaceTagConfiguration : IEntityTypeConfiguration<PlaceTag>
{
    public void Configure(EntityTypeBuilder<PlaceTag> builder)
    {
        builder.ToTable("PlaceTags");

        builder.Property(t => t.Tag).IsRequired().HasMaxLength(50);

        builder.HasOne(t => t.Place)
            .WithMany(p => p.Tags)
            .HasForeignKey(t => t.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(t => t.Tag);
    }
}