using Microsoft.EntityFrameworkCore.Migrations;

namespace ImouRentACar.Data.Migrations
{
    public partial class BookingChanges3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Destination",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "PriceId",
                table: "Bookings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceId",
                table: "Bookings");

            migrationBuilder.AddColumn<string>(
                name: "Destination",
                table: "Bookings",
                nullable: false,
                defaultValue: "");
        }
    }
}
