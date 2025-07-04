using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileClaimV2.Models;
using SmileClaimV2.Helper;

namespace SmileClaimV2.Controllers
{
    [Authorization]
    public class DataMasterController : Controller
    {
        #region Declare

        //**start set context

        private readonly DataCenterV1Entities _dataCenterV1centerV1;

        public DataMasterController()
        {
            _dataCenterV1centerV1 = new DataCenterV1Entities();
        }

        protected override void Dispose(bool disposing)
        {
            _dataCenterV1centerV1.Dispose();
        }

        //**end set context

        #endregion Declare

        // GET: DataMaster
        public ActionResult Index()
        {
            return View();
        }

        #region GetDataMaster

        /// <summary>
        /// ประเภทเคลม
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="pGroupId"></param>
        /// <returns></returns>
        public ActionResult GetClaimType(bool isActive, int? pGroupId = null)
        {
            var list = _dataCenterV1centerV1.usp_ClaimType_Select(pGroupId, isActive).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ClaimAdmitType
        /// </summary>
        /// <param name="pGroupId"></param>
        /// <param name="claimTypeId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public ActionResult GetClaimAdmitType(int? pGroupId = null, int? claimTypeId = null, bool? isActive = true)
        {
            var list = _dataCenterV1centerV1.usp_ClaimAdmitType_Select(pGroupId, claimTypeId, isActive).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// อาการ
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="chiefComplainId"></param>
        /// <returns></returns>
        public ActionResult GetChiefComplain(bool isActive, int? chiefComplainId = null)
        {
            var list = _dataCenterV1centerV1.usp_ChiefComplain_Select(chiefComplainId, isActive).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// จังหวัด
        /// </summary>
        /// <returns></returns>
        public ActionResult GetProvince()
        {
            var list = _dataCenterV1centerV1.usp_Province2_Select(null).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion GetDataMaster
    }
}