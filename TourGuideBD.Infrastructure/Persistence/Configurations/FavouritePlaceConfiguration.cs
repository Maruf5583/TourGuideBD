using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Users;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class FavouritePlaceConfiguration : IEntityTypeConfiguration<FavouritePlace>
{
    public void Configure(EntityTypeBuilder<FavouritePlace> builder)
    {
        builder.ToTable("FavouritePlaces");

        builder.HasOne(f => f.User)
            .WithMany(u => u.FavouritePlaces)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(f => f.Place)
            .WithMany()
            .HasForeignKey(f => f.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(f => new { f.UserId, f.PlaceId }).IsUnique();
    }
}