﻿using System;
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

namespace ImouRentACar.Controllers
{
    public class LogoController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IHostingEnvironment _environment;

        #region Constructor

        public LogoController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHostingEnvironment environment)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
        }

        #endregion

        #region Add Logo

        [HttpGet]
        public IActionResult AddLogo()
        {
            var counter = _database.Logos.Count();

            if(counter == 1)
            {
                TempData["landing"] = "Sorry there exist a logo. You can change it by deleting first before adding!!!";
                TempData["notificationType"] = NotificationType.Info.ToString();
                return RedirectToAction("Index", "Landing");
            }

            return View();
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> AddLogo(Logo logo, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("null_img", "File not selected");
            }
            else
            {
                var fileinfo = new FileInfo(file.FileName);
                var filename = DateTime.Now.ToFileTime() + fileinfo.Extension;
                var uploads = Path.Combine(_environment.WebRootPath, "UploadedFiles\\Logo");
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                if (ModelState.IsValid)
                {
                    logo.Image = filename;
                    logo.CreatedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuser"));
                    logo.DateCreated = DateTime.Now;
                    logo.DateLastModified = DateTime.Now;
                    logo.LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuser"));

                    TempData["landing"] = "You have successfully added Imou's Logo !!!";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    await _database.Logos.AddAsync(logo);
                    await _database.SaveChangesAsync();

                    
                    
                    return RedirectToAction("Index", "Landing");

                }
            }
            return View(logo);
        }

        #endregion
        
        #region Delete Logo

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logo = await _database.Logos
                .SingleOrDefaultAsync(m => m.LogoId == id);
            if (logo == null)
            {
                return NotFound();
            }
            return View("Delete");
        }

        // POST: Meals/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var logo = await _database.Logos.SingleOrDefaultAsync(m => m.LogoId == id);
            if (logo != null)
            {
                _database.Logos.Remove(logo);
                await _database.SaveChangesAsync();

                TempData["landing"] = "You have successfully deleted IMOU Logo!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region View Logo

        [HttpGet]
        public async Task<IActionResult> View(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var _logo = await _database.Logos.SingleOrDefaultAsync(l => l.LogoId == id);

            if(_logo == null)
            {
                return NotFound();
            }

            return View(_logo);
        }

        #endregion

        #region Exist

        private bool LogoExists(int id)
        {
            return _database.Logos.Any(e => e.LogoId == id);
        }

        #endregion
    }
}