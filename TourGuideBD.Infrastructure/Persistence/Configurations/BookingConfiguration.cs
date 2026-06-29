using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Guide;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");

        builder.Property(b => b.TotalAmount).HasColumnType("decimal(10,2)");
        builder.Property(b => b.PlatformFee).HasColumnType("decimal(10,2)");
        builder.Property(b => b.GuideEarning).HasColumnType("decimal(10,2)");
        builder.Property(b => b.StripePaymentIntentId).HasMaxLength(200);
        builder.Property(b => b.StripeChargeId).HasMaxLength(200);
        builder.Property(b => b.CancellationReason).HasMaxLength(500);

        builder.HasOne(b => b.TourPackage)
            .WithMany(t => t.Bookings)
            .HasForeignKey(b => b.TourPackageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(b => b.UserId);
        builder.HasIndex(b => b.Status);
        builder.HasIndex(b => b.StripePaymentIntentId);
    }
}