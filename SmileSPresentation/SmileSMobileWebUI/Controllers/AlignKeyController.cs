using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSMobileAppToken;

namespace SmileSMobileWebUI.Controllers
{
    public class AlignKeyController : Controller
    {
        // GET: AlignKey
        public ActionResult Index()
        {
            var key = new AlignToken().GetAlignToken();
            ViewBag.Key = key;
            return View();
        }
    }
}