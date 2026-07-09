using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourGuideBD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWithdrawalAndPaymentMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GuidePaymentMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuideProfileId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AccountName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BranchName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RoutingNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuidePaymentMethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuidePaymentMethods_GuideProfiles_GuideProfileId",
                        column: x => x.GuideProfileId,
                        principalTable: "GuideProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WithdrawalRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuideProfileId = table.Column<int>(type: "int", nullable: false),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: false),
                    RequestedAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ProcessingFee = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AdminNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProcessedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TransactionReference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawalRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WithdrawalRequests_GuidePaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "GuidePaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WithdrawalRequests_GuideProfiles_GuideProfileId",
                        column: x => x.GuideProfileId,
                        principalTable: "GuideProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuidePaymentMethods_GuideProfileId",
                table: "GuidePaymentMethods",
                column: "GuideProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawalRequests_GuideProfileId",
                table: "WithdrawalRequests",
                column: "GuideProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawalRequests_PaymentMethodId",
                table: "WithdrawalRequests",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawalRequests_Status",
                table: "WithdrawalRequests",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WithdrawalRequests");

            migrationBuilder.DropTable(
                name: "GuidePaymentMethods");
        }
    }
}
