using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmileSUnderwrite.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            ViewBag.AppId = "61130008";
            //ViewBag.Product = "100E-เลือกสิทธิ์";
            //ViewBag.CoverDate = "16/05/2017-16/05/2018";
            //ViewBag.SchoolName = "โรงเรียนสวนกุหลาบวิทยาลัย นนทบุรี";
            //ViewBag.Province = "นนทบุรี";
            //ViewBag.District = "ปากเกร็ด";
            //ViewBag.CoordinateName = "ณัฐวัฒน์ เกตุวิชิต";
            //ViewBag.CoordinateMobileNo = "0814142623";
            //ViewBag.DirectorName = "ประยุทธ์ จันทร์โอชา";
            //ViewBag.DirectorMobileNo = "08XXXXXXX";

            return View();
        }

        public ActionResult SignalRClient()
        {
            return View();
        }

        public ActionResult SignalRMonitor()
        {
            return View();
        }
    }
}