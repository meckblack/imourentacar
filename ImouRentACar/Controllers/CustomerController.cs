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
        public IActionResult Signin()
        {
            var customer = new Customer();
            return PartialView("Signin", customer);
        }

        [HttpPost]
        public async Task<IActionResult> Signin (Customer customer)
        {
            var _customer = await _database.Customers.FirstOrDefaultAsync(c => c.Email == customer.Email);

            if (_customer == null)
            {
                ViewData["mismatch"] = "Email and Password do not match";
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

        // GET: Customer
        public async Task<IActionResult> Index()
        {
            return View(await _database.Customers.ToListAsync());
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _database.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,FirstName,LastName,Email,MobileNumber,Password,ConfrimPassword")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _database.Add(customer);
                await _database.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _database.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,FirstName,LastName,Email,MobileNumber,Password,ConfrimPassword")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _database.Update(customer);
                    await _database.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _database.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _database.Customers.FindAsync(id);
            _database.Customers.Remove(customer);
            await _database.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _database.Customers.Any(e => e.CustomerId == id);
        }
    }
}
