using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSPaymentGateway.Helper;
using SmileSPaymentGateway.Models;

namespace SmileSPaymentGateway.Controllers
{
     [Authorization]
    public class SearchDebtController : Controller
    {
        #region Declare

        private readonly DataCenterV1Entities _dataCenterV1Entities;
        private readonly SSSCashReceiveEntities _sSSCashReceiveEntities;

        public SearchDebtController()
        {
            _dataCenterV1Entities = new DataCenterV1Entities();
            _sSSCashReceiveEntities = new SSSCashReceiveEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _dataCenterV1Entities.Dispose();
            _sSSCashReceiveEntities.Dispose();
        }


        #endregion Declare

        #region Action

        // GET: SearchDebt
        public ActionResult Index()
        {
            ViewBag.listProvince = _dataCenterV1Entities.usp_Province2_Select(null);

            return View();
        }

        public ActionResult Payment(string debtRefIdList, string payerZCardID)
        {
            ViewBag.payerZCard = payerZCardID;

            ViewBag.debtRefId = debtRefIdList;

            //var result = _sSSCashReceiveEntities.usp_GetDebtDetailByDebtRefIdList_Select(debtRefIdList)

            return View();
        }

        #endregion Action

        #region GetData

        public JsonResult GetDistrict(int provinceId)
        {
            var result = _dataCenterV1Entities.usp_District_ByProvinceID_Select(provinceId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubDistrict(int districtId)
        {
            var result = _dataCenterV1Entities.usp_SubDistrict_ByDistrictID_Select(districtId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataTableDebt(int draw, int pageSize, int pageStart, string sortField, string orderType, string search, string provinceId, string districtId, string subDistrictId)
        {
            if (provinceId == "-1")
            {
                provinceId = null;
            }

            if (districtId == "-1")
            {
                districtId = null;
            }

            if (subDistrictId == "-1")
            {
                subDistrictId = null;
            }

            //var list = _sSSCashReceiveEntities.usp_GetPremiumSearchV2_Select("0101", provinceId, districtId, subDistrictId, "3", search, "00004", pageSize, pageStart, sortField, orderType).ToList();

            var list = _sSSCashReceiveEntities.usp_GetPremiumSearchV2_Select("0102", provinceId, districtId, subDistrictId, "3", search, "00004", pageSize, pageStart, sortField, orderType).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault()?.totalcount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault()?.totalcount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataTablePaymentDebt(string debtRefIdList, int draw, int pageSize, int pageStart, string sortField, string orderType, string search)
        {
            var list = _sSSCashReceiveEntities.usp_GetDebtDetailByDebtRefIdList_Select(debtRefIdList, pageStart, 100, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }
        #endregion GetData
    }
}