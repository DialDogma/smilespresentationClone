using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmileSClaimPayBack.Controllers
{
    [AllowAnonymous]
    public class ReportController : Controller
    {
        // GET: Report
        [AllowAnonymous]
        public ActionResult TPAReportHeader()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult TPAReportFooter()
        {
            return View();
        }
    }
}