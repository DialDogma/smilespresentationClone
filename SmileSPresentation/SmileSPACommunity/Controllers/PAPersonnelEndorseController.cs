using SmileSPACommunity.Helper;
using SmileSPACommunity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmileSPACommunity.Controllers
{
    public class PAPersonnelEndorseController : Controller
    {
        private PACommunityEntities _db;

        public PAPersonnelEndorseController()
        {
            _db = new PACommunityEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        // GET: PAPersonnelEndorse
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PAPersonnelAddCustomerMonitor()     //สลักหลังขอเพิ่มรายชื่อ - หน้า Monitor
        {
            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var role = new SSOService.SSOServiceClient().GetRoleByUserName(username);
            var rolelist = role.Split(',').ToList();

            var roleDev = new[] { "Developer" }; //ฝ่ายพัฒนาระบบ
            var rolePA = new[] { "PAPersonnel_PAUnderwrite" }; //ฝ่าย PA Underwrite

            ViewBag.userID = userID;
            ViewBag.Role = rolelist;
            ViewBag.RoleList = role;

            if (rolelist.Intersect(rolePA).Count() > 0 || rolelist.Intersect(roleDev).Count() > 0)
            {
                ViewBag.Branchs = _db.usp_Branch_Select(null, 0, 99, null, null, null).ToList();
                ViewBag.Title = "รออนุมัติเพิ่มรายชื่อ";
            }
            else
            {
                var branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
                ViewBag.Branchs = _db.usp_Branch_Select(branchId, 0, 99, null, null, null).ToList();
                ViewBag.Title = "เพิ่มรายชื่อผู้เอาประกัน";
            }


            ViewBag.PolicyYear = _db.usp_PolicyYear_Select(0, 999, null, null, null).ToList();
            ViewBag.CurrentYear = DateTime.Now.ToString("yyyy");

            var status = _db.usp_ApproveStatus_Select(null, 0, 9999, null, null, null).ToList();
            ViewBag.StatusPAPersonnelEndorse = status ;

            return View();
        }

        public ActionResult PAPersonnelAddImportCustomer(string appId = null) //เพิ่มรายชื่อ     
        {
            int? d_AppID;
            var appIDBase64EncodedBytes = Convert.FromBase64String(appId);
            var deCode_AppID = System.Text.Encoding.UTF8.GetString(appIDBase64EncodedBytes);
            d_AppID = Convert.ToInt32(deCode_AppID);

            ViewBag.AppID = d_AppID;

            return View();
        }

        public ActionResult AddCustomerManage() //จัดการการเพิ่มรายชื่อ
        {
            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = new SSOService.SSOServiceClient().GetRoleByUserName(username);
            var rolelist = role.Split(',').ToList();

            var roleDev = new[] { "Developer" }; //ฝ่ายพัฒนาระบบ
            var rolePA = new[] { "PAPersonnel_PAUnderwrite" }; //ฝ่าย PA Underwrite

            //ถ้าเป็น สิทธิ์ของ Dev และ Underwrite ให้แสดงสาขาทั้งหมด
            if (rolelist.Intersect(rolePA).Count() > 0 || rolelist.Intersect(roleDev).Count() > 0)
            {
                ViewBag.Branch = _db.usp_Branch_Select(null, 0, 99, null, null, null).ToList();
            }
            else
            {
                var branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
                ViewBag.Branch = _db.usp_Branch_Select(branchId, 0, 99, null, null, null).ToList(); ;
            }

            ViewBag.PolicyYear = _db.usp_PolicyYear_Select(0, 999, null, null, null).ToList();
            ViewBag.Role = rolelist;
            return View();
        }

        public ActionResult AddCustomerApprove(string roundId) //อนุมัติ เพิ่มรายชื่อ  // ดูรายชื่อ    
        {
            int? d_roundId;
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var roundIDBase64EncodedBytes = Convert.FromBase64String(roundId);
            var deCode_RoundId = System.Text.Encoding.UTF8.GetString(roundIDBase64EncodedBytes);
            d_roundId = Convert.ToInt32(deCode_RoundId);

            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = new SSOService.SSOServiceClient().GetRoleByUserName(username);
            var rolelist = role.Split(',').ToList();

            ViewBag.Role = rolelist;
            ViewBag.RoleList = role;
            ViewBag.RoundId = d_roundId;
            ViewBag.ApproveCause = _db.usp_ApproveCause_Select(null, 7, 0, 99, null, null, null).ToList();
            ViewBag.userID = userID;
            return View();
        }

        public ActionResult PAPersonnelEndorseSendRequestCancel(string RequestCancelApplicationId = null)
        {
            if (RequestCancelApplicationId == null)
            {
                ViewBag.RequestCancelApplicationID = null;
            }
            else
            {
                //deCode
                var RequestCancelApplicationIdBase64EncodedBytes = Convert.FromBase64String(RequestCancelApplicationId);
                var deCode_RequestCancelApplicationId = System.Text.Encoding.UTF8.GetString(RequestCancelApplicationIdBase64EncodedBytes);

                ViewBag.RequestCancelApplicationID = deCode_RequestCancelApplicationId;
            }

            var branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
            ViewBag.Branchs = _db.usp_Branch_Select(branchId, 0, 999, null, null, null).ToList();
            ViewBag.BranchID = branchId;
            ViewBag.CancelCause = _db.usp_PersonnelApplicationCancelCause_Select(null, 0, 999, null, null, null).ToList();
            ViewBag.Years = _db.usp_PolicyYear_Select(null, null, null, null, null).ToList();
            return View();

        }

        public ActionResult PAPersonnelEndorseRequestCancelMonitor()
        {
            var branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = new SSOService.SSOServiceClient().GetRoleByUserName(username);
            var rolelist = role.Split(',').ToList();
            ViewBag.RoleList = role; 
            var roleDev = new[] { "Developer" }; //ฝ่ายพัฒนาระบบ
            var rolePA = new[] { "PAPersonnel_PAUnderwrite" }; //ฝ่าย PA Underwrite

            // Underwrite ให้แสดงสาขาทั้งหมด
            if (rolelist.Intersect(rolePA).Count() > 0 )
            {
                ViewBag.Branch = _db.usp_Branch_Select(null, 0, 99, null, null, null).ToList();
                ViewBag.Title = "รออนุมัติขอยกเลิกกรมธรรม์";
                ViewBag.Role = "Underwrite";
            }
            // Dev ให้แสดงสาขาทั้งหมด
            else if (rolelist.Intersect(roleDev).Count() > 0)
            {
                ViewBag.Branch = _db.usp_Branch_Select(null, 0, 99, null, null, null).ToList();
                ViewBag.Title = "รออนุมัติขอยกเลิกกรมธรรม์";
                ViewBag.Role = "Dev";
            }
            else
            {
                ViewBag.Branch = _db.usp_Branch_Select(branchId, 0, 99, null, null, null).ToList();
                ViewBag.Title = "ขอยกเลิกกรมธรรม์";
                ViewBag.Role = "MO";                
            }

            ViewBag.ApproveStatus = _db.usp_ApproveStatus_Select(null, 0, 99, null, null, null).ToList();
            ViewBag.PolicyYear = _db.usp_PolicyYear_Select(0, 999, null, null, null).ToList();
            return View();
        }

        public JsonResult GetMonitorCustomer(int? branchId = null, int? year = null, int? appStatusId = null, int? draw = null, int? pageSize = null, int? indexStart = null, string sortField = null, string orderType = null, string search = null)
        {

            var result = _db.usp_PersonnelApplication_Select(null, branchId, year, appStatusId, indexStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };


            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetApplicationRoundMonitor(int? branchId = null, int? year = null, int? appRoundStatusIdList = null, int? draw = null, int? pageSize = null, int? indexStart = null, string sortField = null, string orderType = null, string searchDetail = null)
        {
            var result = _db.usp_PersonnelApplicationRoundWaitApprove_Select(year, branchId, appRoundStatusIdList, indexStart, pageSize, sortField, orderType, searchDetail).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPAPersonnelAppIDByRoundID(int? appRoundId = null, bool isEndorse = true)
        {
            var result = _db.usp_PersonnelApplicationRound_Select(null, null, appRoundId, null, isEndorse, null, null, null, null, null).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetApplicationHeaderByAppId(int appId)
        {
            var result = _db.usp_GetPersonnelApplicationDetail(appId).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPersonnelCustomerDetail(int appId,int roundId, int? draw = null, int? pageSize = null, int? indexStart = null, string sortField = null, string orderType = null, string searchDetail = null)
        {
            var result = _db.usp_PersonnelCustomerDetail_Select(appId, null, roundId,"1,2,3", indexStart, pageSize, sortField, orderType, searchDetail).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DoUpdateApprovePersonnelApplication(int appRoundId, int roundStatusId, int? approveCauseId = null, string approveRemark = null)
        {
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var result = _db.usp_PersonnelApplicationRoundApprove_Update(appRoundId, roundStatusId, approveCauseId, approveRemark, userID).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRequestCancelApplicationDetail(string approveStatusId = null, int? year = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null, string branchID = null)
        {
            int? StatusId = null;
            int? c_branchID = null;
            if (approveStatusId == "-1" || approveStatusId == null)
            {
                StatusId = null;
            }
            else
            {
                StatusId = Convert.ToInt32(approveStatusId);
            }

            if (branchID != "-1" && branchID != null)
            {
                c_branchID = Convert.ToInt32(branchID);
            }
            var result = _db.usp_RequestCancelPersonnelApplication_Select(c_branchID,null, StatusId, year, pageStart,pageSize,sortField,orderType,search).ToList();
            //var result = _db.usp_RequestCancelPersonnelApplication_Select(null, null, c_branchID, StatusId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };           
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Search and Get Personnel application list
        /// </summary>
        /// <param name="applicationID"></param>
        /// <param name="schoolYear"></param>
        /// <param name="personnelStatusId"></param>
        /// <param name="draw"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageStart"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <param name="search"></param>
        /// <param name="branchID"></param>
        /// <returns></returns>
        public JsonResult GetApplicationDetail(int? applicationID = null, int? schoolYear = null, int? personnelStatusId = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null, int? branchID = null)
        {
            var result = _db.usp_PersonnelApplication_Select(applicationID, branchID, schoolYear, personnelStatusId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRequestCancelApplicationById(int requestCancelApplicationId)
        {
            var rs = _db.usp_RequestCancelPersonnelApplication_Select(requestCancelApplicationId, null, null, null, 0, 999, null, null, null).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Save cancel application 
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public ActionResult SaveRequestCancelApplication(int applicationId)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_RequestCancelPersonnelApplication_Insert(applicationId, userID).Single();

            lstArr[0] = rs.IsResult.Value.ToString();
            lstArr[1] = rs.Result;
            lstArr[2] = rs.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get PAPersonnel documents
        /// </summary>
        /// <param name="referenceId"></param>
        /// <param name="documentTypeId"></param>
        /// <param name="draw"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageStart"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public JsonResult GetDocumentRequestCancelDetail(int referenceId, int documentTypeId, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_DocumentRequestCancelPersonnelApplication_Select(referenceId, documentTypeId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
            
        }

        /// <summary>
        /// Update cancel app
        /// </summary>
        /// <param name="requestCancelApplicationId"></param>
        /// <param name="applicationCancelCauseId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public ActionResult UpdateRequestCancelApplication(int requestCancelApplicationId, int applicationCancelCauseId, string remark)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_RequestCancelPersonnelApplication_Sent_Update(requestCancelApplicationId, applicationCancelCauseId, remark, userID).Single();

            lstArr[0] = rs.IsResult.Value.ToString();
            lstArr[1] = rs.Result;
            lstArr[2] = rs.Msg;

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

    }
}