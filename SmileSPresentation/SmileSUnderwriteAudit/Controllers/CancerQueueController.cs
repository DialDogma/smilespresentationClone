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
    public class CancerQueueController : Controller
    {
        private readonly UnderwriteMotorAuditEntity _context2;
        private readonly UnderwriteCancerAuditEntities _context;
        public CancerQueueController()
        {
            _context = new UnderwriteCancerAuditEntities();
            _context2 = new UnderwriteMotorAuditEntity();
        }

        #region view

        // GET: CancerQueue
        public ActionResult CancerQueueIndex()
        {
            return View();
        }
        public ActionResult GetQueueCountCI()
        {
            var periodlist = GlobalFunction.GetPeriodList();
            var period = Convert.ToDateTime(periodlist[0].Display); //PH โทรย้อนหลัง 1 เดือน

            var queCount = _context.usp_QueueCIAuditPendingCount_Select(period, null).Single();

            if (queCount == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(queCount, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CancerQueueCreate()
        {

            var periodlist = GlobalFunction.GetPeriodList();
            var period = Convert.ToDateTime(periodlist[0].Display);

            var queCount = _context.usp_QueueCIAuditPendingCount_Select(period, null).Single();
            ViewBag.queCountAll = queCount.Total;
            ViewBag.CountIsBeware = queCount.CountIsBeware;
            ViewBag.CountIsNotBeware = queCount.CountIsNotBeware;

            return View();
        }

        public ActionResult CancerQueueCheckUnderwriteResult()
        {
            ViewBag.UserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var changePeriodDay = Properties.Settings.Default.CancerChangPeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, numberOfMonthToShow);
            ViewBag.PeriodList = periodList;

            var Today = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 6);
            ViewBag.Today = Today.ToString("dd-MM-yyyy");
            return View();
        }

        public ActionResult CancerQueueManageAssign()
        {
            ViewBag.AreaManager = _context2.usp_AreaManager_Select(null, null, 0, 999, "Area_ID", null, null).ToList();
            ViewBag.Area = _context2.usp_Area_Select(null, 0, 99999, null, null, null).ToList();
            ViewBag.QCUser = _context.usp_QCUserList_Select(10, "").ToList();
            ViewBag.Branch = _context.usp_BranchByAreaId_Select(null).ToList();
            return View();
        }
        public ActionResult CancerTeamManange()
        {
            ViewBag.UserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.PersonName = _context.usp_QCTeam_Select(null).ToList();
            return View();
        }
        public ActionResult CancerQueueConsideringUnderwriteResult()
        {
            ViewBag.UserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.AuditInsureConsiderStatus = _context.usp_AuditCIInsureConsiderStatus_Select(null).ToList();

            var changePeriodDay = Properties.Settings.Default.CancerChangPeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, numberOfMonthToShow);
            ViewBag.PeriodList = periodList;

            var Today = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            ViewBag.Today = Today.ToString("dd-MM-yyyy");
            return View();
        }


        public ActionResult CancerQueueAuditInsureClose()

        {
            ViewBag.UserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            ViewBag.AuditCIInsureConsiderStatus = _context.usp_AuditCIInsureConsiderStatus_Select(null).ToList();


            var changePeriodDay = Properties.Settings.Default.ChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, numberOfMonthToShow);
            ViewBag.PeriodList = periodList;

            var Today = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            ViewBag.Today = Today.ToString("dd-MM-yyyy");

            return View();
        }




        #endregion view
        #region CancerTeamManange
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
        #endregion

        #region QueueCreate

        public JsonResult GetMonitorCancerCreateQueueHistory(string period = null, int? draw = null, int? pageSize = null, int? pageStart = null,
                                                  string sortField = null, string orderType = null)
        {
            DateTime? c_startCoverdate = null;
            if (period != null && period != "")
            {
                c_startCoverdate = GlobalFunction.ConvertDatePickerToDate_BE(period);
            }

            var result = _context.usp_QueueCIAuditLot_Select(c_startCoverdate,
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

        [Authorization(Roles = "Developer,UnderwriteAudit_AdminBusinessPromoteTeam")]
        public ActionResult SaveCancerCreateQueue(string dcrDate)
        {
            var resultArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            DateTime? dcr = null;
            if (dcrDate != null && dcrDate != "")
            {
                dcr = GlobalFunction.ConvertDatePickerToDate_BE(dcrDate);
            }
            var result = _context.usp_QueueCIAuditLot_Insert_v4(dcr, userID).Single();

            resultArr[0] = result.IsResult.Value.ToString();
            resultArr[1] = result.Result;
            resultArr[2] = result.Msg;

            return Json(resultArr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult underwriteExportCancerCreateQueueReport(FormCollection form)
        {
            //await Task.Yield();
            using (var db = new UnderwriteAuditEntities())
            {
                int employeeID;
                employeeID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                var auLotId = Convert.ToInt32(form["queueAuditLotId"]);
                var data_sheet1 = _context.usp_QueueCIAuditLotReport_Select(auLotId).Select(s => new
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

        public ActionResult underwriteDownloadCancerCreateQueueReport()
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



        public JsonResult GetCreateQueueDetail(int queueAuditLotId, int? draw = null, int? pageSize = null, int? indexStart = null,
                                              string sortField = null, string orderType = null)
        {

            var result = _context.usp_pvQueueCIQCUserV2_Select(queueAuditLotId, indexStart, pageSize, sortField, orderType).ToList();

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


            var count = _context.usp_pvQueueCIQCUserV2_Select(queueAuditLotId, 0, 99999, null, null).ToList();

            return Json(count, JsonRequestBehavior.AllowGet);
        }

        #endregion QueueCreate(ui24)

        #region QueueCheckUnderwriteResult(ui25-30)

        public JsonResult CancergetdataQueueAudit(int? draw,
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

            var result = _context.usp_QueueCIAuditV2_Select(
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


        public JsonResult GetQueueCIAuditInsureClose(int? auditHealthStatusId,
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
                                              string auditInsureConsiderStatusIdList, bool? auditInsureClose)
        {
            DateTime? period = null;
            if (c_period != null && c_period != "")
            {
                period = GlobalFunction.ConvertDatePickerToDate_BE(c_period);
            }
            var result = _context.usp_QueueCIAuditInsureV2_Select(
                applicationCode == "" ? null : applicationCode.Trim(),
                insuredName == "" ? null : insuredName.Trim(),
                payerName == "" ? null : payerName.Trim(),
                period,
                auditHealthStatusId,
                auditInsureStatusId,
                auditInsureConsiderStatusIdList == "-1" ? "3,4" : auditInsureConsiderStatusIdList,
                auditInsureClose,
                indexStart,
                pageSize,
                    sortField == "" ? "IsBeware" : sortField,
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

        public JsonResult GetQueueCIAuditInsureComplete(int? auditHealthStatusId,
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
            var period = Convert.ToDateTime(c_period);

            var result = _context.usp_QueueCIAuditInsureV2_Select(
                applicationCode == "" ? null : applicationCode.Trim(),
                insuredName == "" ? null : insuredName.Trim(),
                payerName == "" ? null : payerName.Trim(),
                period,
                auditHealthStatusId,
                3,
                auditInsureConsiderStatusIdList == "-1" ? "3,4" : auditInsureConsiderStatusIdList,
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



        //CloseQueueAll
        [HttpPost]
        public ActionResult QueueCIAuditInsureCloseALL(string[] queueAuditArray)
        {
            try
            {
                var queueAuditIdList = "";

                if (queueAuditArray != null)
                {
                    queueAuditIdList = string.Join(",", queueAuditArray);
                }
                int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                var result = _context.usp_QueueCIAuditInsureCloseALL_Update(queueAuditIdList, userId).ToList();
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
                                       string auditInsureConsiderStatusIdList)
        {
            try
            {
                DateTime? period = null;
                if (c_period != null && c_period != "")
                {
                    period = GlobalFunction.ConvertDatePickerToDate_BE(c_period);
                }
                var result = _context.usp_QueueCIAuditInsure_Select(
                    applicationCode,
                    insuredName,
                    payerName,
                    period,
                    null,
                    auditInsureStatusId,
                    auditInsureConsiderStatusIdList == "-1" ? "3,4" : auditInsureConsiderStatusIdList,
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


        public JsonResult QueueCIhAuditInsureCloseByQueueAuditId(int? queueAuditId)
        {
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _context.usp_QueueCIAuditInsureCloseByQueueAuditId_Update(queueAuditId, userId).Single();
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public JsonResult CancergetdataQueueAuditComplete(int? draw,
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

            var result = _context.usp_QueueCIAuditV2_Select(
                applicationCode == "" ? null : applicationCode.Trim(),
                insuredName == "" ? null : insuredName.Trim(),
                payerName == "" ? null : payerName.Trim(),
                period,
                 userId,
                 queueStatusIdList,
                 indexStart,
                 pageSize,
                 sortField == "" ? "IsBeware" : sortField,
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

        public JsonResult updateCancerCloseQueueAll(
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

            var rs = _context.usp_QueueCIAuditQCCloseALL_Update(
                 applicationCode == "" ? null : applicationCode.Trim(),
                 insuredName == "" ? null : insuredName.Trim(),
                 payerName == "" ? null : payerName.Trim(),
                 c_period,
                 userId,
                 queueStatusListId,
                 userId).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult updateCancerCloseQueue(int queueAuditId)
        {
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _context.usp_QueueCIAuditQCCloseByQueueAuditId_Update(queueAuditId, userId).Single();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getCancerPeriodQueueAuditLot()
        {
            var result = _context.usp_QueueCIAuditLot_Select(null, null, null, "CreatedDate", "DESC").FirstOrDefault();
            if (result == null)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(result.Period, JsonRequestBehavior.AllowGet);
        }

        #endregion QueueCheckUnderwriteResult(ui25-30)

        #region QueueManageAssign

        public JsonResult GetDataQueueAuditCancerQueueAssignDetail(string applicationCode, int? branchId, string customerName, string payerName, string payerPhone, int? indexStart, int? pageSize, string sortField, string orderType, int? draw, int? QCUser)
        {
            var result = _context.usp_QueueCIAuditAssignV2_Select(applicationCode == "" ? null : applicationCode, (branchId == -1 ? null : branchId), (QCUser == -1 ? null : QCUser), customerName, payerName, payerPhone, indexStart, pageSize, sortField, orderType).ToList();

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

        public JsonResult UpdateAssignUserIdInQueueAuditCancer(string[] queueAuditArray, int assignToUserId)
        {
            var queueAuditIdList = "";

            if (queueAuditArray != null)
            {
                queueAuditIdList = string.Join(",", queueAuditArray);
            }
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _context.usp_QueueCIAuditListAssignV2_Update(queueAuditIdList, assignToUserId, userId).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateAssignUserIdInQueueAuditCancerAll(string applicationCode, int? branchId, string customerName, string payerName, string payerPhone, int? QCUser)
        {
            try
            {
                var result = _context.usp_QueueCIAuditAssignV2_Select(applicationCode == "" ? null : applicationCode, (branchId == -1 ? null : branchId), (QCUser == -1 ? null : QCUser), customerName, payerName, payerPhone, 0, 999999, null, null).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        #endregion QueueManageAssign(ui33)

        #region QueueConsidering(ui34)

        public JsonResult GetCancerMonitorQueueUnderwrite(int? auditMotorStatusId,
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
            var result = _context.usp_QueueCIAuditInsureV2_Select(
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

        #endregion QueueConsidering(ui34)

        #region QueueConsidering(ui37)

        public JsonResult GetCancerMonitorQueueUnderwriteComplete(int? auditMotorStatusId,
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
            var result = _context.usp_QueueCIAuditInsureV2_Select(
                applicationCode == "" ? null : applicationCode,
                insuredName == "" ? null : insuredName,
                payerName == "" ? null : payerName,
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

        #endregion QueueConsidering(ui37)

        #region common API

        [HttpGet]
        public ActionResult GetBranchByArea(int areaId)
        {
            var result = _context2.usp_BranchByAreaId_Select(areaId).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getMotorPeriodQueueAuditLot()
        {
            var result = _context.usp_QueueCIAuditLot_Select(null, null, null, "CreatedDate", "DESC").FirstOrDefault();
            if (result == null)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(result.Period, JsonRequestBehavior.AllowGet);
        }

        #endregion common API
    }
}