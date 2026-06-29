using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TourGuideBD.Domain.Entities.Location;

namespace TourGuideBD.Infrastructure.Persistence.Seed;

public static class LocationSeedData
{
    public static void Seed(ModelBuilder builder)
    {
        var divisions = GetDivisions();
        builder.Entity<Division>().HasData(divisions);

        var districts = GetDistricts();
        builder.Entity<District>().HasData(districts);

        var upazilas = GetUpazilas();
        builder.Entity<Upazila>().HasData(upazilas);
    }

    private static List<Division> GetDivisions()
    {
        return new List<Division>
        {
            new() { Id = 1, Name = "Dhaka", NameBn = "ঢাকা" },
            new() { Id = 2, Name = "Chattogram", NameBn = "চট্টগ্রাম" },
            new() { Id = 3, Name = "Rajshahi", NameBn = "রাজশাহী" },
            new() { Id = 4, Name = "Khulna", NameBn = "খুলনা" },
            new() { Id = 5, Name = "Barishal", NameBn = "বরিশাল" },
            new() { Id = 6, Name = "Sylhet", NameBn = "সিলেট" },
            new() { Id = 7, Name = "Rangpur", NameBn = "রংপুর" },
            new() { Id = 8, Name = "Mymensingh", NameBn = "ময়মনসিংহ" },
        };
    }

    private static List<District> GetDistricts()
    {
        var list = new List<District>();
        int id = 1;

        void Add(string name, string nameBn, int divisionId)
        {
            list.Add(new District { Id = id++, Name = name, NameBn = nameBn, DivisionId = divisionId });
        }

        // Dhaka Division (13)
        Add("Dhaka", "ঢাকা", 1);
        Add("Faridpur", "ফরিদপুর", 1);
        Add("Gazipur", "গাজীপুর", 1);
        Add("Gopalganj", "গোপালগঞ্জ", 1);
        Add("Kishoreganj", "কিশোরগঞ্জ", 1);
        Add("Madaripur", "মাদারীপুর", 1);
        Add("Manikganj", "মানিকগঞ্জ", 1);
        Add("Munshiganj", "মুন্সিগঞ্জ", 1);
        Add("Narayanganj", "নারায়ণগঞ্জ", 1);
        Add("Narsingdi", "নরসিংদী", 1);
        Add("Rajbari", "রাজবাড়ী", 1);
        Add("Shariatpur", "শরীয়তপুর", 1);
        Add("Tangail", "টাঙ্গাইল", 1);

        // Chattogram Division (11)
        Add("Bandarban", "বান্দরবান", 2);
        Add("Brahmanbaria", "ব্রাহ্মণবাড়িয়া", 2);
        Add("Chandpur", "চাঁদপুর", 2);
        Add("Chattogram", "চট্টগ্রাম", 2);
        Add("Cumilla", "কুমিল্লা", 2);
        Add("Cox's Bazar", "কক্সবাজার", 2);
        Add("Feni", "ফেনী", 2);
        Add("Khagrachhari", "খাগড়াছড়ি", 2);
        Add("Lakshmipur", "লক্ষ্মীপুর", 2);
        Add("Noakhali", "নোয়াখালী", 2);
        Add("Rangamati", "রাঙ্গামাটি", 2);

        // Rajshahi Division (8)
        Add("Bogura", "বগুড়া", 3);
        Add("Joypurhat", "জয়পুরহাট", 3);
        Add("Naogaon", "নওগাঁ", 3);
        Add("Natore", "নাটোর", 3);
        Add("Chapainawabganj", "চাঁপাইনবাবগঞ্জ", 3);
        Add("Pabna", "পাবনা", 3);
        Add("Rajshahi", "রাজশাহী", 3);
        Add("Sirajganj", "সিরাজগঞ্জ", 3);

        // Khulna Division (10)
        Add("Bagerhat", "বাগেরহাট", 4);
        Add("Chuadanga", "চুয়াডাঙ্গা", 4);
        Add("Jashore", "যশোর", 4);
        Add("Jhenaidah", "ঝিনাইদহ", 4);
        Add("Khulna", "খুলনা", 4);
        Add("Kushtia", "কুষ্টিয়া", 4);
        Add("Magura", "মাগুরা", 4);
        Add("Meherpur", "মেহেরপুর", 4);
        Add("Narail", "নড়াইল", 4);
        Add("Satkhira", "সাতক্ষীরা", 4);

        // Barishal Division (6)
        Add("Barguna", "বরগুনা", 5);
        Add("Barishal", "বরিশাল", 5);
        Add("Bhola", "ভোলা", 5);
        Add("Jhalokati", "ঝালকাঠি", 5);
        Add("Patuakhali", "পটুয়াখালী", 5);
        Add("Pirojpur", "পিরোজপুর", 5);

        // Sylhet Division (4)
        Add("Habiganj", "হবিগঞ্জ", 6);
        Add("Moulvibazar", "মৌলভীবাজার", 6);
        Add("Sunamganj", "সুনামগঞ্জ", 6);
        Add("Sylhet", "সিলেট", 6);

        // Rangpur Division (8)
        Add("Dinajpur", "দিনাজপুর", 7);
        Add("Gaibandha", "গাইবান্ধা", 7);
        Add("Kurigram", "কুড়িগ্রাম", 7);
        Add("Lalmonirhat", "লালমনিরহাট", 7);
        Add("Nilphamari", "নীলফামারী", 7);
        Add("Panchagarh", "পঞ্চগড়", 7);
        Add("Rangpur", "রংপুর", 7);
        Add("Thakurgaon", "ঠাকুরগাঁও", 7);

        // Mymensingh Division (4)
        Add("Jamalpur", "জামালপুর", 8);
        Add("Mymensingh", "ময়মনসিংহ", 8);
        Add("Netrokona", "নেত্রকোণা", 8);
        Add("Sherpur", "শেরপুর", 8);

        return list;
    }

    private static List<Upazila> GetUpazilas()
    {
        var list = new List<Upazila>();
        int id = 1;

        void Add(string name, string nameBn, int districtId)
        {
            list.Add(new Upazila { Id = id++, Name = name, NameBn = nameBn, DistrictId = districtId });
        }

        // Cox's Bazar (District Id = 19)
        Add("Cox's Bazar Sadar", "কক্সবাজার সদর", 19);
        Add("Teknaf", "টেকনাফ", 19);
        Add("Ramu", "রামু", 19);
        Add("Ukhia", "উখিয়া", 19);
        Add("Maheshkhali", "মহেশখালী", 19);

        // Bandarban (District Id = 14)
        Add("Bandarban Sadar", "বান্দরবান সদর", 14);
        Add("Thanchi", "থানচি", 14);
        Add("Ruma", "রুমা", 14);
        Add("Lama", "লামা", 14);

        // Rangamati (District Id = 24)
        Add("Rangamati Sadar", "রাঙ্গামাটি সদর", 24);
        Add("Kaptai", "কাপ্তাই", 24);
        Add("Baghaichhari", "বাঘাইছড়ি", 24);

        // Sylhet (District Id = 46)
        Add("Sylhet Sadar", "সিলেট সদর", 46);
        Add("Jaintiapur", "জৈন্তাপুর", 46);
        Add("Companiganj", "কোম্পানীগঞ্জ", 46);
        Add("Gowainghat", "গোয়াইনঘাট", 46);

        // Moulvibazar (District Id = 45)
        Add("Sreemangal", "শ্রীমঙ্গল", 45);
        Add("Moulvibazar Sadar", "মৌলভীবাজার সদর", 45);
        Add("Kamalganj", "কমলগঞ্জ", 45);

        // Tangail (District Id = 13)
        Add("Tangail Sadar", "টাঙ্গাইল সদর", 13);
        Add("Madhupur", "মধুপুর", 13);
        Add("Ghatail", "ঘাটাইল", 13);

        // Bogura (District Id = 25)
        Add("Bogura Sadar", "বগুড়া সদর", 25);
        Add("Shibganj", "শিবগঞ্জ", 25);
        Add("Mahasthangarh (Shibganj area)", "মহাস্থানগড়", 25);

        // Rajshahi (District Id = 31)
        Add("Rajshahi Sadar", "রাজশাহী সদর", 31);
        Add("Paba", "পবা", 31);
        Add("Bagha", "বাঘা", 31);

        // Dinajpur (District Id = 56)
        Add("Dinajpur Sadar", "দিনাজপুর সদর", 56);
        Add("Kantanagar (Kaharole)", "কাহারোল", 56);

        // Khulna (District Id = 35)
        Add("Khulna Sadar", "খুলনা সদর", 35);
        Add("Dumuria", "ডুমুরিয়া", 35);
        Add("Koyra (Sundarbans)", "কয়রা", 35);

        // Jashore (District Id = 33)
        Add("Jashore Sadar", "যশোর সদর", 33);
        Add("Jhikargacha", "ঝিকরগাছা", 33);

        // Barishal (District Id = 42)
        Add("Barishal Sadar", "বরিশাল সদর", 42);
        Add("Bakerganj", "বাকেরগঞ্জ", 42);

        // Patuakhali (District Id = 44)
        Add("Patuakhali Sadar", "পটুয়াখালী সদর", 44);
        Add("Kuakata (Kalapara)", "কলাপাড়া", 44);

        return list;
    }
}