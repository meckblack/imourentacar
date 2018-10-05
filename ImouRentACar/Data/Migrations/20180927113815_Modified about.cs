using Microsoft.EntityFrameworkCore.Migrations;

namespace ImouRentACar.Data.Migrations
{
    public partial class Modifiedabout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "AboutUsImageTwos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Heading",
                table: "AboutUsImageTwos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "AboutUsImages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Heading",
                table: "AboutUsImages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Body",
                table: "AboutUsImageTwos");

            migrationBuilder.DropColumn(
                name: "Heading",
                table: "AboutUsImageTwos");

            migrationBuilder.DropColumn(
                name: "Body",
                table: "AboutUsImages");

            migrationBuilder.DropColumn(
                name: "Heading",
                table: "AboutUsImages");
        }
    }
}
