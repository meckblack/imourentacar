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
using Microsoft.EntityFrameworkCore;

namespace ImouRentACar.Controllers
{
    public class PolicyController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public PolicyController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Index


        [HttpGet]
        [Route("policy/index")]
        public async Task<IActionResult> Index()
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

            var policy = await _database.Policies.ToListAsync();
            return View(policy);
        }


        #endregion

        #region List

        [HttpGet]
        [Route("policy/list")]
        public async Task<IActionResult> List()
        {
            var userid = _session.GetInt32("imouloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            ViewData["loggedinuserfullname"] = _user.DisplayName;

            var roleid = _user.RoleId;

            var role = _database.Roles.Find(roleid);

            ViewData["userrole"] = role.Name;

            if (role.CanManageLandingDetails == false && role.CanDoEverything == false)
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


            var _policy = await _database.Policies.ToListAsync();
            return View(_policy);
        }

        #endregion

        #region Create

        [HttpGet]
        [SessionExpireFilter]
        public async Task<IActionResult> Create()
        {
            var userid = _session.GetInt32("imouloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);

            if (role.CanManageLandingDetails == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }
            var _policy = new Policy();
            return PartialView("Create", _policy);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Policy policy)
        {
            if (ModelState.IsValid)
            {
                var allPolicy = _database.Policies.Count();

                if(allPolicy > 1)
                {
                    TempData["policy"] = "Sorry you cannot have more than one policy. Try deleting before adding a new one";
                    TempData["notificationType"] = NotificationType.Info.ToString();
                }

                TempData["policy"] = "You have successfully added a new policy";
                TempData["notificationType"] = NotificationType.Success.ToString();

                await _database.Policies.AddAsync(policy);
                await _database.SaveChangesAsync();
                
                return Json(new { success = true });
            }

            return RedirectToAction("List", "Policy");
        }

        #endregion
        
        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            var userid = _session.GetInt32("imouloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);

            if (role.CanManageLandingDetails == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var policy = await _database.Policies
                .SingleOrDefaultAsync(m => m.PolicyId == id);
            if (policy == null)
            {
                return RedirectToAction("Index", "Error");
            }
            return PartialView("Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var policy = await _database.Policies.SingleOrDefaultAsync(m => m.PolicyId == id);
            if (policy != null)
            {
                _database.Policies.Remove(policy);
                await _database.SaveChangesAsync();

                TempData["policy"] = "You have successfully deleted IMOU policy!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return RedirectToAction("List", "Policy");
        }

        #endregion

        #region Get Logos

        private List<Logo> GetLogos()
        {
            var _logos = _database.Logos.ToList();

            return _logos;
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