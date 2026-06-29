using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Guide;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class TourPackageConfiguration : IEntityTypeConfiguration<TourPackage>
{
    public void Configure(EntityTypeBuilder<TourPackage> builder)
    {
        builder.ToTable("TourPackages");

        builder.Property(t => t.Title).IsRequired().HasMaxLength(200);
        builder.Property(t => t.Description).HasMaxLength(2000);
        builder.Property(t => t.PricePerPerson).HasColumnType("decimal(10,2)");
        builder.Property(t => t.MeetingPoint).HasMaxLength(300);
        builder.Property(t => t.MeetingLat).HasColumnType("float");
        builder.Property(t => t.MeetingLng).HasColumnType("float");
        builder.Property(t => t.PlaceIds).HasColumnType("nvarchar(max)");
        builder.Property(t => t.AdditionalIncludes).HasMaxLength(500);

        builder.HasOne(t => t.GuideProfile)
            .WithMany(g => g.TourPackages)
            .HasForeignKey(t => t.GuideProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}