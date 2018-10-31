using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImouRentACar.Migrations
{
    public partial class RemovedTwoWayTrip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TwoWayTrips");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TwoWayTrips",
                columns: table => new
                {
                    TwoWayTripId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BookingNumber = table.Column<string>(nullable: true),
                    CarId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    DateDriverAssigned = table.Column<DateTime>(nullable: false),
                    DateSent = table.Column<DateTime>(nullable: false),
                    DateVerified = table.Column<DateTime>(nullable: false),
                    Destination = table.Column<string>(nullable: false),
                    DestinationLgaId = table.Column<int>(nullable: false),
                    DriverAssignedBy = table.Column<int>(nullable: false),
                    DriverId = table.Column<int>(nullable: false),
                    PassengerInformationId = table.Column<int>(nullable: false),
                    PaymentStatus = table.Column<int>(nullable: false),
                    PickDate = table.Column<DateTime>(nullable: false),
                    PickUpLgaId = table.Column<int>(nullable: false),
                    PickUpLocation = table.Column<string>(nullable: false),
                    PickUpTime = table.Column<DateTime>(nullable: false),
                    PriceId = table.Column<int>(nullable: false),
                    ReturnTripDate = table.Column<DateTime>(nullable: false),
                    ReturnTripTime = table.Column<DateTime>(nullable: false),
                    TotalBookingPrice = table.Column<decimal>(nullable: false),
                    Verification = table.Column<int>(nullable: false),
                    VerifiedBy = table.Column<int>(nullable: false)
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
    }
}
