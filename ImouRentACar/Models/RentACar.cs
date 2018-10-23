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
    public class RentACar : RentalTransport
    {
        #region Model Data

        public int RentACarId { get; set; }

        [Required(ErrorMessage = "Pick up location is required")]
        public string PickUpLocation { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime PickDate { get; set; }

        [Required(ErrorMessage = "Time is required")]
        public DateTime PickUpTime { get; set; }

        [DisplayName("Rental Days")]
        public int Days { get; set; }

        public Verification Verification { get; set; }

        [DisplayName("Assigned Driver")]
        public int DriverId { get; set; }

        public Decimal TotalBookingPrice { get; set; }

        public string BookingNumber { get; set; }

        public int CustomerId { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        #endregion

        #region ForeignKey

        [Display(Name = "Car")]
        [ForeignKey("CarId")]
        public int CarId { get; set; }
        
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
