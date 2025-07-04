using SmileSUnderwrite.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSUnderwrite.DatacenterEmployeeService;
using SmileSUnderwrite.Models;

namespace SmileSUnderwrite.Controllers
{
    [Authorization]
    public class QueueController : Controller
    {
        #region dbContext

        private UnderwriteDBContext _context;
        private SSSPAEntities _db;

        public QueueController()
        {
            _context = new UnderwriteDBContext();
            _db = new SSSPAEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion dbContext

        #region Assign

        public ActionResult Assign()
        {
            ViewBag.QueueType = _context.usp_QueueType_Select();
            ViewBag.Branch = _context.usp_Branch_Select();

            return View();
        }

        //set assign queue
        public JsonResult AssignQueue(int[] queueId, int[] assignToUserId, int createByUserId)
        {
            var index = 0;

            try
            {
                foreach (var itm in queueId)
                {
                    var result = _context.usp_AssignQueue(itm, assignToUserId[index], Convert.ToInt32(Session["User_ID"])).FirstOrDefault();

                    if (index == assignToUserId.Length - 1)
                    {
                        index = 0;
                    }
                    else
                    {
                        index += 1;
                    }
                }

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        //set assign All queue
        public JsonResult AssignQueueAll(int[] assignUserlist, string dtf,
            string dto, string branch_ID,
            int? queueTypeId = null, int? queueStatusId = null,
            int? assignToUserId = null)
        {
            int? queueId = null;
            int pageStart = 0;
            int pageSize = 99999;
            string sortField = null;
            string orderType = null;
            string search = "";
            DateTime? dateFrom = null;
            DateTime? dateTo = null;
            if (branch_ID == "")
            {
                branch_ID = null;
            }
            assignToUserId = Convert.ToInt32(Session["User_ID"]);
            var UserID = Convert.ToInt32(Session["User_ID"]);

            dateFrom = GlobalFunction.ConvertDatePickerToDate_BE(dtf);
            dateTo = GlobalFunction.ConvertDatePickerToDate_BE(dto);

            string result = "false";
            //true บันทึกสำเร็จ
            //notfound ไม่มีคิว
            //assignover คิวน้อยคน
            //false บันทึกไม่สำเร็จ
            try
            {
                var list = _context.usp_Queue_SelectV2(queueId, queueTypeId, queueStatusId, UserID,
                     dateFrom, dateTo, null, null, pageStart, pageSize, sortField, orderType, search, branch_ID).FirstOrDefault();

                if (assignUserlist.Count() > list.TotalCount)
                {
                    result = "assignover";
                }
                else if (list.TotalCount > 0)
                {
                    var TotalCount = list.TotalCount;
                    var AssignCount = assignUserlist.Count();
                    var TotalAssign = TotalCount / AssignCount;

                    try
                    {
                        foreach (var itm in assignUserlist)
                        {
                            var Ans = _context.usp_AssignQueueListV2(queueId, queueTypeId, queueStatusId
                                , dateFrom, dateTo, null, null, branch_ID, itm, UserID, TotalAssign).FirstOrDefault();
                            result = "true";
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        result = "false";
                        throw;
                    }

                    if (result == "true")
                    {
                        var listQueue = _context.usp_Queue_SelectV2(queueId, queueTypeId, queueStatusId, UserID,
                        dateFrom, dateTo, null, null, pageStart, pageSize, sortField, orderType, search, branch_ID).ToList();

                        try
                        {
                            var index = 0;

                            foreach (var itm in listQueue)
                            {
                                var Ans = _context.usp_AssignQueue(itm.QueueId, assignUserlist[index], UserID).FirstOrDefault();
                                if (index == assignUserlist.Length - 1)
                                {
                                    index = 0;
                                }
                                else
                                {
                                    index += 1;
                                }
                                result = "true";
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            result = "false";
                            throw;
                        }
                    }
                }
                else
                {
                    result = "notfound";
                }
            }
            catch (Exception e)
            {
                result = "notfound";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //assign page datatable
        public JsonResult GetDatatableAssign(string dtf,
            string dto, int draw, int pageSize, int pageStart,
            string sortField, string orderType, string search, int? queueId = null,
            int? queueTypeId = null,
            int? queueStatusId = null, int? assignToUserId = null, string branch_ID = null)
        {
            DateTime? dateFrom = null;
            DateTime? dateTo = null;
            if (branch_ID == "")
            {
                branch_ID = null;
            }

            assignToUserId = Convert.ToInt32(Session["User_ID"]);

            dateFrom = GlobalFunction.ConvertDatePickerToDate_BE(dtf);
            dateTo = GlobalFunction.ConvertDatePickerToDate_BE(dto);

            var list = _context.usp_Queue_SelectV2(queueId, queueTypeId, queueStatusId, assignToUserId,
                dateFrom, dateTo, null, null, pageStart, pageSize, sortField, orderType, search, branch_ID).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataTableEmployee(int draw, int pageSize, int pageStart,
            string sortField, string orderType, string search)
        {
            using (var person = new EmployeeServiceClient())
            {
                var list = person.GetEmployeeSelectResults(null, orderType, search, sortField,
                    pageSize, pageStart, null, null, 3, 9, null, null, null).ToList();
                var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDataTableEmployeeAll(int draw, int pageSize, int pageStart,
    string sortField, string orderType, string search)
        {
            {
                string UserCode = Session["Username"].ToString();
                using (var person = new EmployeeServiceClient())
                {
                    var list = person.GetEmployeeSelectResults(null, orderType, search, sortField,
                        pageSize, pageStart, null, null, 3, 9, null, null, null).Where(o => o.EmployeeCode != UserCode).ToList();

                    var list2 = list.Where(o => o.EmployeeCode != UserCode).ToList();

                    var dt = new Dictionary<string, object>

                {
                    {"draw", draw },
                    {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount -1 : list.Count() -1},
                    {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount -1 : list.Count() -1},
                    {"data", list.ToList()}
                };

                    return Json(dt, JsonRequestBehavior.AllowGet);
                }
            }
        }

        #endregion Assign

        // GET: Queue

        //Monitor
        public ActionResult Monitor()
        {
            ViewBag.QueueTypes = _context.usp_QueueType_Select();
            ViewBag.PaYear = _db.vw_CodeGroup_PAYear.OrderByDescending(x => new { x.Code }).ToList();
            return View();
        }

        //MoveQueue
        [HttpGet]
        public ActionResult MoveQueue()
        {
            var person = new EmployeeServiceClient();

            ViewBag.QueueTypes = _context.usp_QueueType_Select();

            ViewBag.FromEmp = person.GetEmployeeSelectResults(null, null, null, null,
                    100, 0, null, null, 3, 9, null, null, null).ToList();

            ViewBag.ToEmp = person.GetEmployeeSelectResults(null, null, null, null,
                    100, 0, null, null, 3, 9, null, null, null).ToList();

            return View();
        }

        [HttpPost]
        public JsonResult MoveQueue(int[] queueId, int assignToUserId, int createByUserId)
        {
            var index = 0;
            var count = 0;
            //var average = queueId.Length / assignToUserId.Length;
            var queue_Count = queueId.Length;
            int userID;

            //userID = 228;

            userID = Convert.ToInt32(Session["User_ID"]);
            try
            {
                foreach (var itm in queueId)
                {
                    //TODO:replace create by userId
                    _context.usp_AssignQueue(itm, assignToUserId, userID);
                    count += 1;
                    if (count == queue_Count)
                    {
                        index += 1;
                    }
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AsyncTimeout(600000)]
        public JsonResult GetDatatableMonitor(int? draw = null, int? queueType = null, string dFrom = null, string dTo = null, string yearData = null)
        {
            DateTime? dateFrom = null;
            DateTime? dateTo = null;
            int? userId = null;
            int? queueTypeId = null;

            dateFrom = GlobalFunction.ConvertDatePickerToDate_BE(dFrom);
            dateTo = GlobalFunction.ConvertDatePickerToDate_BE(dTo);

            if (queueType != -1)
            {
                queueTypeId = queueType;
            }

            string strYearData = null;
            if (!string.IsNullOrEmpty(yearData)) strYearData = yearData;
            try
            {
                var result = _context.usp_pvQueueStatusByUser_Select(dateFrom, dateTo, queueTypeId, null, 9999999, null,
                    null, null, strYearData).ToList();

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
                Console.WriteLine(e);
                throw;
            }
        }

        //GetDatatableDetail
        public JsonResult GetDatatableMonitorDetail(int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null,
            int? queueType = null, string dFrom = null, string dTo = null, int? status = null, string yearData = null)
        {
            DateTime? dateFrom = null;
            DateTime? dateTo = null;

            dateFrom = GlobalFunction.ConvertDatePickerToDate_BE(dFrom);
            dateTo = GlobalFunction.ConvertDatePickerToDate_BE(dTo);

            int? queueTypeId = null;
            if (queueType != -1)
            {
                queueTypeId = queueType;
            }

            string strYearData = null;
            if (!string.IsNullOrEmpty(yearData)) strYearData = yearData;

            var result = _context.usp_Queue_Select(null, queueTypeId, status, null, dateFrom, dateTo, null, null, pageStart, pageSize, sortField, orderType, search, strYearData).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDatatableQueueDetail(int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null,
           int? queueType = null, string dFrom = null, string dTo = null, int? status = null, int? empCode = null, string yearData = null)
        {
            DateTime? dateFrom = null;
            DateTime? dateTo = null;

            dateFrom = GlobalFunction.ConvertDatePickerToDate_BE(dFrom);
            dateTo = GlobalFunction.ConvertDatePickerToDate_BE(dTo);

            int? queueTypeId = null;

            if (queueType != -1)
            {
                queueTypeId = queueType;
            }

            var result = _context.usp_Queue_Select(null, queueTypeId, status, empCode, dateFrom, dateTo, null, null, pageStart, pageSize, sortField, orderType, search, yearData).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDatatableDetail(int underwriteId)
        {
            var result = _context.usp_Call_Select(underwriteId, null).ToList();

            var dt = new Dictionary<string, object>
            {
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        //GetDatatableQueue
        public JsonResult GetDatatableQueue(int draw, int pageSize, int pageStart, string sortField, string orderType, string search, string queueType, string empCode)
        {
            List<Models.MonitorDetail> QueueDetail = new List<Models.MonitorDetail>();

            QueueDetail.Add(new Models.MonitorDetail()
            {
                SchoolName = "โรงเรียนเนตรนารี1",
                Province = "เชียงราย",
                District = "เมือง",
                SubDistrict = "ริมกก",
                Payment = "รอการชำระเงิน",
                CreatedBy = "พี่เบลคนก๊ากกก กากก",
                QueueDate = Convert.ToDateTime("2561-04-09"),
                Day = 4
            });
            QueueDetail.Add(new Models.MonitorDetail()
            {
                SchoolName = "โรงเรียนเนตรนารี2",
                Province = "เชียงราย",
                District = "เมือง",
                SubDistrict = "ริมกก",
                Payment = "รอการชำระเงินมา 3 วัน พี่เบลไม่จ่ายเล้ยยย",
                CreatedBy = "พี่เบลคนก๊ากกก กากก",
                QueueDate = Convert.ToDateTime("2561-04-09"),
                Day = 3
            });
            QueueDetail.Add(new Models.MonitorDetail()
            {
                SchoolName = "โรงเรียนเนตรนารี3",
                Province = "เชียงราย",
                District = "เมือง",
                SubDistrict = "ริมกก",
                Payment = "รอการชำระเงินมา 5 วัน พี่เบลไม่จ่ายเล้ยยย",
                CreatedBy = "พี่เบลคนก๊ากกก กากก",
                QueueDate = Convert.ToDateTime("2561-04-09"),
                Day = 3
            }
            );

            var TotalCount = 3;
            //var result = "00";

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", QueueDetail.Count() != 0 ? TotalCount : QueueDetail.Count()},
                {"recordsFiltered", QueueDetail.Count() != 0 ? TotalCount : QueueDetail.Count()},
                {"data", QueueDetail.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetChartData(string dFrom, string dTo, string queueType, string yearData)
        {
            try
            {
                DateTime? dateFrom = null;
                DateTime? dateTo = null;
                dateFrom = GlobalFunction.ConvertDatePickerToDate_BE(dFrom);
                dateTo = GlobalFunction.ConvertDatePickerToDate_BE(dTo);
                int? queueTypeId = Convert.ToInt32(queueType);

                var chartTotal = _context.usp_pvQueueStatus_Select(dateFrom, dateTo, queueTypeId, yearData).FirstOrDefault();

                return Json(chartTotal, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}