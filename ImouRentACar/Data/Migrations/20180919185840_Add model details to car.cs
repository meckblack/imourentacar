using Microsoft.EntityFrameworkCore.Migrations;

namespace ImouRentACar.Data.Migrations
{
    public partial class Addmodeldetailstocar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Color",
                table: "Cars",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Engine",
                table: "Cars",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Speed",
                table: "Cars",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Engine",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Speed",
                table: "Cars");
        }
    }
}
