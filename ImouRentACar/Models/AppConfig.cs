﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImouRentACar.Models
{
    public class AppConfig
    {
        #region Mailer

        public string EmailServer => "smtp.gmail.com";
        public string Email => "meckydrix@gmail.com";
        public string Password => "bluefire2045";
        public int Port => 465;
        public string BookingPaymentHtml => "wwwroot/EmailTemplates/BookingPayment.html";
        
        #endregion
    }
}