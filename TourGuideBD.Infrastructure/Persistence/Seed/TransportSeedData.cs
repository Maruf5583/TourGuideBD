using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TourGuideBD.Domain.Entities.Trip;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Infrastructure.Persistence.Seed;

public static class TransportSeedData
{
    public static void Seed(ModelBuilder builder)
    {
        builder.Entity<TransportType>().HasData(
            new TransportType { Id = 1, Name = "Bus", Type = TransportTypeEnum.Bus },
            new TransportType { Id = 2, Name = "CNG", Type = TransportTypeEnum.CNG },
            new TransportType { Id = 3, Name = "Train", Type = TransportTypeEnum.Train },
            new TransportType { Id = 4, Name = "Boat", Type = TransportTypeEnum.Boat },
            new TransportType { Id = 5, Name = "Car", Type = TransportTypeEnum.Car },
            new TransportType { Id = 6, Name = "Bike", Type = TransportTypeEnum.Bike }
        );

        // Rates based on approximate Bangladesh averages (BDT per km)
        builder.Entity<TransportRate>().HasData(
            new TransportRate { Id = 1, TransportTypeId = 1, RatePerKm = 1.80m, AverageSpeedKmh = 40, MinimumFare = 20m, IsActive = true },   // Bus
            new TransportRate { Id = 2, TransportTypeId = 2, RatePerKm = 8.00m, AverageSpeedKmh = 25, MinimumFare = 50m, IsActive = true },   // CNG
            new TransportRate { Id = 3, TransportTypeId = 3, RatePerKm = 1.20m, AverageSpeedKmh = 50, MinimumFare = 30m, IsActive = true },   // Train
            new TransportRate { Id = 4, TransportTypeId = 4, RatePerKm = 5.00m, AverageSpeedKmh = 15, MinimumFare = 50m, IsActive = true },   // Boat
            new TransportRate { Id = 5, TransportTypeId = 5, RatePerKm = 15.00m, AverageSpeedKmh = 45, MinimumFare = 100m, IsActive = true }, // Car
            new TransportRate { Id = 6, TransportTypeId = 6, RatePerKm = 4.00m, AverageSpeedKmh = 35, MinimumFare = 20m, IsActive = true }    // Bike
        );
    }
}