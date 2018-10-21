using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImouRentACar.Migrations
{
    public partial class RemovedBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropColumn(
                name: "CanManageCarBrand",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanManageLgas",
                table: "Roles");

            migrationBuilder.RenameColumn(
                name: "CanManageStates",
                table: "Roles",
                newName: "CanManageLocation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CanManageLocation",
                table: "Roles",
                newName: "CanManageStates");

            migrationBuilder.AddColumn<bool>(
                name: "CanManageCarBrand",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanManageLgas",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BookingNumber = table.Column<string>(nullable: true),
                    CarId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    DateDriverAssigned = table.Column<DateTime>(nullable: false),
                    DateSent = table.Column<DateTime>(nullable: false),
                    DateVerified = table.Column<DateTime>(nullable: false),
                    DriverAssignedBy = table.Column<int>(nullable: false),
                    DriverId = table.Column<int>(nullable: false),
                    PassengerInformationId = table.Column<int>(nullable: false),
                    PaymentStatus = table.Column<int>(nullable: false),
                    PickDate = table.Column<DateTime>(nullable: false),
                    PickUpLgaId = table.Column<int>(nullable: false),
                    PickUpLocation = table.Column<string>(nullable: false),
                    PickUpTime = table.Column<DateTime>(nullable: false),
                    PriceId = table.Column<int>(nullable: false),
                    ReturnLgaId = table.Column<int>(nullable: false),
                    ReturnLocation = table.Column<string>(nullable: false),
                    ReturnTime = table.Column<DateTime>(nullable: false),
                    TotalBookingPrice = table.Column<decimal>(nullable: false),
                    Verification = table.Column<int>(nullable: false),
                    VerifiedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Bookings_PassengersInformation_PassengerInformationId",
                        column: x => x.PassengerInformationId,
                        principalTable: "PassengersInformation",
                        principalColumn: "PassengerInformationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PassengerInformationId",
                table: "Bookings",
                column: "PassengerInformationId");
        }
    }
}
