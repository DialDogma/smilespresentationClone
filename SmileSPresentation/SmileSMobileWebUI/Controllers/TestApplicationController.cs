using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSMobileWebUI.PersonService;

namespace SmileSMobileWebUI.Controllers
{
    public class TestApplicationController : Controller
    {
        // GET: TestApplication
        public ActionResult Application()
        {
            var client = new PersonServiceClient();
            var lstTitleResults = client.GetTitle(null, null).ToList();
            var lstTitle = new SelectList(lstTitleResults, "Title_ID", "TitleDetail");
            ViewBag.lstTitle = lstTitle;
            return View();
        }
    }
}