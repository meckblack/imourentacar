using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImouRentACar.Data.Migrations
{
    public partial class modifieddriver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DOB",
                table: "Driver",
                nullable: false,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DOB",
                table: "Driver",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
