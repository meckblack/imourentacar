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

        public int PerssengerInformationId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public DateTime DOB { get; set; }

        public string MemberId { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public Title Title { get; set; }

        #endregion

        #region Foreign Key

        [Display(Name ="Booking")]
        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }

        #endregion
    }
}
