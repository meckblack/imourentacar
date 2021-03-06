﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImouRentACar.Data;
using ImouRentACar.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using ImouRentACar.Models.Enums;
using System.IO;
using ImouRentACar.Services;
using Newtonsoft.Json;

namespace ImouRentACar.Controllers
{
    public class DriverController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IHostingEnvironment _environment;

        #region Constructor

        public DriverController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHostingEnvironment environment)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
        }

        #endregion

        #region Index

        [SessionExpireFilterAttribute]
        [Route("driver/index")]
        public async Task<IActionResult> Index()
        {
            var _cars = _database.Cars;

            var userid = _session.GetInt32("imouloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);

            ViewData["loggedinuserfullname"] = _user.DisplayName;

            var roleid = _user.RoleId;

            var role = _database.Roles.Find(roleid);

            ViewData["userrole"] = role.Name;

            if (role.CanManageDrivers == false && role.CanDoEverything == false)
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
            return View(await _database.Driver.ToListAsync());
        }

        #endregion

        #region Details

        // GET: Driver/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            var userObject = _session.GetString("imouloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);
            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);
            if (role.CanManageDrivers == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            ViewData["rolename"] = role.Name;

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var driver = await _database.Driver
                .FirstOrDefaultAsync(m => m.DriverId == id);
            if (driver == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Details", driver);
        }

        #endregion

        #region Create

        // GET: Driver/Create
        [HttpGet]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Create()
        {
            var userObject = _session.GetString("imouloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);
            ViewData["loggedinuserfullname"] = _user.DisplayName;

            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);

            ViewData["userrole"] = role.Name;

            if (role.CanManageDrivers == false && role.CanDoEverything == false)
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


            return View();
        }

        // POST: Driver/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Create(Driver driver, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("null_img", "File not selected");
            }
            else
            {
                var allDrivers = await _database.Driver.ToListAsync();

                if(allDrivers.Any(d => d.FirstName == driver.FirstName && d.LastName == driver.LastName))
                {
                    TempData["driver"] = "You cannot add " + driver.DisplayName + " car brand because it already exist!!!";
                    TempData["notificationType"] = NotificationType.Error.ToString();
                    return View("Index");
                }

                var fileinfo = new FileInfo(file.FileName);
                var filename = DateTime.Now.ToFileTime() + fileinfo.Extension;
                var uploads = Path.Combine(_environment.WebRootPath, "UploadedFiles\\Drivers");
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                if (ModelState.IsValid)
                {
                    driver.License = filename;
                    driver.DriverAvaliablity = Avaliability.Avaliable;
                    driver.CreatedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));
                    driver.LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));
                    driver.DateCreated = DateTime.Now;
                    driver.DateLastModified = DateTime.Now;

                    await _database.Driver.AddAsync(driver);
                    await _database.SaveChangesAsync();

                    TempData["driver"] = "You have successfully added " + driver.DisplayName + "!!!";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    return RedirectToAction("Index");
                }
            }

            return View(driver);
        }

        #endregion

        #region Edit

        // GET: Driver/Edit/5
        [HttpGet]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Edit(int? id)
        {
            var userObject = _session.GetString("imouloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);
            var roleid = _user.RoleId;

            var role = _database.Roles.Find(roleid);

            ViewData["userrole"] = role.Name;

            if (role.CanManageDrivers == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var driver = await _database.Driver.FindAsync(id);
            if (driver == null)
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


            return View(driver);
        }

        // POST: Driver/Edit/5
        [HttpPost]
        [SessionExpireFilterAttribute]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Driver driver, IFormFile file)
        {
            if (id != driver.DriverId)
            {
                return NotFound();
            }

            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("null_img", "File not selected");
            }
            else
            {
                var fileinfo = new FileInfo(file.FileName);
                var filename = DateTime.Now.ToFileTime() + fileinfo.Extension;
                var uploads = Path.Combine(_environment.WebRootPath, "UploadedFiles\\Cars");
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                if (ModelState.IsValid)
                {
                    try
                    {

                        driver.License = filename;
                        driver.LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));
                        driver.DateLastModified = DateTime.Now;

                        TempData["driver"] = "You have successfully modified " + driver.DisplayName + "!!!";
                        TempData["notificationType"] = NotificationType.Success.ToString();

                        _database.Update(driver);
                        await _database.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!DriverExists(driver.DriverId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(driver);
        }

        #endregion

        #region Delete

        // GET: Driver/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var userObject = _session.GetString("imouloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);
            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);
            if (role.CanManageDrivers == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var driver = await _database.Driver
                .FirstOrDefaultAsync(m => m.DriverId == id);
            if (driver == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", driver);
        }

        // POST: Driver/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var driver = await _database.Driver.FindAsync(id);

            if (driver != null)
            {
                _database.Driver.Remove(driver);
                await _database.SaveChangesAsync();

                TempData["driver"] = "You have successfully deleted " + driver.DisplayName+ " !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            return View("Index");
        }

        #endregion

        #region Driver Exists

        private bool DriverExists(int id)
        {
            return _database.Driver.Any(e => e.DriverId == id);
        }

        #endregion

    }
}
