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
    public class TwoWayTripController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public TwoWayTripController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Processing Two Way Trip

        [HttpGet]
        [SessionExpireFilterAttribute]
        [Route("twowaytrip/processing")]
        public async Task<IActionResult> ProcessingTwoWayTrips()
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
            ViewData["canmangecarbrand"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageCarBrand == true && r.RoleId == roleid);
            ViewData["canmangeprices"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManagePrices == true && r.RoleId == roleid);
            ViewData["canmangeenquries"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageEnquires == true && r.RoleId == roleid);
            ViewData["canmangebookings"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageLandingDetails == true && r.RoleId == roleid);
            ViewData["canmangestates"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageStates == true && r.RoleId == roleid);
            ViewData["canmangelgas"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageLgas == true && r.RoleId == roleid);
            ViewData["canmangedrivers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageDrivers == true && r.RoleId == roleid);
            ViewData["canmangepassengersinformation"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManagePassengersInformation == true && r.RoleId == roleid);
            ViewData["candoeverything"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanDoEverything == true && r.RoleId == roleid);
            ViewData["canmanageapplicationusers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageApplicationUsers == true && r.RoleId == roleid);

            var twoWayTrips = _database.TwoWayTrips.Where(b => b.PaymentStatus == PaymentStatus.Processing);
            return View(await twoWayTrips.ToListAsync());
        }

        #endregion

        #region Paid Two Way Trip

        [HttpGet]
        [SessionExpireFilterAttribute]
        [Route("twowaytrip/paid")]
        public async Task<IActionResult> PaidTwoWayTrips()
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
            ViewData["canmangecarbrand"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageCarBrand == true && r.RoleId == roleid);
            ViewData["canmangeprices"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManagePrices == true && r.RoleId == roleid);
            ViewData["canmangeenquries"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageEnquires == true && r.RoleId == roleid);
            ViewData["canmangebookings"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageLandingDetails == true && r.RoleId == roleid);
            ViewData["canmangestates"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageStates == true && r.RoleId == roleid);
            ViewData["canmangelgas"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageLgas == true && r.RoleId == roleid);
            ViewData["canmangedrivers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageDrivers == true && r.RoleId == roleid);
            ViewData["canmangepassengersinformation"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManagePassengersInformation == true && r.RoleId == roleid);
            ViewData["candoeverything"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanDoEverything == true && r.RoleId == roleid);
            ViewData["canmanageapplicationusers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageApplicationUsers == true && r.RoleId == roleid);

            var twoWayTrips = _database.TwoWayTrips.Where(b => b.PaymentStatus == PaymentStatus.Paid);
            return View(await twoWayTrips.ToListAsync());
        }

        #endregion

        #region Unpaid Two way Trip

        [HttpGet]
        [SessionExpireFilterAttribute]
        [Route("twowaytrip/unpaid")]
        public async Task<IActionResult> UnpaidTwoWayTrips()
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
            ViewData["canmangecarbrand"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageCarBrand == true && r.RoleId == roleid);
            ViewData["canmangeprices"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManagePrices == true && r.RoleId == roleid);
            ViewData["canmangeenquries"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageEnquires == true && r.RoleId == roleid);
            ViewData["canmangebookings"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageLandingDetails == true && r.RoleId == roleid);
            ViewData["canmangestates"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageStates == true && r.RoleId == roleid);
            ViewData["canmangelgas"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageLgas == true && r.RoleId == roleid);
            ViewData["canmangedrivers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageDrivers == true && r.RoleId == roleid);
            ViewData["canmangepassengersinformation"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManagePassengersInformation == true && r.RoleId == roleid);
            ViewData["candoeverything"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanDoEverything == true && r.RoleId == roleid);
            ViewData["canmanageapplicationusers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageApplicationUsers == true && r.RoleId == roleid);

            var twoWayTrips = _database.TwoWayTrips.Where(b => b.PaymentStatus == PaymentStatus.Unpaid);
            return View(await twoWayTrips.ToListAsync());
        }

        #endregion

        #region Expired Two Way Trip

        [HttpGet]
        [SessionExpireFilterAttribute]
        [Route("twowaytrip/expired")]
        public async Task<IActionResult> ExpiredTwoWayTrips()
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
            ViewData["canmangecarbrand"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageCarBrand == true && r.RoleId == roleid);
            ViewData["canmangeprices"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManagePrices == true && r.RoleId == roleid);
            ViewData["canmangeenquries"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageEnquires == true && r.RoleId == roleid);
            ViewData["canmangebookings"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageLandingDetails == true && r.RoleId == roleid);
            ViewData["canmangestates"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageStates == true && r.RoleId == roleid);
            ViewData["canmangelgas"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageLgas == true && r.RoleId == roleid);
            ViewData["canmangedrivers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageDrivers == true && r.RoleId == roleid);
            ViewData["canmangepassengersinformation"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManagePassengersInformation == true && r.RoleId == roleid);
            ViewData["candoeverything"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanDoEverything == true && r.RoleId == roleid);
            ViewData["canmanageapplicationusers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageApplicationUsers == true && r.RoleId == roleid);

            var twoWayTrips = _database.TwoWayTrips.Where(b => b.PaymentStatus == PaymentStatus.Expired);
            return View(await twoWayTrips.ToListAsync());
        }

        #endregion

        #region Book

        [HttpGet]
        [Route("twowaytrip/book")]
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
        public async Task<IActionResult> Book(TwoWayTrip twoWayTrip)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Checks if there is a fixed price rate for the choosen destination
                    var price = await _database.Prices
                        .SingleOrDefaultAsync(p => p.PickUpLgaId == twoWayTrip.PickUpLgaId && p.DestinationLgaId == twoWayTrip.DestinationLgaId
                                           || p.PickUpLgaId == twoWayTrip.DestinationLgaId && p.DestinationLgaId == twoWayTrip.PickUpLgaId);

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

                        return View(twoWayTrip);
                    }

                    var _priceId = price.PriceId;
                    var _tripPrice = price.Amount;

                    var _bookTwoWayTrip = new TwoWayTrip()
                    {
                        Destination = twoWayTrip.Destination,
                        PickDate = twoWayTrip.PickDate,
                        PickUpTime = twoWayTrip.PickUpTime,
                        PickUpLgaId = twoWayTrip.PickUpLgaId,
                        PickUpLocation = twoWayTrip.PickUpLocation,

                        Verification = Verification.YetToReply,
                        PriceId = _priceId,
                        TotalBookingPrice = _tripPrice,
                        DateSent = DateTime.Now
                    };

                    _session.SetString("booktwowaytrip", JsonConvert.SerializeObject(_bookTwoWayTrip));
                    return RedirectToAction("SelectACar", "TwoWayTrip");
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return View();
        }

        #endregion

        #region Passenger Information

        [HttpGet]
        [Route("twowaytrip/passengerinformation")]
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

            if (allCars.Any(c => c.CarId == id))
            {
                var customerObject = _session.GetString("imouloggedincustomer");

                //This function is called if customer is signed in
                if (customerObject == null)
                {
                    var _booktwowaytrip = _session.GetString("booktwowaytrip");

                    if (_booktwowaytrip == null)
                    {
                        return RedirectToAction("Index", "Error");
                    }
                    var _booking = JsonConvert.DeserializeObject<TwoWayTrip>(_booktwowaytrip);

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


                var booktwowaytrip = _session.GetString("booktwowaytrip");

                if (booktwowaytrip == null)
                {
                    return RedirectToAction("Error", "Home");
                }
                var booking = JsonConvert.DeserializeObject<TwoWayTrip>(booktwowaytrip);

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

                    var _booktwowaytrip = _session.GetString("booktwowaytrip");

                    if (_booktwowaytrip != null)
                    {
                        var twoWayTrip = JsonConvert.DeserializeObject<TwoWayTrip>(_booktwowaytrip);
                        var car = await _database.Cars.FindAsync(id);
                        var carPrice = car.Price;

                        var stringGenerator = new RandomStringGenerator();
                        var bookingNumber = stringGenerator.RandomString(8);

                        var saveBooking = new TwoWayTrip()
                        {
                            DateSent = twoWayTrip.DateSent,
                            DestinationLgaId = twoWayTrip.DestinationLgaId,
                            Destination = twoWayTrip.Destination,
                            ReturnTripDate = twoWayTrip.ReturnTripDate,
                            ReturnTripTime = twoWayTrip.ReturnTripTime,
                            PickDate = twoWayTrip.PickDate,
                            PickUpTime = twoWayTrip.PickUpTime,
                            PickUpLgaId = twoWayTrip.PickUpLgaId,
                            PickUpLocation = twoWayTrip.PickUpLocation,
                            Verification = Verification.YetToReply,
                            CarId = Convert.ToInt32(id),
                            TotalBookingPrice = twoWayTrip.TotalBookingPrice + carPrice,
                            PriceId = twoWayTrip.PriceId,
                            PassengerInformationId = _passengerInformations.PassengerInformationId,
                            PassengerInformation = _passengerInformations,
                            CustomerId = _customer.CustomerId,
                            BookingNumber = bookingNumber,
                            PaymentStatus = PaymentStatus.Processing
                        };

                        var getcarname = await _database.Cars.FindAsync(saveBooking.CarId);

                        _session.SetString("successrequestedcarname", getcarname.Name);
                        _session.SetString("successpassengeremail", _passengerInformations.Email);

                        await _database.TwoWayTrips.AddAsync(saveBooking);
                        await _database.SaveChangesAsync();

                        return RedirectToAction("Success", "TwoWayTrip");
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

                        return RedirectToAction("Book", "TwoWayTrip");
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

                    var _booktwowaytrip = _session.GetString("booktwowaytrip");

                    if (_booktwowaytrip != null)
                    {
                        var twoWayTrip = JsonConvert.DeserializeObject<TwoWayTrip>(_booktwowaytrip);
                        var car = await _database.Cars.FindAsync(id);
                        var carPrice = car.Price;

                        //Randomly generate booking number
                        var stringGenerator = new RandomStringGenerator();
                        var bookingNumber = stringGenerator.RandomString(8);

                        var saveBooking = new TwoWayTrip()
                        {
                            DateSent = twoWayTrip.DateSent,
                            DestinationLgaId = twoWayTrip.DestinationLgaId,
                            Destination = twoWayTrip.Destination,
                            ReturnTripDate = twoWayTrip.ReturnTripDate,
                            ReturnTripTime = twoWayTrip.ReturnTripTime,
                            PickDate = twoWayTrip.PickDate,
                            PickUpTime = twoWayTrip.PickUpTime,
                            PickUpLgaId = twoWayTrip.PickUpLgaId,
                            PickUpLocation = twoWayTrip.PickUpLocation,
                            Verification = Verification.YetToReply,
                            CarId = Convert.ToInt32(id),
                            TotalBookingPrice = twoWayTrip.TotalBookingPrice + carPrice,
                            PriceId = twoWayTrip.PriceId,
                            PassengerInformationId = passenger.PassengerInformationId,
                            PassengerInformation = passenger,
                            BookingNumber = bookingNumber,
                            PaymentStatus = PaymentStatus.Processing
                        };

                        var getcarname = await _database.Cars.FindAsync(saveBooking.CarId);

                        _session.SetString("successrequestedcarname", getcarname.Name);
                        _session.SetString("successpassengeremail", passenger.Email);


                        await _database.TwoWayTrips.AddAsync(saveBooking);
                        await _database.SaveChangesAsync();

                        return RedirectToAction("Success", "TwoWayTrip");
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

                var booktwowaytrip = _session.GetString("booktwowaytrip");

                if (booktwowaytrip != null)
                {
                    var twoWayTrip = JsonConvert.DeserializeObject<TwoWayTrip>(booktwowaytrip);
                    var car = await _database.Cars.FindAsync(id);
                    var carPrice = car.Price;

                    //Randomly generate booking number
                    var stringGenerator = new RandomStringGenerator();
                    var bookingNumber = stringGenerator.RandomString(8);

                    var saveBooking = new TwoWayTrip()
                    {
                        DateSent = twoWayTrip.DateSent,
                        DestinationLgaId = twoWayTrip.DestinationLgaId,
                        Destination = twoWayTrip.Destination,
                        ReturnTripTime = twoWayTrip.ReturnTripTime,
                        ReturnTripDate = twoWayTrip.ReturnTripDate,
                        PickUpTime = twoWayTrip.PickUpTime,
                        PickDate = twoWayTrip.PickDate,
                        PickUpLgaId = twoWayTrip.PickUpLgaId,
                        PickUpLocation = twoWayTrip.PickUpLocation,
                        Verification = Verification.YetToReply,
                        CarId = Convert.ToInt32(id),
                        TotalBookingPrice = twoWayTrip.TotalBookingPrice + carPrice,
                        PriceId = twoWayTrip.PriceId,
                        PassengerInformationId = _passengerInformation.PassengerInformationId,
                        PassengerInformation = _passengerInformation,
                        BookingNumber = bookingNumber,
                        PaymentStatus = PaymentStatus.Processing
                    };

                    await _database.TwoWayTrips.AddAsync(saveBooking);
                    await _database.SaveChangesAsync();

                    var getcarname = await _database.Cars.FindAsync(saveBooking.CarId);

                    _session.SetString("successrequestedcarname", getcarname.Name);
                    _session.SetString("successpassengeremail", _passengerInformation.Email);

                    return RedirectToAction("Success", "TwoWayTrip");
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

        #region SelectACar

        [HttpGet]
        [Route("twowaytrip/selectacar")]
        public async Task<IActionResult> SelectACar()
        {
            var trial = _session.GetString("booktwowaytrip");

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

        #region Success

        [HttpGet]
        [Route("twowaytrip/successfullbooking")]
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
                return RedirectToAction("Index", "Error");
            }

            return View();
        }

        #endregion

        #region Cancel

        [HttpGet]
        [Route("twowaytrip/cancelbooking")]
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
            if (role.CanManageApplicationUsers == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return NotFound();
            }

            var twoWayTrip = await _database.TwoWayTrips.SingleOrDefaultAsync(b => b.TwoWayTripId == id);

            if (twoWayTrip == null)
            {
                return NotFound();
            }

            return PartialView("Approve", twoWayTrip);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Approve(int id, TwoWayTrip twoWayTrip)
        {
            if (id != twoWayTrip.TwoWayTripId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    twoWayTrip.DateVerified = DateTime.Now;
                    twoWayTrip.VerifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));
                    twoWayTrip.Verification = Verification.Approve;

                    _database.TwoWayTrips.Update(twoWayTrip);
                    await _database.SaveChangesAsync();

                    TempData["twowaytrip"] = "You have successfully verified a booking request. Next Assign A Driver ";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TwoWayTripExists(twoWayTrip.TwoWayTripId))
                    {
                        return RedirectToAction("Index", "Error");
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View("ProcessingTwoWayTrip");
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
            if (role.CanManageApplicationUsers == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return NotFound();
            }

            var twoWayTrip = await _database.TwoWayTrips.SingleOrDefaultAsync(b => b.TwoWayTripId == id);

            if (twoWayTrip == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Disapprove", twoWayTrip);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Disapprove(int id, TwoWayTrip twoWayTrip)
        {
            if (id != twoWayTrip.TwoWayTripId)
            {
                return RedirectToAction("Index", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    twoWayTrip.DateVerified = DateTime.Now;
                    twoWayTrip.VerifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));
                    twoWayTrip.Verification = Verification.Disapprove;

                    _database.TwoWayTrips.Update(twoWayTrip);
                    await _database.SaveChangesAsync();

                    TempData["twowaytrip"] = "You have successfully disappoved a two way trip.";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TwoWayTripExists(twoWayTrip.TwoWayTripId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View("ProcessingTwoWayTrip");
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
            if (role.CanManageApplicationUsers == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var twoWayTrip = await _database.TwoWayTrips.SingleOrDefaultAsync(b => b.TwoWayTripId == id);

            if (twoWayTrip == null)
            {
                return NotFound();
            }

            ViewBag.DriverId = new SelectList(_database.Driver, "DriverId", "DisplayName");
            return PartialView("AssignDriver", twoWayTrip);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> AssignDriver(int id, TwoWayTrip twoWayTrip)
        {
            if (id != twoWayTrip.TwoWayTripId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    twoWayTrip.DateDriverAssigned = DateTime.Now;
                    twoWayTrip.DriverAssignedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));

                    _database.TwoWayTrips.Update(twoWayTrip);
                    await _database.SaveChangesAsync();

                    TempData["twowaytrip"] = "You have successfully assigned a driver to the a two way trip. Next Send Link";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TwoWayTripExists(twoWayTrip.TwoWayTripId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewBag.DriverId = new SelectList(_database.Driver, "DriverId", "Name", twoWayTrip.DriverId);
            return View("ProcessingTwoWayTrip");
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
            if (role.CanManageApplicationUsers == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var twoWayTrip = await _database.TwoWayTrips.SingleOrDefaultAsync(b => b.TwoWayTripId == id);

            if (twoWayTrip == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("RemoveDriver", twoWayTrip);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> RemoveDriver(int id, TwoWayTrip twoWayTrip)
        {
            if (id != twoWayTrip.TwoWayTripId)
            {
                return RedirectToAction("Index", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    twoWayTrip.DriverId = 0;
                    twoWayTrip.DateDriverAssigned = DateTime.Now;
                    twoWayTrip.DriverAssignedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));

                    _database.TwoWayTrips.Update(twoWayTrip);
                    await _database.SaveChangesAsync();

                    TempData["twowaytrips"] = "You have successfully removed the driver of a two way trip. Next Assign Driver";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TwoWayTripExists(twoWayTrip.TwoWayTripId))
                    {
                        return RedirectToAction("Index", "Error");
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View("ProcessingTwoWayTrip");
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
            if (role.CanManageApplicationUsers == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var twoWayTrip = await _database.TwoWayTrips.SingleOrDefaultAsync(b => b.TwoWayTripId == id);

            if (twoWayTrip == null)
            {
                return NotFound();
            }

            var passengerId = twoWayTrip.PassengerInformationId;
            var _passengerDetails = await _database.PassengersInformation.FindAsync(passengerId);

            ViewData["passengername"] = _passengerDetails.DisplayName;
            ViewData["passengeremail"] = _passengerDetails.Email;

            return PartialView("SendLink", twoWayTrip);
        }

        [HttpPost]
        public async Task<IActionResult> SendALink(int id, TwoWayTrip twoWayTrip)
        {
            if (id != twoWayTrip.TwoWayTripId)
            {
                return RedirectToAction("Index", "Error");
            }


            var _twoWayTrip = await _database.TwoWayTrips.SingleOrDefaultAsync(b => b.TwoWayTripId == id);

            var passengerId = _twoWayTrip.PassengerInformationId;
            var _passenger = await _database.PassengersInformation.FindAsync(passengerId);

            twoWayTrip.Verification = Verification.LinkSent;
            twoWayTrip.PaymentStatus = PaymentStatus.Unpaid;

            _database.TwoWayTrips.Update(twoWayTrip);
            await _database.SaveChangesAsync();

            var car = await _database.Cars.SingleOrDefaultAsync(c => c.CarId == _twoWayTrip.CarId);

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

            //new Mailer().BookingPaymentEmail(new AppConfig().BookingPaymentHtml, twoWayTrip, _passenger);

            return View();
        }

        #endregion

        #region Payment

        [HttpGet]
        public async Task<IActionResult> Payment(string bookingNumber)
        {
            var twoWayTrip = await _database.TwoWayTrips.Where(b => b.PaymentStatus == PaymentStatus.Unpaid).ToListAsync();

            var _twoWayTrip = twoWayTrip.SingleOrDefault(b => b.BookingNumber == bookingNumber);

            if (_twoWayTrip == null)
            {
                TempData["expiredbooking"] = "Dear customer your pickup time has passed and so there your booking has expired.";
                return RedirectToAction("Index", "Error");
            }

            //Get Car
            var carid = _twoWayTrip.CarId;
            var _car = await _database.Cars.FindAsync(carid);

            //Get Car Brand
            var brandid = _car.CarBrandId;
            var _brand = await _database.CarBrands.FindAsync(brandid);


            TempData["carimage"] = _car.Image;
            TempData["carbrand"] = _brand.Name;
            TempData["carname"] = _car.Name;
            TempData["carprice"] = _car.Price;
            TempData["pickuplocation"] = _twoWayTrip.PickUpLocation;
            TempData["returnlocation"] = _twoWayTrip.Destination;
            TempData["destinationprice"] = _twoWayTrip.TotalBookingPrice;
            TempData["totalprice"] = _twoWayTrip.TotalBookingPrice;

            return View();
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
            if (role.CanManageApplicationUsers == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return NotFound();
            }

            var twoWayTrip = await _database.TwoWayTrips.SingleOrDefaultAsync(b => b.TwoWayTripId == id);

            if (twoWayTrip == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", twoWayTrip);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var twoWayTrip = await _database.TwoWayTrips.SingleOrDefaultAsync(b => b.TwoWayTripId == id);

            if (twoWayTrip != null)
            {
                _database.TwoWayTrips.Remove(twoWayTrip);
                await _database.SaveChangesAsync();

                TempData["twowaytrip"] = "You have successfully deleted a paid one way trip!!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return View("PaidTwoWayTrips");
        }

        #endregion

        #region One Way Trip Exists

        private bool TwoWayTripExists(int id)
        {
            return _database.TwoWayTrips.Any(e => e.TwoWayTripId == id);
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