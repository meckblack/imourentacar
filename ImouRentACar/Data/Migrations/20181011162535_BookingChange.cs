using Microsoft.EntityFrameworkCore.Migrations;

namespace ImouRentACar.Data.Migrations
{
    public partial class BookingChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Prices_PriceId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_PriceId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PriceId",
                table: "Bookings");

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prices_Cars_PickUpLgaId",
                table: "Prices");

            
        }
    }
}
