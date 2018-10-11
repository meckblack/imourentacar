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
using Newtonsoft.Json;
using System.Dynamic;

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
            var customer = new Customer();
            return PartialView("Register", customer);
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

                    return Json(new { success = true });
                }
                
            }

            return RedirectToAction("Signin", "Customer");
        }

        #endregion

        #region Signin

        [HttpGet]
        public IActionResult SignIn()
        {
            var customer = new Customer();
            return PartialView("SignIn", customer);
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
                    
                    return RedirectToAction("Dashboard", "Customer");
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
            
            return View("Dashboard");
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
        
        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _database.Customers.FindAsync(id);
            _database.Customers.Remove(customer);
            await _database.SaveChangesAsync();
            return RedirectToAction(nameof(Dashboard));
        }

        private bool CustomerExists(int id)
        {
            return _database.Customers.Any(e => e.CustomerId == id);
        }
    }
}
