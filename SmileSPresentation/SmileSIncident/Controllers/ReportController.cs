using OfficeOpenXml;
using OfficeOpenXml.Style;
using SmileSIncident.Helper;
using SmileSIncident.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SmileSIncident.Controllers
{
    [Authorization]
    public class ReportController : Controller
    {
        #region dbCon

        private readonly IncidentEntities _context;
        private static readonly CultureInfo _dtEn = new CultureInfo("en-US");
        private static readonly CultureInfo _dtTh = new CultureInfo("th-TH");

        public ReportController()

        {
            _context = new IncidentEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion dbCon

        #region View

        // GET: Report
        [Authorization(Roles = "Developer,IncidentDev")]
        public async Task<ActionResult> ReportIncident()
        {
            // Get dev list
            var lstAssignee = await Task.FromResult(_context.usp_Employee_Select(null, null, "6,7").ToList());
            ViewBag.devList = lstAssignee;

            //get incident job status
            var jobStatusList = await Task.FromResult(_context.usp_IncidentJobStatus_Select().ToList());
            ViewBag.jobStatusList = jobStatusList;

            //get status List
            var statusList = await Task.FromResult(_context.usp_IncidentTaskStatus_Select().ToList());
            ViewBag.statusList = statusList;

            //get incident group type list
            var incidentGroupType = await Task.FromResult(_context.usp_IncidentGroupType_Select(null).ToList());
            ViewBag.IncidentTypeList = incidentGroupType;

            return View();
        }

        #endregion View

        #region Method

        //Get Report
        [HttpPost]
        public async Task<ActionResult> GetReportIncident(int draw,
                                              string updateDateFrom,
                                              string updateDateTo,
                                              int? incidentGroupTypeId,
                                              int? actionByUserid,
                                              int? actionByUserDepartmentId,
                                              int? incidentTaskStatusId,
                                              int? incidentJobStatusId,
                                              int indexStart,
                                              int pageSize,
                                              string sortField,
                                              string orderType,
                                              string search)
        {
            //convert to datetime
            var dateFrom = Convert.ToDateTime(updateDateFrom);
            var dateTo = Convert.ToDateTime(updateDateTo);

            //check 0 = null
            if (incidentGroupTypeId == 0) incidentGroupTypeId = null;
            if (actionByUserid == 0) actionByUserid = null;
            if (actionByUserDepartmentId == 0) actionByUserDepartmentId = null;
            if (incidentTaskStatusId == 0) incidentTaskStatusId = null;
            if (incidentJobStatusId == 0) incidentJobStatusId = null;
            //call stored procedure
            var result = await Task.FromResult(_context.usp_IncidentReport_Select(dateFrom,
                                                         dateTo,
                                                         incidentGroupTypeId,
                                                         actionByUserid,
                                                         actionByUserDepartmentId,
                                                         incidentTaskStatusId,
                                                         incidentJobStatusId,
                                                         indexStart,
                                                         pageSize,
                                                         sortField,
                                                         orderType,
                                                         search).ToList());
            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().Total : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().Total : result.Count()},
                {"data", result.ToList()}
            };

            //return json result
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        #endregion Method

        #region Excel

        [HttpGet]
        public async Task<ActionResult> ExportReportIncident(string updateDateFrom,
                                              string updateDateTo,
                                              int? incidentGroupTypeId,
                                              int? actionByUserid,
                                              int? actionByUserDepartmentId,
                                              int? incidentTaskStatusId,
                                              int? incidentJobStatusId,
                                              string search)
        {
            using (var db = new IncidentEntities())
            {
                //convert to datetime
                var dateFrom = Convert.ToDateTime(updateDateFrom);
                var dateTo = Convert.ToDateTime(updateDateTo);

                //check 0 = null
                if (incidentGroupTypeId == 0) incidentGroupTypeId = null;
                if (actionByUserid == 0) actionByUserid = null;
                if (actionByUserDepartmentId == 0) actionByUserDepartmentId = null;
                if (incidentTaskStatusId == 0) incidentTaskStatusId = null;
                if (incidentJobStatusId == 0) incidentJobStatusId = null;

                var lst1 = await Task.FromResult(db.usp_IncidentReport_Select(dateFrom, dateTo, incidentGroupTypeId, actionByUserid, actionByUserDepartmentId, incidentTaskStatusId, incidentJobStatusId, 0, 999999, null, null, search).ToList());

                var output = lst1.Select(x => new
                {
                    IncidentTaskCode = x.IncidentTaskCode,
                    IncidentSubType = x.IncidentSubType,
                    IncidentType = x.IncidentType,
                    IncidentSubject = x.IncidentSubject,
                    IncidentTaskStatus = x.IncidentTaskStatus,
                    TaskCreatedByCode = x.TaskCreatedByCode,
                    TaskCreatedByName = x.TaskCreatedByName,
                    Branch = x.Branch,
                    Department = x.Department,
                    Company = x.Company,
                    Abbreviation = x.Abbreviation,
                    TaskCreatedDate = x.TaskCreatedDate?.ToString("dd/MM/yyyy HH:mm:ss"),
                    TaskUpdatedDate = x.TaskUpdatedDate?.ToString("dd/MM/yyyy HH:mm:ss"),
                    IncidentJobCode = x.IncidentJobCode,
                    JobDescription = x.JobDescription,
                    JobRemark = x.JobRemark,
                    JobPersonCode = x.JobPersonCode,
                    JobPersonName = x.JobPersonName,
                    JobPersonDepartment = x.JobPersonDepartment,
                    JobStatus = x.JobStatus,
                    JobCreatedDate = x.JobCreatedDate?.ToString("dd/MM/yyyy HH:mm:ss"),
                    JobUpdatedDate = x.JobUpdatedDate?.ToString("dd/MM/yyyy HH:mm:ss"),
                    SetTime = x.SetTime,
                    OverDue = x.OverDue,
                    ProcessingTime = x.ProcessingTime
                }).ToList();

                var stream = new MemoryStream();

                using (var package = new ExcelPackage(stream))
                {
                    var workSheet1 = package.Workbook.Worksheets.Add("Detail");
                    workSheet1.Cells.LoadFromCollection(output, true);

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

                    //get new GUID
                    var dataSessionKey = Guid.NewGuid().ToString();
                    //tempData GUID
                    TempData["keyReportIncident"] = dataSessionKey;
                    //Session Data
                    Session[dataSessionKey] = package.GetAsByteArray();

                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// Download File
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> DownloadExportReportIncident()
        {
            var dataKey = TempData["keyReportIncident"].ToString();

            if (Session[dataKey] != null)
            {
                byte[] data = Session[dataKey] as byte[];
                string excelName = $"Report-Incident-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return await Task.FromResult(File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName));
            }
            else
            {
                return new EmptyResult();
            }
        }

        #endregion Excel
    }
}