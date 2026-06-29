using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Guide;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class GuideLiveLocationConfiguration : IEntityTypeConfiguration<GuideLiveLocation>
{
    public void Configure(EntityTypeBuilder<GuideLiveLocation> builder)
    {
        builder.ToTable("GuideLiveLocations");

        builder.Property(g => g.Latitude).HasColumnType("float");
        builder.Property(g => g.Longitude).HasColumnType("float");
        builder.Property(g => g.GuideUserId).IsRequired().HasMaxLength(450);

        builder.HasOne(g => g.Booking)
            .WithMany()
            .HasForeignKey(g => g.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(g => g.BookingId).IsUnique();
        builder.HasIndex(g => g.GuideUserId);
    }
}