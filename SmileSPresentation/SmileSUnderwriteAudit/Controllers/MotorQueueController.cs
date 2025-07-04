using OfficeOpenXml;
using OfficeOpenXml.Style;
using SmileSUnderwriteAudit.Helper;
using SmileSUnderwriteAudit.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace SmileSUnderwriteAudit.Controllers
{
    [Authorization]
    public class MotorQueueController : Controller
    {
        #region View

        private readonly UnderwriteMotorAuditEntity _context;

        public MotorQueueController()
        {
            _context = new UnderwriteMotorAuditEntity();
        }

        // GET: MotorQueue
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MotorQueueCreate()
        {
            var changePeriodDay = Properties.Settings.Default.MotorChangePeriodDay;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, 1);
            ViewBag.PeriodList = periodList;
            var Today1 = periodList.FirstOrDefault();
            var period = Convert.ToDateTime(Today1.Display);

            var queCount = _context.usp_QueueMotorAuditPendingCount_Select(period, null).Single();
            ViewBag.queCountAll = queCount.Total;
            ViewBag.CountIsBeware = queCount.CountIsBeware;
            ViewBag.CountIsNotBeware = queCount.CountIsNotBeware;

            return View();
        }
        public ActionResult MortorTeamManage()
        {
            ViewBag.UserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.PersonName = _context.usp_QCTeam_Select(null).ToList();
            return View();
        }


        public ActionResult MotorQueueCheckUnderwriteResult()
        {
            ViewBag.UserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var changePeriodDay = Properties.Settings.Default.MotorChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, numberOfMonthToShow);
            ViewBag.PeriodList = periodList;

            var Today = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            ViewBag.Today = Today.ToString("dd-MM-yyyy");
            return View();
        }


        public ActionResult MotorQueueManageAssign()
        {
            ViewBag.AreaManager = _context.usp_AreaManager_Select(null, null, 0, 999, "Area_ID", null, null).ToList();
            ViewBag.Area = _context.usp_Area_Select(null, 0, 99999, null, null, null).ToList();
            ViewBag.QCUser = _context.usp_QCUserList_Select(4, "").ToList();
            ViewBag.Branch = _context.usp_BranchByAreaId_Select(null).ToList();
            return View();
        }

        public ActionResult MotorQueueConsideringUnderwriteResult()
        {
            ViewBag.UserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.AuditInsureConsiderStatus = _context.usp_AuditMotorInsureConsiderStatus_Select(null).ToList();

            var changePeriodDay = Properties.Settings.Default.MotorChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, numberOfMonthToShow);
            ViewBag.PeriodList = periodList;

            var Today = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            ViewBag.Today = Today.ToString("dd-MM-yyyy");
            return View();
        }

        // UI 40
        public ActionResult MotorQueueNotApproveFromWarranty()
        {
            ViewBag.UserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.AuditInsureConsiderStatus = _context.usp_AuditMotorInsureConsiderStatus_Select(null).ToList();

            var changePeriodDay = Properties.Settings.Default.MotorChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, numberOfMonthToShow);
            ViewBag.PeriodList = periodList;

            var Today = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            ViewBag.Today = Today.ToString("dd-MM-yyyy");
            return View();
        }

        // UI 45
        public ActionResult MotorQueueNotConsideringFromChairman()
        {
            var Branch_ID = _context.usp_BranchByAreaId_Select(null).ToList();
            ViewBag.Branch_ID = Branch_ID;

            var changePeriodDay = Properties.Settings.Default.MotorChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, numberOfMonthToShow);
            ViewBag.PeriodList = periodList;

            var Today = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            ViewBag.Today = Today.ToString("dd-MM-yyyy");
            return View();
        }

        #endregion View

        #region API

        #region จัดการทีม
        [HttpPost]
        public ActionResult GetTeamManageAssign(int? productGroupId, int? draw, int? indexStart, int? pageSize, string sortField, string orderType, string searchText)
        {
            try
            {

                var result = _context.usp_QCUser_Select(productGroupId, searchText.Trim(), indexStart, pageSize, sortField, orderType).ToList();

                var dt = new Dictionary<string, object>
                      {
                            {"draw", draw },
                            {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                            {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                            {"data", result.ToList()}
                        };
                return Json(dt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public JsonResult GetQCUserById(int? qCUserId)
        {
            var QCUserById = _context.usp_QCUserById_Select(qCUserId).Single();
            ViewBag.IsActive = QCUserById.IsActive;

            return Json(QCUserById, JsonRequestBehavior.AllowGet);
        }
        public JsonResult InsertChangeEmployee(int? productGroupId, int? userId)
        {
            int createdByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _context.usp_QCUser_Insert(productGroupId, userId, createdByUserId).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateChangeEmployee(int? qCUserId, bool isActive)
        {
            int updatedByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _context.usp_QCUserSetIsActiveById_Update(qCUserId, isActive, updatedByUserId).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMonitorMotorCreateQueueHistory(string period = null, int? draw = null, int? pageSize = null, int? pageStart = null,
                                                   string sortField = null, string orderType = null)
        {
            DateTime? c_startCoverdate = null;
            if (period != null && period != "")
            {
                c_startCoverdate = Convert.ToDateTime(period);
            }

            var result = _context.usp_QueueMotorAuditLot_Select(c_startCoverdate,
                                                            pageStart,
                                                            pageSize,
                                                            sortField,
                                                            orderType).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            //var dt = new Dictionary<string, object>();
            return Json(dt, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region สร้างคิวงาน
        public JsonResult GetCreateQueueDetail(int queueAuditLotId, int? draw = null, int? pageSize = null, int? indexStart = null,
                                           string sortField = null, string orderType = null)
        {

            var result = _context.usp_pvQueueMotorQCUserV2_Select(queueAuditLotId, indexStart, pageSize, sortField, orderType).ToList();

            var dt = new Dictionary<string, object>
             {
                 {"draw", draw },
                 {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                 {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                 {"data", result.ToList()}
             };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCount(int queueAuditLotId)
        {

            var count = _context.usp_pvQueueMotorQCUserV2_Select(queueAuditLotId, 0, 99999, null, null).ToList();

            return Json(count, JsonRequestBehavior.AllowGet);

        }
        public ActionResult SaveMotorCreateQueue(string dcrDate)
        {
            var resultArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            DateTime? dcr = null;
            if (dcrDate != null && dcrDate != "")
            {
                dcr = Convert.ToDateTime(dcrDate);
            }
            //var result = _context.usp_QueueMotorAuditLot_Insert_v5(dcr, userID);
            var result = _context.usp_QueueMotorAuditLot_Insert_v5(dcr, userID).Single();

            resultArr[0] = result.IsResult.Value.ToString();
            resultArr[1] = result.Result;
            resultArr[2] = result.Msg;

            return Json(resultArr, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region จัดการคิวงาน
        public JsonResult GetDataQueueAuditMotorQueueAssignDetail(string applicationCode, int? branchId, string customerName, string payerName, string payerPhone, int? indexStart, int? pageSize, string sortField, string orderType, int? draw, int? QCUser)
        {
            var result = _context.usp_QueueMotorAuditAssignV2_Select(applicationCode == "" ? null : applicationCode, (branchId == -1 ? null : branchId), (QCUser == -1 ? null : QCUser), customerName, payerName, payerPhone, indexStart, pageSize, sortField, orderType).ToList();


            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            //var dt = new Dictionary<string, object>();
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateAssignUserIdInQueueAuditMortor(string[] queueAuditArray, int assignToUserId)
        {
            var queueAuditIdList = "";

            if (queueAuditArray != null)
            {
                queueAuditIdList = string.Join(",", queueAuditArray);
            }
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _context.usp_QueueMotorAuditListAssignV2_Update(queueAuditIdList, assignToUserId, userId).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateAssignUserIdInQueueAuditMotorAll(string applicationCode, int? branchId, string customerName, string payerName, string payerPhone, int? QCUser)
        {
            try
            {
                var result = _context.usp_QueueAuditAssign_Select(applicationCode == "" ? null : applicationCode, (branchId == -1 ? null : branchId), (QCUser == -1 ? null : QCUser), customerName, payerName, payerPhone, 0, 999999, null, null).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        #endregion

        public ActionResult underwriteExportMotorCreateQueueReport(FormCollection form)
        {
            //await Task.Yield();
            using (var db = new UnderwriteAuditEntities())
            {
                int employeeID;
                employeeID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                var auLotId = Convert.ToInt32(form["queueAuditLotId"]);
                var data_sheet1 = _context.usp_QueueMotorAuditLotReport_Select(auLotId).Select(s => new
                {
                    วันเริ่มคุ้มครอง = (s.วันเริ่มคุ้มครอง != null ? s.วันเริ่มคุ้มครอง.Value.ToString("dd/MM/yyyy") : ""),
                    App_ID = s.App_ID,
                    ผู้เอาประกัน = s.ผู้เอาประกัน,
                    ผู้ชำระเบี้ย = s.ผู้ชำระเบี้ย,
                    สาขาผู้ชำระเบี้ยPayerbranch = s.สาขาผู้ชำระเบี้ยPayerbranch,
                    ภาคผู้ชำระเบี้ย = s.ภาคผู้ชำระเบี้ย,
                    รหัสพนักงาน_ฝ่ายQC = s.รหัสพนักงาน_ฝ่ายQC,
                    ชื่อสกุล_ฝ่ายQC = s.ชื่อสกุล_ฝ่ายQC
                }).ToList();

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
                    headerCells1.Style.Fill.BackgroundColor.SetColor(Color.Orchid);
                    // Apply the auto-filter to all the columns
                    var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                    allCells1.AutoFilter = true;
                    // Auto-fit all the columns
                    allCells1.AutoFitColumns();

                    package.Save();

                    stream.Position = 0;
                    Session["DownloadExcel_FileUnderwriteCreateQueueReport"] = package.GetAsByteArray();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult underwriteDownloadMotorCreateQueueReport()
        {
            if (Session["DownloadExcel_FileUnderwriteCreateQueueReport"] != null)
            {
                byte[] data = Session["DownloadExcel_FileUnderwriteCreateQueueReport"] as byte[];
                string excelName = $"รายละเอียดการสร้างคิวงาน-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        public JsonResult MotorgetdataQueueAudit(int? draw,
                                            int? pageSize,
                                            int? indexStart,
                                            string sortField,
                                            string orderType,
                                            string applicationCode,
                                            string insuredName,
                                            string payerName,
                                            string c_period,
                                            string queueStatusIdList)
        {
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            DateTime? period = null;
            if (c_period != null && c_period != "")
            {
                period = GlobalFunction.ConvertDatePickerToDate_BE(c_period);
            }

            var result = _context.usp_QueueMotorAudit_Select(
                applicationCode == "" ? null : applicationCode,
                insuredName == "" ? null : insuredName,
                payerName == "" ? null : payerName,
                period,
                 userId,
                 queueStatusIdList,
                 indexStart,
                 pageSize,
                 sortField,
                 orderType
               ).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }
        public JsonResult QueueMotorAuditV2(int? draw,
                                          int? pageSize,
                                          int? indexStart,
                                          string sortField,
                                          string orderType,
                                          string applicationCode,
                                          string insuredName,
                                          string payerName,
                                          string c_period,
                                          string queueStatusIdList)
        {
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            DateTime? period = null;
            if (c_period != null && c_period != "")
            {
                period = GlobalFunction.ConvertDatePickerToDate_BE(c_period);
            }

            var result = _context.usp_QueueMotorAuditV2_Select(
                applicationCode == "" ? null : applicationCode.Trim(),
                insuredName == "" ? null : insuredName.Trim(),
                payerName == "" ? null : payerName.Trim(),
                period,
                 userId,
                 queueStatusIdList,
                 indexStart,
                 pageSize,
                 sortField,
                 orderType
               ).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }



        public JsonResult MotorgetdataQueueAuditComplete(int? draw,
                                           int? pageSize,
                                           int? indexStart,
                                           string sortField,
                                           string orderType,
                                           string applicationCode,
                                           string insuredName,
                                           string payerName,
                                           string c_period,
                                           string queueStatusIdList)
        {
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            DateTime? period = null;
            if (c_period != null && c_period != "")
            {
                period = GlobalFunction.ConvertDatePickerToDate_BE(c_period);
            }

            var result = _context.usp_QueueMotorAudit_Select(
                applicationCode == "" ? null : applicationCode.Trim(),
                insuredName == "" ? null : insuredName.Trim(),
                payerName == "" ? null : payerName.Trim(),
                period,
                 userId,
                 queueStatusIdList,
                 indexStart,
                 pageSize,
                 sortField == "" ? "AuditMotorStatusDetail" : sortField,
                 orderType
               ).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult QueueMotorAuditV2Complete(int? draw,
                                         int? pageSize,
                                         int? indexStart,
                                         string sortField,
                                         string orderType,
                                         string applicationCode,
                                         string insuredName,
                                         string payerName,
                                         string c_period,
                                         string queueStatusIdList)
        {
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            DateTime? period = null;
            if (c_period != null && c_period != "")
            {
                period = GlobalFunction.ConvertDatePickerToDate_BE(c_period);
            }

            var result = _context.usp_QueueMotorAuditV2_Select(
                applicationCode == "" ? null : applicationCode.Trim(),
                insuredName == "" ? null : insuredName.Trim(),
                payerName == "" ? null : payerName.Trim(),
                period,
                userId,
                 queueStatusIdList,
                 indexStart,
                 pageSize,
                 sortField == "" ? "AuditMotorStatusDetail" : sortField,
                 orderType
               ).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult updateMotorCloseQueueAll(
            string applicationCode,
            string insuredName,
            string payerName,
            string period,
            string queueStatusListId)
        {
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            DateTime? c_period = null;

            if (period != null && period != "")
            {
                c_period = GlobalFunction.ConvertDatePickerToDate_BE(period);
            }

            var rs = _context.usp_QueueMotorAuditQCCloseALL_Update(
                 applicationCode == "" ? null : applicationCode.Trim(),
                 insuredName == "" ? null : insuredName.Trim(),
                 payerName == "" ? null : payerName.Trim(),
                 c_period,
                 userId,
                 queueStatusListId,
                 userId).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult updateMotorCloseQueue(int queueAuditId)
        {
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _context.usp_QueueMotorAuditQCCloseByQueueAuditId_Update(queueAuditId, userId).Single();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult GetBranchByArea(int areaId)
        {
            var result = _context.usp_BranchByAreaId_Select(areaId).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetMotorMonitorQueueUnderwrite(int? auditMotorStatusId,
                                                 int? auditInsureStatusId,
                                                 string applicationCode,
                                                 string insuredName,
                                                 string payerName,
                                                 string c_period,
                                                 int? draw,
                                                 int? pageSize,
                                                 int? indexStart,
                                                 string sortField,
                                                 string orderType,
                                                 string auditInsureConsiderStatusIdList)

        {
            DateTime? period = null;
            if (c_period != null && c_period != "")
            {
                period = GlobalFunction.ConvertDatePickerToDate_BE(c_period);
            }
            // statusId รอพิจารณา = 2, พิจารณาแล้ว = 3
            var result = _context.usp_QueueMotorAuditInsure_Select(
                applicationCode == "" ? null : applicationCode.Trim(),
                insuredName == "" ? null : insuredName.Trim(),
                payerName == "" ? null : payerName.Trim(),
                period,
                auditMotorStatusId,
                auditInsureStatusId,
                (auditInsureConsiderStatusIdList == "-1") ? null : auditInsureConsiderStatusIdList,
                indexStart,
                pageSize,
                sortField == "" ? "AuditInsureConsiderStatusDetail" : sortField,
                orderType).ToList();

            var dt = new Dictionary<string, object>
              {
                  {"draw", draw },
                  {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                  {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                  {"data", result.ToList()}
              };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult QueueMotorAuditInsureV2(int? auditMotorStatusId,
                                               int? auditInsureStatusId,
                                               string applicationCode,
                                               string insuredName,
                                               string payerName,
                                               string c_period,
                                               int? draw,
                                               int? pageSize,
                                               int? indexStart,
                                               string sortField,
                                               string orderType,
                                               string auditInsureConsiderStatusIdList)

        {
            DateTime? period = null;
            if (c_period != null && c_period != "")
            {
                period = GlobalFunction.ConvertDatePickerToDate_BE(c_period);
            }
            // statusId รอพิจารณา = 2, พิจารณาแล้ว = 3
            var result = _context.usp_QueueMotorAuditInsureV2_Select(
                applicationCode == "" ? null : applicationCode.Trim(),
                insuredName == "" ? null : insuredName.Trim(),
                payerName == "" ? null : payerName.Trim(),
                period,
                auditMotorStatusId,
                auditInsureStatusId,
                (auditInsureConsiderStatusIdList == "-1") ? null : auditInsureConsiderStatusIdList,
                false,
                indexStart,
                pageSize,
                sortField == "" ? "AuditInsureConsiderStatusDetail" : sortField,
                orderType).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult QueueMotorAuditInsureCompleteV2(int? auditMotorStatusId,
                                       int? auditInsureStatusId,
                                       string applicationCode,
                                       string insuredName,
                                       string payerName,
                                       string c_period,
                                       int? draw,
                                       int? pageSize,
                                       int? indexStart,
                                       string sortField,
                                       string orderType,
                                       string auditInsureConsiderStatusIdList)

        {
            DateTime? period = null;
            if (c_period != null && c_period != "")
            {
                period = GlobalFunction.ConvertDatePickerToDate_BE(c_period);
            }
            // statusId รอพิจารณา = 2, พิจารณาแล้ว = 3
            var result = _context.usp_QueueMotorAuditInsureV2_Select(
                applicationCode == "" ? null : applicationCode.Trim(),
                insuredName == "" ? null : insuredName.Trim(),
                payerName == "" ? null : payerName.Trim(),
                period,
                auditMotorStatusId,
                auditInsureStatusId,
                (auditInsureConsiderStatusIdList == "-1") ? null : auditInsureConsiderStatusIdList,
                true,
                indexStart,
                pageSize,
                sortField == "" ? "AuditInsureConsiderStatusDetail" : sortField,
                orderType).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        //ui40
        public JsonResult GetQueueMotorAuditInsureClose(int? auditMotorStatusId,
                                              int? auditInsureStatusId,
                                              string applicationCode,
                                              string insuredName,
                                              string payerName,
                                              string c_period,
                                              int? branchId,
                                              int? draw,
                                              int? pageSize,
                                              int? indexStart,
                                              string sortField,
                                              string orderType,
                                              string auditInsureConsiderStatusIdList,
                                              bool? insureClose)
        {
            DateTime? period = null;
            if (c_period != null && c_period != "")
            {
                period = GlobalFunction.ConvertDatePickerToDate_BE(c_period);
            }
            var result = _context.usp_QueueMotorAuditInsureClose_Select(
                applicationCode == "" ? null : applicationCode.Trim(),
                insuredName == "" ? null : insuredName.Trim(),
                payerName == "" ? null : payerName.Trim(),
                period,
                branchId,
                auditMotorStatusId,
                auditInsureStatusId,
                auditInsureConsiderStatusIdList == "-1" ? "3,4" : auditInsureConsiderStatusIdList,
                insureClose,
                indexStart,
                pageSize,
                sortField == "" ? "AuditInsureConsiderStatusDetail" : sortField,
                orderType
                ).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetQueueMotorAuditInsureCloseComplete(int? auditMotorStatusId,
                                              string applicationCode,
                                              string insuredName,
                                              string payerName,
                                              string c_period,
                                              int? branchId,
                                              int? draw,
                                              int? pageSize,
                                              int? indexStart,
                                              string sortField,
                                              string orderType,
                                              string auditInsureConsiderStatusIdList,
                                              bool? insureClose = true,
                                              int? auditInsureStatusId = 3)
        {
            var period = Convert.ToDateTime(c_period);

            var result = _context.usp_QueueMotorAuditInsureClose_Select(
                applicationCode == "" ? null : applicationCode.Trim(),
                insuredName == "" ? null : insuredName.Trim(),
                payerName == "" ? null : payerName.Trim(),
                period,
                branchId,
                auditMotorStatusId,
                auditInsureStatusId,
                auditInsureConsiderStatusIdList == "-1" ? "3,4" : auditInsureConsiderStatusIdList,
                insureClose,
                indexStart,
                pageSize,
                sortField == "" ? "AuditInsureConsiderStatusDetail" : sortField,
                orderType).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MotorCloseQueueAuditUpdate(int? queueAuditId)
        {
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _context.usp_QueueMotorAuditInsureCloseByQueueAuditId_Update(queueAuditId, userId).Single();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // ui40 CloseQueueAll
        [HttpPost]
        public ActionResult MotorCloseQueueAll(string[] queueAuditArray)
        {
            try
            {
                var queueAuditIdList = "";

                if (queueAuditArray != null)
                {
                    queueAuditIdList = string.Join(",", queueAuditArray);
                }
                int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                var result = _context.usp_QueueMotorAuditInsureCloseALL_Update(queueAuditIdList, userId).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpPost]
        public ActionResult GetCloseQueueAllDt(
                                              int? auditInsureStatusId,
                                              string applicationCode,
                                              string insuredName,
                                              string payerName,
                                              string c_period,
                                              string auditInsureConsiderStatusIdList,
                                              bool? insureClose)
        {
            try
            {
                DateTime? period = null;
                if (c_period != null && c_period != "")
                {
                    period = GlobalFunction.ConvertDatePickerToDate_BE(c_period);
                }
                var result = _context.usp_QueueMotorAuditInsureClose_Select(
                    applicationCode,
                    insuredName,
                    payerName,
                    period,
                    null,
                    null,
                    auditInsureStatusId,
                    auditInsureConsiderStatusIdList == "-1" ? "3,4" : auditInsureConsiderStatusIdList,
                    insureClose,
                    0,
                    9999999,
                    null,
                    null).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpPost]
        public ActionResult MotorChairmanCloseQueueAll(string[] queueAuditArray)
        {
            try
            {
                var queueAuditIdList = "";

                if (queueAuditArray != null)
                {
                    queueAuditIdList = string.Join(",", queueAuditArray);
                }
                int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                var result = _context.usp_QueueMotorAuditChairmanCloseALL_Update(queueAuditIdList, userId).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public JsonResult MotorChairmanCloseQueueAuditUpdate(int? queueAuditId)
        {
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var result = _context.usp_QueueMotorAuditChairmanCloseByQueueAuditId_Update(queueAuditId, userId).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetChairmanCloseQueueAllDt(
                                             string applicationCode,
                                                string insuredName,
                                                string payerName,
                                                string c_period,
                                                int? branchId,
                                                int? queueStatusId,
                                                bool? chairmanClose)
        {
            try
            {
                DateTime? period = null;
                if (c_period != null && c_period != "")
                {
                    period = GlobalFunction.ConvertDatePickerToDate_BE(c_period);
                }

                var result = _context.usp_QueueMotorAuditChairmanClose_Select(
                applicationCode,
                insuredName,
                payerName,
                period,
                branchId == -1 ? null : branchId,
                queueStatusId,
                chairmanClose,
                0,
                99999999,
                null,
                null).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        //ui47
        public JsonResult GetQueueMotorAuditChairmanClose(
                                                string applicationCode,
                                                string insuredName,
                                                string payerName,
                                                string c_period,
                                                int? branchId,
                                                int? queueStatusId,
                                                bool? chairmanClose,
                                                int? draw,
                                                int? pageSize,
                                                int? indexStart,
                                                string sortField,
                                                string orderType
                                                )

        {
            DateTime? period = null;
            if (c_period != null && c_period != "")
            {
                period = GlobalFunction.ConvertDatePickerToDate_BE(c_period);
            }

            var result = _context.usp_QueueMotorAuditChairmanClose_Select(
                applicationCode == "" ? null : applicationCode,
                insuredName == "" ? null : insuredName,
                payerName == "" ? null : payerName,
                period,
                branchId == -1 ? null : branchId,
                queueStatusId,
                chairmanClose,
                indexStart,
                pageSize,
                sortField,
                orderType).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getMotorPeriodQueueAuditLot()
        {
            var result = _context.usp_QueueMotorAuditLot_Select(null, null, null, "CreatedDate", "DESC").FirstOrDefault();
            if (result == null)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(result.Period, JsonRequestBehavior.AllowGet);
        }

        #endregion API

        [HttpGet]
        public ActionResult GetQueueCount()
        {
            var changePeriodDay = Properties.Settings.Default.MotorChangePeriodDay;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, 1);
            var Today1 = periodList.FirstOrDefault();
            ViewBag.Today = Today1.Display;
            var period = Convert.ToDateTime(Today1.Display);

            var queCount = _context.usp_QueueMotorAuditPendingCount_Select(period, null).Single();

            if (queCount == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(queCount, JsonRequestBehavior.AllowGet);
        }
    }
}