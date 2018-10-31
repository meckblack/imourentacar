using Microsoft.EntityFrameworkCore.Migrations;

namespace ImouRentACar.Migrations
{
    public partial class RemovedPriceFromCar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Cars");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Cars",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
