using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImouRentACar.Models
{
    public class Contact : Transport
    {
        #region Data Model

        public int ContactId { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Mobile number one is required")]
        public string MobileNumberOne { get; set; }
        
        public string MobileNumberTwo { get; set; }

        public string BoxOfficeNumber { get; set; }
        
        #endregion
    }
}
