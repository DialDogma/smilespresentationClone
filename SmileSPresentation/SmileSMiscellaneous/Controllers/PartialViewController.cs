using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSMiscellaneous.Models;
using SmileSMiscellaneous.Helper;

namespace SmileSMiscellaneous.Controllers
{
    public class PartialViewController : Controller
    {
        private MiscellaneousDBContext _db;

        public PartialViewController()
        {
            _db = new MiscellaneousDBContext();
        }

        // GET: PartialView
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAssureList(int? appId = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_ApplicationHeir_Select(appId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTransactionHeaderList(int? tranType = null, int? appId = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_ApplicationTransactionHeader_Select(tranType, appId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPremiumReceiveList(int? appId = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_PremiumReceive_Select(appId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDocument(int? appId = null, string documentTypeIdList = null, string remark = null, int? draw = null, int? pageStart = null, int? pageSize = null,
                                           string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_Document_Select(appId, documentTypeIdList, pageStart, pageSize, sortField, orderType, search).ToList().OrderBy(x => x.DocumentTypeId );

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }
    }

    public class DataAppDetail
    {
        public string ApplicationId { get; set; }
        public string ApplicationCode { get; set; }
        public string ProductId { get; set; }
    }
}