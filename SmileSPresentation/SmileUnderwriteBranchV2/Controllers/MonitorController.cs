using SmileUnderwriteBranchV2.Helper;
using SmileUnderwriteBranchV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SmileUnderwriteBranchV2.Controllers
{
    [Authorization]
    public class MonitorController : Controller
    {
        #region Constructor

        private readonly UnderwriteBranchV2Entities _context;

        public MonitorController()
        {
            _context = new UnderwriteBranchV2Entities();
        }

        #endregion Constructor

        #region View

        /// <summary>
        /// GET: Monitor
        /// </summary>
        /// <returns></returns>
        [Authorization(Roles = "Developer,UnderwriteV2_BusinesPromoteTeam")]
        public ActionResult MonitorSupervisor()
        {
            var periodList = GlobalFunction.GetPeriodList(Properties.Settings.Default.ChangePeriodDay, 12);
            var area = _context.usp_Area_Select(null).ToList();

            ViewBag.Area = area;
            ViewBag.PeriodList = periodList;
            ViewBag.Config = 10;

            return View();
        }

        /// <summary>
        /// GET: SupervisorQueue
        /// </summary>
        /// <param name="period"></param>
        /// <param name="branch"></param>
        /// <param name="district"></param>
        /// <returns></returns>
        [Authorization(Roles = "Developer,UnderwriteV2_BusinesPromoteTeam")]
        public ActionResult SupervisorQueue(string period = "", string branch = "", string district = "")
        {
            var queueStatus = _context.usp_QueueStatus_Select(null).ToList();
            ViewBag.QueueStatus = queueStatus;

            var periodDecode = GlobalFunction.Base64StringDecode(period);
            ViewBag.Period = periodDecode;
            ViewBag.PeriodText = DateTime.TryParse(periodDecode, out var date) ? date.ToString("dd/MM/yyyy") : periodDecode;

            if (!string.IsNullOrEmpty(branch))
            {
                try
                {
                    var branchDecode = GlobalFunction.Base64StringDecode(branch);
                    ViewBag.Branch = GetBranch(int.TryParse(branchDecode, out var branchId) ? branchId : 000);
                }
                catch (Exception)
                {
                    ViewBag.Branch = null;
                }
            }

            if (!string.IsNullOrEmpty(district))
            {
                try
                {
                    var districtDecode = GlobalFunction.Base64StringDecode(district);

                    //District Id
                    ViewBag.District = districtDecode;

                    //District Detail , Province Detail
                    ViewBag.Address = GetAddress(int.TryParse(districtDecode, out var districtId) ? districtId : 000);
                }
                catch (Exception)
                {
                    ViewBag.District = null;
                }
            }

            return View();
        }

        /// <summary>
        /// GET: SupervisorQueueStatusDetail
        /// </summary>
        /// <param name="period"></param>
        /// <param name="branch"></param>
        /// <returns></returns>
        [Authorization(Roles = "Developer,UnderwriteV2_BusinesPromoteTeam")]
        public ActionResult SupervisorQueueStatusDetail(string period = "", string branch = "")
        {
            if (string.IsNullOrEmpty(period) || string.IsNullOrEmpty(branch)) return null;

            try
            {
                var periodDecode = GlobalFunction.Base64StringDecode(period);
                var branchDecode = GlobalFunction.Base64StringDecode(branch);
                ViewBag.Period = periodDecode;
                ViewBag.Branch = GetBranch(int.TryParse(branchDecode, out var branchId) ? branchId : 000);
                ViewBag.PeriodText = DateTime.TryParse(periodDecode, out var date) ? date.ToString("dd/MM/yyyy") : periodDecode;
            }
            catch (Exception)
            {
                ViewBag.Period = null;
                ViewBag.Branch = null;
            }

            return View();
        }

        #endregion View

        #region Method

        /// <summary>
        /// สาขา
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        private usp_Branch_Select_Result GetBranch(int? Id = null)
        {
            //CALL STORE PROCEDURE
            var branches = _context.usp_Branch_Select(branchId: Id).FirstOrDefault();

            return branches;
        }

        /// <summary>
        /// ที่อยู่ โดย อำเภอ
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>
        private usp_AddressByDistrictId_Select_Result GetAddress(int district)
        {
            //CALL STORE PROCEDURE
            var address = _context.usp_AddressByDistrictId_Select(districtId: district).FirstOrDefault();

            return address;
        }

        /// <summary>
        /// ภาค
        /// </summary>
        /// <param name="Id">รหัสภาค</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetArea(int? Id = null)
        {
            //CALL STORE PROCEDURE
            var areas = _context.usp_Area_Select(areaId: Id).ToList();

            return Json(areas, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// สาขา โดย ภาค
        /// </summary>
        /// <param name="area">รหัสภาค</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BranchByArea(int? area = null)
        {
            //CALL STORE PROCEDURE
            var branches = _context.usp_BranchByAreaId_Select(areaId: (area == -1 ? null : area)).ToList();

            return Json(branches, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// สถานะคิวงาน
        /// </summary>
        /// <param name="period"></param>
        /// <param name="area"></param>
        /// <param name="branch"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult QueueStatus(string period, int? area, int? branch)
        {
            //CALL STORE PROCEDURE
            var obj = _context.usp_pivotQueueStatus_Select(appStartCoverDate: DateTime.TryParse(period, out var date) ? date : (DateTime?)null, payerStudyAreaId: null, payerBranchId: (branch == -1 ? null : branch), areaId: (area == -1 ? null : area)).FirstOrDefault();

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// สถานะคัดกรอง
        /// </summary>
        /// <param name="period"></param>
        /// <param name="area"></param>
        /// <param name="branch"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult QueueUnderwriteStatus(string period, int? area, int? branch)
        {
            //CALL STORE PROCEDURE
            var obj = _context.usp_pivotQueueUnderwriteStatus_Select(appStartCoverDate: DateTime.TryParse(period, out var date) ? date : (DateTime?)null, payerStudyAreaId: null, payerBranchId: (branch == -1 ? null : branch), areaId: (area == -1 ? null : area)).FirstOrDefault();

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// สถานะการพิจารณาของประธานสาขา
        /// </summary>
        /// <param name="period"></param>
        /// <param name="area"></param>
        /// <param name="branch"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult QueueApproveStatusV1(string period, int? area, int? branch)
        {
            //CALL STORE PROCEDURE
            var obj = _context.usp_pivotQueueApproveStatusV1_Select(appStartCoverDate: DateTime.TryParse(period, out var date) ? date : (DateTime?)null, payerStudyAreaId: null, payerBranchId: (branch == -1 ? null : branch), areaId: (area == -1 ? null : area)).FirstOrDefault();

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ผลการพิจารณาของประธานสาขา
        /// </summary>
        /// <param name="period"></param>
        /// <param name="area"></param>
        /// <param name="branch"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult QueueApproveStatusV2(string period, int? area, int? branch)
        {
            //CALL STORE PROCEDURE
            var obj = _context.usp_pivotQueueApproveStatusV2_Select(appStartCoverDate: DateTime.TryParse(period, out var date) ? date : (DateTime?)null, payerStudyAreaId: null, payerBranchId: (branch == -1 ? null : branch), areaId: (area == -1 ? null : area)).FirstOrDefault();

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ช่องทางการคัดกรอง
        /// </summary>
        /// <param name="period"></param>
        /// <param name="area"></param>
        /// <param name="branch"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult QueueUnderwriteType(string period, int? area, int? branch)
        {
            //CALL STORE PROCEDURE
            var obj = _context.usp_pivotQueueUnderwriteType_Select(appStartCoverDate: DateTime.TryParse(period, out var date) ? date : (DateTime?)null, payerStudyAreaId: null, payerBranchId: (branch == -1 ? null : branch), areaId: (area == -1 ? null : area)).FirstOrDefault();

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// สถานะมอบบัตร
        /// </summary>
        /// <param name="period"></param>
        /// <param name="area"></param>
        /// <param name="branch"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult QueueCobrand(string period, int? area, int? branch)
        {
            //CALL STORE PROCEDURE
            var obj = _context.usp_pivotQueueCobrand_Select(appStartCoverDate: DateTime.TryParse(period, out var date) ? date : (DateTime?)null, payerStudyAreaId: null, payerBranchId: (branch == -1 ? null : branch), areaId: (area == -1 ? null : area)).FirstOrDefault();

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ระยะเวลามอบบัตร
        /// </summary>
        /// <param name="period"></param>
        /// <param name="area"></param>
        /// <param name="branch"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult QueueCobrandDuration(string period, int? area, int? branch)
        {
            //CALL STORE PROCEDURE
            var obj = _context.usp_pivotQueueCobrandDuration_Select(appStartCoverDate: DateTime.TryParse(period, out var date) ? date : (DateTime?)null, payerStudyAreaId: null, payerBranchId: (branch == -1 ? null : branch), areaId: (area == -1 ? null : area)).FirstOrDefault();

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  ประเภทการมอบบัตร
        /// </summary>
        /// <param name="period"></param>
        /// <param name="area"></param>
        /// <param name="branch"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult QueueCobrandGiveType(string period, int? area, int? branch)
        {
            //CALL STORE PROCEDURE
            var obj = _context.usp_pivotQueueCobrandGiveType_Select(appStartCoverDate: DateTime.TryParse(period, out var date) ? date : (DateTime?)null, payerStudyAreaId: null, payerBranchId: (branch == -1 ? null : branch), areaId: (area == -1 ? null : area)).FirstOrDefault();

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        #endregion Method

        #region Datatable

        /// <summary>
        /// Datatable สถานะคิวงานแบ่งตามพื้นที่ ประธานสาขา
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="period"></param>
        /// <param name="area"></param>
        /// <param name="branch"></param>
        /// <param name="indexStart"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SupervisorQueueApproveStatusDT(int draw, string period, int? area, int? branch, int? indexStart, int? pageSize, string sortField, string orderType, string search)
        {
            try
            {
                var result = _context.usp_pivotSupervisorQueueApproveStatus_Select(appStartCoverDate: DateTime.TryParse(period, out var date) ? date : (DateTime?)null,
                                                                                   payerStudyAreaId: null,
                                                                                   payerBranchId: (branch == -1 ? null : branch),
                                                                                   indexStart: indexStart,
                                                                                   pageSize: pageSize,
                                                                                   sortField: sortField,
                                                                                   orderType: orderType,
                                                                                   searchDetail: search,
                                                                                   areaId: (area == -1 ? null : area)).ToList();

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
        ///  Supervisor Queue รายละเอียดคิวงานของสาขา
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="period"></param>
        /// <param name="queueStatus"></param>
        /// <param name="district"></param>
        /// <param name="branch"></param>
        /// <param name="indexStart"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SupervisorQueueDT(int draw, string period, int? queueStatus, int? district, int? branch, int? indexStart, int? pageSize, string sortField, string orderType, string search)
        {
            try
            {
                var result = _context.usp_SupervisorQueue_Select(appStartCoverDate: DateTime.TryParse(period, out var date) ? date : (DateTime?)null,
                                                 branchId: (branch == null && district == null) ? -1 : branch,
                                                 districtId: (branch == null && district == null) ? -1 : district,
                                                 queueStatusId: (queueStatus == -1 ? null : queueStatus),
                                                 indexStart: indexStart,
                                                 pageSize: pageSize,
                                                 sortField: sortField,
                                                 orderType: orderType,
                                                 searchDetail: search
                                                 ).ToList();

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
        ///  สถานะคิวงานแบ่งตามสาขา
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="period"></param>
        /// <param name="area"></param>
        /// <param name="branch"></param>
        /// <param name="indexStart"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SupervisorQueueStatusDT(int draw, string period, int? area, int? branch, int? indexStart, int? pageSize, string sortField, string orderType, string search)
        {
            try
            {
                var result = _context.usp_pivotSupervisorQueueStatusV1_Select(appStartCoverDate: DateTime.TryParse(period, out var date) ? date : (DateTime?)null,
                                                                              payerStudyAreaId: null,
                                                                              payerBranchId: (branch == -1 ? null : branch),
                                                                              indexStart: indexStart,
                                                                              pageSize: pageSize,
                                                                              sortField: sortField,
                                                                              orderType: orderType,
                                                                              searchDetail: search,
                                                                              areaId: (area == -1 ? null : area)
                                                                              ).ToList();

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
        /// สถานะคิวงานแบ่งตามพื้นที่ ผอ.บล.
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="period"></param>
        /// <param name="branch"></param>
        /// <param name="indexStart"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public ActionResult SupervisorQueueStatusDetailDT(int draw, string period, int? branch, int? indexStart, int? pageSize, string sortField, string orderType, string search)
        {
            try
            {
                var result = _context.usp_pivotSupervisorQueueStatusV2_Select(appStartCoverDate: DateTime.TryParse(period, out var date) ? date : (DateTime?)null,
                                                                              payerBranchId: branch ?? -1,
                                                                              indexStart: indexStart,
                                                                              pageSize: pageSize,
                                                                              sortField: sortField,
                                                                              orderType: orderType,
                                                                              searchDetail: search
                                                                              ).ToList();

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

        #endregion Datatable

        #region Excel

        /// <summary>
        /// ดาวน์โหลดรายงาน สถานะคิวงานแบ่งตามพื้นที่ ประธานสาขา  excel
        /// </summary>
        /// <param name="period"></param>
        /// <param name="area"></param>
        /// <param name="branch"></param>
        /// <param name="search"></param>
        public void SupervisorQueueApproveStatusExcel(string period, int? area, int? branch, string search)
        {
            //Call storeprocedure *DCR
            var obj = _context.usp_pivotSupervisorQueueApproveStatus_Select(appStartCoverDate: DateTime.TryParse(period, out var date) ? date : (DateTime?)null,
                                                                            payerStudyAreaId: null,
                                                                            payerBranchId: (branch == -1 ? null : branch),
                                                                            indexStart: 0,
                                                                            pageSize: 999999,
                                                                            sortField: null,
                                                                            orderType: null,
                                                                            searchDetail: search,
                                                                            areaId: (area == -1 ? null : area))
                .Select(o => new
                {
                    สาขา = o.BranchDetail,
                    ประธานสาขา = $"{o.ChairmanEmployeeCode} {o.ChairmanFullName}",
                    ทั้งหมด = o.QueueAll,
                    รอพิจารณา = o.ApproveStatusId2,
                    พิจารณาแล้ว = o.ApproveComplete,
                    อนุมัติให้ผ่านคัดกรอง = o.ApproveStatusId3,
                    อนุมัติให้ผ่านคัดกรองแบบติดเงื่อนไข = o.ApproveStatusId4,
                    อนุมัติไม่ให้ผ่านคัดกรอง = o.ApproveStatusId5,
                    เกินกำหนดยังไม่ได้พิจารณา = o.ApproveStatusId6
                }).ToList();

            GlobalFunction.ExportToExcel(HttpContext, obj, "Sheet1", "สถานะคิวงานแบ่งตามพื้นที่_ประธานสาขา_" + period);
        }

        /// <summary>
        /// ดาวน์โหลดรายงาน สถานะคิวงานแบ่งตาม สาขา  excel
        /// </summary>
        /// <param name="period"></param>
        /// <param name="area"></param>
        /// <param name="branch"></param>
        /// <param name="search"></param>
        public void SupervisorQueueStatusExcel(string period, int? area, int? branch, string search)
        {
            //Call storeprocedure *DCR
            var obj = _context.usp_pivotSupervisorQueueStatusV1_Select(appStartCoverDate: DateTime.TryParse(period, out var date) ? date : (DateTime?)null,
                                                                       payerStudyAreaId: null,
                                                                       payerBranchId: (branch == -1 ? null : branch),
                                                                       indexStart: 0,
                                                                       pageSize: 999999,
                                                                       sortField: null,
                                                                       orderType: null,
                                                                       searchDetail: search,
                                                                       areaId: (area == -1 ? null : area))
                .Select(o => new
                {
                    สาขา = o.BranchDetail,
                    ทั้งหมด = o.QueueStatusAll,
                    ยังไม่ได้คัดกรอง = o.QueueStatusId2,
                    โทรคัดกรองและมอบบัตรภายหลัง = o.QueueStatusId3,
                    โทรคัดกรองและมอบบัตรแล้ว = o.QueueStatusId4,
                    เข้าพบคัดกรองและมอบบัตร = o.QueueStatusId5,
                    ไม่ผ่านคัดกรอง = o.QueueStatusId6,
                    ยกเลิกก่อน_DCR = o.QueueStatusId7,
                    ยังไม่ได้ดำเนินการ = o.QueueStatusId8
                }).ToList();

            GlobalFunction.ExportToExcel(HttpContext, obj, "Sheet1", $"สถานะคิวงานแบ่งตามสาขา_{period}");
        }

        /// <summary>
        /// ดาวน์โหลดรายงาน สถานะคิวงานแบ่งตามพื้นที่ ผอ.บล.  excel
        /// </summary>
        /// <param name="period"></param>
        /// <param name="branch"></param>
        /// <param name="search"></param>
        public void SupervisorQueueStatusDetailExcel(string period, int? branch, string search)
        {
            //Call storeprocedure *DCR
            var obj = _context.usp_pivotSupervisorQueueStatusV2_Select(appStartCoverDate: DateTime.TryParse(period, out var date) ? date : (DateTime?)null,
                                                                       payerBranchId: branch ?? -1,
                                                                       indexStart: 0,
                                                                       pageSize: 999999,
                                                                       sortField: null,
                                                                       orderType: null,
                                                                       searchDetail: search)
                .Select(o => new
                {
                    อำเภอ = o.DistrictDetail,
                    จังหวัด = o.ProvinceDetail,
                    ผู้รับผิดชอบ = $"{o.DirectorEmployeeCode} {o.DirectorFullName}",
                    ทั้งหมด = o.QueueStatusAll,
                    ยังไม่ได้คัดกรอง = o.QueueStatusId2,
                    โทรคัดกรองและมอบบัตรภายหลัง = o.QueueStatusId3,
                    โทรคัดกรองและมอบบัตรแล้ว = o.QueueStatusId4,
                    เข้าพบคัดกรองและมอบบัตร = o.QueueStatusId5,
                    ไม่ผ่านคัดกรอง = o.QueueStatusId6,
                    ยกเลิกก่อน_DCR = o.QueueStatusId7,
                    ยังไม่ได้ดำเนินการ = o.QueueStatusId8
                }).ToList();

            GlobalFunction.ExportToExcel(HttpContext, obj, "Sheet1", $"สถานะคิวงานแบ่งตามพื้นที่_ผอบล_{period}");
        }

        #endregion Excel
    }
}