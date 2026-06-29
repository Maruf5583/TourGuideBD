using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Trip;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class TransportTypeConfiguration : IEntityTypeConfiguration<TransportType>
{
    public void Configure(EntityTypeBuilder<TransportType> builder)
    {
        builder.ToTable("TransportTypes");

        builder.Property(t => t.Name).IsRequired().HasMaxLength(50);
        builder.HasIndex(t => t.Type).IsUnique();
    }
}