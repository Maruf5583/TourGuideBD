using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Users;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class SavedDistrictConfiguration : IEntityTypeConfiguration<SavedDistrict>
{
    public void Configure(EntityTypeBuilder<SavedDistrict> builder)
    {
        builder.ToTable("SavedDistricts");

        builder.HasOne(s => s.User)
            .WithMany(u => u.SavedDistricts)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.District)
            .WithMany()
            .HasForeignKey(s => s.DistrictId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => new { s.UserId, s.DistrictId }).IsUnique();
    }
}