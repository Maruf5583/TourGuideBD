using Microsoft.EntityFrameworkCore;
using TourGuideBD.Domain.Entities.Trip;

namespace TourGuideBD.Infrastructure.Persistence.Seed;

public static class DistrictBusRouteSeedData
{
    // BRTA approved rate
    private const decimal BrtaRatePerKm = 2.15m;

    private static decimal CalcCost(int km) =>
        Math.Round(km * BrtaRatePerKm / 5, 0) * 5; // 5 টাকা গোলাকার

    private static int CalcTime(int km) =>
        (int)(km / 45.0 * 60); // 45 km/h average bus speed

    public static void Seed(ModelBuilder builder)
    {
        builder.Entity<DistrictBusRoute>().HasData(GetRoutes());
    }

    private static List<DistrictBusRoute> GetRoutes()
    {
        var routes = new List<(int from, int to, int km)>
        {
            // ===== DHAKA (1) থেকে =====
            (1, 2, 105),   // Dhaka→Faridpur
            (1, 3, 40),    // Dhaka→Gazipur
            (1, 4, 190),   // Dhaka→Gopalganj
            (1, 5, 95),    // Dhaka→Kishoreganj
            (1, 6, 130),   // Dhaka→Madaripur
            (1, 7, 75),    // Dhaka→Manikganj
            (1, 8, 35),    // Dhaka→Munshiganj
            (1, 9, 20),    // Dhaka→Narayanganj
            (1, 10, 57),   // Dhaka→Narsingdi
            (1, 11, 115),  // Dhaka→Rajbari
            (1, 12, 95),   // Dhaka→Shariatpur
            (1, 13, 85),   // Dhaka→Tangail
            (1, 14, 392),  // Dhaka→Bandarban
            (1, 15, 135),  // Dhaka→Brahmanbaria
            (1, 16, 115),  // Dhaka→Chandpur
            (1, 17, 245),  // Dhaka→Chattogram
            (1, 18, 95),   // Dhaka→Cumilla
            (1, 19, 414),  // Dhaka→Cox's Bazar
            (1, 20, 155),  // Dhaka→Feni
            (1, 21, 310),  // Dhaka→Khagrachhari
            (1, 22, 140),  // Dhaka→Lakshmipur
            (1, 23, 170),  // Dhaka→Noakhali
            (1, 24, 350),  // Dhaka→Rangamati
            (1, 25, 197),  // Dhaka→Bogura
            (1, 26, 260),  // Dhaka→Joypurhat
            (1, 27, 286),  // Dhaka→Naogaon
            (1, 28, 213),  // Dhaka→Natore
            (1, 29, 340),  // Dhaka→Chapainawabganj
            (1, 30, 172),  // Dhaka→Pabna
            (1, 31, 254),  // Dhaka→Rajshahi
            (1, 32, 133),  // Dhaka→Sirajganj
            (1, 33, 283),  // Dhaka→Bagerhat
            (1, 34, 280),  // Dhaka→Chuadanga
            (1, 35, 270),  // Dhaka→Jashore
            (1, 36, 246),  // Dhaka→Jhenaidah
            (1, 37, 333),  // Dhaka→Khulna
            (1, 38, 210),  // Dhaka→Kushtia
            (1, 39, 215),  // Dhaka→Magura
            (1, 40, 310),  // Dhaka→Meherpur
            (1, 41, 250),  // Dhaka→Narail
            (1, 42, 342),  // Dhaka→Satkhira
            (1, 43, 310),  // Dhaka→Barguna
            (1, 44, 188),  // Dhaka→Barishal
            (1, 45, 190),  // Dhaka→Bhola
            (1, 46, 205),  // Dhaka→Jhalokati
            (1, 47, 245),  // Dhaka→Patuakhali
            (1, 48, 220),  // Dhaka→Pirojpur
            (1, 49, 165),  // Dhaka→Habiganj
            (1, 50, 210),  // Dhaka→Moulvibazar
            (1, 51, 249),  // Dhaka→Sunamganj
            (1, 52, 240),  // Dhaka→Sylhet
            (1, 53, 453),  // Dhaka→Dinajpur
            (1, 54, 287),  // Dhaka→Gaibandha
            (1, 55, 370),  // Dhaka→Kurigram
            (1, 56, 380),  // Dhaka→Lalmonirhat
            (1, 57, 420),  // Dhaka→Nilphamari
            (1, 58, 510),  // Dhaka→Panchagarh
            (1, 59, 320),  // Dhaka→Rangpur
            (1, 60, 480),  // Dhaka→Thakurgaon
            (1, 61, 150),  // Dhaka→Jamalpur
            (1, 62, 118),  // Dhaka→Mymensingh
            (1, 63, 155),  // Dhaka→Netrokona
            (1, 64, 190),  // Dhaka→Sherpur

            // ===== CHATTOGRAM (17) থেকে =====
            (17, 14, 92),  // Ctg→Bandarban
            (17, 15, 140), // Ctg→Brahmanbaria
            (17, 16, 130), // Ctg→Chandpur
            (17, 18, 100), // Ctg→Cumilla
            (17, 19, 153), // Ctg→Cox's Bazar
            (17, 20, 113), // Ctg→Feni
            (17, 21, 110), // Ctg→Khagrachhari
            (17, 22, 118), // Ctg→Lakshmipur
            (17, 23, 133), // Ctg→Noakhali
            (17, 24, 77),  // Ctg→Rangamati
            (17, 52, 330), // Ctg→Sylhet

            // ===== COX'S BAZAR (19) থেকে =====
            (19, 14, 80),  // Cox→Bandarban

            // ===== SYLHET (52) থেকে =====
            (52, 49, 93),  // Sylhet→Habiganj
            (52, 50, 60),  // Sylhet→Moulvibazar
            (52, 51, 95),  // Sylhet→Sunamganj
            (52, 62, 238), // Sylhet→Mymensingh
            (52, 15, 135), // Sylhet→Brahmanbaria

            // ===== RAJSHAHI (31) থেকে =====
            (31, 25, 89),  // Rajshahi→Bogura
            (31, 26, 145), // Rajshahi→Joypurhat
            (31, 27, 86),  // Rajshahi→Naogaon
            (31, 28, 40),  // Rajshahi→Natore
            (31, 29, 45),  // Rajshahi→Chapainawabganj
            (31, 30, 120), // Rajshahi→Pabna
            (31, 32, 135), // Rajshahi→Sirajganj
            (31, 37, 183), // Rajshahi→Khulna
            (31, 59, 150), // Rajshahi→Rangpur
            (31, 53, 200), // Rajshahi→Dinajpur

            // ===== KHULNA (37) থেকে =====
            (37, 33, 35),  // Khulna→Bagerhat
            (37, 34, 128), // Khulna→Chuadanga
            (37, 35, 65),  // Khulna→Jashore
            (37, 36, 100), // Khulna→Jhenaidah
            (37, 38, 175), // Khulna→Kushtia
            (37, 39, 110), // Khulna→Magura
            (37, 40, 192), // Khulna→Meherpur
            (37, 41, 73),  // Khulna→Narail
            (37, 42, 83),  // Khulna→Satkhira
            (37, 44, 122), // Khulna→Barishal

            // ===== BARISHAL (44) থেকে =====
            (44, 43, 99),  // Barishal→Barguna
            (44, 45, 65),  // Barishal→Bhola
            (44, 46, 30),  // Barishal→Jhalokati
            (44, 47, 80),  // Barishal→Patuakhali
            (44, 48, 57),  // Barishal→Pirojpur

            // ===== RANGPUR (59) থেকে =====
            (59, 53, 100), // Rangpur→Dinajpur
            (59, 54, 65),  // Rangpur→Gaibandha
            (59, 55, 115), // Rangpur→Kurigram
            (59, 56, 90),  // Rangpur→Lalmonirhat
            (59, 57, 55),  // Rangpur→Nilphamari
            (59, 58, 170), // Rangpur→Panchagarh
            (59, 60, 140), // Rangpur→Thakurgaon
            (59, 25, 120), // Rangpur→Bogura

            // ===== BOGURA (25) থেকে =====
            (25, 26, 55),  // Bogura→Joypurhat
            (25, 27, 105), // Bogura→Naogaon
            (25, 28, 95),  // Bogura→Natore
            (25, 30, 105), // Bogura→Pabna
            (25, 32, 55),  // Bogura→Sirajganj
            (25, 13, 110), // Bogura→Tangail
            (25, 53, 130), // Bogura→Dinajpur

            // ===== MYMENSINGH (62) থেকে =====
            (62, 61, 55),  // Mymensingh→Jamalpur
            (62, 63, 50),  // Mymensingh→Netrokona
            (62, 64, 52),  // Mymensingh→Sherpur
            (62, 5, 100),  // Mymensingh→Kishoreganj
            (62, 13, 63),  // Mymensingh→Tangail

            // ===== CUMILLA (18) থেকে =====
            (18, 15, 55),  // Cumilla→Brahmanbaria
            (18, 16, 55),  // Cumilla→Chandpur
            (18, 20, 70),  // Cumilla→Feni
            (18, 22, 80),  // Cumilla→Lakshmipur
            (18, 23, 90),  // Cumilla→Noakhali

            // ===== JASHORE (35) থেকে =====
            (35, 34, 75),  // Jashore→Chuadanga
            (35, 36, 60),  // Jashore→Jhenaidah
            (35, 38, 110), // Jashore→Kushtia
            (35, 39, 55),  // Jashore→Magura
            (35, 40, 100), // Jashore→Meherpur
            (35, 41, 80),  // Jashore→Narail
            (35, 42, 90),  // Jashore→Satkhira
        };

        var result = new List<DistrictBusRoute>();
        int id = 1;

        foreach (var (from, to, km) in routes)
        {
            result.Add(new DistrictBusRoute
            {
                Id = id++,
                FromDistrictId = from,
                ToDistrictId = to,
                BusCost = CalcCost(km),
                BusTimeMinutes = CalcTime(km),
                IsBidirectional = true
            });
        }

        return result;
    }
}