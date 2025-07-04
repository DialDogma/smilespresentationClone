using OfficeOpenXml;
using OfficeOpenXml.Style;
using SmileSPACommunity.Helper;
using SmileSPACommunity.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SmileSPACommunity.Controllers
{
    public class PAPersonnelApproveController : Controller
    {
        private PACommunityEntities _db;

        public PAPersonnelApproveController()
        {
            _db = new PACommunityEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        // GET: PAPersonnelApprove
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult PAPersonnelApproveMonitor() //หน้า รออนุมัติ Monitor
        {
            ViewBag.AppRoundStatus = _db.usp_PersonnelApplicationRoundStatus_Select(null, 0, 99, null, null, null).Where(x => x.PersonnelApplicationRoundStatusId != 1).ToList();
            ViewBag.Branch = _db.usp_Branch_Select(null, 0, 99, null, null, null).ToList();
            ViewBag.userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.PolicyYear = _db.usp_PolicyYear_Select(0, 999, null, null, null).ToList();
            return View();
        }

        public ActionResult PAPersonnelApprove(string appRoundId = null) //หน้า รออนุมัติ
        {
            int? d_roundid;

            //deCode
            var roundIDBase64EncodedBytes = Convert.FromBase64String(appRoundId);
            var deCode_roundID = System.Text.Encoding.UTF8.GetString(roundIDBase64EncodedBytes);

            d_roundid = Convert.ToInt32(deCode_roundID);

            ViewBag.RoundId = d_roundid;
            //ViewBag.ApproveCause = _db.usp_ApproveCause_Select(null, 5, 0, 99, null, null, null).ToList();
            ViewBag.ApproveCause = _db.usp_ApproveCause_Select(null, 6, 0, 9999, null, null, null).ToList();
            ViewBag.userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            return View();
        }

        public ActionResult PAPersonnelEndorseSendRequestCancelApprove(string RequestCancelApplicationId) //หน้า อนุมัติ ขอยกเลิกกรมธรรม์
        {
            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = new SSOService.SSOServiceClient().GetRoleByUserName(username);
            var rolelist = role.Split(',').ToList();
            ViewBag.RoleList = role;
            var roleDev = new[] { "Developer" }; //ฝ่ายพัฒนาระบบ
            var rolePA = new[] { "PAPersonnel_PAUnderwrite" }; //ฝ่าย PA Underwrite

            if (rolelist.Intersect(rolePA).Count() > 0 || rolelist.Intersect(roleDev).Count() > 0)
            {
                ViewBag.Role = "Allow";
            }

            if (string.IsNullOrEmpty(RequestCancelApplicationId))
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

            ViewBag.userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.ApproveCause = _db.usp_ApproveCause_Select(null, 8, null, null, null, null,null).ToList();

            return View();
        }


        public JsonResult GetApplicationRoundDetailByRoundId(
            int? appRoundId = null,
            bool isEndorse = false
            )
        {
            var result = _db.usp_PersonnelApplicationRound_Select(null, null, appRoundId, null, isEndorse, null, null, null, null, null).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPersonnelCustomerDetail
            (
            int? appId = null,
            int? appRoundId = null,
            int? customerDetailId = null,
            int? draw = null,
            int? pageSize = null,
            int? indexStart = null,
            string sortField = null,
            string orderType = null,
            string searchDetail = null
            )
        {
            var result = _db.usp_PersonnelCustomerDetail_Select(appId, customerDetailId, appRoundId,"1,2", indexStart, pageSize, sortField, orderType, searchDetail).ToList();
            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetApplicationRoundMonitor(int? appRoundId = null, int? branchId = null, int? year = null, string appRoundStatusIdList = null, int? draw = null, int? pageSize = null, int? indexStart = null, string sortField = null, string orderType = null, string searchDetail = null)
        {
            var result = _db.usp_PersonnelApplicationRound_Select(year, branchId, appRoundId, appRoundStatusIdList, false, indexStart, pageSize, sortField, orderType, searchDetail).ToList();


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
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var result = _db.usp_PersonnelApplicationRoundApprove_Update(appRoundId, roundStatusId, approveCauseId, approveRemark, userID).Single();

            lstArr[0] = result.IsResult.Value.ToString();
            lstArr[1] = result.Result.ToString();
            lstArr[2] = result.Msg.ToString();

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult>ExportApplicationRoundReport(int? branchId = null, int? year = null, string appRoundStatusIdList = null, string searchDetail = null)
        {
            await Task.Yield();
            var result = _db.usp_ReportPersonnelApplicationWaitApprove_Select(year, branchId, appRoundStatusIdList, 0, 99999, "PersonnelApplicationCode", "asc", searchDetail)
              .Select((x) => new
              {
                  สาขา = x.BranchDetail,
                  จังหวัด = x.ProvinceDetail,
                  เขตพื้นที่การศึกษา = x.StudyArea,
                  รหัสโรงเรียน = x.School_id,
                  App_ID = x.PersonnelApplicationCode,
                  ชื่อโรงเรียน = x.PersonnelApplicationName,
                  วันคุ้มครอง = x.EffectiveDate != null ? x.EffectiveDate.Value.Date.ToString("dd/MM/yyyy") : "",
                  สถานะ_App_PA_นักเรียน = x.StudentStatus,
                  แผน_PAนักเรียน = x.StudentProduct,  
                  จำนวนนักเรียน = x.Student,
                  จำนวนครูฟรี = x.Free,
                  จำนวนครูซื้อ = x.Buy,
                  จำนวนรวม = x.Total,
                  แผน_PA_ยิ้มแฉ่ง = x.ProductDetail,
                  เสียชีวิตอุบัติเหตุ = x.CoverageAccident,
                  ฆาตกรรม_MC = x.CoverageMC,
                  จำนวนผู้เอาประกัน_PAยิ้มแฉ่ง = x.CountCustomer,
                  หมายเหตุไม่เข้าเงื่อนไข = x.ValidateRemark,
                  วันที่ทำรายการครั้งแรก =  x.FirstExportDate.Value != null ? x.FirstExportDate.Value.ToString("dd/MM/yyyy HH:mm") : ""
              }).ToList();

            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var workSheet1 = package.Workbook.Worksheets.Add("Detail");
                workSheet1.Cells.LoadFromCollection(result, true);

                var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                headerCells1.Style.Font.Bold = true;
                headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerCells1.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                allCells1.AutoFilter = true;
                allCells1.AutoFitColumns();


                package.Save();

                stream.Position = 0;
                Session["DownloadExportApplicationRound"] = package.GetAsByteArray();

                return Json("", JsonRequestBehavior.AllowGet);
            }

            //var dt1 = GlobalFunction.ToDataTable(result);
            //ExcelManager.ExportToExcel(this.HttpContext, "รายงานรออนุมัติกรมธรรม์_PA_ยิ้มแฉ่ง", dt1);

        }

        public ActionResult ExportApplicationRoundReportDownload()
        {
            //var dataKey = TempData["keyReport"].ToString();
            if (Session["DownloadExportApplicationRound"] != null)
            {
                //var period = TempData[dataKey] as string;

                byte[] data = Session["DownloadExportApplicationRound"] as byte[];
                string excelName = $"รายงานรออนุมัติกรมธรรม์_PA_ยิ้มแฉ่ง_{DateTime.Now}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        public ActionResult GetApplicationDetailById(int PersonnelApplicationId)
        {
            var rs = _db.usp_GetPersonnelApplicationDetail(PersonnelApplicationId).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRequestCancelPersonnelApplication(int requestCancelApplicationId)
        {
            var rs = _db.usp_RequestCancelPersonnelApplication_Select(null,
                                                                    requestCancelApplicationId,
                                                                    null,
                                                                    null,
                                                                    null,
                                                                    null,
                                                                    null,
                                                                    null,
                                                                    null)
                                                                    .Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DoApproveRequestCancelApplication(int requestCancelApplicationId, int approveStatusId, int? approveCauseId = null, string approveRemark = null)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var result = _db.usp_RequestCancelPersonnelApplication_Approve_Update(requestCancelApplicationId, approveStatusId, approveCauseId, approveRemark, userID).Single();

            lstArr[0] = result.IsResult.Value.ToString();
            lstArr[1] = result.Result.ToString();
            lstArr[2] = result.Msg.ToString();

            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }
    }
}