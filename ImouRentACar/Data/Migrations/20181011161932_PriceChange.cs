using Microsoft.EntityFrameworkCore.Migrations;

namespace ImouRentACar.Data.Migrations
{
    public partial class PriceChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prices_Cars_CarId",
                table: "Prices");

            migrationBuilder.DropIndex(
                name: "IX_Prices_CarId",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Prices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Prices",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Prices_CarId",
                table: "Prices",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_Cars_CarId",
                table: "Prices",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "CarId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
