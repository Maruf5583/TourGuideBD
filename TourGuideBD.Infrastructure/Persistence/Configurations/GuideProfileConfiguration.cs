using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Guide;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class GuideProfileConfiguration : IEntityTypeConfiguration<GuideProfile>
{
    public void Configure(EntityTypeBuilder<GuideProfile> builder)
    {
        builder.ToTable("GuideProfiles");

        builder.Property(g => g.FullName).IsRequired().HasMaxLength(100);
        builder.Property(g => g.PhoneNumber).IsRequired().HasMaxLength(20);
        builder.Property(g => g.ProfilePhotoUrl).IsRequired().HasMaxLength(500);
        builder.Property(g => g.Bio).HasMaxLength(1000);
        builder.Property(g => g.Languages).HasMaxLength(200);
        builder.Property(g => g.Specialities).HasMaxLength(200);
        builder.Property(g => g.OperatingDistrictIds).HasColumnType("nvarchar(max)");
        builder.Property(g => g.StripeAccountId).HasMaxLength(100);
        builder.Property(g => g.AverageRating).HasColumnType("float");

        builder.HasOne(g => g.Application)
            .WithMany()
            .HasForeignKey(g => g.ApplicationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(g => g.UserId).IsUnique();
        builder.HasIndex(g => g.IsActive);
    }
}