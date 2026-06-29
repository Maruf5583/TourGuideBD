using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Location;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class UpazilaConfiguration : IEntityTypeConfiguration<Upazila>
{
    public void Configure(EntityTypeBuilder<Upazila> builder)
    {
        builder.ToTable("Upazilas");

        builder.Property(u => u.Name).IsRequired().HasMaxLength(50);
        builder.Property(u => u.NameBn).IsRequired().HasMaxLength(50);

        builder.HasOne(u => u.District)
            .WithMany(d => d.Upazilas)
            .HasForeignKey(u => u.DistrictId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(u => new { u.DistrictId, u.Name }).IsUnique();
    }
}