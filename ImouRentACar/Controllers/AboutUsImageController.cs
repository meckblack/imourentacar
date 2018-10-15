using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImouRentACar.Data;
using ImouRentACar.Models;
using ImouRentACar.Models.Enums;
using ImouRentACar.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ImouRentACar.Controllers
{
    public class AboutUsImageController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IHostingEnvironment _environment;

        #region Constructor

        public AboutUsImageController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHostingEnvironment environment)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
        }

        #endregion

        #region Add Add About Us Image

        [HttpGet]
        public async Task<IActionResult> AddImage()
        {
            var counter = _database.AboutUsImages.Count();

            if (counter == 1)
            {
                TempData["landing"] = "Sorry there exist a About Us image. You can change it by deleting first before adding!!!";
                TempData["notificationType"] = NotificationType.Info.ToString();
                return RedirectToAction("Index", "Landing");
            }

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

            if (role.CanManageLandingDetails == false || role.CanDoEverything == false)
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


            return View();
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> AddImage(AboutUsImage aboutUsImage, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("null_img", "File not selected");
            }
            else
            {
                var fileinfo = new FileInfo(file.FileName);
                var filename = DateTime.Now.ToFileTime() + fileinfo.Extension;
                var uploads = Path.Combine(_environment.WebRootPath, "UploadedFiles\\AboutUs");
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                if (ModelState.IsValid)
                {
                    aboutUsImage.Image = filename;
                    aboutUsImage.CreatedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuser"));
                    aboutUsImage.DateCreated = DateTime.Now;
                    aboutUsImage.DateLastModified = DateTime.Now;
                    aboutUsImage.LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuser"));

                    TempData["landing"] = "You have successfully added Imou's Website About us image !!!";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    await _database.AboutUsImages.AddAsync(aboutUsImage);
                    await _database.SaveChangesAsync();



                    return RedirectToAction("Index", "Landing");

                }
            }
            return View(aboutUsImage);
        }

        #endregion

        #region Delete About Us Image

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutUsImage = await _database.AboutUsImages
                .SingleOrDefaultAsync(m => m.AboutUsImageId == id);
            if (aboutUsImage == null)
            {
                return NotFound();
            }
            return PartialView("Delete", aboutUsImage);
        }

        // POST: Abpout Us/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aboutUsImage = await _database.AboutUsImages.SingleOrDefaultAsync(m => m.AboutUsImageId == id);
            if (aboutUsImage != null)
            {
                _database.AboutUsImages.Remove(aboutUsImage);
                await _database.SaveChangesAsync();

                TempData["landing"] = "You have successfully deleted IMOU Website About Us images!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region View About Us Image

        [HttpGet]
        public async Task<IActionResult> View(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _aboutUs = await _database.AboutUsImages.SingleOrDefaultAsync(h => h.AboutUsImageId == id);

            if (_aboutUs == null)
            {
                return NotFound();
            }

            return View(_aboutUs);
        }

        #endregion

        #region View About Us WriteUp

        [HttpGet]
        public async Task<IActionResult> WriteUp(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _aboutUs = await _database.AboutUsImages.SingleOrDefaultAsync(h => h.AboutUsImageId == id);

            if (_aboutUs == null)
            {
                return NotFound();
            }

            return View(_aboutUs);
        }

        #endregion

        #region Exist

        private bool AboutUsImageExists(int id)
        {
            return _database.AboutUsImages.Any(e => e.AboutUsImageId == id);
        }

        #endregion
    }
}