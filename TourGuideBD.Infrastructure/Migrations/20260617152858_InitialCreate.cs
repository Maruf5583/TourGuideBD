using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TourGuideBD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsBanned = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentRefreshTokenId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Divisions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameBn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Divisions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlaceCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CategoryType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransportTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Itineraries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TripDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstimatedTotalCost = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    EstimatedFoodCost = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itineraries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Itineraries_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameBn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DivisionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Districts_Divisions_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "Divisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransportRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransportTypeId = table.Column<int>(type: "int", nullable: false),
                    RatePerKm = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    AverageSpeedKmh = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    MinimumFare = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportRates_TransportTypes_TransportTypeId",
                        column: x => x.TransportTypeId,
                        principalTable: "TransportTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Broadcasts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: true),
                    SentByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Broadcasts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Broadcasts_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SavedDistricts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedDistricts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedDistricts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedDistricts_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Upazilas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameBn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Upazilas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Upazilas_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WeatherAlerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistrictId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeatherAlerts_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameBn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    EntryFee = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    BestSeason = table.Column<int>(type: "int", nullable: false),
                    DivisionId = table.Column<int>(type: "int", nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: false),
                    UpazilaId = table.Column<int>(type: "int", nullable: true),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    SubmittedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AverageRating = table.Column<double>(type: "float", nullable: false),
                    TotalReviews = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Places_AspNetUsers_SubmittedByUserId",
                        column: x => x.SubmittedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Places_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Places_Divisions_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "Divisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Places_Upazilas_UpazilaId",
                        column: x => x.UpazilaId,
                        principalTable: "Upazilas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CheckIns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    CheckedInAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckIns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckIns_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckIns_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavouritePlaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavouritePlaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavouritePlaces_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavouritePlaces_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItineraryStops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItineraryId = table.Column<int>(type: "int", nullable: false),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    TransportTypeId = table.Column<int>(type: "int", nullable: false),
                    DistanceFromPreviousKm = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    TransportCost = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TravelTimeMinutes = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    EntryFeeAtThisStop = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItineraryStops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItineraryStops_Itineraries_ItineraryId",
                        column: x => x.ItineraryId,
                        principalTable: "Itineraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItineraryStops_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItineraryStops_TransportTypes_TransportTypeId",
                        column: x => x.TransportTypeId,
                        principalTable: "TransportTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LiveVisitors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    CurrentCount = table.Column<int>(type: "int", nullable: false),
                    CrowdLevel = table.Column<int>(type: "int", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveVisitors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LiveVisitors_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaceCategoryMaps",
                columns: table => new
                {
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    PlaceCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceCategoryMaps", x => new { x.PlaceId, x.PlaceCategoryId });
                    table.ForeignKey(
                        name: "FK_PlaceCategoryMaps_PlaceCategories_PlaceCategoryId",
                        column: x => x.PlaceCategoryId,
                        principalTable: "PlaceCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaceCategoryMaps_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlacePhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsCover = table.Column<bool>(type: "bit", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlacePhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlacePhotos_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaceTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaceTags_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    CommentEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CommentBn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ModeratorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModeratorNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisitHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    VisitedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitHistories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VisitHistories_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewId = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewPhotos_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewId = table.Column<int>(type: "int", nullable: false),
                    ReportedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ResolvedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ResolutionNote = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewReports_AspNetUsers_ReportedByUserId",
                        column: x => x.ReportedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReviewReports_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Divisions",
                columns: new[] { "Id", "Name", "NameBn" },
                values: new object[,]
                {
                    { 1, "Dhaka", "ঢাকা" },
                    { 2, "Chattogram", "চট্টগ্রাম" },
                    { 3, "Rajshahi", "রাজশাহী" },
                    { 4, "Khulna", "খুলনা" },
                    { 5, "Barishal", "বরিশাল" },
                    { 6, "Sylhet", "সিলেট" },
                    { 7, "Rangpur", "রংপুর" },
                    { 8, "Mymensingh", "ময়মনসিংহ" }
                });

            migrationBuilder.InsertData(
                table: "PlaceCategories",
                columns: new[] { "Id", "CategoryType", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Beach" },
                    { 2, 2, "Hill" },
                    { 3, 3, "Forest" },
                    { 4, 4, "Historical" },
                    { 5, 5, "Religious" },
                    { 6, 6, "Waterfall" }
                });

            migrationBuilder.InsertData(
                table: "TransportTypes",
                columns: new[] { "Id", "Name", "Type" },
                values: new object[,]
                {
                    { 1, "Bus", 1 },
                    { 2, "CNG", 2 },
                    { 3, "Train", 3 },
                    { 4, "Boat", 4 },
                    { 5, "Car", 5 },
                    { 6, "Bike", 6 }
                });

            migrationBuilder.InsertData(
                table: "Districts",
                columns: new[] { "Id", "DivisionId", "Name", "NameBn" },
                values: new object[,]
                {
                    { 1, 1, "Dhaka", "ঢাকা" },
                    { 2, 1, "Faridpur", "ফরিদপুর" },
                    { 3, 1, "Gazipur", "গাজীপুর" },
                    { 4, 1, "Gopalganj", "গোপালগঞ্জ" },
                    { 5, 1, "Kishoreganj", "কিশোরগঞ্জ" },
                    { 6, 1, "Madaripur", "মাদারীপুর" },
                    { 7, 1, "Manikganj", "মানিকগঞ্জ" },
                    { 8, 1, "Munshiganj", "মুন্সিগঞ্জ" },
                    { 9, 1, "Narayanganj", "নারায়ণগঞ্জ" },
                    { 10, 1, "Narsingdi", "নরসিংদী" },
                    { 11, 1, "Rajbari", "রাজবাড়ী" },
                    { 12, 1, "Shariatpur", "শরীয়তপুর" },
                    { 13, 1, "Tangail", "টাঙ্গাইল" },
                    { 14, 2, "Bandarban", "বান্দরবান" },
                    { 15, 2, "Brahmanbaria", "ব্রাহ্মণবাড়িয়া" },
                    { 16, 2, "Chandpur", "চাঁদপুর" },
                    { 17, 2, "Chattogram", "চট্টগ্রাম" },
                    { 18, 2, "Cumilla", "কুমিল্লা" },
                    { 19, 2, "Cox's Bazar", "কক্সবাজার" },
                    { 20, 2, "Feni", "ফেনী" },
                    { 21, 2, "Khagrachhari", "খাগড়াছড়ি" },
                    { 22, 2, "Lakshmipur", "লক্ষ্মীপুর" },
                    { 23, 2, "Noakhali", "নোয়াখালী" },
                    { 24, 2, "Rangamati", "রাঙ্গামাটি" },
                    { 25, 3, "Bogura", "বগুড়া" },
                    { 26, 3, "Joypurhat", "জয়পুরহাট" },
                    { 27, 3, "Naogaon", "নওগাঁ" },
                    { 28, 3, "Natore", "নাটোর" },
                    { 29, 3, "Chapainawabganj", "চাঁপাইনবাবগঞ্জ" },
                    { 30, 3, "Pabna", "পাবনা" },
                    { 31, 3, "Rajshahi", "রাজশাহী" },
                    { 32, 3, "Sirajganj", "সিরাজগঞ্জ" },
                    { 33, 4, "Bagerhat", "বাগেরহাট" },
                    { 34, 4, "Chuadanga", "চুয়াডাঙ্গা" },
                    { 35, 4, "Jashore", "যশোর" },
                    { 36, 4, "Jhenaidah", "ঝিনাইদহ" },
                    { 37, 4, "Khulna", "খুলনা" },
                    { 38, 4, "Kushtia", "কুষ্টিয়া" },
                    { 39, 4, "Magura", "মাগুরা" },
                    { 40, 4, "Meherpur", "মেহেরপুর" },
                    { 41, 4, "Narail", "নড়াইল" },
                    { 42, 4, "Satkhira", "সাতক্ষীরা" },
                    { 43, 5, "Barguna", "বরগুনা" },
                    { 44, 5, "Barishal", "বরিশাল" },
                    { 45, 5, "Bhola", "ভোলা" },
                    { 46, 5, "Jhalokati", "ঝালকাঠি" },
                    { 47, 5, "Patuakhali", "পটুয়াখালী" },
                    { 48, 5, "Pirojpur", "পিরোজপুর" },
                    { 49, 6, "Habiganj", "হবিগঞ্জ" },
                    { 50, 6, "Moulvibazar", "মৌলভীবাজার" },
                    { 51, 6, "Sunamganj", "সুনামগঞ্জ" },
                    { 52, 6, "Sylhet", "সিলেট" },
                    { 53, 7, "Dinajpur", "দিনাজপুর" },
                    { 54, 7, "Gaibandha", "গাইবান্ধা" },
                    { 55, 7, "Kurigram", "কুড়িগ্রাম" },
                    { 56, 7, "Lalmonirhat", "লালমনিরহাট" },
                    { 57, 7, "Nilphamari", "নীলফামারী" },
                    { 58, 7, "Panchagarh", "পঞ্চগড়" },
                    { 59, 7, "Rangpur", "রংপুর" },
                    { 60, 7, "Thakurgaon", "ঠাকুরগাঁও" },
                    { 61, 8, "Jamalpur", "জামালপুর" },
                    { 62, 8, "Mymensingh", "ময়মনসিংহ" },
                    { 63, 8, "Netrokona", "নেত্রকোণা" },
                    { 64, 8, "Sherpur", "শেরপুর" }
                });

            migrationBuilder.InsertData(
                table: "TransportRates",
                columns: new[] { "Id", "AverageSpeedKmh", "IsActive", "MinimumFare", "RatePerKm", "TransportTypeId" },
                values: new object[,]
                {
                    { 1, 40m, true, 20m, 1.80m, 1 },
                    { 2, 25m, true, 50m, 8.00m, 2 },
                    { 3, 50m, true, 30m, 1.20m, 3 },
                    { 4, 15m, true, 50m, 5.00m, 4 },
                    { 5, 45m, true, 100m, 15.00m, 5 },
                    { 6, 35m, true, 20m, 4.00m, 6 }
                });

            migrationBuilder.InsertData(
                table: "Upazilas",
                columns: new[] { "Id", "DistrictId", "Name", "NameBn" },
                values: new object[,]
                {
                    { 1, 19, "Cox's Bazar Sadar", "কক্সবাজার সদর" },
                    { 2, 19, "Teknaf", "টেকনাফ" },
                    { 3, 19, "Ramu", "রামু" },
                    { 4, 19, "Ukhia", "উখিয়া" },
                    { 5, 19, "Maheshkhali", "মহেশখালী" },
                    { 6, 14, "Bandarban Sadar", "বান্দরবান সদর" },
                    { 7, 14, "Thanchi", "থানচি" },
                    { 8, 14, "Ruma", "রুমা" },
                    { 9, 14, "Lama", "লামা" },
                    { 10, 24, "Rangamati Sadar", "রাঙ্গামাটি সদর" },
                    { 11, 24, "Kaptai", "কাপ্তাই" },
                    { 12, 24, "Baghaichhari", "বাঘাইছড়ি" },
                    { 13, 46, "Sylhet Sadar", "সিলেট সদর" },
                    { 14, 46, "Jaintiapur", "জৈন্তাপুর" },
                    { 15, 46, "Companiganj", "কোম্পানীগঞ্জ" },
                    { 16, 46, "Gowainghat", "গোয়াইনঘাট" },
                    { 17, 45, "Sreemangal", "শ্রীমঙ্গল" },
                    { 18, 45, "Moulvibazar Sadar", "মৌলভীবাজার সদর" },
                    { 19, 45, "Kamalganj", "কমলগঞ্জ" },
                    { 20, 13, "Tangail Sadar", "টাঙ্গাইল সদর" },
                    { 21, 13, "Madhupur", "মধুপুর" },
                    { 22, 13, "Ghatail", "ঘাটাইল" },
                    { 23, 25, "Bogura Sadar", "বগুড়া সদর" },
                    { 24, 25, "Shibganj", "শিবগঞ্জ" },
                    { 25, 25, "Mahasthangarh (Shibganj area)", "মহাস্থানগড়" },
                    { 26, 31, "Rajshahi Sadar", "রাজশাহী সদর" },
                    { 27, 31, "Paba", "পবা" },
                    { 28, 31, "Bagha", "বাঘা" },
                    { 29, 56, "Dinajpur Sadar", "দিনাজপুর সদর" },
                    { 30, 56, "Kantanagar (Kaharole)", "কাহারোল" },
                    { 31, 35, "Khulna Sadar", "খুলনা সদর" },
                    { 32, 35, "Dumuria", "ডুমুরিয়া" },
                    { 33, 35, "Koyra (Sundarbans)", "কয়রা" },
                    { 34, 33, "Jashore Sadar", "যশোর সদর" },
                    { 35, 33, "Jhikargacha", "ঝিকরগাছা" },
                    { 36, 42, "Barishal Sadar", "বরিশাল সদর" },
                    { 37, 42, "Bakerganj", "বাকেরগঞ্জ" },
                    { 38, 44, "Patuakhali Sadar", "পটুয়াখালী সদর" },
                    { 39, 44, "Kuakata (Kalapara)", "কলাপাড়া" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_Timestamp",
                table: "AuditLogs",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_Broadcasts_DistrictId",
                table: "Broadcasts",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_PlaceId",
                table: "CheckIns",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_UserId",
                table: "CheckIns",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_DivisionId",
                table: "Districts",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_Name",
                table: "Districts",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Divisions_Name",
                table: "Divisions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FavouritePlaces_PlaceId",
                table: "FavouritePlaces",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_FavouritePlaces_UserId_PlaceId",
                table: "FavouritePlaces",
                columns: new[] { "UserId", "PlaceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Itineraries_UserId",
                table: "Itineraries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryStops_ItineraryId",
                table: "ItineraryStops",
                column: "ItineraryId");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryStops_PlaceId",
                table: "ItineraryStops",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryStops_TransportTypeId",
                table: "ItineraryStops",
                column: "TransportTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveVisitors_PlaceId",
                table: "LiveVisitors",
                column: "PlaceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlaceCategories_CategoryType",
                table: "PlaceCategories",
                column: "CategoryType",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlaceCategoryMaps_PlaceCategoryId",
                table: "PlaceCategoryMaps",
                column: "PlaceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlacePhotos_PlaceId",
                table: "PlacePhotos",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Places_ApprovalStatus",
                table: "Places",
                column: "ApprovalStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Places_DistrictId",
                table: "Places",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Places_DivisionId",
                table: "Places",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Places_Latitude_Longitude",
                table: "Places",
                columns: new[] { "Latitude", "Longitude" });

            migrationBuilder.CreateIndex(
                name: "IX_Places_Name",
                table: "Places",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Places_SubmittedByUserId",
                table: "Places",
                column: "SubmittedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Places_UpazilaId",
                table: "Places",
                column: "UpazilaId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaceTags_PlaceId",
                table: "PlaceTags",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaceTags_Tag",
                table: "PlaceTags",
                column: "Tag");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewPhotos_ReviewId",
                table: "ReviewPhotos",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewReports_ReportedByUserId",
                table: "ReviewReports",
                column: "ReportedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewReports_ReviewId",
                table: "ReviewReports",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewReports_Status",
                table: "ReviewReports",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_PlaceId_Status",
                table: "Reviews",
                columns: new[] { "PlaceId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_Status",
                table: "Reviews",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedDistricts_DistrictId",
                table: "SavedDistricts",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedDistricts_UserId_DistrictId",
                table: "SavedDistricts",
                columns: new[] { "UserId", "DistrictId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransportRates_TransportTypeId",
                table: "TransportRates",
                column: "TransportTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportTypes_Type",
                table: "TransportTypes",
                column: "Type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Upazilas_DistrictId_Name",
                table: "Upazilas",
                columns: new[] { "DistrictId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VisitHistories_PlaceId",
                table: "VisitHistories",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitHistories_UserId",
                table: "VisitHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherAlerts_DistrictId_IsActive",
                table: "WeatherAlerts",
                columns: new[] { "DistrictId", "IsActive" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "Broadcasts");

            migrationBuilder.DropTable(
                name: "CheckIns");

            migrationBuilder.DropTable(
                name: "FavouritePlaces");

            migrationBuilder.DropTable(
                name: "ItineraryStops");

            migrationBuilder.DropTable(
                name: "LiveVisitors");

            migrationBuilder.DropTable(
                name: "PlaceCategoryMaps");

            migrationBuilder.DropTable(
                name: "PlacePhotos");

            migrationBuilder.DropTable(
                name: "PlaceTags");

            migrationBuilder.DropTable(
                name: "ReviewPhotos");

            migrationBuilder.DropTable(
                name: "ReviewReports");

            migrationBuilder.DropTable(
                name: "SavedDistricts");

            migrationBuilder.DropTable(
                name: "TransportRates");

            migrationBuilder.DropTable(
                name: "VisitHistories");

            migrationBuilder.DropTable(
                name: "WeatherAlerts");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Itineraries");

            migrationBuilder.DropTable(
                name: "PlaceCategories");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "TransportTypes");

            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Upazilas");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "Divisions");
        }
    }
}
