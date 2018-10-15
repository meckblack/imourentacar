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
using Newtonsoft.Json;

namespace ImouRentACar.Controllers
{
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public RoleController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Index

        // GET: Role
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

            return View(await _database.Roles.ToListAsync());
        }

        #endregion

        #region Details

        // GET: Role/Details/5
        [SessionExpireFilter]
        public async Task<IActionResult> Details(int? id)
        {
            var userObject = _session.GetString("imouloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);
            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);
            ViewData["rolename"] = role.Name;
            if (id == null)
            {
                return NotFound();
            }

            var _role = await _database.Roles
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (_role == null)
            {
                return NotFound();
            }

            var creatorid = _role.CreatedBy;
            var creator = await _database.ApplicationUsers.FindAsync(creatorid);
            ViewData["createdby"] = creator.DisplayName;

            var modifierid = _role.LastModifiedBy;
            var modifier = await _database.ApplicationUsers.FindAsync(modifierid);
            ViewData["modifiedby"] = modifier.DisplayName;
            
            return PartialView(_role);
        }

        #endregion

        #region Create

        // GET: Role/Create
        [HttpGet]
        [SessionExpireFilter]
        public async Task<IActionResult> Create()
        {
            var userid = _session.GetInt32("imouloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);

            if (role.CanManageStates == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            var _role = new Role();
            return PartialView("Create", _role);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Create(Role role)
        {
            if (ModelState.IsValid)
            {
                var allRoles = await _database.Roles.ToListAsync();

                if(allRoles.Any(r => r.Name == role.Name))
                {
                    TempData["role"] = "You cannot add " + role.Name + " Role because it already exist!!!";
                    TempData["notificationType"] = NotificationType.Error.ToString();
                    return Json(new { success = true });
                }

                role.CanDoEverything = false;
                role.DateCreated = DateTime.Now;
                role.DateLastModified = DateTime.Now;
                role.CreatedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));
                role.LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));

                await _database.Roles.AddAsync(role);
                await _database.SaveChangesAsync();

                TempData["role"] = "You have successfully added " + role.Name + " Role.";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Edit

        // GET: Role/Edit/5
        [HttpGet]
        [SessionExpireFilter]
        public async Task<IActionResult> Edit(int? id)
        {
            var userid = _session.GetInt32("imouloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);

            if (role.CanManageStates == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return NotFound();
            }

            var _role = await _database.Roles.FindAsync(id);
            if (_role == null)
            {
                return NotFound();
            }
            return PartialView("Edit", _role);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Edit(int? id, Role role)
        {
            if(id != role.RoleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    role.CanDoEverything = false;
                    role.LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));
                    role.DateLastModified = DateTime.Now;

                    _database.Update(role);
                    await _database.SaveChangesAsync();

                    TempData["role"] = "You have successfully modified " + role.Name + " Role.";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.RoleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(role);
        }

        #endregion

        #region Delete

        // GET: Role/Delete/5
        [HttpGet]
        [SessionExpireFilter]
        public async Task<IActionResult> Delete(int? id)
        {
            var userid = _session.GetInt32("imouloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);

            if (role.CanManageStates == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return NotFound();
            }

            var _role = await _database.Roles
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (role == null)
            {
                return NotFound();
            }

            return PartialView("Delete", _role);
        }

        // POST: Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = await _database.Roles.FindAsync(id);

            if(role != null)
            {
                _database.Roles.Remove(role);
                await _database.SaveChangesAsync();

                TempData["role"] = "You have successfully deleted  " + role.Name + "  Role!!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return View(role);
        }

        #endregion

        #region Role Exists

        private bool RoleExists(int id)
        {
            return _database.Roles.Any(r => r.RoleId == id);
        }

        #endregion



    }
}
