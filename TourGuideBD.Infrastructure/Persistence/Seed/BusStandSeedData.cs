using Microsoft.EntityFrameworkCore;
using TourGuideBD.Domain.Entities.Trip;

namespace TourGuideBD.Infrastructure.Persistence.Seed;

public static class BusStandSeedData
{
    public static void Seed(ModelBuilder builder)
    {
        builder.Entity<BusStand>().HasData(GetBusStands());
    }

    private static List<BusStand> GetBusStands()
    {
        return new List<BusStand>
        {
            // ===== DHAKA DIVISION =====
            // Dhaka (District Id=1) — 3 main terminals
            new() { Id = 1,  Name = "Saidabad Bus Terminal",         NameBn = "সায়েদাবাদ বাস টার্মিনাল",        DistrictId = 1,  Latitude = 23.7191, Longitude = 90.4283, IsMainStand = true },
            new() { Id = 2,  Name = "Mohakhali Bus Terminal",         NameBn = "মহাখালী বাস টার্মিনাল",          DistrictId = 1,  Latitude = 23.7808, Longitude = 90.4035, IsMainStand = true },
            new() { Id = 3,  Name = "Gabtoli Bus Terminal",           NameBn = "গাবতলী বাস টার্মিনাল",           DistrictId = 1,  Latitude = 23.7737, Longitude = 90.3481, IsMainStand = true },

            // Faridpur (2)
            new() { Id = 4,  Name = "Faridpur Bus Terminal",          NameBn = "ফরিদপুর বাস টার্মিনাল",          DistrictId = 2,  Latitude = 23.6070, Longitude = 89.8429, IsMainStand = true },

            // Gazipur (3)
            new() { Id = 5,  Name = "Gazipur Chowrasta Bus Terminal", NameBn = "গাজীপুর চৌরাস্তা বাস টার্মিনাল", DistrictId = 3,  Latitude = 23.9999, Longitude = 90.4203, IsMainStand = true },

            // Gopalganj (4)
            new() { Id = 6,  Name = "Gopalganj Bus Terminal",         NameBn = "গোপালগঞ্জ বাস টার্মিনাল",        DistrictId = 4,  Latitude = 23.0050, Longitude = 89.8261, IsMainStand = true },

            // Kishoreganj (5)
            new() { Id = 7,  Name = "Kishoreganj Bus Terminal",       NameBn = "কিশোরগঞ্জ বাস টার্মিনাল",       DistrictId = 5,  Latitude = 24.4449, Longitude = 90.7766, IsMainStand = true },

            // Madaripur (6)
            new() { Id = 8,  Name = "Madaripur Bus Terminal",         NameBn = "মাদারীপুর বাস টার্মিনাল",        DistrictId = 6,  Latitude = 23.1641, Longitude = 90.1991, IsMainStand = true },

            // Manikganj (7)
            new() { Id = 9,  Name = "Manikganj Bus Terminal",         NameBn = "মানিকগঞ্জ বাস টার্মিনাল",        DistrictId = 7,  Latitude = 23.8643, Longitude = 90.0021, IsMainStand = true },

            // Munshiganj (8)
            new() { Id = 10, Name = "Munshiganj Bus Terminal",        NameBn = "মুন্সিগঞ্জ বাস টার্মিনাল",       DistrictId = 8,  Latitude = 23.5422, Longitude = 90.5300, IsMainStand = true },

            // Narayanganj (9)
            new() { Id = 11, Name = "Narayanganj Bus Terminal",       NameBn = "নারায়ণগঞ্জ বাস টার্মিনাল",      DistrictId = 9,  Latitude = 23.6238, Longitude = 90.4987, IsMainStand = true },

            // Narsingdi (10)
            new() { Id = 12, Name = "Narsingdi Bus Terminal",         NameBn = "নরসিংদী বাস টার্মিনাল",         DistrictId = 10, Latitude = 23.9225, Longitude = 90.7154, IsMainStand = true },

            // Rajbari (11)
            new() { Id = 13, Name = "Rajbari Bus Terminal",           NameBn = "রাজবাড়ী বাস টার্মিনাল",         DistrictId = 11, Latitude = 23.7574, Longitude = 89.6440, IsMainStand = true },

            // Shariatpur (12)
            new() { Id = 14, Name = "Shariatpur Bus Terminal",        NameBn = "শরীয়তপুর বাস টার্মিনাল",        DistrictId = 12, Latitude = 23.2433, Longitude = 90.4352, IsMainStand = true },

            // Tangail (13)
            new() { Id = 15, Name = "Tangail Bus Terminal",           NameBn = "টাঙ্গাইল বাস টার্মিনাল",         DistrictId = 13, Latitude = 24.2513, Longitude = 89.9167, IsMainStand = true },

            // ===== CHATTOGRAM DIVISION =====
            // Bandarban (14)
            new() { Id = 16, Name = "Bandarban Bus Terminal",         NameBn = "বান্দরবান বাস টার্মিনাল",         DistrictId = 14, Latitude = 22.1953, Longitude = 92.2184, IsMainStand = true },

            // Brahmanbaria (15)
            new() { Id = 17, Name = "Brahmanbaria Bus Terminal",      NameBn = "ব্রাহ্মণবাড়িয়া বাস টার্মিনাল",  DistrictId = 15, Latitude = 23.9608, Longitude = 91.1115, IsMainStand = true },

            // Chandpur (16)
            new() { Id = 18, Name = "Chandpur Bus Terminal",          NameBn = "চাঁদপুর বাস টার্মিনাল",          DistrictId = 16, Latitude = 23.2333, Longitude = 90.6517, IsMainStand = true },

            // Chattogram (17)
            new() { Id = 19, Name = "Dampara Bus Terminal",           NameBn = "দামপাড়া বাস টার্মিনাল",          DistrictId = 17, Latitude = 22.3569, Longitude = 91.7832, IsMainStand = true },

            // Cumilla (18)
            new() { Id = 20, Name = "Cumilla Bus Terminal",           NameBn = "কুমিল্লা বাস টার্মিনাল",          DistrictId = 18, Latitude = 23.4607, Longitude = 91.1809, IsMainStand = true },

            // Cox's Bazar (19)
            new() { Id = 21, Name = "Cox's Bazar Bus Terminal",       NameBn = "কক্সবাজার বাস টার্মিনাল",         DistrictId = 19, Latitude = 21.4272, Longitude = 92.0058, IsMainStand = true },

            // Feni (20)
            new() { Id = 22, Name = "Feni Bus Terminal",              NameBn = "ফেনী বাস টার্মিনাল",              DistrictId = 20, Latitude = 23.0235, Longitude = 91.3960, IsMainStand = true },

            // Khagrachhari (21)
            new() { Id = 23, Name = "Khagrachhari Bus Terminal",      NameBn = "খাগড়াছড়ি বাস টার্মিনাল",        DistrictId = 21, Latitude = 23.1193, Longitude = 91.9847, IsMainStand = true },

            // Lakshmipur (22)
            new() { Id = 24, Name = "Lakshmipur Bus Terminal",        NameBn = "লক্ষ্মীপুর বাস টার্মিনাল",        DistrictId = 22, Latitude = 22.9425, Longitude = 90.8412, IsMainStand = true },

            // Noakhali (23)
            new() { Id = 25, Name = "Noakhali Bus Terminal",          NameBn = "নোয়াখালী বাস টার্মিনাল",          DistrictId = 23, Latitude = 22.8696, Longitude = 91.0996, IsMainStand = true },

            // Rangamati (24)
            new() { Id = 26, Name = "Rangamati Bus Terminal",         NameBn = "রাঙ্গামাটি বাস টার্মিনাল",         DistrictId = 24, Latitude = 22.6452, Longitude = 92.1703, IsMainStand = true },

            // ===== RAJSHAHI DIVISION =====
            // Bogura (25)
            new() { Id = 27, Name = "Bogura Bus Terminal",            NameBn = "বগুড়া বাস টার্মিনাল",            DistrictId = 25, Latitude = 24.8465, Longitude = 89.3773, IsMainStand = true },

            // Joypurhat (26)
            new() { Id = 28, Name = "Joypurhat Bus Terminal",         NameBn = "জয়পুরহাট বাস টার্মিনাল",         DistrictId = 26, Latitude = 25.1025, Longitude = 89.0226, IsMainStand = true },

            // Naogaon (27)
            new() { Id = 29, Name = "Naogaon Bus Terminal",           NameBn = "নওগাঁ বাস টার্মিনাল",             DistrictId = 27, Latitude = 24.8044, Longitude = 88.9356, IsMainStand = true },

            // Natore (28)
            new() { Id = 30, Name = "Natore Bus Terminal",            NameBn = "নাটোর বাস টার্মিনাল",             DistrictId = 28, Latitude = 24.4204, Longitude = 88.9945, IsMainStand = true },

            // Chapainawabganj (29)
            new() { Id = 31, Name = "Chapainawabganj Bus Terminal",   NameBn = "চাঁপাইনবাবগঞ্জ বাস টার্মিনাল",   DistrictId = 29, Latitude = 24.5965, Longitude = 88.2775, IsMainStand = true },

            // Pabna (30)
            new() { Id = 32, Name = "Pabna Bus Terminal",             NameBn = "পাবনা বাস টার্মিনাল",             DistrictId = 30, Latitude = 24.0064, Longitude = 89.2372, IsMainStand = true },

            // Rajshahi (31)
            new() { Id = 33, Name = "Rajshahi Bus Terminal",          NameBn = "রাজশাহী বাস টার্মিনাল",           DistrictId = 31, Latitude = 24.3636, Longitude = 88.6241, IsMainStand = true },

            // Sirajganj (32)
            new() { Id = 34, Name = "Sirajganj Bus Terminal",         NameBn = "সিরাজগঞ্জ বাস টার্মিনাল",         DistrictId = 32, Latitude = 24.4543, Longitude = 89.7006, IsMainStand = true },

            // ===== KHULNA DIVISION =====
            // Bagerhat (33)
            new() { Id = 35, Name = "Bagerhat Bus Terminal",          NameBn = "বাগেরহাট বাস টার্মিনাল",          DistrictId = 33, Latitude = 22.6602, Longitude = 89.7854, IsMainStand = true },

            // Chuadanga (34)
            new() { Id = 36, Name = "Chuadanga Bus Terminal",         NameBn = "চুয়াডাঙ্গা বাস টার্মিনাল",        DistrictId = 34, Latitude = 23.6401, Longitude = 88.8415, IsMainStand = true },

            // Jashore (35)
            new() { Id = 37, Name = "Jashore Bus Terminal",           NameBn = "যশোর বাস টার্মিনাল",              DistrictId = 35, Latitude = 23.1664, Longitude = 89.2080, IsMainStand = true },

            // Jhenaidah (36)
            new() { Id = 38, Name = "Jhenaidah Bus Terminal",         NameBn = "ঝিনাইদহ বাস টার্মিনাল",           DistrictId = 36, Latitude = 23.5449, Longitude = 89.1543, IsMainStand = true },

            // Khulna (37)
            new() { Id = 39, Name = "Sonadanga Bus Terminal",         NameBn = "সোনাডাঙ্গা বাস টার্মিনাল",        DistrictId = 37, Latitude = 22.8456, Longitude = 89.5403, IsMainStand = true },

            // Kushtia (38)
            new() { Id = 40, Name = "Kushtia Bus Terminal",           NameBn = "কুষ্টিয়া বাস টার্মিনাল",          DistrictId = 38, Latitude = 23.9012, Longitude = 89.1202, IsMainStand = true },

            // Magura (39)
            new() { Id = 41, Name = "Magura Bus Terminal",            NameBn = "মাগুরা বাস টার্মিনাল",            DistrictId = 39, Latitude = 23.4876, Longitude = 89.4197, IsMainStand = true },

            // Meherpur (40)
            new() { Id = 42, Name = "Meherpur Bus Terminal",          NameBn = "মেহেরপুর বাস টার্মিনাল",          DistrictId = 40, Latitude = 23.7622, Longitude = 88.6318, IsMainStand = true },

            // Narail (41)
            new() { Id = 43, Name = "Narail Bus Terminal",            NameBn = "নড়াইল বাস টার্মিনাল",            DistrictId = 41, Latitude = 23.1731, Longitude = 89.5122, IsMainStand = true },

            // Satkhira (42)
            new() { Id = 44, Name = "Satkhira Bus Terminal",          NameBn = "সাতক্ষীরা বাস টার্মিনাল",         DistrictId = 42, Latitude = 22.7185, Longitude = 89.0705, IsMainStand = true },

            // ===== BARISHAL DIVISION =====
            // Barguna (43)
            new() { Id = 45, Name = "Barguna Bus Terminal",           NameBn = "বরগুনা বাস টার্মিনাল",            DistrictId = 43, Latitude = 22.1500, Longitude = 90.1118, IsMainStand = true },

            // Barishal (44)
            new() { Id = 46, Name = "Natullabad Bus Terminal",        NameBn = "নথুল্লাবাদ বাস টার্মিনাল",        DistrictId = 44, Latitude = 22.7010, Longitude = 90.3535, IsMainStand = true },

            // Bhola (45)
            new() { Id = 47, Name = "Bhola Bus Terminal",             NameBn = "ভোলা বাস টার্মিনাল",              DistrictId = 45, Latitude = 22.6858, Longitude = 90.6482, IsMainStand = true },

            // Jhalokati (46)
            new() { Id = 48, Name = "Jhalokati Bus Terminal",         NameBn = "ঝালকাঠি বাস টার্মিনাল",           DistrictId = 46, Latitude = 22.6406, Longitude = 90.1988, IsMainStand = true },

            // Patuakhali (47)
            new() { Id = 49, Name = "Patuakhali Bus Terminal",        NameBn = "পটুয়াখালী বাস টার্মিনাল",         DistrictId = 47, Latitude = 22.3596, Longitude = 90.3298, IsMainStand = true },

            // Pirojpur (48)
            new() { Id = 50, Name = "Pirojpur Bus Terminal",          NameBn = "পিরোজপুর বাস টার্মিনাল",          DistrictId = 48, Latitude = 22.5841, Longitude = 89.9754, IsMainStand = true },

            // ===== SYLHET DIVISION =====
            // Habiganj (49)
            new() { Id = 51, Name = "Habiganj Bus Terminal",          NameBn = "হবিগঞ্জ বাস টার্মিনাল",           DistrictId = 49, Latitude = 24.3745, Longitude = 91.4158, IsMainStand = true },

            // Moulvibazar (50)
            new() { Id = 52, Name = "Moulvibazar Bus Terminal",       NameBn = "মৌলভীবাজার বাস টার্মিনাল",        DistrictId = 50, Latitude = 24.4826, Longitude = 91.7774, IsMainStand = true },

            // Sunamganj (51)
            new() { Id = 53, Name = "Sunamganj Bus Terminal",         NameBn = "সুনামগঞ্জ বাস টার্মিনাল",         DistrictId = 51, Latitude = 24.8807, Longitude = 91.3962, IsMainStand = true },

            // Sylhet (52)
            new() { Id = 54, Name = "Sylhet Kadamtali Bus Terminal",  NameBn = "সিলেট কদমতলী বাস টার্মিনাল",      DistrictId = 52, Latitude = 24.8949, Longitude = 91.8687, IsMainStand = true },

            // ===== RANGPUR DIVISION =====
            // Dinajpur (53)
            new() { Id = 55, Name = "Dinajpur Bus Terminal",          NameBn = "দিনাজপুর বাস টার্মিনাল",          DistrictId = 53, Latitude = 25.6279, Longitude = 88.6338, IsMainStand = true },

            // Gaibandha (54)
            new() { Id = 56, Name = "Gaibandha Bus Terminal",         NameBn = "গাইবান্ধা বাস টার্মিনাল",          DistrictId = 54, Latitude = 25.3288, Longitude = 89.5288, IsMainStand = true },

            // Kurigram (55)
            new() { Id = 57, Name = "Kurigram Bus Terminal",          NameBn = "কুড়িগ্রাম বাস টার্মিনাল",         DistrictId = 55, Latitude = 25.8074, Longitude = 89.6360, IsMainStand = true },

            // Lalmonirhat (56)
            new() { Id = 58, Name = "Lalmonirhat Bus Terminal",       NameBn = "লালমনিরহাট বাস টার্মিনাল",        DistrictId = 56, Latitude = 25.9218, Longitude = 89.4507, IsMainStand = true },

            // Nilphamari (57)
            new() { Id = 59, Name = "Nilphamari Bus Terminal",        NameBn = "নীলফামারী বাস টার্মিনাল",         DistrictId = 57, Latitude = 25.9318, Longitude = 88.8560, IsMainStand = true },

            // Panchagarh (58)
            new() { Id = 60, Name = "Panchagarh Bus Terminal",        NameBn = "পঞ্চগড় বাস টার্মিনাল",            DistrictId = 58, Latitude = 26.3411, Longitude = 88.5541, IsMainStand = true },

            // Rangpur (59)
            new() { Id = 61, Name = "Rangpur Modern Bus Terminal",    NameBn = "রংপুর মডার্ন বাস টার্মিনাল",       DistrictId = 59, Latitude = 25.7439, Longitude = 89.2752, IsMainStand = true },

            // Thakurgaon (60)
            new() { Id = 62, Name = "Thakurgaon Bus Terminal",        NameBn = "ঠাকুরগাঁও বাস টার্মিনাল",         DistrictId = 60, Latitude = 26.0336, Longitude = 88.4616, IsMainStand = true },

            // ===== MYMENSINGH DIVISION =====
            // Jamalpur (61)
            new() { Id = 63, Name = "Jamalpur Bus Terminal",          NameBn = "জামালপুর বাস টার্মিনাল",          DistrictId = 61, Latitude = 24.9375, Longitude = 89.9370, IsMainStand = true },

            // Mymensingh (62)
            new() { Id = 64, Name = "Mymensingh Bus Terminal",        NameBn = "ময়মনসিংহ বাস টার্মিনাল",         DistrictId = 62, Latitude = 24.7471, Longitude = 90.4203, IsMainStand = true },

            // Netrokona (63)
            new() { Id = 65, Name = "Netrokona Bus Terminal",         NameBn = "নেত্রকোণা বাস টার্মিনাল",         DistrictId = 63, Latitude = 24.8696, Longitude = 90.7270, IsMainStand = true },

            // Sherpur (64)
            new() { Id = 66, Name = "Sherpur Bus Terminal",           NameBn = "শেরপুর বাস টার্মিনাল",            DistrictId = 64, Latitude = 25.0204, Longitude = 90.0152, IsMainStand = true },
        };
    }
}