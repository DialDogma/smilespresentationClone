using SmileSDataCenterV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmileSDataCenterV2.Controllers
{
    public class InsuranceInformationManagementController : Controller
    {
        // GET: InsuranceInformationManagement
        private DataCenterV1Entities _db;

        public ActionResult Index()
        {
            return View();
        }
    }
}