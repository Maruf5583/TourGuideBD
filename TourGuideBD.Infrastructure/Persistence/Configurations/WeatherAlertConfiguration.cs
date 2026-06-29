using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Realtime;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class WeatherAlertConfiguration : IEntityTypeConfiguration<WeatherAlert>
{
    public void Configure(EntityTypeBuilder<WeatherAlert> builder)
    {
        builder.ToTable("WeatherAlerts");

        builder.Property(w => w.Title).IsRequired().HasMaxLength(150);
        builder.Property(w => w.Message).IsRequired().HasMaxLength(500);
        builder.Property(w => w.Severity).IsRequired().HasMaxLength(20);

        builder.HasOne(w => w.District)
            .WithMany()
            .HasForeignKey(w => w.DistrictId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(w => new { w.DistrictId, w.IsActive });
    }
}