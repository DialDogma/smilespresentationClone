using OfficeOpenXml;
using OfficeOpenXml.Style;
using SmileUnderwriteBranchV2.Helper;
using SmileUnderwriteBranchV2.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Color = System.Drawing.Color;

namespace SmileUnderwriteBranchV2.Controllers
{
    [Authorization]
    public class MotorQueueController : Controller
    {
        private readonly UnderwriteBranchV2MotorModelContainer _contextMotor;
        private readonly UnderwriteBranchV2Entities _contextPH;

        public MotorQueueController()
        {
            _contextMotor = new UnderwriteBranchV2MotorModelContainer();
            _contextPH = new UnderwriteBranchV2Entities();
        }

        #region View

        // GET: MotorQueue
        public ActionResult Index()
        {
            return View();
        }

        //UI1
        [Authorization(Roles = "Developer,UnderwriteV2_Director")]
        public ActionResult MotorQueueUnderwritePending()
        {
            //DCR
            var MortorChangeStartCoverDate = Properties.Settings.Default.MortorChangeStartCoverDate;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.MotorGetPeriodList(MortorChangeStartCoverDate, 4);
            ViewBag.PeriodList = periodList;
            ViewBag.PeriodListForCheckDCR = periodList[0].Display;

            var MortorCloseDate = Properties.Settings.Default.MotorCloseDate;
            ViewBag.MortorCloseDate = MortorCloseDate;




            ViewBag.UserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            return View();
        }

        /// รอผลพิจารณา UI13/ UI15
        public ActionResult MotorQueueWaitConsider(string menu, string dcr, int? branchId)
        {
            ViewBag.menu = menu;
            ViewBag.branchId = branchId;
            ViewBag.UserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

            var cDCR = DateTime.ParseExact(dcr, "yyyyMMdd", new CultureInfo("th-TH"));
            ViewBag.DCR = cDCR.ToString("yyyy-MM-dd");
            return View();
        }

        public ActionResult MotorQueueCancelBeforeDCR(string dcr, int? branchId)
        {
            ViewBag.branchId = branchId;
            ViewBag.UserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

            var cDCR = DateTime.ParseExact(dcr, "yyyyMMdd", new CultureInfo("th-TH"));
            ViewBag.DCR = cDCR.ToString("yyyy-MM-dd");

            return View();
        }

        [Authorization(Roles = "Developer,UnderwriteV2_BusinesPromoteTeam")]
        public ActionResult MotorQueueUnderwriteOverdue()
        {
            ViewBag.UserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            var branchList = _contextPH.usp_Branch_Select(null).ToList();
            ViewBag.branchList = branchList;
            return View();
        }

        [Authorization(Roles = "Developer,UnderwriteV2_Director")]
        public ActionResult MotorQueueAssign()
        {
            ViewBag.UserDetail = GlobalFunction.GetLoginDetail(HttpContext);
            var userDetail = GlobalFunction.GetLoginDetail(HttpContext);
            var provinceList = _contextPH.usp_ProvinceByChairmanUserId_Select(userDetail.User_ID).ToList();
            var directorList = _contextPH.usp_DirectorByBranchId_Select(userDetail.Branch_ID).ToList();
            ViewBag.directorList = directorList;
            ViewBag.province = provinceList;
            return View();
        }


        // [Authorization(Roles = "Developer,UnderwriteV2_Director")]
        public ActionResult MotorQueueAdminAssign()
        {
            var MortorChangeStartCoverDate = Properties.Settings.Default.MortorChangeStartCoverDate;
            var periodList = GlobalFunction.MotorGetPeriodList(MortorChangeStartCoverDate, 13);
            ViewBag.PeriodList = periodList;
            ViewBag.UserDetail = GlobalFunction.GetLoginDetail(HttpContext);
            var userDetail = GlobalFunction.GetLoginDetail(HttpContext);
            var provinceList = _contextPH.usp_ProvinceByChairmanUserId_Select(userDetail.User_ID).ToList();
            var directorList = _contextPH.usp_DirectorByBranchId_Select(null).ToList();
            ViewBag.directorList = directorList;
            ViewBag.province = provinceList;
            return View();
        }

        public ActionResult GetMotorQueueUnderwrite()
        {
            ViewBag.UserDetail = GlobalFunction.GetLoginDetail(HttpContext);
            return View();
        }

        public ActionResult EditMotorQueueUnderwrite()
        {
            ViewBag.UserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            ViewBag.UserDetail = GlobalFunction.GetLoginDetail(HttpContext);

            var MotorCloseDate = Properties.Settings.Default.MotorCloseDate;
            ViewBag.MotorCloseDate = MotorCloseDate;

            var MotorChangePeriodDay = Properties.Settings.Default.MotorChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.GetPeriodList(MotorChangePeriodDay, numberOfMonthToShow);
            ViewBag.PeriodList = periodList;
            ViewBag.PeriodListForCheckDCR = periodList[0].Display;

            return View();
        }

        #endregion View

        #region API

        //รอพิจารณา Tab1
        [HttpPost]
        public ActionResult MotorQueueWaitConsider_dt(
                       int? draw,
                       string dcr,
                       int? payerBranchId,
                       string searchDetail,
                       int? indexStart,
                       int? pageSize,
                       string sortField,
                       string orderType)
        {
            try
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext);
                var cDCR = Convert.ToDateTime(dcr);
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');

                if (roleList.Contains("Developer"))
                {
                    var result = _contextMotor.usp_QueueMotorApprovePending_Select(
                        cDCR,
                        (payerBranchId != null ? payerBranchId : user.Branch_ID),
                        null,
                        null,
                        null,
                        searchDetail,
                        indexStart,
                        pageSize,
                        sortField,
                        orderType).ToList();

                    var dt = new Dictionary<string, object>
                        {
                            {"draw", draw },
                            {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                            {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                            {"data", result.ToList()}
                        };
                    return Json(dt, JsonRequestBehavior.AllowGet);
                }
                else if (roleList.Contains("UnderwriteV2_Chairman"))
                {
                    var result = _contextMotor.usp_QueueMotorApprovePending_Select(
                        cDCR,
                        (payerBranchId != null ? payerBranchId : user.Branch_ID),
                        null,
                        null,
                        null,
                        searchDetail,
                        indexStart,
                        pageSize,
                        sortField,
                        orderType).ToList();

                    var dt = new Dictionary<string, object>
                    {
                        {"draw", draw },
                        {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                        {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                        {"data", result.ToList()}
                    };
                    return Json(dt, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = _contextMotor.usp_QueueMotorApprovePending_Select(
                        cDCR,
                        (payerBranchId != null ? payerBranchId : user.Branch_ID),
                        null,
                        null,
                        null,
                        searchDetail,
                        indexStart,
                        pageSize,
                        sortField,
                        orderType).ToList();

                    var dt = new Dictionary<string, object>
                    {
                        {"draw", draw },
                        {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                        {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                        {"data", result.ToList()}
                    };
                    return Json(dt, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        // พิจารนาแล้ว Tab2
        [HttpPost]
        public ActionResult MotorQueueConsiderComplete_dt(
                       int? draw,
                       string dcr,
                       int? payerBranchId,
                       string searchDetail,
                       int? indexStart,
                       int? pageSize,
                       string sortField,
                       string orderType)
        {
            try
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext);
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                var cDCR = Convert.ToDateTime(dcr);
                if (roleList.Contains("Developer"))
                {
                    var result = _contextMotor.usp_QueueMotorApproveComplete_Select(
                        cDCR,
                        (payerBranchId != null ? payerBranchId : user.Branch_ID),
                        null,
                        null,
                        null,
                        searchDetail,
                        indexStart,
                        pageSize,
                        sortField,
                        orderType).ToList();

                    var dt = new Dictionary<string, object>
                    {
                        {"draw", draw },
                        {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                        {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                        {"data", result.ToList()}
                    };
                    return Json(dt, JsonRequestBehavior.AllowGet);
                }
                else if (roleList.Contains("UnderwriteV2_Chairman"))
                {
                    var result = _contextMotor.usp_QueueMotorApproveComplete_Select(
                         cDCR,
                        (payerBranchId != null ? payerBranchId : user.Branch_ID),
                        null,
                        null,
                        null,
                        searchDetail,
                        indexStart,
                        pageSize,
                        sortField,
                        orderType).ToList();

                    var dt = new Dictionary<string, object>
                    {
                        {"draw", draw },
                        {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                        {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                        {"data", result.ToList()}
                    };
                    return Json(dt, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = _contextMotor.usp_QueueMotorApproveComplete_Select(
                        cDCR,
                        (payerBranchId != null ? payerBranchId : user.Branch_ID),
                        null,
                        null,
                        null,
                        searchDetail,
                        indexStart,
                        pageSize,
                        sortField,
                        orderType).ToList();

                    var dt = new Dictionary<string, object>
                    {
                        {"draw", draw },
                        {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                        {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                        {"data", result.ToList()}
                    };
                    return Json(dt, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        // คิวงานยกเลิกก่อน DCR (*ของเก่า*)
        [HttpGet]
        public ActionResult MotorQueueByQueueStatus_dt(int? draw, int? indexStart, int? pageSize, string sortField, string orderType, string searchDetail, string dcr, int? payerBranchId)
        {
            try
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext);
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                var cDCR = Convert.ToDateTime(dcr);
                if (roleList.Contains("Developer"))
                {
                    var result = _contextMotor.usp_QueueMotorCancelBeforeDCR_Select(
                        cDCR,
                        (payerBranchId != null ? payerBranchId : user.Branch_ID),
                        null,
                        null,
                        null,
                        searchDetail,
                        indexStart,
                        pageSize,
                        sortField,
                        orderType).ToList();

                    var dt = new Dictionary<string, object>
                    {
                        {"draw", draw },
                        {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                        {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                        {"data", result.ToList()}
                    };
                    return Json(dt, JsonRequestBehavior.AllowGet);
                }
                else if (roleList.Contains("UnderwriteV2_Chairman"))
                {
                    var result = _contextMotor.usp_QueueMotorCancelBeforeDCR_Select(
                          cDCR,
                        (payerBranchId != null ? payerBranchId : user.Branch_ID),
                        null,
                        null,
                        null,
                        searchDetail,
                        indexStart,
                        pageSize,
                        sortField,
                        orderType).ToList();

                    var dt = new Dictionary<string, object>
                    {
                        {"draw", draw },
                        {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                        {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                        {"data", result.ToList()}
                    };
                    return Json(dt, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = _contextMotor.usp_QueueMotorCancelBeforeDCR_Select(
                        cDCR,
                        (payerBranchId != null ? payerBranchId : user.Branch_ID),
                        null,
                        null,
                        null,
                        searchDetail,
                        indexStart,
                        pageSize,
                        sortField,
                        orderType).ToList();

                    var dt = new Dictionary<string, object>
                    {
                        {"draw", draw },
                        {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                        {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                        {"data", result.ToList()}
                    };
                    return Json(dt, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }


        //รายละเอียดคิวงานที่ยังไม่ได้คัดกรอง
        [HttpPost]
        [Authorization(Roles = "Developer,UnderwriteV2_Director")]
        public ActionResult MotorQueueUnderwritePendingDT(string startCoverDate,
                                                    int? playerBranchId,
                                                    int? payerDistricId,
                                                    int? payerStudyAreaId,
                                                    string applicationCode,
                                                    string insuredName,
                                                    string payerName,
                                                    int? draw,
                                                    int? indexStart,
                                                    int? pageSize,
                                                    string sortField,
                                                    string orderType)
        {
            try
            {
                DateTime? nStartCoverdate = null;
                if (startCoverDate != "-1" && startCoverDate != null)
                {
                    nStartCoverdate = Convert.ToDateTime(startCoverDate);
                }
                var user = GlobalFunction.GetLoginDetail(HttpContext);
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                //var coverDate = Convert.ToDateTime(startCoverDate);

                var result = _contextMotor.usp_QueueMotorUnderwritePending_Select(nStartCoverdate, playerBranchId, payerDistricId, payerStudyAreaId, Convert.ToInt32(user.User_ID), applicationCode, insuredName, payerName, indexStart, pageSize, sortField, orderType).ToList();

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
        public ActionResult DownloadMotorQueueUnderwritePendingDTReport(string startCoverDate,
                                                    int? playerBranchId,
                                                    int? payerDistricId,
                                                    int? payerStudyAreaId,
                                                    string applicationCode,
                                                    string insuredName,
                                                    string payerName,
                                                    int? draw,
                                                    int? indexStart,
                                                    int? pageSize,
                                                    string sortField,
                                                    string orderType)
        {
            //await Task.Yield();
            using (var db = new UnderwriteBranchV2Entities())
            {
                var stream = new MemoryStream();
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                DateTime? nStartCoverdate = null;
                if (startCoverDate != "-1" && startCoverDate != null)
                {
                    nStartCoverdate = Convert.ToDateTime(startCoverDate);
                }
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet1 = package.Workbook.Worksheets.Add("คิวงานยังไม่ได้มอบกรมธรรม์ประกันรถยนต์");
                    {
                        var data_sheet1 = _contextMotor.usp_QueueMotorUnderwritePending_Select(nStartCoverdate, playerBranchId, payerDistricId, payerStudyAreaId, Convert.ToInt32(userId), applicationCode, insuredName, payerName, 0, 9999999, sortField, orderType).Select(o => new
                        {
                            เลข_App = o.ApplicationCode,
                            ชื่อผู้เอาประกัน = o.InsuredName,
                            ชื่อผู้ชำระเบี้ย = o.PayerName,
                            ชื่อหน่วยงาน = o.PayerOfficeName,
                            ตำบล = o.SubDistrictDetail,
                            อำเภอ = o.DistrictDetail,
                            จังหวัด = o.ProvinceDetail,
                            วันครบกำหนดคิวงาน7วัน = (o.QueueChallengeDate != null ? o.QueueChallengeDate.Value.ToString("dd/MM/yyyy HH:mm", new CultureInfo("th-TH")) : ""),

                            วันครบกำหนดคิวงาน = (o.QueueExpireDate != null ? o.QueueExpireDate.Value.ToString("dd/MM/yyyy HH:mm", new CultureInfo("th-TH")) : ""),
                            แผน = o.Premium,
                            วันเริ่มคุ้มครอง = (o.StartCoverDate != null ? o.StartCoverDate.Value.ToString("dd/MM/yyyy", new CultureInfo("th-TH")) : ""),

                        }).ToList();

                        workSheet1.Cells.LoadFromCollection(data_sheet1, true);
                        // Select only the header cells
                        var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];

                        // Set their text to bold.
                        headerCells1.Style.Font.Bold = true;

                        // Set their background color
                        headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        headerCells1.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

                        // Apply the auto-filter to all the columns
                        var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];

                        allCells1.AutoFilter = true;

                        // Auto-fit all the columns
                        allCells1.AutoFitColumns();
                    }

                    package.Save();

                    stream.Position = 0;
                    Session["DownloadExcel_FileUnderwriteQueueReport"] = package.GetAsByteArray();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
        }
        public ActionResult Download(string reportName)
        {
            if (Session["DownloadExcel_FileUnderwriteQueueReport"] != null)
            {
                byte[] data = Session["DownloadExcel_FileUnderwriteQueueReport"] as byte[];
                string excelName = $"{reportName}-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }
        //รายละเอียดคิวงานที่คัดกรองแล้ว
        [HttpPost]
        [Authorization(Roles = "Developer,UnderwriteV2_Director")]
        public ActionResult QueueMotorUnderwritePendingComplete_DT(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string startCoverDate, string searchDetail1, string searchDetail2, string applicationCode)
        {
            try
            {
                DateTime? nStartCoverdate = null;
                if (startCoverDate != "-1" && startCoverDate != null)
                {
                    nStartCoverdate = Convert.ToDateTime(startCoverDate);
                }
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _contextMotor.usp_QueueMotorUnderwriteComplete_Select(applicationCode.Trim(), searchDetail1.Trim(), searchDetail2.Trim(), nStartCoverdate, userId, indexStart, pageSize, sortField, orderType).ToList();

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

        //UI ยกเลิกก่อน DCR
        [HttpPost]
        [Authorization(Roles = "Developer,UnderwriteV2_Director")]
        public ActionResult QueueMotorUnderwriteCancelBeforeDCR_DT(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string startCoverDate, string searchDetail1, string searchDetail2, string applicationCode)
        {
            try
            {
                DateTime? nStartCoverdate = null;
                if (startCoverDate != "-1" && startCoverDate != null)
                {
                    nStartCoverdate = Convert.ToDateTime(startCoverDate);
                }
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _contextMotor.usp_QueueMotorUnderwriteCancelBeforeDCR_Select(applicationCode.Trim(), searchDetail1.Trim(), searchDetail2.Trim(), nStartCoverdate, userId, indexStart, pageSize, sortField, orderType).ToList();

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
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpPost]
        public ActionResult QueueMotorAssign_DT(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string applicationCode, string insuredName, string payerName, int? isAcceptQueue)
        {
            try
            {
                bool? IsAcceptQueue_val = null;
                if (isAcceptQueue != -1)
                    if (isAcceptQueue == 1)
                    {
                        IsAcceptQueue_val = true;
                    }
                    else
                    {
                        IsAcceptQueue_val = false;
                    }
                {
                }
                string appCode = null;
                if (applicationCode != "")
                {
                    appCode = applicationCode.Trim();
                }

                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var user = GlobalFunction.GetLoginDetail(HttpContext);
                var roleListRaw = OAuth2Helper.GetRoles();
                var devRole = roleListRaw.Contains("Developer");
                var result = _contextMotor.usp_QueueMotorAssignV2_Select(appCode, insuredName.Trim(), payerName.Trim(), userId, IsAcceptQueue_val, indexStart, pageSize, sortField, orderType).ToList();
                if (devRole)
                {
                    result = _contextMotor.usp_QueueMotorAssignV2_Select(appCode, insuredName.Trim(), payerName.Trim(), null, IsAcceptQueue_val, indexStart, pageSize, sortField, orderType).ToList();

                }
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
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpPost]
        public ActionResult QueueMotorAdminAssign_DT(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string applicationCode, string cutomerName, string payerName, string StartCoverDate, int? payerSubDistrictId, int? payerDistrictId, int? payerProvinceId, int? assignToUserId)
        {
            try
            {
                string appCode = null;
                if (applicationCode != "")
                {
                    appCode = applicationCode.Trim();
                }
                string C_cutomerName = null;
                if (cutomerName != "")
                {
                    C_cutomerName = cutomerName.Trim();
                }
                string P_payerName = null;
                if (payerName != "")
                {
                    P_payerName = payerName.Trim();
                }
                var appStartCoverDate = Convert.ToDateTime(StartCoverDate);

                var result = _contextMotor.usp_QueueMotorAdminAssign_Select(appStartCoverDate, appCode, C_cutomerName, P_payerName, (payerSubDistrictId == -1 ? null : payerSubDistrictId), (payerDistrictId == -1 ? null : payerDistrictId), (payerProvinceId == -1 ? null : payerProvinceId), (assignToUserId == -1 ? null : assignToUserId), indexStart, pageSize, sortField, orderType, null).ToList();

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
                throw new Exception(e.Message, e.InnerException);
            }
        }

        //รับทราบคิวงาน
        [HttpPost]
        public ActionResult MotorQueueAccept_DT(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string applicationCode, string insuredName, string payerName)
        {
            try
            {
                string appCode = null;
                if (applicationCode != "")
                {
                    appCode = applicationCode.Trim();
                }
                string n_insuredName = null;

                if (insuredName == "")
                {
                    n_insuredName = null;
                }
                else
                {
                    n_insuredName = insuredName.Trim();
                }

                string n_payerName = null;

                if (payerName == "")
                {
                    n_payerName = null;
                }
                else
                {
                    n_payerName = payerName.Trim();
                }
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                var result = _contextMotor.usp_QueueMotorAccept_Select(appCode, n_insuredName, n_payerName, userId, indexStart, pageSize, sortField, orderType).ToList();

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
                throw new Exception(e.Message, e.InnerException);
            }
        }

        /// <summary>
        /// QueueListAccept Update
        /// </summary>
        /// <param name="queueArray"></param>
        /// <param name="assignTo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MotorQueueListAccept(string[] queueArray)//, int assignTo
        {
            try
            {
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var queueIdList = "";

                if (queueArray != null)
                {
                    queueIdList = string.Join(",", queueArray);
                }

                var result = _contextMotor.usp_QueueMotorListAccept_Update(queueIdList, userId, userId).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpPost]
        public ActionResult QueueMotorAssignAll(string applicationCode, string insuredName, string payerName, int? isAcceptQueue)
        {
            try
            {
                bool? IsAcceptQueue_val = null;
                if (isAcceptQueue != -1)
                    if (isAcceptQueue == 1)
                    {
                        IsAcceptQueue_val = true;
                    }
                    else
                    {
                        IsAcceptQueue_val = false;
                    }
                string appCode = null;
                if (applicationCode != "")
                {
                    appCode = applicationCode.Trim();
                }
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _contextMotor.usp_QueueMotorAssignV2_Select(appCode, insuredName.Trim(), payerName.Trim(), userId, IsAcceptQueue_val, 0, 999999999, null, null).ToList();


                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }
        [HttpPost]
        public ActionResult QueueMotorAdminAssignAll(string applicationCode, string cutomerName, string payerName, string StartCoverDate, int? payerSubDistrictId, int? payerDistrictId, int? payerProvinceId, int? assignToUserId)
        {
            try
            {
                string appCode = null;
                if (applicationCode != "")
                {
                    appCode = applicationCode.Trim();
                }
                string C_cutomerName = null;
                if (cutomerName != "")
                {
                    C_cutomerName = cutomerName.Trim();
                }
                string P_payerName = null;
                if (payerName != "")
                {
                    P_payerName = payerName.Trim();
                }
                var appStartCoverDate = Convert.ToDateTime(StartCoverDate);
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _contextMotor.usp_QueueMotorAdminAssign_Select(appStartCoverDate, appCode, C_cutomerName, P_payerName, (payerSubDistrictId == -1 ? null : payerSubDistrictId), (payerDistrictId == -1 ? null : payerDistrictId), (payerProvinceId == -1 ? null : payerProvinceId), (assignToUserId == -1 ? null : assignToUserId), 0, 999999999, null, null, null).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpPost]
        public ActionResult QueueMotorListAssign(string[] queueArray, int assignTo)
        {
            try
            {
                var updateByUserId = OAuth2Helper.GetLoginDetail();
                var queueIdList = "";

                if (queueArray != null)
                {
                    queueIdList = string.Join(",", queueArray);
                }
                var result = _contextMotor.usp_QueueMotorListAssignV2_Update(queueIdList, assignTo, updateByUserId.User_ID).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }


        [HttpPost]
        public ActionResult QueueMotorListAdminAssign(string[] queueArray, int assignTo)
        {
            try
            {
                var updateByUserId = OAuth2Helper.GetLoginDetail();
                var queueIdList = "";

                if (queueArray != null)
                {
                    queueIdList = string.Join(",", queueArray);
                }
                var result = _contextMotor.usp_QueueMotorListAssign_Update(queueIdList, assignTo, updateByUserId.User_ID).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpPost]
        public ActionResult QueueMotorUnderwriteOverdue_dt(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string searchDetail1, string searchDetail2, string searchDetail3,
            string searchDetail4, int? payerBranchId)
        {
            try
            {
                int? payerBranch = null;
                if (payerBranchId != -1)
                {
                    payerBranch = payerBranchId;
                }
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                var result = _contextMotor.usp_QueueMotorApprovePendingByBusinessPromoteTeamUserIdV2_Select(userId, searchDetail1, searchDetail2, searchDetail3, searchDetail4, null, payerBranch, indexStart, pageSize, sortField, orderType).ToList();

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
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpPost]
        public ActionResult QueueMotorBranch(int? assignToUserId, int? payerBranchId, string directorName)
        {
            try
            {
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _contextMotor.usp_QueueMotorAssign_Select(null,
                    payerBranchId,
                    null,
                    null,
                    userId,
                    null,
                    null,
                    null,
                    null,
                    null,
                    directorName,
                    0,
                    999999999,
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
        public ActionResult QueueMotorEdit_DT(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string searchDetail1, string searchDetail2, string searchDetail3)
        {
            try
            {
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _contextMotor.usp_QueueMotorEdit_Select(searchDetail1.Trim(), searchDetail2.Trim(), searchDetail3.Trim(), indexStart, pageSize, sortField, orderType).ToList();
                var dt = new Dictionary<string, object>
                {
                    {"draw", draw },
                    {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                    {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                    {"data", result.ToList()},
                };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        #endregion API

        #region commonAPI

        [HttpGet]
        public ActionResult GetProvinceChairman()
        {
            var userDetail = GlobalFunction.GetLoginDetail(HttpContext);
            var provinceList = _contextPH.usp_ProvinceByChairmanUserId_Select(userDetail.User_ID).ToList();
            ViewBag.province = provinceList;

            return Json(provinceList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// GetProvince
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetProvince()
        {
            var userDetail = GlobalFunction.GetLoginDetail(HttpContext);
            var provinceList = _contextPH.usp_Province_Select().ToList();

            return Json(provinceList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDistrict(int provinceId)
        {
            var districtList = _contextMotor.usp_DistrictByProvinceId_Select(provinceId).ToList();
            return Json(districtList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSubDistrict(int districtId)
        {
            var subDistrictList = _contextPH.usp_SubDistrictByDistrictId_Select(districtId).ToList();
            return Json(subDistrictList, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult GetBranch()
        {
            var branchList = _contextPH.usp_Branch_Select(null).ToList();
            return Json(branchList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDirector(int branchId)
        {
            var directorList = _contextPH.usp_DirectorByBranchId_Select(branchId).ToList();
            return Json(directorList, JsonRequestBehavior.AllowGet);
        }
    }
}

#endregion commonAPI