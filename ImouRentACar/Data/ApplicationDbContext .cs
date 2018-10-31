using System;
using System.Collections.Generic;
using System.Text;
using ImouRentACar.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ImouRentACar.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<CarBrand> CarBrands { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Enquiry> Enquiries { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Logo> Logos { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<AboutUsImage> AboutUsImages { get; set; }
        public DbSet<AboutUsImageTwo> AboutUsImageTwos { get; set; }
        public DbSet<Header> Headers { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<LGA> Lgas { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<PassengerInformation> PassengersInformation { get; set; }
        public DbSet<Driver> Driver { get; set; }
        public DbSet<RentACar> RentACars { get; set; }
        public DbSet<Policy> Policies { get; set; }



    }
}
