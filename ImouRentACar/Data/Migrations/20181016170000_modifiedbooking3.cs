using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImouRentACar.Migrations
{
    public partial class modifiedbooking3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PickUpDate",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ReturnDate",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "Returner",
                table: "Bookings",
                newName: "ReturnTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "PickUpTime",
                table: "Bookings",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PickUpTime",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "ReturnTime",
                table: "Bookings",
                newName: "Returner");

            migrationBuilder.AddColumn<string>(
                name: "PickUpDate",
                table: "Bookings",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReturnDate",
                table: "Bookings",
                nullable: false,
                defaultValue: "");
        }
    }
}
