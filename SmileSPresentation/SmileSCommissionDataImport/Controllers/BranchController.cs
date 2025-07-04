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
    public class BranchController : Controller
    {
        #region Declare Variables

        private readonly CommissionCalculateEntities _context;

        public BranchController()
        {
            // Set entity
            _context = new CommissionCalculateEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // TODO: Prepare invalid model and extend model for this import
        public class BranchInvalid : D_Branch
        {
            public List<string> InvalidList { get; set; }
        }

        #endregion Declare Variables

        // GET: Branch
        public ActionResult Index()
        {
            // TODO: Prepare valid and invalid model for initial table
            ViewBag.Valid = new List<D_Branch>();
            ViewBag.ValidJson = "[]";
            ViewBag.Invalid = new List<BranchInvalid>();
            ViewBag.InvalidJson = "[]";

            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var helpers = new ValidationHelpers();

            // TODO: Prepare valid and invalid model
            var valid = new List<D_Branch>();
            var invalid = new List<BranchInvalid>();

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
                        "BranchCode",
                        "Branch"
                    };

                    var missingColumn = helpers.ValidateColumn(rows, columns);

                    if (missingColumn.Count > 0)
                    {
                        ViewBag.isInvalidColumn = true;

                        ViewBag.isInvalidMsg = "ไม่พบ column : " + String.Join(", ", missingColumn);

                        ViewBag.Valid = new List<D_Branch>();
                        ViewBag.ValidJson = "[]";
                        ViewBag.Invalid = new List<BranchInvalid>();
                        ViewBag.InvalidJson = "[]";

                        return View("Index");
                    }

                    #endregion Validate Column

                    //loop all rows
                    for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row - 1; i++)
                    {
                        // TODO: Prepare valid and invalid model for each row
                        var rowResult = new D_Branch();
                        var invalidList = new List<string>();

                        #region Validate Column

                        // TODO: Check type and require column from document condition

                        // Column BranchCode
                        var BranchCode = helpers.ValidateString("BranchCode", rows[i, 0], true);

                        if (BranchCode.IsValid) rowResult.BranchCode = BranchCode.Value;
                        if (!BranchCode.IsValid) invalidList.Add(BranchCode.InvalidMessage);

                        // Column Branch
                        var Branch = helpers.ValidateString("Branch", rows[i, 1], true);

                        if (Branch.IsValid) rowResult.Branch = Branch.Value;
                        if (!Branch.IsValid) invalidList.Add(Branch.InvalidMessage);

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
                                cfg.CreateMap<D_Branch, BranchInvalid>();
                            });

                            IMapper mapper = config.CreateMapper();
                            var dest = mapper.Map<D_Branch, BranchInvalid>(rowResult);
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
                var getDupplidateInFile = valid.GroupBy(x => x.BranchCode.Trim()).Where(x => x.Count() > 1)
                    .Select(x => x.Key).ToList();

                // TODO: Find in valid
                foreach (var item in valid.Where(x => getDupplidateInFile.Any(y => y.Equals(x.BranchCode.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_Branch, BranchInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_Branch, BranchInvalid>(item);
                    dest.InvalidList = new List<string>() { "Dupplicate column BranchCode in upload file" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                //// TODO: Check Duplicate in database

                //var existingDupplicate = _context.Branches.SqlQuery(
                //    string.Format("SELECT * FROM dbo.Branch WHERE BranchCode IN ('{0}')",
                //        string.Join("','", tmpValid.Select(x => x.BranchCode).ToArray()))).ToList();

                //// TODO: Find in exist invalid
                //foreach (var item in invalid.Where(x => existingDupplicate.Any(y => y.BranchCode.Trim().Equals(x.BranchCode.Trim()))).ToList())
                //{
                //    item.InvalidList.Add("Dupplicate column BranchCode in database");
                //}

                //// TODO: Find in valid
                //foreach (var item in valid.Where(x => existingDupplicate.Any(y => y.BranchCode.Trim().Equals(x.BranchCode.Trim()))).ToList())
                //{
                //    // Remove item from valid list
                //    valid.Remove(item);

                //    // Map valid item to invalid
                //    var config = new MapperConfiguration(cfg =>
                //    {
                //        cfg.CreateMap<Branch, BranchInvalid>();
                //    });

                //    IMapper mapper = config.CreateMapper();
                //    var dest = mapper.Map<Branch, BranchInvalid>(item);
                //    dest.InvalidList = new List<string>() { "Dupplicate column BranchCode in database" };

                //    // Add item to invalid list
                //    invalid.Add(dest);
                //}

                #endregion Check Duplicate

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
                        cfg.CreateMap<D_Branch, BranchInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_Branch, BranchInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found BranchCode" };

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
            var latestPeriodList = _context.D_Branch.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_Branch.Remove(itm);
                    _context.SaveChanges();
                }
            }

            // Get data from session by datakey
            var valid = (List<D_Branch>)Session[dataKey];

            EFBatchOperation.For(_context, _context.D_Branch).InsertAll(valid);

            //call sp to update import log
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogManual_Update(periodResult.periodId, "D_Branch", user);

            Session[dataKey] = null;

            return Json("บันทึกข้อมูลเรียบร้อย");
        }

        [HttpPost]
        public JsonResult SaveNoData()
        {
            var periodResult = GlobalFunction.GetLatestPeriod();

            var latestPeriodList = _context.D_Branch.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_Branch.Remove(itm);
                    _context.SaveChanges();
                }
            }

            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogNoData_Update(periodResult.periodId, "D_Branch", user);

            return Json("บันทึกข้อมูลเรียบร้อย");
        }
    }
}