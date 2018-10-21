using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ImouRentACar.Controllers
{
    public class ErrorController : Controller
    {
        #region Index

        [Route("error/index")]
        public IActionResult Index()
        {
            return View();
        }

        #endregion
    }
}