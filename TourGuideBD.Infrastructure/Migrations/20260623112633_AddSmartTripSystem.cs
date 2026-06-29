using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TourGuideBD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSmartTripSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusStands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameBn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    IsMainStand = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusStands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusStands_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DistrictBusRoutes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromDistrictId = table.Column<int>(type: "int", nullable: false),
                    ToDistrictId = table.Column<int>(type: "int", nullable: false),
                    BusCost = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    BusTimeMinutes = table.Column<int>(type: "int", nullable: false),
                    IsBidirectional = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistrictBusRoutes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DistrictBusRoutes_Districts_FromDistrictId",
                        column: x => x.FromDistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DistrictBusRoutes_Districts_ToDistrictId",
                        column: x => x.ToDistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "BusStands",
                columns: new[] { "Id", "DistrictId", "IsMainStand", "Latitude", "Longitude", "Name", "NameBn" },
                values: new object[,]
                {
                    { 1, 1, true, 23.719100000000001, 90.428299999999993, "Saidabad Bus Terminal", "সায়েদাবাদ বাস টার্মিনাল" },
                    { 2, 1, true, 23.780799999999999, 90.403499999999994, "Mohakhali Bus Terminal", "মহাখালী বাস টার্মিনাল" },
                    { 3, 1, true, 23.773700000000002, 90.348100000000002, "Gabtoli Bus Terminal", "গাবতলী বাস টার্মিনাল" },
                    { 4, 51, true, 24.7471, 90.420299999999997, "Mymensingh Bus Terminal", "ময়মনসিংহ বাস টার্মিনাল" },
                    { 5, 13, true, 24.251300000000001, 89.916700000000006, "Tangail Bus Terminal", "টাঙ্গাইল বাস টার্মিনাল" },
                    { 6, 3, true, 23.9999, 90.420299999999997, "Gazipur Bus Terminal", "গাজীপুর বাস টার্মিনাল" },
                    { 7, 9, true, 23.623799999999999, 90.498699999999999, "Narayanganj Bus Terminal", "নারায়ণগঞ্জ বাস টার্মিনাল" },
                    { 8, 17, true, 22.3569, 91.783199999999994, "Chattogram Bus Terminal", "চট্টগ্রাম বাস টার্মিনাল" },
                    { 9, 19, true, 21.427199999999999, 92.005799999999994, "Cox's Bazar Bus Terminal", "কক্সবাজার বাস টার্মিনাল" },
                    { 10, 14, true, 22.1953, 92.218400000000003, "Bandarban Bus Terminal", "বান্দরবান বাস টার্মিনাল" },
                    { 11, 24, true, 22.645199999999999, 92.170299999999997, "Rangamati Bus Terminal", "রাঙ্গামাটি বাস টার্মিনাল" },
                    { 12, 21, true, 23.119299999999999, 91.984700000000004, "Khagrachhari Bus Terminal", "খাগড়াছড়ি বাস টার্মিনাল" },
                    { 13, 18, true, 23.460699999999999, 91.180899999999994, "Cumilla Bus Terminal", "কুমিল্লা বাস টার্মিনাল" },
                    { 14, 15, true, 23.960799999999999, 91.111500000000007, "Brahmanbaria Bus Terminal", "ব্রাহ্মণবাড়িয়া বাস টার্মিনাল" },
                    { 15, 23, true, 22.869599999999998, 91.099599999999995, "Noakhali Bus Terminal", "নোয়াখালী বাস টার্মিনাল" },
                    { 16, 20, true, 23.023499999999999, 91.396000000000001, "Feni Bus Terminal", "ফেনী বাস টার্মিনাল" },
                    { 17, 46, true, 24.8949, 91.868700000000004, "Sylhet Bus Terminal", "সিলেট বাস টার্মিনাল" },
                    { 18, 45, true, 24.482600000000001, 91.7774, "Moulvibazar Bus Terminal", "মৌলভীবাজার বাস টার্মিনাল" },
                    { 19, 44, true, 24.374500000000001, 91.415800000000004, "Habiganj Bus Terminal", "হবিগঞ্জ বাস টার্মিনাল" },
                    { 20, 47, true, 24.880700000000001, 91.396199999999993, "Sunamganj Bus Terminal", "সুনামগঞ্জ বাস টার্মিনাল" },
                    { 21, 31, true, 24.363600000000002, 88.624099999999999, "Rajshahi Bus Terminal", "রাজশাহী বাস টার্মিনাল" },
                    { 22, 25, true, 24.846499999999999, 89.377300000000005, "Bogura Bus Terminal", "বগুড়া বাস টার্মিনাল" },
                    { 23, 29, true, 24.006399999999999, 89.237200000000001, "Pabna Bus Terminal", "পাবনা বাস টার্মিনাল" },
                    { 24, 32, true, 24.4543, 89.700599999999994, "Sirajganj Bus Terminal", "সিরাজগঞ্জ বাস টার্মিনাল" },
                    { 25, 27, true, 24.804400000000001, 88.935599999999994, "Naogaon Bus Terminal", "নওগাঁ বাস টার্মিনাল" },
                    { 26, 35, true, 22.845600000000001, 89.540300000000002, "Khulna Bus Terminal", "খুলনা বাস টার্মিনাল" },
                    { 27, 33, true, 23.166399999999999, 89.207999999999998, "Jashore Bus Terminal", "যশোর বাস টার্মিনাল" },
                    { 28, 33, true, 22.6602, 89.785399999999996, "Bagerhat Bus Terminal", "বাগেরহাট বাস টার্মিনাল" },
                    { 29, 41, true, 22.718499999999999, 89.070499999999996, "Satkhira Bus Terminal", "সাতক্ষীরা বাস টার্মিনাল" },
                    { 30, 37, true, 23.901199999999999, 89.120199999999997, "Kushtia Bus Terminal", "কুষ্টিয়া বাস টার্মিনাল" },
                    { 31, 42, true, 22.701000000000001, 90.353499999999997, "Barishal Bus Terminal", "বরিশাল বাস টার্মিনাল" },
                    { 32, 44, true, 22.3596, 90.329800000000006, "Patuakhali Bus Terminal", "পটুয়াখালী বাস টার্মিনাল" },
                    { 33, 43, true, 22.6858, 90.648200000000003, "Bhola Bus Terminal", "ভোলা বাস টার্মিনাল" },
                    { 34, 61, true, 25.7439, 89.275199999999998, "Rangpur Bus Terminal", "রংপুর বাস টার্মিনাল" },
                    { 35, 56, true, 25.6279, 88.633799999999994, "Dinajpur Bus Terminal", "দিনাজপুর বাস টার্মিনাল" },
                    { 36, 57, true, 25.328800000000001, 89.528800000000004, "Gaibandha Bus Terminal", "গাইবান্ধা বাস টার্মিনাল" },
                    { 37, 58, true, 25.807400000000001, 89.635999999999996, "Kurigram Bus Terminal", "কুড়িগ্রাম বাস টার্মিনাল" },
                    { 38, 59, true, 25.931799999999999, 88.855999999999995, "Nilphamari Bus Terminal", "নীলফামারী বাস টার্মিনাল" },
                    { 39, 60, true, 26.341100000000001, 88.554100000000005, "Panchagarh Bus Terminal", "পঞ্চগড় বাস টার্মিনাল" },
                    { 40, 49, true, 24.9375, 89.936999999999998, "Jamalpur Bus Terminal", "জামালপুর বাস টার্মিনাল" },
                    { 41, 52, true, 24.869599999999998, 90.727000000000004, "Netrokona Bus Terminal", "নেত্রকোণা বাস টার্মিনাল" },
                    { 42, 53, true, 25.020399999999999, 90.015199999999993, "Sherpur Bus Terminal", "শেরপুর বাস টার্মিনাল" }
                });

            migrationBuilder.InsertData(
                table: "DistrictBusRoutes",
                columns: new[] { "Id", "BusCost", "BusTimeMinutes", "FromDistrictId", "IsBidirectional", "ToDistrictId" },
                values: new object[,]
                {
                    { 1, 800m, 360, 1, true, 17 },
                    { 2, 1050m, 480, 1, true, 19 },
                    { 3, 500m, 300, 1, true, 46 },
                    { 4, 150m, 120, 1, true, 51 },
                    { 5, 450m, 240, 1, true, 31 },
                    { 6, 400m, 300, 1, true, 35 },
                    { 7, 350m, 240, 1, true, 42 },
                    { 8, 600m, 360, 1, true, 61 },
                    { 9, 400m, 240, 1, true, 25 },
                    { 10, 150m, 120, 1, true, 13 },
                    { 11, 350m, 180, 1, true, 18 },
                    { 12, 500m, 360, 1, true, 33 },
                    { 13, 700m, 420, 1, true, 56 },
                    { 14, 200m, 120, 17, true, 19 },
                    { 15, 250m, 150, 17, true, 14 },
                    { 16, 200m, 120, 17, true, 24 },
                    { 17, 150m, 90, 17, true, 21 },
                    { 18, 200m, 120, 17, true, 18 },
                    { 19, 400m, 240, 17, true, 46 },
                    { 20, 600m, 300, 46, true, 51 },
                    { 21, 100m, 60, 46, true, 45 },
                    { 22, 150m, 90, 46, true, 44 },
                    { 23, 100m, 60, 46, true, 47 },
                    { 24, 600m, 300, 51, true, 46 },
                    { 25, 100m, 60, 51, true, 49 },
                    { 26, 150m, 90, 31, true, 25 },
                    { 27, 300m, 180, 31, true, 35 },
                    { 28, 100m, 60, 31, true, 29 },
                    { 29, 200m, 120, 35, true, 42 },
                    { 30, 100m, 60, 35, true, 33 },
                    { 31, 150m, 90, 35, true, 41 },
                    { 32, 150m, 90, 61, true, 56 },
                    { 33, 200m, 120, 61, true, 25 },
                    { 34, 100m, 60, 61, true, 57 },
                    { 35, 100m, 60, 25, true, 32 },
                    { 36, 200m, 120, 25, true, 13 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusStands_DistrictId",
                table: "BusStands",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_DistrictBusRoutes_FromDistrictId_ToDistrictId",
                table: "DistrictBusRoutes",
                columns: new[] { "FromDistrictId", "ToDistrictId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DistrictBusRoutes_ToDistrictId",
                table: "DistrictBusRoutes",
                column: "ToDistrictId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusStands");

            migrationBuilder.DropTable(
                name: "DistrictBusRoutes");
        }
    }
}
