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
        #region Data Model

        public int BookingId { get; set; }
        
        [Required(ErrorMessage="Pick up date is required")]
        [DisplayName("Pick Up Date")]
        public DateTime PickUpDate { get; set; }

        [Required(ErrorMessage = "Return date is required")]
        [DisplayName("Return Date")]
        public DateTime ReturnDate { get; set; }

        [Required(ErrorMessage = "Pick up location is required")]
        [DisplayName("Pick Up Location")]
        public string PickUpLocation { get; set; }

        [Required(ErrorMessage = "Drop off location is requried")]
        [DisplayName("Drop Off Location")]
        public string DropOffLocation { get; set; }

        [Required(ErrorMessage = "Destination is requried")]
        public string Destination { get; set; }

        public Verification Verification { get; set; }

        [DisplayName("LGA")]
        [Required(ErrorMessage = "LGA is requried")]
        public int pickUpLGAId { get; set; }

        [DisplayName("LGA")]
        [Required(ErrorMessage = "LGA is requried")]
        public int dropOffLGAId { get; set; }
        
        #endregion

        #region Foreign Key
        
        [DisplayName("Price")]
        public int PriceId { get; set; }
        [ForeignKey("PriceId")]
        public Price Price { get; set; }
        
        #endregion
    }
}
