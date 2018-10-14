using System;
using System.Collections.Generic;
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
    public class ApplicationUserController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public ApplicationUserController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Index

        [HttpGet]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Index()
        {
            //Counters
            ViewData["carbrandcounter"] = _database.CarBrands.Count();
            ViewData["caravaliablecounter"] = _database.Cars.Where(c => c.CarAvaliability == Avaliability.Avaliable).Count();
            ViewData["carrentedout"] = _database.Cars.Where(c => c.CarAvaliability == Avaliability.Rented).Count();
            ViewData["contactcounter"] = _database.Contacts.Count();
            ViewData["enquirycounter"] = _database.Enquiries.Count();
            ViewData["carcounter"] = _database.Cars.Count();
            ViewData["pricecounter"] = _database.Prices.Count();
            ViewData["statecounter"] = _database.States.Count();
            ViewData["lgacounter"] = _database.Lgas.Count();
            ViewData["bookingcounter"] = _database.Bookings.Count();



            var userObject = _session.GetString("imouloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);

            ViewData["loggedinuserfullname"] = _user.DisplayName;
            var roleid = _user.RoleId;

            ViewData["canmangecars"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageCars == true);
            ViewData["canmangecustomers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageCustomers == true);
            ViewData["canmangelandingdetails"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageLandingDetails == true);
            ViewData["canmangecarbrand"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageCarBrand == true);
            ViewData["canmangeprices"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManagePrices == true);
            ViewData["canmangeenquries"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageEnquires == true);
            ViewData["canmangebookings"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageLandingDetails == true);
            ViewData["canmangestates"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageStates == true);
            ViewData["canmangelgas"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageLgas == true);
            ViewData["canmangedrivers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageDrivers == true);
            ViewData["canmangepassengersinformation"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManagePassengersInformation == true);
            ViewData["candoeverything"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanDoEverything == true);
            ViewData["canmanageapplicationusers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageApplicationUsers == true);
            var appUser = _database.ApplicationUsers;
            return View(await appUser.ToListAsync());
        }

        #endregion

        #region Create

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(_database.Roles.Where(r => r.CanDoEverything == false), "RoleId", "Name");
            var appUser = new ApplicationUser();
            return PartialView("Create", appUser);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Create(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var allUsers = await _database.ApplicationUsers.ToListAsync();
                if(allUsers.Any(au => au.Email == user.Email))
                {
                    TempData["appuser"] = "You cannot add " + user.Email + " as an application user because it already exist!!!";
                    TempData["notificationType"] = NotificationType.Error.ToString();
                    return View("Index");
                }

                var _user = new ApplicationUser()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                    ConfirmPassword = BCrypt.Net.BCrypt.HashPassword(user.ConfirmPassword),
                    RoleId = user.RoleId,
                    CreatedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid")),
                    DateCreated = DateTime.Now,
                    LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid")),
                    DateLastModified = DateTime.Now
                };

                await _database.ApplicationUsers.AddAsync(_user);
                await _database.SaveChangesAsync();

                TempData["appuser"] = "You have successfully added " + _user.DisplayName + " as an Application User.";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            ViewBag.Roles = new SelectList(_database.Roles, "RoleId", "Name", user.RoleId);
            return View("Index");
        }

        #endregion

        #region Edit



        #endregion

    }
}