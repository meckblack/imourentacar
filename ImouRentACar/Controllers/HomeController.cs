using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ImouRentACar.Models;
using ImouRentACar.Data;
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Hangfire;
using ImouRentACar.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace ImouRentACar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public HomeController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        //public void RestOneWayTripCars()
        //{
        //    var _expiredOneWayTrips = _database.OneWayTrips.Where(e => e.PickDate < DateTime.Now.Date && e.PickUpTime.TimeOfDay < DateTime.Now.TimeOfDay && e.PaymentStatus == PaymentStatus.Unpaid);
            
        //    foreach(var item in _expiredOneWayTrips.ToList())
        //    {
        //        var car = _database.Cars.Find(item.CarId);

        //        car.CarAvaliability = Avaliability.Avaliable;

        //        _database.Entry(car).State = EntityState.Modified;
        //        _database.SaveChanges();
        //    }
            
        //}

        //public void ExpiredOneWayTrip()

        //{
        //    var _expiredOneWayTrip = _database.OneWayTrips.Where(e => e.PickDate < DateTime.Now.Date && e.PaymentStatus == PaymentStatus.Unpaid);

        //    foreach(var item in _expiredOneWayTrip.ToList())
        //    {
        //        var onewaytrip = _database.OneWayTrips.Find(item.OneWayTripId);

        //        onewaytrip.PaymentStatus = PaymentStatus.Expired;

        //        _database.Entry(onewaytrip).State = EntityState.Modified;
        //        _database.SaveChanges();
        //    }
        //}



        #region Index

        public IActionResult Index()
        {
            //BackgroundJob.Schedule(() => RestOneWayTripCars(), TimeSpan.FromHours(1));
            
            //BackgroundJob.Enqueue(() => ExpiredOneWayTrip());


            dynamic mymodel = new ExpandoObject();
            mymodel.Logos = GetLogos();
            mymodel.Headers = GetHeaders();
            mymodel.AboutUsImages = GetAboutUsImage();
            mymodel.AboutUsImageTwos = GetAboutUsImageTwo();
            mymodel.Contacts = GetContacts();
            mymodel.Policies = GetPolicies();


            foreach(Policy policy in mymodel.Policies)
            {
                ViewData["policyid"] = policy.PolicyId;
            }

            foreach (Header header in mymodel.Headers)
            {
                ViewData["imageofheader"] = header.Image;
                ViewData["bodyofheader"] = header.Body;
                ViewData["headofheader"] = header.Heading;
            }

            foreach (AboutUsImage aboutUsImage in mymodel.AboutUsImages)
            {
                ViewData["imageofaboutus"] = aboutUsImage.Image;
                ViewData["headingofaboutus"] = aboutUsImage.Heading;
                ViewData["bodyofaboutus"] = aboutUsImage.Body;
            }

            foreach (AboutUsImageTwo aboutUsImageTwo in mymodel.AboutUsImageTwos)
            {
                ViewData["imageofaboutustwo"] = aboutUsImageTwo.Image;
                ViewData["headingofaboutustwo"] = aboutUsImageTwo.Heading;
                ViewData["bodyofaboutustwo"] = aboutUsImageTwo.Body;
            }

            foreach (Contact contact in mymodel.Contacts)
            {
                ViewData["contactnumber"] = contact.MobileNumberOne;
            }

            foreach (Logo logo in mymodel.Logos)
            {
                ViewData["imageoflogo"] = logo.Image;
            }

            var customerObject = _session.GetString("imouloggedincustomer");
            if(customerObject == null)
            {
                return View();
            }

            var _customer = JsonConvert.DeserializeObject<Customer>(customerObject);
            TempData["customername"] = _customer.DisplayName;


            return View();
        }

        #endregion
        
        #region Get Logos

        private List<Logo> GetLogos()
        {
            var _logos = _database.Logos.ToList();

            return _logos;
        }

        #endregion

        #region Get Headers

        private List<Header> GetHeaders()
        {
            var _headers = _database.Headers.ToList();
            return _headers;
        }

        #endregion

        #region Get AboutUsImage

        private List<AboutUsImage> GetAboutUsImage()
        {
            var _aboutUsImage = _database.AboutUsImages.ToList();
            return _aboutUsImage;
        }

        #endregion

        #region Get AboutUsImageTwo

        private List<AboutUsImageTwo> GetAboutUsImageTwo()
        {
            var _aboutUsImageTwo = _database.AboutUsImageTwos.ToList();
            return _aboutUsImageTwo;
        }

        #endregion

        #region Get Contacts

        private List<Contact> GetContacts()
        {
            var _contact = _database.Contacts.ToList();
            return _contact;
        }

        #endregion

        #region Get Policy

        private List<Policy> GetPolicies()
        {
            var _policy = _database.Policies.ToList();
            return _policy;
        }

        #endregion
    }
}
