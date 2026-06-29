using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Trip;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class ItineraryConfiguration : IEntityTypeConfiguration<Itinerary>
{
    public void Configure(EntityTypeBuilder<Itinerary> builder)
    {
        builder.ToTable("Itineraries");

        builder.Property(i => i.Title).IsRequired().HasMaxLength(150);
        builder.Property(i => i.EstimatedTotalCost).HasColumnType("decimal(10,2)");
        builder.Property(i => i.EstimatedFoodCost).HasColumnType("decimal(10,2)");

        builder.HasOne(i => i.User)
            .WithMany(u => u.Itineraries)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}