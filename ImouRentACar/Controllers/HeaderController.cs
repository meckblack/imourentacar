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
    public class HeaderController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IHostingEnvironment _environment;

        #region Constructor

        public HeaderController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHostingEnvironment environment)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
        }

        #endregion

        #region Add Header

        [HttpGet]
        public IActionResult AddHeader()
        {
            var counter = _database.Headers.Count();

            if (counter == 1)
            {
                TempData["landing"] = "Sorry there exist a header image. You can change it by deleting first before adding!!!";
                TempData["notificationType"] = NotificationType.Info.ToString();
                return RedirectToAction("Index", "Landing");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddHeader(Header header, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("null_img", "File not selected");
            }
            else
            {
                var fileinfo = new FileInfo(file.FileName);
                var filename = DateTime.Now.ToFileTime() + fileinfo.Extension;
                var uploads = Path.Combine(_environment.WebRootPath, "UploadedFiles\\HeaderImage");
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                if (ModelState.IsValid)
                {
                    header.Image = filename;
                    header.CreatedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuser"));
                    header.DateCreated = DateTime.Now;
                    header.DateLastModified = DateTime.Now;
                    header.LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuser"));

                    TempData["landing"] = "You have successfully added Imou's Website header image !!!";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    await _database.Headers.AddAsync(header);
                    await _database.SaveChangesAsync();



                    return RedirectToAction("Index", "Landing");

                }
            }
            return View(header);
        }

        #endregion

        #region Delete Header

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var header = await _database.Headers
                .SingleOrDefaultAsync(m => m.HeaderId == id);
            if (header == null)
            {
                return NotFound();
            }
            return PartialView("Delete", header);
        }

        // POST: Meals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var header = await _database.Headers.SingleOrDefaultAsync(m => m.HeaderId == id);
            if (header != null)
            {
                _database.Headers.Remove(header);
                await _database.SaveChangesAsync();

                TempData["landing"] = "You have successfully deleted IMOU Website header images!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region View Header Image

        [HttpGet]
        public async Task<IActionResult> View(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _header = await _database.Headers.SingleOrDefaultAsync(h => h.HeaderId == id);

            if (_header == null)
            {
                return NotFound();
            }

            return View(_header);
        }

        #endregion

        #region View Header WriteUp

        [HttpGet]
        public async Task<IActionResult> WriteUp(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _header = await _database.Headers.SingleOrDefaultAsync(h => h.HeaderId == id);

            if (_header == null)
            {
                return NotFound();
            }

            return View(_header);
        }

        #endregion
        
        #region Exist

        private bool HeaderExists(int id)
        {
            return _database.Headers.Any(e => e.HeaderId == id);
        }

        #endregion
    }
}