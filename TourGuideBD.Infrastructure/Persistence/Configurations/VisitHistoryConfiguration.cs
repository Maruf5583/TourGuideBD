using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Users;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class VisitHistoryConfiguration : IEntityTypeConfiguration<VisitHistory>
{
    public void Configure(EntityTypeBuilder<VisitHistory> builder)
    {
        builder.ToTable("VisitHistories");

        builder.HasOne(v => v.User)
            .WithMany(u => u.VisitHistories)
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(v => v.Place)
            .WithMany()
            .HasForeignKey(v => v.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}