using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.PlaceDetails;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class PlacePhotoConfiguration : IEntityTypeConfiguration<PlacePhoto>
{
    public void Configure(EntityTypeBuilder<PlacePhoto> builder)
    {
        builder.ToTable("PlacePhotos");

        builder.Property(p => p.Url).IsRequired().HasMaxLength(500);
        builder.Property(p => p.Caption).HasMaxLength(200);

        builder.HasOne(p => p.Place)
            .WithMany(pl => pl.Photos)
            .HasForeignKey(p => p.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}