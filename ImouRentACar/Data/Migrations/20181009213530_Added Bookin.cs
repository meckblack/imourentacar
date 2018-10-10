using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImouRentACar.Data.Migrations
{
    public partial class AddedBookin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Prices_PriceId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_PassengersInformation_Booking_BookingId",
                table: "PassengersInformation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Booking",
                table: "Booking");

            migrationBuilder.RenameTable(
                name: "Booking",
                newName: "Bookings");

            migrationBuilder.RenameColumn(
                name: "pickUpLGAId",
                table: "Bookings",
                newName: "PickUpLgaId");

            migrationBuilder.RenameColumn(
                name: "dropOffLGAId",
                table: "Bookings",
                newName: "ReturnLgaId");

            migrationBuilder.RenameColumn(
                name: "DropOffLocation",
                table: "Bookings",
                newName: "ReturnLocation");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_PriceId",
                table: "Bookings",
                newName: "IX_Bookings_PriceId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "Bookings",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PickUpDate",
                table: "Bookings",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Prices_PriceId",
                table: "Bookings",
                column: "PriceId",
                principalTable: "Prices",
                principalColumn: "PriceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PassengersInformation_Bookings_BookingId",
                table: "PassengersInformation",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "BookingId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Prices_PriceId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_PassengersInformation_Bookings_BookingId",
                table: "PassengersInformation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.RenameTable(
                name: "Bookings",
                newName: "Booking");

            migrationBuilder.RenameColumn(
                name: "PickUpLgaId",
                table: "Booking",
                newName: "pickUpLGAId");

            migrationBuilder.RenameColumn(
                name: "ReturnLocation",
                table: "Booking",
                newName: "DropOffLocation");

            migrationBuilder.RenameColumn(
                name: "ReturnLgaId",
                table: "Booking",
                newName: "dropOffLGAId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_PriceId",
                table: "Booking",
                newName: "IX_Booking_PriceId");

            migrationBuilder.AlterColumn<string>(
                name: "ReturnDate",
                table: "Booking",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "PickUpDate",
                table: "Booking",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Booking",
                table: "Booking",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Prices_PriceId",
                table: "Booking",
                column: "PriceId",
                principalTable: "Prices",
                principalColumn: "PriceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PassengersInformation_Booking_BookingId",
                table: "PassengersInformation",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "BookingId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
