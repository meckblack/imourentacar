using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImouRentACar.Models
{
    public class Policy
    {
        #region Data Model

        public int PolicyId { get; set; }

        [Required(ErrorMessage="Policy is required")]
        public string Text { get; set; }

        #endregion
    }
}
