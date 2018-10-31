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
using Newtonsoft.Json;

namespace ImouRentACar.Controllers
{
    public class StateController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public StateController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Index

        [HttpGet]
        [Route("state/index")]
        public async Task<IActionResult> Index()
        {
            var userid = _session.GetInt32("imouloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            ViewData["loggedinuserfullname"] = _user.DisplayName;

            var roleid = _user.RoleId;

            var role = _database.Roles.Find(roleid);

            ViewData["userrole"] = role.Name;

            if (role.CanManageLocation == false && role.CanDoEverything == false)
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


            var _state = await _database.States.ToListAsync();
            return View(_state);
        }

        #endregion

        #region Create

        [HttpGet]
        public IActionResult Create()
        {
            var _state = new State();
            return PartialView("Create", _state);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Create(State state)
        {
            if (ModelState.IsValid)
            {
                var allStates = await _database.States.ToListAsync();
                if (allStates.Any(b => b.Name == state.Name))
                {
                    TempData["state"] = "You cannot add " + state.Name + " as a state because it already exist!!!";
                    TempData["notificationType"] = NotificationType.Error.ToString();
                    return View("Index");
                }

                var _state = new State()
                {
                    Name = state.Name,
                    CreatedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid")),
                    DateCreated = DateTime.Now,
                    LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid")),
                    DateLastModified = DateTime.Now
                };

                await _database.States.AddAsync(_state);
                await _database.SaveChangesAsync();

                TempData["state"] = "You have successfully added " + state.Name + " State.";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return View("Index");
        }

        #endregion

        #region Edit

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _state = await _database.States.SingleOrDefaultAsync(s => s.StateId == id);

            if (_state == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Edit", _state);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Edit(int id, State state)
        {
            if(id != state.StateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    state.LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));
                    state.DateLastModified = DateTime.Now;

                    _database.Update(state);
                    await _database.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StateExists(state.StateId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                TempData["state"] = "You have successfully modified " + state.Name + " State.";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return View(state);
        }

        #endregion

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            var userObject = _session.GetString("imouloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);
            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);
            ViewData["rolename"] = role.Name;
            if (role.CanManageLocation == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _state = await _database.States.SingleOrDefaultAsync(c => c.StateId == id);

            if (_state == null)
            {
                return RedirectToAction("Index", "Error");
            }

            
            var creatorid = _state.CreatedBy;
            var creator = await _database.ApplicationUsers.FindAsync(creatorid);
            ViewData["creatorby"] = creator.DisplayName;

            var modifierid = _state.LastModifiedBy;
            var modifier = await _database.ApplicationUsers.FindAsync(modifierid);
            ViewData["modifiedby"] = modifier.DisplayName;
            
            return PartialView(_state);
        }

        #endregion

        #region Delete

        [HttpGet]
        [SessionExpireFilter]
        public async Task<IActionResult> Delete(int? id)
        {
            var userid = _session.GetInt32("imouloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);

            if (role.CanManageLocation == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _state = await _database.States.SingleOrDefaultAsync(s => s.StateId == id);

            if(_state == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", _state);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var state = await _database.States.SingleOrDefaultAsync(s => s.StateId == id);

            if(state != null)
            {
                _database.States.Remove(state);
                await _database.SaveChangesAsync();

                TempData["state"] = "You have successfully deleted " + state.Name + " State.";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return View("Index");
        }

        #endregion

        #region View Lgas

        [HttpGet]
        public async Task<IActionResult> ViewLgas(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var stateLgas = await _database.Lgas.Where(sl => sl.StateId == id).ToListAsync();

            if(stateLgas == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var state = await _database.States.FindAsync(id);
            ViewData["state"] = state.Name;

            return View(stateLgas);

        }

        #endregion

        #region State Exists

        private bool StateExists(int id)
        {
            return _database.States.Any(s => s.StateId == id);
        }

        #endregion
    }
}