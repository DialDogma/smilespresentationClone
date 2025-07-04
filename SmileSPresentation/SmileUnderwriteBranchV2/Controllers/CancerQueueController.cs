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
    public class CancerQueueController : Controller
    {
        private readonly UnderwriteBranchV2CancerEntities _contextCI;
        private readonly UnderwriteBranchV2Entities _contextPH;

        public CancerQueueController()
        {
            _contextCI = new UnderwriteBranchV2CancerEntities();
            _contextPH = new UnderwriteBranchV2Entities();
        }

        // GET: CancerQueue
        [Authorization(Roles = "Developer,UnderwriteV2_Director")]
        public ActionResult CancerQueueUnderwritePending()
        {
            var CancerChangeStartCoverDate = Properties.Settings.Default.CancerChangeStartCoverDate;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.GetPeriodList(CancerChangeStartCoverDate, numberOfMonthToShow);
            ViewBag.PeriodList = periodList;
            ViewBag.PeriodListForCheckDCR = periodList[0].Display;

            /*  var CancerOpenDate = Properties.Settings.Default.CancerOpenDate;
              ViewBag.CancerOpenDate = CancerOpenDate;*/
            var CancerCloseDate = Properties.Settings.Default.CancerCloseDate;
            ViewBag.CancerCloseDate = CancerCloseDate;

            ViewBag.UserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            return View();
        }

        public ActionResult CancerQueueAssign()
        {
            ViewBag.UserDetail = GlobalFunction.GetLoginDetail(HttpContext);
            var userDetail = GlobalFunction.GetLoginDetail(HttpContext);
            var provinceList = _contextPH.usp_ProvinceByChairmanUserId_Select(userDetail.User_ID).ToList();
            ViewBag.province = provinceList;
            return View();
        }


        public ActionResult CancerQueueAdminAssign()
        {
            var CancerChangeStartCoverDate = Properties.Settings.Default.CancerChangeStartCoverDate;
            var periodList = GlobalFunction.GetPeriodList(CancerChangeStartCoverDate, 13);
            ViewBag.PeriodList = periodList;
            ViewBag.UserDetail = GlobalFunction.GetLoginDetail(HttpContext);
            var directorList = _contextPH.usp_DirectorByBranchId_Select(null).ToList();
            ViewBag.directorList = directorList;

            return View();
        }
        public ActionResult GetCancerQueueUnderwrite()
        {
            ViewBag.UserDetail = GlobalFunction.GetLoginDetail(HttpContext);
            var userDetail = GlobalFunction.GetLoginDetail(HttpContext);
            var provinceList = _contextPH.usp_ProvinceByChairmanUserId_Select(userDetail.User_ID).ToList();
            ViewBag.province = provinceList;
            return View();
        }


        public ActionResult EditCancerQueueUnderwrite()
        {
            ViewBag.UserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            ViewBag.UserDetail = GlobalFunction.GetLoginDetail(HttpContext);

            var CancerCloseDate = Properties.Settings.Default.CancerCloseDate;
            ViewBag.CancerCloseDate = CancerCloseDate;

            var CancerChangePeriodDay = Properties.Settings.Default.CancerChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.GetPeriodList(CancerChangePeriodDay, numberOfMonthToShow);
            ViewBag.PeriodList = periodList;
            ViewBag.PeriodListForCheckDCR = periodList[0].Display;

            return View();
        }

        public ActionResult CancerQueueConsidering(string menu, string dcr, int? branchId)
        {
            ViewBag.menu = menu;
            ViewBag.branchId = branchId;
            ViewBag.UserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

            var cDCR = DateTime.ParseExact(dcr, "yyyyMMdd", new CultureInfo("th-TH"));
            ViewBag.DCR = cDCR.ToString("yyyy-MM-dd");
            return View();
        }

        public ActionResult CancerQueueCancelBeforeDCR(string dcr, int? branchId)
        {
            ViewBag.branchId = branchId;
            ViewBag.UserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

            var cDCR = DateTime.ParseExact(dcr, "yyyyMMdd", new CultureInfo("th-TH"));
            ViewBag.DCR = cDCR.ToString("yyyy-MM-dd");
            return View();
        }

        public ActionResult CancerQueueUnderwriteOverdue()
        {
            ViewBag.UserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            var branchList = _contextPH.usp_Branch_Select(null).ToList();
            ViewBag.branchList = branchList;
            return View();
        }

        #region API
        [HttpPost]
        public ActionResult QueueCancerEdit_DT(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string searchDetail1, string searchDetail2, string searchDetail3)
        {
            try
            {
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _contextCI.usp_QueueCIEdit_Select(searchDetail1.Trim(), searchDetail2.Trim(), searchDetail3.Trim(), indexStart, pageSize, sortField, orderType).ToList();
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

        //ยังไม่ได้คัดกรอง
        [HttpPost]
        public ActionResult CancerQueueUnderwritePendingDT(string startCoverDate,
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

                var result = _contextCI.usp_QueueCIUnderwritePending_Select(nStartCoverdate, playerBranchId, payerDistricId, payerStudyAreaId, Convert.ToInt32(user.User_ID), applicationCode, insuredName, payerName, indexStart, pageSize, sortField, orderType).ToList();

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

        // คัดกรองแล้ว
        [HttpPost]
        [Authorization(Roles = "Developer,UnderwriteV2_Director")]
        public ActionResult QueueCancerUnderwritePendingComplete_DT(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string startCoverDate, string searchDetail1, string searchDetail2, string applicationCode)
        {
            try
            {
                DateTime? nStartCoverdate = null;
                if (startCoverDate != "-1" && startCoverDate != null)
                {
                    nStartCoverdate = Convert.ToDateTime(startCoverDate);
                }
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _contextCI.usp_QueueCIUnderwriteComplete_Select(applicationCode.Trim(), searchDetail1.Trim(), searchDetail2.Trim(), nStartCoverdate, userId, indexStart, pageSize, sortField, orderType).ToList();

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

        // ยกเลิกก่อน DCR
        [HttpPost]
        [Authorization(Roles = "Developer,UnderwriteV2_Director")]
        public ActionResult QueueCancerUnderwriteCancelBeforeDCR_DT(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string startCoverDate, string searchDetail1, string searchDetail2, string applicationCode)
        {
            try
            {
                DateTime? nStartCoverdate = null;
                if (startCoverDate != "-1" && startCoverDate != null)
                {
                    nStartCoverdate = Convert.ToDateTime(startCoverDate);
                }
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _contextCI.usp_QueueCIUnderwriteCancelBeforeDCR_Select(applicationCode.Trim(), searchDetail1.Trim(), searchDetail2.Trim(), nStartCoverdate, userId, indexStart, pageSize, sortField, orderType).ToList();

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

        #region จัดการคิวงาน
        [HttpPost]
        public ActionResult QueueCancerAssign_DT(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string applicationCode, string insuredName, string payerName, int? isAcceptQueue)
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
                var result = _contextCI.usp_QueueCIAssignV2_Select(appCode, insuredName.Trim(), payerName.Trim(), userId, IsAcceptQueue_val, indexStart, pageSize, sortField, orderType).ToList();
                if (devRole)
                {
                    result = _contextCI.usp_QueueCIAssignV2_Select(appCode, insuredName.Trim(), payerName.Trim(), null, IsAcceptQueue_val, indexStart, pageSize, sortField, orderType).ToList();

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
        public ActionResult QueueCancerAdminAssign_DT(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string applicationCode, string cutomerName, string payerName, string StartCoverDate, int? payerSubDistrictId, int? payerDistrictId, int? payerProvinceId, int? assignToUserId)
        {
            try
            {
                string appCode = null;
                if (applicationCode != "")
                {
                    appCode = applicationCode.Trim();
                }
                var appStartCoverDate = Convert.ToDateTime(StartCoverDate);



                var result = _contextCI.usp_QueueCIAdminAssign_Select(appStartCoverDate, appCode, cutomerName.Trim(), payerName.Trim(), (payerSubDistrictId == -1 ? null : payerSubDistrictId), (payerDistrictId == -1 ? null : payerDistrictId), (payerProvinceId == -1 ? null : payerProvinceId), (assignToUserId == -1 ? null : assignToUserId), indexStart, pageSize, sortField, orderType, null).ToList();

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
        public ActionResult QueueCancerListAssign(string[] queueArray, int assignTo)
        {
            try
            {
                var UserDetail = OAuth2Helper.GetLoginDetail();
                var queueIdList = "";

                if (queueArray != null)
                {
                    queueIdList = string.Join(",", queueArray);
                }

                var result = _contextCI.usp_QueueCIListAssignV2_Update(queueIdList, assignTo, UserDetail.User_ID).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpPost]
        public ActionResult QueueCancerListAdminAssign(string[] queueArray, int assignTo)
        {
            try
            {
                var UserDetail = OAuth2Helper.GetLoginDetail();
                var queueIdList = "";

                if (queueArray != null)
                {
                    queueIdList = string.Join(",", queueArray);
                }

                var result = _contextCI.usp_QueueCIListAssign_Update(queueIdList, assignTo, UserDetail.User_ID).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }





        [HttpPost]
        public ActionResult QueueCancerAssignAll(string applicationCode, string insuredName, string payerName, int? isAcceptQueue)
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
                var result = _contextCI.usp_QueueCIAssignV2_Select(appCode, insuredName.Trim(), payerName.Trim(), userId, IsAcceptQueue_val, 0, 999999999, null, null).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }


        [HttpPost]
        public ActionResult QueueCancerAdminAssignAll(string applicationCode, string cutomerName, string payerName, string StartCoverDate, int? payerSubDistrictId, int? payerDistrictId, int? payerProvinceId, int? assignToUserId)
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
                var result = _contextCI.usp_QueueCIAdminAssign_Select(appStartCoverDate, appCode, cutomerName.Trim(), payerName.Trim(), (payerSubDistrictId == -1 ? null : payerSubDistrictId), (payerDistrictId == -1 ? null : payerDistrictId), (payerProvinceId == -1 ? null : payerProvinceId), (assignToUserId == -1 ? null : assignToUserId), 0, 999999999, null, null, null).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }
        #endregion

        #region รับทราบคิวงาน
        [HttpPost]
        public ActionResult CancerQueueAccept_DT(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string applicationCode, string insuredName, string payerName)
        {
            try
            {
                string appCode = null;
                if (applicationCode != "")
                {
                    appCode = applicationCode.Trim();
                }
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                var result = _contextCI.usp_QueueCIAccept_Select(appCode, (insuredName == "" ? null : insuredName.Trim()), (payerName == "" ? null : payerName.Trim()), userId, indexStart, pageSize, sortField, orderType).ToList();

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
        public ActionResult CancerQueueAcceptAll(string applicationCode, string insuredName, string payerName)
        {
            try
            {
                string appCode = null;
                if (applicationCode != "")
                {
                    appCode = applicationCode.Trim();
                }
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _contextCI.usp_QueueCIAccept_Select(appCode, insuredName.Trim(), payerName.Trim(), userId, 0, 999999999, null, null).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpPost]
        public ActionResult CancerQueueListAccept(string[] queueArray)//, int assignTo
        {
            try
            {
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var queueIdList = "";

                if (queueArray != null)
                {
                    queueIdList = string.Join(",", queueArray);
                }

                var result = _contextCI.usp_QueueCIListAccept_Update(queueIdList, userId, userId).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        #endregion

        //UI13 tab1 รอพิจารณา
        [HttpPost]
        public ActionResult CancerQueueWaitConsider_dt(
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
                    var result = _contextCI.usp_QueueCIApprovePending_Select(
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
                    var result = _contextCI.usp_QueueCIApprovePending_Select(
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
                    var result = _contextCI.usp_QueueCIApprovePending_Select(
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

        //UI15 พิจารนาแล้ว Tab2
        [HttpPost]
        public ActionResult CancerQueueConsiderComplete_dt(
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
                    var result = _contextCI.usp_QueueCIApproveComplete_Select(
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
                    var result = _contextCI.usp_QueueCIApproveComplete_Select(
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
                    var result = _contextCI.usp_QueueCIApproveComplete_Select(
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

        //ui17 คิวงานยกเลิกก่อนDCR
        [HttpGet]
        public ActionResult CancerQueueByQueueStatus_dt(
            int? draw,
            int? indexStart,
            int? pageSize,
            string sortField,
            string orderType,
            string searchDetail,
            string dcr,
            int? payerBranchId)
        {
            try
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext);
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                var cDCR = Convert.ToDateTime(dcr);
                if (roleList.Contains("Developer"))
                {
                    var result = _contextCI.usp_QueueCICancelBeforeDCR_Select(
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
                    var result = _contextCI.usp_QueueCICancelBeforeDCR_Select(
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
                    var result = _contextCI.usp_QueueCICancelBeforeDCR_Select(
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

        // คิวงานเกินกำหนด
        [HttpPost]
        public ActionResult QueueCancerUnderwriteOverdue_dt(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string searchDetail1, string searchDetail2, string searchDetail3,
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

                var result = _contextCI.usp_QueueCIApprovePendingByBusinessPromoteTeamUserIdV2_Select(userId, searchDetail1, searchDetail2, searchDetail3, searchDetail4, null, payerBranch, indexStart, pageSize, sortField, orderType).ToList();

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


        public ActionResult DownloadQueuePendingExcelReport(string startCoverDate, string insuredName, string payerName, string appId)
        {
            //await Task.Yield();
            using (var db = new UnderwriteBranchV2Entities())
            {
                var stream = new MemoryStream();

                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet1 = package.Workbook.Worksheets.Add("รายงานการมอบกรมธรรม์");
                    {
                        var data_sheet1 = _contextCI.usp_QueueCIUnderwritePending_Select(null, null, null, null, userId, appId, insuredName, payerName, 0, 99999, null, null).Select(o => new
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


        #endregion API

        #region common API

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
            var districtList = _contextPH.usp_DistrictByProvinceId_Select(provinceId).ToList();
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

        #endregion common API
    }
}