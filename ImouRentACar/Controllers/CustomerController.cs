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
                    var _customer = new Customer()
                    {
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Email = customer.Email,
                        MobileNumber = customer.MobileNumber,
                        Password = BCrypt.Net.BCrypt.HashPassword(customer.Password),
                        ConfrimPassword = BCrypt.Net.BCrypt.HashPassword(customer.ConfrimPassword)
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

        #region Dashboard

        [HttpGet]
        public IActionResult Dashboard()
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
            var _customer = JsonConvert.DeserializeObject<Customer>(customerObject);

            TempData["customername"] = _customer.DisplayName;
            
            return RedirectToAction("Home", "Index");
        }

        #endregion

        #region SignOut
        
        public IActionResult SignOut()
        {
            _database.Dispose();
            _session.Clear();
            return RedirectToAction("Index", "Home");

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
            var roleid = _user.RoleId;
            var role = await _database.Roles.SingleOrDefaultAsync(r => r.RoleId == roleid && r.CanManageCustomers == true);

            ViewData["loggedinuserfullname"] = _user.DisplayName;
            ViewData["canmanagecustomer"] = role;

            var customers = await _database.Customers.ToListAsync();
            return View(customers);
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

            var customer = await _database.Customers.SingleOrDefaultAsync(b => b.CustomerId == id);

            if (customer == null)
            {
                return NotFound();
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

                TempData["customer"] = "You have successfully deleted " + customer.DisplayName + " !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return View("Index");
        }

        #endregion
    }
}
