using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Guide;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class GuideReviewConfiguration : IEntityTypeConfiguration<GuideReview>
{
    public void Configure(EntityTypeBuilder<GuideReview> builder)
    {
        builder.ToTable("GuideReviews");

        builder.Property(r => r.Comment).HasMaxLength(500);

        builder.HasOne(r => r.GuideProfile)
            .WithMany(g => g.Reviews)
            .HasForeignKey(r => r.GuideProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Booking)
            .WithOne(b => b.Review)
            .HasForeignKey<GuideReview>(r => r.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(r => new { r.BookingId }).IsUnique();
    }
}