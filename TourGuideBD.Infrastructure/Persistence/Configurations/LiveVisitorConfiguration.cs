using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Realtime;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class LiveVisitorConfiguration : IEntityTypeConfiguration<LiveVisitor>
{
    public void Configure(EntityTypeBuilder<LiveVisitor> builder)
    {
        builder.ToTable("LiveVisitors");

        builder.HasOne(l => l.Place)
            .WithMany()
            .HasForeignKey(l => l.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(l => l.PlaceId).IsUnique();
    }
}