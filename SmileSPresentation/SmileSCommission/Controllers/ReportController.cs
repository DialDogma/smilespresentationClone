using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using SmileSCommission.EmployeeService;
using SmileSCommission.Helper;
using SmileSCommission.Models;

namespace SmileSCommission.Controllers
{
    [Authorization]
    public class ReportController : Controller
    {
        #region Context

        private readonly CommissionCalculateEntities _context;
        private readonly EmployeeServiceClient _employeeService;
        private static readonly CultureInfo _dtEn = new CultureInfo("en-US");

        public ReportController()

        {
            _employeeService = new EmployeeServiceClient();
            _context = new CommissionCalculateEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Context

        #region View

        public ActionResult AgentReportMain()
        {
            // check role
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var userRoleList = new SSOService.SSOServiceClient().GetRoleByUserName(user);
            var userRole = userRoleList.Split(',');
            if (userRole.Contains("Developer"))
            {
                ViewBag.IsHasAuthen = "true";
            }
            else
            {
                ViewBag.IsHasAuthen = "false";
                ViewBag.empCode = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
                ViewBag.empFullName = GlobalFunction.GetLoginDetail(this.HttpContext).FullName;
            }
            //get period list to dropdown list
            var periodResult = _context.usp_CommissionPeriod_Select(null, 4, null, null, null, null, null).ToList();
            ViewBag.periodList = periodResult;

            return View();
        }

        public ActionResult AgentReport(string empCode, int? periodId)
        {
            // check role
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var userRoleList = new SSOService.SSOServiceClient().GetRoleByUserName(user);
            var userRole = userRoleList.Split(',');
            if (userRole.Contains("Developer"))
            {
                ViewBag.IsHasAuthen = "true";
            }
            else
            {
                ViewBag.IsHasAuthen = "false";
            }

            //get parameter if has empCode
            ViewBag.empCode = empCode;
            if (empCode != "null")
            {
                var empResult = _employeeService.GetEmployeeSearch(empCode).FirstOrDefault();
                ViewBag.empFullName = empResult.FirstName + " " + empResult.LastName;
            }

            ViewBag.PeriodId = periodId;
            // get period list to dropdown list
            var periodResult = _context.usp_CommissionPeriod_Select(null, 4, null, null, null, null, null).ToList();
            ViewBag.periodList = periodResult;

            return View();
        }

        public ActionResult CompensationHistory_Main()
        {
            return View();
        }

        public ActionResult CompensationHistory_Detail(int periodId)
        {
            ViewBag.serviceURL = Properties.Settings.Default.UATlink;
            ViewBag.periodId = periodId;

            var result = _context.usp_CommissionPeriod_Select(periodId, null, null, null, null, null, null).FirstOrDefault();

            ViewBag.periodStatus = result.CommissionPeriodStatus;
            ViewBag.periodStatusId = result.CommissionPeriodStatusId;
            ViewBag.payPeriod = result.PayPeriod?.ToString("dd/MM/yyyy");
            ViewBag.description = result.Description;

            var alertNotiCount = _context.usp_DatasourceImportedLog_Notify_Select(periodId, null, null, 9999, null,
                null, null).Count();
            ViewBag.alertNotiCount = alertNotiCount;

            return View();
        }

        #endregion View

        #region api

        [HttpGet]
        public JsonResult GetEmployeeData(string empCode)
        {
            var result = _employeeService.GetEmployeeSearch(empCode).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPeriodList(int? periodId, int? periodStatusId)
        {
            //get period list to dropdown list
            var result = _context.usp_CommissionPeriod_Select(periodId, periodStatusId, null, null, null, null, null).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //get datatable of param by periodid and main key
        [HttpPost]
        public JsonResult GetParamValue_DT(int periodId, string mainKey, int? draw = null, int? indexStart = null,
            int? pageSize = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _context.usp_ParamValue_Select(periodId, mainKey, indexStart, pageSize, sortField, orderType,
                search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        #region ReportDetail

        [HttpPost]
        public JsonResult GetNewAppPH_DT(int periodId, string mainKey, int? draw = null, int? indexStart = null,
            int? pageSize = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _context.usp_AppDetailPH_Select(periodId, mainKey, indexStart, pageSize, sortField, orderType,
              search).ToList();

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
        public JsonResult GetNewAppPA30_DT(int periodId, string mainKey, int? draw = null, int? indexStart = null,
           int? pageSize = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _context.usp_AppDetailPA30_Select(periodId, mainKey, indexStart, pageSize, sortField, orderType,
              search).ToList();

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
        public JsonResult GetNewAppPL_DT(int periodId, string mainKey, int? draw = null, int? indexStart = null,
           int? pageSize = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _context.usp_AppDetailPL_Select(periodId, mainKey, indexStart, pageSize, sortField, orderType,
              search).ToList();

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
        public JsonResult GetNewAppHP_DT(int periodId, string mainKey, int? draw = null, int? indexStart = null,
           int? pageSize = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _context.usp_AppDetailHP_Select(periodId, mainKey, indexStart, pageSize, sortField, orderType,
              search).ToList();

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
        public JsonResult GetNewAppMP_DT(int periodId, string mainKey, int? draw = null, int? indexStart = null,
           int? pageSize = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _context.usp_AppDetailMP_Select(periodId, mainKey, indexStart, pageSize, sortField, orderType,
              search).ToList();

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
        public JsonResult GetNewAppPACommunity_DT(int periodId, string mainKey, int? draw = null, int? indexStart = null,
           int? pageSize = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _context.usp_AppDetailPAcommunity_Select(periodId, mainKey, indexStart, pageSize, sortField, orderType,
              search).ToList();

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
        public JsonResult GetLapse_DT(int periodId, string mainKey, int? draw = null, int? indexStart = null,
           int? pageSize = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _context.usp_Lapse_Select(periodId, mainKey, indexStart, pageSize, sortField, orderType,
              search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        #endregion ReportDetail

        #region CompensationHistory

        //get period datatable criteria by datetime
        [HttpPost]
        public JsonResult GetPeriodDCR_DT(string payPeriod, bool? isWaitProcess, bool? isSentPayroll,
            bool? isSucess, bool? isCancel, int? draw = null, int? indexStart = null, int? pageSize = null,
            string sortField = null, string orderType = null, string search = null)
        {
            DateTime payPeriodConvert = DateTime.ParseExact(payPeriod, "dd-MM-yyyy", new CultureInfo("en-Us"));

            var result = _context.usp_CommissionPeriodSearch_Select(payPeriodConvert, isWaitProcess, isSentPayroll, isSucess,
                isCancel, indexStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        //get period datasource
        [HttpGet]
        public JsonResult GetPeriodDatasourceLog(int payPeriodId, int? draw = null, int? indexStart = null,
            int? pageSize = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _context.usp_DatasourceImportedLog_Select(payPeriodId, null, indexStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        //get latest model for datatable
        [HttpGet]
        public JsonResult GetModelPeriod_DT(int periodId, int? draw = null, int? indexStart = null, int? pageSize = null,
            string sortField = null, string orderType = null, string search = null)
        {
            var result = _context.usp_ModelCalculationIndex_Select(periodId, indexStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        //get notify list
        [HttpGet]
        public JsonResult GetNotifyLog(int periodId, int? draw = null, int? indexStart = null,
            int? pageSize = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _context.usp_DatasourceImportedLog_Notify_Select(periodId, null, indexStart, pageSize, sortField,
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

        [HttpGet]
        public JsonResult GetDownloadWithFunctionLink(int periodId)
        {
            var result = _context.CommissionPeriod.Where(x => x.Id == periodId).Select(x => x.CalculatedFileURLForDownload).FirstOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //download datasource
        [HttpGet]
        public void DownloadDts(int periodId, string modelName)
        {
            var ExcelModel = new ExcelPackage();

            var db = new CommissionCalculateEntities();
            // Get Datasource list
            var dsList = db.ModelDatasource.Where(x => x.Period_Id == periodId && x.DatasourceName == modelName)
                .Select(x => x.DatasourceName)
                .Distinct().ToList();

            //Loop in list
            foreach (var ds in dsList)
            {
                //Create Sheet
                ExcelModel.Workbook.Worksheets.Add(ds);

                BindDatasourceSheet(ExcelModel.Workbook.Worksheets[ds], periodId);
            }

            HttpContext.Response.Clear();
            HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Response.AddHeader("content-disposition", "attachment;filename=" + modelName + "_" + periodId.ToString() + ".xlsx");

            using (var tmpMemoryStream = new MemoryStream())
            {
                ExcelModel.SaveAs(tmpMemoryStream);
                tmpMemoryStream.WriteTo(HttpContext.Response.OutputStream);
                tmpMemoryStream.Close();
            }
            HttpContext.Response.End();
            db.Dispose();
        }

        //download all datasource
        [HttpGet]
        public void DownloadAllDatasource(int periodId)
        {
            var ExcelModel = new ExcelPackage();

            var db = new CommissionCalculateEntities();
            // Get Datasource list
            var dsList = db.ModelDatasource.Where(x => x.Period_Id == periodId)
                .Select(x => x.DatasourceName)
                .Distinct().ToList();

            //Loop in list
            foreach (var ds in dsList)
            {
                //Create Sheet
                ExcelModel.Workbook.Worksheets.Add(ds);

                BindDatasourceSheet(ExcelModel.Workbook.Worksheets[ds], periodId);
            }

            HttpContext.Response.Clear();
            HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Response.AddHeader("content-disposition", "attachment;filename=Datasource_" + DateTime.Now.ToShortDateString() + "_" + periodId.ToString() + ".xlsx");

            using (var tmpMemoryStream = new MemoryStream())
            {
                ExcelModel.SaveAs(tmpMemoryStream);
                tmpMemoryStream.WriteTo(HttpContext.Response.OutputStream);
                tmpMemoryStream.Close();
            }
            HttpContext.Response.End();
            db.Dispose();
        }

        //download model
        [HttpGet]
        public void DownloadModel(int periodId, string modelName)
        {
            var ExcelModel = new ExcelPackage();

            var db = new CommissionCalculateEntities();

            //get modelId
            var modelId = db.Model.SingleOrDefault(x => x.ModelName == modelName).Id;

            // Get Datasource list
            var dsList = db.ModelDatasource.Where(x => x.Period_Id == periodId && x.Id == modelId)
                 .Select(x => x.DatasourceName)
                .Distinct().ToList();

            //Loop in list
            foreach (var ds in dsList)
            {
                //Create Sheet
                ExcelModel.Workbook.Worksheets.Add(ds);

                BindDatasourceSheet(ExcelModel.Workbook.Worksheets[ds], periodId);
            }

            HttpContext.Response.Clear();
            HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Response.AddHeader("content-disposition", "attachment;filename=" + modelName + "_" + periodId.ToString() + ".xlsx");

            using (var tmpMemoryStream = new MemoryStream())
            {
                ExcelModel.SaveAs(tmpMemoryStream);
                tmpMemoryStream.WriteTo(HttpContext.Response.OutputStream);
                tmpMemoryStream.Close();
            }
            HttpContext.Response.End();
            db.Dispose();
        }

        //bind data to sheet
        private bool BindDatasourceSheet(ExcelWorksheet sheet, int periodId)
        {
            //Bind Sheet
            //Use SQLConnection instead of entity framework
            //Because usp returns table with dynamic columns
            SqlConnection connection = new SqlConnection(new CommissionCalculateEntities().Database.Connection.ConnectionString);

            SqlCommand command = new SqlCommand("usp_GetDatasourceByTableNameAndPeriodId", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("TableName", sheet.Name));
            command.Parameters.Add(new SqlParameter("Period_Id", periodId.ToString()));
            connection.Open();

            //Get Datatable
            System.Data.DataTable dt = new System.Data.DataTable();

            dt.Load(command.ExecuteReader());

            connection.Close();

            //Bind Table To sheet
            if (dt.Rows.Count > 0)
            {
                sheet.Cells["A1"].LoadFromDataTable(dt, true);
                //sheet.Cell("A1").InsertTable(dt);
            }

            //Naming Ranges
            for (int i = 1; i < dt.Columns.Count + 1; i++)
            {
                //Get numberofrow
                var nameToRow = dt.Rows.Count + 1;
                //Get address
                var address = sheet.Cells[2, i, nameToRow, i].Address;
                //gat name
                var name = sheet.Name + "_" + sheet.Cells[1, i].Text;
                //Naming Range
                var nr = new ExcelNamedRange(name, null, sheet, address, 1);
                sheet.Workbook.Names.Add(name, nr);
            }

            //Naming Ranges for table
            var tableAddress = sheet.Cells[2, 1, dt.Rows.Count + 1, dt.Columns.Count].Address;
            var tableNamingRange = new ExcelNamedRange(sheet.Name, null, sheet, tableAddress, 1);
            sheet.Workbook.Names.Add(sheet.Name, tableNamingRange);

            return true;
        }

        #endregion CompensationHistory

        #endregion api
    }
}