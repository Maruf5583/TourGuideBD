using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Reviews;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class ReviewPhotoConfiguration : IEntityTypeConfiguration<ReviewPhoto>
{
    public void Configure(EntityTypeBuilder<ReviewPhoto> builder)
    {
        builder.ToTable("ReviewPhotos");

        builder.Property(p => p.Url).IsRequired().HasMaxLength(500);

        builder.HasOne(p => p.Review)
            .WithMany(r => r.Photos)
            .HasForeignKey(p => p.ReviewId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}