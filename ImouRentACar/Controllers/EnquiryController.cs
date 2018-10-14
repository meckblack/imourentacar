using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImouRentACar.Data;
using ImouRentACar.Models;
using ImouRentACar.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ImouRentACar.Controllers
{
    public class EnquiryController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public EnquiryController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Index

        public async Task<IActionResult> Index()
        {
            //Counters
            ViewData["carbrandcounter"] = _database.CarBrands.Count();
            ViewData["caravaliablecounter"] = _database.Cars.Where(c => c.CarAvaliability == Avaliability.Avaliable).Count();
            ViewData["carrentedout"] = _database.Cars.Where(c => c.CarAvaliability == Avaliability.Rented).Count();
            ViewData["contactcounter"] = _database.Contacts.Count();
            ViewData["enquirycounter"] = _database.Enquiries.Count();

            var userObject = _session.GetString("imouloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);

            ViewData["loggedinuserfullname"] = _user.DisplayName;
            ViewData["loggedinuseremail"] = _user.Email;

            var roleid = _user.RoleId;

            var role = _database.Roles.Find(roleid);

            ViewData["userrole"] = role.Name;

            if (role.CanManageEnquires == false)
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

            var enquiries = await _database.Enquiries.ToListAsync();
            return View(enquiries);
        }

        #endregion

        #region Enquiry
        
        [HttpPost]
        public async Task<IActionResult> Enquiry(Enquiry enquiry)
        {
            if (ModelState.IsValid)
            {
                var _enquiry = new Enquiry()
                {
                    FirstName = enquiry.FirstName,
                    LastName = enquiry.LastName,
                    Organization = enquiry.Organization,
                    Comment = enquiry.Comment,
                    Email = enquiry.Email,
                    Verification = Verification.YetToReply,
                    DateSent = DateTime.Now,
                };

                await _database.Enquiries.AddAsync(_enquiry);
                await _database.SaveChangesAsync();

                TempData["enquiry"] = "Mr "+ enquiry.DisplayName +" your enquiry has been sent";
                TempData["notificationType"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home", enquiry);
        }

        #endregion

        #region Details

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enquiry = await _database.Enquiries.SingleOrDefaultAsync(e => e.EnquiryId == id);

            if (enquiry == null)
            {
                return NotFound();
            }
            return PartialView("Details", enquiry);
        }

        #endregion

        #region Delete

        [HttpGet]
        //[SessionExpireFilterAttribute]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enquiry = await _database.Enquiries.SingleOrDefaultAsync(b => b.EnquiryId == id);

            if (enquiry == null)
            {
                return NotFound();
            }

            return PartialView("Delete", enquiry);
        }

        [HttpPost]
        //[SessionExpireFilterAttribute]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var enquiry = await _database.Enquiries.SingleOrDefaultAsync(b => b.EnquiryId == id);

            if (enquiry != null)
            {
                _database.Enquiries.Remove(enquiry);
                await _database.SaveChangesAsync();

                TempData["enquiry"] = "You have successfully deleted the enquiry made by " + enquiry.DisplayName + " !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return View("Index");
        }

        #endregion
    }
}