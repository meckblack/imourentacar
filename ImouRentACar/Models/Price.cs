using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImouRentACar.Models
{
    public class Price : Transport
    {
        #region Data Model

        public int PriceId { get; set; }

        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage="Amount is required")]
        public double Amount { get; set; }

        #endregion

        #region ForeignKey

        [DisplayName("Car")]
        public int CarId { get; set; }
        [ForeignKey("CarId")]
        public Car Car { get; set; }

        #endregion
    }
}
