using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobileTopup.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Transaction_Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TopUpBeneficiaries_TopUpBeneficiaryBeneficiaryId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_TopUpBeneficiaryBeneficiaryId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TopUpBeneficiaryBeneficiaryId",
                table: "Transactions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TopUpBeneficiaryBeneficiaryId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TopUpBeneficiaryBeneficiaryId",
                table: "Transactions",
                column: "TopUpBeneficiaryBeneficiaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TopUpBeneficiaries_TopUpBeneficiaryBeneficiaryId",
                table: "Transactions",
                column: "TopUpBeneficiaryBeneficiaryId",
                principalTable: "TopUpBeneficiaries",
                principalColumn: "BeneficiaryId");
        }
    }
}
