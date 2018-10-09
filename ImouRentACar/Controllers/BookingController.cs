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

//            var reservations = _database.Bookings.Include(p => p.Price);
//            return View(await reservations.ToListAsync());
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
//                    PickUpDate = booking.PickUpDate,
//                    pickUpLGAId = booking.pickUpLGAId,
//                    PickUpLocation = booking.PickUpLocation,

                    
//                    dropOffLGAId = booking.dropOffLGAId,
//                    DropOffLocation = booking.DropOffLocation,

//                    Destination = booking.Destination,
//                    Verification = Verification.YetToReply,
//                    DateSent = DateTime.Now,
//                };

//                _session.SetString("bookingobject", JsonConvert.SerializeObject(_booking));

                
//                return RedirectToAction("SelectACar", "Booking");
//            }
//            catch(Exception e)
//            {
//                throw e;
//            }
            

//        }

//        #endregion

//        #region Create

//        public IActionResult Create()
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
//        public IActionResult Create(Booking booking)
//        {
//            try
//            {
//                var _booking = new Booking()
//                {
//                    PickUpDate = booking.PickUpDate,
//                    pickUpLGAId = booking.pickUpLGAId,
//                    PickUpLocation = booking.PickUpLocation,

//                    ReturnDate = booking.ReturnDate,
//                    dropOffLGAId = booking.dropOffLGAId,
//                    DropOffLocation = booking.DropOffLocation,

//                    Destination = booking.Destination,
//                    Verification = Verification.YetToReply,
//                    DateSent = DateTime.Now,
//                };

//                _session.SetString("bookingobject", JsonConvert.SerializeObject(_booking));


//                ViewBag.PickOffStateId = new SelectList(_database.States, "StateId", "Name", booking.dropOffLGAId);
//                ViewBag.DropOffStateId = new SelectList(_database.States, "StateId", "Name", booking.dropOffLGAId);
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

//        [HttpPost]
//        public IActionResult SelectACar(Booking booking)
//        {

//            return View();
//        }

//        #endregion

//        #region ViewPrices

//        public async Task<IActionResult> ViewPrices(int id)
//        {
//            var _prices = await _database.Prices.Where(p => p.CarId == id).ToListAsync();
//            ViewData["prices"] = _prices.Count();
//            return PartialView("ViewPrices", _prices);
//        }

//        #endregion

//        #region SelectedCar

//        [HttpPost]
//        public async Task<IActionResult> Proceed()
//        {
//            var bookingObject = _session.GetString("bookingobject");
//            if(bookingObject != null)
//            {
//                var booking = JsonConvert.DeserializeObject<Booking>(bookingObject);

//                var saveBooking = new Booking()
//                {
//                    DateSent = booking.DateSent,

//                };

//                await _database.Bookings.AddAsync(saveBooking);
//                await _database.SaveChangesAsync();

//                return RedirectToAction("PassengerInformation", "Form");
//            }
//            else
//            {
//                TempData["booking"] = "Sorry your session has expired. Try booking again";
//                TempData["notificationType"] = NotificationType.Error.ToString();

//                return RedirectToAction("Booking", "RentalForm");
//            }
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
