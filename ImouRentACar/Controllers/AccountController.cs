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
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public AccountController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region SignIn

        [HttpGet]
        public async Task<IActionResult> SignIn()
        {
            var user = await _database.ApplicationUsers.CountAsync();
            if(user > 0)
            {
                return View();
            }
            else
            {
                return RedirectToAction("FirstRegistration");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(ApplicationUser user)
        {
            var _user = await _database.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == user.Email);

            if(_user == null)
            {
                ViewData["mismatch"] = "Email and Password do not match";
            }

            if(_user != null)
            {
                var password = BCrypt.Net.BCrypt.Verify(user.Password, _user.Password);
                if(password == true)
                {
                    _session.SetString("imouloggedinuser", JsonConvert.SerializeObject(_user));
                    _session.SetInt32("imouloggedinuserid", _user.ApplicationUserId);
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    ViewData["mismatch"] = "Email and Password do not match";
                }
            }
            return View(user);
        }

        #endregion

        #region First Registration

        [HttpGet]
        public IActionResult FirstRegistration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FirstRegistration(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var _role = new Role()
                {
                    Name = "SuperUser",
                    CanDoEverything = true,
                    DateCreated = DateTime.Now,
                    DateLastModified = DateTime.Now,
                };

                await _database.AddAsync(_role);
                await _database.SaveChangesAsync();

                var _user = new ApplicationUser()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                    ConfirmPassword = BCrypt.Net.BCrypt.HashPassword(user.ConfirmPassword),
                    CreatedBy = 1,
                    LastModifiedBy = 1,
                    DateCreated = DateTime.Now,
                    DateLastModified = DateTime.Now,
                    RoleId = _role.RoleId,
                };

                await _database.AddAsync(_user);
                await _database.SaveChangesAsync();

                return RedirectToAction("SignIn");
            }

            return View(user);
        }


        #endregion

        #region SignOut

        public IActionResult SignOut()
        {
            _session.Clear();
            _database.Dispose();
            return RedirectToAction("SignIn", "Account");
        }

        #endregion

        #region View Profile

        [SessionExpireFilterAttribute]
        public async Task<IActionResult> ViewProfile()
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

            if (role.RoleId != roleid)
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
            
            return View(_user);
        }

        #endregion

        #region Edit Profile

        [HttpGet]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> EditProfile()
        {
            var userid = _session.GetInt32("imouloggedinuserid");

            if (userid == null)
            {
                TempData["error"] = "Sorry your session has expired. Try signin again";
                return RedirectToAction("Signin", "Account");
            }

            var _user = await _database.ApplicationUsers.SingleOrDefaultAsync(u => u.ApplicationUserId == userid);

            if (_user == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("EditProfile", _user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> EditProfile(ApplicationUser applicationUser)
        {
            var userid = _session.GetInt32("imouloggedinuserid");

            if(userid != applicationUser.ApplicationUserId)
            {
                return RedirectToAction("Index", "Error");
            }
            
                try
                {
                    var _user = new ApplicationUser()
                    {
                        ApplicationUserId = applicationUser.ApplicationUserId,
                        ConfirmPassword = applicationUser.ConfirmPassword,
                        Password = applicationUser.Password,
                        DateCreated = applicationUser.DateCreated,
                        DateLastModified = DateTime.Now,
                        CreatedBy = applicationUser.CreatedBy,
                        LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid")),
                        Email = applicationUser.Email,
                        FirstName = applicationUser.FirstName,
                        LastName = applicationUser.LastName,
                        RoleId = applicationUser.RoleId

                    };

                TempData["user"] = "You have successfully modified " + applicationUser.DisplayName + " profile.";
                TempData["notificationType"] = NotificationType.Success.ToString();

                _database.ApplicationUsers.Update(_user);
                await _database.SaveChangesAsync();

                return Json(new { success = true });

                }
                catch(Exception ex)
                {
                    throw ex;
                }
        }

        #endregion

        #region Change Password

        [HttpGet]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> ChangePassword()
        {
            var userid = _session.GetInt32("imouloggedinuserid");

            if (userid == null)
            {
                TempData["error"] = "Sorry your session has expired. Try signin again";
                return RedirectToAction("Signin", "Account");
            }

            var _user = await _database.ApplicationUsers.SingleOrDefaultAsync(u => u.ApplicationUserId == userid);

            if (_user == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("ChangePassword", _user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> ChangePassword(ApplicationUser applicationUser)
        {
            var userid = _session.GetInt32("imouloggedinuserid");

            if (userid != applicationUser.ApplicationUserId)
            {
                return RedirectToAction("Index", "Error");
            }
            try
            {
                var _user = new ApplicationUser()
                {
                    ApplicationUserId = applicationUser.ApplicationUserId,
                    ConfirmPassword = BCrypt.Net.BCrypt.HashPassword(applicationUser.ConfirmPassword),
                    Password = BCrypt.Net.BCrypt.HashPassword(applicationUser.Password),
                    DateCreated = applicationUser.DateCreated,
                    DateLastModified = DateTime.Now,
                    CreatedBy = applicationUser.CreatedBy,
                    LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid")),
                    Email = applicationUser.Email,
                    FirstName = applicationUser.FirstName,
                    LastName = applicationUser.LastName,
                    RoleId = applicationUser.RoleId

                };

                TempData["user"] = "You have successfully changed " + applicationUser.DisplayName + " password.";
                TempData["notificationType"] = NotificationType.Success.ToString();

                _database.ApplicationUsers.Update(_user);
                await _database.SaveChangesAsync();

                return Json(new { success = true });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Password Recovery

        [HttpGet]
        public IActionResult PasswordRecovery()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordRecovery(ApplicationUser user)
        {
            var _user = await _database.ApplicationUsers.SingleOrDefaultAsync(u => u.Email == user.Email);
            
            if(_user == null)
            {
                ViewData["appuser"] = "Sorry the email you enter does not exist. Check the email and try again";
                return View();
            }

            new Mailer().PasswordRecovery(new AppConfig().ForgotPasswordHtml, _user);

            _session.SetString("recoveriedemail", _user.Email);

            return RedirectToAction("Success", "Account");

        }

        #endregion

        #region Success

        [HttpGet]
        public IActionResult Success()
        {
            ViewData["recoveriedemail"] = _session.GetString("recoveriedemail");
            if(ViewData["recoveriedemail"] == null)
            {
                return RedirectToAction("Index", "Error");
            }
            return View();
        }

        #endregion

        #region Application User Exist

        private bool ApplicationUserExists(int id)
        {
            return _database.ApplicationUsers.Any(e => e.ApplicationUserId == id);
        }

        #endregion

    }
}