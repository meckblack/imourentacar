using ImouRentACar.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImouRentACar.Models
{
    public class Car : Transport
    {
        #region Model Data

        public int CarId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public string Image { get; set; }

        [Required(ErrorMessage = "Trip price is required")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Rental price is required")]
        public decimal RentalPrice { get; set; }

        [Required(ErrorMessage = "Color")]
        public Color Color { get; set; }

        [Required(ErrorMessage = "Speed KM/H")]
        public string Speed { get; set; }

        [Required(ErrorMessage = "Engine Details")]
        public string Engine { get; set; }

        [Required(ErrorMessage = "Car Avaliablity")]
        public Avaliability CarAvaliability { get; set; }

        #endregion

        #region Foreign Key

        [DisplayName("Car Brand")]
        public int CarBrandId { get; set; }
        [ForeignKey("CarBrandId")]
        public CarBrand CarBrand { get; set; }
        
        #endregion
        
    }
}
