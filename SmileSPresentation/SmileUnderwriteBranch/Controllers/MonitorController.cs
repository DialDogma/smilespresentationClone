using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.Expressions;
using Microsoft.AspNet.SignalR;
using SmileUnderwriteBranch.Helper;
using SmileUnderwriteBranch.Models;

namespace SmileUnderwriteBranch.Controllers
{
    [Authorization]
    public class MonitorController : Controller
    {
        /// <summary>
        /// Monitor Supervisor
        /// </summary>
        /// <returns></returns>
        // GET: Monitor
        [Authorization(Roles = "Developer,SmileCall_Admin,SmileCall_Underwrite")]
        public ActionResult MonitorSupervisor()
        {
            using (var db = new UnderwriteBranchEntities())
            {
                //GET CONFIG FROM STOREPROCEDURE
                var config = db.usp_PHQueueConfig_Select().First();
                var periodList = LocalFunction.GetPeriodList();
                ViewBag.PeriodList = periodList;
                ViewBag.Config = config.DurationDays;
            }

            return View();
        }

        /// <summary>
        /// Monitor Supervisor BranchStatus Detail
        /// </summary>
        /// <returns></returns>
        [Authorization(Roles = "Developer,SmileCall_Admin,SmileCall_Underwrite")]
        public ActionResult MonitorSupervisorBranchStatusDetail(string Id, string AreaName, string period, string title)
        {
            ViewBag.AreaId = Id;
            ViewBag.AreaName = AreaName;
            ViewBag.Period = period;
            ViewBag.Title = title;
            return View();
        }

        /// <summary>
        /// Admin config
        /// </summary>
        /// <returns></returns>
        [Authorization(Roles = "Developer,SmileCall_Admin")]
        public ActionResult AdminConfig()
        {
            return View();
        }

        /// <summary>
        /// CM Config
        /// </summary>
        /// <returns></returns>
        [Authorization(Roles = "Developer,SmileCall_Admin")]
        public ActionResult CMConfig()
        {
            return View();
        }

        /// <summary>
        /// Assign config
        /// </summary>
        /// <returns></returns>
        [Authorization(Roles = "Developer,SmileCall_Admin")]
        public ActionResult AssignConfig()
        {
            var periodList = LocalFunction.GetPeriodList();
            ViewBag.PeriodList = periodList;
            return View();
        }

        [Authorization(Roles = "Developer,SmileUnderwriteBranch_Advisor")]
        public ActionResult AdminConsult()
        {
            var periodList = LocalFunction.GetPeriodList();
            ViewBag.PeriodList = periodList;
            return View();
        }

        public ActionResult GetDatatablesMonitorSupervisorBranchStatusDetail(int draw, int schoolAreaId, string period, int pHQueueStatusId, string search, int pageStart, int pageSize, string sortField, string orderType)
        {
            using (var db = new UnderwriteBranchEntities())
            {
                //GET CONFIG FROM STOREPROCEDURE
                var config = db.usp_PHQueueConfig_Select().First();
                DateTime? convert_period;
                //try Convert period to datetime
                try
                {
                    convert_period = DateTime.Parse(period);
                }
                catch (Exception)
                {
                    convert_period = null;
                }

                //check period != null || period == null || userId = null
                var lst = convert_period != null ? db.usp_PHQueue_Select(null, schoolAreaId, null, convert_period, pHQueueStatusId, search, null, pageStart, pageSize, sortField, orderType).ToList() : db.usp_PHQueue_Select(null, schoolAreaId, config.DurationDays, null, pHQueueStatusId, search, null, pageStart, pageSize, sortField, orderType).ToList();

                var dt = new Dictionary<string, object>
                {
                    {"draw",draw },
                    {"recordsTotal", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"recordsFiltered", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"data", lst.ToList()}
                };
                return Json(dt, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GET DATATABLES ASSIGN CONFIG
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="search"></param>
        /// <param name="period"></param>
        /// <param name="pageStart"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public ActionResult GetDatatablesAssignConfig(int draw, string search, string period, int pageStart, int pageSize, string sortField, string orderType)
        {
            using (var db = new UnderwriteBranchEntities())
            {
                var c_period = DateTime.Parse(period);
                var lst = db.usp_PHQueueAssign_Select(int.Parse(search), null, null, c_period, 4, null, pageStart, pageSize, sortField, orderType).ToList();

                var dt = new Dictionary<string, object>
                {
                    {"draw",draw },
                    {"recordsTotal", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"recordsFiltered", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"data", lst.ToList()}
                };
                return Json(dt, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///PH QUEUE ASSIGN  INSERT
        /// </summary>
        /// <param name="ListQueueId"></param>
        /// <param name="expireDate"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public ActionResult PHQueueAssignInsert(List<string> ListQueueId, string expireDate, string period)
        {
            using (var db = new UnderwriteBranchEntities())
            {
                try
                {
                    var result = 0;
                    var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                    var c_queueExpireDate = DateTime.Parse(expireDate);
                    var c_period = DateTime.Parse(period);
                    foreach (var item in ListQueueId)
                    {
                        result = db.usp_PHQueueAssign_Insert(int.Parse(item), c_queueExpireDate, c_period, userId).FirstOrDefault().Value;
                    }

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        /// <summary>
        /// GET Study Area Config
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="search"></param>
        /// <param name="pageStart"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Get_StudyAreaUserConfig(int draw, string search, int pageStart, int pageSize, string sortField, string orderType)
        {
            using (var db = new UnderwriteBranchEntities())
            {
                var lst = db.usp_StudyAreaUserConfig_Select(search, pageStart, pageSize, sortField, orderType).ToList();

                var dt = new Dictionary<string, object>
                {
                    {"draw",draw },
                    {"recordsTotal", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"recordsFiltered", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"data", lst.ToList()}
                };
                return Json(dt, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Get_StudyAreaCMUserConfig(int draw, string search, int pageStart, int pageSize, string sortField, string orderType)
        {
            using (var db = new UnderwriteBranchEntities())
            {
                var lst = db.usp_StudyAreaCMUserConfig_Select(search, pageStart, pageSize, sortField, orderType).ToList();

                var dt = new Dictionary<string, object>
                {
                    {"draw",draw },
                    {"recordsTotal", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"recordsFiltered", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"data", lst.ToList()}
                };
                return Json(dt, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GET USER AREA DETAIL //ผอ สาขา
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get_StudyAreaUserDetail(int configId)
        {
            using (var db = new UnderwriteBranchEntities())
            {
                var result = db.usp_StudyAreaUserConfigByStudyAreaUserConfigId_Select(configId).First();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GET CM USER AREA DETAIL //ประธานสาขา
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get_StudyAreaCMUserDetail(int configId)
        {
            using (var db = new UnderwriteBranchEntities())
            {
                var result = db.usp_StudyAreaCMUserConfigByStudyAreaUserConfigId_Select(configId).First();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// SEARCH DATA CENTER PERSON
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get_DataCenterPerson(string search)
        {
            using (var db = new UnderwriteBranchEntities())
            {
                var lst = db.usp_DataCenter_PersonUser_Select(search).ToList();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// ผอ สาขา
        /// </summary>
        /// <param name="configId">CONFIG ID</param>
        /// <param name="userId">USER ID NEW PERSON</param>
        /// <param name="moveQueue">TRUE/FALSE</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult StudyAreaConfig_Update(int configId, int userId, bool moveQueue)
        {
            using (var db = new UnderwriteBranchEntities())
            {
                var lst = db.usp_StudyAreaUserConfigUpdateByStudyAreaUserConfigId_Update(configId, userId, moveQueue).First();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// ประธานสาขา
        /// </summary>
        /// <param name="configId"></param>
        /// <param name="userId"></param>
        /// <param name="moveQueue"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult StudyAreaCMConfig_Update(int configId, int userId)
        {
            using (var db = new UnderwriteBranchEntities())
            {
                var lst = db.usp_StudyAreaCMUserConfigUpdateByStudyAreaUserConfigId_Update(configId, userId).First();

                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Status Count
        /// </summary>
        /// <param name="period"></param>
        /// <returns>Json list</returns>
        [HttpGet]
        public ActionResult Get_PHQueueStatusCountAll(string period = "")
        {
            using (var db = new UnderwriteBranchEntities())
            {
                //DECLARE NEW OBJECT
                var newObject = new StatusDetail();

                if (period != "")
                {
                    //CALL STORE PROCEDURE
                    var lst = db.usp_PHQueueStatusCountAll_Select(null, DateTime.Parse(period)).ToList().First();

                    //Assign result to new object
                    newObject.PHQueueComplete = lst.PHQueueComplete.Value;
                    newObject.PHQueueOnProcess = lst.PHQueueOnProcess.Value;
                    newObject.PHQueueOverdue = lst.PHQueueOverdue.Value;
                    newObject.PHQueueOverdueComplete = lst.PHQueueOverdueComplete.Value;
                    newObject.PHQueueCancel = lst.PHQueueCancel.Value;
                    newObject.PHQueueNotYet = lst.PHQueueNotYet.Value;
                    newObject.PHQueueDueComplete = lst.PHQueueDueComplete.Value;

                    return Json(newObject, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //GET CONFIG FROM STOREPROCEDURE
                    var config = db.usp_PHQueueConfig_Select().First();
                    //CALL STORE PROCEDURE
                    var lst = db.usp_PHQueueStatusCountAll_Select(config.DurationDays, null).ToList().First();

                    //Assign result to new object
                    newObject.PHQueueComplete = lst.PHQueueComplete.Value;
                    newObject.PHQueueOnProcess = lst.PHQueueOnProcess.Value;
                    newObject.PHQueueOverdue = lst.PHQueueOverdue.Value;
                    newObject.PHQueueOverdueComplete = lst.PHQueueOverdueComplete.Value;
                    newObject.PHQueueCancel = lst.PHQueueCancel.Value;
                    newObject.PHQueueNotYet = lst.PHQueueNotYet.Value;
                    newObject.PHQueueDueComplete = lst.PHQueueDueComplete.Value;

                    return Json(newObject, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// Status Count
        /// </summary>
        /// <param name="period"></param>
        /// <returns>Json list</returns>
        [HttpGet]
        public ActionResult Get_PHQueueCMConsiderCountAll(string period = "")
        {
            using (var db = new UnderwriteBranchEntities())
            {
                if (period != "")
                {
                    //CALL STORE PROCEDURE
                    var lst = db.usp_PHQueueCMConsiderCountAll_Select(null, DateTime.Parse(period)).ToList();

                    return Json(lst, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //GET CONFIG FROM STOREPROCEDURE
                    var config = db.usp_PHQueueConfig_Select().First();
                    //CALL STORE PROCEDURE
                    var lst = db.usp_PHQueueCMConsiderCountAll_Select(config.DurationDays, null).ToList();

                    return Json(lst, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// PHQueueCMResultCount
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public ActionResult Get_PHQueueCMResultCountAll(string period = "")
        {
            using (var db = new UnderwriteBranchEntities())
            {
                if (period != "")
                {
                    //CALL STORE PROCEDURE
                    var lst = db.usp_PHQueueCMResultCountAll_Select(null, DateTime.Parse(period)).ToList();

                    return Json(lst, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //GET CONFIG FROM STOREPROCEDURE
                    var config = db.usp_PHQueueConfig_Select().First();
                    //CALL STORE PROCEDURE
                    var lst = db.usp_PHQueueCMResultCountAll_Select(config.DurationDays, null).ToList();

                    return Json(lst, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// UnderwriteBranchResultCountAll
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get_PHQueueUnderwriteBranchResultCountAll_Select(string period = "")
        {
            using (var db = new UnderwriteBranchEntities())
            {
                //DECLARE NEW OBJECT
                var newObject = new UnderwriteBranchResult();

                if (period != "")
                {
                    //CALL STORE PROCEDURE
                    var lst = db.usp_PHQueueUnderwriteBranchResultCountAll_Select(null, DateTime.Parse(period)).ToList().First();

                    //Assign result to new object
                    newObject.UnderwriteBranchResultTrue = lst.UnderwriteBranchResultTrue.Value;
                    newObject.UnderwriteBranchResultFalse = lst.UnderwriteBranchResultFalse.Value;
                    newObject.UnderwriteBranchResultNA = lst.UnderwriteBranchResultNA.Value;

                    return Json(newObject, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //GET CONFIG FROM STOREPROCEDURE
                    var config = db.usp_PHQueueConfig_Select().First();
                    //CALL STORE PROCEDURE
                    var lst = db.usp_PHQueueUnderwriteBranchResultCountAll_Select(config.DurationDays, null).ToList().First();

                    //Assign result to new object
                    newObject.UnderwriteBranchResultTrue = lst.UnderwriteBranchResultTrue.Value;
                    newObject.UnderwriteBranchResultFalse = lst.UnderwriteBranchResultFalse.Value;
                    newObject.UnderwriteBranchResultNA = lst.UnderwriteBranchResultNA.Value;

                    return Json(newObject, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// Call Status
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get_PHQueueCallStatusCountAll(string period = "")
        {
            using (var db = new UnderwriteBranchEntities())
            {
                if (period != "")
                {
                    //Call storeprocedure
                    var lst = db.usp_PHQueueCallStatusCountAll_Select(null, DateTime.Parse(period)).ToList();

                    return Json(lst, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //GET CONFIG FROM STOREPROCEDURE
                    var config = db.usp_PHQueueConfig_Select().First();
                    //Call storeprocedure
                    var lst = db.usp_PHQueueCallStatusCountAll_Select(config.DurationDays, null).ToList();

                    return Json(lst, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get_AdminLineChartByDCR(string period = "")
        {
            using (var db = new UnderwriteBranchEntities())
            {
                if (period != "")
                {
                    //Call storeprocedure
                    var lst = db.usp_AdminLineChart_ByDCR(DateTime.Parse(period)).ToList();
                    return Json(lst, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //Call storeprocedure
                    var lst = db.usp_AdminLineChart_ByDuration().ToList();
                    return Json(lst, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="period"></param>
        /// <param name="search"></param>
        /// <param name="pageStart"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <returns>Json list</returns>
        [HttpPost]
        public ActionResult Get_DRPHQueueStudyAreaStatusCount(int draw, string search, int pageStart, int pageSize, string sortField, string orderType, string period = "")
        {
            using (var db = new UnderwriteBranchEntities())
            {
                if (period != "")
                {
                    //Call storeprocedure *DCR
                    var lst = db.usp_DRPHQueueStudyAreaStatusCount_Select(null, DateTime.Parse(period), search, pageStart, pageSize, sortField, orderType).ToList();

                    var dt = new Dictionary<string, object>
                    {
                        {"draw", draw},
                        {"recordsTotal", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                        {"recordsFiltered", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                        {"data", lst.ToList()}
                    };
                    return Json(dt, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //GET CONFIG FROM STOREPROCEDURE
                    var config = db.usp_PHQueueConfig_Select().First();
                    //Call storeprocedure *DCR
                    var lst = db.usp_DRPHQueueStudyAreaStatusCount_Select(config.DurationDays, null, search, pageStart,
                        pageSize, sortField, orderType).ToList();
                    var dt = new Dictionary<string, object>
                    {
                        {"draw", draw},
                        {"recordsTotal", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                        {"recordsFiltered", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                        {"data", lst.ToList()}
                    };
                    return Json(dt, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="period"></param>
        /// <param name="search"></param>
        /// <param name="pageStart"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <returns>Json list</returns>
        [HttpPost]
        public ActionResult Get_CMPHQueueStudyAreaStatusCount(int draw, string search, int pageStart, int pageSize, string sortField, string orderType, string period = "")
        {
            using (var db = new UnderwriteBranchEntities())
            {
                if (period != "")
                {
                    //Call storeprocedure *DCR
                    var lst = db.usp_CMPHQueueStudyAreaStatusCount_Select(null, DateTime.Parse(period), search, pageStart, pageSize, sortField, orderType).ToList();

                    var dt = new Dictionary<string, object>
                    {
                        {"draw", draw},
                        {"recordsTotal", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                        {"recordsFiltered", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                        {"data", lst.ToList()}
                    };
                    return Json(dt, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //GET CONFIG FROM STOREPROCEDURE
                    var config = db.usp_PHQueueConfig_Select().First();
                    //Call storeprocedure *DCR
                    var lst = db.usp_CMPHQueueStudyAreaStatusCount_Select(config.DurationDays, null, search, pageStart,
                        pageSize, sortField, orderType).ToList();
                    var dt = new Dictionary<string, object>
                    {
                        {"draw", draw},
                        {"recordsTotal", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                        {"recordsFiltered", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                        {"data", lst.ToList()}
                    };
                    return Json(dt, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// Export Excel
        /// </summary>
        /// <param name="period"></param>
        /// <param name="search"></param>
        public void ExportToExcel_Monitor(string search, string period = "")
        {
            using (var db = new UnderwriteBranchEntities())
            {
                var d = DateTime.Now;
                if (period != "")
                {
                    int i = 1;
                    //Call storeprocedure *DCR
                    var lst = db.usp_DRPHQueueStudyAreaStatusCount_Select(null, DateTime.Parse(period), search, 0, 999999, null, null)
                        .Select(o => new
                        {
                            ลำดับ = i++,
                            เขตพื้นที่ = o.StudyArea,
                            ผู้รับผิดชอบ = o.DirectorFullName,
                            Appทั้งหมด = o.PHQueueAll,
                            คัดกรองแล้วภายในกำหนด = o.PHQueueDueComplete,
                            ยังไม่ได้คัดกรอง = o.PHQueueOnProcess,
                            เกินกำหนด = o.PHQueueOverdue,
                            คัดกรองแล้วเกินกำหนด = o.PHQueueOverdueComplete,
                            ยกเลิก = o.PHQueueCancel,
                            ยังไม่ดำเนินการ = o.PHQueueNotYet,
                            คัดกรองแล้ว = o.PHQueueComplete
                        }).ToList();

                    ExcelManager.ExportToExcel(HttpContext, lst, "Sheet1", "ผอ.พท_สถานะรายการแต่ละพื้นที่_" + d);
                }
                else
                {
                    int i = 1;
                    //GET CONFIG FROM STOREPROCEDURE
                    var config = db.usp_PHQueueConfig_Select().First();
                    //Call storeprocedure *DCR
                    var lst = db.usp_DRPHQueueStudyAreaStatusCount_Select(config.DurationDays, null, search, 0, 999999, null, null)
                        .Select(o => new
                        {
                            ลำดับ = i++,
                            เขตพื้นที่ = o.StudyArea,
                            ผู้รับผิดชอบ = o.DirectorFullName,
                            Appทั้งหมด = o.PHQueueAll,
                            คัดกรองแล้วภายในกำหนด = o.PHQueueDueComplete,
                            ยังไม่ได้คัดกรอง = o.PHQueueOnProcess,
                            เกินกำหนด = o.PHQueueOverdue,
                            คัดกรองแล้วเกินกำหนด = o.PHQueueOverdueComplete,
                            ยกเลิก = o.PHQueueCancel,
                            ยังไม่ดำเนินการ = o.PHQueueNotYet,
                            คัดกรองแล้ว = o.PHQueueComplete
                        }).ToList();

                    ExcelManager.ExportToExcel(HttpContext, lst, "Sheet1", "ผอ.พท_สถานะรายการแต่ละพื้นที่_" + d);
                }
            }
        }

        /// <summary>
        /// Export Excel
        /// </summary>
        /// <param name="period"></param>
        /// <param name="search"></param>
        public void ExportToExcel_MonitorCM(string search, string period = "")
        {
            var d = DateTime.Now;
            using (var db = new UnderwriteBranchEntities())
            {
                if (period != "")
                {
                    int i = 1;

                    //Call storeprocedure *DCR
                    var lst = db.usp_CMPHQueueStudyAreaStatusCount_Select(null, DateTime.Parse(period), search, 0, 999999, null, null)
                        .Select(o => new
                        {
                            ลำดับ = i++,
                            เขตพื้นที่ = o.StudyArea,
                            ผู้รับผิดชอบ = o.DirectorFullName,
                            รหัสผู้อนุมัติคัดกรองสาขา = o.CMEmployeeCode,
                            ผู้อนุมัติคัดกรองสาขา = o.CMFullName,
                            Appทั้งหมด = o.PHQueueAll,
                            รอพิจารณา = o.CMPendingConsider,
                            พิจารณาแล้ว = o.CMCompleteConsider,
                            อนุมัติให้ผ่านคัดกรอง = o.CMApprove,
                            อนุมัติไม่ให้ผ่านคัดกรอง = o.CMNotApprove
                        }).ToList();

                    ExcelManager.ExportToExcel(HttpContext, lst, "Sheet1", "ประธานสาขา_สถานะรายการแต่ละพื้นที่_" + d);
                }
                else
                {
                    int i = 1;
                    //GET CONFIG FROM STOREPROCEDURE
                    var config = db.usp_PHQueueConfig_Select().First();
                    //Call storeprocedure *DCR
                    var lst = db.usp_CMPHQueueStudyAreaStatusCount_Select(config.DurationDays, null, search, 0, 999999, null, null)
                        .Select(o => new
                        {
                            ลำดับ = i++,
                            เขตพื้นที่ = o.StudyArea,
                            ผู้รับผิดชอบ = o.DirectorFullName,
                            รหัสผู้อนุมัติคัดกรองสาขา = o.CMEmployeeCode,
                            ผู้อนุมัติคัดกรองสาขา = o.CMFullName,
                            Appทั้งหมด = o.PHQueueAll,
                            รอพิจารณา = o.CMPendingConsider,
                            พิจารณาแล้ว = o.CMCompleteConsider,
                            อนุมัติให้ผ่านคัดกรอง = o.CMApprove,
                            อนุมัติไม่ให้ผ่านคัดกรอง = o.CMNotApprove
                        }).ToList();

                    ExcelManager.ExportToExcel(HttpContext, lst, "Sheet1", "ประธานสาขา_สถานะรายการแต่ละพื้นที่_" + d);
                }
            }
        }

        /// <summary>
        /// Export Excel
        /// </summary>
        /// <param name="period"></param>
        /// <param name="search"></param>
        public void ExportToExcel_MonitorCM2(string search, string period = "")
        {
            var d = DateTime.Now;
            using (var db = new UnderwriteBranchEntities())
            {
                if (period != "")
                {
                    int i = 1;

                    //Call storeprocedure *DCR
                    var lst = db.usp_CMPHQueueStudyAreaStatusCountV2_Select(null, DateTime.Parse(period), search, 0, 999999, null, null)
                        .Select(o => new
                        {
                            ลำดับ = i++,
                            ผู้รับผิดชอบ = o.CMFullName,
                            รหัสผู้อนุมัติคัดกรองสาขา = o.CMEmployeeCode,
                            ผู้อนุมัติคัดกรองสาขา = o.CMFullName,
                            Appทั้งหมด = o.PHQueueAll,
                            รอพิจารณา = o.CMPendingConsider,
                            พิจารณาแล้ว = o.CMCompleteConsider,
                            อนุมัติให้ผ่านคัดกรอง = o.CMApprove,
                            อนุมัติไม่ให้ผ่านคัดกรอง = o.CMNotApprove
                        }).ToList();

                    ExcelManager.ExportToExcel(HttpContext, lst, "Sheet1", "ประธานสาขา_สถานะรายการแยกตามรายบุคคล_" + d);
                }
                else
                {
                    int i = 1;
                    //GET CONFIG FROM STOREPROCEDURE
                    var config = db.usp_PHQueueConfig_Select().First();
                    //Call storeprocedure *DCR
                    var lst = db.usp_CMPHQueueStudyAreaStatusCountV2_Select(config.DurationDays, null, search, 0, 999999, null, null)
                        .Select(o => new
                        {
                            ลำดับ = i++,
                            ผู้รับผิดชอบ = o.CMFullName,
                            รหัสผู้อนุมัติคัดกรองสาขา = o.CMEmployeeCode,
                            ผู้อนุมัติคัดกรองสาขา = o.CMFullName,
                            Appทั้งหมด = o.PHQueueAll,
                            รอพิจารณา = o.CMPendingConsider,
                            พิจารณาแล้ว = o.CMCompleteConsider,
                            อนุมัติให้ผ่านคัดกรอง = o.CMApprove,
                            อนุมัติไม่ให้ผ่านคัดกรอง = o.CMNotApprove
                        }).ToList();

                    ExcelManager.ExportToExcel(HttpContext, lst, "Sheet1", "ประธานสาขา_สถานะรายการแยกตามรายบุคคล_" + d);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="search"></param>
        public void ExportToExcel_AdminConfig(string search)
        {
            using (var db = new UnderwriteBranchEntities())
            {
                int i = 1;
                var d = DateTime.Now;
                var lst = db.usp_StudyAreaUserConfigExcel_Select(search)
                    .Select(o => new
                    {
                        ลำดับ = i++,
                        เขตพื้นที่ = o.StudyArea,
                        รหัสพนักงาน = o.EmployeeCode,
                        ผู้ดูแลคิว = o.UserFullName
                    }).ToList();

                ExcelManager.ExportToExcel(HttpContext, lst, "Sheet1", "Admin_Configuration_" + d);
            }
        }

        public void ExportToExcel_PHQueueDetail(string period = "")
        {
            using (var db = new UnderwriteBranchEntities())
            {
                if (period == "")
                {
                    var d = DateTime.Now;
                    //GET CONFIG FROM STOREPROCEDURE
                    var config = db.usp_PHQueueConfig_Select().First();

                    var lst = db.usp_PHQueueDetailExcel_Select(config.DurationDays, null).Select(o => new
                    {
                        o.PHQueueId,
                        o.QueueCode,
                        เขตพื้นที่ = o.StudyArea,
                        รหัสผู้ดูแลคิวงาน = o.AssignToUserEmployeeCode,
                        ผู้ดูแลคิวงาน = o.AssignToUserFullName,
                        รหัสประธานสาขา = o.รหัสประธานสาขา__CMEmployeeCode_,
                        ชื่อประธานสาขา = o.ชื่อประธานสาขา__CMFullName_,
                        งวดความคุ้มครอง = o.Period,
                        o.ApplicationCode,
                        สถานะApplication = o.ApplicationStatus,
                        วันเริ่มคุ้มครอง = o.StartCoverDate,
                        ชื่อลูกค้า = o.CustomerFullName,
                        วันเกิดลูกค้า = o.CustomerBirthDate,
                        เบอร์โทรศัพท์ลูกค้า = o.CustomerMobileNo,
                        ความสัมพันธ์ = o.PayerRelate,
                        ชื่อผู้ชำระเบี้ย = o.PayerFullName,
                        ที่ทำงานผู้ชำระเบี้ย = o.PayerOfficeName,
                        เบอร์โทรศัพท์ผู้ชำระเบี้ย = o.PayerTelephoneNo,
                        เบอร์โทรศัพท์ที่ทำงานผู้ชำระเบี้ย = o.PayerOfficeTelephoneNo,
                        สาขาผู้ชำระเบี้ย = o.BranchDetail,
                        รหัสตัวแทน = o.Sale_id1,
                        ชื่อตัวแทน = o.Sale_Name,
                        สาขาตัวแทน = o.SaleBranch,
                        สถานะคิวงาน = o.PHQueueStatus,
                        วันที่สร้างคิวงาน = o.CreatedDate,
                        วันหมดอายุคิวงาน = o.QueueExpireDate,
                        วันที่ส่งคิวงาน = o.AssignDate,
                        สถานะคัดกรองสาขา = o.สถานะคัดกรองสาขา__UnderwriteBranchResult_,
                        ช่องทางการเข้าพบ = o.QueueTypeDetail,
                        สถานะการโทร = o.CallStatus,
                        คัดกรองลูกค้า = o.คัดกรองลูกค้า__UnderwriteCustomer_,
                        คัดกรองผู้ชำระเบี้ย = o.คัดกรองผู้ชำระเบี้ย__UnderwritePayer_,
                        สถานะการเข้าพบ = o.สถานะการเข้าพบ__IsFoundCustomer_,
                        //การยินยอมให้หักบัญชีธนาคาร = o.การยินยอมให้หักบัญชีธนาคาร__IsBankAccountAllow_,
                        //หมายเหตุการยินยอมให้หักบัญชีธนาคาร = o.หมายเหตุการยินยอมให้หักบัญชีธนาคาร__BankAccountAllowRemark_,
                        การชำระเบี้ย = o.การชำระเบี้ย__UWPaymentTypeDetail_,
                        ผลการคัดกรองสาขาชุดคำถามเก่า = o.ผลการคัดกรองสาขา_ผ่าน_ไม่ผ่าน__IsResultPass_,
                        ผ่านตามมาตรฐานทุกประการ = o.ผ่านตามมาตรฐานทุกประการ__ResultPassStandard_,
                        ผ่านมีประวัติสุขภาพติดข้อยกเว้น = o.ผ่านมีประวัติสุขภาพติดข้อยกเว้น__ResultPassCondition_,
                        ระบุในใบสมัครหรือไม่ = o.ระบุในใบสมัครหรือไม่___ResultPassIsSpecify_,
                        หมายเหตุผ่านติดข้อยกเว้น = o.หมายเหตุผ่านติดข้อยกเว้น__ResultPassConditionRemark_,
                        ไม่ผ่านด้านสุขภาพ = o.ไม่ผ่านด้านสุขภาพ__DenyHealth_,
                        ไม่ผ่านด้านอาชีพ = o.ไม่ผ่านด้านอาชีพ__DenyOccupation_,
                        ไม่ผ่านไม่สามารถชำระเบี้ยตามเงื่อนไขได้ = o.ไม่ผ่านไม่สามารถชำระเบี้ยตามเงื่อนไขได้__DenyCantPay_,
                        ไม่ผ่านอื่นๆ = o.ไม่ผ่านอื่น_ๆ__DenyOther_,
                        หมายเหตุไม่ผ่านอื่นๆ = o.หมายเหตุไม่ผ่านอื่น_ๆ__DenyOtherRemark_,
                        หมายเหตุผลการคัดกรองสาขา = o.หมายเหตุผลการคัดกรองสาขา__ResultRemark_,
                        ผู้บันทึกผลคัดกรองสาขา = o.ผู้บันทึกผลการคัดกรองสาขา__ClosedByUserFullName_,
                        วันที่บันทึกผลคัดกรองสาขา = o.วันที่บันทึกผลการคัดกรองสาขา__ClosedDate_,
                        จำนวนวันที่เกินกำหนดบันทึกผลคัดกรองสาขา = o.จำนวนวันที่เกินกำหนด,
                        ผลการพิจารณาของประธานสาขา = o.ผลการพิจารณาของประธานสาขา__CMIsApprove_,
                        อนุมัติให้ผ่านคัดกรองแบบติดเงื่อนไข = o.อนุมัติให้ผ่านคัดกรองแบบติดเงื่อนไข__CMIsApprovePassCondition_,
                        หมายเหตุการอนุมัติ = o.หมายเหตุการอนุมัติ__CMRemark_,
                        ผู้อนุมัติ = o.ผู้อนุมัติ__ConsiderByFullName_,
                        วันที่บันทึกผลการพิจารณาของประธานสาขา = o.วันที่บันทึกผลการพิจารณาของประธานสาขา__CMIApproveDate_,
                        CurrentApplicationStatus = o.CurrentApplicationStatus,
                        SSSสถานะคัดกรองSSSUnderwriteStatus = o.SSS_สถานะคัดกรอง__SSS_UnderwriteStatus_,
                        ผลการคัดกรองสำนักงานใหญ่ = o.ผลการคัดกรองสำนักงานใหญ่_ผ่าน_ไม่ผ่าน__UnderwriteResult_,
                        หมายเหตุผลการคัดกรองสำนักงานใหญ่ = o.หมายเหตุผลการคัดกรองสำนักงานใหญ่__UnderwriteRemark_,
                        วันที่บันทึกผลการคัดกรองสำนักงานใหญ่ = o.วันที่บันทึกผลการคัดกรองสำนักงานใหญ่__UnderwriteDate_
                    }).ToList();

                    ExcelManager.ExportToExcel(HttpContext, lst, "Sheet1", "QueueDetail_02_DATE" + d);
                }
                else
                {
                    var d = DateTime.Now;

                    var lst = db.usp_PHQueueDetailExcel_Select(null, DateTime.Parse(period)).Select(o => new
                    {
                        //o.PHQueueId,
                        //o.QueueCode,
                        //เขตพื้นที่ = o.StudyArea,
                        //รหัสผู้ดูแลคิวงาน = o.AssignToUserEmployeeCode,
                        //ผู้ดูแลคิวงาน = o.AssignToUserFullName,
                        //รหัสประธานสาขา = o.รหัสประธานสาขา__CMEmployeeCode_,
                        //ชื่อประธานสาขา = o.ชื่อประธานสาขา__CMFullName_,
                        //งวดความคุ้มครอง = o.Period,
                        //o.ApplicationCode,
                        //สถานะApplication = o.ApplicationStatus,
                        //วันเริ่มคุ้มครอง = o.StartCoverDate,
                        //ชื่อลูกค้า = o.CustomerFullName,
                        //วันเกิดลูกค้า = o.CustomerBirthDate,
                        //เบอร์โทรศัพท์ลูกค้า = o.CustomerMobileNo,
                        //ความสัมพันธ์ = o.PayerRelate,
                        //ชื่อผู้ชำระเบี้ย = o.PayerFullName,
                        //ที่ทำงานผู้ชำระเบี้ย = o.PayerOfficeName,
                        //เบอร์โทรศัพท์ผู้ชำระเบี้ย = o.PayerTelephoneNo,
                        //เบอร์โทรศัพท์ที่ทำงานผู้ชำระเบี้ย = o.PayerOfficeTelephoneNo,
                        //วันที่สร้างคิวงาน = o.CreatedDate,
                        //วันที่บันทึกผลการคัดกรองสาขา = o.วันที่บันทึกผลการคัดกรองสาขา__ClosedDate_,
                        //วันที่ส่งคิวงาน = o.AssignDate,
                        //วันหมดอายุคิวงาน = o.QueueExpireDate,
                        //จำนวนวันที่เกินกำหนด = o.จำนวนวันที่เกินกำหนด,
                        //CurrentApplicationStatus = o.CurrentApplicationStatus,
                        //SSSสถานะคัดกรองSSSUnderwriteStatus = o.SSS_สถานะคัดกรอง__SSS_UnderwriteStatus_,
                        //ผลการคัดกรองสำนักงานใหญ่ = o.ผลการคัดกรองสำนักงานใหญ่_ผ่าน_ไม่ผ่าน__UnderwriteResult_,
                        //หมายเหตุผลการคัดกรองสำนักงานใหญ่ = o.หมายเหตุผลการคัดกรองสำนักงานใหญ่__UnderwriteRemark_,
                        //วันที่บันทึกผลการคัดกรองสำนักงานใหญ่ = o.วันที่บันทึกผลการคัดกรองสำนักงานใหญ่__UnderwriteDate_,
                        //สถานะคัดกรองสาขา = o.สถานะคัดกรองสาขา__UnderwriteBranchResult_,
                        //สถานะคิวงาน = o.PHQueueStatus,
                        //ช่องทางการเข้าพบ = o.QueueTypeDetail,
                        //สถานะการโทร = o.CallStatus,
                        //คัดกรองลูกค้า = o.คัดกรองลูกค้า__UnderwriteCustomer_,
                        //คัดกรองผู้ชำระเบี้ย = o.คัดกรองผู้ชำระเบี้ย__UnderwritePayer_,
                        //สถานะการเข้าพบ = o.สถานะการเข้าพบ__IsFoundCustomer_,
                        ////การยินยอมให้หักบัญชีธนาคาร = o.การยินยอมให้หักบัญชีธนาคาร__IsBankAccountAllow_,
                        ////หมายเหตุการยินยอมให้หักบัญชีธนาคาร = o.หมายเหตุการยินยอมให้หักบัญชีธนาคาร__BankAccountAllowRemark_,
                        //การชำระเบี้ย = o.การชำระเบี้ย__UWPaymentTypeDetail_,
                        //ผ่านตามมาตรฐานทุกประการ = o.ผ่านตามมาตรฐานทุกประการ__ResultPassStandard_,
                        //ผ่านมีประวัติสุขภาพติดข้อยกเว้น = o.ผ่านมีประวัติสุขภาพติดข้อยกเว้น__ResultPassCondition_,
                        //หมายเหตุผ่านติดข้อยกเว้น = o.หมายเหตุผ่านติดข้อยกเว้น__ResultPassConditionRemark_,
                        ////ผลการคัดกรองสาขา = o.ผลการคัดกรองสาขา_ผ่าน_ไม่ผ่าน__IsResultPass_,
                        //ไม่ผ่านด้านสุขภาพ = o.ไม่ผ่านด้านสุขภาพ__DenyHealth_,
                        //ไม่ผ่านด้านอาชีพ = o.ไม่ผ่านด้านอาชีพ__DenyOccupation_,
                        //ไม่ผ่านไม่สามารถชำระเบี้ยตามเงื่อนไขได้ = o.ไม่ผ่านไม่สามารถชำระเบี้ยตามเงื่อนไขได้__DenyCantPay_,
                        //ไม่ผ่านอื่นๆ = o.ไม่ผ่านอื่น_ๆ__DenyOther_,
                        //หมายเหตุกรณีไม่ผ่านคัดกรองสาขา = o.หมายเหตุกรณีไม่ผ่านคัดกรองสาขา__ResultRemark_,
                        //ผลการพิจารณาของประธานสาขา = o.ผลการพิจารณาของประธานสาขา__CMIsApprove_,
                        //หมายเหตุการอนุมัติ = o.หมายเหตุการอนุมัติ__CMRemark_,
                        //วันที่บันทึกผลการพิจารณาของประธานสาขา = o.วันที่บันทึกผลการพิจารณาของประธานสาขา__CMIApproveDate_,
                        //ผู้อนุมัติ = o.ผู้อนุมัติ__ConsiderByFullName_,
                        //รหัสตัวแทน = o.Sale_id1,
                        //ชื่อตัวแทน = o.Sale_Name,
                        //สาขาตัวแทน = o.SaleBranch
                        o.PHQueueId,
                        o.QueueCode,
                        เขตพื้นที่ = o.StudyArea,
                        รหัสผู้ดูแลคิวงาน = o.AssignToUserEmployeeCode,
                        ผู้ดูแลคิวงาน = o.AssignToUserFullName,
                        รหัสประธานสาขา = o.รหัสประธานสาขา__CMEmployeeCode_,
                        ชื่อประธานสาขา = o.ชื่อประธานสาขา__CMFullName_,
                        งวดความคุ้มครอง = o.Period,
                        o.ApplicationCode,
                        สถานะApplication = o.ApplicationStatus,
                        วันเริ่มคุ้มครอง = o.StartCoverDate,
                        ชื่อลูกค้า = o.CustomerFullName,
                        วันเกิดลูกค้า = o.CustomerBirthDate,
                        เบอร์โทรศัพท์ลูกค้า = o.CustomerMobileNo,
                        ความสัมพันธ์ = o.PayerRelate,
                        ชื่อผู้ชำระเบี้ย = o.PayerFullName,
                        ที่ทำงานผู้ชำระเบี้ย = o.PayerOfficeName,
                        เบอร์โทรศัพท์ผู้ชำระเบี้ย = o.PayerTelephoneNo,
                        เบอร์โทรศัพท์ที่ทำงานผู้ชำระเบี้ย = o.PayerOfficeTelephoneNo,
                        สาขาผู้ชำระเบี้ย = o.BranchDetail,
                        รหัสตัวแทน = o.Sale_id1,
                        ชื่อตัวแทน = o.Sale_Name,
                        สาขาตัวแทน = o.SaleBranch,
                        สถานะคิวงาน = o.PHQueueStatus,
                        วันที่สร้างคิวงาน = o.CreatedDate,
                        วันหมดอายุคิวงาน = o.QueueExpireDate,
                        วันที่ส่งคิวงาน = o.AssignDate,
                        สถานะคัดกรองสาขา = o.สถานะคัดกรองสาขา__UnderwriteBranchResult_,
                        ช่องทางการเข้าพบ = o.QueueTypeDetail,
                        สถานะการโทร = o.CallStatus,
                        คัดกรองลูกค้า = o.คัดกรองลูกค้า__UnderwriteCustomer_,
                        คัดกรองผู้ชำระเบี้ย = o.คัดกรองผู้ชำระเบี้ย__UnderwritePayer_,
                        สถานะการเข้าพบ = o.สถานะการเข้าพบ__IsFoundCustomer_,
                        //การยินยอมให้หักบัญชีธนาคาร = o.การยินยอมให้หักบัญชีธนาคาร__IsBankAccountAllow_,
                        //หมายเหตุการยินยอมให้หักบัญชีธนาคาร = o.หมายเหตุการยินยอมให้หักบัญชีธนาคาร__BankAccountAllowRemark_,
                        การชำระเบี้ย = o.การชำระเบี้ย__UWPaymentTypeDetail_,
                        ผลการคัดกรองสาขาชุดคำถามเก่า = o.ผลการคัดกรองสาขา_ผ่าน_ไม่ผ่าน__IsResultPass_,
                        ผ่านตามมาตรฐานทุกประการ = o.ผ่านตามมาตรฐานทุกประการ__ResultPassStandard_,
                        ผ่านมีประวัติสุขภาพติดข้อยกเว้น = o.ผ่านมีประวัติสุขภาพติดข้อยกเว้น__ResultPassCondition_,
                        ระบุในใบสมัครหรือไม่ = o.ระบุในใบสมัครหรือไม่___ResultPassIsSpecify_,
                        หมายเหตุผ่านติดข้อยกเว้น = o.หมายเหตุผ่านติดข้อยกเว้น__ResultPassConditionRemark_,
                        ไม่ผ่านด้านสุขภาพ = o.ไม่ผ่านด้านสุขภาพ__DenyHealth_,
                        ไม่ผ่านด้านอาชีพ = o.ไม่ผ่านด้านอาชีพ__DenyOccupation_,
                        ไม่ผ่านไม่สามารถชำระเบี้ยตามเงื่อนไขได้ = o.ไม่ผ่านไม่สามารถชำระเบี้ยตามเงื่อนไขได้__DenyCantPay_,
                        ไม่ผ่านอื่นๆ = o.ไม่ผ่านอื่น_ๆ__DenyOther_,
                        หมายเหตุไม่ผ่านอื่นๆ = o.หมายเหตุไม่ผ่านอื่น_ๆ__DenyOtherRemark_,
                        หมายเหตุผลการคัดกรองสาขา = o.หมายเหตุผลการคัดกรองสาขา__ResultRemark_,
                        ผู้บันทึกผลคัดกรองสาขา = o.ผู้บันทึกผลการคัดกรองสาขา__ClosedByUserFullName_,
                        วันที่บันทึกผลคัดกรองสาขา = o.วันที่บันทึกผลการคัดกรองสาขา__ClosedDate_,
                        จำนวนวันที่เกินกำหนดบันทึกผลคัดกรองสาขา = o.จำนวนวันที่เกินกำหนด,
                        ผลการพิจารณาของประธานสาขา = o.ผลการพิจารณาของประธานสาขา__CMIsApprove_,
                        อนุมัติให้ผ่านคัดกรองแบบติดเงื่อนไข = o.อนุมัติให้ผ่านคัดกรองแบบติดเงื่อนไข__CMIsApprovePassCondition_,
                        หมายเหตุการอนุมัติ = o.หมายเหตุการอนุมัติ__CMRemark_,
                        ผู้อนุมัติ = o.ผู้อนุมัติ__ConsiderByFullName_,
                        วันที่บันทึกผลการพิจารณาของประธานสาขา = o.วันที่บันทึกผลการพิจารณาของประธานสาขา__CMIApproveDate_,
                        CurrentApplicationStatus = o.CurrentApplicationStatus,
                        SSSสถานะคัดกรองSSSUnderwriteStatus = o.SSS_สถานะคัดกรอง__SSS_UnderwriteStatus_,
                        ผลการคัดกรองสำนักงานใหญ่ = o.ผลการคัดกรองสำนักงานใหญ่_ผ่าน_ไม่ผ่าน__UnderwriteResult_,
                        หมายเหตุผลการคัดกรองสำนักงานใหญ่ = o.หมายเหตุผลการคัดกรองสำนักงานใหญ่__UnderwriteRemark_,
                        วันที่บันทึกผลการคัดกรองสำนักงานใหญ่ = o.วันที่บันทึกผลการคัดกรองสำนักงานใหญ่__UnderwriteDate_
                    }).ToList();

                    ExcelManager.ExportToExcel(HttpContext, lst, "Sheet1", "QueueDetail_01_DATE" + d);
                }
            }
        }

        /// <summary>
        /// ค้นหา
        /// </summary>
        /// <param name="searchType"></param>
        /// <param name="draw"></param>
        /// <param name="search"></param>
        /// <param name="period"></param>
        /// <param name="pageStart"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public ActionResult GetDatatablesConsult(int searchType, int draw, string search, int pageStart, int pageSize, string sortField, string orderType)
        {
            using (var db = new UnderwriteBranchEntities())
            {
                switch (searchType)
                {
                    case 1:
                        //ค้นหาคิวงานที่ปรึกษา
                        var lst = db.usp_PHQueueConsult_Select(search, pageStart, pageSize, sortField, orderType).ToList();

                        var dt = new Dictionary<string, object>
                        {
                            {"draw",draw },
                            {"recordsTotal", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                            {"recordsFiltered", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                            {"data", lst.ToList()}
                        };
                        return Json(dt, JsonRequestBehavior.AllowGet);

                    case 2:
                        //ค้นหาคิวงานทั้งหมด
                        var lst2 = db.usp_PHQueue_Select(null, null, null, null, null, search, null, pageStart, pageSize, sortField, orderType).ToList();

                        var dt2 = new Dictionary<string, object>
                        {
                            {"draw",draw },
                            {"recordsTotal", lst2.Count() != 0 ? lst2.FirstOrDefault()?.TotalCount : lst2.Count()},
                            {"recordsFiltered", lst2.Count() != 0 ? lst2.FirstOrDefault()?.TotalCount : lst2.Count()},
                            {"data", lst2.ToList()}
                        };
                        return Json(dt2, JsonRequestBehavior.AllowGet);

                    default:
                        return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// รายละเอียดข้อมูล การบันทึกข้อความ ขอคำปรึกษา
        /// </summary>
        /// <param name="PHQueueId"></param>
        /// <param name="draw"></param>
        /// <param name="search"></param>
        /// <param name="pageStart"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public ActionResult GetMessageDetailConsult(int PHQueueId, int draw, string search, int pageStart, int pageSize, string sortField, string orderType)
        {
            using (var db = new UnderwriteBranchEntities())
            {
                var lst = db.usp_PHQueueConsultByPHQueueId_Select(PHQueueId, search, pageStart, pageSize, sortField, orderType).ToList();

                var dt = new Dictionary<string, object>
                        {
                            {"draw",draw },
                            {"recordsTotal", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                            {"recordsFiltered", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                            {"data", lst.ToList()}
                        };
                return Json(dt, JsonRequestBehavior.AllowGet);
            }
        }
    }
}