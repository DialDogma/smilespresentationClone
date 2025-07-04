using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSClaimOnLine.EmployeeService;
using SmileSClaimOnLine.DataCenterMobileService;
using SmileSClaimOnLine.Models;
using SmileSClaimOnLine.Helper;

namespace SmileSClaimOnLine.Controllers
{
    public class ManageController : Controller
    {

        #region dbContext

        private ClaimOnLineDBContext _context;

        public ManageController()
                
        {
            _context = new ClaimOnLineDBContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion dbContext

        // GET: Manage
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageClaimOnline()
        {
            return View();
        }



        #region "Method"

        public ActionResult GetClaimOnLineId(string claimCode)
        {
            var result = _context.usp_ClaimOnLineMonitor_Select(null,null,null, null, null, null, 0, 1, null, null, claimCode).SingleOrDefault().ClaimOnLineId;

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetMonitorClaimOnLine(string ClaimOnLineId)
        {
            //int userID;

            //userID = Convert.ToInt32(Session["User_ID"]);



            var result = _context.usp_ClaimOnLine_Select( Convert.ToInt32(ClaimOnLineId), null).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", 1 },
                {"recordsTotal", 1},
                {"recordsFiltered", 1},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClaimOnlineTransfer(string ClaimOnLineId)
        {
            //int userID;

            //userID = Convert.ToInt32(Session["User_ID"]);



            var result = _context.usp_ClaimOnLineTransfer_Select(null, Convert.ToInt32(ClaimOnLineId), null).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", 1 },
                {"recordsTotal", result.Count() != 0 ? result.ToList().Count : result.ToList().Count()},
                {"recordsFiltered", result.Count() != 0 ? result.ToList().Count : result.ToList().Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClaimOnlineTransferItem(string ClaimOnLineCode)
        {
            //int userID;

            //userID = Convert.ToInt32(Session["User_ID"]);



            var result = _context.usp_ClaimOnLineTransferItem_Select(ClaimOnLineCode, null, null).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", 1 },
                {"recordsTotal", result.Count() != 0 ? result.ToList().Count : result.ToList().Count()},
                {"recordsFiltered", result.Count() != 0 ? result.ToList().Count : result.ToList().Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClaimOnlineAccountTransaction(int ClaimOnLineTransferId)
        {
            //int userID;

            //userID = Convert.ToInt32(Session["User_ID"]);



            var result = _context.usp_ClaimOnLineAccountTransaction_Select(null, ClaimOnLineTransferId, null).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", 1 },
                {"recordsTotal", result.Count() != 0 ? result.ToList().Count : result.ToList().Count()},
                {"recordsFiltered", result.Count() != 0 ? result.ToList().Count : result.ToList().Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }


        #endregion

    }
}