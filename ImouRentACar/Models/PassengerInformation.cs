using ImouRentACar.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImouRentACar.Models
{
    public class PassengerInformation
    {
        #region Data Model

        public int PassengerInformationId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string DOB { get; set; }

        public string MemberId { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public Title Title { get; set; }

        public string DisplayName
            => FirstName + " " + LastName;

        #endregion

        #region Foreign Key

        #endregion

        public IEnumerable<Booking> Booking { get; set; }
    }
}
