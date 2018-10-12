using ImouRentACar.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImouRentACar.Models
{
    public class Customer
    {
        #region Model Data

        public int CustomerId { get; set; }

        [Required(ErrorMessage = "First name field is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name field is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email field is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Number field is required")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Passoword field is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage="Confirm passoword field is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfrimPassword { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public Title Title { get; set; }

        public string DisplayName
            => FirstName + " " + LastName;

        public int MemberId { get; set; }

        #endregion
    }
}
