﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImouRentACar.Data;
using ImouRentACar.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Dynamic;
using ImouRentACar.Services;
using ImouRentACar.Models.Enums;

namespace ImouRentACar.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public CustomerController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Register

        [HttpGet]
        [Route("customer/register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var allCustomers = await _database.Customers.ToListAsync();

                if(allCustomers.Any(c => c.Email == customer.Email))
                {
                    TempData["emailexists"] = "Sorry a customer with this email address exists";

                    return View(customer);
                }
                else
                {
                    //var generator = new Random();
                    //var number = generator.Next(0, 100000000).ToString();

                    var stringGenerator = new RandomStringGenerator();
                    var memberid = stringGenerator.RandomString(8);

                    var _customer = new Customer()
                    {
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Email = customer.Email,
                        MobileNumber = customer.MobileNumber,
                        Password = BCrypt.Net.BCrypt.HashPassword(customer.Password),
                        ConfrimPassword = BCrypt.Net.BCrypt.HashPassword(customer.ConfrimPassword),
                        MemberId = memberid
                        
                    };

                    await _database.Customers.AddAsync(_customer);
                    await _database.SaveChangesAsync();

                    return RedirectToAction("Signin", "Customer");
                }
                
            }

            return RedirectToAction("Signin", "Customer");
        }

        #endregion

        #region Signin

        [HttpGet]
        [Route("customer/signin")]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn (Customer customer)
        {
            var _customer = await _database.Customers.FirstOrDefaultAsync(c => c.Email == customer.Email);

            if (_customer == null)
            {
                ViewData["mismatch"] = "Email and Password do not match";
                return View(customer);
            }
            if(_customer != null)
            {
                var password = BCrypt.Net.BCrypt.Verify(customer.Password, _customer.Password);
                if(password == true)
                {
                    _session.SetString("imouloggedincustomer", JsonConvert.SerializeObject(_customer));
                    _session.SetInt32("imouloggedincustomerid", _customer.CustomerId);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewData["mismatch"] = "Email and Password do not match";
                }
            }

            return View(customer);
        }

        #endregion
        
        #region SignOut

        [Route("customer/signout")]
        public IActionResult SignOut()
        {
            _database.Dispose();
            _session.Clear();
            return RedirectToAction("Index", "Home");

        }

        #endregion

        #region Index

        [SessionExpireFilterAttribute]
        [Route("customer/index")]
        public async Task<IActionResult> Index()
        {
            var userid = _session.GetInt32("imouloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            var roleid = _user.RoleId;
            var _role = await _database.Roles.SingleOrDefaultAsync(r => r.RoleId == roleid && r.CanManageCustomers == true);

            ViewData["loggedinuserfullname"] = _user.DisplayName;
            ViewData["canmanagecustomer"] = _role;
            
            var role = _database.Roles.Find(roleid);

            ViewData["userrole"] = role.Name;

            if (role.CanManageCustomers == false && role.CanDoEverything == false)
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

            var customers = await _database.Customers.ToListAsync();
            return View(customers);
        }

        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            var userObject = _session.GetString("imouloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);
            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);
            if (role.CanManageCustomers == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var customer = await _database.Customers.SingleOrDefaultAsync(b => b.CustomerId == id);

            if (customer == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _database.Customers.SingleOrDefaultAsync(b => b.CustomerId == id);

            if (customer != null)
            {
                _database.Customers.Remove(customer);
                await _database.SaveChangesAsync();

                TempData["customer"] = "You has successfully deleted " + customer.DisplayName + " !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return View("Index");
        }

        #endregion

        #region View Profile

        [HttpGet]
        [Route("customer/viewprofile")]
        public async Task<IActionResult> ViewProfile()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Logos = GetLogos();
            mymodel.Contacts = GetContacts();

            foreach (Contact contact in mymodel.Contacts)
            {
                ViewData["contactnumber"] = contact.MobileNumberOne;
            }

            foreach (Logo logo in mymodel.Logos)
            {
                ViewData["imageoflogo"] = logo.Image;
            }

            var customerObject = _session.GetString("imouloggedincustomer");
            if (customerObject == null)
            {
                TempData["error"] = "Sorry your session has expired. Kindly signin again and try again.";
                return RedirectToAction("Signin", "Customer");
            }

            var _customer = JsonConvert.DeserializeObject<Customer>(customerObject);
            TempData["customername"] = _customer.DisplayName;


            var id = _session.GetInt32("imouloggedincustomerid");
            if(id == null)
            {
                TempData["error"] = "Sorry your session has expired. Kindly signin again and try again.";
                return RedirectToAction("Signin", "Customer");
            }

            var customer = await _database.Customers.SingleOrDefaultAsync(c => c.CustomerId == id);

            if(customer == null)
            {
                return RedirectToAction("Index", "Error");
            }
            
            return View("ViewProfile", customer);
        }

        #endregion

        #region Edit Profile

        [HttpGet]
        [Route("customer/editprofile")]
        public async Task<IActionResult> EditProfile()
        {
            var id = _session.GetInt32("imouloggedincustomerid");
            if(id == null)
            {
                TempData["error"] = "Sorry your session has expired. Kindly signin again and try again.";
                return RedirectToAction("Signin", "Customer");
            }

            var customer = await _database.Customers.SingleOrDefaultAsync(c => c.CustomerId == id);

            if (customer == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("EditProfile", customer);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(Customer customer)
        {
            var id = _session.GetInt32("imouloggedincustomerid");
            if (id == null)
            {
                TempData["error"] = "Sorry your session has expired. Kindly signin again and try again.";
                return RedirectToAction("Signin", "Customer");
            }
            if (id != customer.CustomerId)
            {
                return RedirectToAction("Index", "Error");
            }

            try
            {

                _database.Customers.Update(customer);
                await _database.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        #endregion

        #region Change Password

        [HttpGet]
        [Route("customer/changepassword")]
        public IActionResult ChangePassword()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Logos = GetLogos();
            mymodel.Contacts = GetContacts();

            foreach (Contact contact in mymodel.Contacts)
            {
                ViewData["contactnumber"] = contact.MobileNumberOne;
            }

            foreach (Logo logo in mymodel.Logos)
            {
                ViewData["imageoflogo"] = logo.Image;
            }

            var customerObject = _session.GetString("imouloggedincustomer");
            if (customerObject == null)
            {
                TempData["error"] = "Sorry your session has expired. Kindly signin again and try again.";
                return RedirectToAction("Signin", "Customer");
            }

            var _customer = JsonConvert.DeserializeObject<Customer>(customerObject);
            TempData["customername"] = _customer.DisplayName;
            
            return View(_customer);
        }

        [HttpPost, ActionName("ChangePassword")]
        public async Task<IActionResult> ChangePasswordConfirm(Customer customer)
        {
            var id = _session.GetInt32("imouloggedincustomerid");
            if (id == null)
            {
                TempData["error"] = "Sorry your session has expired. Kindly signin again and try again.";

                return RedirectToAction("Signin", "Customer");
            }

            if(id != customer.CustomerId)
            {
                return RedirectToAction("Signin", "Customer");
            }

            try
            {
                customer.Password = BCrypt.Net.BCrypt.HashPassword(customer.Password);
                customer.ConfrimPassword = BCrypt.Net.BCrypt.HashPassword(customer.ConfrimPassword);


                _database.Customers.Update(customer);
                await _database.SaveChangesAsync();

                TempData["passwordcustomer"] = "You have successfully changed your password";
                return RedirectToAction("SignOut", "Customer");

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        #endregion

        #region Veiw Bookings
        
        [HttpGet]
        [Route("customer/viewcarrentals")]
        public async Task<IActionResult> ViewCarRentals()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Logos = GetLogos();
            mymodel.Contacts = GetContacts();

            foreach (Contact contact in mymodel.Contacts)
            {
                ViewData["contactnumber"] = contact.MobileNumberOne;
            }

            foreach (Logo logo in mymodel.Logos)
            {
                ViewData["imageoflogo"] = logo.Image;
            }

            var customerObject = _session.GetString("imouloggedincustomer");
            if (customerObject == null)
            {
                TempData["error"] = "Sorry your session has expired. Kindly signin again and try again.";
                return RedirectToAction("Signin", "Customer");
            }

            var _customer = JsonConvert.DeserializeObject<Customer>(customerObject);
            TempData["customername"] = _customer.DisplayName;

            var id = _session.GetInt32("imouloggedincustomerid");
            if (id == null)
            {
                TempData["error"] = "Sorry your session has expired. Kindly signin again and try again.";
                return RedirectToAction("Signin", "Customer");
            }

            var _id = _session.GetInt32("imouloggedincustomerid");
            var allCarRentals = await _database.RentACars.Where(owt => owt.CustomerId == _id).ToListAsync();

            return View(allCarRentals);
        }

        #endregion

        #region Password Recovery

        [HttpGet]
        [Route("customer/passwordrecovery")]
        public IActionResult PasswordRecovery()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordRecovery(Customer customer)
        {
            var _customer = await _database.Customers.SingleOrDefaultAsync(u => u.Email == customer.Email);

            if (_customer == null)
            {
                ViewData["customer"] = "Sorry the email you enter does not exist. Check the email and try again";
                return View();
            }

            new Mailer().PasswordRecovery(new AppConfig().ForgotPasswordHtml, _customer);

            _session.SetString("recoveriedemail", _customer.Email);

            return RedirectToAction("Success", "Account");

        }

        #endregion

        #region Success

        [HttpGet]
        [Route("customer/success")]
        public IActionResult Success()
        {
            ViewData["recoveriedemail"] = _session.GetString("recoveriedemail");
            if (ViewData["recoveriedemail"] == null)
            {
                return RedirectToAction("Index", "Error");
            }
            return View();
        }

        #endregion

        #region New Password

        [HttpGet]
        [Route("customer/newpassword")]
        public IActionResult NewPassword(string user)
        {
            if(user == null)
            {
                return RedirectToAction("Index", "Error");
            }

            _session.SetInt32("id", Convert.ToInt32(user));
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> NewPassword(Customer customer)
        {
            var id = _session.GetInt32("id");
            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            try
            {
                var _customer = new Customer()
                {
                    CustomerId = customer.CustomerId,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    MobileNumber = customer.MobileNumber,
                    Title = customer.Title,
                    Gender = customer.Gender,
                    MemberId = customer.MemberId,
                    Password = BCrypt.Net.BCrypt.HashPassword(customer.Password),
                    ConfrimPassword = BCrypt.Net.BCrypt.HashPassword(customer.ConfrimPassword)
                };


                _database.Customers.Update(_customer);
                await _database.SaveChangesAsync();

                TempData["passwordcustomer"] = "You have successfully changed your password";
                return RedirectToAction("Index", "Home");

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region Get Data

        private List<Logo> GetLogos()
        {
            var _logos = _database.Logos.ToList();

            return _logos;
        }

        private List<Contact> GetContacts()
        {
            var _contact = _database.Contacts.ToList();
            return _contact;
        }
        
        #endregion

    }
}
