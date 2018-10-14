using Microsoft.EntityFrameworkCore.Migrations;

namespace ImouRentACar.Migrations
{
    public partial class ModidifiedRole2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CanManagePassengerInformation",
                table: "Roles",
                newName: "CanManagePassengersInformation");

            migrationBuilder.AddColumn<bool>(
                name: "CanManageApplicationUsers",
                table: "Roles",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanManageApplicationUsers",
                table: "Roles");

            migrationBuilder.RenameColumn(
                name: "CanManagePassengersInformation",
                table: "Roles",
                newName: "CanManagePassengerInformation");
        }
    }
}
