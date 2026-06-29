using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Location;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class PlaceConfiguration : IEntityTypeConfiguration<Place>
{
    public void Configure(EntityTypeBuilder<Place> builder)
    {
        builder.ToTable("Places");

        builder.Property(p => p.Name).IsRequired().HasMaxLength(150);
        builder.Property(p => p.NameBn).IsRequired().HasMaxLength(150);
        builder.Property(p => p.Description).HasMaxLength(4000);
        builder.Property(p => p.OpeningHours).HasMaxLength(100);
        builder.Property(p => p.ClosingHours).HasMaxLength(100);

        builder.Property(p => p.EntryFee).HasColumnType("decimal(10,2)");

        // double -> SQL float (8-byte), correct mapping for GPS coordinates & ratings
        builder.Property(p => p.Latitude).HasColumnType("float");
        builder.Property(p => p.Longitude).HasColumnType("float");
        builder.Property(p => p.AverageRating).HasColumnType("float");

        builder.HasOne(p => p.Division)
            .WithMany()
            .HasForeignKey(p => p.DivisionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.District)
            .WithMany(d => d.Places)
            .HasForeignKey(p => p.DistrictId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Upazila)
            .WithMany(u => u.Places)
            .HasForeignKey(p => p.UpazilaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.SubmittedByUser)
            .WithMany(u => u.SubmittedPlaces)
            .HasForeignKey(p => p.SubmittedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(p => p.Name);
        builder.HasIndex(p => new { p.Latitude, p.Longitude });
        builder.HasIndex(p => p.ApprovalStatus);
    }
}