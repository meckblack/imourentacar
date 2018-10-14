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
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ImouRentACar.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public AdminController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Dashboard

        [HttpGet]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Dashboard()
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
            ViewData["applicationusercounter"] = _database.ApplicationUsers.Count();
            ViewData["customercounter"] = _database.Customers.Count();



            var userObject = _session.GetString("imouloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);

            ViewData["loggedinuserfullname"] = _user.DisplayName;
            ViewData["loggedinuseremail"] = _user.Email;

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


            return View();
        }

        #endregion

    }
}