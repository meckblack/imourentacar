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

namespace ImouRentACar.Controllers
{
    public class CarBrandController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public CarBrandController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Index

        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Index()
        {
            //Counters
            ViewData["carbrandcounter"] = _database.CarBrands.Count();
            ViewData["caravaliablecounter"] = _database.Cars.Where(c => c.CarAvaliability == Avaliability.Avaliable).Count();
            ViewData["carrentedout"] = _database.Cars.Where(c => c.CarAvaliability == Avaliability.Rented).Count();
            ViewData["contactcounter"] = _database.Contacts.Count();
            ViewData["enquirycounter"] = _database.Enquiries.Count();

            var userid = _session.GetInt32("imouloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            ViewData["loggedinuserfullname"] = _user.DisplayName;

            var brands = await _database.CarBrands.ToListAsync();
            return View(brands);
        }

        #endregion

        #region Create

        [HttpGet]
        public IActionResult Create()
        {
            var brand = new CarBrand();
            return PartialView("Create", brand);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarBrand brand)
        {
            if (ModelState.IsValid)
            {
                var allBrands = await _database.CarBrands.ToListAsync();
                if (allBrands.Any(b => b.Name == brand.Name))
                {
                    TempData["carbrand"] = "You cannot add " + brand.Name + " car brand because it already exist!!!";
                    TempData["notificationType"] = NotificationType.Error.ToString();
                    return View("Index");
                }
                
                var _brand = new CarBrand()
                {
                    Name = brand.Name,
                    CreatedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid")),
                    DateCreated = DateTime.Now,
                    LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid")),
                    DateLastModified = DateTime.Now
                };

                await _database.CarBrands.AddAsync(_brand);
                await _database.SaveChangesAsync();

                TempData["carbrand"] = "You have successfully added " + brand.Name + " car brand.";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            return View("Index");
        }

        #endregion

        #region Edit

        [HttpGet]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _brand = await _database.CarBrands.SingleOrDefaultAsync(b => b.CarBrandId == id);

            if (_brand == null)
            {
                return NotFound();
            }

            return PartialView("Edit", _brand);
        }
        
        [HttpPost]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Edit(int id, CarBrand brand)
        {
            if(id != brand.CarBrandId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    brand.LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));
                    brand.DateLastModified = DateTime.Now;
                    _database.Update(brand);
                    await _database.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarBrandExists(brand.CarBrandId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                TempData["carbrand"] = "You have successfully modified " + brand.Name + " car brand.";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return View(brand);
        }

        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if ( id == null)
            {
                return NotFound();
            }

            var brand = await _database.CarBrands.SingleOrDefaultAsync(b => b.CarBrandId == id);

            if(brand == null)
            {
                return NotFound();
            }
            
            return PartialView("Delete", brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _database.CarBrands.SingleOrDefaultAsync(b => b.CarBrandId == id);

            if (brand != null)
            {
                _database.CarBrands.Remove(brand);
                await _database.SaveChangesAsync();

                TempData["carbrand"] = "You have successfully deleted  " + brand.Name + "  car brand!!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return View("Index");
        }

        #endregion

        #region View Cars

        [HttpGet]
        public async Task<IActionResult> ViewCars(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            
            var _cars = await _database.Cars.Where(c => c.CarBrandId == id).ToListAsync();

            if(_cars == null)
            {
                return NotFound();
            }

            var brand = await _database.CarBrands.FindAsync(id);

            ViewData["brandname"] = brand.Name;

            return View(_cars);
        }

        #endregion

        #region Car Brand Exists

        private bool CarBrandExists(int id)
        {
            return _database.CarBrands.Any(e => e.CarBrandId == id);
        }

        #endregion
    }
}