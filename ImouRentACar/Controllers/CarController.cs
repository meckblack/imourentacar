using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImouRentACar.Data;
using ImouRentACar.Models;
using ImouRentACar.Models.Enums;
using ImouRentACar.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ImouRentACar.Areas
{
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IHostingEnvironment _environment;

        #region Constructor

        public CarController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHostingEnvironment environment)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
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

            var _cars = _database.Cars;
            
            var userid = _session.GetInt32("imouloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);

            var roleid = _user.RoleId;

            var role = _database.Roles.Find(roleid);

            ViewData["userrole"] = role.Name;

            if (role.CanManageCars == false && role.CanDoEverything == false)
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

            ViewData["loggedinuserfullname"] = _user.DisplayName;
            
            return View(await _cars.ToListAsync());
        }

        #endregion
        
        #region Create

        [HttpGet]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Create()
        {
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
            
            ViewBag.CarBrands = new SelectList(_database.CarBrands, "CarBrandId", "Name");
            return View();
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Create(Car car, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("null_img", "File not selected");
            }
            else
            {
                var carBrandid = _session.GetInt32("carbrandid");
                var allCars = await _database.Cars.Where(ac => ac.CarBrandId == carBrandid).AnyAsync(ac => ac.Name == car.Name);

                if (allCars == true)
                {
                    TempData["car"] = "You cannot add " + car.Name + " because it already exist!!!";
                    TempData["notificationType"] = NotificationType.Error.ToString();
                    return RedirectToAction("Index");
                }

                var fileinfo = new FileInfo(file.FileName);
                var filename = DateTime.Now.ToFileTime() + fileinfo.Extension;
                var uploads = Path.Combine(_environment.WebRootPath, "UploadedFiles\\Cars");
                if(file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                if (ModelState.IsValid)
                {
                    car.Image = filename;
                    car.CarAvaliability = Avaliability.Avaliable;
                    car.CreatedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));
                    car.LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));
                    car.DateCreated = DateTime.Now;
                    car.DateLastModified = DateTime.Now;

                    await _database.Cars.AddAsync(car);
                    await _database.SaveChangesAsync();

                    TempData["car"] = "You have successfully added " + car.Name + "!!!";
                    TempData["notificationType"] = NotificationType.Success.ToString();
                    
                    return RedirectToAction("Index");
                }
            }

            //ViewBag.CarBrands = new SelectList(_database.CarBrands, "CarBrandId", "Name", car.CarBrandId);
            return View(car);
        }

        #endregion

        #region Edit

        [HttpGet]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _database.Cars.SingleOrDefaultAsync(c => c.CarId == id);

            if(car == null)
            {
                return NotFound();
            }

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



            ViewBag.CarBrands = new SelectList(_database.CarBrands, "CarBrandId", "Name");
            return View(car);
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Edit (int? id, Car car, IFormFile file)
        {
            if (id != car.CarId)
            {
                return NotFound();
            }

            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("null_img", "File not selected");
            }
            else
            {
                var fileinfo = new FileInfo(file.FileName);
                var filename = DateTime.Now.ToFileTime() + fileinfo.Extension;
                var uploads = Path.Combine(_environment.WebRootPath, "UploadedFiles\\Cars");
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        car.Image = filename;
                        car.LastModifiedBy = Convert.ToInt32(_session.GetInt32("imouloggedinuserid"));
                        car.DateLastModified = DateTime.Now;

                        TempData["car"] = "You have successfully modified " + car.Name + "!!!";
                        TempData["notificationType"] = NotificationType.Success.ToString();

                        _database.Cars.Update(car);
                        await _database.SaveChangesAsync();

                        return RedirectToAction("Index");
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CarExists(car.CarId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }    
            }

            ViewBag.CarBrands = new SelectList(_database.CarBrands, "CarBrandId", "Name", car.CarBrandId);
            return View(car);
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
            if (role.CanManageApplicationUsers == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return NotFound();
            }

            var _car = await _database.Cars.SingleOrDefaultAsync(c => c.CarId == id);

            if (_car == null)
            {
                return NotFound();
            }

            var brandid = _car.CarBrandId;
            var _carbrand = await _database.CarBrands.FindAsync(brandid);

            var creatorid = _car.CreatedBy;
            var creator = await _database.ApplicationUsers.FindAsync(creatorid);
            ViewData["creatorby"] = creator.DisplayName;

            var modifierid = _car.LastModifiedBy;
            var modifier = await _database.ApplicationUsers.FindAsync(modifierid);
            ViewData["modifiedby"] = modifier.DisplayName;

            ViewData["brandname"] = _carbrand.Name;


            return PartialView(_car);
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
            if (role.CanManageApplicationUsers == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return NotFound();
            }

            var brand = await _database.Cars.SingleOrDefaultAsync(b => b.CarId == id);

            if (brand == null)
            {
                return NotFound();
            }

            return PartialView("Delete", brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var car = await _database.Cars.SingleOrDefaultAsync(b => b.CarId == id);

            if (car != null)
            {
                _database.Cars.Remove(car);
                await _database.SaveChangesAsync();

                TempData["car"] = "You have successfully deleted " + car.Name + " !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return View("Index", new { id = _session.GetInt32("carbrandid") });
        }

        #endregion

        #region Avaliable Cars

        [HttpGet]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> AvaliableCars()
        {
            var userid = _session.GetInt32("imouloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            ViewData["loggedinuserfullname"] = _user.DisplayName;

            var roleid = _user.RoleId;

            var role = _database.Roles.Find(roleid);

            ViewData["userrole"] = role.Name;

            if (role.CanManageCars == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            var avaliableCars = await _database.Cars.Where(c => c.CarAvaliability == Avaliability.Avaliable).ToListAsync();

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

            return View(avaliableCars);

        }

        #endregion

        #region Rented Cars

        [HttpGet]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> RentedCars()
        {
            var userid = _session.GetInt32("imouloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            ViewData["loggedinuserfullname"] = _user.DisplayName;

            var roleid = _user.RoleId;

            var role = _database.Roles.Find(roleid);

            ViewData["userrole"] = role.Name;

            if (role.CanManageCars == false && role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            var rentedCars = await _database.Cars.Where(c => c.CarAvaliability == Avaliability.Rented).ToListAsync();

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

            return View(rentedCars);
        }

        #endregion

        #region View All Cars

        [HttpGet]
        public async Task<IActionResult> ViewAllCars()
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
            
            var _allCars = await _database.Cars.ToListAsync();

            var customerObject = _session.GetString("imouloggedincustomer");
            if (customerObject == null)
            {
                return View(_allCars);
            }

            var _customer = JsonConvert.DeserializeObject<Customer>(customerObject);
            TempData["customername"] = _customer.DisplayName;

            return View(_allCars);
        }

        #endregion

        #region Car Exist

        private bool CarExists(int id)
        {
            return _database.Cars.Any(e => e.CarId == id);
        }

        #endregion

        #region Get Data

        public JsonResult CarBrand()
        {
            var brand = _database.CarBrands.ToArray();
            return Json(new { brands = brand });
        }

        public JsonResult GetCarByBrand(int brand)
        {
            var car = _database.Cars.Where(s => s.CarBrandId == brand).ToArray();
            return Json(new { cars = car });
        }

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