using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Realtime;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class BroadcastConfiguration : IEntityTypeConfiguration<Broadcast>
{
    public void Configure(EntityTypeBuilder<Broadcast> builder)
    {
        builder.ToTable("Broadcasts");

        builder.Property(b => b.Title).IsRequired().HasMaxLength(150);
        builder.Property(b => b.Message).IsRequired().HasMaxLength(1000);

        builder.HasOne(b => b.District)
            .WithMany()
            .HasForeignKey(b => b.DistrictId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}