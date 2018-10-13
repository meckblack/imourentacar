using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImouRentACar.Services
{
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.GetString("imouloggedinuser") == null ||
                filterContext.HttpContext.Session.GetInt32("imouloggedinuserid") == null)
                filterContext.Result =
                    new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            {"controller", "Account"},
                            {"action", "SignIn"},
                            {"returnUrl", "sessionExpired"},
                        });


            base.OnActionExecuting(filterContext);
        }
    }
}
