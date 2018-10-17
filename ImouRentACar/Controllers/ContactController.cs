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
using System.Dynamic;
using Newtonsoft.Json;

namespace ImouRentACar.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public ContactController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Index
        
        public IActionResult Index()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Logos = GetLogos();
            mymodel.Contacts = GetContacts();


            
            foreach (Contact contact in mymodel.Contacts)
            {
                _session.SetString("contactnumber", contact.MobileNumberOne);

                ViewData["contactnumber"] = contact.MobileNumberOne;
                ViewData["contactaddress"] = contact.Address;
                ViewData["contactboxoffice"] = contact.BoxOfficeNumber;
                ViewData["contactnumber1"] = contact.MobileNumberOne;
                ViewData["contactnumber2"] = contact.MobileNumberTwo;
            }

            foreach (Logo logo in mymodel.Logos)
            {
                _session.SetString("imageoflogo", logo.Image);

                ViewData["imageoflogo"] = logo.Image;
            }

            var customerObject = _session.GetString("imouloggedincustomer");
            if (customerObject == null)
            {
                return View();
            }

            var _customer = JsonConvert.DeserializeObject<Customer>(customerObject);
            TempData["customername"] = _customer.DisplayName;

            return View();
        }

        #endregion

        #region Create

        [HttpGet]
        public IActionResult Create()
        {
            var contact = new Contact();
            return PartialView("Create", contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                var _contact = new Contact()
                {
                    MobileNumberOne = contact.MobileNumberOne,
                    MobileNumberTwo = contact.MobileNumberTwo,
                    Address = contact.Address,
                    BoxOfficeNumber = contact.BoxOfficeNumber,
                    DateCreated = DateTime.Now,
                    CreatedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid")),
                    DateLastModified = DateTime.Now,
                    LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"))
                };

                await _database.Contacts.AddAsync(_contact);
                await _database.SaveChangesAsync();

                TempData["landing"] = "You have successfully added the contact details";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return View("Index");
        }

        #endregion

        #region Edit

        // GET: Contact/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _database.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return PartialView("Edit", contact);
        }

        // POST: Contact/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [SessionExpireFilterAttribute]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContactId,Address,MobileNumberOne,MobileNumberTwo,BoxOfficeNumber,CreatedBy,DateCreated,DateLastModified,LastModifiedBy")] Contact contact)
        {
            if (id != contact.ContactId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    contact.DateLastModified = DateTime.Now;
                    contact.LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));

                    _database.Update(contact);
                    await _database.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.ContactId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["landing"] = "You have successfully modified the contact details";
                TempData["notificationType"] = NotificationType.Success.ToString();
                return Json(new { success = true });
            }
            return View("Index");
        }

        #endregion

        #region Delete

        // GET: Contact/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _database.Contacts
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (contact == null)
            {
                return NotFound();
            }

            return PartialView("Delete", contact);
        }

        // POST: Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var _contact = await _database.Contacts.FindAsync(id);
            if (_contact != null)
            {
                _database.Contacts.Remove(_contact);
                await _database.SaveChangesAsync();
                TempData["landing"] = "You have successfully deleted the contact details";
                TempData["notificationType"] = NotificationType.Success.ToString();
                return Json(new { success = true });
            }
            return View("Index");
        }

        #endregion

        #region View

        // GET: Contact/View/5
        public async Task<IActionResult> View(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _database.Contacts
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (contact == null)
            {
                return NotFound();
            }

            return PartialView("View", contact);
        }

        #endregion

        #region Exists

        private bool ContactExists(int id)
        {
            return _database.Contacts.Any(e => e.ContactId == id);
        }

        #endregion

        #region Get Logos

        private List<Logo> GetLogos()
        {
            var _logos = _database.Logos.ToList();

            return _logos;
        }

        #endregion
        
        #region Get Contacts

        private List<Contact> GetContacts()
        {
            var _contact = _database.Contacts.ToList();
            return _contact;
        }

        #endregion

    }
}
