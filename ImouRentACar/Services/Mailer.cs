using ImouRentACar.Models;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ImouRentACar.Services
{
    public class Mailer
    {
        public void BookingPaymentEmail(string path, Booking booking, PassengerInformation passenger)
        {
            try
            {
                //From Address
                var FromAddress = "info@imourentacar.com";
                var FromAddressTitle = "ImouRentACar";
                //To Address
                var toCustomer = passenger.Email;
                var ToAddressTitle = passenger.DisplayName;
                var Subject = "ImouRenACar (Booking Payment).";

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
                        replace = replace.Replace("URL", "http://imourentacar.com/booking/payment?bookingNumber=" + booking.BookingNumber);
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
                    client.Disconnect(true);
                }

            }
            catch (Exception e)
            {
                //ignore
            }
        }
    }
}
