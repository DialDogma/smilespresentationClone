using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmileSMobileWebUI.Controllers
{
    public class FirstPageController : Controller
    {
        // GET: FirstPage
        public ActionResult Index()
        {
            ViewBag.APIUrlFirstPage1 = Properties.Settings.Default["APIUrlFirstPage"].ToString();
            return View();
        }

        public ActionResult IndexV2()
        {
            ViewBag.APIUrlFirstPage1 = Properties.Settings.Default["APIUrlFirstPage"].ToString();
            return View();
        }
    }
}