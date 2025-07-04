using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SmileSTNIDocumentDownload.Helper
{
    public class Authorization : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            var loggedIn = filterContext.HttpContext.Session["IsLoggedIn"];

            if (loggedIn != null)
            {
                var user = filterContext.HttpContext.Session["IsLoggedIn"].ToString();
                if (user != "")
                {
                    OnActionExecuting(filterContext);
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Login", controller = "Auth" }));
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Login", controller = "Auth" }));
            }
        }

    }
}