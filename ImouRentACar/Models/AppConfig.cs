using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImouRentACar.Models
{
    public class AppConfig
    {
        #region Mailer

        public string EmailServer => "smtp.gmail.com";
        public string Email => "imourentacar@gmail.com";
        public string Password => "ImouRentACar18";
        public int Port => 465;
        public string BookingPaymentHtml => "wwwroot/EmailTemplates/BookingPayment.html";
        public string ForgotPasswordHtml => "wwwroot/EmailTemplates/ForgotPassword.html";

        #endregion
    }
}
