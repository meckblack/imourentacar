﻿// <auto-generated />
using System;
using ImouRentACar.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ImouRentACar.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20181011160832_rchange")]
    partial class rchange
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ImouRentACar.Models.AboutUsImage", b =>
                {
                    b.Property<int>("AboutUsImageId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body");

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("Heading");

                    b.Property<string>("Image");

                    b.Property<int>("LastModifiedBy");

                    b.HasKey("AboutUsImageId");

                    b.ToTable("AboutUsImages");
                });

            modelBuilder.Entity("ImouRentACar.Models.AboutUsImageTwo", b =>
                {
                    b.Property<int>("AboutUsImageTwoId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body");

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("Heading");

                    b.Property<string>("Image");

                    b.Property<int>("LastModifiedBy");

                    b.HasKey("AboutUsImageTwoId");

                    b.ToTable("AboutUsImageTwos");
                });

            modelBuilder.Entity("ImouRentACar.Models.ApplicationUser", b =>
                {
                    b.Property<int>("ApplicationUserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConfirmPassword")
                        .IsRequired();

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<int>("LastModifiedBy");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<int>("RoleId");

                    b.HasKey("ApplicationUserId");

                    b.HasIndex("RoleId");

                    b.ToTable("ApplicationUsers");
                });

            modelBuilder.Entity("ImouRentACar.Models.Booking", b =>
                {
                    b.Property<int>("BookingId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateDriverAssigned");

                    b.Property<DateTime>("DateSent");

                    b.Property<DateTime>("DateVerified");

                    b.Property<string>("Destination")
                        .IsRequired();

                    b.Property<int>("DriverAssignedBy");

                    b.Property<int>("DriverId");

                    b.Property<int>("PassengerInformationId");

                    b.Property<string>("PickUpDate")
                        .IsRequired();

                    b.Property<int>("PickUpLgaId");

                    b.Property<string>("PickUpLocation")
                        .IsRequired();

                    b.Property<int>("PriceId");

                    b.Property<string>("ReturnDate")
                        .IsRequired();

                    b.Property<int>("ReturnLgaId");

                    b.Property<string>("ReturnLocation")
                        .IsRequired();

                    b.Property<int>("Verification");

                    b.Property<int>("VerifiedBy");

                    b.HasKey("BookingId");

                    b.HasIndex("PassengerInformationId");

                    b.HasIndex("PriceId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("ImouRentACar.Models.Car", b =>
                {
                    b.Property<int>("CarId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CarAvaliability");

                    b.Property<int>("CarBrandId");

                    b.Property<int>("Color");

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Engine")
                        .IsRequired();

                    b.Property<string>("Image");

                    b.Property<int>("LastModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Price");

                    b.Property<string>("Speed")
                        .IsRequired();

                    b.HasKey("CarId");

                    b.HasIndex("CarBrandId");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("ImouRentACar.Models.CarBrand", b =>
                {
                    b.Property<int>("CarBrandId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<int>("LastModifiedBy");

                    b.Property<string>("Name");

                    b.HasKey("CarBrandId");

                    b.ToTable("CarBrands");
                });

            modelBuilder.Entity("ImouRentACar.Models.Contact", b =>
                {
                    b.Property<int>("ContactId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<string>("BoxOfficeNumber");

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<int>("LastModifiedBy");

                    b.Property<string>("MobileNumberOne")
                        .IsRequired();

                    b.Property<string>("MobileNumberTwo");

                    b.HasKey("ContactId");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("ImouRentACar.Models.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConfrimPassword")
                        .IsRequired();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<int>("MemberId");

                    b.Property<string>("MobileNumber")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.HasKey("CustomerId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("ImouRentACar.Models.Driver", b =>
                {
                    b.Property<int>("DriverId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<string>("DOB")
                        .IsRequired();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<int>("DriverAvaliablity");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<int>("LastModifiedBy");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("License")
                        .IsRequired();

                    b.Property<string>("MiddleName");

                    b.HasKey("DriverId");

                    b.ToTable("Driver");
                });

            modelBuilder.Entity("ImouRentACar.Models.Enquiry", b =>
                {
                    b.Property<int>("EnquiryId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment")
                        .IsRequired();

                    b.Property<DateTime>("DateDriverAssigned");

                    b.Property<DateTime>("DateSent");

                    b.Property<DateTime>("DateVerified");

                    b.Property<int>("DriverAssignedBy");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Organization")
                        .IsRequired();

                    b.Property<int>("Verification");

                    b.Property<int>("VerifiedBy");

                    b.HasKey("EnquiryId");

                    b.ToTable("Enquiries");
                });

            modelBuilder.Entity("ImouRentACar.Models.Header", b =>
                {
                    b.Property<int>("HeaderId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body");

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("Heading");

                    b.Property<string>("Image");

                    b.Property<int>("LastModifiedBy");

                    b.HasKey("HeaderId");

                    b.ToTable("Headers");
                });

            modelBuilder.Entity("ImouRentACar.Models.LGA", b =>
                {
                    b.Property<int>("LGAId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<int>("LastModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("StateId");

                    b.HasKey("LGAId");

                    b.HasIndex("StateId");

                    b.ToTable("Lgas");
                });

            modelBuilder.Entity("ImouRentACar.Models.Logo", b =>
                {
                    b.Property<int>("LogoId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("Image");

                    b.Property<int>("LastModifiedBy");

                    b.HasKey("LogoId");

                    b.ToTable("Logos");
                });

            modelBuilder.Entity("ImouRentACar.Models.PassengerInformation", b =>
                {
                    b.Property<int>("PassengerInformationId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DOB");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<int>("Gender");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("MemberId");

                    b.Property<string>("MiddleName");

                    b.Property<string>("PhoneNumber")
                        .IsRequired();

                    b.Property<int>("Title");

                    b.HasKey("PassengerInformationId");

                    b.ToTable("PassengersInformation");
                });

            modelBuilder.Entity("ImouRentACar.Models.Price", b =>
                {
                    b.Property<int>("PriceId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount");

                    b.Property<int>("CarId");

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<int>("LastModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("PriceId");

                    b.HasIndex("CarId");

                    b.ToTable("Prices");
                });

            modelBuilder.Entity("ImouRentACar.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("CanDoEverything");

                    b.Property<bool>("CanManageCars");

                    b.Property<bool>("CanManageCustomers");

                    b.Property<bool>("CanManageLandingDetails");

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<int>("LastModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ImouRentACar.Models.State", b =>
                {
                    b.Property<int>("StateId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<int>("LastModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("StateId");

                    b.ToTable("States");
                });

            modelBuilder.Entity("ImouRentACar.Models.ApplicationUser", b =>
                {
                    b.HasOne("ImouRentACar.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImouRentACar.Models.Booking", b =>
                {
                    b.HasOne("ImouRentACar.Models.PassengerInformation", "PassengerInformation")
                        .WithMany()
                        .HasForeignKey("PassengerInformationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImouRentACar.Models.Price", "Price")
                        .WithMany()
                        .HasForeignKey("PriceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImouRentACar.Models.Car", b =>
                {
                    b.HasOne("ImouRentACar.Models.CarBrand", "CarBrand")
                        .WithMany()
                        .HasForeignKey("CarBrandId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImouRentACar.Models.LGA", b =>
                {
                    b.HasOne("ImouRentACar.Models.State", "State")
                        .WithMany()
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImouRentACar.Models.Price", b =>
                {
                    b.HasOne("ImouRentACar.Models.Car", "Car")
                        .WithMany()
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
