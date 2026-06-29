using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Guide;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class GuideApplicationConfiguration : IEntityTypeConfiguration<GuideApplication>
{
    public void Configure(EntityTypeBuilder<GuideApplication> builder)
    {
        builder.ToTable("GuideApplications");

        builder.Property(g => g.FullName).IsRequired().HasMaxLength(100);
        builder.Property(g => g.PhoneNumber).IsRequired().HasMaxLength(20);

        // NidNumber সরানো হয়েছে — Photo দিয়ে replace
        builder.Property(g => g.NidFrontPhotoUrl).IsRequired().HasMaxLength(500);
        builder.Property(g => g.NidBackPhotoUrl).IsRequired().HasMaxLength(500);

        // DOB Certificate Photo add হয়েছে
        builder.Property(g => g.DobCertificatePhotoUrl).IsRequired().HasMaxLength(500);

        builder.Property(g => g.ProfilePhotoUrl).IsRequired().HasMaxLength(500);
        builder.Property(g => g.Address).IsRequired().HasMaxLength(300);
        builder.Property(g => g.Bio).HasMaxLength(1000);
        builder.Property(g => g.Languages).HasMaxLength(200);
        builder.Property(g => g.Specialities).HasMaxLength(200);
        builder.Property(g => g.CertificateUrl).HasMaxLength(500);
        builder.Property(g => g.AdminNote).HasMaxLength(500);
        builder.Property(g => g.OperatingDistrictIds).HasColumnType("nvarchar(max)");

        builder.HasIndex(g => g.UserId);
        builder.HasIndex(g => g.Status);
    }
}