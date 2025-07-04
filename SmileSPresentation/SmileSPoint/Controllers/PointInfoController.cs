using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmileSPoint.Controllers
{
    public class PointInfoController : Controller
    {
        // GET: PointInfo
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Portfolio(string accountType, string referenceId)
        {
            return View();
        }

        public ActionResult ShowPoint(string id)
        {
            ViewBag.UserId = id;
            return View();
        }
    }
}