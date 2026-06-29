using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Trip;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class ItineraryStopConfiguration : IEntityTypeConfiguration<ItineraryStop>
{
    public void Configure(EntityTypeBuilder<ItineraryStop> builder)
    {
        builder.ToTable("ItineraryStops");

        builder.Property(s => s.TransportCost).HasColumnType("decimal(10,2)");
        builder.Property(s => s.EntryFeeAtThisStop).HasColumnType("decimal(10,2)");
        builder.Property(s => s.DistanceFromPreviousKm).HasColumnType("decimal(8,2)");
        builder.Property(s => s.TravelTimeMinutes).HasColumnType("decimal(8,2)");

        builder.HasOne(s => s.Itinerary)
            .WithMany(i => i.Stops)
            .HasForeignKey(s => s.ItineraryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Place)
            .WithMany()
            .HasForeignKey(s => s.PlaceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.TransportType)
            .WithMany()
            .HasForeignKey(s => s.TransportTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}