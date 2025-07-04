using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSTNIDocumentDownload.Helper;
using SmileSTNIDocumentDownload.Models;

namespace SmileSTNIDocumentDownload.Controllers
{
    [Authorization]
    public class DocumentDownloadController : Controller
    {
        // GET: DocumentDownload
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoadDataTables(int draw,int pageStart, int pageSize, DateTime? dateStart , DateTime? dateEnd )
        {
            //add dateEnd
            if (dateEnd.HasValue)
            {
                dateEnd = dateEnd.Value.AddDays(1);
            }

            var result = new CriticalIllnessEntities().usp_DocumentFileControlBatchLogHeader_Select(pageStart,pageSize,"","",dateStart,dateEnd).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public FileResult GetFile(int id)
        {
            //get objLog
            var db = new CriticalIllnessEntities();
            var objLog = db.DocumentFileControlBatchLogHeaders.Where(x => x.DocumentFileControlBatchLogHeaderId == id).FirstOrDefault();

            //Stamp UpdatedDate
            if (!objLog.UserDownloadDate.HasValue)
            {
                objLog.UserDownloadDate = DateTime.Now;
                db.SaveChanges();
            }
            
            //Return file result
            byte[] fileBytes = System.IO.File.ReadAllBytes(objLog.CreatedFileFullDirectory);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, objLog.CreatedFileName);
        }
    }

    public class mockData
    {
        public DateTime CreatedDate { get; set; }
        public string FileName { get; set; }
        public DateTime? DownloadedDate { get; set; }
    }
}