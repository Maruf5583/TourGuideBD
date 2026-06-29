using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Location;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class DistrictConfiguration : IEntityTypeConfiguration<District>
{
    public void Configure(EntityTypeBuilder<District> builder)
    {
        builder.ToTable("Districts");

        builder.Property(d => d.Name).IsRequired().HasMaxLength(50);
        builder.Property(d => d.NameBn).IsRequired().HasMaxLength(50);

        builder.HasIndex(d => d.Name).IsUnique();

        builder.HasOne(d => d.Division)
            .WithMany(div => div.Districts)
            .HasForeignKey(d => d.DivisionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}