using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImouRentACar.Migrations
{
    public partial class MOdification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateDriverAssigned",
                table: "TwoWayTrips",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSent",
                table: "TwoWayTrips",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateVerified",
                table: "TwoWayTrips",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DriverAssignedBy",
                table: "TwoWayTrips",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VerifiedBy",
                table: "TwoWayTrips",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDriverAssigned",
                table: "RentACars",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSent",
                table: "RentACars",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateVerified",
                table: "RentACars",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DriverAssignedBy",
                table: "RentACars",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VerifiedBy",
                table: "RentACars",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDriverAssigned",
                table: "OneWayTrips",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSent",
                table: "OneWayTrips",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateVerified",
                table: "OneWayTrips",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DriverAssignedBy",
                table: "OneWayTrips",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VerifiedBy",
                table: "OneWayTrips",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateDriverAssigned",
                table: "TwoWayTrips");

            migrationBuilder.DropColumn(
                name: "DateSent",
                table: "TwoWayTrips");

            migrationBuilder.DropColumn(
                name: "DateVerified",
                table: "TwoWayTrips");

            migrationBuilder.DropColumn(
                name: "DriverAssignedBy",
                table: "TwoWayTrips");

            migrationBuilder.DropColumn(
                name: "VerifiedBy",
                table: "TwoWayTrips");

            migrationBuilder.DropColumn(
                name: "DateDriverAssigned",
                table: "RentACars");

            migrationBuilder.DropColumn(
                name: "DateSent",
                table: "RentACars");

            migrationBuilder.DropColumn(
                name: "DateVerified",
                table: "RentACars");

            migrationBuilder.DropColumn(
                name: "DriverAssignedBy",
                table: "RentACars");

            migrationBuilder.DropColumn(
                name: "VerifiedBy",
                table: "RentACars");

            migrationBuilder.DropColumn(
                name: "DateDriverAssigned",
                table: "OneWayTrips");

            migrationBuilder.DropColumn(
                name: "DateSent",
                table: "OneWayTrips");

            migrationBuilder.DropColumn(
                name: "DateVerified",
                table: "OneWayTrips");

            migrationBuilder.DropColumn(
                name: "DriverAssignedBy",
                table: "OneWayTrips");

            migrationBuilder.DropColumn(
                name: "VerifiedBy",
                table: "OneWayTrips");
        }
    }
}
