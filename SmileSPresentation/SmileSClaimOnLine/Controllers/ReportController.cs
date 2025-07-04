using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSClaimOnLine.EmployeeService;
using SmileSClaimOnLine.DataCenterMobileService;
using SmileSClaimOnLine.Models;
using SmileSClaimOnLine.Helper;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Serilog;

namespace SmileSClaimOnLine.Controllers
{
    [Authorization]
    public class ReportController : Controller
    {
        #region dbContext

        private ClaimOnLineDBContext _context;
        private const string _controllerName = nameof(ReportController);
        private const string _dataNotFound = "ไม่พบข้อมูล";
        private const string _exportToExcelSuccessFul = "Export To Excel สำเร็จ";
        public ReportController()

        {
            _context = new ClaimOnLineDBContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion dbContext

        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ClaimOnLineReport()
        {
            //using (var _client = new DataCenterMobileService.MobileServiceClient())
            //{
            //    ViewBag.Branch = _client.GetBranch();
            //}

            ViewBag.Zone = _context.usp_Zone_Select(null, true).ToList();

            return View();
        }

        public ActionResult ClaimOnLineAllReport()
        {
            return View();
        }

        public ActionResult ClaimOnLineAccountReport()
        {
            var List = _context.usp_ClaimOnLineAccount_Select(null, null, true).ToList();
            ViewBag.AccountNo = List;

            return View();
        }

        public ActionResult ClaimOnLineForCompensateReport()
        {
            ViewBag.Branch = _context.usp_Branch_Select(null, null, true).ToList();
            return View();
        }

        public ActionResult ClaimOnLineFollowClaimBranchReport()
        {
            ViewBag.Branch = _context.usp_Branch_Select(null, null, true).ToList();
            return View();
        }

        public ActionResult CreatedClaimReport()
        {
            int employeeID;

            employeeID = GlobalFunction.GetLoginDetail(this.HttpContext).Employee_ID;

            int c;

            c = _context.usp_ZoneXEmployee_Select(employeeID, true).Count();

            int? zoneId;

            if (c > 0)
            {
                zoneId = Convert.ToInt32(_context.usp_ZoneXEmployee_Select(employeeID, true).SingleOrDefault().ZoneId);
            }
            else
            {
                zoneId = null;
            }

            ViewBag.Zone = _context.usp_Zone_Select(null, true).ToList();

            //ViewBag.Branch = _context.usp_Branch_Select(null, zoneId, true).ToList();
            var result = _context.usp_ProductGroup_Select(0, 999, null, null, null).ToList();

            var ww = result.Where(x => x.ProductGroup_ID == 2 || x.ProductGroup_ID == 3).ToList();

            ViewBag.ProductType = ww;
            return View();
        }

        public ActionResult ClaimOnLineRemarkReport()
        {
            ViewBag.Zone = _context.usp_Zone_Select(null, true).ToList();
            return View();
        }

        public ActionResult ClaimOnLineCompensationReport()
        {
            //ViewBag.Zone = _context.usp_Zone_Select(null, true).ToList();
            return View();
        }

        public ActionResult ClaimOnLineCutOffPaymentReport()
        {
            var checkMonth = _context.usp_ProgramConfig_Select("ClaimOnLine_ConfigMonth_Report", null, null, null, null, null).FirstOrDefault();
            ViewBag.checkMonth = checkMonth.ValueNumber;
            return View();
        }

        public ActionResult ClaimOnLineSurveyReport()
        {
            ViewBag.Zone = _context.usp_Area_Select(null, 0, 999999, null, null, null).ToList();
            return View();
        }

        public ActionResult ClaimOnLineHeaderGroupReport()
        {
            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = OAuth2Helper.GetRoles();
            var rolelist = role.Split(',').ToList();

            var roleMO = new[] { "SmileSClaimOnLine_Operation" }; //Operator

            var branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;

            if (rolelist.Intersect(roleMO).Count() > 0)
            {
                ViewBag.Branch = _context.usp_Branch_Select1(branchId, 0, 99999, null, null, null).ToList();
                ViewBag.checkBranch = false;
            }
            else
            {
                ViewBag.Branch = _context.usp_Branch_Select1(null, 0, 99999, null, null, null).ToList();
                ViewBag.checkBranch = true;
            }

            var checkMonth = _context.usp_ProgramConfig_Select("ClaimOnLine_ConfigMonth_Report", null, null, null, null, null).FirstOrDefault();
            ViewBag.checkMonth = checkMonth.ValueNumber;
            return View();
        }

        public ActionResult ClaimOnLineReserveForBranchReport()
        {
            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = OAuth2Helper.GetRoles();
            var rolelist = role.Split(',').ToList();

            var roleMO = new[] { "SmileSClaimOnLine_Operation" }; //Operator

            var branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;

            if (rolelist.Intersect(roleMO).Count() > 0)
            {
                ViewBag.Branch = _context.usp_Branch_Select1(branchId, 0, 99999, null, null, null).ToList();
                ViewBag.checkBranch = false;
            }
            else
            {
                ViewBag.Branch = _context.usp_Branch_Select1(null, 0, 99999, null, null, null).ToList();
                ViewBag.checkBranch = true;
            }

            var checkMonth = _context.usp_ProgramConfig_Select("ClaimOnLine_ConfigMonth_Report", null, null, null, null, null).FirstOrDefault();
            ViewBag.checkMonth = checkMonth.ValueNumber;
            return View();
        }

        public ActionResult ClaimOnlineTranferReconcileReport()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> ExportClaimOnLineAllReport(string DateFrom, string DateTo, string productGroup)
        {
            await Task.Yield();
            using (var db = new ClaimOnLineDBContext())
            {
                DateTime? nDateFrom = null;
                DateTime? nDateTo = null;
                int? productGroupId = null;
                try
                {
                    if (DateFrom != "") nDateFrom = Convert.ToDateTime(DateFrom);
                    if (DateTo != "") nDateTo = Convert.ToDateTime(DateTo);
                    if (productGroup != "") productGroupId = Convert.ToInt32(productGroup);
                }
                catch (Exception)
                {
                    throw;
                }

                var lst = db.usp_ClaimOnLineReportAll_Ensemble_Select(dateFrom: nDateFrom, dateTo: nDateTo, productGroupId: productGroupId).ToList();

                var stream = new MemoryStream();

                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Detail");
                    workSheet.Cells.LoadFromCollection(lst, true);

                    // Select only the header cells
                    var headerCells = workSheet.Cells[1, 1, 1, workSheet.Dimension.Columns];
                    // Set their text to bold.
                    headerCells.Style.Font.Bold = true;
                    // Set their background color
                    headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                    // Apply the auto-filter to all the columns
                    var allCells = workSheet.Cells[workSheet.Dimension.Address];
                    allCells.AutoFilter = true;
                    // Auto-fit all the columns
                    allCells.AutoFitColumns();

                    package.Save();

                    stream.Position = 0;
                    Session["DownloadExcel_FileClaimOnlineAll"] = package.GetAsByteArray();
                    Session["ProductGroup"] = (productGroupId == 2 ? "PH" : (productGroupId == 3 ? "PA" : "TypeNotAvailable"));
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult DownloadExportClaimOnLineAllReport()
        {
            if (Session["DownloadExcel_FileClaimOnlineAll"] != null)
            {
                string productGroup = Session["ProductGroup"] as string;
                byte[] data = Session["DownloadExcel_FileClaimOnlineAll"] as byte[];
                string excelName = $"Report-ClaimOnLineAll-{productGroup}-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        public void ClaimOnLineAddRemarkReportExport(string dateFrom, string dateTo, string branchId, string zoneId)
        {
            int? zone = null;
            int? branch = null;

            var conDateFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
            var conDateTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));

            if (zoneId != "-1")
            {
                zone = Convert.ToInt32(zoneId);
            }

            if (branchId != "-1")
            {
                branch = Convert.ToInt32(branchId);
            }

            var rs = _context.usp_ClaimOnLineRemarkReport_Select(conDateFrom, conDateTo, branch, zone, 0, 99999999, null, null, null).Select(s => new
            {
                เลขที่อ้างอิง = s.ClaimOnLineCode,
                ประเภทแผน = s.ProductTypeDetail
                                                                                                                                                ,
                วันที่สร้าง_COL = s.CreateDate,
                รายละเอียด = s.Detail,
                โซน = s.Zone,
                สาขา = s.BranchDetail
                                                                                                                                                ,
                รหัสผู้ให้บริการ = s.RemarkByUser,
                ผู้ให้บริการ = s.RemarkByName,
                หมายเหตุเพิ่มเติม = s.Remark
                                                                                                                                                ,
                วันที่แก้ไขผู้ให้บริการ = s.CreateRemarkDate
            }).ToList();

            var dt = GlobalFunction.ToDataTable(rs);

            ExcelManager.ExportToExcel(this.HttpContext, "ClaimOnLineRemarkReport", dt, "Detail");
        }

        public ActionResult GetCreatedClaimDetail(string dateFrom, string dateTo, int productGroupId, string branchId, string zoneId,
                                                  int? draw = null, int? pageSize = null, int? pageStart = null,
                                                  string sortField = null, string orderType = null, string search = null)
        {
            int? zone = null;
            int? branch = null;

            var conDateFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
            var conDateTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));

            if (zoneId != "-1")
            {
                zone = Convert.ToInt32(zoneId);
            }

            if (branchId != "-1")
            {
                branch = Convert.ToInt32(branchId);
            }

            var rs = _context.usp_ClaimHeaderMonitorAll_Select(conDateFrom, conDateTo, productGroupId, branch, zone, pageStart, pageSize, sortField, orderType, search).ToList().OrderBy(s => s.CreatedDate);

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", rs.Count() != 0 ? rs.FirstOrDefault()?.TotalCount : rs.Count()},
                {"recordsFiltered", rs.Count() != 0 ? rs.FirstOrDefault()?.TotalCount : rs.Count()},
                {"data", rs.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public void CreatedClaimReportExport(string dateFrom, string dateTo, int productGroupId, string branchId, string zoneId)
        {
            int? zone = null;
            int? branch = null;

            var conDateFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
            var conDateTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));

            if (zoneId != "-1")
            {
                zone = Convert.ToInt32(zoneId);
            }

            if (branchId != "-1")
            {
                branch = Convert.ToInt32(branchId);
            }

            var rs = _context.usp_ClaimHeaderMonitorAllReport_Select(conDateFrom, conDateTo, productGroupId, branch, zone, 0, 999999999, null, null, null).ToList();

            var dt = GlobalFunction.ToDataTable(rs);

            ExcelManager.ExportToExcel(this.HttpContext, "CreateClaimHeaderReport", dt, "Detail");

            // return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ClaimOnLineAccountReport(FormCollection form)
        {
            var datefrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(form["dtpdatefrom"]));
            var dateto = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(form["dtpdateto"]));

            int? Acc = Convert.ToInt32(form["ddlAccount"]);

            if (Acc == -1)
            {
                Acc = null;
            }

            var result = _context.usp_ClaimOnLineReportAccountStatement_Select(datefrom, dateto, Acc).ToList();

            var dt1 = GlobalFunction.ToDataTable(result);

            ExcelManager.ExportToExcel(this.HttpContext, "ClaimOnLineAccountReport", dt1, "ClaimOnLineAccountReport");

            return View();
        }

        public ActionResult GetBranchByZoneId(int? zoneId)
        {
            if (zoneId == -1)
            {
                zoneId = null;
            }

            var result = _context.usp_Branch_Select(null, zoneId, true).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBranchByAreaId(int? areaId = null, int? branchId = null)
        {
            if (areaId == -1)
            {
                areaId = null;
            }
            if (branchId == -1)
            {
                branchId = null;
            }
            var rs = _context.usp_BranchByAreaId_Select(branchId == -1 ? null : branchId, areaId == -1 ? null : areaId, true, 0, 99999, null, null, null).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetZoneByEmployee()
        {
            int employeeID;

            employeeID = GlobalFunction.GetLoginDetail(this.HttpContext).Employee_ID;

            int c;

            c = _context.usp_ZoneXEmployee_Select(employeeID, true).Count();

            int? result;

            if (c > 0)
            {
                result = Convert.ToInt32(_context.usp_ZoneXEmployee_Select(employeeID, true).SingleOrDefault().ZoneId);
            }
            else
            {
                result = 0;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void ClaimOnLineReport(FormCollection form)
        {
            var datefrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(form["dtpdatefrom"]));
            var dateto = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(form["dtpdateto"]));

            var version = form["ddlVersion"];

            int? branchId = Convert.ToInt32(form["ddlBranch"]);

            //V1
            int? zoneId = Convert.ToInt32(form["ddlZone"]);

            //V2
            int? areaId = Convert.ToInt32(form["ddlArea"]);

            if (branchId == -1) branchId = null;

            if (zoneId == -1) zoneId = null;

            if (areaId == -1) areaId = null;

            switch (version)
            {
                case "V1":
                    var result = _context.usp_ClaimOnLineReportV2_Select(datefrom, dateto, zoneId, branchId, null).ToList();

                    var dt1 = GlobalFunction.ToDataTable(result);

                    ExcelManager.ExportToExcel(this.HttpContext, "ClaimOnLineReportV1", dt1, "ClaimOnLineReportV1");
                    break;

                case "V2":

                    var result2 = _context.usp_ClaimOnLineReportArea_Select(datefrom, dateto, areaId, branchId, null).Select(x => new
                    {
                        x.เลขอ้างอิง,
                        //x.จำนวนรายเคลม, -- 06074 change 2022-11-01
                        x.ประเภทแผน,
                        x.รายละเอียด,
                        x.ภาค,
                        x.สาขา,
                        รหัสผู้อนุมัติเคลม = x.TransferByEmployeeCode,
                        ผู้อนุมัติเคลม = x.TransferByEmployeeName,
                        x.รหัสผู้ให้บริการ,
                        x.ผู้ให้บริการ,
                        x.รหัสเจ้าของรถ,
                        x.ชื่อเจ้าของรถ,
                        x.สถานะ,
                        x.ยอดเงินที่โอน,
                        x.วันโอนวันแรก,
                        x.วันที่ส่วนกลางโอนล่าสุด,
                        x.ยอดเงินเคลมจริง,
                        x.ยอดเงินที่รับจากกองทุน,
                        x.วันที่กองทุนโอนล่าสุด,
                        x.วันที่เงินครบ,
                        x.ยอดเงินค้างรับ,
                        x.จำนวนวันที่ดำเนินการ,
                        x.CreateDate,
                        หมายเหตุ = x.TransferRemark,
                        ระยะเวลาโอน_นาที = x.TransferTimeAmount,
                        x.จำนวนรายเคลม,
                        x.ธนาคาร,
                        x.เลขบัญชี,
                        x.ชื่อบัญชี
                    }).ToList();

                    var dt2 = GlobalFunction.ToDataTable(result2);

                    ExcelManager.ExportToExcel(this.HttpContext, "ClaimOnLineReportV2", dt2, "ClaimOnLineReportV2");
                    break;

                default:
                    break;
            }

            //เพิ่ม Version 2  ไม่ต้อง return view
            // ViewBag.Branch = _context.usp_Branch_Select(null, null, true).ToList();

            // return View();
        }

        [HttpPost]
        public ActionResult ClaimOnLineForCompensateReport(FormCollection form)
        {
            var datefrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(form["dtpdatefrom"]));
            var dateto = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(form["dtpdateto"]));

            //var dateCompensate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(form["dtpCompensate"]));

            int? branchId = Convert.ToInt32(form["ddlBranch"]);

            if (branchId == -1)
            {
                branchId = null;
            }

            var result = _context.usp_ClaimOnLineReport_ForCompensate_Select(datefrom, dateto, branchId).ToList();

            var dt1 = GlobalFunction.ToDataTable(result);

            ExcelManager.ExportToExcel(this.HttpContext, "ClaimOnLineForCompensateReport", dt1, "ClaimOnLineReport");

            ViewBag.Branch = _context.usp_Branch_Select(null, null, true).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult ClaimOnLineFollowClaimBranchReport(FormCollection form)
        {
            var datefrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(form["dtpdatefrom"]));
            var dateto = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(form["dtpdateto"]));

            //var dateCompensate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(form["dtpCompensate"]));

            int? branchId = Convert.ToInt32(form["ddlBranch"]);

            if (branchId == -1)
            {
                branchId = null;
            }

            var result = _context.usp_ClaimOnLineReport_FollowClaimBranch_Select(datefrom, dateto, branchId).ToList();

            var dt1 = GlobalFunction.ToDataTable(result);

            ExcelManager.ExportToExcel(this.HttpContext, "ClaimOnLineFollowBranchReport", dt1, "ClaimOnLineReport");

            ViewBag.Branch = _context.usp_Branch_Select(null, null, true).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult ClaimOnLineCompensationCreateReport(FormCollection form)
        {
            int employeeID;
            employeeID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var datecover = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(form["dateCover"]));
            var result = _context.usp_ClaimOnLineRawdataHeader_Insert(datecover, employeeID).Single();

            string[] rs = new string[3];

            rs[0] = result.IsResult.Value.ToString();
            rs[1] = result.Result;
            rs[2] = result.Msg;

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClaimOnLineCompensationSearchReport(string dateFrom, string dateTo,
                                                  int? draw = null, int? pageSize = null, int? pageStart = null,
                                                  string sortField = null, string orderType = null, string search = null)
        {
            int employeeID;
            employeeID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var datefrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
            var dateto = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));
            var result = _context.usp_ClaimOnLineRawdataHeader_Select(datefrom, dateto, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClaimOnLineCompensationUpdateReport(FormCollection form)
        {
            int employeeID;
            employeeID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var header_id = Convert.ToInt32(form["hid"]);
            var result = _context.usp_ClaimOnLineRawdataHeader_Update(header_id, employeeID, false).Single();

            string[] rs = new string[3];

            rs[0] = result.IsResult.Value.ToString();
            rs[1] = result.Result;
            rs[2] = result.Msg;

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClaimOnLineCompensationExportReport(FormCollection form)
        {
            //await Task.Yield();
            using (var db = new ClaimOnLineDBContext())
            {
                int employeeID;
                employeeID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                var header_id = Convert.ToInt32(form["hid"]);
                var data_sheet1 = _context.usp_ClaimOnLineRawdataSheet1Detail_Select(header_id).ToList();
                //var data_sheet2 = _context.usp_ClaimOnLineRawdataSheet2Detail_Select(header_id).ToList();

                var data_sheet2 = _context.usp_ClaimOnLineRawdataSheet2Detail_Select(header_id).Select((x) => new
                {
                    x.RawdataDetailSheet2Id,
                    x.RawdataHeaderId,
                    x.ClaimOnLineCode,
                    x.Code,
                    x.Pay,
                    x.ReceiveDocDate,
                    x.ApproveById,
                    x.ServiceByEmpCode,
                    x.ServiceByEmpName,
                    x.Branch,
                    x.TransferDateStart,
                    x.Detail,
                    x.ZebraByEmpCode,
                    x.ZebraByEmpName,
                    x.ClaimOnLineCreateByCode,
                    x.ClaimOnLineCreateByName,
                    x.ClaimOnLineStatus,
                    x.ClaimOnLineCreateDate,
                    เขตพื้นที่ = x.StudyArea
                }).ToList();

                var stream = new MemoryStream();

                using (var package = new ExcelPackage(stream))
                {
                    var workSheet1 = package.Workbook.Worksheets.Add("MO");
                    workSheet1.Cells.LoadFromCollection(data_sheet1, true);

                    // Select only the header cells
                    var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                    // Set their text to bold.
                    headerCells1.Style.Font.Bold = true;
                    // Set their background color
                    headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells1.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                    // Apply the auto-filter to all the columns
                    var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                    allCells1.AutoFilter = true;
                    // Auto-fit all the columns
                    allCells1.AutoFitColumns();

                    var workSheet2 = package.Workbook.Worksheets.Add("ตัวแทน");
                    workSheet2.Cells.LoadFromCollection(data_sheet2, true);

                    // Select only the header cells
                    var headerCells2 = workSheet2.Cells[1, 1, 1, workSheet2.Dimension.Columns];
                    // Set their text to bold.
                    headerCells2.Style.Font.Bold = true;
                    // Set their background color
                    headerCells2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells2.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                    // Apply the auto-filter to all the columns
                    var allCells = workSheet2.Cells[workSheet2.Dimension.Address];
                    allCells.AutoFilter = true;
                    // Auto-fit all the columns
                    allCells.AutoFitColumns();
                    workSheet1.Cells[1, 23].Value = "เขตพื้นที่";
                    //workSheet2.Cells[1, 18].Value = "เขตพื้นที่";

                    package.Save();

                    stream.Position = 0;
                    Session["DownloadExcel_FileClaimOnlineCompensation"] = package.GetAsByteArray();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult ClaimOnLineCompensationDownloadReport()
        {
            if (Session["DownloadExcel_FileClaimOnlineCompensation"] != null)
            {
                byte[] data = Session["DownloadExcel_FileClaimOnlineCompensation"] as byte[];
                string excelName = $"Report-ClaimOnLineCompensation-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpPost]
        public ActionResult ClaimOnLineReserveBranchExport(int? branchId, string dateFrom, string dateTo)
        {
            Log.Debug("{_controllerName} Start ClaimOnLineReserveBranchExport [branchId = {branchId}, dateFrom = {dateFrom}, dateTo = {dateTo}]", _controllerName, branchId, dateFrom, dateTo);
            Session.Remove("DownloadExcel");
            using (var db = new ClaimOnLineDBContext())
            {
                var datefrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
                var dateto = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));

                try
                {
                    Log.Debug($"{_controllerName} - ClaimOnLineReserveBranchExport Call Store procedure: usp_ClaimOnLineReserveBranch_Select [dateFrom = {dateFrom}, dateTo = {dateTo}, productType_ID = 6,branchId = {branchId}]");
                    var dataSheet1 = db.usp_ClaimOnLineReserveBranch_Select(datefrom, dateto, 6, branchId).ToList()
                        .Select(x => new
                        {
                            เลข_COL = x.ClaimOnlineCode,
                            จำนวน_CL = x.ClaimCount,
                            ประเภทแผน = x.ProductType,
                            รายละเอียด = x.ClaimDetail,
                            ชื่อผู้เอาประกัน = x.ClaimCustomer,
                            ภาค = x.Area,
                            สาขา = x.Branch,
                            CreateDate = x.ClaimCreatedDate != null ? x.ClaimCreatedDate.Value.Date.ToString("dd/MM/yyyy") : "",
                            //ยอดที่แจ้งเคลม = x.TransferAmountTotal,
                            รหัสผู้แจ้งเคลม = x.CreatedByEmployeeCode,
                            ชื่อผู้แจ้งเคลม = x.CreatedEmployeeName,
                            รหัสผู้ให้บริการ = x.ServiceByEmployeeCode,
                            ผู้ให้บริการ = x.ServiceByEmployeeName,
                            รหัสเจ้าของรถ = x.ZebraCarOwnerByEmployeeId,
                            ชื่อเจ้าของรถ = x.ZebraCarOwnerByEmployeeName,
                            ยอดเงินที่โอน = x.TransferAmountTotal,
                            วันโอนวันแรก = x.minTransferDate != null ? x.minTransferDate.Value.Date.ToString("dd/MM/yyyy") : "",
                            วันที่ส่วนกลางโอนล่าสุด = x.TransferDateLatest != null ? x.TransferDateLatest.Value.Date.ToString("dd/MM/yyyy") : "",
                            รหัสผู้บันทึกโอนเงิน = x.TransferByEmployeeCode,
                            ผู้บันทึกโอนเงิน = x.TransferByEmployeeName,
                            ประเภทผู้รับเงิน = x.PayeeType,
                            ธนาคาร = x.PayeeBank,
                            เลขที่บัญชี = x.PayeeAccountNo,
                            ชื่อบัญชี = x.PayeeAccountName
                        });

                    Log.Debug($"{_controllerName} - ClaimOnLineReserveBranchExport Call Store procedure: usp_ClaimOnLineReserveBranch_Select [dateFrom = {dateFrom}, dateTo = {dateTo}, productType_ID = 26,branchId = {branchId}]");
                    var dataSheet2 = db.usp_ClaimOnLineReserveBranch_Select(datefrom, dateto, 26, branchId).ToList()
                        .Select(x => new
                        {
                            เลข_COL = x.ClaimOnlineCode,
                            จำนวน_CL = x.ClaimCount,
                            ประเภทแผน = x.ProductType,
                            รายละเอียด = x.ClaimDetail,
                            ชื่อโรงเรียน = x.School,
                            ภาค = x.Area,
                            สาขา = x.Branch,
                            CreateDate = x.ClaimCreatedDate != null ? x.ClaimCreatedDate.Value.Date.ToString("dd/MM/yyyy") : "",
                            //ยอดที่แจ้งเคลม = x.TransferAmountTotal,
                            รหัสผู้แจ้งเคลม = x.CreatedByEmployeeCode,
                            ชื่อผู้แจ้งเคลม = x.CreatedEmployeeName,
                            รหัสผู้ให้บริการ = x.ServiceByEmployeeCode,
                            ผู้ให้บริการ = x.ServiceByEmployeeName,
                            รหัสเจ้าของรถ = x.ZebraCarOwnerByEmployeeId,
                            ชื่อเจ้าของรถ = x.ZebraCarOwnerByEmployeeName,
                            ยอดเงินที่โอน = x.TransferAmountTotal,
                            วันโอนวันแรก = x.minTransferDate != null ? x.minTransferDate.Value.Date.ToString("dd/MM/yyyy") : "",
                            วันที่ส่วนกลางโอนล่าสุด = x.TransferDateLatest != null ? x.TransferDateLatest.Value.Date.ToString("dd/MM/yyyy") : "",
                            รหัสผู้บันทึกโอนเงิน = x.TransferByEmployeeCode,
                            ผู้บันทึกโอนเงิน = x.TransferByEmployeeName,
                            ประเภทผู้รับเงิน = x.PayeeType,
                            ธนาคาร = x.PayeeBank,
                            เลขที่บัญชี = x.PayeeAccountNo,
                            ชื่อบัญชี = x.PayeeAccountName
                        });

                    Log.Debug($"{_controllerName} - ClaimOnLineReserveBranchExport Call Store procedure: usp_ClaimOnLineReserveBranchDetailPA_Select [dateFrom = {dateFrom}, dateTo = {dateTo}, productType_ID = 26,branchId = {branchId}]");
                    var dataSheet3 = db.usp_ClaimOnLineReserveBranchDetailPA_Select(datefrom, dateto, 26, branchId).ToList()
                        .Select(x => new
                        {
                            เลข_COL = x.ClaimOnlineCode,
                            เลข_CL = x.ClaimCode,
                            ประเภทแผน = x.ProductType,
                            รายละเอียด = x.ClaimDetail,
                            ชื่อโรงเรียน = x.School,
                            ชื่อผู้เอาประกัน = x.CustomerName,
                            ภาค = x.Area,
                            สาขา = x.Branch,
                            CreateDate = x.ClaimCreatedDate != null ? x.ClaimCreatedDate.Value.Date.ToString("dd/MM/yyyy") : "",
                            //ยอดที่แจ้งเคลม = x.TransferAmountTotal,
                            รหัสผู้แจ้งเคลม = x.CreatedByEmployeeCode,
                            ชื่อผู้แจ้งเคลม = x.CreatedEmployeeName,
                            รหัสผู้ให้บริการ = x.ServiceByEmployeeCode,
                            ผู้ให้บริการ = x.ServiceByEmployeeName,
                            รหัสเจ้าของรถ = x.ZebraCarOwnerByEmployeeId,
                            ชื่อเจ้าของรถ = x.ZebraCarOwnerByEmployeeName,
                            ยอดเงินที่โอน = x.ClaimAmount,
                            วันโอนวันแรก = x.minTransferDate != null ? x.minTransferDate.Value.Date.ToString("dd/MM/yyyy") : "",
                            วันที่ส่วนกลางโอนล่าสุด = x.TransferDateLatest != null ? x.TransferDateLatest.Value.Date.ToString("dd/MM/yyyy") : "",
                            รหัสผู้บันทึกโอนเงิน = x.TransferByEmployeeCode,
                            ผู้บันทึกโอนเงิน = x.TransferByEmployeeName,
                            ประเภทผู้รับเงิน = x.PayeeType,
                            ธนาคาร = x.PayeeBank,
                            เลขที่บัญชี = x.PayeeAccountNo,
                            ชื่อบัญชี = x.PayeeAccountName
                        });

                    if (dataSheet1.Count() != 0 || dataSheet2.Count() != 0 || dataSheet3.Count() != 0)
                    {
                        var stream = new MemoryStream();
                        using (var package = new ExcelPackage(stream))
                        {
                            var workSheet1 = package.Workbook.Worksheets.Add("PH");
                            workSheet1.Cells.LoadFromCollection(dataSheet1, true);

                            var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                            headerCells1.Style.Font.Bold = true;
                            headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerCells1.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                            var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                            allCells1.AutoFilter = true;
                            allCells1.AutoFitColumns();

                            var workSheet2 = package.Workbook.Worksheets.Add("PA");
                            workSheet2.Cells.LoadFromCollection(dataSheet2, true);
                            var headerCells2 = workSheet2.Cells[1, 1, 1, workSheet2.Dimension.Columns];
                            headerCells2.Style.Font.Bold = true;
                            headerCells2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerCells2.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                            var allCells = workSheet2.Cells[workSheet2.Dimension.Address];
                            allCells.AutoFilter = true;
                            allCells.AutoFitColumns();

                            var workSheet3 = package.Workbook.Worksheets.Add("Detail");
                            workSheet3.Cells.LoadFromCollection(dataSheet3, true);
                            var headerCells3 = workSheet3.Cells[1, 1, 1, workSheet3.Dimension.Columns];
                            headerCells3.Style.Font.Bold = true;
                            headerCells3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerCells3.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                            var allCells3 = workSheet3.Cells[workSheet3.Dimension.Address];
                            allCells3.AutoFilter = true;
                            allCells3.AutoFitColumns();

                            package.Save();

                            stream.Position = 0;
                            Session["DownloadExcel"] = package.GetAsByteArray();
                            Log.Information("{_controllerName} - ClaimOnLineReserveBranchExport Successful [branchId = {branchId}, dateFrom = {dateFrom}, dateTo = {dateTo}]", _controllerName, branchId, dateFrom, dateTo);
                            //return Json(ResponseResult.Success<string>(_exportToExcelSuccessFul), JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        Log.Debug("{_controllerName} End ClaimOnLineReserveBranchExport [Data not found] [branchId = {branchId}, dateFrom = {dateFrom}, dateTo = {dateTo}]", _controllerName, branchId, dateFrom, dateTo);
                        return Json(ResponseResult.Failure<string>(_dataNotFound), JsonRequestBehavior.AllowGet);
                    }
                    return Json(ResponseResult.Success<string>(_exportToExcelSuccessFul), JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    Session.Remove("DownloadExcel");
                    //Log.Error(ex, "{_controllerName} - ClaimOnLineReserveBranchExport Error: {Message}", _controllerName, ex.Message);
                    Log.Error(ex, "{_controllerName} - ClaimOnLineReserveBranchExport Error: {Message} [branchId = {branchId}, dateFrom = {dateFrom}, dateTo = {dateTo}]", _controllerName, ex.Message, branchId, dateFrom, dateTo);
                    return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult ClaimOnLineReserveBranchExportDownload()
        {
            if (Session["DownloadExcel"] != null)
            {
                byte[] data = Session["DownloadExcel"] as byte[];
                string excelName = $"Report-ClaimOnLineReserveBranch-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpPost]
        public ActionResult ClaimOnLineHeaderGroupExport(int? branchId, string dateFrom, string dateTo)
        {
            Log.Debug("{_controllerName} Start ClaimOnLineHeaderGroupExport [branchId = {branchId}, dateFrom = {dateFrom}, dateTo = {dateTo}]", _controllerName, branchId, dateFrom, dateTo);
            Session.Remove("DownloadExcel");
            using (var db = new ClaimOnLineDBContext())
            {
                var datefrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
                var dateto = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));
                try
                {
                    //PH
                    Log.Debug($"{_controllerName} - ClaimOnLineHeaderGroupExport Call Store procedure: usp_ClaimOnLineHeaderGroup_Select [dateFrom = {dateFrom}, dateTo = {dateTo}, productType_ID = 6,branchId = {branchId}]");
                    var dataSheet1 = db.usp_ClaimOnLineHeaderGroup_Select(datefrom, dateto, 6, branchId).ToList()
                        .Select(x => new
                        {
                            เลข_COL = x.ClaimOnlineCode,
                            เลข_CL = x.claimPH_ClaimHeaderCode,
                            ยอดเงินเคลม = x.ClaimPH_Total,
                            เลข_บส = x.claimPH_ClaimHeaderGroup_Id,
                            ยอดเงินตาม_บส = x.TransferAmountTotal,
                            รหัสผู้แจ้งเคลม = x.CreatedByEmployeeCode,
                            ชื่อผู้แจ้งเคลม = x.CreatedEmployeeName,
                            รหัสผู้ให้บริการ = x.ServiceByEmployeeCode,
                            ผู้ให้บริการ = x.ServiceByEmployeeName,
                            วันที่โอนวันแรก = x.minTransferDate != null ? x.minTransferDate.Value.Date.ToString("dd/MM/yyyy") : "",
                            วันที่โอนล่าสุด = x.TransferDateLatest != null ? x.TransferDateLatest.Value.Date.ToString("dd/MM/yyyy") : "",
                            ประเภทแผน = x.ProductType,
                            ชื่อผู้เอาประกัน = x.ClaimPH_Customer,
                            รายละเอียด = x.ClaimDetail,
                            ภาค = x.Area,
                            สาขา = x.Branch,
                            วันที่สร้าง_บส = x.ClaimPH_ClaimHeaderGroupCreatedDate != null ? x.ClaimPH_ClaimHeaderGroupCreatedDate.Value.Date.ToString("dd/MM/yyyy") : "",
                            รหัสผู้ทำ_บส = x.ClaimPH_ClaimHeaderGroupCreatedByCode,
                            ชื่อผู้ทำ_บส = x.ClaimPH_ClaimHeaderGroupCreatedByName,
                            วันที่อนุมัติเคลม = x.ClaimPH_ClaimApprovedDate != null ? x.ClaimPH_ClaimApprovedDate.Value.Date.ToString("dd/MM/yyyy") : "",
                            รหัสผู้อนุมัติเคลม = x.ClaimPH_ClaimApprovedByCode,
                            ชื่อผู้อนุมัติเคลม = x.ClaimPH_ClaimApprovedByName,
                            สถานะเคลม = x.ClaimPH_Status
                        });

                    //PA
                    Log.Debug($"{_controllerName} - ClaimOnLineHeaderGroupExport Call Store procedure: usp_ClaimOnLineHeaderGroup_Select [dateFrom = {dateFrom}, dateTo = {dateTo}, productType_ID = 26,branchId = {branchId}]");
                    var dataSheet2 = db.usp_ClaimOnLineHeaderGroup_Select(datefrom, dateto, 26, branchId).ToList()
                        .Select(x => new
                        {
                            เลข_COL = x.ClaimOnlineCode,
                            เลข_CL = x.ClaimPA_ClaimHeaderCode,
                            ยอดเงินเคลม = x.ClaimPA_AmountNet,
                            เลข_บส = x.ClaimPA_ClaimHeaderGroup_Id,
                            ยอดเงินตาม_บส = x.TransferAmountTotal,
                            รหัสผู้แจ้งเคลม = x.CreatedByEmployeeCode,
                            ชื่อผู้แจ้งเคลม = x.CreatedEmployeeName,
                            รหัสผู้ให้บริการ = x.ServiceByEmployeeCode,
                            ผู้ให้บริการ = x.ServiceByEmployeeName,
                            วันที่โอนวันแรก = x.minTransferDate != null ? x.minTransferDate.Value.Date.ToString("dd/MM/yyyy") : "",
                            วันที่โอนล่าสุด = x.TransferDateLatest != null ? x.TransferDateLatest.Value.Date.ToString("dd/MM/yyyy") : "",
                            ประเภทแผน = x.ProductType,
                            ชื่อโรงเรียน = x.ClaimPA_School,
                            รายละเอียด = x.ClaimDetail,
                            ภาค = x.Area,
                            สาขา = x.Branch,
                            วันที่สร้าง_บส = x.ClaimPA_headerGroupCreatedDate != null ? x.ClaimPA_headerGroupCreatedDate.Value.Date.ToString("dd/MM/yyyy") : "",
                            รหัสผู้ทำ_บส = x.ClaimPA_headerGroupCreatedByCode,
                            ชื่อผู้ทำ_บส = x.ClaimPA_headerGroupCreatedByName,
                            วันที่อนุมัติเคลม = x.ClaimPA_ApprovedDate != null ? x.ClaimPA_ApprovedDate.Value.Date.ToString("dd/MM/yyyy") : "",
                            รหัสผู้อนุมัติเคลม = x.ClaimPA_ApprovedByCode,
                            ชื่อผู้อนุมัติเคลม = x.ClaimPA_ApprovedByName,
                            สถานะเคลม = x.ClaimPA_Status
                        });

                    var stream = new MemoryStream();
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet1 = package.Workbook.Worksheets.Add("PH");
                        workSheet1.Cells.LoadFromCollection(dataSheet1, true);

                        var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                        headerCells1.Style.Font.Bold = true;
                        headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        headerCells1.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                        var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                        allCells1.AutoFilter = true;
                        allCells1.AutoFitColumns();

                        var workSheet2 = package.Workbook.Worksheets.Add("PA");
                        workSheet2.Cells.LoadFromCollection(dataSheet2, true);
                        var headerCells2 = workSheet2.Cells[1, 1, 1, workSheet2.Dimension.Columns];
                        headerCells2.Style.Font.Bold = true;
                        headerCells2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        headerCells2.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                        var allCells = workSheet2.Cells[workSheet2.Dimension.Address];
                        allCells.AutoFilter = true;
                        allCells.AutoFitColumns();

                        package.Save();

                        stream.Position = 0;
                        Session["DownloadExcel"] = package.GetAsByteArray();
                        Log.Information("{_controllerName} - ClaimOnLineHeaderGroupExport Successful", _controllerName);
                        return Json(ResponseResult.Success<string>(_exportToExcelSuccessFul), JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    Session.Remove("DownloadExcel");
                    Log.Error(ex, "{_controllerName} - ClaimOnLineHeaderGroupExport Error: {Message}", _controllerName, ex.Message);
                    return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult ClaimOnLineHeaderGroupExportDownload()
        {
            if (Session["DownloadExcel"] != null)
            {
                byte[] data = Session["DownloadExcel"] as byte[];
                string excelName = $"Report-ClaimOnLineHeaderGroup-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpPost]
        public ActionResult ClaimOnLineCutOffPaymentExport(string dateFrom, string dateTo)
        {
            Log.Debug("{_controllerName} Start ClaimOnLineCutOffPaymentExport [dateFrom = {dateFrom}, dateTo = {dateTo}]", _controllerName, dateFrom, dateTo);
            Session.Remove("DownloadExcel");
            var datefrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
            var dateto = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));

            try
            {
                Log.Debug($"{_controllerName} - ClaimOnLineCutOffPaymentExport Call Store procedure: usp_ClaimOnLinePayment_Select [dateFrom = {dateFrom}, dateTo = {dateTo}]");
                var result = _context.usp_ClaimOnLinePayment_Select(datefrom, dateto).Select(x => new
                {
                    หมายเลข_บส = x.ClaimHeaderGroup_id,
                    หมายเลข_COL = x.ClaimOnlineCode,
                    หมายเลข_CL = x.ClaimCode,
                    ประเภทแผน = x.ProductType,
                    ภาค = x.Area,
                    สาขา = x.Branch,
                    วันที่รับเงินจากกองทุน = x.TransferDate != null ? x.TransferDate.Value.Date.ToString("dd/MM/yyyy") : "",
                    ยอดเงินเคลม = x.TotalClaim,
                    จำนวนเงินรับยอดตาม_บส = x.TotalPay,
                    COL_ครั้งที่รับเงิน = x.ReturnRow
                }).ToList();

                if (result.Count > 0)
                {
                    var stream = new MemoryStream();
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet1 = package.Workbook.Worksheets.Add("รายงานตัดชำระ");
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
                        Session["DownloadExcel"] = package.GetAsByteArray();
                        Log.Information("{_controllerName} - ClaimOnLineCutOffPaymentExport Successful", _controllerName);
                        return Json(ResponseResult.Success<string>(_exportToExcelSuccessFul), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    Log.Debug("{_controllerName} - ClaimOnLineCutOffPaymentExport [Data not found]", _controllerName);
                    return Json(ResponseResult.Failure<string>(_dataNotFound), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Session.Remove("DownloadExcel");
                Log.Error(ex, "{_controllerName} - ClaimOnLineCutOffPaymentExport Error: {Message}", _controllerName, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ClaimOnLineCutOffPaymentExportDownload()
        {
            if (Session["DownloadExcel"] != null)
            {
                byte[] data = Session["DownloadExcel"] as byte[];
                string excelName = $"Report-ClaimOnLinePayment-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpPost]
        public ActionResult ClaimOnLineSurveyExport(string dateFrom, string dateTo, int? areaId = null, int? branchId = null, int? serviceByUserId = null, int? dateType = null,int? reportVersion=null)
        {
            Log.Debug("{_controllerName} Start ClaimOnLineSurveyExport [dateFrom = {dateFrom}, dateTo = {dateTo}, areaId = {areaId}, branchId = {branchId}, serviceByUserId = {serviceByUserId}, dateType = {dateType}]", _controllerName, dateFrom, dateTo, areaId, branchId, serviceByUserId, dateType);
            Session.Remove("DownloadExcel");
            var datefrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
            var dateto = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));
            var TransactionDateTimeFrom = datefrom;
            var TransactionDateTimeTo = dateto;
            if (areaId == -1) areaId = null;
            if (branchId == -1) branchId = null;
            if (serviceByUserId == -1) serviceByUserId = null;
            
            try
            {
                if (reportVersion == 1)
                {
                    Log.Debug($"{_controllerName} - ClaimOnLineSurveyExport Call Store procedure: usp_ClaimOnlineSurveyReport_Select [dateFrom = {dateFrom}, dateTo = {dateTo} ]");
                    var result = _context.usp_ClaimOnlineSurveyReport_Select(TransactionDateTimeFrom, TransactionDateTimeTo).Select(x => new
                    {
                        วันที่ส่ง_SMS = x.SentSMSDate != null ? x.SentSMSDate.Value.ToString("dd/MM/yyyy") : "",
                        เวลาส่ง_SMS = x.SentSMSDate != null ? x.SentSMSDate.Value.ToString("HH:mm:ss") : "",
                        COL = x.ClaimOnLineCode,
                        ชื่อลูกค้า = x.ToAccountName,
                        //เบอร์โทรศัพท์ = x.SMSPhoneNo,
                        วันที่บันทึกเคลม = x.ClaimDate != null ? x.ClaimDate.Value.ToString("dd/MM/yyyy") : "",
                        ยอดจ่าย = x.Amount,
                        Product_Group = x.ProductGroup,
                        Product_Type = x.ProductType,
                        จำนวนเคลม = x.ClaimCount,
                        รหัสผู้ให้บริการ = x.EmployeeCode,
                        ชื่อผู้ให้บริการ = x.PersonName,
                        สาขาผู้ให้บริการ = x.Branch,
                        ///สถานะส่ง_SMS = x.StatusSentSMS,
                        ผลการตอบกลับแบบประเมิน = x.AnswerResult,
                        คะแนนความพึงพอใจ = x.AnswerDetail,
                        ข้อเสนอแนะ = x.AnswerMore,
                        วันที่ตอบกลับ = x.SubmitSurveyDate != null ? x.SubmitSurveyDate.Value.ToString("dd/MM/yyyy") : "",
                        เวลาที่ตอบกลับ = x.SubmitSurveyDate != null ? x.SubmitSurveyDate.Value.ToString("HH:mm:ss") : "",
                        IsSMSSurveyLink = x.IsSMSSurveyLink
                    }).ToList();

                    if (result.Count > 0)
                    {
                        var stream = new MemoryStream();
                        using (var package = new ExcelPackage(stream))
                        {
                            var workSheet1 = package.Workbook.Worksheets.Add("รายงานแบบประเมินความพึงพอใจ V1");
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
                            Session["DownloadExcel"] = package.GetAsByteArray();
                            Log.Information("{_controllerName} - ClaimOnLineSurveyExport Successful", _controllerName);
                            //return Json(ResponseResult.Success<string>(_exportToExcelSuccessFul), JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        Log.Debug("{_controllerName} - ClaimOnLineSurveyExport [Data not found]", _controllerName);
                        return Json(ResponseResult.Failure<string>(_dataNotFound), JsonRequestBehavior.AllowGet);
                    }
                }
                else if (reportVersion == 2)
                {
                    Log.Debug($"{_controllerName} - ClaimOnLineSurveyExport Call Store procedure: usp_ClaimOnlineSurveyReport_Select_V4 [dateFrom = {dateFrom}, dateTo = {dateTo}, areaId = {areaId}, branchId = {branchId}, serviceByUserId = {serviceByUserId}, dateType = {dateType} ]");
                    var result = _context.usp_ClaimOnlineSurveyReport_Select_V4(areaId, branchId, serviceByUserId, dateType, datefrom, dateto).Select(x => new
                    {
                        วันที่ส่ง_SMS = x.SentSMSDate != null ? x.SentSMSDate.Value.ToString("dd/MM/yyyy") : "",
                        เวลาส่ง_SMS = x.SentSMSDate != null ? x.SentSMSDate.Value.ToString("HH:mm:ss") : "",
                        COL = x.ClaimOnLineCode,
                        ชื่อลูกค้า = x.CustomerName,
                        วันที่บันทึกเคลม = x.ClaimDate != null ? x.ClaimDate.Value.ToString("dd/MM/yyyy") : "",
                        ยอดจ่าย = x.Amount,
                        Product_Group = x.ProductGroup,
                        Product_Type = x.ProductType,
                        จำนวนเคลม = x.ClaimCount,
                        รหัสผู้ให้บริการ = x.ServiceByCode,
                        ชื่อผู้ให้บริการ = x.ServiceByName,
                        สาขาผู้ให้บริการ = x.ServiceBranch,
                        ผลการตอบกลับแบบประเมิน = x.AnswerResult,
                        คะแนนความพึงพอใจ = x.AnswerDetail,
                        ข้อเสนอแนะ = x.AnswerMore,
                        วันที่ตอบกลับ = x.SubmitSurveyDate != null ? x.SubmitSurveyDate.Value.ToString("dd/MM/yyyy") : "",
                        เวลาที่ตอบกลับ = x.SubmitSurveyDate != null ? x.SubmitSurveyDate.Value.ToString("HH:mm:ss") : "",
                        IsSMSSurveyLink = x.IsSMSSurveyLink
                    }).ToList();
                    if (result.Count > 0)
                    {
                        var stream = new MemoryStream();
                        using (var package = new ExcelPackage(stream))
                        {
                            var workSheet1 = package.Workbook.Worksheets.Add("รายงานแบบประเมินความพึงพอใจ V2");
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
                            Session["DownloadExcel"] = package.GetAsByteArray();
                            Log.Information("{_controllerName} - ClaimOnLineSurveyExport Successful", _controllerName);
                            //return Json(ResponseResult.Success<string>(_exportToExcelSuccessFul), JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        Log.Debug("{_controllerName} - ClaimOnLineSurveyExport [Data not found]", _controllerName);
                        return Json(ResponseResult.Failure<string>(_dataNotFound), JsonRequestBehavior.AllowGet);
                    }                    
                }
                return Json(ResponseResult.Success<string>(_exportToExcelSuccessFul), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Session.Remove("DownloadExcel");
                Log.Error(ex, "{_controllerName} - ClaimOnLineSurveyExport Error: {Message}", _controllerName, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ClaimOnLineSurveyExportDownload()
        {
            if (Session["DownloadExcel"] != null)
            {
                byte[] data = Session["DownloadExcel"] as byte[];
                string excelName = $"Report-ClaimOnLineSurvey-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        public ActionResult ClaimOnLineRejectClaimReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ClaimOnLineRejectClaimReportExport(string dateFrom, string dateTo)
        {
            Log.Debug("{_controllerName} Start ClaimOnLineRejectClaimReportExport [dateFrom = {dateFrom}, dateTo = {dateTo}]", _controllerName, dateFrom, dateTo);
            Session.Remove("DownloadExcel");
            var datefrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
            var dateto = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));
            try
            {
                Log.Debug($"{_controllerName} - ClaimOnLineRejectClaimReportExport Call Store procedure: usp_ClaimOnLineRejectClaimReport [dateFrom = {dateFrom}, dateTo = {dateTo}]");
                var result = _context.usp_ClaimOnLineRejectClaimReport(datefrom, dateto).Select(x => new
                {
                    เลข_COL = x.ClaimOnLineCode,
                    ประเภทเคลม = x.ProductTypeDetail,
                    เลข_AppID = x.App_Id,
                    ชื่อผู้เอาประกัน = x.ClaimCustomer,
                    ภาค = x.AreaDetail,
                    สาขาผู้ให้บริการ = x.BranchDetail,
                    รหัสผู้ให้บริการ = x.ServiceByEmployeeId,
                    ชื่อผู้ให้บริการ = x.ServiceByEmployeeName,
                    claimAmount = x.ClaimAmount,
                    รหัสผู้ยกเลิก_Admin = x.CancelByEmployeeId,
                    ชื่อผู้ยกเลิก_Admin = x.CancelByEmployeeName,
                    สาเหตุที่Reject = x.CancelCause,
                    หมายเหตุที่Reject = x.Remark,
                    จำนวนรายเคลม = x.ClaimCount
                }).ToList();

                if (result.Count > 0)
                {
                    var stream = new MemoryStream();
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet1 = package.Workbook.Worksheets.Add("รายงานรีเจคเคลม");
                        workSheet1.Cells.LoadFromCollection(result, true);
                        workSheet1.Cells["I1"].Value = "ยอดที่แจ้ง / ยกเลิก";
                        var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                        headerCells1.Style.Font.Bold = true;
                        headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        headerCells1.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                        var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                        allCells1.AutoFilter = true;
                        allCells1.AutoFitColumns();

                        package.Save();

                        stream.Position = 0;
                        Session["DownloadExcel"] = package.GetAsByteArray();
                        Log.Information("{_controllerName} - ClaimOnLineRejectClaimReportExport Successful", _controllerName);
                        return Json(ResponseResult.Success<string>(_exportToExcelSuccessFul), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    Log.Debug("{_controllerName} - ClaimOnLineRejectClaimReportExport [Data not found]", _controllerName);
                    return Json(ResponseResult.Failure<string>(_dataNotFound), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Session.Remove("DownloadExcel");
                Log.Error(ex, "{_controllerName} - ClaimOnLineRejectClaimReportExport Error: {Message}", _controllerName, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ClaimOnLineRejectClaimReportExportDownload()
        {
            if (Session["DownloadExcel"] != null)
            {
                byte[] data = Session["DownloadExcel"] as byte[];
                string excelName = $"Report-ClaimOnLineRejectClaim-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        public ActionResult RefundTransferReport()
        {
            var checkMonth = _context.usp_ProgramConfig_Select("ClaimOnLine_FundTransfer_Report", null, null, null, null, null).FirstOrDefault();
            ViewBag.checkMonth = checkMonth.ValueNumber;
            return View();
        }

        [HttpPost]
        public ActionResult RefundTransferReportExport(string dateFrom, string dateTo)
        {
            Log.Debug("{_controllerName} Start RefundTransferReportExport [dateFrom = {dateFrom}, dateTo = {dateTo}]", _controllerName, dateFrom, dateTo);
            Session.Remove("DownloadExcel");
            var datefrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
            var dateto = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));
            try
            {
                Log.Debug($"{_controllerName} - RefundTransferReportExport Call Store procedure: usp_RefundTransferReport [dateFrom = {dateFrom}, dateTo = {dateTo}]");
                var result = _context.usp_RefundTransferReport(datefrom, dateto).Select(x => new
                {
                    เลข_COL = x.ClaimOnLineCode,
                    ภาค = x.AreaDetail,
                    สาขา = x.BranchDetail,
                    รหัสผู้ให้บริการ = x.ServiceByEmployeeId,
                    ชื่อผู้ให้บริการ = x.ServiceByEmployeeName,
                    รหัสผู้แจ้งเคลมออนไลน์ = x.CreateByEmployeeId,
                    ผู้ให้บริการเคลมออนไลน์ = x.CreateByEmployeeName,
                    วันที่บันทึกเคลม = x.ClaimOnLineCreateDate != null ? x.ClaimOnLineCreateDate.Value.ToString("dd-MM-yyyy") : "",
                    วันที่สาขา_โอนคืน = x.ClaimOnLineTransferCreateDate != null ? x.ClaimOnLineTransferCreateDate.Value.ToString("dd-MM-yyyy") : "",
                    จํานวนเงินคืน = x.Amount,
                    สาเหตุที่โอนคืน = x.RefundCause,
                    หมายเหตุที่โอนคืน = x.Remark,
                    จำนวนราย = x.ClaimCount
                }).ToList();

                if (result.Count > 0)
                {
                    var stream = new MemoryStream();
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet1 = package.Workbook.Worksheets.Add("รายงานคืนเงิน");
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
                        Session["DownloadExcel"] = package.GetAsByteArray();
                        Log.Information("{_controllerName} - RefundTransferReportExport Successful", _controllerName);
                        return Json(ResponseResult.Success<string>(_exportToExcelSuccessFul), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    Log.Debug("{_controllerName} - RefundTransferReportExport [Data not found]", _controllerName);
                    return Json(ResponseResult.Failure<string>(_dataNotFound), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Session.Remove("DownloadExcel");
                Log.Error(ex, "{_controllerName} - RefundTransferReportExport Error: {Message}", _controllerName, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult RefundTransferReportExportDownload()
        {
            if (Session["DownloadExcel"] != null)
            {
                byte[] data = Session["DownloadExcel"] as byte[];
                string excelName = $"Report-RefundTransfer-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        ///-----KIttisak 20230215
        public JsonResult GetArea()
        {
            int? areaId = null;
            var rs = _context.usp_Area_Select(areaId, 0, 99999, null, null, null).ToList();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBranch(int? AreaId)
        {
            int? areaId = AreaId;
            var result = _context.usp_BranchByAreaId_Select(null, areaId, null, 0, 9999999, null, null, null).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUserService(int? areaId, int? branchId)
        {
            if (areaId == -1) areaId = null;
            if (branchId == -1) branchId = null;
            var result = _context.usp_GetEmployeeByAreaOrBranch_Select(areaId, branchId).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConsiderTransferPremiumReport()
        {
            ViewBag.Branch = _context.usp_Branch_Select1(null, 0, 99999, null, null, null).ToList();

            return View();
        }

        public ActionResult ConsiderTransferPremiumDailyReport()
        {
            ViewBag.Branch = _context.usp_Branch_Select1(null, 0, 99999, null, null, null).ToList();

            return View();
        }

        private class HeaderConsiderTransferPremiumExport
        {
            public string ประจำวันที่ { get; set; }
            public int? จำนวนรายการ { get; set; }
            public decimal? จำนวนเงิน { get; set; }
        }


        [HttpPost]
        public ActionResult ConsiderTransferPremiumExport(int? branchId = null, string dateFrom = null, string dateTo = null)
        {
            Log.Debug("{_controllerName} Start ConsiderTransferPremiumExport [branchId = {branchId}, dateFrom = {dateFrom}, dateTo = {dateTo}]", _controllerName, branchId, dateFrom, dateTo);
            Session.Remove("DownloadExcel");
            var datefrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
            var dateto = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));

            if (branchId == -1) branchId = null;

            try
            {
                Log.Debug($"{_controllerName} - ConsiderTransferPremiumExport Call Store procedure: usp_ConsiderTransferPremiumReport [branchId = {branchId}, dateFrom = {datefrom}, dateTo = {dateto} ]");
                var result = _context.usp_ConsiderTransferPremiumReport(branchId, datefrom, dateto).ToList();

                var resultSheet1 = new List<HeaderConsiderTransferPremiumExport>();

                var resultSheet2 = result.Where(x => x.PayAutoTypeId == 3).Select(x => new
                {
                    วันที่โอน = x.PaymentDate != null ? x.PaymentDate.Value.ToString("dd/MM/yyyy") : "",
                    เวลาที่โอน = x.PaymentDate != null ? x.PaymentDate.Value.ToString("HH:mm:ss") : "",
                    เลขที่อ้างอิง = x.TransRefNo,
                    สถานะการโอน = x.PayAutoTypeName,
                    เลขที่_COL = x.ClaimOnLineCode,
                    ผู้เอาประกัน = x.ClaimCustomer,
                    จำนวนเงิน = x.Amount,
                    ผู้อนุมัติเคลม = x.ClaimOnLinePersonName,
                    ภาค = x.AreaDetail,
                    สาขา = x.BranchDetail,
                    ชื่อบัญชี = x.ToAccountName,
                    เลขที่บัญชี = x.ToAccountNo,
                    ธนาคาร = x.ToBankName,
                    หมายเหตุ = x.ThaiDescription
                }).ToList();

                var header = new HeaderConsiderTransferPremiumExport
                {
                    ประจำวันที่ = DateTime.Now.ToString("dd-MM-yyyy"),
                    จำนวนรายการ = resultSheet2.Count,
                    จำนวนเงิน = result.Where(x => x.PayAutoTypeId == 3).Sum(x => x.Amount).Value
                };

                resultSheet1.Add(header);

                var resultSheet3 = result.Where(x => x.PayAutoTypeId == 6).Select(x => new
                {
                    วันที่โอน = x.CreatedDate != null ? x.CreatedDate.Value.ToString("dd/MM/yyyy") : "",
                    เวลาที่โอน = x.CreatedDate != null ? x.CreatedDate.Value.ToString("HH:mm:ss") : "",
                    เลขที่อ้างอิง = x.TransRefNo,
                    สถานะการโอน = x.PayAutoTypeName,
                    เลขที่_COL = x.ClaimOnLineCode,
                    ผู้เอาประกัน = x.ClaimCustomer,
                    จำนวนเงิน = x.Amount,
                    ผู้อนุมัติเคลม = x.ClaimOnLinePersonName,
                    ภาค = x.AreaDetail,
                    สาขา = x.BranchDetail,
                    ชื่อบัญชี = x.ToAccountName,
                    เลขที่บัญชี = x.ToAccountNo,
                    ธนาคาร = x.ToBankName,
                    หมายเหตุ = x.ThaiDescription
                }).ToList();

                if (resultSheet2.Count > 0)
                {
                    var stream = new MemoryStream();
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet1 = package.Workbook.Worksheets.Add("สรุปการโอนเงิน");
                        workSheet1.Cells.LoadFromCollection(resultSheet1, true);
                        var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                        headerCells1.Style.Font.Bold = true;
                        headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        headerCells1.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                        var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                        allCells1.AutoFilter = true;
                        allCells1.AutoFitColumns();

                        var workSheet2 = package.Workbook.Worksheets.Add("รายละเอียดการโอนเงิน");
                        workSheet2.Cells.LoadFromCollection(resultSheet2, true);
                        var headerCells2 = workSheet2.Cells[1, 1, 1, workSheet2.Dimension.Columns];
                        headerCells2.Style.Font.Bold = true;
                        headerCells2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        headerCells2.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                        var allCells2 = workSheet2.Cells[workSheet2.Dimension.Address];
                        allCells2.AutoFilter = true;
                        allCells2.AutoFitColumns();

                        var workSheet3 = package.Workbook.Worksheets.Add("ไม่สำเร็จ");
                        workSheet3.Cells.LoadFromCollection(resultSheet3, true);
                        var headerCells3 = workSheet3.Cells[1, 1, 1, workSheet3.Dimension.Columns];
                        headerCells3.Style.Font.Bold = true;
                        headerCells3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        headerCells3.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                        var allCells3 = workSheet3.Cells[workSheet3.Dimension.Address];
                        allCells3.AutoFilter = true;
                        allCells3.AutoFitColumns();

                        package.Save();
                        stream.Position = 0;
                        Session["DownloadExcel"] = package.GetAsByteArray();
                        Log.Information("{_controllerName} - ConsiderTransferPremiumExport Successful", _controllerName);
                        return Json(ResponseResult.Success<string>(_exportToExcelSuccessFul), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    Log.Debug("{_controllerName} - ConsiderTransferPremiumExport [Data not found]", _controllerName);
                    return Json(ResponseResult.Failure<string>(_dataNotFound), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Session.Remove("DownloadExcel");
                Log.Error(ex, "{_controllerName} - ConsiderTransferPremiumExport Error: {Message}", _controllerName, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ConsiderTransferPremiumExportDownLoad()
        {
            if (Session["DownloadExcel"] != null)
            {
                byte[] data = Session["DownloadExcel"] as byte[];
                string excelName = $"{DateTime.Now.ToString("ddMMyyHHmm")}_รายงานการโอนเงินเคลมออนไลน์.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpPost]
        public ActionResult ConsiderTransferPremiumDailyExport(int? branchId = null, string transferDate = null)
        {
            Log.Debug("{_controllerName} Start ConsiderTransferPremiumDailyExport [branchId = {branchId}, transferDate = {transferDate}]", _controllerName, branchId, transferDate);
            Session.Remove("DownloadExcel");
            var d_transferDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(transferDate));

            if (branchId == -1) branchId = null;

            try
            {
                Log.Debug($"{_controllerName} - ConsiderTransferPremiumDailyExport Call Store procedure: usp_ConsiderTransferPremiumDailyReport [branchId = {branchId}, transferDate = {transferDate}]");
                var result = _context.usp_ConsiderTransferPremiumDailyReport(branchId, d_transferDate).Select(x => new
                {
                    วันที่โอน = x.PaymentDate != null ? x.PaymentDate.Value.ToString("dd/MM/yyyy") : "",
                    สถานะการโอน = x.PayAutoTypeName,
                    เลขที่_COL = x.ClaimOnLineCode,
                    ผู้เอาประกัน = x.ClaimCustomer,
                    จำนวนราย = x.ClaimCount,
                    จำนวนเงิน = x.Amount,
                    ผู้อนุมัติเคลม = x.ClaimOnLinePersonName,
                    ภาค = x.AreaDetail,
                    สาขา = x.BranchDetail,
                    ชื่อบัญชี = x.ToAccountName,
                    เลขที่บัญชี = x.ToAccountNo,
                    ธนาคาร = x.ToBankName,
                    หมายเหตุ = x.ThaiDescription
                }).ToList();

                if (result.Count > 0)
                {
                    var stream = new MemoryStream();
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet1 = package.Workbook.Worksheets.Add("รายงานการโอนเงินประจำวัน");
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
                        Session["DownloadExcel"] = package.GetAsByteArray();
                        Log.Information("{_controllerName} - ConsiderTransferPremiumDailyExport Successful", _controllerName);
                        return Json(ResponseResult.Success<string>(_exportToExcelSuccessFul), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    Log.Debug("{_controllerName} - ConsiderTransferPremiumDailyExport [Data not found]", _controllerName);
                    return Json(ResponseResult.Failure<string>(_dataNotFound), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Session.Remove("DownloadExcel");
                Log.Error(ex, "{_controllerName} - ConsiderTransferPremiumDailyExport Error: {Message}", _controllerName, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ConsiderTransferPremiumDailyExportDownLoad()
        {
            if (Session["DownloadExcel"] != null)
            {
                byte[] data = Session["DownloadExcel"] as byte[];
                string excelName = $"{DateTime.Now.ToString("ddMMyyHHmm")}_รายงานการโอนเงินประจำวัน.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpPost]
        public ActionResult WhiteListExport()
        {
            Log.Debug("{_controllerName} Start WhiteListExport", _controllerName);
            Session.Remove("DownloadExcel");

            try
            {
                Log.Debug($"{_controllerName} - WhiteListExport Call Store procedure: usp_ConsiderTransferPremiumDailyReport");
                var result = _context.usp_WhiteList_Select(null, 5000, null, null, null).Select(x => new
                {
                    รหัสพนักงาน = x.EmployeeCode,
                    ชื่อ_สกุลพนักงาน = x.EmployeeName,
                }).ToList();

                if (result.Count > 0)
                {
                    var stream = new MemoryStream();
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet1 = package.Workbook.Worksheets.Add("รายชื่อ WhiteList");
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
                        Session["DownloadExcel"] = package.GetAsByteArray();
                        Log.Information("{_controllerName} - WhiteListExport Successful", _controllerName);
                        return Json(ResponseResult.Success<string>(_exportToExcelSuccessFul), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    Log.Debug("{_controllerName} - WhiteListExport [Data not found]", _controllerName);
                    return Json(ResponseResult.Failure<string>(_dataNotFound), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Session.Remove("DownloadExcel");
                Log.Error(ex, "{_controllerName} - WhiteListExport Error: {Message}", _controllerName, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult WhiteListExportDownLoad()
        {
            if (Session["DownloadExcel"] != null)
            {
                byte[] data = Session["DownloadExcel"] as byte[];
                string excelName = $"{DateTime.Now.ToString("ddMMyyyy")}-รายชื่อ WhiteList.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        public ActionResult ClaimAIReviewReport()
        {
            var checkMonth = _context.usp_ProgramConfig_Select("ClaimAIReviewReport", null, null, null, null, null).FirstOrDefault();
            ViewBag.checkMonth = checkMonth.ValueNumber;
            return View();
        }

        [HttpPost]
        public ActionResult ClaimAIReviewExport(string dateFrom = null, string dateTo = null)
        {
            Log.Debug("{_controllerName} Start ClaimAIReviewExport [dateFrom = {dateFrom}, dateTo = {dateTo}]", _controllerName, dateFrom, dateTo);
            Session.Remove("DownloadExcel");
            var d_dateFrom = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateFrom));
            var d_dateTo = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dateTo));

            try
            {
                Log.Debug($"{_controllerName} - ClaimAIReviewExport Call Store procedure: usp_ClaimAIReviewReport [dateFrom = {d_dateFrom}, dateTo = {d_dateTo}]");
                var result = _context.usp_ClaimAIReviewReport(d_dateFrom, d_dateTo).Select(x => new
                {
                    เลขที่อ้างอิง = x.ClaimOnLineCode,
                    สาขา = x.BranchDetail,
                    รหัสผู้ให้บริการ = x.EmployeeCode,
                    ผู้ให้บริการ = x.PersonName,
                    วันที่โอน = x.TransferDateLatest != null ? x.TransferDateLatest.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                    ยอดเงินที่โอน = x.TransferAmountTotal,
                    ผลการพิจารณา_AI = x.AIResult,
                    หมายเหตุ = x.ErrorDescription,
                }).ToList();

                if (result.Count > 0)
                {
                    var stream = new MemoryStream();
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet1 = package.Workbook.Worksheets.Add("รายงานผลการพิจารณา AI");
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
                        Session["DownloadExcel"] = package.GetAsByteArray();
                        Log.Information("{_controllerName} - ClaimAIReviewExport Successful", _controllerName);
                        return Json(ResponseResult.Success<string>(_exportToExcelSuccessFul), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    Log.Debug("{_controllerName} - ClaimAIReviewExport [Data not found]", _controllerName);
                    return Json(ResponseResult.Failure<string>(_dataNotFound), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Session.Remove("DownloadExcel");
                Log.Error(ex, "{_controllerName} - ClaimAIReviewExport Error: {Message}", _controllerName, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ClaimAIReviewExportDownLoad()
        {
            if (Session["DownloadExcel"] != null)
            {
                byte[] data = Session["DownloadExcel"] as byte[];
                string excelName = $"{DateTime.Now.ToString("ddMMyyHHmm")}_รายงานผลการพิจารณา AI.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        private class ClaimOnlineTranferReconcileModel
        {
            public decimal? sumPaymentAmount { get; set; }
            public decimal? sumPayTransferRefAmount { get; set; }
            public int? countColPayAuto { get; set; }
            public int? countPayTransferRef { get; set; }
        }
        public ActionResult GetClaimOnlineTranferReconcile(DateTime? searchDete)
        {
            var result = _context.usp_ClaimOnlineTransferReconcileGetSumAmount_Select(searchDete).ToList();
            var rs = new ClaimOnlineTranferReconcileModel();
            if (result.Count > 0)
            {
                rs.sumPaymentAmount = result.FirstOrDefault().SumPaymentAmount;
                rs.sumPayTransferRefAmount = result.FirstOrDefault().SumPayTransferRefAmount;
            }
            string sumPaymentAmountText = rs.sumPaymentAmount != 0 ? rs.sumPaymentAmount?.ToString("#,##0.00") : "0.00";
            string sumPayTransferRefAmountText = rs.sumPayTransferRefAmount != 0 ? rs.sumPayTransferRefAmount?.ToString("#,##0.00"): "0.00" ;
            var dt = new Dictionary<string, object>
            {
                {"sumPaymentAmount", sumPaymentAmountText},
                {"sumPayTransferRefAmount", sumPayTransferRefAmountText},
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClaimOnlineTranferReconcileMonitor(int? draw = null, int? pageSize = null, int? indexStart = null, DateTime? searchDete = null, string sortField = null, string orderType = null, string search = null)
        {
            Log.Debug($"{_controllerName} Start ClaimOnlineTranferReconcileMonitor SearchDete = {searchDete}", _controllerName, searchDete);
            var result = _context.usp_ClaimOnlineTransferReconcileMonitor_Select(indexStart, pageSize, searchDete, sortField, orderType, search).ToList();
            var rs = new ClaimOnlineTranferReconcileModel();
            if (result.Count > 0)
            {
                rs.countColPayAuto = result.FirstOrDefault().CountColPayauto != 0 ? result.FirstOrDefault().CountColPayauto : 0;
                rs.countPayTransferRef = result.FirstOrDefault().CountPayTransferRef != 0 ? result.FirstOrDefault().CountPayTransferRef : 0;

            }
            else
            {
                rs.countColPayAuto =  0;
                rs.countPayTransferRef = 0;
            }
            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()},
                {"CountColPayAuto", rs.countColPayAuto},
                {"CountPayTransferRef", rs.countPayTransferRef},
            };
            Log.Debug($"{_controllerName} End ClaimOnlineTranferReconcileMonitor SearchDete = {searchDete}", _controllerName, searchDete);
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExporTransferReconcileToExcel(DateTime searchDete)
        {
            Log.Debug($"{_controllerName} Start ExporTransferReconcileToExcel SearchDete = {searchDete}", _controllerName, searchDete);
            Session.Remove("DownloadExcel_FileFileTransferReconcileReport");
            try
            {
                Log.Debug($"{_controllerName} - ExporTransferReconcileToExcel Call Store procedure: usp_ClaimOnlineTransferReconcile_Select [searchDete = {searchDete}]");
                var data_sheet1 = _context.usp_ClaimOnlineTransferReconcileMonitor_Select(null, 9999, searchDete, null, null, null).Select(x => new
                {
                    ClaimOnLineId = x.ClaimOnLineId,
                    ClaimOnLineCode = x.ClaimOnLineCode == null ? "" : x.ClaimOnLineCode,
                    PaymentDate = x.PaymentDate != null ? x.PaymentDate.Value.ToString("dd/MM/yyyy") : null,
                    PaymentAmount = x.PaymentAmount,
                    PayListHeaderId = x.PayListHeaderId,
                    ClaimOnLineTransferId = x.ClaimOnLineTransferId,
                    จำนวนรายการClaimOnline = x.CountColPayauto,
                    RefCode = x.RefCode,
                    Ref1 = x.Ref1 == null ? "" : x.Ref1,
                    StatementCode = x.StatementCode == null ? "" : x.StatementCode,
                    UpdatedDate = x.UpdatedDate != null ? x.UpdatedDate.Value.ToString("dd/MM/yyyy") : null,
                    PayAutoTypeId = x.PayAutoTypeId,
                    PayTransferRefAmount = x.PayTransferRefAmount,
                    จำนวนรายการStatement = x.CountPayTransferRef

                }).ToList();
                if (data_sheet1.Count() != 0)
                {
                    var stream = new MemoryStream();
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet1 = package.Workbook.Worksheets.Add("Sheet1");
                        workSheet1.Cells.LoadFromCollection(data_sheet1, true);

                        // Select only the header cells
                        var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                        // Set their text to bold.
                        headerCells1.Style.Font.Bold = true;
                        // Set their background color
                        headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                        // Apply the auto-filter to all the columns
                        var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                        allCells1.AutoFilter = true;
                        // Auto-fit all the columns
                        allCells1.AutoFitColumns();

                        package.Save();

                        stream.Position = 0;
                        Session["DownloadExcel_FileFileTransferReconcileReport"] = package.GetAsByteArray();
                    }

                }
                else
                {
                    Log.Debug($"{_controllerName} End ExporTransferReconcileToExcel not found data SearchDete = {searchDete}", _controllerName, searchDete);
                    return Json(ResponseResult.Failure<string>(_dataNotFound), JsonRequestBehavior.AllowGet);
                }
                Log.Debug($"{_controllerName} End ExporTransferReconcileToExcel SearchDete = {searchDete}", _controllerName, searchDete);
                return Json(ResponseResult.Success<string>(_exportToExcelSuccessFul), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Session.Remove("DownloadExcel_FileFileTransferReconcileReport");
                Log.Error(ex, "{_controllerName} - Error: {Message} SearchDate = {searchDete}", _controllerName, ex.Message, searchDete);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DownloadTransferReconcile()
        {
            Log.Debug($"{_controllerName} Start DownloadTransferReconcile ", _controllerName);
            if (Session["DownloadExcel_FileFileTransferReconcileReport"] != null)
            {
                byte[] data = Session["DownloadExcel_FileFileTransferReconcileReport"] as byte[];
                string excelName = $"รายงานกระทบยอดการโอน-{DateTime.Now.ToString("ddMMyyyy-HHMM")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }
    }
}