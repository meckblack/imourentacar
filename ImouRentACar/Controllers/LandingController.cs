using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImouRentACar.Data;
using ImouRentACar.Models;
using ImouRentACar.Models.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ImouRentACar.Controllers
{
    public class LandingController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IHostingEnvironment _environment;

        #region Constructor

        public LandingController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHostingEnvironment environment)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
        }

        #endregion

        #region Index

        public IActionResult Index()
        {
            //Counters
            ViewData["carbrandcounter"] = _database.CarBrands.Count();
            ViewData["caravaliablecounter"] = _database.Cars.Where(c => c.CarAvaliability == CarAvaliability.Avaliable).Count();
            ViewData["carrentedout"] = _database.Cars.Where(c => c.CarAvaliability == CarAvaliability.Rented).Count();
            ViewData["contactcounter"] = _database.Contacts.Count();
            ViewData["enquirycounter"] = _database.Enquiries.Count();

            var userObject = _session.GetString("imouloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);

            ViewData["loggedinuserfullname"] = _user.DisplayName;
            ViewData["loggedinuseremail"] = _user.Email;

            dynamic mymodel = new ExpandoObject();
            mymodel.Logos = GetLogos();
            mymodel.Headers = GetHeaders();
            mymodel.AboutUsImages = GetAboutUsImage();
            mymodel.AboutUsImageTwos = GetAboutUsImageTwo();
            mymodel.Contacts = GetContacts();
            
            ViewData["logochecker"] = _database.Logos.Count();
            ViewData["headerchecker"] = _database.Headers.Count();
            ViewData["aboutusimagechecker"] = _database.AboutUsImages.Count();
            ViewData["aboutusimagetwochecker"] = _database.AboutUsImageTwos.Count();
            ViewData["contactchecker"] = _database.Contacts.Count();
            
            return View(mymodel);
            //return View();
        }

        #endregion
        
        #region Exist

        private bool LogoExists(int id)
        {
            return _database.Logos.Any(e => e.LogoId == id);
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

        #region Get Contact Details

        private List<Contact> GetContacts()
        {
            var _contacts = _database.Contacts.ToList();
            return _contacts;
        }

        #endregion

    }
}