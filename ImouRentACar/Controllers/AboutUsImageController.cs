using System;
using System.Collections.Generic;
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
        public IActionResult AddImage()
        {
            var counter = _database.AboutUsImages.Count();

            if (counter == 1)
            {
                TempData["landing"] = "Sorry there exist a About Us image. You can change it by deleting first before adding!!!";
                TempData["notificationType"] = NotificationType.Info.ToString();
                return RedirectToAction("Index", "Landing");
            }

            return View();
        }

        [HttpPost]
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