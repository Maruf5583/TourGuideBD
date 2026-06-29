using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Users;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class CheckInConfiguration : IEntityTypeConfiguration<CheckIn>
{
    public void Configure(EntityTypeBuilder<CheckIn> builder)
    {
        builder.ToTable("CheckIns");

        builder.Property(c => c.Note).HasMaxLength(300);

        builder.HasOne(c => c.User)
            .WithMany(u => u.CheckIns)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Place)
            .WithMany()
            .HasForeignKey(c => c.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}