using Microsoft.EntityFrameworkCore.Migrations;

namespace ImouRentACar.Migrations
{
    public partial class ModidifiedRole3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanManageCarBrand",
                table: "Roles",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanManageCarBrand",
                table: "Roles");
        }
    }
}
