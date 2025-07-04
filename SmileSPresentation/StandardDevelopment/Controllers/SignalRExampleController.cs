using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StandardDevelopment.Controllers
{
    public class SignalRExampleController : Controller
    {
        // GET: SignalR
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignalRMonitor()
        {
            return View();
        }

        public ActionResult SignalRClient()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendFromCodeBehind_All(string name, string message)
        {
            //Do something
            var x = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            x.Clients.All.ReceivePublicMessage(name, message + " (from code behind [All])");
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SendFromCodeBehind_Group(string groupName, string name, string message)
        {
            //Do something
            var x = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            x.Clients.Group(groupName).ReceiveGroupMessage(name, message + "(from code behind [group])");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}