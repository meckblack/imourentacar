using Microsoft.EntityFrameworkCore.Migrations;

namespace ImouRentACar.Migrations
{
    public partial class ModifiedBookingaAndCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MemberId",
                table: "Customers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "BookingNumber",
                table: "Bookings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingNumber",
                table: "Bookings");

            migrationBuilder.AlterColumn<int>(
                name: "MemberId",
                table: "Customers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
