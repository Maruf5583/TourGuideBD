using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Trip;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class TransportRateConfiguration : IEntityTypeConfiguration<TransportRate>
{
    public void Configure(EntityTypeBuilder<TransportRate> builder)
    {
        builder.ToTable("TransportRates");

        builder.Property(r => r.RatePerKm).HasColumnType("decimal(10,2)");
        builder.Property(r => r.MinimumFare).HasColumnType("decimal(10,2)");
        builder.Property(r => r.AverageSpeedKmh).HasColumnType("decimal(6,2)");

        builder.HasOne(r => r.TransportType)
            .WithMany(t => t.Rates)
            .HasForeignKey(r => r.TransportTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}