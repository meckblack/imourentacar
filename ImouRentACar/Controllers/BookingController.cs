//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using ImouRentACar.Data;
//using ImouRentACar.Models;
//using Microsoft.AspNetCore.Http;
//using ImouRentACar.Models.Enums;
//using Newtonsoft.Json;
//using System.Dynamic;
//using Microsoft.AspNetCore.SignalR.Protocol;
//using MimeKit;
//using MailKit.Net.Smtp;

//namespace ImouRentACar.Controllers
//{
//    public class BookingController : Controller
//    {
//        private readonly ApplicationDbContext _database;
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private ISession _session => _httpContextAccessor.HttpContext.Session;

//        #region Constructor

//        public BookingController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
//        {
//            _database = context;
//            _httpContextAccessor = httpContextAccessor;
//        }

//        #endregion

//        #region Index

//        [HttpGet]
//        public async Task<IActionResult> Index()
//        {
//            //Counters
//            ViewData["carbrandcounter"] = _database.CarBrands.Count();
//            ViewData["caravaliablecounter"] = _database.Cars.Where(c => c.CarAvaliability == Avaliability.Avaliable).Count();
//            ViewData["carrentedout"] = _database.Cars.Where(c => c.CarAvaliability == Avaliability.Rented).Count();
//            ViewData["contactcounter"] = _database.Contacts.Count();
//            ViewData["enquirycounter"] = _database.Enquiries.Count();
//            ViewData["reservationcounter"] = _database.Bookings.Where(r => r.Verification == Verification.Approve).Count();

//            var userid = _session.GetInt32("imouloggedinuserid");
//            var _user = await _database.ApplicationUsers.FindAsync(userid);
//            ViewData["loggedinuserfullname"] = _user.DisplayName;

//            var bookings = _database.Bookings.Include(p => p.Price);
//            return View(await bookings.ToListAsync());
//        }

//        #endregion

//        #region Rental Form

//        [HttpGet]
//        public IActionResult RentalForm()
//        {
//            dynamic mymodel = new ExpandoObject();
//            mymodel.Logos = GetLogos();
//            mymodel.Contacts = GetContacts();

//            foreach (Contact contact in mymodel.Contacts)
//            {
//                ViewData["contactnumber"] = contact.MobileNumberOne;
//            }

//            foreach (Logo logo in mymodel.Logos)
//            {
//                ViewData["imageoflogo"] = logo.Image;
//            }

//            ViewBag.PickOffStateId = new SelectList(_database.States, "StateId", "Name");
//            ViewBag.DropOffStateId = new SelectList(_database.States, "StateId", "Name");

//            return View();
//        }

//        [HttpPost]
//        public IActionResult RentalForm(Booking booking)
//        {
//            try
//            {
//                var _booking = new Booking()
//                {
//                    ReturnDate = booking.ReturnDate,
//                    ReturnLgaId = booking.ReturnLgaId,
//                    ReturnLocation = booking.ReturnLocation,
//                    PickUpDate = booking.PickUpDate,
//                    PickUpLgaId = booking.PickUpLgaId,
//                    PickUpLocation = booking.PickUpLocation,
//                    Destination = booking.Destination,
//                    Verification = Verification.YetToReply,
//                    DateSent = DateTime.Now
//                };

//                _session.SetString("bookingobject", JsonConvert.SerializeObject(_booking));
//                return RedirectToAction("SelectACar", "Booking");
//            }
//            catch (Exception e)
//            {
//                throw e;
//            }


//        }

//        #endregion

//        #region SelectACar

//        [HttpGet]
//        public async Task<IActionResult> SelectACar()
//        {
//            var trial = _session.GetString("bookingobject");

//            if(trial == null)
//            {
//                return NotFound();
//            }

//            dynamic mymodel = new ExpandoObject();
//            mymodel.Logos = GetLogos();
//            mymodel.Contacts = GetContacts();

//            foreach (Contact contact in mymodel.Contacts)
//            {
//                ViewData["contactnumber"] = contact.MobileNumberOne;
//            }

//            foreach (Logo logo in mymodel.Logos)
//            {
//                ViewData["imageoflogo"] = logo.Image;
//            }

//            var cars = await _database.Cars.Where(c => c.CarAvaliability == Avaliability.Avaliable).ToListAsync();
//            return View(cars);
//        }

//        #endregion

//        #region ViewPrices

//        [HttpGet]
//        public async Task<IActionResult> ViewPrices(int id)
//        {
//            var prices = await _database.Prices.Where(p => p.CarId == id).ToListAsync();
//            ViewData["prices"] = prices.Count();

//            return PartialView("ViewPrices", prices);
//        }

//        #endregion

//        #region PassengerInformation

//        [HttpGet]
//        public IActionResult PassengerInformation(int? id)
//        {
//            dynamic mymodel = new ExpandoObject();
//            mymodel.Logos = GetLogos();
//            mymodel.Contacts = GetContacts();

//            foreach (Contact contact in mymodel.Contacts)
//            {
//                ViewData["contactnumber"] = contact.MobileNumberOne;
//            }

//            foreach (Logo logo in mymodel.Logos)
//            {
//                ViewData["imageoflogo"] = logo.Image;
//            }

//            if (id == null)
//            {
//                return RedirectToAction("RentalForm", "Booking");
//            }
//            _session.SetInt32("priceid", Convert.ToInt32(id));
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> PassengerInformation(PassengerInformation passengerInformation)
//        {
//            if (ModelState.IsValid)
//            {
//                var _passengerInformation = new PassengerInformation()
//                {
//                    FirstName = passengerInformation.FirstName,
//                    LastName = passengerInformation.LastName,
//                    MiddleName = passengerInformation.MiddleName,
//                    Email = passengerInformation.Email,
//                    DOB = passengerInformation.DOB,
//                    Gender = passengerInformation.Gender,
//                    PhoneNumber = passengerInformation.PhoneNumber,
//                    MemberId = passengerInformation.MemberId,
//                };

//                await _database.PassengersInformation.AddAsync(_passengerInformation);
//                await _database.SaveChangesAsync();

//                var bookingObject = _session.GetString("bookingobject");

//                if (bookingObject != null)
//                {
//                    var booking = JsonConvert.DeserializeObject<Booking>(bookingObject);

//                    var saveBooking = new Booking()
//                    {
//                        DateSent = booking.DateSent,
//                        ReturnDate = booking.ReturnDate,
//                        ReturnLgaId = booking.ReturnLgaId,
//                        ReturnLocation = booking.ReturnLocation,
//                        PickUpDate = booking.PickUpDate,
//                        PickUpLgaId = booking.PickUpLgaId,
//                        PickUpLocation = booking.PickUpLocation,
//                        Destination = booking.Destination,
//                        Verification = Verification.YetToReply,
//                        PriceId = Convert.ToInt32(_session.GetInt32("priceid")),
//                        PassengerInformationId = _passengerInformation.PassengerInformationId,
//                        PassengerInformation = _passengerInformation
//                    };

//                    await _database.Bookings.AddAsync(saveBooking);
//                    await _database.SaveChangesAsync();
                    
//                    var carprice = await _database.Prices.FindAsync(Convert.ToInt32(_session.GetInt32("priceid")));
//                    var car = carprice.CarId;
//                    var carName = await _database.Cars.FindAsync(car);

//                    var _requestedCarName = carName.Name;
//                    var _passengerEmail = _passengerInformation.Email;

//                    _session.SetString("successrequestedcarname", _requestedCarName);
//                    _session.SetString("successpassengeremail", _passengerEmail);

//                    return RedirectToAction("Success", "Booking");
//                }
//                else
//                {
//                    TempData["booking"] = "Sorry your session has expired. Try booking again";
//                    TempData["notificationType"] = NotificationType.Error.ToString();

//                    return RedirectToAction("Booking", "RentalForm");
//                }
//            }

//            return View("PassengerInformation", passengerInformation);
//        }

//        #endregion

//        #region Success

//        [HttpGet]
//        public IActionResult Success()
//        {
//            dynamic mymodel = new ExpandoObject();
//            mymodel.Logos = GetLogos();
//            mymodel.Contacts = GetContacts();

//            foreach (Contact contact in mymodel.Contacts)
//            {
//                ViewData["contactnumber"] = contact.MobileNumberOne;
//            }

//            foreach (Logo logo in mymodel.Logos)
//            {
//                ViewData["imageoflogo"] = logo.Image;
//            }

//            TempData["successrequestedcarname"] = _session.GetString("successrequestedcarname");
//            TempData["successpassengeremail"] = _session.GetString("successpassengeremail");

//            if (TempData["successrequestedcarname"] == null && TempData["successpassengeremail"] == null)
//            {
//                return NotFound();
//            }

//            return View();
//        }

//        #endregion

//        #region Approve Booking Request

//        [HttpGet]
//        public async Task<IActionResult> Approve(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var _booking = await _database.Bookings.SingleOrDefaultAsync(b => b.BookingId == id);

//            if (_booking == null)
//            {
//                return NotFound();
//            }

//            return PartialView("Approve", _booking);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Approve(int id, Booking booking)
//        {
//            if (id != booking.BookingId)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    booking.DateVerified = DateTime.Now;
//                    booking.VerifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));
//                    booking.Verification = Verification.Approve;

//                    _database.Bookings.Update(booking);
//                    await _database.SaveChangesAsync();

//                    TempData["booking"] = "You have successfully verified a booking request. Next Assign A Driver ";
//                    TempData["notificationType"] = NotificationType.Success.ToString();

//                    return Json(new { success = true });
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!BookingExists(booking.BookingId))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//            }

//            return View("Index");
//        }

//        #endregion

//        #region Disapprove Booking Request

//        [HttpGet]
//        public async Task<IActionResult> Disapprove(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var _booking = await _database.Bookings.SingleOrDefaultAsync(b => b.BookingId == id);

//            if (_booking == null)
//            {
//                return NotFound();
//            }

//            return PartialView("Disapprove", _booking);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Disapprove(int id, Booking booking)
//        {
//            if (id != booking.BookingId)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    booking.DateVerified = DateTime.Now;
//                    booking.VerifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));
//                    booking.Verification = Verification.Disapprove;

//                    _database.Bookings.Update(booking);
//                    await _database.SaveChangesAsync();

//                    TempData["booking"] = "You have successfully disappoved a booking request.";
//                    TempData["notificationType"] = NotificationType.Success.ToString();

//                    return Json(new { success = true });
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!BookingExists(booking.BookingId))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//            }

//            return View("Index");
//        }

//        #endregion

//        #region Assign Driver to Booking Request

//        [HttpGet]
//        public async Task<IActionResult> AssignDriver(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var _booking = await _database.Bookings.SingleOrDefaultAsync(b => b.BookingId == id);

//            if (_booking == null)
//            {
//                return NotFound();
//            }

//            ViewBag.DriverId = new SelectList(_database.Driver, "DriverId", "DisplayName");
//            return PartialView("AssignDriver", _booking);
//        }

//        [HttpPost]
//        public async Task<IActionResult> AssignDriver(int id, Booking booking)
//        {
//            if (id != booking.BookingId)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    booking.DateDriverAssigned = DateTime.Now;
//                    booking.DriverAssignedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));
                    
//                    _database.Bookings.Update(booking);
//                    await _database.SaveChangesAsync();

//                    TempData["booking"] = "You have successfully assigned a driver to the a booking request. Next Send Link";
//                    TempData["notificationType"] = NotificationType.Success.ToString();

//                    return Json(new { success = true });
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!BookingExists(booking.BookingId))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//            }
//            ViewBag.DriverId = new SelectList(_database.Driver, "DriverId", "Name", booking.DriverId);
//            return View("Index");
//        }

//        #endregion

//        #region Remove Driver from Booking Request

//        [HttpGet]
//        public async Task<IActionResult> RemoveDriver(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var _booking = await _database.Bookings.SingleOrDefaultAsync(b => b.BookingId == id);

//            if (_booking == null)
//            {
//                return NotFound();
//            }
            
//            return PartialView("RemoveDriver", _booking);
//        }

//        [HttpPost]
//        public async Task<IActionResult> RemoveDriver(int id, Booking booking)
//        {
//            if (id != booking.BookingId)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    booking.DriverId = 0;
//                    booking.DateDriverAssigned = DateTime.Now;
//                    booking.DriverAssignedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));

//                    _database.Bookings.Update(booking);
//                    await _database.SaveChangesAsync();

//                    TempData["booking"] = "You have successfully removed the driver of a booking request. Next Assign Driver";
//                    TempData["notificationType"] = NotificationType.Success.ToString();

//                    return Json(new { success = true });
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!BookingExists(booking.BookingId))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//            }

//            return View("Index");
//        }

//        #endregion

//        #region Send Link to Passenger

//        [HttpGet]
//        public async Task<IActionResult> SendLink(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var _booking = await _database.Bookings.SingleOrDefaultAsync(b => b.BookingId == id);

//            if (_booking == null)
//            {
//                return NotFound();
//            }

//            var passengerId = _booking.PassengerInformationId;
//            var _passengerDetails = await _database.PassengersInformation.FindAsync(passengerId);

//            ViewData["passengername"] = _passengerDetails.DisplayName;
//            ViewData["passengeremail"] = _passengerDetails.Email;

//            return PartialView("SendLink", _booking);
//        }

//        [HttpPost]
//        public async Task<IActionResult> SendLink(int id)
//        {
//            var _booking = await _database.Bookings.SingleOrDefaultAsync(b => b.BookingId == id);

//            var passengerId = _booking.PassengerInformationId;
//            var _passengerDetails = await _database.PassengersInformation.FindAsync(passengerId);

//            var message = new MimeMessage();
//            message.From.Add(new MailboxAddress("imourentacar", "mecktinum@gmail.com"));

//            message.To.Add(new MailboxAddress(_passengerDetails.DisplayName, _passengerDetails.Email));
//            message.Subject = "ImouRentACar Payment Link";

//            //var url = Url.Action("", "", new { }, protocol: Request.Url.Scheme)

//            //var emailBody = "<div>" +
//            //            "<h3 style='font-size: 30px; text-align:center;'><strong>ASSOCIATION INFORMATION MANAGEMENT SYSTEM</strong></h3>" +
//            //            "<div style='position: relative; min-height: 1px; padding-right: 15px; padding-left: 15px; padding-top: 5px;'>" +
//            //                "<h4 style='font-size: 18px; text-align:justify;'>You have been added to the Association Information Management System. </h4>" +
//            //                "<p style='font-size: 18px; text-align:justify;'>Your username is " + _passengerDetails.Email + " and your password is Please login and change your password by clicking <a href=\"" + url + "\">here</a></p>" +
//            //            "<footer style='font-size: 18px; text-align:center;'>" +
//            //                "<p>&copy;" + DateTime.Now.Year + " Override.</p></footer></div>";

//            //message.Body = new BodyBuilder()
//            //{
//            //    HtmlBody = message.
//            //};

//            message.Body = new TextPart("plain")
//            {
//                Text = "This is the message"
//            };

//            using (var client = new SmtpClient())
//            {
//                client.Connect("smtp.gmail.com", 587, false);
//                await client.AuthenticateAsync("mecktinum@gmail.com", "bluefire2045");

//                client.Disconnect(true);

//                TempData["booking"] = "You have successfully sent a payment link to " + _passengerDetails.Email + " email address for a booking request made by "  
//                                        + _passengerDetails.DisplayName;
//                TempData["notificationType"] = NotificationType.Success.ToString();

//                return Json(new { success = true });
//            }
//        }

//        #endregion

//        #region Booking Exists

//        private bool BookingExists(int id)
//        {
//            return _database.Bookings.Any(e => e.BookingId == id);
//        }

//        #endregion

//        #region Get Data

//        public JsonResult CarBrand()
//        {
//            var brand = _database.CarBrands.ToArray();
//            return Json(new { brands = brand });
//        }

//        public JsonResult GetCarByBrand(int brand)
//        {
//            var car = _database.Cars.Where(s => s.CarBrandId == brand).ToArray();
//            return Json(new { cars = car });
//        }

//        public JsonResult GetLgasForState(int id)
//        {
//            var lgas = _database.Lgas.Where(l => l.StateId == id);
//            return Json(lgas);
//        }

//        private List<Logo> GetLogos()
//        {
//            var _logos = _database.Logos.ToList();

//            return _logos;
//        }

//        private List<Contact> GetContacts()
//        {
//            var _contact = _database.Contacts.ToList();
//            return _contact;
//        }

//        #endregion

//    }
//}
