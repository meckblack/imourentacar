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
    public class Booking : RentalTransport
    {
        #region Model Data

        public int BookingId { get; set; }

        [Required(ErrorMessage ="Pick up location is required")]
        public string PickUpLocation { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public string PickUpDate { get; set; }

        [Required(ErrorMessage = "Return location is required")]
        public string ReturnLocation { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public string ReturnDate { get; set; }

        [Required(ErrorMessage = "Destination location is required")]
        public string Destination { get; set; }

        public Verification Verification { get; set; }

        [DisplayName("Assigned Driver")]
        public int DriverId { get; set; }

        #endregion

        #region ForeignKey

        [DisplayName("Price")]
        public int PriceId { get; set; }
        [ForeignKey("PriceId")]
        public Price Price { get; set; }

        [ForeignKey("ReturnLgaId")]
        [Required(ErrorMessage ="Lga is required")]
        public int ReturnLgaId { get; set; }

        [ForeignKey("PickUpLgaId")]
        [Required(ErrorMessage = "Lga is required")]
        public int PickUpLgaId { get; set; }

        [Display(Name = "PassengerInformation")]
        public int PassengerInformationId { get; set; }
        [ForeignKey("PassengerInformationId")]
        public PassengerInformation PassengerInformation { get; set; }

        #endregion
    }
}   
