using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImouRentACar.Migrations
{
    public partial class AddRentACar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RentACars",
                columns: table => new
                {
                    RentACarId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PickUpLocation = table.Column<string>(nullable: false),
                    PickDate = table.Column<DateTime>(nullable: false),
                    PickUpTime = table.Column<DateTime>(nullable: false),
                    Days = table.Column<int>(nullable: false),
                    Verification = table.Column<int>(nullable: false),
                    DriverId = table.Column<int>(nullable: false),
                    TotalBookingPrice = table.Column<decimal>(nullable: false),
                    BookingNumber = table.Column<string>(nullable: true),
                    CustomerId = table.Column<int>(nullable: false),
                    PaymentStatus = table.Column<int>(nullable: false),
                    CarId = table.Column<int>(nullable: false),
                    PriceId = table.Column<int>(nullable: false),
                    PickUpLgaId = table.Column<int>(nullable: false),
                    PassengerInformationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentACars", x => x.RentACarId);
                    table.ForeignKey(
                        name: "FK_RentACars_PassengersInformation_PassengerInformationId",
                        column: x => x.PassengerInformationId,
                        principalTable: "PassengersInformation",
                        principalColumn: "PassengerInformationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RentACars_PassengerInformationId",
                table: "RentACars",
                column: "PassengerInformationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentACars");
        }
    }
}
