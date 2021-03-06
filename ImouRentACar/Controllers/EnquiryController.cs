﻿using System;
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

        [HttpGet]
        [Route("enquiry/index")]
        public async Task<IActionResult> Index()
        {
            var userObject = _session.GetString("imouloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);

            ViewData["loggedinuserfullname"] = _user.DisplayName;
            ViewData["loggedinuseremail"] = _user.Email;

            var roleid = _user.RoleId;

            var role = _database.Roles.Find(roleid);

            ViewData["userrole"] = role.Name;

            if (role.CanManageEnquires == false && role.CanDoEverything == false)
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
                
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home", enquiry);
        }

        #endregion

        #region Details

        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Details(int? id)
        {
            var userObject = _session.GetString("imouloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);
            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);
            if (role.CanManageApplicationUsers == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            ViewData["rolename"] = role.Name;

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var enquiry = await _database.Enquiries.SingleOrDefaultAsync(e => e.EnquiryId == id);

            if (enquiry == null)
            {
                return RedirectToAction("Index", "Error");
            }
            return PartialView("Details", enquiry);
        }

        #endregion

        #region Delete

        [HttpGet]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Delete(int? id)
        {
            var userObject = _session.GetString("imouloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);
            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);
            if (role.CanManageEnquires == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
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