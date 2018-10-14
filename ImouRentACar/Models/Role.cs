using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImouRentACar.Models
{
    public class Role : Transport
    {
        #region Data Model

        public int RoleId { get; set; }

        [Required(ErrorMessage="Name is required")]
        public string Name { get; set; }

        [Display(Name = "Can Manage Customers")]
        public bool CanManageCustomers { get; set; }

        [Display(Name = "Can Manage Landing Details")]
        public bool CanManageLandingDetails { get; set; }

        [Display(Name = "Can Manage Cars")]
        public bool CanManageCars { get; set; }

        [Display(Name = "Can Manage Prices")]
        public bool CanManagePrices { get; set; }

        [Display(Name = "Can Manage Enquires")]
        public bool CanManageEnquires { get; set; }

        [Display(Name = "Can Manage Bookings")]
        public bool CanManageBookings { get; set; }

        [Display(Name = "Can Manage States")]
        public bool CanManageStates { get; set; }

        [Display(Name = "Can Manage LGA")]
        public bool CanManageLgas { get; set; }

        [Display(Name = "Can Manage Driver")]
        public bool CanManageDrivers{ get; set; }

        [Display(Name = "Can Manage ApplicationUsers")]
        public bool CanManageApplicationUsers { get; set; }

        [Display(Name = "Can Manage PassengerInformation")]
        public bool CanManagePassengersInformation { get; set; }
        
        [Display(Name="Can Do Everything")]
        public bool CanDoEverything { get; set; }

        #endregion
    }
}
