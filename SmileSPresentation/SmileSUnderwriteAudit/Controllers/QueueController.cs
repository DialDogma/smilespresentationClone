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
    public class QueueController : Controller
    {
        #region Context

        private readonly UnderwriteAuditEntities _context;
        private readonly UnderwriteBranchV2Entities _context2;

        public QueueController()
        {
            _context = new UnderwriteAuditEntities();
            _context2 = new UnderwriteBranchV2Entities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            _context2.Dispose();
        }

        #endregion Context

        #region View



        public ActionResult QueueAuditInsureClose()

        {
            ViewBag.UserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.AuditInsureConsiderStatus = _context.usp_AuditInsureConsiderStatus_Select(null).ToList();

            var changePeriodDay = Properties.Settings.Default.ChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, numberOfMonthToShow);
            ViewBag.PeriodList = periodList;

            var Today = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            ViewBag.Today = Today.ToString("dd-MM-yyyy");

            var versionCheckDay = Convert.ToDateTime("2567-08-01");
            ViewBag.versionCheckDay = versionCheckDay;

            return View();
        }

        public ActionResult QueueCheckUnderwriteResult()
        {
            ViewBag.UserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            return View();
        }
        public ActionResult TeamManage()
        {
            ViewBag.UserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.PersonName = _context.usp_QCTeam_Select(null).ToList();
            return View();
        }


        [Authorization(Roles = "Developer,UnderwriteAudit_AdminBusinessPromoteTeam,UnderwriteAudit_BusinessPromoteTeam")]
        public ActionResult QueueManageAssign()
        {
            ViewBag.AreaManager = _context.usp_AreaManager_Select(null, null, 0, 999, null, null, null).ToList();
            ViewBag.Area = _context.usp_Area_Select(null, 0, 99999, null, null, null).ToList();
            ViewBag.QCUser = _context.usp_QCUserList_Select(2, "").ToList();
            ViewBag.Branch = _context.usp_BranchByAreaId_Select(null).ToList();

            return View();
        }

        [Authorization(Roles = "Developer,UnderwriteAudit_AdminBusinessPromoteTeam,UnderwriteAudit_BusinessPromoteTeam")]
        public ActionResult QueueCreate()
        {
            var periodlist = GlobalFunction.GetPeriodList();
            var period = Convert.ToDateTime(periodlist[1].Display); //PH โทรย้อนหลัง 1 เดือน

            var queCount = _context.usp_QueueAuditPendingCount_Select(period, null).Single();
            ViewBag.queCountAll = queCount.Total;
            ViewBag.CountIsBeware = queCount.CountIsBeware;
            ViewBag.CountIsNotBeware = queCount.CountIsNotBeware;

            return View();
        }

        [Authorization(Roles = "Developer,UnderwriteAudit_Insure")]
        public ActionResult QueueConsideringUnderwriteResult()
        {
            ViewBag.UserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.AuditInsureConsiderStatus = _context.usp_AuditInsureConsiderStatus_Select(null).ToList();
            return View();
        }

        #endregion View

        #region API
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
        public JsonResult GetMonitorCreateQueueHistory(string period = null, int? draw = null, int? pageSize = null, int? pageStart = null,
                                                   string sortField = null, string orderType = null, string search = null)
        {
            DateTime? c_startCoverdate = null;
            if (period != null && period != "")
            {
                c_startCoverdate = GlobalFunction.ConvertDatePickerToDate_BE(period);
            }

            var result = _context.usp_QueueAuditLot_Select(c_startCoverdate,
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

        public JsonResult GetCreateQueueDetail(int QueueAuditLotId, int? draw = null, int? pageSize = null, int? indexStart = null,
                                                 string sortField = null, string orderType = null)
        {
            var result = _context.usp_pvQueueQCUserV2_Select(QueueAuditLotId, indexStart, pageSize, sortField, orderType).ToList();

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


            var count = _context.usp_pvQueueQCUserV2_Select(queueAuditLotId, 0, 99999, null, null).ToList();

            return Json(count, JsonRequestBehavior.AllowGet);
        }

        [Authorization(Roles = "Developer,UnderwriteAudit_AdminBusinessPromoteTeam")]
        public ActionResult SaveCreateQueue(string dcrDate)
        {
            var resultArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            DateTime? c_dcr = null;
            if (dcrDate != null && dcrDate != "")
            {
                c_dcr = GlobalFunction.ConvertDatePickerToDate_BE(dcrDate);
            }
            var rs = _context.usp_QueueAuditLot_Insert_v4(c_dcr, userID).Single();

            resultArr[0] = rs.IsResult.Value.ToString();
            resultArr[1] = rs.Result;
            resultArr[2] = rs.Msg;

            return Json(resultArr, JsonRequestBehavior.AllowGet);
        }

        //public void ExportQueueReport(int queueAuditLotId)
        //{
        //    var data_sheet1 = _context.usp_QueueAuditLotReport_Select(queueAuditLotId).Select(s => new
        //    {
        //        DCR = s.Period,
        //        App_ID = s.ApplicationCode,
        //        ผู้เอาประกัน = s.CustomerName,
        //        ผู้ชำระเบี้ย = s.PayerName,
        //        สาขาผู้ชำระเบี้ยPayerbranch = s.PayerBranch,
        //        ภาคผู้ชำระเบี้ย = s.AreaDetail,
        //        รหัสพนักงาน_ฝ่ายส่งเสริม = s.EmployeeCode,
        //        ชื่อสกุล_ฝ่ายส่งเสริม = s.PersonName
        //    }).ToList();
        //    var dt1 = GlobalFunction.ToDataTable(data_sheet1);
        //    ExcelManager.ExportToExcel(this.HttpContext, "รายละเอียดการสร้างคิวงาน", dt1, "รายละเอียดการสร้างคิวงาน");
        //}

        public ActionResult underwriteExportCreateQueueReport(FormCollection form)
        {
            //await Task.Yield();
            using (var db = new UnderwriteAuditEntities())
            {
                int employeeID;
                employeeID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                var auLotId = Convert.ToInt32(form["queueAuditLotId"]);
                var data_sheet1 = _context.usp_QueueAuditLotReport_Select(auLotId).Select(s => new
                {
                    DCR = s.Period,
                    App_ID = s.ApplicationCode,
                    ผู้เอาประกัน = s.CustomerName,
                    ผู้ชำระเบี้ย = s.PayerName,
                    สาขาผู้ชำระเบี้ยPayerbranch = s.PayerBranch,
                    ภาคผู้ชำระเบี้ย = s.AreaDetail,
                    รหัสพนักงาน_ฝ่ายส่งเสริม = s.EmployeeCode,
                    ชื่อสกุล_ฝ่ายส่งเสริม = s.PersonName
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
                    headerCells1.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
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

        public ActionResult underwriteDownloadCreateQueueReport()
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

        //ui40
        public JsonResult GetQueueHealthAuditInsureClose(int? auditHealthStatusId,
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
            var result = _context.usp_QueueHealthAuditInsure_Select(
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

        public JsonResult GetQueueHealthAuditInsureComplete(int? auditHealthStatusId,
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
            var period = Convert.ToDateTime(c_period);

            var result = _context.usp_QueueHealthAuditInsure_Select(
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
                var result = _context.usp_QueueHealthAuditInsure_Select(
                    applicationCode,
                    insuredName,
                    payerName,
                    period,
                    null,
                    auditInsureStatusId,
                    auditInsureConsiderStatusIdList == "-1" ? "3,4" : auditInsureConsiderStatusIdList,
                    false,
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
        //CloseQueueByQueueAuditId
        public JsonResult QueueHealthAuditInsureCloseByQueueAuditId(int? queueAuditId)
        {
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _context.usp_QueueHealthAuditInsureCloseByQueueAuditId_Update(queueAuditId, userId).Single();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //CloseQueueAll
        [HttpPost]
        public ActionResult QueueHealthAuditInsureCloseALL(string[] queueAuditArray)
        {
            try
            {
                var queueAuditIdList = "";

                if (queueAuditArray != null)
                {
                    queueAuditIdList = string.Join(",", queueAuditArray);
                }
                int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                var result = _context.usp_QueueHealthAuditInsureCloseALL_Update(queueAuditIdList, userId).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }




        public JsonResult GetMonitorQueueUnderwrite(int? auditHealthStatusId,
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
                                              string auditInsureConsiderStatus)
        {

            DateTime? period = null;
            if (c_period != null && c_period != "")
            {
                period = GlobalFunction.ConvertDatePickerToDate_BE(c_period);
            }
            string auditInsureConsiderStatusIdList = null;
            if (auditInsureConsiderStatus == "-1")
            {
                auditInsureConsiderStatusIdList = "2,3,4";
            }
            else
            {
                auditInsureConsiderStatusIdList = auditInsureConsiderStatus;
            }
            // statusId รอพิจารณา = 2, พิจารณาแล้ว = 3
            var result = _context.usp_QueueHealthAuditInsure_Select(
                applicationCode == "" ? null : applicationCode.Trim(),
                insuredName == "" ? null : insuredName.Trim(),
                payerName == "" ? null : payerName.Trim(),
                period,
                auditHealthStatusId,
                auditInsureStatusId,
                (auditInsureStatusId == 2 ? null : auditInsureConsiderStatusIdList),
                null,
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





        [HttpGet]
        public ActionResult GetBranchByArea(int areaId)
        {
            var result = _context2.usp_BranchByAreaId_Select(areaId).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion API

        #region API_Miw

        public JsonResult getPeriodQueueAuditLot()
        {
            var result = _context.usp_QueueAuditLot_Select(null, null, null, "CreatedDate", "DESC").FirstOrDefault();
            if (result == null)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(result.Period, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataQueueAuditQueueAssignDetail(string applicationCode, int? branchId, string customerName, string payerName, string payerPhone, int? indexStart, int? pageSize, string sortField, string orderType, int? draw, int? QCUser)
        {
            var result = _context.usp_QueueAuditAssign_Select(applicationCode == "" ? null : applicationCode, (branchId == -1 ? null : branchId), (QCUser == -1 ? null : QCUser), customerName, payerName, payerPhone, indexStart, pageSize, sortField, orderType).ToList();

            var dt = new Dictionary<string, object>
               {
                   {"draw", draw },
                   {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                   {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                   {"data", result.ToList()}
               };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateAssignUserIdInQueueAudit(string[] queueAuditArray, int assignToUserId)
        {
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var queueAuditIdList = "";

            if (queueAuditArray != null)
            {
                queueAuditIdList = string.Join(",", queueAuditArray);
            }

            //var result = _context.usp_QueueAuditChange_Update(queueAuditIdList, assignToUserId, userId).Single();
            var result = _context.usp_QueueAuditListAssign_Update(queueAuditIdList, assignToUserId, userId).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateAssignUserIdInQueueAuditAll(string applicationCode, int? branchId, string customerName, string payerName, string payerPhone, int? QCUser)
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
        public JsonResult getdataQueueAudit(int? draw = null,
                                            int? pageSize = null,
                                            int? pageStart = null,
                                            string sortField = null,
                                            string orderType = null,
                                            string search = null,
                                            string applicationCode = null,
                                            string custFullName = null,
                                            string payerFullName = null,
                                            string appStartCoverDate = null,
                                            string queueStatusList = null)
        {
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            DateTime? c_startCoverdate = null;
            if (appStartCoverDate != null && appStartCoverDate != "")
            {
                c_startCoverdate = GlobalFunction.ConvertDatePickerToDate_BE(appStartCoverDate);
            }

            var result = _context.usp_QueueAuditResultClose_Select(applicationCode == "" ? null : applicationCode.Trim(),
                                                                   custFullName == "" ? null : custFullName.Trim(),
                                                                   payerFullName == "" ? null : payerFullName.Trim(),
                                                                   c_startCoverdate,
                                                                   userId,
                                                                   queueStatusList,
                                                                   pageStart,
                                                                   pageSize,
                                                                   sortField,
                                                                   orderType,
                                                                   search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult QueueHealthAuditV2_DT(int? draw = null,
                                    int? pageSize = null,
                                    int? pageStart = null,
                                    string sortField = null,
                                    string orderType = null,
                                    string applicationCode = null,
                                    string custFullName = null,
                                    string payerFullName = null,
                                    string appStartCoverDate = null,
                                    string queueStatusList = null)
        {
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            DateTime? c_startCoverdate = null;
            if (appStartCoverDate != null && appStartCoverDate != "")
            {
                c_startCoverdate = GlobalFunction.ConvertDatePickerToDate_BE(appStartCoverDate);
            }

            var result = _context.usp_QueueHealthAuditV2_Select(applicationCode == "" ? null : applicationCode.Trim(),
                                                                   custFullName == "" ? null : custFullName.Trim(),
                                                                   payerFullName == "" ? null : payerFullName.Trim(),
                                                                   c_startCoverdate,
                                                                   userId,
                                                                   queueStatusList,
                                                                   pageStart,
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

        public JsonResult updateCloseQueue(int queueAuditId)
        {
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _context.usp_QueueHealthAuditQCCloseByQueueAuditId_Update(queueAuditId, userId).Single();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult updateCloseQueueAll(string applicationCode = null, string custFullName = null, string payerFullName = null,
                                              string period = null, string queueStatusListId = null)
        {
            int userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            DateTime? c_period = null;

            if (period != null && period != "")
            {
                c_period = GlobalFunction.ConvertDatePickerToDate_BE(period);
            }

            var rs = _context.usp_QueueHealthAuditQCCloseALL_Update(applicationCode == "" ? null : applicationCode, custFullName == "" ? null : custFullName,
                                                                  payerFullName == "" ? null : payerFullName, c_period, userId, queueStatusListId, userId).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }


        #endregion API_Miw

        [HttpGet]
        public ActionResult GetQueueCount()
        {
            var periodlist = GlobalFunction.GetPeriodList();
            var period = Convert.ToDateTime(periodlist[1].Display); //PH โทรย้อนหลัง 1 เดือน

            var queCount = _context.usp_QueueAuditPendingCount_Select(period, null).Single();

            if (queCount == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(queCount, JsonRequestBehavior.AllowGet);
        }
    }
}