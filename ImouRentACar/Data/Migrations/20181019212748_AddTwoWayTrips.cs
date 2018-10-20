using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImouRentACar.Migrations
{
    public partial class AddTwoWayTrips : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TwoWayTrips",
                columns: table => new
                {
                    TwoWayTripId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PickUpLocation = table.Column<string>(nullable: false),
                    PickDate = table.Column<DateTime>(nullable: false),
                    PickUpTime = table.Column<DateTime>(nullable: false),
                    Destination = table.Column<string>(nullable: false),
                    ReturnTripDate = table.Column<DateTime>(nullable: false),
                    ReturnTripTime = table.Column<DateTime>(nullable: false),
                    Verification = table.Column<int>(nullable: false),
                    DriverId = table.Column<int>(nullable: false),
                    TotalBookingPrice = table.Column<decimal>(nullable: false),
                    BookingNumber = table.Column<string>(nullable: true),
                    CustomerId = table.Column<int>(nullable: false),
                    PaymentStatus = table.Column<int>(nullable: false),
                    CarId = table.Column<int>(nullable: false),
                    PriceId = table.Column<int>(nullable: false),
                    DestinationLgaId = table.Column<int>(nullable: false),
                    PickUpLgaId = table.Column<int>(nullable: false),
                    PassengerInformationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwoWayTrips", x => x.TwoWayTripId);
                    table.ForeignKey(
                        name: "FK_TwoWayTrips_PassengersInformation_PassengerInformationId",
                        column: x => x.PassengerInformationId,
                        principalTable: "PassengersInformation",
                        principalColumn: "PassengerInformationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TwoWayTrips_PassengerInformationId",
                table: "TwoWayTrips",
                column: "PassengerInformationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TwoWayTrips");
        }
    }
}
