using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using ImouRentACar.Data;
using ImouRentACar.Models;
using ImouRentACar.Models.Enums;
using ImouRentACar.Services;
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

        #region Processing OneWayTrip

        [HttpGet]
        [SessionExpireFilterAttribute]
        [Route("onewaytrip/processing")]
        public async Task<IActionResult> ProcessingOneWayTrips()
        {
            var userid = _session.GetInt32("imouloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            ViewData["loggedinuserfullname"] = _user.DisplayName;

            var roleid = _user.RoleId;

            var role = _database.Roles.Find(roleid);

            ViewData["userrole"] = role.Name;

            if (role.CanManageBookings == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            ViewData["canmangecars"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageCars == true && r.RoleId == roleid);
            ViewData["canmangecustomers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageCustomers == true && r.RoleId == roleid);
            ViewData["canmangelandingdetails"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageLandingDetails == true && r.RoleId == roleid);
            ViewData["canmangeprices"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManagePrices == true && r.RoleId == roleid);
            ViewData["canmangeenquries"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageEnquires == true && r.RoleId == roleid);
            ViewData["canmangebookings"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageBookings == true && r.RoleId == roleid);
            ViewData["canmangelocation"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageLocation == true && r.RoleId == roleid);
            ViewData["canmangedrivers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageDrivers == true && r.RoleId == roleid);
            ViewData["canmangepassengersinformation"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManagePassengersInformation == true && r.RoleId == roleid);
            ViewData["candoeverything"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanDoEverything == true && r.RoleId == roleid);
            ViewData["canmanageapplicationusers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageApplicationUsers == true && r.RoleId == roleid);

            var twoWayTrips = _database.TwoWayTrips.Where(b => b.PaymentStatus == PaymentStatus.Processing);
            return View(await twoWayTrips.ToListAsync());
        }

        #endregion

        #region Paid One Way Trip

        [HttpGet]
        [SessionExpireFilterAttribute]
        [Route("onewaytrip/paid")]
        public async Task<IActionResult> PaidOneWayTrips()
        {
            var userid = _session.GetInt32("imouloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            ViewData["loggedinuserfullname"] = _user.DisplayName;

            var roleid = _user.RoleId;

            var role = _database.Roles.Find(roleid);

            ViewData["userrole"] = role.Name;

            if (role.CanManageBookings == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            ViewData["canmangecars"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageCars == true && r.RoleId == roleid);
            ViewData["canmangecustomers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageCustomers == true && r.RoleId == roleid);
            ViewData["canmangelandingdetails"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageLandingDetails == true && r.RoleId == roleid);
            ViewData["canmangeprices"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManagePrices == true && r.RoleId == roleid);
            ViewData["canmangeenquries"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageEnquires == true && r.RoleId == roleid);
            ViewData["canmangebookings"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageBookings == true && r.RoleId == roleid);
            ViewData["canmangelocation"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageLocation == true && r.RoleId == roleid);
            ViewData["canmangedrivers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageDrivers == true && r.RoleId == roleid);
            ViewData["canmangepassengersinformation"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManagePassengersInformation == true && r.RoleId == roleid);
            ViewData["candoeverything"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanDoEverything == true && r.RoleId == roleid);
            ViewData["canmanageapplicationusers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageApplicationUsers == true && r.RoleId == roleid);

            var oneWayTrips = _database.OneWayTrips.Where(b => b.PaymentStatus == PaymentStatus.Paid);
            return View(await oneWayTrips.ToListAsync());
        }

        #endregion

        #region Unpaid One way Trip

        [HttpGet]
        [SessionExpireFilterAttribute]
        [Route("onewaytrip/unpaid")]
        public async Task<IActionResult> UnpaidOneWayTrips()
        {
            var userid = _session.GetInt32("imouloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            ViewData["loggedinuserfullname"] = _user.DisplayName;

            var roleid = _user.RoleId;

            var role = _database.Roles.Find(roleid);

            ViewData["userrole"] = role.Name;

            if (role.CanManageBookings == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            ViewData["canmangecars"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageCars == true && r.RoleId == roleid);
            ViewData["canmangecustomers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageCustomers == true && r.RoleId == roleid);
            ViewData["canmangelandingdetails"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageLandingDetails == true && r.RoleId == roleid);
            ViewData["canmangeprices"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManagePrices == true && r.RoleId == roleid);
            ViewData["canmangeenquries"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageEnquires == true && r.RoleId == roleid);
            ViewData["canmangebookings"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageBookings == true && r.RoleId == roleid);
            ViewData["canmangelocation"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageLocation == true && r.RoleId == roleid);
            ViewData["canmangedrivers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageDrivers == true && r.RoleId == roleid);
            ViewData["canmangepassengersinformation"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManagePassengersInformation == true && r.RoleId == roleid);
            ViewData["candoeverything"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanDoEverything == true && r.RoleId == roleid);
            ViewData["canmanageapplicationusers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageApplicationUsers == true && r.RoleId == roleid);

            var oneWayTrips = _database.OneWayTrips.Where(b => b.PaymentStatus == PaymentStatus.Unpaid);
            return View(await oneWayTrips.ToListAsync());
        }

        #endregion

        #region Expired One Way Trip

        [HttpGet]
        [SessionExpireFilterAttribute]
        [Route("onewaytrip/expired")]
        public async Task<IActionResult> ExpiredOneWayTrips()
        {
            var userid = _session.GetInt32("imouloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            ViewData["loggedinuserfullname"] = _user.DisplayName;

            var roleid = _user.RoleId;

            var role = _database.Roles.Find(roleid);

            ViewData["userrole"] = role.Name;

            if (role.CanManageBookings == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            ViewData["canmangecars"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageCars == true && r.RoleId == roleid);
            ViewData["canmangecustomers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageCustomers == true && r.RoleId == roleid);
            ViewData["canmangelandingdetails"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageLandingDetails == true && r.RoleId == roleid);
            ViewData["canmangeprices"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManagePrices == true && r.RoleId == roleid);
            ViewData["canmangeenquries"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageEnquires == true && r.RoleId == roleid);
            ViewData["canmangebookings"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageBookings == true && r.RoleId == roleid);
            ViewData["canmangelocation"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageLocation == true && r.RoleId == roleid);
            ViewData["canmangedrivers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageDrivers == true && r.RoleId == roleid);
            ViewData["canmangepassengersinformation"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManagePassengersInformation == true && r.RoleId == roleid);
            ViewData["candoeverything"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanDoEverything == true && r.RoleId == roleid);
            ViewData["canmanageapplicationusers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageApplicationUsers == true && r.RoleId == roleid);

            var oneWayTrips = _database.OneWayTrips.Where(b => b.PaymentStatus == PaymentStatus.Expired);
            return View(await oneWayTrips.ToListAsync());
        }

        #endregion

        #region Book

        [HttpGet]
        [Route("onewaytrip/book")]
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
                ViewBag.PickOffStateId = new SelectList(_database.States, "StateId", "Name");
                ViewBag.DropOffStateId = new SelectList(_database.States, "StateId", "Name");
                return View();
            }

            var _customer = JsonConvert.DeserializeObject<Customer>(customerObject);
            TempData["customername"] = _customer.DisplayName;

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
                        DestinationLgaId = oneWayTrip.DestinationLgaId,
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

        #region SelectACar

        [HttpGet]
        [Route("onewaytrip/selectacar")]
        public async Task<IActionResult> SelectACar()
        {
            var trial = _session.GetString("bookonewaytrip");

            if (trial == null)
            {
                return RedirectToAction("Index", "Error");
            }

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

            var customerObject = _session.GetString("imouloggedincustomer");
            if (customerObject == null)
            {
                return View(cars);
            }

            var _customer = JsonConvert.DeserializeObject<Customer>(customerObject);
            TempData["customername"] = _customer.DisplayName;

            return View(cars);
        }

        #endregion

        #region Passenger Information

        [HttpGet]
        [Route("onewaytrip/passengerinformation")]
        public async Task<IActionResult> PassengerInformation(int? id)
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

            var allCars = await _database.Cars.ToListAsync();

            if(allCars.Any(c => c.CarId == id))
            {
                var customerObject = _session.GetString("imouloggedincustomer");

                //This function is called if customer is signed in
                if (customerObject == null)
                {
                    var _bookonewaytrip = _session.GetString("bookonewaytrip");

                    if (_bookonewaytrip == null)
                    {
                        return RedirectToAction("Index", "Error");
                    }
                    var _booking = JsonConvert.DeserializeObject<OneWayTrip>(_bookonewaytrip);

                    //Get Car
                    var _carid = id;
                    var car = await _database.Cars.FindAsync(_carid);

                    //Get Car Brand
                    var _brandid = car.CarBrandId;
                    var brand = await _database.CarBrands.FindAsync(_brandid);

                    
                    TempData["carimage"] = car.Image;
                    TempData["carbrand"] = brand.Name;
                    TempData["carname"] = car.Name;
                    TempData["carprice"] = car.Price;
                    TempData["pickuplocation"] = _booking.PickUpLocation;
                    TempData["returnlocation"] = _booking.Destination;
                    TempData["destinationprice"] = _booking.TotalBookingPrice;
                    TempData["totalprice"] = _booking.TotalBookingPrice + car.Price;
                    return View();
                }

                var _customer = JsonConvert.DeserializeObject<Customer>(customerObject);
                TempData["customername"] = _customer.DisplayName;


                var bookonewaytrip = _session.GetString("bookonewaytrip");

                if (bookonewaytrip == null)
                {
                    return RedirectToAction("Error", "Home");
                }
                var booking = JsonConvert.DeserializeObject<OneWayTrip>(bookonewaytrip);

                //Get Car
                var carid = id;
                var _car = await _database.Cars.FindAsync(carid);

                //Get Car Brand
                var brandid = _car.CarBrandId;
                var _brand = await _database.CarBrands.FindAsync(brandid);


                TempData["carimage"] = _car.Image;
                TempData["carbrand"] = _brand.Name;
                TempData["carname"] = _car.Name;
                TempData["carprice"] = _car.Price;
                TempData["pickuplocation"] = booking.PickUpLocation;
                TempData["returnlocation"] = booking.Destination;
                TempData["destinationprice"] = booking.TotalBookingPrice;
                TempData["totalprice"] = booking.TotalBookingPrice + _car.Price;

                return View();
            }

            return RedirectToAction("Index", "Error");
        }

        [HttpPost]
        public async Task<IActionResult> PassengerInformation(int? id, PassengerInformation passengerInformation)
        {
            try
            {
                //This function is called when a customer is signed in
                var customerObject = _session.GetString("imouloggedincustomer");
                if (customerObject != null)
                {
                    var _customer = JsonConvert.DeserializeObject<Customer>(customerObject);

                    var _passengerInformations = new PassengerInformation()
                    {
                        FirstName = _customer.FirstName,
                        LastName = _customer.LastName,
                        Email = _customer.Email,
                        Title = _customer.Title,
                        Gender = _customer.Gender,
                        PhoneNumber = _customer.MobileNumber,
                        MemberId = _customer.MemberId
                    };

                    await _database.PassengersInformation.AddAsync(_passengerInformations);
                    await _database.SaveChangesAsync();

                    var _bookonewaytrip = _session.GetString("bookonewaytrip");

                    if (_bookonewaytrip != null)
                    {
                        var oneWayTrip = JsonConvert.DeserializeObject<OneWayTrip>(_bookonewaytrip);
                        var car = await _database.Cars.FindAsync(id);
                        var carPrice = car.Price;

                        var stringGenerator = new RandomStringGenerator();
                        var bookingNumber = stringGenerator.RandomString(8);

                        var saveBooking = new OneWayTrip()
                        {
                            DateSent = oneWayTrip.DateSent,
                            DestinationLgaId = oneWayTrip.DestinationLgaId,
                            Destination = oneWayTrip.Destination,
                            PickDate = oneWayTrip.PickDate,
                            PickUpTime = oneWayTrip.PickUpTime,
                            PickUpLgaId = oneWayTrip.PickUpLgaId,
                            PickUpLocation = oneWayTrip.PickUpLocation,
                            Verification = Verification.YetToReply,
                            CarId = Convert.ToInt32(id),
                            TotalBookingPrice = oneWayTrip.TotalBookingPrice + carPrice,
                            PriceId = oneWayTrip.PriceId,
                            PassengerInformationId = _passengerInformations.PassengerInformationId,
                            PassengerInformation = _passengerInformations,
                            CustomerId = _customer.CustomerId,
                            BookingNumber = bookingNumber,
                            PaymentStatus = PaymentStatus.Processing
                        };

                        var getcarname = await _database.Cars.FindAsync(saveBooking.CarId);

                        _session.SetString("successrequestedcarname", getcarname.Name);
                        _session.SetString("successpassengeremail", _passengerInformations.Email);

                        await _database.OneWayTrips.AddAsync(saveBooking);
                        await _database.SaveChangesAsync();

                        return RedirectToAction("Success", "OneWayTrip");
                    }
                    else
                    {
                        TempData["error"] = "Sorry your session has expired.";
                        return RedirectToAction("Signin", "Customer");
                    }
                }

                //Thus function is called when the customer is not signed in buh has
                // an account and enters his memberid
                if (passengerInformation.MemberId != null)
                {
                    var checkMemberId = await _database.Customers.SingleOrDefaultAsync(c => c.MemberId == passengerInformation.MemberId);
                    if (checkMemberId == null)
                    {
                        TempData["error"] = "Sorry the MemberId you entered is invalid";

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

                        return RedirectToAction("Book", "OneWayTrip");
                    }

                    var passenger = new PassengerInformation()
                    {
                        FirstName = passengerInformation.FirstName,
                        LastName = passengerInformation.LastName,
                        MiddleName = passengerInformation.MiddleName,
                        Email = passengerInformation.Email,
                        DOB = passengerInformation.DOB,
                        Title = passengerInformation.Title,
                        Gender = passengerInformation.Gender,
                        PhoneNumber = passengerInformation.PhoneNumber,
                        MemberId = passengerInformation.MemberId,
                    };

                    await _database.PassengersInformation.AddAsync(passenger);
                    await _database.SaveChangesAsync();

                    var _customer = await _database.Customers.SingleOrDefaultAsync(c => c.MemberId == passengerInformation.MemberId);

                    var _bookonewaytrip = _session.GetString("bookonewaytrip");

                    if (_bookonewaytrip != null)
                    {
                        var oneWayTrip = JsonConvert.DeserializeObject<OneWayTrip>(_bookonewaytrip);
                        var car = await _database.Cars.FindAsync(id);
                        var carPrice = car.Price;

                        //Randomly generate booking number
                        var stringGenerator = new RandomStringGenerator();
                        var bookingNumber = stringGenerator.RandomString(8);

                        var saveBooking = new OneWayTrip()
                        {
                            DateSent = oneWayTrip.DateSent,
                            DestinationLgaId = oneWayTrip.DestinationLgaId,
                            Destination = oneWayTrip.Destination,
                            PickDate = oneWayTrip.PickDate,
                            PickUpTime = oneWayTrip.PickUpTime,
                            PickUpLgaId = oneWayTrip.PickUpLgaId,
                            PickUpLocation = oneWayTrip.PickUpLocation,
                            Verification = Verification.YetToReply,
                            CarId = Convert.ToInt32(id),
                            TotalBookingPrice = oneWayTrip.TotalBookingPrice + carPrice,
                            PriceId = oneWayTrip.PriceId,
                            PassengerInformationId = passenger.PassengerInformationId,
                            PassengerInformation = passenger,
                            BookingNumber = bookingNumber,
                            CustomerId = _customer.CustomerId,
                            PaymentStatus = PaymentStatus.Processing
                        };

                        var getcarname = await _database.Cars.FindAsync(saveBooking.CarId);

                        _session.SetString("successrequestedcarname", getcarname.Name);
                        _session.SetString("successpassengeremail", passenger.Email);


                        await _database.OneWayTrips.AddAsync(saveBooking);
                        await _database.SaveChangesAsync();

                        return RedirectToAction("Success", "OneWayTrip");
                    }
                    else
                    {
                        TempData["bookingerror"] = "Sorry your session has expired.";
                        return RedirectToAction("Index", "Error");
                    }
                }

                //This function is called when the customer is not signed in and does not have an account.
                //Therefore the memberid is left un entered.
                var _passengerInformation = new PassengerInformation()
                {
                    FirstName = passengerInformation.FirstName,
                    LastName = passengerInformation.LastName,
                    MiddleName = passengerInformation.MiddleName,
                    Email = passengerInformation.Email,
                    DOB = passengerInformation.DOB,
                    Title = passengerInformation.Title,
                    Gender = passengerInformation.Gender,
                    PhoneNumber = passengerInformation.PhoneNumber,
                    MemberId = "unregisteredcustomer",
                };

                await _database.PassengersInformation.AddAsync(_passengerInformation);
                await _database.SaveChangesAsync();

                var bookonewaytrip = _session.GetString("bookonewaytrip");

                if (bookonewaytrip != null)
                {
                    var oneWayTrip = JsonConvert.DeserializeObject<OneWayTrip>(bookonewaytrip);
                    var car = await _database.Cars.FindAsync(id);
                    var carPrice = car.Price;

                    //Randomly generate booking number
                    var stringGenerator = new RandomStringGenerator();
                    var bookingNumber = stringGenerator.RandomString(8);

                    var saveBooking = new OneWayTrip()
                    {
                        DateSent = oneWayTrip.DateSent,
                        DestinationLgaId = oneWayTrip.DestinationLgaId,
                        Destination = oneWayTrip.Destination,
                        PickUpTime = oneWayTrip.PickUpTime,
                        PickDate = oneWayTrip.PickDate,
                        PickUpLgaId = oneWayTrip.PickUpLgaId,
                        PickUpLocation = oneWayTrip.PickUpLocation,
                        Verification = Verification.YetToReply,
                        CarId = Convert.ToInt32(id),
                        TotalBookingPrice = oneWayTrip.TotalBookingPrice + carPrice,
                        PriceId = oneWayTrip.PriceId,
                        PassengerInformationId = _passengerInformation.PassengerInformationId,
                        PassengerInformation = _passengerInformation,
                        BookingNumber = bookingNumber,
                        PaymentStatus = PaymentStatus.Processing
                    };

                    await _database.OneWayTrips.AddAsync(saveBooking);
                    await _database.SaveChangesAsync();

                    var getcarname = await _database.Cars.FindAsync(saveBooking.CarId);

                    _session.SetString("successrequestedcarname", getcarname.Name);
                    _session.SetString("successpassengeremail", _passengerInformation.Email);

                    return RedirectToAction("Success", "OneWayTrip");
                }
                else
                {
                    TempData["bookingerror"] = "Sorry your session has expired.";
                    return RedirectToAction("Index", "Error");
                }

            }
            catch (Exception e)
            {
                return View(e);
            }
        }

        #endregion

        #region Success

        [HttpGet]
        [Route("onewaytrip/successfullbooking")]
        public IActionResult Success()
        {
            var customerObject = _session.GetString("imouloggedincustomer");

            if (customerObject != null)
            {
                var _customer = JsonConvert.DeserializeObject<Customer>(customerObject);

                dynamic cmymodel = new ExpandoObject();
                cmymodel.Logos = GetLogos();
                cmymodel.Contacts = GetContacts();

                foreach (Contact contact in cmymodel.Contacts)
                {
                    ViewData["contactnumber"] = contact.MobileNumberOne;
                }

                foreach (Logo logo in cmymodel.Logos)
                {
                    ViewData["imageoflogo"] = logo.Image;
                }

                TempData["successrequestedcarname"] = _session.GetString("successrequestedcarname");
                TempData["successpassengeremail"] = _session.GetString("successpassengeremail");

                if (TempData["successrequestedcarname"] == null && TempData["successpassengeremail"] == null)
                {
                    return RedirectToAction("Index", "Error");
                }

                TempData["customername"] = _customer.DisplayName;

                return View();
            }

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

        #region Cancel

        [HttpGet]
        [Route("onewaytrip/cancelbooking")]
        public IActionResult Cancel()
        {
            _session.Clear();
            _database.Dispose();
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Approve One Way Trip

        [HttpGet]
        public async Task<IActionResult> Approve(int? id)
        {
            var userObject = _session.GetString("imouloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);
            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);
            if (role.CanManageBookings == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var oneWayTrip = await _database.OneWayTrips.SingleOrDefaultAsync(b => b.OneWayTripId == id);

            if (oneWayTrip == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Approve", oneWayTrip);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Approve(int id, OneWayTrip oneWayTrip)
        {
            if (id != oneWayTrip.OneWayTripId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    oneWayTrip.DateVerified = DateTime.Now;
                    oneWayTrip.VerifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));
                    oneWayTrip.Verification = Verification.Approve;

                    _database.OneWayTrips.Update(oneWayTrip);
                    await _database.SaveChangesAsync();

                    TempData["onewaytrip"] = "You have successfully verified a booking request. Next Assign A Driver ";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OneWayTripExists(oneWayTrip.OneWayTripId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View("ProcessingOneWayTrip");
        }

        #endregion

        #region Disapprove One Way Trip

        [HttpGet]
        [SessionExpireFilter]
        public async Task<IActionResult> Disapprove(int? id)
        {
            var userObject = _session.GetString("imouloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);
            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);
            if (role.CanManageBookings == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var oneWayTrip = await _database.OneWayTrips.SingleOrDefaultAsync(b => b.OneWayTripId == id);

            if (oneWayTrip == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Disapprove", oneWayTrip);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Disapprove(int id, OneWayTrip oneWayTrip)
        {
            if (id != oneWayTrip.OneWayTripId)
            {
                return RedirectToAction("Index", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    oneWayTrip.DateVerified = DateTime.Now;
                    oneWayTrip.VerifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));
                    oneWayTrip.Verification = Verification.Disapprove;

                    _database.OneWayTrips.Update(oneWayTrip);
                    await _database.SaveChangesAsync();

                    TempData["onewaytrip"] = "You have successfully disappoved a booking request.";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OneWayTripExists(oneWayTrip.OneWayTripId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View("ProcessingOneWayTrip");
        }

        #endregion

        #region Assign Driver To One Way Trip

        [HttpGet]
        public async Task<IActionResult> AssignDriver(int? id)
        {
            var userObject = _session.GetString("imouloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);
            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);
            if (role.CanManageBookings == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var oneWayTrip = await _database.OneWayTrips.SingleOrDefaultAsync(b => b.OneWayTripId == id);

            if (oneWayTrip == null)
            {
                return NotFound();
            }

            ViewBag.DriverId = new SelectList(_database.Driver, "DriverId", "DisplayName");
            return PartialView("AssignDriver", oneWayTrip);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> AssignDriver(int id, OneWayTrip oneWayTrip)
        {
            if (id != oneWayTrip.OneWayTripId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    oneWayTrip.DateDriverAssigned = DateTime.Now;
                    oneWayTrip.DriverAssignedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));

                    _database.OneWayTrips.Update(oneWayTrip);
                    await _database.SaveChangesAsync();

                    TempData["onewaytrip"] = "You have successfully assigned a driver to the a booking request. Next Send Link";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OneWayTripExists(oneWayTrip.OneWayTripId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewBag.DriverId = new SelectList(_database.Driver, "DriverId", "Name", oneWayTrip.DriverId);
            return View("ProcessingOneWayTrip");
        }

        #endregion

        #region Remove Driver From One Way Trip

        [HttpGet]
        public async Task<IActionResult> RemoveDriver(int? id)
        {
            var userObject = _session.GetString("imouloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);
            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);
            if (role.CanManageBookings == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var oneWayTrip = await _database.OneWayTrips.SingleOrDefaultAsync(b => b.OneWayTripId == id);

            if (oneWayTrip == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("RemoveDriver", oneWayTrip);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> RemoveDriver(int id, OneWayTrip oneWayTrip)
        {
            if (id != oneWayTrip.OneWayTripId)
            {
                return RedirectToAction("Index", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    oneWayTrip.DriverId = 0;
                    oneWayTrip.DateDriverAssigned = DateTime.Now;
                    oneWayTrip.DriverAssignedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));

                    _database.OneWayTrips.Update(oneWayTrip);
                    await _database.SaveChangesAsync();

                    TempData["onewaytrip"] = "You have successfully removed the driver of a one way trip. Next Assign Driver";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OneWayTripExists(oneWayTrip.OneWayTripId))
                    {
                        return RedirectToAction("Index", "Error");
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View("ProcessingOneWayTrip");
        }

        #endregion

        #region Send Link to Passenger

        [HttpGet]
        [SessionExpireFilter]
        public async Task<IActionResult> SendALink(int? id)
        {
            var userObject = _session.GetString("imouloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);
            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);
            if (role.CanManageBookings == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var oneWayTrip = await _database.OneWayTrips.SingleOrDefaultAsync(b => b.OneWayTripId == id);

            if (oneWayTrip == null)
            {
                return NotFound();
            }

            var passengerId = oneWayTrip.PassengerInformationId;
            var _passengerDetails = await _database.PassengersInformation.FindAsync(passengerId);

            ViewData["passengername"] = _passengerDetails.DisplayName;
            ViewData["passengeremail"] = _passengerDetails.Email;

            return PartialView("SendLink", oneWayTrip);
        }

        [HttpPost]
        public async Task<IActionResult> SendALink(int id, OneWayTrip oneWayTrip)
        {
            if (id != oneWayTrip.OneWayTripId)
            {
                return RedirectToAction("Index", "Error");
            }


            var _oneWayTrip = await _database.OneWayTrips.SingleOrDefaultAsync(b => b.OneWayTripId == id);

            var passengerId = _oneWayTrip.PassengerInformationId;
            var _passenger = await _database.PassengersInformation.FindAsync(passengerId);

            oneWayTrip.Verification = Verification.LinkSent;
            oneWayTrip.PaymentStatus = PaymentStatus.Unpaid;

            _database.OneWayTrips.Update(oneWayTrip);
            await _database.SaveChangesAsync();

            var car = await _database.Cars.SingleOrDefaultAsync(c => c.CarId == _oneWayTrip.CarId);

            var _car = new Car()
            {
                CarId = car.CarId,
                Name = car.Name,
                Price = car.Price,
                RentalPrice = car.RentalPrice,
                Speed = car.Speed,
                CarBrandId = car.CarBrandId,
                Color = car.Color,
                CreatedBy = car.CreatedBy,
                DateCreated = car.DateCreated,
                Description = car.Description,
                Engine = car.Engine,
                Image = car.Image,
                CarAvaliability = Avaliability.Rented,
                LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid")),
                DateLastModified = DateTime.Now,
            };

            _database.Cars.Update(_car);
            await _database.SaveChangesAsync();

            new Mailer().OneWayTripPaymentEmail(new AppConfig().BookingPaymentHtml, oneWayTrip, _passenger);

            return View();
        }

        #endregion

        #region Payment

        [HttpGet]
        public async Task<IActionResult> Payment(string bookingNumber)
        {
            var oneWayTrip = await _database.OneWayTrips.Where(b => b.PaymentStatus == PaymentStatus.Unpaid).ToListAsync();

            var _oneWayTrip = oneWayTrip.SingleOrDefault(b => b.BookingNumber == bookingNumber);

            if (_oneWayTrip == null)
            {
                TempData["expiredbooking"] = "Dear customer your pickup time has passed and so there your booking has expired.";
                return RedirectToAction("Index", "Error");
            }

            //Get Car
            var carid = _oneWayTrip.CarId;
            var _car = await _database.Cars.FindAsync(carid);

            //Get Car Brand
            var brandid = _car.CarBrandId;
            var _brand = await _database.CarBrands.FindAsync(brandid);


            TempData["carimage"] = _car.Image;
            TempData["carbrand"] = _brand.Name;
            TempData["carname"] = _car.Name;
            TempData["carprice"] = _car.Price;
            TempData["pickuplocation"] = _oneWayTrip.PickUpLocation;
            TempData["returnlocation"] = _oneWayTrip.Destination;
            TempData["destinationprice"] = _oneWayTrip.TotalBookingPrice;
            TempData["totalprice"] = _oneWayTrip.TotalBookingPrice;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PaymentSuccessful(string bookingNumber)
        {
            var oneWayTrip = await _database.OneWayTrips.Where(b => b.PaymentStatus == PaymentStatus.Unpaid).ToListAsync();

            var _oneWayTrip = oneWayTrip.SingleOrDefault(b => b.BookingNumber == bookingNumber);

            var oneway = new OneWayTrip()
            {
                OneWayTripId = _oneWayTrip.OneWayTripId,
                BookingNumber = _oneWayTrip.BookingNumber,
                CarId = _oneWayTrip.CarId,
                CustomerId = _oneWayTrip.CustomerId,
                DateDriverAssigned = _oneWayTrip.DateDriverAssigned,
                DateSent = _oneWayTrip.DateSent,
                DateVerified = _oneWayTrip.DateVerified,
                Destination = _oneWayTrip.Destination,
                DestinationLgaId = _oneWayTrip.DestinationLgaId,
                DriverAssignedBy = _oneWayTrip.DriverAssignedBy,
                DriverId = _oneWayTrip.DriverId,
                PickDate = _oneWayTrip.PickDate,
                PassengerInformation = _oneWayTrip.PassengerInformation,
                PaymentStatus = PaymentStatus.Paid,
                PickUpLgaId = _oneWayTrip.PickUpLgaId,
                PassengerInformationId = _oneWayTrip.PassengerInformationId,
                PickUpLocation = _oneWayTrip.PickUpLocation,
                PickUpTime = _oneWayTrip.PickUpTime,
                PriceId = _oneWayTrip.PriceId,
                TotalBookingPrice = _oneWayTrip.TotalBookingPrice,
                Verification = _oneWayTrip.Verification,
                VerifiedBy = _oneWayTrip.VerifiedBy,
            };

            _database.OneWayTrips.Update(oneway);
            await _database.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        #endregion
        
        #region Delete

        [HttpGet]
        [SessionExpireFilter]
        public async Task<IActionResult> Delete(int? id)
        {
            var userObject = _session.GetString("imouloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);
            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);
            if (role.CanManageBookings == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var oneWayTrip = await _database.OneWayTrips.SingleOrDefaultAsync(b => b.OneWayTripId == id);

            if (oneWayTrip == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", oneWayTrip);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var oneWayTrip = await _database.OneWayTrips.SingleOrDefaultAsync(b => b.OneWayTripId == id);

            if (oneWayTrip != null)
            {
                _database.OneWayTrips.Remove(oneWayTrip);
                await _database.SaveChangesAsync();

                TempData["onewaytrip"] = "You have successfully deleted a paid one way trip!!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return View("PaidOneWayTrips");
        }

        #endregion

        #region Completed

        [HttpGet]
        public async Task<IActionResult> Completed(int? id)
        {
            var userObject = _session.GetString("imouloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);
            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);
            if (role.CanManageBookings == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var oneWayTrip = await _database.OneWayTrips.SingleOrDefaultAsync(b => b.OneWayTripId == id);

            if (oneWayTrip == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Completed", oneWayTrip);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Completed(int id, OneWayTrip oneWayTrip)
        {
            if (id != oneWayTrip.OneWayTripId)
            {
                return RedirectToAction("Index", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    oneWayTrip.DateVerified = DateTime.Now;
                    oneWayTrip.VerifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));
                    oneWayTrip.Verification = Verification.Approve;

                    _database.OneWayTrips.Update(oneWayTrip);
                    await _database.SaveChangesAsync();

                    var car = await _database.Cars.SingleOrDefaultAsync(c => c.CarId == oneWayTrip.CarId);

                    var _car = new Car()
                    {
                        CarId = car.CarId,
                        CarAvaliability = Avaliability.Avaliable,
                        CarBrand = car.CarBrand,
                        CarBrandId = car.CarBrandId,
                        Color = car.Color,
                        CreatedBy = car.CreatedBy,
                        DateCreated = car.DateCreated,
                        DateLastModified = car.DateLastModified,
                        Description = car.Description,
                        Engine = car.Engine,
                        Image = car.Image,
                        LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid")),
                        Name = car.Name,
                        Price = car.Price,
                        RentalPrice = car.RentalPrice,
                        Speed = car.Speed
                    };

                    _database.Cars.Update(_car);
                    await _database.SaveChangesAsync();

                    TempData["onewaytrip"] = "One way trip has been completed ";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OneWayTripExists(oneWayTrip.OneWayTripId))
                    {
                        return RedirectToAction("Index", "Error");
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View("ProcessingOneWayTrip");
        }

        #endregion

        #region One Way Trip Exists

        private bool OneWayTripExists(int id)
        {
            return _database.OneWayTrips.Any(e => e.OneWayTripId == id);
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