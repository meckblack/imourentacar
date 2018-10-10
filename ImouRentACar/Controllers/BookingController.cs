﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImouRentACar.Data;
using ImouRentACar.Models;
using Microsoft.AspNetCore.Http;
using ImouRentACar.Models.Enums;
using Newtonsoft.Json;
using System.Dynamic;

namespace ImouRentACar.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public BookingController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Index

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //Counters
            ViewData["carbrandcounter"] = _database.CarBrands.Count();
            ViewData["caravaliablecounter"] = _database.Cars.Where(c => c.CarAvaliability == Avaliability.Avaliable).Count();
            ViewData["carrentedout"] = _database.Cars.Where(c => c.CarAvaliability == Avaliability.Rented).Count();
            ViewData["contactcounter"] = _database.Contacts.Count();
            ViewData["enquirycounter"] = _database.Enquiries.Count();
            ViewData["reservationcounter"] = _database.Bookings.Where(r => r.Verification == Verification.Approve).Count();

            var userid = _session.GetInt32("imouloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            ViewData["loggedinuserfullname"] = _user.DisplayName;

            var bookings = _database.Bookings.Include(p => p.Price);
            return View(await bookings.ToListAsync());
        }

        #endregion

        #region Rental Form

        [HttpGet]
        public IActionResult RentalForm()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Logos = GetLogos();
            mymodel.Contacts = GetContacts();

            foreach (Contact contact in mymodel.Contacts)
            {
                ViewData["contactnumber"] = contact.MobileNumberOne;
            }

            foreach (Logo logo in mymodel.Logos)
            {
                ViewData["imageoflogo"] = logo.Image;
            }

            ViewBag.PickOffStateId = new SelectList(_database.States, "StateId", "Name");
            ViewBag.DropOffStateId = new SelectList(_database.States, "StateId", "Name");

            return View();
        }

        [HttpPost]
        public IActionResult RentalForm(Booking booking)
        {
            try
            {
                var _booking = new Booking()
                {
                    ReturnDate = booking.ReturnDate,
                    ReturnLgaId = booking.ReturnLgaId,
                    ReturnLocation = booking.ReturnLocation,
                    PickUpDate = booking.PickUpDate,
                    PickUpLgaId = booking.PickUpLgaId,
                    PickUpLocation = booking.PickUpLocation,
                    Destination = booking.Destination,
                    Verification = Verification.YetToReply,
                    DateSent = DateTime.Now
                };

                _session.SetString("bookingobject", JsonConvert.SerializeObject(_booking));
                return RedirectToAction("SelectACar", "Booking");
            }
            catch (Exception e)
            {
                throw e;
            }


        }

        #endregion
        
        #region SelectACar

        [HttpGet]
        public async Task<IActionResult> SelectACar()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Logos = GetLogos();
            mymodel.Contacts = GetContacts();

            foreach (Contact contact in mymodel.Contacts)
            {
                ViewData["contactnumber"] = contact.MobileNumberOne;
            }

            foreach (Logo logo in mymodel.Logos)
            {
                ViewData["imageoflogo"] = logo.Image;
            }

            var cars = await _database.Cars.Where(c => c.CarAvaliability == Avaliability.Avaliable).ToListAsync();
            return View(cars);
        }
        
        #endregion

        #region ViewPrices

        [HttpGet]
        public async Task<IActionResult> ViewPrices(int id)
        {
            var prices = await _database.Prices.Where(p => p.CarId == id).ToListAsync();
            ViewData["prices"] = prices.Count();
            
            return PartialView("ViewPrices", prices);
        }

        #endregion

        #region PassengerInformation

        [HttpGet]
        public IActionResult PassengerInformation(int? id)
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Logos = GetLogos();
            mymodel.Contacts = GetContacts();

            foreach (Contact contact in mymodel.Contacts)
            {
                ViewData["contactnumber"] = contact.MobileNumberOne;
            }

            foreach (Logo logo in mymodel.Logos)
            {
                ViewData["imageoflogo"] = logo.Image;
            }

            if (id == null)
            {
                return RedirectToAction("RentalForm", "Booking");
            }
            _session.SetInt32("priceid", Convert.ToInt32(id));
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PassengerInformation(PassengerInformation passengerInformation)
        {
            var bookingObject = _session.GetString("bookingobject");
            if (bookingObject != null)
            {
                var booking = JsonConvert.DeserializeObject<Booking>(bookingObject);

                var saveBooking = new Booking()
                {
                    DateSent = booking.DateSent,
                    ReturnDate = booking.ReturnDate,
                    ReturnLgaId = booking.ReturnLgaId,
                    ReturnLocation = booking.ReturnLocation,
                    PickUpDate = booking.PickUpDate,
                    PickUpLgaId = booking.PickUpLgaId,
                    PickUpLocation = booking.PickUpLocation,
                    Destination = booking.Destination,
                    Verification = Verification.YetToReply,
                    PriceId = Convert.ToInt32(_session.GetInt32("priceid"))
                };

                await _database.Bookings.AddAsync(saveBooking);
                await _database.SaveChangesAsync();

                var _passengerInformation = new PassengerInformation()
                {
                    FirstName = passengerInformation.FirstName,
                    LastName = passengerInformation.LastName,
                    MiddleName = passengerInformation.MiddleName,
                    Email = passengerInformation.Email,
                    DOB = passengerInformation.DOB,
                    Gender = passengerInformation.Gender,
                    PhoneNumber = passengerInformation.PhoneNumber,
                    MemberId = passengerInformation.MemberId,
                    BookingId = saveBooking.BookingId,
                    Booking = booking
                };

                await _database.PassengersInformation.AddAsync(_passengerInformation);
                await _database.SaveChangesAsync();

                var carprice = await _database.Prices.FindAsync(Convert.ToInt32(_session.GetInt32("priceid")));
                var car = carprice.CarId;
                var carName = await _database.Cars.FindAsync(car);

                var _requestedCarName = carName.Name;
                var _passengerEmail = _passengerInformation.Email;
                              
                _session.SetString("successrequestedcarname", _requestedCarName);
                _session.SetString("successpassengeremail", _passengerEmail);

                return RedirectToAction("Success", "Booking");
            }
            else
            {
                TempData["booking"] = "Sorry your session has expired. Try booking again";
                TempData["notificationType"] = NotificationType.Error.ToString();

                return RedirectToAction("Booking", "RentalForm");
            }
        }

        #endregion

        #region Success

        [HttpGet]
        public IActionResult Success()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Logos = GetLogos();
            mymodel.Contacts = GetContacts();

            foreach (Contact contact in mymodel.Contacts)
            {
                ViewData["contactnumber"] = contact.MobileNumberOne;
            }

            foreach (Logo logo in mymodel.Logos)
            {
                ViewData["imageoflogo"] = logo.Image;
            }

            TempData["successrequestedcarname"] = _session.GetString("successrequestedcarname");
            TempData["successpassengeremail"] = _session.GetString("successpassengeremail");

            if (TempData["successrequestedcarname"] == null && TempData["successpassengeremail"] == null)
            {
                return NotFound();
            }

            return View();
        }

        #endregion

        #region Index



        #endregion

        #region Get Data

        public JsonResult CarBrand()
        {
            var brand = _database.CarBrands.ToArray();
            return Json(new { brands = brand });
        }

        public JsonResult GetCarByBrand(int brand)
        {
            var car = _database.Cars.Where(s => s.CarBrandId == brand).ToArray();
            return Json(new { cars = car });
        }

        public JsonResult GetLgasForState(int id)
        {
            var lgas = _database.Lgas.Where(l => l.StateId == id);
            return Json(lgas);
        }

        private List<Logo> GetLogos()
        {
            var _logos = _database.Logos.ToList();

            return _logos;
        }

        private List<Contact> GetContacts()
        {
            var _contact = _database.Contacts.ToList();
            return _contact;
        }

        #endregion

    }
}
