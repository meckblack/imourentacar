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
    public class LgaController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public LgaController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Index

        // GET: Lga
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

            var roleid = _user.RoleId;

            var role = _database.Roles.Find(roleid);

            ViewData["userrole"] = role.Name;

            if (role.CanManageStates == false && role.CanDoEverything == false)
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

            var lgas = _database.Lgas.Include(l => l.State);
            return View(await lgas.ToListAsync());
        }

        #endregion

        #region Create

        // GET: Lga/Create
        public IActionResult Create()
        {
            var lga = new LGA();
            ViewBag.StateId = new SelectList(_database.States, "StateId", "Name");
            return PartialView("Create", lga);
        }

        // POST: Lga/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [SessionExpireFilterAttribute]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LGAId,Name,StateId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy")] LGA lga)
        {
            if (ModelState.IsValid)
            {
                var allLga = await _database.Lgas.Where(l => l.StateId == lga.StateId).ToListAsync();
                if (allLga.Any(b => b.Name == lga.Name))
                {
                    TempData["lga"] = "You cannot add " + lga.Name + " as a local government area because it already exist!!!";
                    TempData["notificationType"] = NotificationType.Error.ToString();
                    return View("Index");
                }

                var _lga = new LGA()
                {
                    Name = lga.Name,
                    StateId = lga.StateId,
                    CreatedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid")),
                    DateCreated = DateTime.Now,
                    LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid")),
                    DateLastModified = DateTime.Now
                };

                await _database.Lgas.AddAsync(_lga);
                await _database.SaveChangesAsync();

                TempData["lga"] = "You have successfully added " + lga.Name + " Local Government Area.";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            ViewBag.StateId = new SelectList(_database.States, "StateId", "Name", lga.StateId);
            return View("Index");
        }

        #endregion

        #region Edit

        // GET: Lga/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lga = await _database.Lgas.FindAsync(id);
            if (lga == null)
            {
                return NotFound();
            }
            ViewBag.StateId = new SelectList(_database.States, "StateId", "Name", lga.StateId);

            return PartialView("Edit", lga);
        }

        // POST: Lga/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Edit(int id, [Bind("LGAId,Name,StateId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy")] LGA lga)
        {
            if (id != lga.LGAId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    lga.LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));
                    lga.DateLastModified = DateTime.Now;

                    _database.Update(lga);
                    await _database.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LGAExists(lga.LGAId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                TempData["lga"] = "You have successfully modified " + lga.Name + " Local government area.";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            ViewBag.StateId = new SelectList(_database.States, "StateId", "Name", lga.StateId);
            return View(lga);
        }

        #endregion

        #region Delete

        // GET: Lga/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lga = await _database.Lgas
                .Include(l => l.State)
                .FirstOrDefaultAsync(m => m.LGAId == id);
            if (lga == null)
            {
                return NotFound();
            }

            return PartialView("Delete", lga);
        }

        // POST: Lga/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lga = await _database.Lgas.FindAsync(id);

            if(lga != null)
            {
                _database.Lgas.Remove(lga);
                await _database.SaveChangesAsync();

                TempData["lga"] = "You have successfully deleted " + lga.Name + " Local government area.";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return View("Index");
        }

        #endregion

        #region Lga Exists

        private bool LGAExists(int id)
        {
            return _database.Lgas.Any(e => e.LGAId == id);
        }

        #endregion


    }
}
