using Microsoft.EntityFrameworkCore.Migrations;

namespace ImouRentACar.Data.Migrations
{
    public partial class ModifyReservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PriceId",
                table: "Reservations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_PriceId",
                table: "Reservations",
                column: "PriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Prices_PriceId",
                table: "Reservations",
                column: "PriceId",
                principalTable: "Prices",
                principalColumn: "PriceId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Prices_PriceId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_PriceId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "PriceId",
                table: "Reservations");
        }
    }
}
