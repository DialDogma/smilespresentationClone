using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using EntityFramework.Utilities;
using Newtonsoft.Json;
using OfficeOpenXml;

using SmileSCommissionDataImport.Helper;
using SmileSCommissionDataImport.Models;
using SmileSCommissionDataImport.Utils;

namespace SmileSCommissionDataImport.Controllers
{
    [Authorization]
    public class WeeklySaleTargetController : Controller
    {
        #region Declare Variables

        private readonly CommissionCalculateEntities _context;

        public WeeklySaleTargetController()
        {
            // Set entity
            _context = new CommissionCalculateEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // TODO: Prepare invalid model and extend model for this import
        public class WeeklySaleTargetInvalid : D_WeeklySaleTarget
        {
            public List<string> InvalidList { get; set; }
        }

        #endregion Declare Variables

        // GET: WeeklySaleTarget
        public ActionResult Index()
        {
            // TODO: Prepare valid and invalid model for initial table
            ViewBag.Valid = new List<D_WeeklySaleTarget>();
            ViewBag.ValidJson = "[]";
            ViewBag.Invalid = new List<WeeklySaleTargetInvalid>();
            ViewBag.InvalidJson = "[]";

            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var helpers = new ValidationHelpers();

            // TODO: Prepare valid and invalid model
            var valid = new List<D_WeeklySaleTarget>();
            var invalid = new List<WeeklySaleTargetInvalid>();

            // Check file
            if (file != null)
            {
                System.IO.Stream fileContent = file.InputStream;

                using (ExcelPackage excelPackage = new ExcelPackage(fileContent))
                {
                    // Select first worksheet
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];

                    var rows = ((object[,])worksheet.Cells.Value);

                    #region Validate Column

                    // TODO: Validate column, if not match not allow to process.

                    var columns = new List<String> {
                        "Code_Employee",
                        "EmployeeName",
                        "ID_Team",
                        "Team",
                        "ID_Branch",
                        "Branch",
                        "CountNewApp_PH_Week1",
                        "CountNewApp_PH_Week1_Status",
                        "CountNewApp_PH_Week2",
                        "CountNewApp_PH_Week2_Status",
                        "CountNewApp_PH_Week3",
                        "CountNewApp_PH_Week3_Status",
                        "CountNewApp_PH_Week4",
                        "CountNewApp_PH_Week4_Status",
                        "CountNewApp_PH_Week5",
                        "CountNewApp_PH_Week5_Status"
                    };

                    var missingColumn = helpers.ValidateColumn(rows, columns);

                    if (missingColumn.Count > 0)
                    {
                        ViewBag.isInvalidColumn = true;

                        ViewBag.isInvalidMsg = "ไม่พบ column : " + String.Join(", ", missingColumn);

                        ViewBag.Valid = new List<D_WeeklySaleTarget>();
                        ViewBag.ValidJson = "[]";
                        ViewBag.Invalid = new List<WeeklySaleTargetInvalid>();
                        ViewBag.InvalidJson = "[]";

                        return View("Index");
                    }

                    #endregion Validate Column

                    //loop all rows
                    for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row - 1; i++)
                    {
                        // TODO: Prepare valid and invalid model for each row
                        var rowResult = new D_WeeklySaleTarget();
                        var invalidList = new List<string>();

                        #region Validate Column

                        // TODO: Check type and require column from document condition

                        //Code_Employee
                        var Code_Employee = helpers.ValidateString("Code_Employee", rows[i, 0], true);

                        if (Code_Employee.IsValid) rowResult.Code_Employee = Code_Employee.Value;
                        if (!Code_Employee.IsValid) invalidList.Add(Code_Employee.InvalidMessage);

                        //EmployeeName
                        var EmployeeName = helpers.ValidateString("EmployeeName", rows[i, 1], true);

                        if (EmployeeName.IsValid) rowResult.EmployeeName = EmployeeName.Value;
                        if (!EmployeeName.IsValid) invalidList.Add(EmployeeName.InvalidMessage);

                        //ID_Team
                        var ID_Team = helpers.ValidateString("ID_Team", rows[i, 2], true);

                        if (ID_Team.IsValid) rowResult.ID_Team = ID_Team.Value;
                        if (!ID_Team.IsValid) invalidList.Add(ID_Team.InvalidMessage);

                        //Team
                        var Team = helpers.ValidateString("Team", rows[i, 3], true);

                        if (Team.IsValid) rowResult.Team = Team.Value;
                        if (!Team.IsValid) invalidList.Add(Team.InvalidMessage);

                        //ID_Branch
                        var ID_Branch = helpers.ValidateString("ID_Branch", rows[i, 4], true);

                        if (ID_Branch.IsValid) rowResult.ID_Branch = ID_Branch.Value;
                        if (!ID_Branch.IsValid) invalidList.Add(ID_Branch.InvalidMessage);

                        //Branch
                        var Branch = helpers.ValidateString("Branch", rows[i, 5], true);

                        if (Branch.IsValid) rowResult.Branch = Branch.Value;
                        if (!Branch.IsValid) invalidList.Add(Branch.InvalidMessage);

                        //CountNewApp_PH_Week1
                        var CountNewApp_PH_Week1 = helpers.ValidateInt("CountNewApp_PH_Week1", rows[i, 6], true);

                        if (CountNewApp_PH_Week1.IsValid) rowResult.CountNewApp_PH_Week1 = CountNewApp_PH_Week1.Value;
                        if (!CountNewApp_PH_Week1.IsValid) invalidList.Add(CountNewApp_PH_Week1.InvalidMessage);

                        //CountNewApp_PH_Week1_Status
                        var CountNewApp_PH_Week1_Status = helpers.ValidateString("CountNewApp_PH_Week1_Status", rows[i, 7], true);

                        if (CountNewApp_PH_Week1_Status.IsValid) rowResult.CountNewApp_PH_Week1_Status = CountNewApp_PH_Week1_Status.Value;
                        if (!CountNewApp_PH_Week1_Status.IsValid) invalidList.Add(CountNewApp_PH_Week1_Status.InvalidMessage);

                        //CountNewApp_PH_Week2
                        var CountNewApp_PH_Week2 = helpers.ValidateInt("CountNewApp_PH_Week2", rows[i, 8], true);

                        if (CountNewApp_PH_Week2.IsValid) rowResult.CountNewApp_PH_Week2 = CountNewApp_PH_Week2.Value;
                        if (!CountNewApp_PH_Week2.IsValid) invalidList.Add(CountNewApp_PH_Week2.InvalidMessage);

                        //CountNewApp_PH_Week2_Status
                        var CountNewApp_PH_Week2_Status = helpers.ValidateString("CountNewApp_PH_Week2_Status", rows[i, 9], true);

                        if (CountNewApp_PH_Week2_Status.IsValid) rowResult.CountNewApp_PH_Week2_Status = CountNewApp_PH_Week2_Status.Value;
                        if (!CountNewApp_PH_Week2_Status.IsValid) invalidList.Add(CountNewApp_PH_Week2_Status.InvalidMessage);

                        //CountNewApp_PH_Week3
                        var CountNewApp_PH_Week3 = helpers.ValidateInt("CountNewApp_PH_Week3", rows[i, 10], true);

                        if (CountNewApp_PH_Week3.IsValid) rowResult.CountNewApp_PH_Week3 = CountNewApp_PH_Week3.Value;
                        if (!CountNewApp_PH_Week3.IsValid) invalidList.Add(CountNewApp_PH_Week3.InvalidMessage);

                        //CountNewApp_PH_Week3_Status
                        var CountNewApp_PH_Week3_Status = helpers.ValidateString("CountNewApp_PH_Week3_Status", rows[i, 11], true);

                        if (CountNewApp_PH_Week3_Status.IsValid) rowResult.CountNewApp_PH_Week3_Status = CountNewApp_PH_Week3_Status.Value;
                        if (!CountNewApp_PH_Week3_Status.IsValid) invalidList.Add(CountNewApp_PH_Week3_Status.InvalidMessage);

                        //CountNewApp_PH_Week4
                        var CountNewApp_PH_Week4 = helpers.ValidateInt("CountNewApp_PH_Week4", rows[i, 12], true);

                        if (CountNewApp_PH_Week4.IsValid) rowResult.CountNewApp_PH_Week4 = CountNewApp_PH_Week4.Value;
                        if (!CountNewApp_PH_Week4.IsValid) invalidList.Add(CountNewApp_PH_Week4.InvalidMessage);

                        //CountNewApp_PH_Week4_Status
                        var CountNewApp_PH_Week4_Status = helpers.ValidateString("CountNewApp_PH_Week4_Status", rows[i, 13], true);

                        if (CountNewApp_PH_Week4_Status.IsValid) rowResult.CountNewApp_PH_Week4_Status = CountNewApp_PH_Week4_Status.Value;
                        if (!CountNewApp_PH_Week4_Status.IsValid) invalidList.Add(CountNewApp_PH_Week4_Status.InvalidMessage);

                        //CountNewApp_PH_Week5
                        var CountNewApp_PH_Week5 = helpers.ValidateInt("CountNewApp_PH_Week5", rows[i, 14], true);

                        if (CountNewApp_PH_Week5.IsValid) rowResult.CountNewApp_PH_Week5 = CountNewApp_PH_Week5.Value;
                        if (!CountNewApp_PH_Week5.IsValid) invalidList.Add(CountNewApp_PH_Week5.InvalidMessage);

                        //CountNewApp_PH_Week5_Status
                        var CountNewApp_PH_Week5_Status = helpers.ValidateString("CountNewApp_PH_Week5_Status", rows[i, 15], true);

                        if (CountNewApp_PH_Week5_Status.IsValid) rowResult.CountNewApp_PH_Week5_Status = CountNewApp_PH_Week5_Status.Value;
                        if (!CountNewApp_PH_Week5_Status.IsValid) invalidList.Add(CountNewApp_PH_Week5_Status.InvalidMessage);

                        // column periodId
                        var periodResult = GlobalFunction.GetLatestPeriod();
                        rowResult.Period_Id = periodResult.periodId;

                        #endregion Validate Column

                        // Check Valid Row
                        if (invalidList.Count == 0)
                        {
                            valid.Add(rowResult);
                        }
                        else
                        {
                            // TODO: Map valid item to invalid
                            var config = new MapperConfiguration(cfg =>
                            {
                                cfg.CreateMap<D_WeeklySaleTarget, WeeklySaleTargetInvalid>();
                            });

                            IMapper mapper = config.CreateMapper();
                            var dest = mapper.Map<D_WeeklySaleTarget, WeeklySaleTargetInvalid>(rowResult);
                            dest.InvalidList = invalidList;

                            // Add item to invalid list
                            invalid.Add(dest);
                        }
                    }
                }
            }

            // TODO: Call webservice to trigger log and update status

            if (invalid.Count == 0)
            {
                var tmpValid = valid.ToList();

                #region Check Duplicate

                // TODO: Check Duplicate in File
                var getDupplidateInFile = valid.GroupBy(x => x.Code_Employee.Trim()).Where(x => x.Count() > 1)
                    .Select(x => x.Key).ToList();

                // TODO: Find in valid
                foreach (var item in valid.Where(x => getDupplidateInFile.Any(y => y.Equals(x.Code_Employee.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_WeeklySaleTarget, WeeklySaleTargetInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_WeeklySaleTarget, WeeklySaleTargetInvalid>(item);
                    dest.InvalidList = new List<string>() { "Dupplicate column Code_Employee in upload file" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                //// TODO: Check Duplicate in database

                //var existingDupplicate = _context.WeeklySaleTargets.SqlQuery(
                //    string.Format("SELECT * FROM dbo.WeeklySaleTarget WHERE Code_Employee IN ('{0}')",
                //        string.Join("','", tmpValid.Select(x => x.Code_Employee).ToArray()))).ToList();

                //// TODO: Find in exist invalid
                //foreach (var item in invalid.Where(x => existingDupplicate.Any(y => y.Code_Employee.Trim().Equals(x.Code_Employee.Trim()))).ToList())
                //{
                //    item.InvalidList.Add("Dupplicate column Code_Employee in database");
                //}

                //// TODO: Find in valid
                //foreach (var item in valid.Where(x => existingDupplicate.Any(y => y.Code_Employee.Trim().Equals(x.Code_Employee.Trim()))).ToList())
                //{
                //    // Remove item from valid list
                //    valid.Remove(item);

                //    // Map valid item to invalid
                //    var config = new MapperConfiguration(cfg =>
                //    {
                //        cfg.CreateMap<WeeklySaleTarget, WeeklySaleTargetInvalid>();
                //    });

                //    IMapper mapper = config.CreateMapper();
                //    var dest = mapper.Map<WeeklySaleTarget, WeeklySaleTargetInvalid>(item);
                //    dest.InvalidList = new List<string>() { "Dupplicate column Code_Employee in database" };

                //    // Add item to invalid list
                //    invalid.Add(dest);
                //}

                #endregion Check Duplicate

                #region Validate EmployeeSS

                var notExistEmployeeSS = helpers.ValidateEmployeeSS(tmpValid.Where(x => !string.IsNullOrEmpty(x.Code_Employee)).Select(x => x.Code_Employee.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistEmployeeSS.Any(y => y.Equals(x.Code_Employee.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found Code_Employee");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistEmployeeSS.Any(y => y.Equals(x.Code_Employee.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_WeeklySaleTarget, WeeklySaleTargetInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_WeeklySaleTarget, WeeklySaleTargetInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found Code_Employee" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate EmployeeSS

                #region Validate Team

                var notExistTeam = helpers.ValidateTeam(tmpValid.Where(x => !string.IsNullOrEmpty(x.ID_Team)).Select(x => x.ID_Team.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistTeam.Any(y => y.Equals(x.ID_Team.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found ID_Team");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistTeam.Any(y => y.Equals(x.ID_Team.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_WeeklySaleTarget, WeeklySaleTargetInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_WeeklySaleTarget, WeeklySaleTargetInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found ID_Team" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Team

                #region Validate Branch

                var notExistBranch = helpers.ValidateBranch(tmpValid.Where(x => !string.IsNullOrEmpty(x.ID_Branch)).Select(x => x.ID_Branch.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistBranch.Any(y => y.Equals(x.ID_Branch.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found ID_Branch");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistBranch.Any(y => y.Equals(x.ID_Branch.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_WeeklySaleTarget, WeeklySaleTargetInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_WeeklySaleTarget, WeeklySaleTargetInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found ID_Branch" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Branch
            }

            // Set data for pageview
            ViewBag.isInvalidColumn = false;

            ViewBag.Valid = valid;
            ViewBag.ValidJson = JsonConvert.SerializeObject(valid);

            ViewBag.Invalid = invalid;
            ViewBag.InvalidJson = JsonConvert.SerializeObject(invalid);

            // Generate key for mapping data
            var dataKey = Guid.NewGuid().ToString();

            ViewBag.DataKey = dataKey;

            // Save valid data to session
            Session[dataKey] = valid;

            // Session will expire in 30 minutes
            Session.Timeout = 30;

            return View("Index");
        }

        [HttpPost]
        public JsonResult Save(string dataKey)
        {
            var periodResult = GlobalFunction.GetLatestPeriod();
            var latestPeriodList = _context.D_WeeklySaleTarget.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_WeeklySaleTarget.Remove(itm);
                    _context.SaveChanges();
                }
            }

            // Get data from session by datakey
            var valid = (List<D_WeeklySaleTarget>)Session[dataKey];

            EFBatchOperation.For(_context, _context.D_WeeklySaleTarget).InsertAll(valid);

            //call sp to update import log
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogManual_Update(periodResult.periodId, "D_WeeklySaleTarget", user);

            Session[dataKey] = null;

            return Json("บันทึกข้อมูลเรียบร้อย");
        }

        [HttpPost]
        public JsonResult SaveNoData()
        {
            var periodResult = GlobalFunction.GetLatestPeriod();

            var latestPeriodList = _context.D_WeeklySaleTarget.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_WeeklySaleTarget.Remove(itm);
                    _context.SaveChanges();
                }
            }

            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogNoData_Update(periodResult.periodId, "D_WeeklySaleTarget", user);

            return Json("บันทึกข้อมูลเรียบร้อย");
        }
    }
}