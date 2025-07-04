using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using EntityFramework.Utilities;
using OfficeOpenXml;
using Authorization = SmileSCommissionDataImport.Helper.Authorization;
using SmileSCommissionDataImport.Helper;
using SmileSCommissionDataImport.Models;
using SmileSCommissionDataImport.Utils;

namespace SmileSCommissionDataImport.Controllers
{
    [Authorization]
    public class Area_Calculation_LastMonthController : Controller
    {
        #region Declare Variables

        private readonly CommissionCalculateEntities _context;

        public Area_Calculation_LastMonthController()
        {
            // Set entity
            _context = new CommissionCalculateEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // Prepare invalid model and extend model for this import
        public class Area_Calculation_LastMonthInvalid : D_Area_Calculation_LastMonth
        {
            public List<string> InvalidList { get; set; }
        }

        #endregion Declare Variables

        public ActionResult Index()
        {
            // Prepare valid and invalid model for initial table
            ViewBag.Valid = new List<D_Area_Calculation_LastMonth>();
            ViewBag.ValidJson = "[]";
            ViewBag.Invalid = new List<Area_Calculation_LastMonthInvalid>();
            ViewBag.InvalidJson = "[]";

            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var helpers = new ValidationHelpers();

            // TODO: Prepare valid and invalid model
            var valid = new List<D_Area_Calculation_LastMonth>();
            var invalid = new List<Area_Calculation_LastMonthInvalid>();

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
                        "MainKey",
                        "Area",
                        "Best_Loss_Claim_Area",
                        "Best_Lapse_Area",
                        "Balance_All_LastMonth",
                        "Balance_PH_All_LastMonth",
                        "Balance_MP_All_LastMonth"
                    };

                    var missingColumn = helpers.ValidateColumn(rows, columns);

                    if (missingColumn.Count > 0)
                    {
                        ViewBag.isInvalidColumn = true;

                        ViewBag.isInvalidMsg = "ไม่พบ column : " + String.Join(", ", missingColumn);

                        ViewBag.Valid = new List<D_Area_Calculation_LastMonth>();
                        ViewBag.ValidJson = "[]";
                        ViewBag.Invalid = new List<Area_Calculation_LastMonthInvalid>();
                        ViewBag.InvalidJson = "[]";

                        return View("Index");
                    }

                    #endregion Validate Column

                    //loop all rows
                    for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row - 1; i++)
                    {
                        // TODO: Prepare valid and invalid model for each row
                        var rowResult = new D_Area_Calculation_LastMonth();
                        var invalidList = new List<string>();

                        #region Validate Column

                        // TODO: Check type and require column from document condition

                        // Column MainKey
                        var MainKey = helpers.ValidateString("MainKey", rows[i, 0], true);

                        if (MainKey.IsValid) rowResult.MainKey = MainKey.Value;
                        if (!MainKey.IsValid) invalidList.Add(MainKey.InvalidMessage);

                        // Column Area
                        var Area = helpers.ValidateString("Area", rows[i, 1], true);

                        if (Area.IsValid) rowResult.Area = Area.Value;
                        if (!Area.IsValid) invalidList.Add(Area.InvalidMessage);

                        // Column Best_Loss_Claim_Area
                        var Best_Loss_Claim_Area = helpers.ValidateFloat("Best_Loss_Claim_Area", rows[i, 2], true);

                        if (Best_Loss_Claim_Area.IsValid) rowResult.Best_Loss_Claim_Area = Best_Loss_Claim_Area.Value;
                        if (!Best_Loss_Claim_Area.IsValid) invalidList.Add(Best_Loss_Claim_Area.InvalidMessage);

                        // Column Best_Lapse_Area
                        var Best_Lapse_Area = helpers.ValidateFloat("Best_Lapse_Area", rows[i, 3], true);

                        if (Best_Lapse_Area.IsValid) rowResult.Best_Lapse_Area = Best_Lapse_Area.Value;
                        if (!Best_Lapse_Area.IsValid) invalidList.Add(Best_Lapse_Area.InvalidMessage);

                        // Column Balance_All_LastMonth
                        var Balance_All_LastMonth = helpers.ValidateInt("Balance_All_LastMonth", rows[i, 4], true);

                        if (Balance_All_LastMonth.IsValid) rowResult.Balance_All_LastMonth = Balance_All_LastMonth.Value;
                        if (!Balance_All_LastMonth.IsValid) invalidList.Add(Balance_All_LastMonth.InvalidMessage);

                        // Column Balance_PH_All_LastMonth
                        var Balance_PH_All_LastMonth = helpers.ValidateInt("Balance_PH_All_LastMonth", rows[i, 5], true);

                        if (Balance_PH_All_LastMonth.IsValid) rowResult.Balance_PH_All_LastMonth = Balance_PH_All_LastMonth.Value;
                        if (!Balance_PH_All_LastMonth.IsValid) invalidList.Add(Balance_PH_All_LastMonth.InvalidMessage);

                        // Column Balance_PH_All_LastMonth
                        var Balance_MP_All_LastMonth = helpers.ValidateInt("Balance_MP_All_LastMonth", rows[i, 6], true);

                        if (Balance_MP_All_LastMonth.IsValid) rowResult.Balance_MP_All_LastMonth = Balance_MP_All_LastMonth.Value;
                        if (!Balance_MP_All_LastMonth.IsValid) invalidList.Add(Balance_MP_All_LastMonth.InvalidMessage);

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
                                cfg.CreateMap<D_Area_Calculation_LastMonth, Area_Calculation_LastMonthInvalid>();
                            });

                            IMapper mapper = config.CreateMapper();
                            var dest = mapper.Map<D_Area_Calculation_LastMonth, Area_Calculation_LastMonthInvalid>(rowResult);
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
                var getDupplidateInFile = valid.GroupBy(x => x.MainKey.Trim()).Where(x => x.Count() > 1)
                    .Select(x => x.Key).ToList();

                // TODO: Find in valid
                foreach (var item in valid.Where(x => getDupplidateInFile.Any(y => y.Equals(x.MainKey.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_Area_Calculation_LastMonth, Area_Calculation_LastMonthInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_Area_Calculation_LastMonth, Area_Calculation_LastMonthInvalid>(item);
                    dest.InvalidList = new List<string>() { "Dupplicate column MainKey in upload file" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                //// TODO: Check Duplicate in database

                //var existingDupplicate = _context.Area_Calculation_LastMonth.SqlQuery(
                //    string.Format("SELECT * FROM dbo.Area_Calculation_LastMonth WHERE MainKey IN ('{0}')",
                //        string.Join("','", tmpValid.Select(x => x.MainKey).ToArray()))).ToList();

                //// TODO: Find in exist invalid
                //foreach (var item in invalid.Where(x => existingDupplicate.Any(y => y.MainKey.Trim().Equals(x.MainKey.Trim()))).ToList())
                //{
                //    item.InvalidList.Add("Dupplicate column MainKey in database");
                //}

                //// TODO: Find in valid
                //foreach (var item in valid.Where(x => existingDupplicate.Any(y => y.MainKey.Trim().Equals(x.MainKey.Trim()))).ToList())
                //{
                //    // Remove item from valid list
                //    valid.Remove(item);

                //    // Map valid item to invalid
                //    var config = new MapperConfiguration(cfg =>
                //    {
                //        cfg.CreateMap<Area_Calculation_LastMonth, Area_Calculation_LastMonthInvalid>();
                //    });

                //    IMapper mapper = config.CreateMapper();
                //    var dest = mapper.Map<Area_Calculation_LastMonth, Area_Calculation_LastMonthInvalid>(item);
                //    dest.InvalidList = new List<string>() { "Dupplicate column MainKey in database" };

                //    // Add item to invalid list
                //    invalid.Add(dest);
                //}

                #endregion Check Duplicate

                #region Validate Area

                var notExistArea = helpers.ValidateArea(tmpValid.Where(x => !string.IsNullOrEmpty(x.MainKey)).Select(x => x.MainKey.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistArea.Any(y => y.Equals(x.MainKey.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found MainKey");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistArea.Any(y => y.Equals(x.MainKey.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_Area_Calculation_LastMonth, Area_Calculation_LastMonthInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_Area_Calculation_LastMonth, Area_Calculation_LastMonthInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found MainKey" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Area
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

            var latestPeriodList = _context.D_Area_Calculation_LastMonth.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_Area_Calculation_LastMonth.Remove(itm);
                    _context.SaveChanges();
                }
            }

            // Get data from session by datakey
            var valid = (List<D_Area_Calculation_LastMonth>)Session[dataKey];

            EFBatchOperation.For(_context, _context.D_Area_Calculation_LastMonth).InsertAll(valid);

            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogManual_Update(periodResult.periodId, "D_Area_Calculation_LastMonth", user);

            Session[dataKey] = null;

            return Json("บันทึกข้อมูลเรียบร้อย");
        }

        [HttpPost]
        public JsonResult SaveNoData()
        {
            var periodResult = GlobalFunction.GetLatestPeriod();

            var latestPeriodList = _context.D_Area_Calculation_LastMonth.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_Area_Calculation_LastMonth.Remove(itm);
                    _context.SaveChanges();
                }
            }

            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogNoData_Update(periodResult.periodId, "D_Area_Calculation_LastMonth", user);

            return Json("บันทึกข้อมูลเรียบร้อย");
        }
    }
}