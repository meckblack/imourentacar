using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using ImouRentACar.Data;
using ImouRentACar.Models;
using ImouRentACar.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ImouRentACar.Controllers
{
    public class OneWayTripController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public OneWayTripController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Book

        [HttpGet]
        public IActionResult Book()
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
            
            var customerObject = _session.GetString("imouloggedincustomer");

            //This function is called if a customer is signed in
            if (customerObject == null)
            {
                var _customer = JsonConvert.DeserializeObject<Customer>(customerObject);
                TempData["customername"] = _customer.DisplayName;

                ViewBag.PickOffStateId = new SelectList(_database.States, "StateId", "Name");
                ViewBag.DropOffStateId = new SelectList(_database.States, "StateId", "Name");
                return View();
            }

            ViewBag.PickOffStateId = new SelectList(_database.States, "StateId", "Name");
            ViewBag.DropOffStateId = new SelectList(_database.States, "StateId", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Book (OneWayTrip oneWayTrip)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Checks if there is a fixed price rate for the choosen destination
                    var price = await _database.Prices
                        .SingleOrDefaultAsync(p => p.PickUpLgaId == oneWayTrip.PickUpLgaId && p.DestinationLgaId == oneWayTrip.DestinationLgaId
                                           || p.PickUpLgaId == oneWayTrip.DestinationLgaId && p.DestinationLgaId == oneWayTrip.PickUpLgaId);

                    //This function is called if a fixed price rate is not choosen 
                    if (price == null)
                    {
                        TempData["error"] = "Sorry there is no fixed price for your desired destination. Can you kindly urtler your " +
                            "distination before you continue the booking process";
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

                        return View(oneWayTrip);
                    }

                    var _priceId = price.PriceId;
                    var _tripPrice = price.Amount;

                    var _bookOneWayTrip = new OneWayTrip()
                    {
                        Destination = oneWayTrip.Destination,
                        PickDate = oneWayTrip.PickDate,
                        PickUpTime = oneWayTrip.PickUpTime,
                        PickUpLgaId = oneWayTrip.PickUpLgaId,
                        PickUpLocation = oneWayTrip.PickUpLocation,

                        Verification = Verification.YetToReply,
                        PriceId = _priceId,
                        TotalBookingPrice = _tripPrice,
                        DateSent = DateTime.Now
                    };

                    _session.SetString("bookonewaytrip", JsonConvert.SerializeObject(_bookOneWayTrip));
                    return RedirectToAction("SelectACar", "OneWayTrip");
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return View();
        }

        #endregion

        #region Get Data

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