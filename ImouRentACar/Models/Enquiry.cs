using ImouRentACar.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImouRentACar.Models
{
    public class Enquiry : RentalTransport
    {
        #region Data model

        public int EnquiryId { get; set; }

        [DisplayName("First Name")]
        [Required(ErrorMessage="First name is required")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [DisplayName("Email Address")]
        [Required(ErrorMessage = "Email address is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Organization is required")]
        public string Organization { get; set; }

        [Required(ErrorMessage = "Comment is required")]
        public string Comment { get; set; }

        public Verification Verification { get; set; }

        public string DisplayName
            => FirstName + " " + LastName;

        #endregion
    }
}
