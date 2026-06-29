using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourGuideBD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPlaceOpeningClosingHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClosingHours",
                table: "Places",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OpeningHours",
                table: "Places",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosingHours",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "OpeningHours",
                table: "Places");
        }
    }
}
