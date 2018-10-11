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
        public decimal Amount { get; set; }

        #endregion

        #region ForeignKey

        [ForeignKey("DestinationLgaId")]
        [Required(ErrorMessage = "Lga is required")]
        public int DestinationLgaId { get; set; }

        [ForeignKey("PickUpLgaId")]
        [Required(ErrorMessage = "Lga is required")]
        public int PickUpLgaId { get; set; }

        #endregion
    }
}
