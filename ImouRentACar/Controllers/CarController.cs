using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImouRentACar.Data;
using ImouRentACar.Models;
using ImouRentACar.Models.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> Index()
        {
            //Counters
            ViewData["carbrandcounter"] = _database.CarBrands.Count();
            ViewData["caravaliablecounter"] = _database.Cars.Where(c => c.CarAvaliability == CarAvaliability.Avaliable).Count();
            ViewData["carrentedout"] = _database.Cars.Where(c => c.CarAvaliability == CarAvaliability.Rented).Count();
            ViewData["contactcounter"] = _database.Contacts.Count();
            ViewData["enquirycounter"] = _database.Enquiries.Count();

            var _cars = _database.Cars;
            
            var userid = _session.GetInt32("imouloggedinuserid");
            var user = await _database.ApplicationUsers.FindAsync(userid);
            ViewData["loggedinuserfullname"] = user.DisplayName;
            
            return View(await _cars.ToListAsync());
        }

        #endregion

        #region Create

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userid = _session.GetInt32("imouloggedinuserid");
            var user = await _database.ApplicationUsers.FindAsync(userid);
            ViewData["loggedinuserfullname"] = user.DisplayName;

            ViewBag.CarBrands = new SelectList(_database.CarBrands, "CarBrandId", "Name");
            return View();
        }

        [HttpPost]
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
                    car.CarAvaliability = CarAvaliability.Avaliable;
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

            ViewBag.CarBrands = new SelectList(_database.CarBrands, "CarBrandId", "Name", car.CarBrandId);
            return View(car);
        }

        #endregion

        #region Edit

        [HttpGet]
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
            var user = await _database.ApplicationUsers.FindAsync(userid);
            ViewData["loggedinuserfullname"] = user.DisplayName;

            ViewBag.CarBrands = new SelectList(_database.CarBrands, "CarBrandId", "Name");
            return View(car);
        }

        [HttpPost]
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
                        car.CarAvaliability = CarAvaliability.Avaliable;

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

        public async Task<IActionResult> Details(int? id)
        {
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
        //[SessionExpireFilterAttribute]
        public async Task<IActionResult> Delete(int? id)
        {
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
        //[SessionExpireFilterAttribute]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var car = await _database.Cars.SingleOrDefaultAsync(b => b.CarId == id);

            if (car != null)
            {
                _database.Cars.Remove(car);
                await _database.SaveChangesAsync();

                TempData["car"] = "You have successfully deleted  " + car.Name + " !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return View("Index", new { id = _session.GetInt32("carbrandid") });
        }

        #endregion

        #region Avaliable Cars

        [HttpGet]
        public async Task<IActionResult> AvaliableCars()
        {
            var avaliableCars = await _database.Cars.Where(c => c.CarAvaliability == CarAvaliability.Avaliable).ToListAsync();
            return View(avaliableCars);
        }

        #endregion

        #region Rented Cars

        [HttpGet]
        public async Task<IActionResult> RentedCars()
        {
            var rentedCars = await _database.Cars.Where(c => c.CarAvaliability == CarAvaliability.Rented).ToListAsync();
            return View(rentedCars);
        }

        #endregion

        #region View All Cars

        [HttpGet]
        public async Task<IActionResult> ViewAllCars()
        {
            ViewData["imageoflogo"] = _session.GetString("imageoflogo");
            ViewData["contactnumber"] = _session.GetString("contactnumber");

            var _allCars = await _database.Cars.ToListAsync();
            return View(_allCars);
        }

        #endregion

        #region Car Exist

        private bool CarExists(int id)
        {
            return _database.Cars.Any(e => e.CarId == id);
        }

        #endregion

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
        
    }
}