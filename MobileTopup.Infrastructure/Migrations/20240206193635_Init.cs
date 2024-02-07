using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobileTopup.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Balance = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "TopUpBeneficiaries",
                columns: table => new
                {
                    BeneficiaryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nickname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopUpBeneficiaries", x => x.BeneficiaryId);
                    table.ForeignKey(
                        name: "FK_TopUpBeneficiaries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TopUpBeneficiaryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TopUpBeneficiaryBeneficiaryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transactions_TopUpBeneficiaries_TopUpBeneficiaryBeneficiaryId",
                        column: x => x.TopUpBeneficiaryBeneficiaryId,
                        principalTable: "TopUpBeneficiaries",
                        principalColumn: "BeneficiaryId");
                    table.ForeignKey(
                        name: "FK_Transactions_TopUpBeneficiaries_TopUpBeneficiaryId",
                        column: x => x.TopUpBeneficiaryId,
                        principalTable: "TopUpBeneficiaries",
                        principalColumn: "BeneficiaryId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TopUpBeneficiaries_UserId",
                table: "TopUpBeneficiaries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TopUpBeneficiaryBeneficiaryId",
                table: "Transactions",
                column: "TopUpBeneficiaryBeneficiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TopUpBeneficiaryId",
                table: "Transactions",
                column: "TopUpBeneficiaryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "TopUpBeneficiaries");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
