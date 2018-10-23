using ImouRentACar.Models;
using MimeKit;
using System;
using System.IO;
using System.Globalization;

namespace ImouRentACar.Services
{
    public class Mailer
    {
        public void OneWayTripPaymentEmail(string path, OneWayTrip oneWayTrip, PassengerInformation passenger)
        {
            try
            {
                //From Address
                var FromAddress = "imourentacar@gmail.com";
                var FromAddressTitle = "imourentacar@gmail.com";
                //To Address
                var toCustomer = passenger.Email;
                var ToAddressTitle = passenger.DisplayName;
                var Subject = "ImouRenACar (One Way Payment).";

                //Smtp Server
                var smtpServer = new AppConfig().EmailServer;

                //Smtp Port Number
                var smtpPortNumber = new AppConfig().Port;

                var mimeMessageCustomer = new MimeMessage();
                mimeMessageCustomer.From.Add(new MailboxAddress(FromAddressTitle, FromAddress));
                mimeMessageCustomer.To.Add(new MailboxAddress(ToAddressTitle, toCustomer));
                mimeMessageCustomer.Subject = Subject;

                var bodyBuilder = new BodyBuilder();
                using (var data = File.OpenText(path))
                {
                    if(data.BaseStream != null)
                    {
                        //MANAGE CONTENT

                        bodyBuilder.HtmlBody = data.ReadToEnd();
                        var body = bodyBuilder.HtmlBody;

                        var replace = body.Replace("USER", passenger.DisplayName);
                        replace = replace.Replace("BOOKINGNUMBER", oneWayTrip.BookingNumber);
                        replace = replace.Replace("PICKUPLOCATION ", oneWayTrip.PickUpLocation);
                        replace = replace.Replace("DESTINATION  ", oneWayTrip.Destination);
                        replace = replace.Replace("URL", "http://imourentacar.com/booking/payment?bookingNumber=" + oneWayTrip.BookingNumber);
                        replace = replace.Replace("LOGO", "https://www.imourentacar.com/images/logo.png");
                        replace = replace.Replace("PRIVACY", "https://www.imourentacar.com/privacy/index");
                        replace = replace.Replace("TC", "https://www.imourentacar.com/privacy/index");
                        bodyBuilder.HtmlBody = replace;
                        mimeMessageCustomer.Body = bodyBuilder.ToMessageBody();
                    }
                }

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect(smtpServer, smtpPortNumber);
                    // Note: only needed if the SMTP server requires authentication
                    // Error 5.5.1 Authentication 
                    client.Authenticate(new AppConfig().Email, new AppConfig().Password);
                    client.Send(mimeMessageCustomer);
                    client.Disconnect(true);
                }

            }
            catch (Exception e)
            {
                //ignore
            }
        }

        public void TwoWayTripPaymentEmail(string path, TwoWayTrip twoWayTrip, PassengerInformation passenger)
        {
            try
            {
                //From Address
                var FromAddress = "imourentacar@gmail.com";
                var FromAddressTitle = "ImouRentACar";
                //To Address
                var toCustomer = passenger.Email;
                var ToAddressTitle = passenger.DisplayName;
                var Subject = "ImouRenACar (Two Way Trip Payment).";

                //Smtp Server
                var smtpServer = new AppConfig().EmailServer;

                //Smtp Port Number
                var smtpPortNumber = new AppConfig().Port;

                var mimeMessageCustomer = new MimeMessage();
                mimeMessageCustomer.From.Add(new MailboxAddress(FromAddressTitle, FromAddress));
                mimeMessageCustomer.To.Add(new MailboxAddress(ToAddressTitle, toCustomer));
                mimeMessageCustomer.Subject = Subject;

                var bodyBuilder = new BodyBuilder();
                using (var data = File.OpenText(path))
                {
                    if (data.BaseStream != null)
                    {
                        //MANAGE CONTENT

                        bodyBuilder.HtmlBody = data.ReadToEnd();
                        var body = bodyBuilder.HtmlBody;

                        var replace = body.Replace("USER", passenger.DisplayName);
                        replace = replace.Replace("URL", "http://imourentacar.com/booking/payment?bookingNumber=" + twoWayTrip.BookingNumber);
                        replace = replace.Replace("LOGO", "https://www.imourentacar.com/images/logo.png");
                        replace = replace.Replace("PRIVACY", "https://www.imourentacar.com/privacy/index");
                        replace = replace.Replace("TC", "https://www.imourentacar.com/privacy/index");
                        bodyBuilder.HtmlBody = replace;
                        mimeMessageCustomer.Body = bodyBuilder.ToMessageBody();
                    }
                }

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect(smtpServer, smtpPortNumber);
                    // Note: only needed if the SMTP server requires authentication
                    // Error 5.5.1 Authentication 
                    client.Authenticate(new AppConfig().Email, new AppConfig().Password);
                    client.Send(mimeMessageCustomer);
                    client.Disconnect(true);
                }

            }
            catch (Exception e)
            {
                //ignore
            }
        }

        public void RentACarPaymentEmail(string path, RentACar rentACar, PassengerInformation passenger)
        {
            try
            {
                //From Address
                var FromAddress = "imourentacar@gmail.com";
                var FromAddressTitle = "ImouRentACar";
                //To Address
                var toCustomer = passenger.Email;
                var ToAddressTitle = passenger.DisplayName;
                var Subject = "ImouRenACar (Car Rental Payment).";

                //Smtp Server
                var smtpServer = new AppConfig().EmailServer;

                //Smtp Port Number
                var smtpPortNumber = new AppConfig().Port;

                var mimeMessageCustomer = new MimeMessage();
                mimeMessageCustomer.From.Add(new MailboxAddress(FromAddressTitle, FromAddress));
                mimeMessageCustomer.To.Add(new MailboxAddress(ToAddressTitle, toCustomer));
                mimeMessageCustomer.Subject = Subject;

                var bodyBuilder = new BodyBuilder();
                using (var data = File.OpenText(path))
                {
                    if (data.BaseStream != null)
                    {
                        //MANAGE CONTENT

                        bodyBuilder.HtmlBody = data.ReadToEnd();
                        var body = bodyBuilder.HtmlBody;

                        var replace = body.Replace("USER", passenger.DisplayName);
                        replace = replace.Replace("URL", "http://imourentacar.com/booking/payment?bookingNumber=" + rentACar.BookingNumber);
                        replace = replace.Replace("LOGO", "https://www.imourentacar.com/images/logo.png");
                        replace = replace.Replace("PRIVACY", "https://www.imourentacar.com/privacy/index");
                        replace = replace.Replace("TC", "https://www.imourentacar.com/privacy/index");
                        bodyBuilder.HtmlBody = replace;
                        mimeMessageCustomer.Body = bodyBuilder.ToMessageBody();
                    }
                }

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect(smtpServer, smtpPortNumber);
                    // Note: only needed if the SMTP server requires authentication
                    // Error 5.5.1 Authentication 
                    client.Authenticate(new AppConfig().Email, new AppConfig().Password);
                    client.Send(mimeMessageCustomer);
                    client.Disconnect(true);
                }

            }
            catch (Exception e)
            {
                //ignore
            }
        }

        public void CustomerRequestCarRental(string path, RentACar rentACar, PassengerInformation passenger)
        {
            try
            {
                //From Address
                var FromAddress = "imourentacar@gmail.com";
                var FromAddressTitle = "ImouRentACar";
                //To Address
                var toCustomer = "imourentacar@gmail.com";
                var ToAddressTitle = "imourentacar@gmail.com";
                var Subject = "ImouRenACar (Car Rental Payment).";

                //Smtp Server
                var smtpServer = new AppConfig().EmailServer;

                //Smtp Port Number
                var smtpPortNumber = new AppConfig().Port;

                var mimeMessageCustomer = new MimeMessage();
                mimeMessageCustomer.From.Add(new MailboxAddress(FromAddressTitle, FromAddress));
                mimeMessageCustomer.To.Add(new MailboxAddress(ToAddressTitle, toCustomer));
                mimeMessageCustomer.Subject = Subject;

                var bodyBuilder = new BodyBuilder();
                using (var data = File.OpenText(path))
                {
                    if (data.BaseStream != null)
                    {
                        //MANAGE CONTENT

                        bodyBuilder.HtmlBody = data.ReadToEnd();
                        var body = bodyBuilder.HtmlBody;

                        var replace = body.Replace("USER", passenger.DisplayName);
                        replace = replace.Replace("PICKUPLOCATION", rentACar.PickUpLocation);
                        replace = replace.Replace("LOGO", "https://www.imourentacar.com/images/logo.png");
                        replace = replace.Replace("PRIVACY", "https://www.imourentacar.com/privacy/index");
                        replace = replace.Replace("TC", "https://www.imourentacar.com/privacy/index");
                        bodyBuilder.HtmlBody = replace;
                        mimeMessageCustomer.Body = bodyBuilder.ToMessageBody();
                    }
                }

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect(smtpServer, smtpPortNumber);
                    // Note: only needed if the SMTP server requires authentication
                    // Error 5.5.1 Authentication 
                    client.Authenticate(new AppConfig().Email, new AppConfig().Password);
                    client.Send(mimeMessageCustomer);
                    client.Disconnect(true);
                }

            }
            catch (Exception e)
            {
                //ignore
            }
        }

        public void CustomerRequestTwoWayTrip(string path, TwoWayTrip twoWayTrip, PassengerInformation passenger)
        {
            try
            {
                //From Address
                var FromAddress = "imourentacar@gmail.com";
                var FromAddressTitle = "ImouRentACar";
                //To Address
                var toCustomer = "imourentacar@gmail.com";
                var ToAddressTitle = "imourentacar@gmail.com";
                var Subject = "ImouRenACar (Car Rental Payment).";

                //Smtp Server
                var smtpServer = new AppConfig().EmailServer;

                //Smtp Port Number
                var smtpPortNumber = new AppConfig().Port;

                var mimeMessageCustomer = new MimeMessage();
                mimeMessageCustomer.From.Add(new MailboxAddress(FromAddressTitle, FromAddress));
                mimeMessageCustomer.To.Add(new MailboxAddress(ToAddressTitle, toCustomer));
                mimeMessageCustomer.Subject = Subject;

                var bodyBuilder = new BodyBuilder();
                using (var data = File.OpenText(path))
                {
                    if (data.BaseStream != null)
                    {
                        //MANAGE CONTENT

                        bodyBuilder.HtmlBody = data.ReadToEnd();
                        var body = bodyBuilder.HtmlBody;

                        var replace = body.Replace("USER", passenger.DisplayName);
                        replace = replace.Replace("PICKUPLOCATION", twoWayTrip.PickUpLocation);
                        replace = replace.Replace("LOGO", "https://www.imourentacar.com/images/logo.png");
                        replace = replace.Replace("PRIVACY", "https://www.imourentacar.com/privacy/index");
                        replace = replace.Replace("TC", "https://www.imourentacar.com/privacy/index");
                        bodyBuilder.HtmlBody = replace;
                        mimeMessageCustomer.Body = bodyBuilder.ToMessageBody();
                    }
                }

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect(smtpServer, smtpPortNumber);
                    // Note: only needed if the SMTP server requires authentication
                    // Error 5.5.1 Authentication 
                    client.Authenticate(new AppConfig().Email, new AppConfig().Password);
                    client.Send(mimeMessageCustomer);
                    client.Disconnect(true);
                }

            }
            catch (Exception e)
            {
                //ignore
            }
        }

        public void CustomerRequestOneWayTrip(string path, OneWayTrip oneWayTrip, PassengerInformation passenger)
        {
            try
            {
                //From Address
                var FromAddress = "imourentacar@gmail.com";
                var FromAddressTitle = "ImouRentACar";
                //To Address
                var toCustomer = "imourentacar@gmail.com";
                var ToAddressTitle = "imourentacar@gmail.com";
                var Subject = "ImouRenACar (Car Rental Payment).";

                //Smtp Server
                var smtpServer = new AppConfig().EmailServer;

                //Smtp Port Number
                var smtpPortNumber = new AppConfig().Port;

                var mimeMessageCustomer = new MimeMessage();
                mimeMessageCustomer.From.Add(new MailboxAddress(FromAddressTitle, FromAddress));
                mimeMessageCustomer.To.Add(new MailboxAddress(ToAddressTitle, toCustomer));
                mimeMessageCustomer.Subject = Subject;

                var bodyBuilder = new BodyBuilder();
                using (var data = File.OpenText(path))
                {
                    if (data.BaseStream != null)
                    {
                        //MANAGE CONTENT

                        bodyBuilder.HtmlBody = data.ReadToEnd();
                        var body = bodyBuilder.HtmlBody;

                        var replace = body.Replace("USER", passenger.DisplayName);
                        replace = replace.Replace("LOGO", "https://www.imourentacar.com/images/logo.png");
                        replace = replace.Replace("PRIVACY", "https://www.imourentacar.com/privacy/index");
                        replace = replace.Replace("TC", "https://www.imourentacar.com/privacy/index");
                        bodyBuilder.HtmlBody = replace;
                        mimeMessageCustomer.Body = bodyBuilder.ToMessageBody();
                    }
                }

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect(smtpServer, smtpPortNumber);
                    // Note: only needed if the SMTP server requires authentication
                    // Error 5.5.1 Authentication 
                    client.Authenticate(new AppConfig().Email, new AppConfig().Password);
                    client.Send(mimeMessageCustomer);
                    client.Disconnect(true);
                }

            }
            catch (Exception e)
            {
                //ignore
            }
        }

        public void PasswordRecovery(string path, ApplicationUser user)
        {
            //From Address
            var FromAddress = "imourentacar@gmail.com";
            var FromAdressTitle = "Imou Rent A Car";
            //To Address
            var toUser = user.Email;
            //var toCustomer = email;
            var ToAdressTitle = user.DisplayName;
            var Subject = "ImouRentACar (Password Reset).";

            //Smtp Server
            var smtpServer = new AppConfig().EmailServer;
            //Smtp Port Number
            var smtpPortNumber = new AppConfig().Port;

            var mimeMessageUser = new MimeMessage();
            mimeMessageUser.From.Add(new MailboxAddress(FromAdressTitle, FromAddress));
            mimeMessageUser.To.Add(new MailboxAddress(ToAdressTitle, toUser));
            mimeMessageUser.Subject = Subject;
            var bodyBuilder = new BodyBuilder();

            using (var data = File.OpenText(path))
            {
                if(data.BaseStream != null)
                {
                    // manage content
                    bodyBuilder.HtmlBody = data.ReadToEnd();
                    var body = bodyBuilder.HtmlBody;

                    var replace = body.Replace("USER", user.DisplayName);
                    replace = replace.Replace("DATE", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    replace = replace.Replace("LOGO", "https://www.imourentacar.com/images/logo.png");
                    replace = replace.Replace("APPURL", "https://www.imourentacar.com");
                    replace = replace.Replace("PRIVACY", "https://www.imourentacar.com/privacy/index");
                    replace = replace.Replace("TC", "https://www.imourentacar.com/privacy/index");
                    replace = replace.Replace("URL",
                        "http://imourentacar.com/Account/ForgotPassword?appUser=" + Convert.ToString(user.ApplicationUserId));
                    bodyBuilder.HtmlBody = replace;
                    mimeMessageUser.Body = bodyBuilder.ToMessageBody();
                }
            }

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(smtpServer, smtpPortNumber);
                // Note: only needed if the SMTP server requires authentication
                // Error 5.5.1 Authentication 
                client.Authenticate(new AppConfig().Email, new AppConfig().Password);
                client.Send(mimeMessageUser);
                client.Disconnect(true);
            }

        }

        public void PasswordRecovery(string path, Customer customer)
        {
            //From Address
            var FromAddress = "imourentacar@gmail.com";
            var FromAdressTitle = "Imou Rent A Car";
            //To Address
            var toUser = customer.Email;
            //var toCustomer = email;
            var ToAdressTitle = customer.DisplayName;
            var Subject = "ImouRentACar (Password Reset).";

            //Smtp Server
            var smtpServer = new AppConfig().EmailServer;
            //Smtp Port Number
            var smtpPortNumber = new AppConfig().Port;

            var mimeMessageUser = new MimeMessage();
            mimeMessageUser.From.Add(new MailboxAddress(FromAdressTitle, FromAddress));
            mimeMessageUser.To.Add(new MailboxAddress(ToAdressTitle, toUser));
            mimeMessageUser.Subject = Subject;
            var bodyBuilder = new BodyBuilder();

            using (var data = File.OpenText(path))
            {
                if (data.BaseStream != null)
                {
                    // manage content
                    bodyBuilder.HtmlBody = data.ReadToEnd();
                    var body = bodyBuilder.HtmlBody;

                    var replace = body.Replace("USER", customer.DisplayName);
                    replace = replace.Replace("DATE", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    replace = replace.Replace("LOGO", "https://www.imourentacar.com/images/logo.png");
                    replace = replace.Replace("APPURL", "https://www.imourentacar.com");
                    replace = replace.Replace("PRIVACY", "https://www.imourentacar.com/privacy/index");
                    replace = replace.Replace("TC", "https://www.imourentacar.com/privacy/index");
                    replace = replace.Replace("URL",
                        "http://imourentacar.com/Account/newPassword?user=" + Convert.ToString(customer.CustomerId));
                    bodyBuilder.HtmlBody = replace;
                    mimeMessageUser.Body = bodyBuilder.ToMessageBody();
                }
            }

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(smtpServer, smtpPortNumber);
                // Note: only needed if the SMTP server requires authentication
                // Error 5.5.1 Authentication 
                client.Authenticate(new AppConfig().Email, new AppConfig().Password);
                client.Send(mimeMessageUser);
                client.Disconnect(true);
            }

        }
    }
}
