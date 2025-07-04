using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmileSMobileWebUI.Controllers
{
    public class EmployeesController : Controller
    {
        // GET: Employees
        public ActionResult EmployeeList()
        {
            var mobilekey = new SmileSMobileAppToken.AlignToken().GetAlignToken();
            ViewBag.MBKey = mobilekey;
            return View(ViewBag);
        }
    }
}