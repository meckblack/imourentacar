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

        #region Index

        public IActionResult Index()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Logos = GetLogos();
            mymodel.Headers = GetHeaders();
            mymodel.AboutUsImages = GetAboutUsImage();
            mymodel.AboutUsImageTwos = GetAboutUsImageTwo();
            mymodel.Contacts = GetContacts();


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
                _session.SetString("contactnumber", contact.MobileNumberOne);
                
                ViewData["contactnumber"] = contact.MobileNumberOne;
            }

            foreach (Logo logo in mymodel.Logos)
            {
                _session.SetString("imageoflogo", logo.Image);

                ViewData["imageoflogo"] = logo.Image;
            }


            return View();
        }

        #endregion


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

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
    }
}
