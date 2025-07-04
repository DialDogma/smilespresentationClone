using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StandardDevelopment.Controllers
{
    public class SSOController : Controller
    {
        // GET: SSO
        public ActionResult Index()
        {
            return View();
        }

        public FileResult DownloadSSOManual()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Documents/";
            string fileName = "SSO Manual.xlsx";
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + fileName);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}