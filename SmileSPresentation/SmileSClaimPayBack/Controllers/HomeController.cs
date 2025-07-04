using System.Web.Mvc;
using System;
using SmileSClaimPayBack.Models;
using SmileSClaimPayBack.Helper;
using System.Linq;
using System.Collections.Generic;
using Rotativa;

namespace SmileSClaimPayBack.Controllers
{
    [Authorization]
    public class HomeController : Controller
    {
        private readonly ClaimPayBackEntities _context;

        public HomeController()

        {
            _context = new ClaimPayBackEntities();
        }
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AnotherLink()
        {
            return View("Index");
        }

        public ActionResult ConvertToPDF()
        {

            Dictionary<string, string> cookieCollection = new Dictionary<string, string>();
            foreach (var key in Request.Cookies.AllKeys)
            {
                cookieCollection.Add(key, Request.Cookies.Get(key).Value);
            }
            var printpdf = new ViewAsPdf("Index")
            {
                FileName = "Name.pdf",
                Cookies = cookieCollection
            };

            return printpdf;
        }





        public JsonResult GetClaimPaymentType()
        {
            ViewBag.ClaimGroupType = _context.usp_ClaimGroupType_Select(null, 0, 999, null, null, null).ToList();

            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetClaimPaymentReceiptGroup(string dateStart, string dateEnd, int? claimGroupTypeId, int indexStart, int draw, int pageSize, string sortField, string orderType, string searchDetail)
        {
            DateTime d_dateStart;
            DateTime d_dateEnd;

            d_dateStart = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateStart));
            d_dateEnd = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateEnd));


            var list = _context.usp_ClaimPayBackForGroupTransfer_Select(d_dateStart, d_dateEnd, claimGroupTypeId, indexStart, pageSize, sortField, orderType, searchDetail).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"data", list.ToList()}
            };


            return Json(dt, JsonRequestBehavior.AllowGet);
        }
    }
}
