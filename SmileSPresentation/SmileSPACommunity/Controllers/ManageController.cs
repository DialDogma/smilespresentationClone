using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSPACommunity.Models;
using SmileSPACommunity.Helper;

namespace SmileSPACommunity.Controllers
{
    public class ManageController : Controller
    {
        //private DataCenterV1Entities _db;
        private PACommunityEntities _db;

        private DataCenterV1Entities _ctdb;

        public ManageController()

        {
            _db = new PACommunityEntities();
            _ctdb = new DataCenterV1Entities();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            _ctdb.Dispose();
        }

        // GET: Manage
        public ActionResult Index()
        {
            return View();
        }

        //Queue management by branch
        public ActionResult ManageQueueByBranch()
        {
            ViewBag.Employee = _db.usp_Employee_Select(null, null, 0, 9999999, null, null, null).ToList();
            return View();
        }

        public ActionResult ManagePACommunitySystem()
        {
            ViewBag.OrganizeGroups = _db.usp_OrganizeGroup_Select(null, 0, 9999, null, null, null).ToList();
            ViewBag.Provinces = _db.usp_Province_Select(null, 0, 999, null, null, null).ToList();
            return View();
        }

        public JsonResult GetMonitorOrganizeDetail(int? organizeId = null, int? organizeGroupId = null, int? organizeTypeId = null, string provinceId = null,
                                                    int? draw = null, int? pageSize = null, int? pageStart = null,
                                                    string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_Organize_Select(organizeId, organizeGroupId == -1 ? null : organizeGroupId, organizeTypeId == -1 ? null : organizeTypeId, provinceId == "-1" ? null : provinceId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAddressByDetail(string keyword = null, int? subDistrictId = null)
        {
            var rs = _db.usp_ProvinceDistrictSubDistrict_Select(subDistrictId, null, null, null, 0, 99999, null, null, keyword).ToList();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrganizeTitle(int? organizeTitleId = null, int? organizeTitleTypeId = null)
        {
            var rs = _db.usp_OrganizeTitle_Select(organizeTitleId, organizeTitleTypeId, 0, 9999, null, null, null).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DoSaveOrUpdateOrganizeDetail(int? organizeId = null, int? organizeTypeId = null, int? organizeTitleId = null,
                                                       string organizeDetail = null, string buildingName = "-", string villageName = "-",
                                                       string no = "-", string moo = "-", string floor = "-", string room = "-", string soi = "-",
                                                       string road = "-", string subDistrictId = null, string zipCode = null)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            if (organizeId == null) //Insert
            {
                var result = _ctdb.usp_Organize_Insert(organizeTypeId, organizeTitleId, organizeDetail,
                                                        buildingName, villageName, no, moo, floor, room, soi,
                                                        road, subDistrictId, zipCode, userID).Single();
                lstArr[0] = result.IsResult.Value.ToString();
                lstArr[1] = result.Result.ToString();
                lstArr[2] = result.Msg.ToString();
            }
            else
            {
                var result = _ctdb.usp_Organize_Update(organizeId, organizeTypeId, organizeTitleId, organizeDetail,
                                                        buildingName, villageName, no, moo, floor, room, soi,
                                                        road, subDistrictId, zipCode, userID).Single();

                lstArr[0] = result.IsResult.Value.ToString();
                lstArr[1] = result.Result.ToString();
                lstArr[2] = result.Msg.ToString();
            }

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetMonitorManageQueueByBranch(int? draw = null, int? pageSize = null, int? pageStart = null,
        //                                            string sortField = null, string orderType = null, string search = null)
        //{
        //    var result = _db.* *************(pageStart, pageSize, sortField, orderType, search).ToList();

        //    var dt = new Dictionary<string, object>
        //    {
        //        {"draw", draw },
        //        {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
        //        {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
        //        {"data", result.ToList()}
        //    };

        //    return Json(dt, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult DoUpdateManageQueueByBranch(int? branchId = null, int? empId = null)
        //{
        //    var lstArr = new string[3];
        //    var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
        //    var result = _ctdb.usp_Organize_Update(organizeId, organizeTypeId).Single();

        //    lstArr[0] = result.IsResult.Value.ToString();
        //    lstArr[1] = result.Result.ToString();
        //    lstArr[2] = result.Msg.ToString();

        //    return Json(lstArr, JsonRequestBehavior.AllowGet);
        //}
    }
}