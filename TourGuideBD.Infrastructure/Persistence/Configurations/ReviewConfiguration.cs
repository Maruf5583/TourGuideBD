using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Reviews;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");

        builder.Property(r => r.CommentEn).HasMaxLength(500);
        builder.Property(r => r.CommentBn).HasMaxLength(500);

        builder.HasOne(r => r.Place)
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(r => r.Status);
        builder.HasIndex(r => new { r.PlaceId, r.Status });
    }
}