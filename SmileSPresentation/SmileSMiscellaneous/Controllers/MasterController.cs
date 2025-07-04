using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSMiscellaneous.Models;
using SmileSMiscellaneous.Helper;

namespace SmileSMiscellaneous.Controllers
{
    public class MasterController : Controller
    {
        private MiscellaneousDBContext _db;

        public MasterController()
        {
            _db = new MiscellaneousDBContext();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }



        // GET: Master
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetProductDetail(int? productId = null, int? productTypeId = null, int? pageStart = null, int? pageSize = null,
                                             string sortField = null, string orderType = null, string search = null)
        {
            var rs = _db.usp_Product_Select(productId, productTypeId, pageStart, pageSize, sortField, orderType, search).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetAgentName(string q)
        {
            var rs = _db.usp_EmployeeSearch_Select(q).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetZebra(string q = null, int? ownerEmployeeId = null, int? zebraId = null)
        {
            var rs = _db.usp_Zebra_Select(ownerEmployeeId, zebraId, 0, 999, null, null, q).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPersonType(string isgroup = null)
        {
            List<usp_PersonType_Select_Result> lst = new List<usp_PersonType_Select_Result>();

            //ถ้าเป็นรายเดี่ยว 
            if (isgroup == "0")
            {
                lst = _db.usp_PersonType_Select(null, 0, 99999, null, null, null).Where(s => s.PersonType_ID == 2).ToList();
            }
            else if (isgroup == "1")
            {
                lst = _db.usp_PersonType_Select(null, 0, 99999, null, null, null).Where(s => s.PersonType_ID == 3).ToList();
            }
            else
            {
                lst = _db.usp_PersonType_Select(null, 0, 99999, null, null, null).Where(s => s.PersonType_ID == 2 || s.PersonType_ID == 3).ToList();
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTitle(int? titleId = null, int? personTypeId = null)
        {
            var rs = _db.usp_Title_Select(titleId, personTypeId, 0, 9999, null, null, null).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CalculageAge(string ageDate, string startCoverDate)
        {
            DateTime? d_age = GlobalFunction.ConvertDatePickerToDate_BE(ageDate);
            DateTime? d_CoverDate = GlobalFunction.ConvertDatePickerToDate_BE(startCoverDate);

            var rs = _db.usp_CalculateAge_Select(d_age, d_CoverDate).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ProvinceDistrictSubDistrict(int? subDistrictId = null, int? districtId = null, int? provinceId = null, string zipCode = null, string q = null)
        {
            var rs = _db.usp_ProvinceDistrictSubDistrict_Select(subDistrictId, districtId, provinceId, zipCode, 0, 999, null, null, q).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRelationType(int? personRelationTypeId = null)
        {
            var rs = _db.usp_PersonRelationType_Select(personRelationTypeId, 0, 9999, null, null, null).ToList();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInsuranceCompany(int? productTypeId = null)
        {
            var rs = _db.usp_OrganizeINMiscellaneous_Select(productTypeId == -1 ? null: productTypeId, 0, 999, null, null, null).ToList();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }
    }
}