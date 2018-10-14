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
using ImouRentACar.Services;

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
        [SessionExpireFilterAttribute]
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

            var roleid = _user.RoleId;

            ViewData["canmangecars"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageCars == true);
            ViewData["canmangecustomers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageCustomers == true);
            ViewData["canmangelandingdetails"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageLandingDetails == true);
            ViewData["canmangecarbrand"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageCarBrand == true);
            ViewData["canmangeprices"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManagePrices == true);
            ViewData["canmangeenquries"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageEnquires == true);
            ViewData["canmangebookings"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageLandingDetails == true);
            ViewData["canmangestates"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageStates == true);
            ViewData["canmangelgas"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageLgas == true);
            ViewData["canmangedrivers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageDrivers == true);
            ViewData["canmangepassengersinformation"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManagePassengersInformation == true);
            ViewData["candoeverything"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanDoEverything == true);
            ViewData["canmanageapplicationusers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageApplicationUsers == true);

            var prices = _database.Prices;

            return View(await prices.ToListAsync());
        }

        #endregion

        #region Create

        // GET: Price/Create
        public IActionResult Create()
        {
            ViewBag.DestinationStateId = new SelectList(_database.States, "StateId", "Name");
            ViewBag.PickUpStateId = new SelectList(_database.States, "StateId", "Name");
            
            return View("Create");
        }

        // POST: Price/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Create(Price price)
        {
            if (ModelState.IsValid)
            {
                var allPrices = await _database.Prices.ToListAsync();
                if (allPrices.Any(p => p.DestinationLgaId == price.DestinationLgaId && p.PickUpLgaId == price.PickUpLgaId))
                {
                    TempData["price"] = "You can not add that price because it already exist!!!";
                    TempData["notificationType"] = NotificationType.Error.ToString();
                    return RedirectToAction("Index");
                }

                if(price.PickUpLgaId == price.DestinationLgaId)
                {
                    TempData["price"] = "Pick up local government area and destination local government cannot be the same";
                    TempData["notificationType"] = NotificationType.Error.ToString();
                    return RedirectToAction("Index");
                }

                var _price = new Price()
                {
                    Name = price.Name,
                    Amount = price.Amount,
                    DestinationLgaId = price.DestinationLgaId,
                    PickUpLgaId = price.PickUpLgaId,
                    CreatedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuser")),
                    DateCreated = DateTime.Now,
                    LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuser")),
                    DateLastModified = DateTime.Now
                };

                await _database.AddAsync(_price);
                await _database.SaveChangesAsync();

                TempData["price"] = "You have successfully added the price";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return RedirectToAction("Index");
            }

            ViewBag.DestinationStateId = new SelectList(_database.States, "StateId", "Name", price.DestinationLgaId);
            ViewBag.PickUpStateId = new SelectList(_database.States, "StateId", "Name", price.PickUpLgaId);
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

            ViewBag.DestinationStateId = new SelectList(_database.States, "StateId", "Name");
            ViewBag.PickUpStateId = new SelectList(_database.States, "StateId", "Name");

            return View(price);
        }

        // POST: Price/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Edit(int id, Price price)
        {
            if (id != price.PriceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (price.PickUpLgaId == price.DestinationLgaId)
                    {
                        TempData["price"] = "Pick up local government area and destination local government cannot be the same";
                        TempData["notificationType"] = NotificationType.Error.ToString();
                        return RedirectToAction("Index");
                    }

                    price.LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuser"));
                    price.DateLastModified = DateTime.Now;

                    _database.Update(price);
                    await _database.SaveChangesAsync();

                    TempData["price"] = "You have successfully modified the price";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    return RedirectToAction("Index");
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

            ViewBag.DestinationStateId = new SelectList(_database.States, "StateId", "Name", price.DestinationLgaId);
            ViewBag.PickUpStateId = new SelectList(_database.States, "StateId", "Name", price.PickUpLgaId);

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
            if (id != null)
            {
                var price = await _database.Prices.FindAsync(id);

                _database.Prices.Remove(price);
                await _database.SaveChangesAsync();

                TempData["price"] = "You have successfully deleted " + price.Name + " price";
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

        #region Get Data

        public JsonResult GetLgasForState(int id)
        {
            var lgas = _database.Lgas.Where(l => l.StateId == id);
            return Json(lgas);
        }

        #endregion

    }
}
