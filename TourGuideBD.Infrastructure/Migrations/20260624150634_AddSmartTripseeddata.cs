using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TourGuideBD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSmartTripseeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 2, 23.606999999999999, 89.8429, "Faridpur Bus Terminal", "ফরিদপুর বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 3, 23.9999, 90.420299999999997, "Gazipur Chowrasta Bus Terminal", "গাজীপুর চৌরাস্তা বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 4, 23.004999999999999, 89.826099999999997, "Gopalganj Bus Terminal", "গোপালগঞ্জ বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 5, 24.444900000000001, 90.776600000000002, "Kishoreganj Bus Terminal", "কিশোরগঞ্জ বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 6, 23.164100000000001, 90.199100000000001, "Madaripur Bus Terminal", "মাদারীপুর বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 7, 23.8643, 90.002099999999999, "Manikganj Bus Terminal", "মানিকগঞ্জ বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 8, 23.542200000000001, 90.530000000000001, "Munshiganj Bus Terminal", "মুন্সিগঞ্জ বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 9, 23.623799999999999, 90.498699999999999, "Narayanganj Bus Terminal", "নারায়ণগঞ্জ বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 10, 23.922499999999999, 90.715400000000002, "Narsingdi Bus Terminal", "নরসিংদী বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 11, 23.757400000000001, 89.644000000000005, "Rajbari Bus Terminal", "রাজবাড়ী বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 12, 23.243300000000001, 90.435199999999995, "Shariatpur Bus Terminal", "শরীয়তপুর বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 13, 24.251300000000001, 89.916700000000006, "Tangail Bus Terminal", "টাঙ্গাইল বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 14, 22.1953, 92.218400000000003, "Bandarban Bus Terminal", "বান্দরবান বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 15, 23.960799999999999, 91.111500000000007, "Brahmanbaria Bus Terminal", "ব্রাহ্মণবাড়িয়া বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 16, 23.2333, 90.651700000000005, "Chandpur Bus Terminal", "চাঁদপুর বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 17, 22.3569, 91.783199999999994, "Dampara Bus Terminal", "দামপাড়া বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 18, 23.460699999999999, 91.180899999999994, "Cumilla Bus Terminal", "কুমিল্লা বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 19, 21.427199999999999, 92.005799999999994, "Cox's Bazar Bus Terminal", "কক্সবাজার বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 20, 23.023499999999999, 91.396000000000001, "Feni Bus Terminal", "ফেনী বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 21, 23.119299999999999, 91.984700000000004, "Khagrachhari Bus Terminal", "খাগড়াছড়ি বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 22, 22.942499999999999, 90.841200000000001, "Lakshmipur Bus Terminal", "লক্ষ্মীপুর বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 23, 22.869599999999998, 91.099599999999995, "Noakhali Bus Terminal", "নোয়াখালী বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 24, 22.645199999999999, 92.170299999999997, "Rangamati Bus Terminal", "রাঙ্গামাটি বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 25, 24.846499999999999, 89.377300000000005, "Bogura Bus Terminal", "বগুড়া বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 26, 25.102499999999999, 89.022599999999997, "Joypurhat Bus Terminal", "জয়পুরহাট বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 27, 24.804400000000001, 88.935599999999994, "Naogaon Bus Terminal", "নওগাঁ বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 28, 24.420400000000001, 88.994500000000002, "Natore Bus Terminal", "নাটোর বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 29, 24.596499999999999, 88.277500000000003, "Chapainawabganj Bus Terminal", "চাঁপাইনবাবগঞ্জ বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 30, 24.006399999999999, 89.237200000000001, "Pabna Bus Terminal", "পাবনা বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 31, 24.363600000000002, 88.624099999999999, "Rajshahi Bus Terminal", "রাজশাহী বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 32, 24.4543, 89.700599999999994, "Sirajganj Bus Terminal", "সিরাজগঞ্জ বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 33, 22.6602, 89.785399999999996, "Bagerhat Bus Terminal", "বাগেরহাট বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 34, 23.6401, 88.841499999999996, "Chuadanga Bus Terminal", "চুয়াডাঙ্গা বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 35, 23.166399999999999, 89.207999999999998, "Jashore Bus Terminal", "যশোর বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 36, 23.544899999999998, 89.154300000000006, "Jhenaidah Bus Terminal", "ঝিনাইদহ বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 37, 22.845600000000001, 89.540300000000002, "Sonadanga Bus Terminal", "সোনাডাঙ্গা বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 38, 23.901199999999999, 89.120199999999997, "Kushtia Bus Terminal", "কুষ্টিয়া বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 39, 23.4876, 89.419700000000006, "Magura Bus Terminal", "মাগুরা বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 40, 23.7622, 88.631799999999998, "Meherpur Bus Terminal", "মেহেরপুর বাস টার্মিনাল" });

            migrationBuilder.InsertData(
                table: "BusStands",
                columns: new[] { "Id", "DistrictId", "IsMainStand", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[,]
                {
                    { 43, 41, true, 23.173100000000002, 89.512200000000007, "Narail Bus Terminal", "নড়াইল বাস টার্মিনাল" },
                    { 44, 42, true, 22.718499999999999, 89.070499999999996, "Satkhira Bus Terminal", "সাতক্ষীরা বাস টার্মিনাল" },
                    { 45, 43, true, 22.149999999999999, 90.111800000000002, "Barguna Bus Terminal", "বরগুনা বাস টার্মিনাল" },
                    { 46, 44, true, 22.701000000000001, 90.353499999999997, "Natullabad Bus Terminal", "নথুল্লাবাদ বাস টার্মিনাল" },
                    { 47, 45, true, 22.6858, 90.648200000000003, "Bhola Bus Terminal", "ভোলা বাস টার্মিনাল" },
                    { 48, 46, true, 22.640599999999999, 90.198800000000006, "Jhalokati Bus Terminal", "ঝালকাঠি বাস টার্মিনাল" },
                    { 49, 47, true, 22.3596, 90.329800000000006, "Patuakhali Bus Terminal", "পটুয়াখালী বাস টার্মিনাল" },
                    { 50, 48, true, 22.584099999999999, 89.975399999999993, "Pirojpur Bus Terminal", "পিরোজপুর বাস টার্মিনাল" },
                    { 51, 49, true, 24.374500000000001, 91.415800000000004, "Habiganj Bus Terminal", "হবিগঞ্জ বাস টার্মিনাল" },
                    { 52, 50, true, 24.482600000000001, 91.7774, "Moulvibazar Bus Terminal", "মৌলভীবাজার বাস টার্মিনাল" },
                    { 53, 51, true, 24.880700000000001, 91.396199999999993, "Sunamganj Bus Terminal", "সুনামগঞ্জ বাস টার্মিনাল" },
                    { 54, 52, true, 24.8949, 91.868700000000004, "Sylhet Kadamtali Bus Terminal", "সিলেট কদমতলী বাস টার্মিনাল" },
                    { 55, 53, true, 25.6279, 88.633799999999994, "Dinajpur Bus Terminal", "দিনাজপুর বাস টার্মিনাল" },
                    { 56, 54, true, 25.328800000000001, 89.528800000000004, "Gaibandha Bus Terminal", "গাইবান্ধা বাস টার্মিনাল" },
                    { 57, 55, true, 25.807400000000001, 89.635999999999996, "Kurigram Bus Terminal", "কুড়িগ্রাম বাস টার্মিনাল" },
                    { 58, 56, true, 25.921800000000001, 89.450699999999998, "Lalmonirhat Bus Terminal", "লালমনিরহাট বাস টার্মিনাল" },
                    { 59, 57, true, 25.931799999999999, 88.855999999999995, "Nilphamari Bus Terminal", "নীলফামারী বাস টার্মিনাল" },
                    { 60, 58, true, 26.341100000000001, 88.554100000000005, "Panchagarh Bus Terminal", "পঞ্চগড় বাস টার্মিনাল" },
                    { 61, 59, true, 25.7439, 89.275199999999998, "Rangpur Modern Bus Terminal", "রংপুর মডার্ন বাস টার্মিনাল" },
                    { 62, 60, true, 26.0336, 88.461600000000004, "Thakurgaon Bus Terminal", "ঠাকুরগাঁও বাস টার্মিনাল" },
                    { 63, 61, true, 24.9375, 89.936999999999998, "Jamalpur Bus Terminal", "জামালপুর বাস টার্মিনাল" },
                    { 64, 62, true, 24.7471, 90.420299999999997, "Mymensingh Bus Terminal", "ময়মনসিংহ বাস টার্মিনাল" },
                    { 65, 63, true, 24.869599999999998, 90.727000000000004, "Netrokona Bus Terminal", "নেত্রকোণা বাস টার্মিনাল" },
                    { 66, 64, true, 25.020399999999999, 90.015199999999993, "Sherpur Bus Terminal", "শেরপুর বাস টার্মিনাল" }
                });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 225m, 140, 2 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 85m, 53, 3 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 410m, 253, 4 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 205m, 126, 5 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 280m, 173, 6 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 160m, 100, 7 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 75m, 46, 8 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 45m, 26, 9 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 125m, 76, 10 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 245m, 153, 11 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 205m, 126, 12 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 185m, 113, 13 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 845m, 522, 14 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 290m, 180, 1, 15 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 245m, 153, 1, 16 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 525m, 326, 1, 17 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 205m, 126, 1, 18 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 890m, 552, 1, 19 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 335m, 206, 1, 20 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 665m, 413, 1, 21 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 300m, 186, 1, 22 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 365m, 226, 1, 23 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 750m, 466, 1, 24 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 425m, 262, 1, 25 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 560m, 346, 1, 26 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 615m, 381, 1, 27 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 460m, 284, 1, 28 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId" },
                values: new object[] { 730m, 453, 1 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 370m, 229, 1, 30 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 545m, 338, 1, 31 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 285m, 177, 1, 32 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 610m, 377, 1, 33 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 600m, 373, 1, 34 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 580m, 360, 1, 35 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 530m, 328, 1, 36 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 715m, 444, 1, 37 });

            migrationBuilder.InsertData(
                table: "DistrictBusRoutes",
                columns: new[] { "Id", "BusCost", "BusTimeMinutes", "FromDistrictId", "IsBidirectional", "ToDistrictId" },
                values: new object[,]
                {
                    { 37, 450m, 280, 1, true, 38 },
                    { 38, 460m, 286, 1, true, 39 },
                    { 39, 665m, 413, 1, true, 40 },
                    { 40, 540m, 333, 1, true, 41 },
                    { 41, 735m, 456, 1, true, 42 },
                    { 42, 665m, 413, 1, true, 43 },
                    { 43, 405m, 250, 1, true, 44 },
                    { 44, 410m, 253, 1, true, 45 },
                    { 45, 440m, 273, 1, true, 46 },
                    { 46, 525m, 326, 1, true, 47 },
                    { 47, 475m, 293, 1, true, 48 },
                    { 48, 355m, 220, 1, true, 49 },
                    { 49, 450m, 280, 1, true, 50 },
                    { 50, 535m, 332, 1, true, 51 },
                    { 51, 515m, 320, 1, true, 52 },
                    { 52, 975m, 604, 1, true, 53 },
                    { 53, 615m, 382, 1, true, 54 },
                    { 54, 795m, 493, 1, true, 55 },
                    { 55, 815m, 506, 1, true, 56 },
                    { 56, 905m, 560, 1, true, 57 },
                    { 57, 1095m, 680, 1, true, 58 },
                    { 58, 690m, 426, 1, true, 59 },
                    { 59, 1030m, 640, 1, true, 60 },
                    { 60, 320m, 200, 1, true, 61 },
                    { 61, 255m, 157, 1, true, 62 },
                    { 62, 335m, 206, 1, true, 63 },
                    { 63, 410m, 253, 1, true, 64 },
                    { 64, 200m, 122, 17, true, 14 },
                    { 65, 300m, 186, 17, true, 15 },
                    { 66, 280m, 173, 17, true, 16 },
                    { 67, 215m, 133, 17, true, 18 },
                    { 68, 330m, 204, 17, true, 19 },
                    { 69, 245m, 150, 17, true, 20 },
                    { 70, 235m, 146, 17, true, 21 },
                    { 71, 255m, 157, 17, true, 22 },
                    { 72, 285m, 177, 17, true, 23 },
                    { 73, 165m, 102, 17, true, 24 },
                    { 74, 710m, 440, 17, true, 52 },
                    { 75, 170m, 106, 19, true, 14 },
                    { 76, 200m, 124, 52, true, 49 },
                    { 77, 130m, 80, 52, true, 50 },
                    { 78, 205m, 126, 52, true, 51 },
                    { 79, 510m, 317, 52, true, 62 },
                    { 80, 290m, 180, 52, true, 15 },
                    { 81, 190m, 118, 31, true, 25 },
                    { 82, 310m, 193, 31, true, 26 },
                    { 83, 185m, 114, 31, true, 27 },
                    { 84, 85m, 53, 31, true, 28 },
                    { 85, 95m, 60, 31, true, 29 },
                    { 86, 260m, 160, 31, true, 30 },
                    { 87, 290m, 180, 31, true, 32 },
                    { 88, 395m, 244, 31, true, 37 },
                    { 89, 320m, 200, 31, true, 59 },
                    { 90, 430m, 266, 31, true, 53 },
                    { 91, 75m, 46, 37, true, 33 },
                    { 92, 275m, 170, 37, true, 34 },
                    { 93, 140m, 86, 37, true, 35 },
                    { 94, 215m, 133, 37, true, 36 },
                    { 95, 375m, 233, 37, true, 38 },
                    { 96, 235m, 146, 37, true, 39 },
                    { 97, 415m, 256, 37, true, 40 },
                    { 98, 155m, 97, 37, true, 41 },
                    { 99, 180m, 110, 37, true, 42 },
                    { 100, 260m, 162, 37, true, 44 },
                    { 101, 215m, 132, 44, true, 43 },
                    { 102, 140m, 86, 44, true, 45 },
                    { 103, 65m, 40, 44, true, 46 },
                    { 104, 170m, 106, 44, true, 47 },
                    { 105, 125m, 76, 44, true, 48 },
                    { 106, 215m, 133, 59, true, 53 },
                    { 107, 140m, 86, 59, true, 54 },
                    { 108, 245m, 153, 59, true, 55 },
                    { 109, 195m, 120, 59, true, 56 },
                    { 110, 120m, 73, 59, true, 57 },
                    { 111, 365m, 226, 59, true, 58 },
                    { 112, 300m, 186, 59, true, 60 },
                    { 113, 260m, 160, 59, true, 25 },
                    { 114, 120m, 73, 25, true, 26 },
                    { 115, 225m, 140, 25, true, 27 },
                    { 116, 205m, 126, 25, true, 28 },
                    { 117, 225m, 140, 25, true, 30 },
                    { 118, 120m, 73, 25, true, 32 },
                    { 119, 235m, 146, 25, true, 13 },
                    { 120, 280m, 173, 25, true, 53 },
                    { 121, 120m, 73, 62, true, 61 },
                    { 122, 110m, 66, 62, true, 63 },
                    { 123, 110m, 69, 62, true, 64 },
                    { 124, 215m, 133, 62, true, 5 },
                    { 125, 135m, 84, 62, true, 13 },
                    { 126, 120m, 73, 18, true, 15 },
                    { 127, 120m, 73, 18, true, 16 },
                    { 128, 150m, 93, 18, true, 20 },
                    { 129, 170m, 106, 18, true, 22 },
                    { 130, 195m, 120, 18, true, 23 },
                    { 131, 160m, 100, 35, true, 34 },
                    { 132, 130m, 80, 35, true, 36 },
                    { 133, 235m, 146, 35, true, 38 },
                    { 134, 120m, 73, 35, true, 39 },
                    { 135, 215m, 133, 35, true, 40 },
                    { 136, 170m, 106, 35, true, 41 },
                    { 137, 195m, 120, 35, true, 42 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 137);

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 51, 24.7471, 90.420299999999997, "Mymensingh Bus Terminal", "ময়মনসিংহ বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 13, 24.251300000000001, 89.916700000000006, "Tangail Bus Terminal", "টাঙ্গাইল বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 3, 23.9999, 90.420299999999997, "Gazipur Bus Terminal", "গাজীপুর বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 9, 23.623799999999999, 90.498699999999999, "Narayanganj Bus Terminal", "নারায়ণগঞ্জ বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 17, 22.3569, 91.783199999999994, "Chattogram Bus Terminal", "চট্টগ্রাম বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 19, 21.427199999999999, 92.005799999999994, "Cox's Bazar Bus Terminal", "কক্সবাজার বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 14, 22.1953, 92.218400000000003, "Bandarban Bus Terminal", "বান্দরবান বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 24, 22.645199999999999, 92.170299999999997, "Rangamati Bus Terminal", "রাঙ্গামাটি বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 21, 23.119299999999999, 91.984700000000004, "Khagrachhari Bus Terminal", "খাগড়াছড়ি বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 18, 23.460699999999999, 91.180899999999994, "Cumilla Bus Terminal", "কুমিল্লা বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 15, 23.960799999999999, 91.111500000000007, "Brahmanbaria Bus Terminal", "ব্রাহ্মণবাড়িয়া বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 23, 22.869599999999998, 91.099599999999995, "Noakhali Bus Terminal", "নোয়াখালী বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 20, 23.023499999999999, 91.396000000000001, "Feni Bus Terminal", "ফেনী বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 46, 24.8949, 91.868700000000004, "Sylhet Bus Terminal", "সিলেট বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 45, 24.482600000000001, 91.7774, "Moulvibazar Bus Terminal", "মৌলভীবাজার বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 44, 24.374500000000001, 91.415800000000004, "Habiganj Bus Terminal", "হবিগঞ্জ বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 47, 24.880700000000001, 91.396199999999993, "Sunamganj Bus Terminal", "সুনামগঞ্জ বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 31, 24.363600000000002, 88.624099999999999, "Rajshahi Bus Terminal", "রাজশাহী বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 25, 24.846499999999999, 89.377300000000005, "Bogura Bus Terminal", "বগুড়া বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 29, 24.006399999999999, 89.237200000000001, "Pabna Bus Terminal", "পাবনা বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 32, 24.4543, 89.700599999999994, "Sirajganj Bus Terminal", "সিরাজগঞ্জ বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 27, 24.804400000000001, 88.935599999999994, "Naogaon Bus Terminal", "নওগাঁ বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 35, 22.845600000000001, 89.540300000000002, "Khulna Bus Terminal", "খুলনা বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 33, 23.166399999999999, 89.207999999999998, "Jashore Bus Terminal", "যশোর বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 33, 22.6602, 89.785399999999996, "Bagerhat Bus Terminal", "বাগেরহাট বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 41, 22.718499999999999, 89.070499999999996, "Satkhira Bus Terminal", "সাতক্ষীরা বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 37, 23.901199999999999, 89.120199999999997, "Kushtia Bus Terminal", "কুষ্টিয়া বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 42, 22.701000000000001, 90.353499999999997, "Barishal Bus Terminal", "বরিশাল বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 44, 22.3596, 90.329800000000006, "Patuakhali Bus Terminal", "পটুয়াখালী বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 43, 22.6858, 90.648200000000003, "Bhola Bus Terminal", "ভোলা বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 61, 25.7439, 89.275199999999998, "Rangpur Bus Terminal", "রংপুর বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 56, 25.6279, 88.633799999999994, "Dinajpur Bus Terminal", "দিনাজপুর বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 57, 25.328800000000001, 89.528800000000004, "Gaibandha Bus Terminal", "গাইবান্ধা বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 58, 25.807400000000001, 89.635999999999996, "Kurigram Bus Terminal", "কুড়িগ্রাম বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 59, 25.931799999999999, 88.855999999999995, "Nilphamari Bus Terminal", "নীলফামারী বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 60, 26.341100000000001, 88.554100000000005, "Panchagarh Bus Terminal", "পঞ্চগড় বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 49, 24.9375, 89.936999999999998, "Jamalpur Bus Terminal", "জামালপুর বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 52, 24.869599999999998, 90.727000000000004, "Netrokona Bus Terminal", "নেত্রকোণা বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "BusStands",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "DistrictId", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[] { 53, 25.020399999999999, 90.015199999999993, "Sherpur Bus Terminal", "শেরপুর বাস টার্মিনাল" });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 800m, 360, 17 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 1050m, 480, 19 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 500m, 300, 46 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 150m, 120, 51 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 450m, 240, 31 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 400m, 300, 35 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 350m, 240, 42 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 600m, 360, 61 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 400m, 240, 25 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 150m, 120, 13 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 350m, 180, 18 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 500m, 360, 33 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "BusCost", "BusTimeMinutes", "ToDistrictId" },
                values: new object[] { 700m, 420, 56 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 200m, 120, 17, 19 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 250m, 150, 17, 14 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 200m, 120, 17, 24 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 150m, 90, 17, 21 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 200m, 120, 17, 18 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 400m, 240, 17, 46 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 600m, 300, 46, 51 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 100m, 60, 46, 45 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 150m, 90, 46, 44 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 100m, 60, 46, 47 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 600m, 300, 51, 46 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 100m, 60, 51, 49 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 150m, 90, 31, 25 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 300m, 180, 31, 35 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId" },
                values: new object[] { 100m, 60, 31 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 200m, 120, 35, 42 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 100m, 60, 35, 33 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 150m, 90, 35, 41 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 150m, 90, 61, 56 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 200m, 120, 61, 25 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 100m, 60, 61, 57 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 100m, 60, 25, 32 });

            migrationBuilder.UpdateData(
                table: "DistrictBusRoutes",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "BusCost", "BusTimeMinutes", "FromDistrictId", "ToDistrictId" },
                values: new object[] { 200m, 120, 25, 13 });
        }
    }
}
