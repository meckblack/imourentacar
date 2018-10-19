using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImouRentACar.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImouRentACar.Controllers
{
    public class OneWayTripController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public OneWayTripController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Book

        public async Task<IActionResult> Book()
        {

        }

        #endregion

        public IActionResult Index()
        {
            return View();
        }
    }
}