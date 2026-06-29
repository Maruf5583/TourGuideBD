using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Location;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class DivisionConfiguration : IEntityTypeConfiguration<Division>
{
    public void Configure(EntityTypeBuilder<Division> builder)
    {
        builder.ToTable("Divisions");

        builder.Property(d => d.Name).IsRequired().HasMaxLength(50);
        builder.Property(d => d.NameBn).IsRequired().HasMaxLength(50);

        builder.HasIndex(d => d.Name).IsUnique();
    }
}