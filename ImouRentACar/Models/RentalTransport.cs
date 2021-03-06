﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ImouRentACar.Models
{
    public class RentalTransport
    {
        #region Data Model

        [DisplayName("Verified By")]
        public int VerifiedBy { get; set; }

        [DisplayName("Date Verified")]
        public DateTime DateVerified { get; set; }

        [DisplayName("Date Sent")]
        public DateTime DateSent { get; set; }

        [DisplayName("Driver Assigned By")]
        public int DriverAssignedBy { get; set; }

        [DisplayName("Date Driver Assigned")]
        public DateTime DateDriverAssigned { get; set; }

        #endregion
    }
}
