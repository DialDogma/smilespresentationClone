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
    public class OperatingCapitalController : Controller
    {
        #region Declare Variables

        private readonly CommissionCalculateEntities _context;

        public OperatingCapitalController()
        {
            // Set entity
            _context = new CommissionCalculateEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // TODO: Prepare invalid model and extend model for this import
        public class OperatingCapitalInvalid : D_OperatingCapital
        {
            public List<string> InvalidList { get; set; }
        }

        #endregion Declare Variables

        // GET: OperatingCapital
        public ActionResult Index()
        {
            // TODO: Prepare valid and invalid model for initial table
            ViewBag.Valid = new List<D_OperatingCapital>();
            ViewBag.ValidJson = "[]";
            ViewBag.Invalid = new List<OperatingCapitalInvalid>();
            ViewBag.InvalidJson = "[]";
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var helpers = new ValidationHelpers();

            // TODO: Prepare valid and invalid model
            var valid = new List<D_OperatingCapital>();
            var invalid = new List<OperatingCapitalInvalid>();

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
                        "SS ID",
                        "HCM ID",
                        "EmployeeName",
                        "EmployeeStatus",
                        "BranchCode",
                        "Branch",
                        "TeamCode",
                        "Team",
                        "AreaCode",
                        "Area",
                        "MSNID",
                        "Position Level",
                        "Position Abbr.",
                        "Position Abbr. SSS",
                        "OperatingCapital"
                    };

                    var missingColumn = helpers.ValidateColumn(rows, columns);

                    if (missingColumn.Count > 0)
                    {
                        ViewBag.isInvalidColumn = true;

                        ViewBag.isInvalidMsg = "ไม่พบ column : " + String.Join(", ", missingColumn);

                        ViewBag.Valid = new List<D_OperatingCapital>();
                        ViewBag.ValidJson = "[]";
                        ViewBag.Invalid = new List<OperatingCapitalInvalid>();
                        ViewBag.InvalidJson = "[]";

                        return View("Index");
                    }

                    #endregion Validate Column

                    //loop all rows
                    for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row - 1; i++)
                    {
                        // TODO: Prepare valid and invalid model for each row
                        var rowResult = new D_OperatingCapital();
                        var invalidList = new List<string>();

                        #region Validate Column

                        // TODO: Check type and require column from document condition

                        // Column SS_ID
                        var SS_ID = helpers.ValidateString("SS ID", rows[i, 0], true);

                        if (SS_ID.IsValid) rowResult.SS_ID = SS_ID.Value;
                        if (!SS_ID.IsValid) invalidList.Add(SS_ID.InvalidMessage);

                        // Column HCM_ID
                        var HCM_ID = helpers.ValidateString("HCM ID", rows[i, 1], true);

                        if (HCM_ID.IsValid) rowResult.HCM_ID = HCM_ID.Value;
                        if (!HCM_ID.IsValid) invalidList.Add(HCM_ID.InvalidMessage);

                        // Column EmployeeName
                        var EmployeeName = helpers.ValidateString("EmployeeName", rows[i, 2], true);

                        if (EmployeeName.IsValid) rowResult.EmployeeName = EmployeeName.Value;
                        if (!EmployeeName.IsValid) invalidList.Add(EmployeeName.InvalidMessage);

                        // Column EmployeeStatus
                        var EmployeeStatus = helpers.ValidateString("EmployeeStatus", rows[i, 3], true);

                        if (EmployeeStatus.IsValid) rowResult.EmployeeStatus = EmployeeStatus.Value;
                        if (!EmployeeStatus.IsValid) invalidList.Add(EmployeeStatus.InvalidMessage);

                        // Column BranchCode
                        var BranchCode = helpers.ValidateString("BranchCode", rows[i, 4], true);

                        if (BranchCode.IsValid) rowResult.BranchCode = BranchCode.Value;
                        if (!BranchCode.IsValid) invalidList.Add(BranchCode.InvalidMessage);

                        // Column Branch
                        var Branch = helpers.ValidateString("Branch", rows[i, 5], true);

                        if (Branch.IsValid) rowResult.Branch = Branch.Value;
                        if (!Branch.IsValid) invalidList.Add(Branch.InvalidMessage);

                        // Column TeamCode
                        var TeamCode = helpers.ValidateString("TeamCode", rows[i, 6], false);

                        if (TeamCode.IsValid) rowResult.TeamCode = TeamCode.Value;
                        if (!TeamCode.IsValid) invalidList.Add(TeamCode.InvalidMessage);

                        // Column Team
                        var Team = helpers.ValidateString("Team", rows[i, 7], false);

                        if (Team.IsValid) rowResult.Team = Team.Value;
                        if (!Team.IsValid) invalidList.Add(Team.InvalidMessage);

                        // Column AreaCode
                        var AreaCode = helpers.ValidateString("AreaCode", rows[i, 8], false);

                        if (AreaCode.IsValid) rowResult.AreaCode = AreaCode.Value;
                        if (!AreaCode.IsValid) invalidList.Add(AreaCode.InvalidMessage);

                        // Column Area
                        var Area = helpers.ValidateString("Area", rows[i, 9], false);

                        if (Area.IsValid) rowResult.Area = Area.Value;
                        if (!Area.IsValid) invalidList.Add(Area.InvalidMessage);

                        // Column MSNID
                        var MSNID = helpers.ValidateString("MSNID", rows[i, 10], false);

                        if (MSNID.IsValid) rowResult.MSNID = MSNID.Value;
                        if (!MSNID.IsValid) invalidList.Add(MSNID.InvalidMessage);

                        // Column Position_Level
                        var Position_Level = helpers.ValidateString("Position Level", rows[i, 11], true);

                        if (Position_Level.IsValid) rowResult.Position_Level = Position_Level.Value;
                        if (!Position_Level.IsValid) invalidList.Add(Position_Level.InvalidMessage);

                        // Column Position_Abbr
                        var Position_Abbr = helpers.ValidateString("Position Abbr.", rows[i, 12], true);

                        if (Position_Abbr.IsValid) rowResult.Position_Abbr = Position_Abbr.Value;
                        if (!Position_Abbr.IsValid) invalidList.Add(Position_Abbr.InvalidMessage);

                        // Column Position_Abbr_SSS
                        var Position_Abbr_SSS = helpers.ValidateString("Position Abbr. SSS", rows[i, 13], true);

                        if (Position_Abbr_SSS.IsValid) rowResult.Position_Abbr_SSS = Position_Abbr_SSS.Value;
                        if (!Position_Abbr_SSS.IsValid) invalidList.Add(Position_Abbr_SSS.InvalidMessage);

                        // Column OperatingCapital
                        var OperatingCapital = helpers.ValidateInt("OperatingCapital", rows[i, 14], true);

                        if (OperatingCapital.IsValid) rowResult.OperatingCapital = OperatingCapital.Value;
                        if (!OperatingCapital.IsValid) invalidList.Add(OperatingCapital.InvalidMessage);

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
                                cfg.CreateMap<D_OperatingCapital, OperatingCapitalInvalid>();
                            });

                            IMapper mapper = config.CreateMapper();
                            var dest = mapper.Map<D_OperatingCapital, OperatingCapitalInvalid>(rowResult);
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
                var getDupplidateInFile = valid.GroupBy(x => x.SS_ID.Trim()).Where(x => x.Count() > 1)
                    .Select(x => x.Key).ToList();

                // TODO: Find in valid
                foreach (var item in valid.Where(x => getDupplidateInFile.Any(y => y.Equals(x.SS_ID.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_OperatingCapital, OperatingCapitalInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_OperatingCapital, OperatingCapitalInvalid>(item);
                    dest.InvalidList = new List<string>() { "Dupplicate column SS_ID in upload file" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                //// TODO: Check Duplicate in database

                //var existingDupplicate = _context.OperatingCapitals.SqlQuery(
                //    string.Format("SELECT * FROM dbo.OperatingCapital WHERE SS_ID IN ('{0}')",
                //        string.Join("','", tmpValid.Select(x => x.SS_ID).ToArray()))).ToList();

                //// TODO: Find in exist invalid
                //foreach (var item in invalid.Where(x => existingDupplicate.Any(y => y.SS_ID.Trim().Equals(x.SS_ID.Trim()))).ToList())
                //{
                //    item.InvalidList.Add("Dupplicate column SS_ID in database");
                //}

                //// TODO: Find in valid
                //foreach (var item in valid.Where(x => existingDupplicate.Any(y => y.SS_ID.Trim().Equals(x.SS_ID.Trim()))).ToList())
                //{
                //    // Remove item from valid list
                //    valid.Remove(item);

                //    // Map valid item to invalid
                //    var config = new MapperConfiguration(cfg =>
                //    {
                //        cfg.CreateMap<OperatingCapital, OperatingCapitalInvalid>();
                //    });

                //    IMapper mapper = config.CreateMapper();
                //    var dest = mapper.Map<OperatingCapital, OperatingCapitalInvalid>(item);
                //    dest.InvalidList = new List<string>() { "Dupplicate column SS_ID in database" };

                //    // Add item to invalid list
                //    invalid.Add(dest);
                //}

                #endregion Check Duplicate

                #region Validate EmployeeSS

                var notExistEmployeeSS = helpers.ValidateEmployeeSS(tmpValid.Where(x => !string.IsNullOrEmpty(x.SS_ID)).Select(x => x.SS_ID.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistEmployeeSS.Any(y => y.Equals(x.SS_ID.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found SS_ID");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistEmployeeSS.Any(y => y.Equals(x.SS_ID.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_OperatingCapital, OperatingCapitalInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_OperatingCapital, OperatingCapitalInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found SS_ID" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate EmployeeSS

                #region Validate HC

                var notExistHC = helpers.ValidateHC(tmpValid.Where(x => !string.IsNullOrEmpty(x.HCM_ID)).Select(x => x.HCM_ID.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistHC.Any(y => y.Equals(x.HCM_ID.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found HCM_ID");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistHC.Any(y => y.Equals(x.HCM_ID.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_OperatingCapital, OperatingCapitalInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_OperatingCapital, OperatingCapitalInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found HCM_ID" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate HC

                #region Validate Branch

                var notExistBranch = helpers.ValidateBranch(tmpValid.Where(x => !string.IsNullOrEmpty(x.BranchCode)).Select(x => x.BranchCode.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistBranch.Any(y => y.Equals(x.BranchCode.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found BranchCode");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistBranch.Any(y => y.Equals(x.BranchCode.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_OperatingCapital, OperatingCapitalInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_OperatingCapital, OperatingCapitalInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found BranchCode" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Branch

                #region Validate Team

                var notExistTeam = helpers.ValidateTeam(tmpValid.Where(x => !string.IsNullOrEmpty(x.TeamCode)).Select(x => x.TeamCode.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistTeam.Any(y => y.Equals(x.TeamCode))).ToList())
                {
                    item.InvalidList.Add("Not found TeamCode");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistTeam.Any(y => y.Equals(x.TeamCode))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_OperatingCapital, OperatingCapitalInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_OperatingCapital, OperatingCapitalInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found TeamCode" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Team

                #region Validate Area

                var notExistArea = helpers.ValidateArea(tmpValid.Where(x => !string.IsNullOrEmpty(x.AreaCode)).Select(x => x.AreaCode.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistArea.Any(y => y.Equals(x.AreaCode))).ToList())
                {
                    item.InvalidList.Add("Not found AreaCode");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistArea.Any(y => y.Equals(x.AreaCode))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_OperatingCapital, OperatingCapitalInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_OperatingCapital, OperatingCapitalInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found AreaCode" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Area

                #region Validate EmployeeSS

                var notExistEmployeeSS2 = helpers.ValidateEmployeeSS(tmpValid.Where(x => !string.IsNullOrEmpty(x.MSNID)).Select(x => x.MSNID.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistEmployeeSS2.Any(y => y.Equals(x.MSNID))).ToList())
                {
                    item.InvalidList.Add("Not found MSNID");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistEmployeeSS2.Any(y => y.Equals(x.MSNID))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_OperatingCapital, OperatingCapitalInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_OperatingCapital, OperatingCapitalInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found MSNID" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate EmployeeSS
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
            var latestPeriodList = _context.D_OperatingCapital.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_OperatingCapital.Remove(itm);
                    _context.SaveChanges();
                }
            }

            // Get data from session by datakey
            var valid = (List<D_OperatingCapital>)Session[dataKey];

            EFBatchOperation.For(_context, _context.D_OperatingCapital).InsertAll(valid);

            //call sp to update import log
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogManual_Update(periodResult.periodId, "D_OperatingCapital", user);

            Session[dataKey] = null;

            return Json("บันทึกข้อมูลเรียบร้อย");
        }

        [HttpPost]
        public JsonResult SaveNoData()
        {
            var periodResult = GlobalFunction.GetLatestPeriod();

            var latestPeriodList = _context.D_OperatingCapital.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_OperatingCapital.Remove(itm);
                    _context.SaveChanges();
                }
            }

            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogNoData_Update(periodResult.periodId, "D_OperatingCapital", user);

            return Json("บันทึกข้อมูลเรียบร้อย");
        }
    }
}