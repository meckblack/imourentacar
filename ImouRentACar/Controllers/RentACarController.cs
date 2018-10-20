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
    public class RentACarController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public RentACarController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Processing Two Way Trip

        [HttpGet]
        [SessionExpireFilterAttribute]
        [Route("rentacar/processing")]
        public async Task<IActionResult> ProcessingRentACar()
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

            var rentACar = _database.RentACars.Where(b => b.PaymentStatus == PaymentStatus.Processing);
            return View(await rentACar.ToListAsync());
        }

        #endregion

        #region Paid Two Way Trip

        [HttpGet]
        [SessionExpireFilterAttribute]
        [Route("rentacar/paid")]
        public async Task<IActionResult> PaidRentACar()
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

            var rentACar = _database.RentACars.Where(b => b.PaymentStatus == PaymentStatus.Paid);
            return View(await rentACar.ToListAsync());
        }

        #endregion

        #region Unpaid Two way Trip

        [HttpGet]
        [SessionExpireFilterAttribute]
        [Route("rentacar/unpaid")]
        public async Task<IActionResult> UnpaidRentACar()
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

            var rentACar = _database.RentACars.Where(b => b.PaymentStatus == PaymentStatus.Unpaid);
            return View(await rentACar.ToListAsync());
        }

        #endregion

        #region Expired Two Way Trip

        [HttpGet]
        [SessionExpireFilterAttribute]
        [Route("rentacar/expired")]
        public async Task<IActionResult> ExpiredRentACar()
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

            var rentACar = _database.RentACars.Where(b => b.PaymentStatus == PaymentStatus.Expired);
            return View(await rentACar.ToListAsync());
        }

        #endregion

        #region Book

        [HttpGet]
        [Route("rentacar/book")]
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
        public IActionResult Book(RentACar rentACar)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var _rentACar = new RentACar()
                    {
                        PickDate = rentACar.PickDate,
                        PickUpTime = rentACar.PickUpTime,
                        PickUpLgaId = rentACar.PickUpLgaId,
                        PickUpLocation = rentACar.PickUpLocation,
                        Days = rentACar.Days,
                        Verification = Verification.YetToReply,
                        DateSent = DateTime.Now
                    };

                    _session.SetString("bookacar", JsonConvert.SerializeObject(_rentACar));
                    return RedirectToAction("SelectACar", "RentACar");
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
        [Route("rentacar/passengerinformation")]
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
                    var _bookacar = _session.GetString("bookacar");

                    if (_bookacar == null)
                    {
                        return RedirectToAction("Index", "Error");
                    }
                    var _booking = JsonConvert.DeserializeObject<RentACar>(_bookacar);

                    //Get Car
                    var _carid = id;
                    var car = await _database.Cars.FindAsync(_carid);

                    //Get Car Brand
                    var _brandid = car.CarBrandId;
                    var brand = await _database.CarBrands.FindAsync(_brandid);


                    TempData["carimage"] = car.Image;
                    TempData["carbrand"] = brand.Name;
                    TempData["carname"] = car.Name;
                    TempData["carprice"] = car.RentalPrice;
                    TempData["pickuplocation"] = _booking.PickUpLocation;
                    TempData["numberofdays"] = _booking.Days;
                    TempData["destinationprice"] = _booking.TotalBookingPrice;
                    TempData["totalprice"] = _booking.TotalBookingPrice + car.RentalPrice;
                    return View();
                }

                var _customer = JsonConvert.DeserializeObject<Customer>(customerObject);
                TempData["customername"] = _customer.DisplayName;


                var bookACar = _session.GetString("bookacar");

                if (bookACar == null)
                {
                    return RedirectToAction("Error", "Home");
                }
                var booking = JsonConvert.DeserializeObject<RentACar>(bookACar);

                //Get Car
                var carid = id;
                var _car = await _database.Cars.FindAsync(carid);

                //Get Car Brand
                var brandid = _car.CarBrandId;
                var _brand = await _database.CarBrands.FindAsync(brandid);


                TempData["carimage"] = _car.Image;
                TempData["carbrand"] = _brand.Name;
                TempData["carname"] = _car.Name;
                TempData["carprice"] = _car.RentalPrice;
                TempData["pickuplocation"] = booking.PickUpLocation;
                TempData["numberofdays"] = booking.Days;
                TempData["totalprice"] = _car.RentalPrice * booking.Days;

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

                    var _bookacar = _session.GetString("bookacar");

                    if (_bookacar != null)
                    {
                        var rentACar = JsonConvert.DeserializeObject<RentACar>(_bookacar);
                        var car = await _database.Cars.FindAsync(id);
                        var carPrice = car.RentalPrice;

                        var stringGenerator = new RandomStringGenerator();
                        var bookingNumber = stringGenerator.RandomString(8);

                        var saveBooking = new RentACar()
                        {
                            DateSent = rentACar.DateSent,
                            PickDate = rentACar.PickDate,
                            PickUpTime = rentACar.PickUpTime,
                            PickUpLgaId = rentACar.PickUpLgaId,
                            PickUpLocation = rentACar.PickUpLocation,
                            Verification = Verification.YetToReply,
                            CarId = Convert.ToInt32(id),
                            TotalBookingPrice = carPrice * rentACar.Days,
                            PassengerInformationId = _passengerInformations.PassengerInformationId,
                            PassengerInformation = _passengerInformations,
                            CustomerId = _customer.CustomerId,
                            BookingNumber = bookingNumber,
                            PaymentStatus = PaymentStatus.Processing
                        };

                        var getcarname = await _database.Cars.FindAsync(saveBooking.CarId);

                        _session.SetString("successrequestedcarname", getcarname.Name);
                        _session.SetString("successpassengeremail", _passengerInformations.Email);

                        await _database.RentACars.AddAsync(saveBooking);
                        await _database.SaveChangesAsync();

                        return RedirectToAction("Success", "RentACar");
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

                        return RedirectToAction("Book", "RentACar");
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

                    var _bookacar = _session.GetString("bookacar");

                    if (_bookacar != null)
                    {
                        var rentACar = JsonConvert.DeserializeObject<RentACar>(_bookacar);
                        var car = await _database.Cars.FindAsync(id);
                        var carPrice = car.RentalPrice;

                        //Randomly generate booking number
                        var stringGenerator = new RandomStringGenerator();
                        var bookingNumber = stringGenerator.RandomString(8);

                        var saveBooking = new RentACar()
                        {
                            DateSent = rentACar.DateSent,
                            PickDate = rentACar.PickDate,
                            PickUpTime = rentACar.PickUpTime,
                            PickUpLgaId = rentACar.PickUpLgaId,
                            PickUpLocation = rentACar.PickUpLocation,
                            Verification = Verification.YetToReply,
                            CarId = Convert.ToInt32(id),
                            TotalBookingPrice = carPrice * rentACar.Days,
                            PriceId = rentACar.PriceId,
                            PassengerInformationId = passenger.PassengerInformationId,
                            PassengerInformation = passenger,
                            BookingNumber = bookingNumber,
                            PaymentStatus = PaymentStatus.Processing
                        };

                        var getcarname = await _database.Cars.FindAsync(saveBooking.CarId);

                        _session.SetString("successrequestedcarname", getcarname.Name);
                        _session.SetString("successpassengeremail", passenger.Email);


                        await _database.RentACars.AddAsync(saveBooking);
                        await _database.SaveChangesAsync();

                        return RedirectToAction("Success", "RentACar");
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

                var bookacar = _session.GetString("bookacar");

                if (bookacar != null)
                {
                    var rentACar = JsonConvert.DeserializeObject<RentACar>(bookacar);
                    var car = await _database.Cars.FindAsync(id);
                    var carPrice = car.RentalPrice;

                    //Randomly generate booking number
                    var stringGenerator = new RandomStringGenerator();
                    var bookingNumber = stringGenerator.RandomString(8);

                    var saveBooking = new RentACar()
                    {
                        PickUpTime = rentACar.PickUpTime,
                        PickDate = rentACar.PickDate,
                        PickUpLgaId = rentACar.PickUpLgaId,
                        PickUpLocation = rentACar.PickUpLocation,
                        Verification = Verification.YetToReply,
                        CarId = Convert.ToInt32(id),
                        TotalBookingPrice = carPrice * rentACar.Days,
                        PriceId = rentACar.PriceId,
                        PassengerInformationId = _passengerInformation.PassengerInformationId,
                        PassengerInformation = _passengerInformation,
                        BookingNumber = bookingNumber,
                        PaymentStatus = PaymentStatus.Processing
                    };

                    await _database.RentACars.AddAsync(saveBooking);
                    await _database.SaveChangesAsync();

                    var getcarname = await _database.Cars.FindAsync(saveBooking.CarId);

                    _session.SetString("successrequestedcarname", getcarname.Name);
                    _session.SetString("successpassengeremail", _passengerInformation.Email);

                    return RedirectToAction("Success", "RentACar");
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
        [Route("rentacar/selectacar")]
        public async Task<IActionResult> SelectACar()
        {
            var trial = _session.GetString("bookacar");

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
        [Route("rentacar/successfullbooking")]
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
        [Route("rentacar/cancelbooking")]
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

            var rentACar = await _database.RentACars.SingleOrDefaultAsync(b => b.RentACarId == id);

            if (rentACar == null)
            {
                return NotFound();
            }

            return PartialView("Approve", rentACar);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Approve(int id, RentACar rentACar)
        {
            if (id != rentACar.RentACarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    rentACar.DateVerified = DateTime.Now;
                    rentACar.VerifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));
                    rentACar.Verification = Verification.Approve;

                    _database.RentACars.Update(rentACar);
                    await _database.SaveChangesAsync();

                    TempData["rentacar"] = "You have successfully verified a booking request. Next Assign A Driver ";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentACarExists(rentACar.RentACarId))
                    {
                        return RedirectToAction("Index", "Error");
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View("ProcessingRentACar");
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

            var rentACar = await _database.RentACars.SingleOrDefaultAsync(b => b.RentACarId == id);

            if (rentACar == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Disapprove", rentACar);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Disapprove(int id, RentACar rentACar)
        {
            if (id != rentACar.RentACarId)
            {
                return RedirectToAction("Index", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    rentACar.DateVerified = DateTime.Now;
                    rentACar.VerifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));
                    rentACar.Verification = Verification.Disapprove;

                    _database.RentACars.Update(rentACar);
                    await _database.SaveChangesAsync();

                    TempData["rentacar"] = "You have successfully disappoved a two way trip.";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentACarExists(rentACar.RentACarId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View("ProcessingRentACar");
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

            var rentACar = await _database.RentACars.SingleOrDefaultAsync(b => b.RentACarId == id);

            if (rentACar == null)
            {
                return NotFound();
            }

            ViewBag.DriverId = new SelectList(_database.Driver, "DriverId", "DisplayName");
            return PartialView("AssignDriver", rentACar);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> AssignDriver(int id, RentACar rentACar)
        {
            if (id != rentACar.RentACarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    rentACar.DateDriverAssigned = DateTime.Now;
                    rentACar.DriverAssignedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));

                    _database.RentACars.Update(rentACar);
                    await _database.SaveChangesAsync();

                    TempData["rentacar"] = "You have successfully assigned a driver to the a two way trip. Next Send Link";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentACarExists(rentACar.RentACarId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewBag.DriverId = new SelectList(_database.Driver, "DriverId", "Name", rentACar.DriverId);
            return View("ProcessingRentACar");
        }

        #endregion

        #region Remove Driver From Rent A Car

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

            var rentACar = await _database.RentACars.SingleOrDefaultAsync(b => b.RentACarId == id);

            if (rentACar == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("RemoveDriver", rentACar);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> RemoveDriver(int id, RentACar rentACar)
        {
            if (id != rentACar.RentACarId)
            {
                return RedirectToAction("Index", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    rentACar.DriverId = 0;
                    rentACar.DateDriverAssigned = DateTime.Now;
                    rentACar.DriverAssignedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));

                    _database.RentACars.Update(rentACar);
                    await _database.SaveChangesAsync();

                    TempData["rentacar"] = "You have successfully removed the driver of a car rental request. Next Assign Driver";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentACarExists(rentACar.RentACarId))
                    {
                        return RedirectToAction("Index", "Error");
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View("ProcessingRentACar");
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

            var rentACar = await _database.RentACars.SingleOrDefaultAsync(b => b.RentACarId == id);

            if (rentACar == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var passengerId = rentACar.PassengerInformationId;
            var _passengerDetails = await _database.PassengersInformation.FindAsync(passengerId);

            ViewData["passengername"] = _passengerDetails.DisplayName;
            ViewData["passengeremail"] = _passengerDetails.Email;

            return PartialView("SendLink", rentACar);
        }

        [HttpPost]
        public async Task<IActionResult> SendALink(int id, RentACar rentACar)
        {
            if (id != rentACar.RentACarId)
            {
                return RedirectToAction("Index", "Error");
            }


            var _rentACar = await _database.RentACars.SingleOrDefaultAsync(b => b.RentACarId == id);

            var passengerId = _rentACar.PassengerInformationId;
            var _passenger = await _database.PassengersInformation.FindAsync(passengerId);

            rentACar.Verification = Verification.LinkSent;
            rentACar.PaymentStatus = PaymentStatus.Unpaid;

            _database.RentACars.Update(rentACar);
            await _database.SaveChangesAsync();

            var car = await _database.Cars.SingleOrDefaultAsync(c => c.CarId == _rentACar.CarId);

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

            new Mailer().RentACarPaymentEmail(new AppConfig().BookingPaymentHtml, rentACar, _passenger);

            return View();
        }

        #endregion

        #region Payment

        [HttpGet]
        [Route("rentacar/payment")]
        public async Task<IActionResult> Payment(string bookingNumber)
        {
            var rentACar = await _database.RentACars.Where(b => b.PaymentStatus == PaymentStatus.Unpaid).ToListAsync();

            var _rentACar = rentACar.SingleOrDefault(b => b.BookingNumber == bookingNumber);

            if (_rentACar == null)
            {
                TempData["expiredbooking"] = "Dear customer your pickup time has passed and so there your booking has expired.";
                return RedirectToAction("Index", "Error");
            }

            //Get Car
            var carid = _rentACar.CarId;
            var _car = await _database.Cars.FindAsync(carid);

            //Get Car Brand
            var brandid = _car.CarBrandId;
            var _brand = await _database.CarBrands.FindAsync(brandid);


            TempData["carimage"] = _car.Image;
            TempData["carbrand"] = _brand.Name;
            TempData["carname"] = _car.Name;
            TempData["rentalprice"] = _car.RentalPrice;
            TempData["numberofdays"] = _rentACar.Days;
            TempData["pickuplocation"] = _rentACar.PickUpLocation;
            TempData["totalprice"] = _rentACar.TotalBookingPrice;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PaymentSuccessful(string bookingNumber)
        {
            var rentACar = await _database.RentACars.Where(b => b.PaymentStatus == PaymentStatus.Unpaid).ToListAsync();

            var _rentACar = rentACar.SingleOrDefault(b => b.BookingNumber == bookingNumber);

            var rent = new RentACar()
            {
                RentACarId = _rentACar.RentACarId,
                BookingNumber = _rentACar.BookingNumber,
                CarId = _rentACar.CarId,
                CustomerId = _rentACar.CustomerId,
                DateDriverAssigned = _rentACar.DateDriverAssigned,
                DateSent = _rentACar.DateSent,
                DateVerified = _rentACar.DateVerified,
                Days = _rentACar.Days,
                DriverAssignedBy = _rentACar.DriverAssignedBy,
                DriverId = _rentACar.DriverId,
                PickDate = _rentACar.PickDate,
                PassengerInformation = _rentACar.PassengerInformation,
                PaymentStatus = PaymentStatus.Paid,
                PickUpLgaId = _rentACar.PickUpLgaId,
                PassengerInformationId = _rentACar.PassengerInformationId,
                PickUpLocation = _rentACar.PickUpLocation,
                PickUpTime = _rentACar.PickUpTime,
                PriceId = _rentACar.PriceId,
                TotalBookingPrice = _rentACar.TotalBookingPrice,
                Verification = _rentACar.Verification,
                VerifiedBy = _rentACar.VerifiedBy,
            };

            _database.RentACars.Update(rent);
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
            if (role.CanManageApplicationUsers == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return NotFound();
            }

            var rentACar = await _database.RentACars.SingleOrDefaultAsync(b => b.RentACarId == id);

            if (rentACar == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", rentACar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var rentACar = await _database.RentACars.SingleOrDefaultAsync(b => b.RentACarId == id);

            if (rentACar != null)
            {
                _database.RentACars.Remove(rentACar);
                await _database.SaveChangesAsync();

                TempData["rentacar"] = "You have successfully deleted a car rental request!!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return View("PaidRentACar");
        }

        #endregion

        #region Completed

        [HttpGet]
        [SessionExpireFilter]
        public async Task<IActionResult> Completed(int? id)
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

            var rentACar = await _database.RentACars.SingleOrDefaultAsync(b => b.RentACarId == id);

            if (rentACar == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Complete", rentACar);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Complete(int id, RentACar rentACar)
        {
            if (id != rentACar.RentACarId)
            {
                return RedirectToAction("Index", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    rentACar.DateVerified = DateTime.Now;
                    rentACar.VerifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));
                    rentACar.Verification = Verification.Approve;

                    _database.RentACars.Update(rentACar);
                    await _database.SaveChangesAsync();

                    var car = await _database.Cars.SingleOrDefaultAsync(c => c.CarId == rentACar.CarId);

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

                    TempData["rentacar"] = "Car rental has been completed";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentACarExists(rentACar.RentACarId))
                    {
                        return RedirectToAction("Index", "Error");
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View("ProcessingRentACar");
        }

        #endregion

        #region Rent A Car Exists

        private bool RentACarExists(int id)
        {
            return _database.RentACars.Any(e => e.RentACarId == id);
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