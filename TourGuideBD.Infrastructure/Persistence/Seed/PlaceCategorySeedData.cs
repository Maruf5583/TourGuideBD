using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TourGuideBD.Domain.Entities.PlaceDetails;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Infrastructure.Persistence.Seed;

public static class PlaceCategorySeedData
{
    public static void Seed(ModelBuilder builder)
    {
        builder.Entity<PlaceCategory>().HasData(
            new PlaceCategory { Id = 1, Name = "Beach", CategoryType = PlaceCategoryEnum.Beach },
            new PlaceCategory { Id = 2, Name = "Hill", CategoryType = PlaceCategoryEnum.Hill },
            new PlaceCategory { Id = 3, Name = "Forest", CategoryType = PlaceCategoryEnum.Forest },
            new PlaceCategory { Id = 4, Name = "Historical", CategoryType = PlaceCategoryEnum.Historical },
            new PlaceCategory { Id = 5, Name = "Religious", CategoryType = PlaceCategoryEnum.Religious },
            new PlaceCategory { Id = 6, Name = "Waterfall", CategoryType = PlaceCategoryEnum.Waterfall }
        );
    }
}