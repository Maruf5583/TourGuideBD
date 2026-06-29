using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Guide;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class GuideRevenueConfiguration : IEntityTypeConfiguration<GuideRevenue>
{
    public void Configure(EntityTypeBuilder<GuideRevenue> builder)
    {
        builder.ToTable("GuideRevenues");

        builder.Property(r => r.TotalAmount).HasColumnType("decimal(10,2)");
        builder.Property(r => r.PlatformFee).HasColumnType("decimal(10,2)");
        builder.Property(r => r.GuideEarning).HasColumnType("decimal(10,2)");

        builder.HasOne(r => r.GuideProfile)
            .WithMany()
            .HasForeignKey(r => r.GuideProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Booking)
            .WithMany()
            .HasForeignKey(r => r.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(r => r.GuideProfileId);
        builder.HasIndex(r => r.Status);
    }
}