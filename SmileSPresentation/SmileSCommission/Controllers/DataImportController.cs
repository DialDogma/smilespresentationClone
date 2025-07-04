using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Spreadsheet;
using SmileSCommission.Helper;
using SmileSCommission.Models;

namespace SmileSCommission.Controllers
{
    [Authorization]
    public class DataImportController : Controller
    {
        #region Context

        private readonly CommissionCalculateEntities _context;
        private readonly SalaryReportEntities _contextSalary;
        private static readonly CultureInfo _dtEn = new CultureInfo("en-US");

        public DataImportController()

        {
            _context = new CommissionCalculateEntities();
            _contextSalary = new SalaryReportEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            _contextSalary.Dispose();
        }

        #endregion Context

        #region View

        public ActionResult DataSystem_push_SSS()
        {
            return View();
        }

        public ActionResult DataSystem_push_Manual()
        {
            var modelResult = _context.usp_CommissionPeriod_GetLastedPeriod_Select().FirstOrDefault();

            var causeList = _context.usp_DatasourceImportedEditCause_Select(null, null, 999, null, null, null).ToList();

            ViewBag.causeList = causeList;

            if (modelResult.CommissionPeriodStatusId != 2)
            {
                return RedirectToAction("InternalServerError", "Error", new { exception = "รอบค่าตอบแทนได้ถูกนำส่งแล้ว!" });
            }

            if (modelResult == null)
            {
                return RedirectToAction("InternalServerError", "Error", new { exception = "กรุณาสร้างรอบการคิดค่าตอบแทนใหม่" });
            }
            else
            {
                ViewBag.periodId = modelResult.PeriodId;
                ViewBag.PeriodDetail = modelResult.PayPeriod?.ToString("dd/MM/yyyy");
                ViewBag.periodStatusId = modelResult.CommissionPeriodStatusId;
                ViewBag.payPeriod = modelResult.PayPeriod?.ToString("dd/MM/yyyy");
                ViewBag.description = modelResult.Description;
            }
            return View();
        }

        public ActionResult ImportData(int dtsId)
        {
            ViewBag.srcTitle = "Weekly Sale Target";
            ViewBag.serviceURL = Properties.Settings.Default.UATlink;
            return View();
        }

        public ActionResult Download_PreviewData(int headerId, string systemName)
        {
            ViewBag.headerId = headerId;
            ViewBag.systemName = systemName;

            return View();
        }

        public ActionResult InsertPreviewData(int headerId, string systemName)
        {
            ViewBag.headerId = headerId;
            ViewBag.systemName = systemName;
            return View();
        }

        #endregion View

        #region api

        [HttpGet]
        public JsonResult GetDatasourceImportLog(int periodId, int? draw = null, int? indexStart = null,
            int? pageSize = null, string sortField = null, string orderType = null, string search = null)
        {
            //get user
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            //get datasource log with user
            var result = _context.usp_DatasourceImportedLog_Select(periodId, user, indexStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateNotify(int dtsId, int editCauseId, string editRemark)
        {
            //get user
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            var result = _context.usp_DatasourceImportedLog_Notify_Update(dtsId, editCauseId, editRemark, user).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDataSourceDetail(int periodId, int datasourceId)
        {
            var result = _context.usp_DatasourceImportedLog_Select(
                periodId, null, null, 99, null, null, null).FirstOrDefault(x => x.Id == datasourceId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Integrate_Insert(string period, string systemName)
        {
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            var dateConvert = DateTime.ParseExact(period, "dd-MM-yyyy", new CultureInfo("en-Us"));

            if (systemName == "SSS")
            {
                var result = _contextSalary.usp_IntegrateDataLog_SSS_Insert(dateConvert, user).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else if (systemName == "CLA")
            {
                var result = _contextSalary.usp_IntegrateDataLog_ClaimOnline_Insert(dateConvert, user).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else if (systemName == "Motor")
            {
                var result = _contextSalary.usp_IntegrateDataLog_Motor_Insert(dateConvert, user).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Integrate_Update(int headerId, string systemName)
        {
            try
            {
                HttpContext ctx = System.Web.HttpContext.Current;

                //Thread obj = new Thread(() => Integrate_Update_Run(headerId,systemName));
                Thread obj = new Thread(() =>
                {
                    System.Web.HttpContext.Current = ctx;
                    Integrate_Update_Run(headerId, systemName);
                });
                obj.IsBackground = true;
                obj.Start();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }

        private void Integrate_Update_Run(int headerId, string systemName)
        {
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            using (var _dbContext = new SalaryReportEntities())
            {
                _dbContext.Database.CommandTimeout = 5000;
                if (systemName == "SSS")
                {
                    _dbContext.usp_IntegrateDataLog_SSS_Update(headerId, user);
                }
                else if (systemName == "Motor")
                {
                    _dbContext.usp_IntegrateDataLog_Motor_Update(headerId, user);
                }
                else if (systemName == "CLA")
                {
                    _dbContext.usp_IntegrateDataLog_ClaimOnline_Update(headerId, user);
                }
            }
        }

        [HttpGet]
        public JsonResult IntegrateHeader_Select(string systemName, int headerId)
        {
            if (systemName == "SSS")
            {
                var result = _contextSalary.usp_IntegrateDataLogHeader_SSS_Select(headerId, null, null, 0, 99,
                    null, null, null).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else if (systemName == "CLA")
            {
                var result = _contextSalary.usp_IntegrateDataLogHeader_ClaimOnline_Select(headerId, null, null, 0, 99,
                   null, null, null).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else if (systemName == "Motor")
            {
                var result = _contextSalary.usp_IntegrateDataLogHeader_Motor_Select(headerId, null, null, 0, 99,
                  null, null, null).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IntegrateDetail_Select(string systemName, int headerId, int? draw = null,
            int? indexStart = null, int? pageSize = null, string sortField = null, string orderType = null,
            string search = null)
        {
            if (systemName == "SSS")
            {
                var result = _contextSalary.usp_IntegrateDataLogDetail_SSS_Select(headerId, indexStart, pageSize, sortField
                    , orderType, search).ToList();

                var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
            else if (systemName == "CLA")
            {
                var result = _contextSalary.usp_IntegrateDataLogDetail_ClaimOnline_Select(headerId, indexStart, pageSize, sortField
                    , orderType, search).ToList();

                var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
            else if (systemName == "Motor")
            {
                var result = _contextSalary.usp_IntegrateDataLogDetail_Motor_Select(headerId, indexStart, pageSize, sortField
                   , orderType, search).ToList();

                var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult IntegrateReport_Select(string systemName, int headerId, int? draw = null,
            int? indexStart = null, int? pageSize = null, string sortField = null, string orderType = null,
            string search = null)
        {
            if (systemName == "SSS")
            {
                var result = _contextSalary.usp_IntegrateDataLogReport_SSS_Select(headerId, indexStart, pageSize, sortField,
                    orderType, search).ToList();

                var dt = new Dictionary<string, object>
                {
                    {"draw", draw },
                    {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                    {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                    {"data", result.ToList()}
                };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
            else if (systemName == "CLA")
            {
                var result = _contextSalary.usp_IntegrateDataLogReport_ClaimOnline_Select(headerId, indexStart, pageSize, sortField,
                   orderType, search).ToList();

                var dt = new Dictionary<string, object>
                {
                    {"draw", draw },
                    {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                    {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                    {"data", result.ToList()}
                };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
            else if (systemName == "Motor")
            {
                var result = _contextSalary.usp_IntegrateDataLogReport_Motor_Select(headerId, indexStart, pageSize, sortField,
                   orderType, search).ToList();

                var dt = new Dictionary<string, object>
                {
                    {"draw", draw },
                    {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                    {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                    {"data", result.ToList()}
                };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public void DatalogReport_Select(int id, string systemName)
        {
            SqlConnection connection = new SqlConnection(new SalaryReportEntities().Database.Connection.ConnectionString);
            SqlCommand command = new SqlCommand();
            if (systemName == "SSS")
            {
                command = new SqlCommand("usp_DataLogReport_SSS_Select", connection);
            }
            else if (systemName == "CLA")
            {
                command = new SqlCommand("usp_DataLogReport_ClaimOnline_Select", connection);
            }
            else if (systemName == "Motor")
            {
                command = new SqlCommand("usp_DataLogReport_Motor_Select", connection);
            }

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("Id", id);
            connection.Open();

            DataTable dt = new DataTable();
            dt.Load(command.ExecuteReader());
            connection.Close();

            var wb = new ClosedXML.Excel.XLWorkbook();
            wb.AddWorksheet(dt, "sheet1");

            HttpContext.Response.Clear();
            HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Response.AddHeader("content-disposition", "attachment;filename=" + id + ".xlsx");

            using (var tmpMemoryStream = new MemoryStream())
            {
                wb.SaveAs(tmpMemoryStream);
                tmpMemoryStream.WriteTo(HttpContext.Response.OutputStream);
                tmpMemoryStream.Close();
            }
            HttpContext.Response.End();
        }

        [HttpPost]
        public JsonResult IntegrateDataToCommission_Insert(string systemName)
        {
            try
            {
                var periodId = _context.usp_CommissionPeriod_GetLastedPeriod_Select().FirstOrDefault().PeriodId;
                HttpContext ctx = System.Web.HttpContext.Current;

                Thread obj = new Thread(() =>
                {
                    System.Web.HttpContext.Current = ctx;
                    Integrate_Insert_Run(systemName, periodId);
                });
                obj.IsBackground = true;
                obj.Start();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }

        private void Integrate_Insert_Run(string systemName, int periodId)
        {
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;

            using (var _dbContext2 = new SalaryReportEntities())
            {
                _dbContext2.Database.CommandTimeout = 5000;
                if (systemName == "SSS")
                {
                    _dbContext2.usp_IntegrateData_SSS_ToCommission_Insert(periodId, user);
                }
                else if (systemName == "Motor")
                {
                    _dbContext2.usp_IntegrateData_Motor_ToCommission_Insert(periodId, user);
                }
                else if (systemName == "CLA")
                {
                    _dbContext2.usp_IntegrateData_ClaimOnline_ToCommission_Insert(periodId, user);
                }
            }
        }

        [HttpPost]
        public JsonResult RawData_Delete(int headerId, string systemName)
        {
            try
            {
                var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
                if (systemName == "SSS")
                {
                    var result = _contextSalary.usp_RawDataAndCalculateData_SSS_Delete(headerId, user).FirstOrDefault();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else if (systemName == "Motor")
                {
                    var result = _contextSalary.usp_RawDataAndCalculateData_Motor_Delete(headerId, user).FirstOrDefault();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else if (systemName == "CLA")
                {
                    var result = _contextSalary.usp_RawDataAndCalculateData_ClaimOnline_Delete(headerId, user).FirstOrDefault();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { IsResult = false, Result = "System Name Required!!", Msg = "System Name Required!!" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                var result = new { IsResult = false, Result = e.Message, Msg = e.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult IntegrateDataToCommission_Select(string systemName, int? draw = null,
            int? indexStart = null, int? pageSize = null, string sortField = null, string orderType = null,
            string search = null)
        {
            var periodId = _context.usp_CommissionPeriod_GetLastedPeriod_Select().FirstOrDefault().PeriodId;
            if (systemName == "SSS")
            {
                var result = _contextSalary.usp_IntegrateData_SSS_ToCommission_Select(periodId, indexStart, pageSize, sortField,
                    orderType, search).ToList();

                var dt = new Dictionary<string, object>
                {
                    {"draw", draw },
                    {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                    {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                    {"data", result.ToList()}
                };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
            else if (systemName == "CLA")
            {
                var result = _contextSalary.usp_IntegrateData_ClaimOnline_ToCommission_Select(periodId, indexStart, pageSize, sortField,
                   orderType, search).ToList();

                var dt = new Dictionary<string, object>
                {
                    {"draw", draw },
                    {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                    {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                    {"data", result.ToList()}
                };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
            else if (systemName == "Motor")
            {
                var result = _contextSalary.usp_IntegrateData_Motor_ToCommission_Select(periodId, indexStart, pageSize, sortField,
                   orderType, search).ToList();

                var dt = new Dictionary<string, object>
                {
                    {"draw", draw },
                    {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                    {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                    {"data", result.ToList()}
                };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        #endregion api
    }
}