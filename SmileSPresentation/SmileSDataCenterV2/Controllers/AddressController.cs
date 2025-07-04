using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSDataCenterV2.Helper;
using SmileSDataCenterV2.Models;

namespace SmileSDataCenterV2.Controllers
{
    [Authorization]
    public class AddressController : Controller
    {
        private DataCenterV1Entities _db;

        public AddressController()
        {
            _db = new DataCenterV1Entities();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        // GET: Address
        public ActionResult Index()
        {
            return View();
        }

        [Authorization(Roles = "Developer")]
        public ActionResult AreaManagement()
        {
            return View();
        }

        [Authorization(Roles = "Developer")]
        public ActionResult BranchManagement()
        {
            return View();
        }

        [Authorization(Roles = "Developer")]
        public ActionResult BranchManagementByArea()
        {
            ViewBag.Area = _db.usp_Area_Select(null, null, 9999, null, null, null).ToList();
            return View();
        }

        [Authorization(Roles = "Developer,DataCenter_Supervisor")]
        public ActionResult PersonalInChargeOfAreaManagement()
        {
            ViewBag.Employee = _db.usp_BusinesPromoteTeam_Select(null, null, 9999999, null, null, null).Where(x => x.IsActive == true).ToList();
            return View();
        }

        [Authorization(Roles = "Developer")]
        public ActionResult DistrictByBranchManagement()
        {
            ViewBag.Branch = _db.usp_Branch_SelectV2(null, null, 0, 9999, null, null, null).ToList();
            return View();
        }

        //Add View 2021-03-30
        [Authorization(Roles = "Developer,DataCenter_Supervisor")]
        public ActionResult UpdateBranchInformation()
        {
            return View();
        }

        public JsonResult GetRequestAllArea(int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_Area_Select(null, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetUpdateArea(int areaId, string areaEdit, bool isActive)
        {
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _db.usp_Area_Update(areaId, areaEdit.Trim(), isActive, userID).SingleOrDefault();

            var lstArr = new string[3];
            lstArr[0] = result.IsResult.Value.ToString();
            lstArr[1] = result.Result;
            lstArr[2] = result.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetInsertArea(string areaInsert)
        {
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _db.usp_Area_Insert(areaInsert.Trim(), userID).SingleOrDefault();

            var lstArr = new string[3];
            lstArr[0] = result.IsResult.Value.ToString();
            lstArr[1] = result.Result;
            lstArr[2] = result.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRequestAllBranch(int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_Branch_SelectV2(null, null, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetUpdateBranch(int branchId, string branchEdit, bool isActive)
        {
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _db.usp_Branch_Update(branchId, branchEdit.Trim(), isActive, userID).SingleOrDefault();

            var lstArr = new string[3];
            lstArr[0] = result.IsResult.Value.ToString();
            lstArr[1] = result.Result;
            lstArr[2] = result.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetInsertBranch(string branchInsert)
        {
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _db.usp_Branch_Insert(branchInsert.Trim(), userID).SingleOrDefault();

            var lstArr = new string[3];
            lstArr[0] = result.IsResult.Value.ToString();
            lstArr[1] = result.Result;
            lstArr[2] = result.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetUpdateBranchXArea(int branchId, int? areaid, string branchEdit, bool isActive)
        {
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _db.usp_BranchXArea_Update(branchId, areaid, branchEdit.Trim(), userID).SingleOrDefault();

            var lstArr = new string[3];
            lstArr[0] = result.IsResult.Value.ToString();
            lstArr[1] = result.Result;
            lstArr[2] = result.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRequestAllAreaXEmp(int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_AreaManager_Select(null, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetUpdateAreaXEmp(int areaid, int userid)
        {
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _db.usp_AreaManager_Update(areaid, userid, userID).SingleOrDefault();

            var lstArr = new string[3];
            lstArr[0] = result.IsResult.Value.ToString();
            lstArr[1] = result.Result;
            lstArr[2] = result.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProvinceDistrictSubDistrict(int? subDistrictId = null, int? districtId = null, int? provinceId = null, string zipCode = null, string q = null)
        {
            var rs = _db.usp_ProvinceDistrictSubDistrict_Select(subDistrictId, districtId, provinceId, zipCode, 0, 999, null, null, q).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBranchInformation(int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_BranchInformation_Select(pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBranchInformationById(int branchInformationId)
        {
            var rs = _db.usp_BranchInformationById_Select(branchInformationId).SingleOrDefault();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPersonByDetail(string keyword)
        {
            var rs = _db.usp_PersonUserV2_Select(keyword).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveBranchInformation(int BranchInformation_ID, int Branch_ID, string AddressDetail1, string AddressDetail2,
                                                int SubDistrict_ID, string NearBy, string Telephone, string SecondPhoneNo, string Email,
                                                string Latitude, string Longitude, int? ManagerUser_ID = null, int? ChairmanUser_ID=null, int? DirectorUser_ID = null,
                                                int? BusinessPromoteTeamUser_ID = null)
        {
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_BranchInformation_Insert(BranchInformation_ID, Branch_ID, AddressDetail1, AddressDetail2, SubDistrict_ID, NearBy, Telephone, Latitude, Longitude,
                                                      ManagerUser_ID, ChairmanUser_ID, DirectorUser_ID, BusinessPromoteTeamUser_ID, userID, Email, SecondPhoneNo).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        #region "GetData"

        public JsonResult GetDistrictByBranchDetail(int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_BranchArea_Select(null, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        #endregion "GetData"

        #region "InsertData"

        public ActionResult UpdateDistrictByBranch(int? branchAreaId = null, int? branchId = null)
        {
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _db.usp_BranchArea_Update(branchAreaId, branchId, userID).Single();

            var lstArr = new string[3];
            lstArr[0] = result.IsResult.Value.ToString();
            lstArr[1] = result.Result;
            lstArr[2] = result.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        #endregion "InsertData"
    }
}