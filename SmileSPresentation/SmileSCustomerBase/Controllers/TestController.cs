using Microsoft.AspNet.SignalR;
using SmileSCustomerBase.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmileSCustomerBase.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignalRClient()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignalRClient(FormCollection form)
        {
            //Do something
            var x = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
            x.Clients.All.RefreshQueueManagerResult("Hey I am updated from code behind");
            return View();
        }

        public ActionResult SignalRMonitor()
        {
            return View();
        }

        public ActionResult ExportExcel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ExportExcel(FormCollection form)
        {
            var x = new List<testX>();
            x.Add(new testX() { Id = 1, Detail = "a" });
            x.Add(new testX() { Id = 2, Detail = "b" });
            x.Add(new testX() { Id = 3, Detail = "c" });
            x.Add(new testX() { Id = 4, Detail = "d" });
            x.Add(new testX() { Id = 5, Detail = "e" });

            var dt1 = GlobalFunction.ToDataTable(x);
            ExcelManager.ExportToExcel(this.HttpContext, "Report_CustomerBase", dt1, "Report_CustomerBase");
            return View("");
        }
    }
}

public class testX
{
    public int Id { get; set; }
    public string Detail { get; set; }
}