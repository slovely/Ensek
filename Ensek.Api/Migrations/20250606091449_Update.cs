using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ensek.Api.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_meter_reading_account_account_id",
                table: "meter_reading");

            migrationBuilder.DropPrimaryKey(
                name: "pk_meter_reading",
                table: "meter_reading");

            migrationBuilder.DropPrimaryKey(
                name: "pk_account",
                table: "account");

            migrationBuilder.RenameTable(
                name: "meter_reading",
                newName: "meter_readings");

            migrationBuilder.RenameTable(
                name: "account",
                newName: "accounts");

            migrationBuilder.RenameIndex(
                name: "ix_meter_reading_account_id",
                table: "meter_readings",
                newName: "ix_meter_readings_account_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_meter_readings",
                table: "meter_readings",
                column: "meter_reading_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_accounts",
                table: "accounts",
                column: "account_id");

            migrationBuilder.AddForeignKey(
                name: "fk_meter_readings_accounts_account_id",
                table: "meter_readings",
                column: "account_id",
                principalTable: "accounts",
                principalColumn: "account_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_meter_readings_accounts_account_id",
                table: "meter_readings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_meter_readings",
                table: "meter_readings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_accounts",
                table: "accounts");

            migrationBuilder.RenameTable(
                name: "meter_readings",
                newName: "meter_reading");

            migrationBuilder.RenameTable(
                name: "accounts",
                newName: "account");

            migrationBuilder.RenameIndex(
                name: "ix_meter_readings_account_id",
                table: "meter_reading",
                newName: "ix_meter_reading_account_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_meter_reading",
                table: "meter_reading",
                column: "meter_reading_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_account",
                table: "account",
                column: "account_id");

            migrationBuilder.AddForeignKey(
                name: "fk_meter_reading_account_account_id",
                table: "meter_reading",
                column: "account_id",
                principalTable: "account",
                principalColumn: "account_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
