using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SmileSPACommunity.Models;


namespace SmileSPACommunity.Controllers
{

    public class SystemController : Controller
    {
        private PACommunityEntities _db;

        public SystemController()   
        {
            _db = new PACommunityEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }



        // GET: System
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SystemGuideDownload()
        {
            return View();
        }

        public ActionResult SystemGuideDownloadForPAPersonnel()
        {
            return View();
        }

        public ActionResult GetSystemGuideData(int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null,int? productTypeId = null)
        {

            var result = _db.usp_ProgramManual_Select(productTypeId,pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        


        public void DownloadSystem(string path)
        {
            //string filePath = Server.MapPath(@"D:\Work\Miscellaneous\input_NewApp.txt");

            //var filePath = Directory.GetFiles(@"D:\Work\Miscellaneous\input_NewApp.txt");

            //HttpResponse res = System.Web.HttpContext.Current.Response;
            //res.Clear();
            //res.AppendHeader("content-disposition", "attachment; filename=" + filePath);
            //res.ContentType = "application/octet-stream";
            //res.WriteFile(filePath.);
            ////res.TransmitFile(filePath);
            //res.Flush();
            //res.End();

            System.IO.FileInfo file = new System.IO.FileInfo(@path);
            if (file.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(file.FullName);
                Response.End();

            }
            else
            {
                Response.Write("This file does not exist.");
            }
        }
    }
}