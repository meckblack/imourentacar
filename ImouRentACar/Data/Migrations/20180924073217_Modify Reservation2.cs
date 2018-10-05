using Microsoft.EntityFrameworkCore.Migrations;

namespace ImouRentACar.Data.Migrations
{
    public partial class ModifyReservation2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Destination",
                table: "Reservations",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Verification",
                table: "Reservations",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Destination",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Verification",
                table: "Reservations");
        }
    }
}
