using ImouRentACar.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImouRentACar.Models
{
    public class Driver : Transport
    {
        public int DriverId { get; set; }

        [DisplayName("First Name")]
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage="Last name is required")]
        public string LastName { get; set; }

        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }

        //public string Age { get; set; }

        [DisplayName("Date Of Birth")]
        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DOB { get; set; }

        [DisplayName("Driver License")]
        [Required(ErrorMessage = "Drivers License is required")]
        public string License { get; set; }

        public Avaliability DriverAvaliablity { get; set; }

    }
}
