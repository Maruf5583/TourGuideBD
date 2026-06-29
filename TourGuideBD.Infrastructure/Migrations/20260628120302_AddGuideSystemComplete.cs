using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourGuideBD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGuideSystemComplete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GuideApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ProfilePhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NidFrontPhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NidBackPhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DobCertificatePhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ExperienceYears = table.Column<int>(type: "int", nullable: false),
                    Languages = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Specialities = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CertificateUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OperatingDistrictIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AdminNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReviewedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuideApplications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuideProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ProfilePhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Languages = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Specialities = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ExperienceYears = table.Column<int>(type: "int", nullable: false),
                    OperatingDistrictIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Badge = table.Column<int>(type: "int", nullable: false),
                    AverageRating = table.Column<double>(type: "float", nullable: false),
                    TotalReviews = table.Column<int>(type: "int", nullable: false),
                    TotalToursCompleted = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    StripeAccountId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuideProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuideProfiles_GuideApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "GuideApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TourPackages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuideProfileId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    PricePerPerson = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    MaxPeople = table.Column<int>(type: "int", nullable: false),
                    DurationDays = table.Column<int>(type: "int", nullable: false),
                    IncludesFood = table.Column<bool>(type: "bit", nullable: false),
                    IncludesTransport = table.Column<bool>(type: "bit", nullable: false),
                    IncludesAccommodation = table.Column<bool>(type: "bit", nullable: false),
                    AdditionalIncludes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MeetingPoint = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    MeetingLat = table.Column<double>(type: "float", nullable: false),
                    MeetingLng = table.Column<double>(type: "float", nullable: false),
                    PlaceIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourPackages_GuideProfiles_GuideProfileId",
                        column: x => x.GuideProfileId,
                        principalTable: "GuideProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TourPackageId = table.Column<int>(type: "int", nullable: false),
                    TourDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumberOfPeople = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PlatformFee = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    GuideEarning = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StripePaymentIntentId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StripeChargeId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancellationReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_TourPackages_TourPackageId",
                        column: x => x.TourPackageId,
                        principalTable: "TourPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GuideAvailabilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourPackageId = table.Column<int>(type: "int", nullable: false),
                    AvailableDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxBookings = table.Column<int>(type: "int", nullable: false),
                    CurrentBookings = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuideAvailabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuideAvailabilities_TourPackages_TourPackageId",
                        column: x => x.TourPackageId,
                        principalTable: "TourPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuideLiveLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    GuideUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    IsSharing = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuideLiveLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuideLiveLocations_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuideRevenues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuideProfileId = table.Column<int>(type: "int", nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PlatformFee = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    GuideEarning = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PaidOutAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuideRevenues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuideRevenues_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GuideRevenues_GuideProfiles_GuideProfileId",
                        column: x => x.GuideProfileId,
                        principalTable: "GuideProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GuideReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GuideProfileId = table.Column<int>(type: "int", nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    PunctualityRating = table.Column<int>(type: "int", nullable: false),
                    KnowledgeRating = table.Column<int>(type: "int", nullable: false),
                    CommunicationRating = table.Column<int>(type: "int", nullable: false),
                    SafetyRating = table.Column<int>(type: "int", nullable: false),
                    ValueRating = table.Column<int>(type: "int", nullable: false),
                    OverallRating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuideReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuideReviews_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GuideReviews_GuideProfiles_GuideProfileId",
                        column: x => x.GuideProfileId,
                        principalTable: "GuideProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_Status",
                table: "Bookings",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_StripePaymentIntentId",
                table: "Bookings",
                column: "StripePaymentIntentId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TourPackageId",
                table: "Bookings",
                column: "TourPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GuideApplications_Status",
                table: "GuideApplications",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_GuideApplications_UserId",
                table: "GuideApplications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GuideAvailabilities_TourPackageId_AvailableDate",
                table: "GuideAvailabilities",
                columns: new[] { "TourPackageId", "AvailableDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuideLiveLocations_BookingId",
                table: "GuideLiveLocations",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuideLiveLocations_GuideUserId",
                table: "GuideLiveLocations",
                column: "GuideUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GuideProfiles_ApplicationId",
                table: "GuideProfiles",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_GuideProfiles_IsActive",
                table: "GuideProfiles",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_GuideProfiles_UserId",
                table: "GuideProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuideRevenues_BookingId",
                table: "GuideRevenues",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_GuideRevenues_GuideProfileId",
                table: "GuideRevenues",
                column: "GuideProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_GuideRevenues_Status",
                table: "GuideRevenues",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_GuideReviews_BookingId",
                table: "GuideReviews",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuideReviews_GuideProfileId",
                table: "GuideReviews",
                column: "GuideProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_TourPackages_GuideProfileId",
                table: "TourPackages",
                column: "GuideProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuideAvailabilities");

            migrationBuilder.DropTable(
                name: "GuideLiveLocations");

            migrationBuilder.DropTable(
                name: "GuideRevenues");

            migrationBuilder.DropTable(
                name: "GuideReviews");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "TourPackages");

            migrationBuilder.DropTable(
                name: "GuideProfiles");

            migrationBuilder.DropTable(
                name: "GuideApplications");
        }
    }
}
