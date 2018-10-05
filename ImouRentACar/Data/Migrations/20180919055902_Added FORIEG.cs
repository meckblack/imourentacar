using Microsoft.EntityFrameworkCore.Migrations;

namespace ImouRentACar.Data.Migrations
{
    public partial class AddedFORIEG : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarBrandId",
                table: "CarModels",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CarModels_CarBrandId",
                table: "CarModels",
                column: "CarBrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarModels_CarBrands_CarBrandId",
                table: "CarModels",
                column: "CarBrandId",
                principalTable: "CarBrands",
                principalColumn: "CarBrandId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarModels_CarBrands_CarBrandId",
                table: "CarModels");

            migrationBuilder.DropIndex(
                name: "IX_CarModels_CarBrandId",
                table: "CarModels");

            migrationBuilder.DropColumn(
                name: "CarBrandId",
                table: "CarModels");
        }
    }
}
