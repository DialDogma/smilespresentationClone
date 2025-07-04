using OfficeOpenXml;
using OfficeOpenXml.Style;
using SmileUnderwriteBranchV2.Helper;
using SmileUnderwriteBranchV2.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace SmileUnderwriteBranchV2.Controllers
{
    [Authorization]
    public class QueueController : Controller
    {
        #region Context

        private readonly UnderwriteBranchV2Entities _context;

        public QueueController()
        {
            _context = new UnderwriteBranchV2Entities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Context

        #region View

        /// <summary>
        /// รอผลพิจารณา
        /// </summary>
        /// <returns></returns>
        /*[Authorization(Roles = "Developer,UnderwriteV2_Chairman")]*/

        public ActionResult QueueWaitConsider(string menu, string dcr, int? branchId)
        {
            ViewBag.menu = menu;
            ViewBag.branchId = branchId;
            ViewBag.UserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

            var cDCR = DateTime.ParseExact(dcr, "yyyyMMdd", new CultureInfo("th-TH"));
            ViewBag.DCR = cDCR.ToString("yyyy-MM-dd");

            return View();
        }

        /// <summary>
        /// ยกเลิกก่อน DCR
        /// </summary>
        /// <param name="dcr"></param>
        /// <param name="branchId"></param>
        /// <returns></returns>
        /*[Authorization(Roles = "Developer,UnderwriteV2_Chairman")]*/

        public ActionResult QueueCancelBeforeDCR(string dcr, int? branchId)
        {
            ViewBag.branchId = branchId;
            ViewBag.UserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

            var cDCR = DateTime.ParseExact(dcr, "yyyyMMdd", new CultureInfo("th-TH"));
            ViewBag.DCR = cDCR.ToString("yyyy-MM-dd");

            return View();
        }

        /// <summary>
        /// คิวที่ยังไม่ได้คัดกรอง
        /// </summary>
        /// <returns></returns>
        [Authorization(Roles = "Developer,UnderwriteV2_Director")]
        public ActionResult QueueUnderwritePending(string menu)
        {
            ViewBag.menu = menu;
            var PHChangeStartCoverDate = Properties.Settings.Default.PHChangeStartCoverDate;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.GetPeriodList(PHChangeStartCoverDate, numberOfMonthToShow);
            ViewBag.PeriodList = periodList;
            ViewBag.PeriodListForCheckDCR = periodList[0].Display;

            var PHCloseDate = Properties.Settings.Default.PHCloseDate;
            ViewBag.PHCloseDate = PHCloseDate;


            ViewBag.PHChangeStartCoverDate = PHChangeStartCoverDate;

            ViewBag.UserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

            return View();
        }

        /// <summary>
        /// จัดการคิวงานคัดกรองและมอบบัตร
        /// </summary>
        /// <returns></returns>
        [Authorization(Roles = "Developer,UnderwriteV2_Director")]
        public ActionResult QueueAssign()
        {
            ViewBag.UserDetail = GlobalFunction.GetLoginDetail(HttpContext);
            return View();
        }

        // [Authorization(Roles = "Developer")]
        public ActionResult QueueAdminAssign()
        {
            var PHChangeStartCoverDate = Properties.Settings.Default.PHChangeStartCoverDate;
            var periodList = GlobalFunction.GetPeriodList(PHChangeStartCoverDate, 13);
            ViewBag.PeriodList = periodList;
            ViewBag.UserDetail = GlobalFunction.GetLoginDetail(HttpContext);
            var directorList = _context.usp_DirectorByBranchId_Select(null).ToList();
            ViewBag.directorList = directorList;

            return View();
        }

        /// <summary>
        /// คิวงานคัดกรองเกินกำหนด
        /// </summary>
        /// <returns></returns>
        [Authorization(Roles = "Developer,UnderwriteV2_BusinesPromoteTeam")]
        public ActionResult QueueUnderwriteOverdue()
        {
            ViewBag.UserDetail = GlobalFunction.GetLoginDetail(HttpContext);
            ViewBag.UserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            var branchList = _context.usp_Branch_Select(null).ToList();
            ViewBag.branchList = branchList;
            return View();
        }

        /// <summary>
        /// รับทราบคิวงาน
        /// </summary>
        /// <returns></returns>
        [Authorization(Roles = "Developer,UnderwriteV2_Director")]
        public ActionResult GetQueueUnderwrite()
        {
            ViewBag.UserDetail = GlobalFunction.GetLoginDetail(HttpContext);
            return View();
        }

        /// <summary>
        /// คิวงานคัดกรองเกินกำหนด
        /// </summary>
        /// <returns></returns>
        //[Authorization(Roles = "Developer")]
        public ActionResult EditQueueUnderwrite()
        {
            ViewBag.UserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            /* var PHOpenDate = Properties.Settings.Default.PHOpenDate;
             ViewBag.PHOpenDate = PHOpenDate;*/
            var PHCloseDate = Properties.Settings.Default.PHCloseDate;
            ViewBag.PHCloseDate = PHCloseDate;

            return View();
        }

        #endregion View

        #region api

        [HttpPost]
        public ActionResult QueueWaitConsider_dt(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string search, string dcr, int? branchId)
        {
            try
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext);
                var cDCR = Convert.ToDateTime(dcr);
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');

                if (roleList.Contains("Developer"))
                {
                    var result = _context.usp_QueueApprovePending_Select(cDCR, (branchId != null ? branchId : user.Branch_ID), null, null, null,
                                   indexStart, pageSize, sortField, orderType, search).ToList();

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
                    var result = _context.usp_QueueApprovePending_Select(cDCR, (branchId != null ? branchId : user.Branch_ID), null, null, null, indexStart, pageSize, sortField, orderType, search).ToList();

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
                    var result = _context.usp_QueueApprovePending_Select(cDCR, (branchId != null ? branchId : user.Branch_ID), null, null, null, indexStart, pageSize, sortField, orderType, search).ToList();

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

        [HttpPost]
        public ActionResult QueueConsidered_dt(int draw, int? indexStart, int? pageSize, string sortField,
            string orderType, string search, string dcr, int? branchId)
        {
            try
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext);
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                var cDCR = Convert.ToDateTime(dcr);
                if (roleList.Contains("Developer"))
                {
                    var result = _context.usp_QueueApproveComplete_Select(cDCR, (branchId != null ? branchId : user.Branch_ID), null, null, null,
                            indexStart, pageSize, sortField, orderType, search).ToList();

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
                    var result = _context.usp_QueueApproveComplete_Select(cDCR, (branchId != null ? branchId : user.Branch_ID), null, null, null,
                        indexStart, pageSize, sortField, orderType, search).ToList();

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
                    var result = _context.usp_QueueApproveComplete_Select(cDCR, (branchId != null ? branchId : user.Branch_ID), null, null, null,
                                                indexStart, pageSize, sortField, orderType, search).ToList();

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

        [HttpGet]
        public ActionResult QueueeByQueueStatus_dt(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string search, string dcr, int? branchId, int queueStatusId = 7)
        {
            try
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext);
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                var cDCR = Convert.ToDateTime(dcr);
                if (roleList.Contains("Developer"))
                {
                    var result = _context.usp_QueueByQueueStatusId_Select(cDCR, (branchId != null ? branchId : user.Branch_ID), null, null, null, queueStatusId,
                            indexStart, pageSize, sortField, orderType, search).ToList();

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
                    var result = _context.usp_QueueByQueueStatusId_Select(cDCR, (branchId != null ? branchId : user.Branch_ID), null, null, null, queueStatusId,
                        indexStart, pageSize, sortField, orderType, search).ToList();

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
                    var result = _context.usp_QueueByQueueStatusId_Select(cDCR, (branchId != null ? branchId : user.Branch_ID), null, null, null, queueStatusId,
                                                indexStart, pageSize, sortField, orderType, search).ToList();

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

        [HttpPost]
        [Authorization(Roles = "Developer,UnderwriteV2_Director")]
        public ActionResult QueueUnderwritePending_DT(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string startCoverDate, string search1, string search2, string appId)
        {
            try
            {
                DateTime? nStartCoverdate = null;
                if (startCoverDate != "-1" && startCoverDate != null)
                {
                    nStartCoverdate = Convert.ToDateTime(startCoverDate);
                }
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _context.usp_QueueUnderwritePending_Select(nStartCoverdate, null, null, null, userId, indexStart, pageSize, sortField, orderType, search1.Trim(), search2.Trim(), appId.Trim()).ToList();

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
        [Authorization(Roles = "Developer,UnderwriteV2_Director")]
        public ActionResult QueueUnderwritePendingComplete_DT(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string startCoverDate, string searchDetail1, string searchDetail2, string applicationCode)
        {
            try
            {
                DateTime? nStartCoverdate = null;
                if (startCoverDate != "-1" && startCoverDate != null)
                {
                    nStartCoverdate = Convert.ToDateTime(startCoverDate);
                }
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _context.usp_QueueUnderwriteComplete_Select(nStartCoverdate, null, null, null, userId, indexStart, pageSize, sortField, orderType, searchDetail1.Trim(), searchDetail2.Trim(), applicationCode.Trim()).ToList();

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
        [Authorization(Roles = "Developer,UnderwriteV2_Director")]
        public ActionResult QueueUnderwriteCancelBeforeDCR_DT(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string startCoverDate, string searchDetail1, string searchDetail2, string applicationCode)
        {
            try
            {
                DateTime? nStartCoverdate = null;
                if (startCoverDate != "-1" && startCoverDate != null)
                {
                    nStartCoverdate = Convert.ToDateTime(startCoverDate);
                }
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _context.usp_QueueUnderwriteCancelBeforeDCR_Select(nStartCoverdate, null, null, null, userId, indexStart, pageSize, sortField, orderType, searchDetail1.Trim(), searchDetail2.Trim(), applicationCode.Trim()).ToList();

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
        public ActionResult QueueAccept_DT(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string applicationCode, string insuredName, string payerName)
        {
            try
            {
                string appCode = null;
                if (applicationCode != "")
                {
                    appCode = applicationCode.Trim();
                }
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                var result = _context.usp_QueueAccept_Select(appCode, insuredName.Trim(), payerName.Trim(), userId, indexStart, pageSize, sortField, orderType).ToList();

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
        /// QueueAssign All / No paging
        /// </summary>
        /// <param name="search"></param>
        /// <param name="provinceId"></param>
        /// <param name="districtId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QueueAcceptAll(string applicationCode, string insuredName, string payerName)
        {
            try
            {
                string appCode = null;
                if (applicationCode != "")
                {
                    appCode = applicationCode.Trim();
                }
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _context.usp_QueueAccept_Select(appCode, insuredName.Trim(), payerName.Trim(), userId, 0, 999999999, null, null).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
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
        public ActionResult QueueListAccept(string[] queueArray)//, int assignTo
        {
            try
            {
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var queueIdList = "";

                if (queueArray != null)
                {
                    queueIdList = string.Join(",", queueArray);
                }

                var result = _context.usp_QueueListAccept_Update(queueIdList, userId, userId).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        /// <summary>
        /// QueueAssign Datatable
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="indexStart"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <param name="search"></param>
        /// <param name="provinceId"></param>
        /// <param name="districtId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QueueAssign_DT(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string applicationCode, string insuredName, string payerName, int? isAcceptQueue)
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
                var result = _context.usp_QueueAssignV2_Select(appCode, insuredName.Trim(), payerName.Trim(), userId, IsAcceptQueue_val, indexStart, pageSize, sortField, orderType).ToList();
                if (devRole)
                {
                    result = _context.usp_QueueAssignV2_Select(appCode, insuredName.Trim(), payerName.Trim(), null, IsAcceptQueue_val, indexStart, pageSize, sortField, orderType).ToList();
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
        public ActionResult QueueAdminAssign_DT(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string applicationCode, string cutomerName, string payerName, string StartCoverDate, int? payerSubDistrictId, int? payerDistrictId, int? payerProvinceId, int? assignToUserId)
        {
            try
            {
                string appCode = null;
                if (applicationCode != "")
                {
                    appCode = applicationCode.Trim();
                }
                var appStartCoverDate = Convert.ToDateTime(StartCoverDate);



                var result = _context.usp_QueueAdminAssign_Select(appStartCoverDate, appCode, cutomerName.Trim(), payerName.Trim(), (payerSubDistrictId == -1 ? null : payerSubDistrictId), (payerDistrictId == -1 ? null : payerDistrictId), (payerProvinceId == -1 ? null : payerProvinceId), (assignToUserId == -1 ? null : assignToUserId), indexStart, pageSize, sortField, orderType, null).ToList();

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
        /// QueueAssign All / No paging
        /// </summary>
        /// <param name="search"></param>
        /// <param name="provinceId"></param>
        /// <param name="districtId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QueueAssignAll(string applicationCode, string insuredName, string payerName, int? isAcceptQueue)
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
                var result = _context.usp_QueueAssignV2_Select(appCode, insuredName.Trim(), payerName.Trim(), userId, IsAcceptQueue_val, 0, 999999999, null, null).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpPost]
        public ActionResult QueueAdminAssignAll(string applicationCode, string cutomerName, string payerName, string StartCoverDate, int? payerSubDistrictId, int? payerDistrictId, int? payerProvinceId, int? assignToUserId)
        {
            try
            {

                string appCode = null;
                if (applicationCode != "")
                {
                    appCode = applicationCode.Trim();
                }
                var appStartCoverDate = Convert.ToDateTime(StartCoverDate);
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _context.usp_QueueAdminAssign_Select(appStartCoverDate, appCode, cutomerName.Trim(), payerName.Trim(), (payerSubDistrictId == -1 ? null : payerSubDistrictId), (payerDistrictId == -1 ? null : payerDistrictId), (payerProvinceId == -1 ? null : payerProvinceId), (assignToUserId == -1 ? null : assignToUserId), 0, 999999999, null, null, null).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        /// <summary>
        /// QueueListAssign Update
        /// </summary>
        /// <param name="queueArray"></param>
        /// <param name="assignTo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QueueListAssign(string[] queueArray, int assignTo)
        {
            try
            {
                var UserDetail = OAuth2Helper.GetLoginDetail();
                var queueIdList = "";

                if (queueArray != null)
                {
                    queueIdList = string.Join(",", queueArray);
                }

                var result = _context.usp_QueueListAssignV2_Update(queueIdList, assignTo, UserDetail.User_ID).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }


        [HttpPost]
        public ActionResult QueueListAdminAssign(string[] queueArray, int assignTo)
        {
            try
            {
                var UserDetail = OAuth2Helper.GetLoginDetail();
                var queueIdList = "";

                if (queueArray != null)
                {
                    queueIdList = string.Join(",", queueArray);
                }

                var result = _context.usp_QueueListAssign_Update(queueIdList, assignTo, UserDetail.User_ID).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        /// <summary>
        /// ทดสอบ
        /// </summary>
        private class ResultObj
        {
            public bool IsResult { get; set; }
        }

        [HttpPost]
        public ActionResult QueueUnderwriteOverdue_dt(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string searchDetail1, string searchDetail2, string searchDetail3,
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
                var result = _context.usp_QueueApprovePendingByBusinessPromoteTeamUserIdV2_Select(userId, searchDetail1.Trim(), searchDetail2.Trim(), searchDetail3.Trim(), searchDetail4.Trim(), null, payerBranch, indexStart, pageSize, sortField, orderType).ToList();
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
        public ActionResult QueueEdit_DT(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string searchDetail1, string searchDetail2, string searchDetail3)
        {
            try
            {
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _context.usp_QueueEdit_Select(searchDetail1.Trim(), searchDetail2.Trim(), searchDetail3.Trim(), indexStart, pageSize, sortField, orderType).ToList();
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


        public ActionResult DownloadQueuePendingExcelReport(string startCoverDate, string search1, string search2, string appId)
        {
            //await Task.Yield();
            using (var db = new UnderwriteBranchV2Entities())
            {
                var stream = new MemoryStream();
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet1 = package.Workbook.Worksheets.Add("รายงานคัดกรองและมอบบัตร_ยังไม่ได้คัดกรอง");
                    {
                        var data_sheet1 = _context.usp_QueueUnderwritePending_Select(null, null, null, null, userId, 0, 9999999, null, null, search1.Trim(), search2.Trim(), appId.Trim()).Select(o => new
                        {
                            เลข_App = o.ApplicationCode,
                            ชื่อผู้เอาประกัน = o.CustomerName,
                            ชื่อผู้ชำระเบี้ย = o.PayerName,
                            ชื่อหน่วยงาน = o.PayerBuildingName,
                            ตำบล = o.SubDistrictDetail,
                            อำเภอ = o.DistrictDetail,
                            จังหวัด = o.ProvinceDetail,
                            วันครบกำหนดคิวงาน7วัน = (o.QueueChallengeDate != null ? o.QueueChallengeDate.Value.ToString("dd/MM/yyyy HH:mm", new CultureInfo("th-TH")) : ""),
                            วันครบกำหนดคิวงาน = (o.QueueExpireDate != null ? o.QueueExpireDate.Value.ToString("dd/MM/yyyy HH:mm", new CultureInfo("th-TH")) : ""),
                            แผน = o.Premium,
                            วันเริ่มคุ้มครอง = (o.AppStartCoverDate != null ? o.AppStartCoverDate.Value.ToString("dd/MM/yyyy", new CultureInfo("th-TH")) : ""),

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
        #endregion api

        #region commonAPI

        /// <summary>
        /// GET Province By UserId
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetProvinceChairman()
        {
            var userDetail = GlobalFunction.GetLoginDetail(HttpContext);
            var provinceList = _context.usp_ProvinceByChairmanUserId_Select(userDetail.User_ID).ToList();

            return Json(provinceList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GET District By Province ID
        /// </summary>
        /// <param name="provinceId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDistrict(int provinceId)
        {
            var districtList = _context.usp_DistrictByProvinceId_Select(provinceId).ToList();
            return Json(districtList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSubDistrict(int districtId)
        {
            var subDistrictList = _context.usp_SubDistrictByDistrictId_Select(districtId).ToList();
            return Json(subDistrictList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GET BRANCH
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetBranch()
        {
            var branchList = _context.usp_Branch_Select(null).ToList();
            return Json(branchList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDirector(int branchId)
        {
            var directorList = _context.usp_DirectorByBranchId_Select(branchId).ToList();
            return Json(directorList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetProvince
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetProvince()
        {
            var userDetail = GlobalFunction.GetLoginDetail(HttpContext);
            var provinceList = _context.usp_Province_Select().ToList();

            return Json(provinceList, JsonRequestBehavior.AllowGet);
        }

        #endregion commonAPI
    }
}