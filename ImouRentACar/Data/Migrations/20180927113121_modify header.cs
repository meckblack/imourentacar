using Microsoft.EntityFrameworkCore.Migrations;

namespace ImouRentACar.Data.Migrations
{
    public partial class modifyheader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "Headers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Heading",
                table: "Headers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Body",
                table: "Headers");

            migrationBuilder.DropColumn(
                name: "Heading",
                table: "Headers");
        }
    }
}
