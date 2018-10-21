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
    public class PolicyController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public PolicyController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Index



        #endregion

        #region Create

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
            var _policy = new Policy();
            return PartialView("Create", _policy);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Policy policy)
        {
            if (ModelState.IsValid)
            {
                var allPolicy = _database.Policies.Count();

                if(allPolicy > 1)
                {
                    TempData["landing"] = "Sorry you cannot have more than one policy. Try deleting before adding a new one";
                    TempData["notificationType"] = NotificationType.Info.ToString();
                }

                TempData["landing"] = "You have successfully added a new policy";
                TempData["notificationType"] = NotificationType.Success.ToString();

                await _database.Policies.AddAsync(policy);
                await _database.SaveChangesAsync();
                
                return Json(new { success = true });
            }

            return RedirectToAction("Index", "Landing");
        }

        #endregion

        #region View

        [HttpGet]
        public async Task<IActionResult> View(int? id)
        {
            var userid = _session.GetInt32("imouloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);

            if (role.CanManageStates == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if(id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var policy = await _database.Policies.SingleOrDefaultAsync(p => p.PolicyId == id);

            if(policy == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("View", policy);
        }

        #endregion

        #region Delete

        [HttpGet]
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
                return RedirectToAction("Index", "Error");
            }

            var policy = await _database.Policies
                .SingleOrDefaultAsync(m => m.PolicyId == id);
            if (policy == null)
            {
                return RedirectToAction("Index", "Error");
            }
            return PartialView("Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var policy = await _database.Policies.SingleOrDefaultAsync(m => m.PolicyId == id);
            if (policy != null)
            {
                _database.Policies.Remove(policy);
                await _database.SaveChangesAsync();

                TempData["landing"] = "You have successfully deleted IMOU policy!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return RedirectToAction("Index", "Landing");
        }

        #endregion
    }
}