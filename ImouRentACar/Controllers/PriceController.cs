using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImouRentACar.Data;
using ImouRentACar.Models;
using Microsoft.AspNetCore.Http;
using ImouRentACar.Models.Enums;

namespace ImouRentACar.Controllers
{
    public class PriceController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public PriceController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Index

        // GET: Price
        public async Task<IActionResult> Index()
        {
            //Counters
            ViewData["carbrandcounter"] = _database.CarBrands.Count();
            ViewData["caravaliablecounter"] = _database.Cars.Where(c => c.CarAvaliability == Avaliability.Avaliable).Count();
            ViewData["carrentedout"] = _database.Cars.Where(c => c.CarAvaliability == Avaliability.Rented).Count();
            ViewData["contactcounter"] = _database.Contacts.Count();
            ViewData["enquirycounter"] = _database.Enquiries.Count();
            //ViewData["reservationcounter"] = _database.Bookings.Where(r => r.Verification == Verification.Approve).Count();

            var userid = _session.GetInt32("imouloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            ViewData["loggedinuserfullname"] = _user.DisplayName;

            var prices = _database.Prices.Include(p => p.Car);
            return View(await prices.ToListAsync());
        }

        #endregion

        #region Create

        // GET: Price/Create
        public IActionResult Create()
        {
            ViewBag.CarId = new SelectList(_database.Cars, "CarId", "Name");
            var price = new Price();
            return PartialView("Create", price);
        }

        // POST: Price/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PriceId,Name,Amount,CarId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy")] Price price)
        {
            if (ModelState.IsValid)
            {
                var allPrices = await _database.Prices.Where(p => p.CarId == price.CarId).AnyAsync(p => p.Name == price.Name);
                if(allPrices == true)
                {
                    TempData["price"] = "You can not add that price because it already exist!!!";
                    TempData["notificationType"] = NotificationType.Error.ToString();
                    return RedirectToAction("Index");
                }

                var _price = new Price()
                {
                    Name = price.Name,
                    Amount = price.Amount,
                    CarId = price.CarId,
                    CreatedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuser")),
                    DateCreated = DateTime.Now,
                    LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuser")),
                    DateLastModified = DateTime.Now
                };

                _database.Add(_price);
                await _database.SaveChangesAsync();

                TempData["price"] = "You have successfully added the price";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            ViewBag.CarId = new SelectList(_database.Cars, "CarId", "Name", price.CarId);
            return RedirectToAction("Index");
        }

        #endregion

        #region Details

        // GET: Price/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var price = await _database.Prices
                .Include(p => p.Car)
                .FirstOrDefaultAsync(m => m.PriceId == id);
            if (price == null)
            {
                return NotFound();
            }

            return PartialView("Details", price);
        }

        #endregion

        #region Edit

        // GET: Price/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var price = await _database.Prices.FindAsync(id);
            if (price == null)
            {
                return NotFound();
            }

            ViewBag.CarId = new SelectList(_database.Cars, "CarId", "Name", price.CarId);
            return PartialView("Edit", price);
        }

        // POST: Price/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PriceId,Name,Amount,CarId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy")] Price price)
        {
            if (id != price.PriceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    price.LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuser"));
                    price.DateLastModified = DateTime.Now;

                    _database.Update(price);
                    await _database.SaveChangesAsync();

                    TempData["price"] = "You have successfully modified the price";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PriceExists(price.PriceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.CarId = new SelectList(_database.Cars, "CarId", "Name", price.CarId);
            return RedirectToAction("Index");
        }

        #endregion

        #region Delete

        // GET: Price/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var price = await _database.Prices
                .Include(p => p.Car)
                .FirstOrDefaultAsync(m => m.PriceId == id);
            if (price == null)
            {
                return NotFound();
            }

            return PartialView("Delete", price);
        }

        // POST: Price/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if(id != null)
            {
                var price = await _database.Prices.FindAsync(id);

                _database.Prices.Remove(price);
                await _database.SaveChangesAsync();

                TempData["price"] = "You have successfully deleted "+ price.Name +" price";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Price Exists

        private bool PriceExists(int id)
        {
            return _database.Prices.Any(e => e.PriceId == id);
        }

        #endregion
        
    }
}
