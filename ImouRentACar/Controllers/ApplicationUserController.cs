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

            var role = _database.Roles.Find(roleid);
            
            ViewData["userrole"] = role.Name;

            if (role.CanManageApplicationUsers == false && role.CanDoEverything == false)
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

            var appUser = _database.ApplicationUsers;
            return View(await appUser.ToListAsync());
        }

        #endregion

        #region Create

        [HttpGet]
        public IActionResult Create()
        {
            var userObject = _session.GetString("imouloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);
            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);
            if (role.CanManageApplicationUsers == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            ViewBag.Roles = new SelectList(_database.Roles, "RoleId", "Name");
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
                if(allUsers.Any(au => au.Email == user.Email ))
                {
                    TempData["appuser"] = "You cannot add " + user.Email + " as an application user because it already exist!!!";
                    TempData["notificationType"] = NotificationType.Error.ToString();
                    return Json(new { success = true });
                }
                if(user.RoleId == 1)
                {
                    TempData["appuser"] = "You cannot add " + user.Email + " as an application user because only one Supper User can exist!!!";
                    TempData["notificationType"] = NotificationType.Error.ToString();
                    return Json(new { success = true });
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
            return RedirectToAction("Index");
        }

        #endregion
        
        #region Delete

        [HttpGet]
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
                return RedirectToAction("Index", "Error");
            }

            var user = await _database.ApplicationUsers.SingleOrDefaultAsync(u => u.ApplicationUserId == id);

            if (_user == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var _user = await _database.ApplicationUsers.SingleOrDefaultAsync(u => u.ApplicationUserId == id);

            if (_user == null)
            {
                return RedirectToAction("Index", "Error");
            }

            _database.ApplicationUsers.Remove(_user);
            await _database.SaveChangesAsync();

            TempData["appuser"] = "You have successfully deleted  " + _user.DisplayName + "  as an application user!!!";
            TempData["notificationType"] = NotificationType.Success.ToString();

            return Json(new { success = true });
        }

        #endregion

        #region Details

        [HttpGet]
        [SessionExpireFilter]
        public async Task<IActionResult> Details(int? id)
        {
            var userObject = _session.GetString("imouloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);
            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);
            ViewData["rolename"] = role.Name;
            if (role.CanManageApplicationUsers == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return NotFound();
            }

            var _appUser = await _database.ApplicationUsers.SingleOrDefaultAsync(c => c.ApplicationUserId == id);

            if (_appUser == null)
            {
                return NotFound();
            }

            var _roleid = _appUser.RoleId;
            var _role = await _database.Roles.FindAsync(_roleid);
            ViewData["rolename"] = _role.Name;

            var creatorid = _appUser.CreatedBy;
            var creator = await _database.ApplicationUsers.FindAsync(creatorid);
            ViewData["creatorby"] = creator.DisplayName;

            var modifierid = _appUser.LastModifiedBy;
            var modifier = await _database.ApplicationUsers.FindAsync(modifierid);
            ViewData["modifiedby"] = modifier.DisplayName;

            return PartialView(_appUser);
        }

        #endregion

    }
}