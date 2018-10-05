using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImouRentACar.Data.Migrations
{
    public partial class ModifyingEnquiry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Enquiries",
                newName: "DateVerified");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSent",
                table: "Enquiries",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "VerifiedBy",
                table: "Enquiries",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateSent",
                table: "Enquiries");

            migrationBuilder.DropColumn(
                name: "VerifiedBy",
                table: "Enquiries");

            migrationBuilder.RenameColumn(
                name: "DateVerified",
                table: "Enquiries",
                newName: "Date");
        }
    }
}
