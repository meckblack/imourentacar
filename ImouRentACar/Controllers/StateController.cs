using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImouRentACar.Data;
using ImouRentACar.Models;
using ImouRentACar.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> Index()
        {
            //Counters
            ViewData["carbrandcounter"] = _database.CarBrands.Count();
            ViewData["caravaliablecounter"] = _database.Cars.Where(c => c.CarAvaliability == CarAvaliability.Avaliable).Count();
            ViewData["carrentedout"] = _database.Cars.Where(c => c.CarAvaliability == CarAvaliability.Rented).Count();
            ViewData["contactcounter"] = _database.Contacts.Count();
            ViewData["enquirycounter"] = _database.Enquiries.Count();

            var userid = _session.GetInt32("imouloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            ViewData["loggedinuserfullname"] = _user.DisplayName;

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
                return NotFound();
            }

            var _state = await _database.States.SingleOrDefaultAsync(s => s.StateId == id);

            if (_state == null)
            {
                return NotFound();
            }

            return PartialView("Edit", _state);
        }

        [HttpPost]
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

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _state = await _database.States.SingleOrDefaultAsync(s => s.StateId == id);

            if(_state == null)
            {
                return NotFound();
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
                return NotFound();
            }

            var stateLgas = await _database.Lgas.Where(sl => sl.StateId == id).ToListAsync();

            if(stateLgas == null)
            {
                return NotFound();
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