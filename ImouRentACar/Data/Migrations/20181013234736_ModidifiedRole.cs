using Microsoft.EntityFrameworkCore.Migrations;

namespace ImouRentACar.Migrations
{
    public partial class ModidifiedRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanManageBookings",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanManageDrivers",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanManageEnquires",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanManageLgas",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanManagePassengerInformation",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanManagePrices",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanManageStates",
                table: "Roles",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanManageBookings",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanManageDrivers",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanManageEnquires",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanManageLgas",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanManagePassengerInformation",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanManagePrices",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanManageStates",
                table: "Roles");
        }
    }
}
