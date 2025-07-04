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
using Authorization = SmileSCommissionDataImport.Helper.Authorization;
using SmileSCommissionDataImport.Models;
using SmileSCommissionDataImport.Utils;

namespace SmileSCommissionDataImport.Controllers
{
    [Authorization]
    public class AreaController : Controller
    {
        #region Declare Variables

        private readonly CommissionCalculateEntities _context;

        public AreaController()
        {
            // Set entity
            _context = new CommissionCalculateEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // TODO: Prepare invalid model and extend model for this import
        public class AreaInvalid : D_Area
        {
            public List<string> InvalidList { get; set; }
        }

        #endregion Declare Variables

        // GET: Area
        public ActionResult Index()
        {
            // TODO: Prepare valid and invalid model for initial table
            ViewBag.Valid = new List<D_Area>();
            ViewBag.ValidJson = "[]";
            ViewBag.Invalid = new List<AreaInvalid>();
            ViewBag.InvalidJson = "[]";
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var helpers = new ValidationHelpers();

            // TODO: Prepare valid and invalid model
            var valid = new List<D_Area>();
            var invalid = new List<AreaInvalid>();

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
                        "StudyAreaCode",
                        "StudyArea",
                        "BranchCode",
                        "Branch",
                        "ProvinceID",
                        "ProvinceName"
                    };

                    var missingColumn = helpers.ValidateColumn(rows, columns);

                    if (missingColumn.Count > 0)
                    {
                        ViewBag.isInvalidColumn = true;

                        ViewBag.isInvalidMsg = "ไม่พบ column : " + String.Join(", ", missingColumn);

                        ViewBag.Valid = new List<D_Area>();
                        ViewBag.ValidJson = "[]";
                        ViewBag.Invalid = new List<AreaInvalid>();
                        ViewBag.InvalidJson = "[]";

                        return View("Index");
                    }

                    #endregion Validate Column

                    //loop all rows
                    for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row - 1; i++)
                    {
                        // TODO: Prepare valid and invalid model for each row
                        var rowResult = new D_Area();
                        var invalidList = new List<string>();

                        #region Validate Column

                        // TODO: Check type and require column from document condition

                        // Column StudyAreaCode
                        var StudyAreaCode = helpers.ValidateString("StudyAreaCode", rows[i, 0], true);

                        if (StudyAreaCode.IsValid) rowResult.StudyAreaCode = StudyAreaCode.Value;
                        if (!StudyAreaCode.IsValid) invalidList.Add(StudyAreaCode.InvalidMessage);

                        // Column StudyArea
                        var StudyArea = helpers.ValidateString("StudyArea", rows[i, 1], true);

                        if (StudyArea.IsValid) rowResult.StudyArea = StudyArea.Value;
                        if (!StudyArea.IsValid) invalidList.Add(StudyArea.InvalidMessage);

                        // Column BranchCode
                        var BranchCode = helpers.ValidateString("BranchCode", rows[i, 2], true);

                        if (BranchCode.IsValid) rowResult.BranchCode = BranchCode.Value;
                        if (!BranchCode.IsValid) invalidList.Add(BranchCode.InvalidMessage);

                        // Column Branch
                        var Branch = helpers.ValidateString("Branch", rows[i, 3], true);

                        if (Branch.IsValid) rowResult.Branch = Branch.Value;
                        if (!Branch.IsValid) invalidList.Add(Branch.InvalidMessage);

                        // Column ProvinceID
                        var ProvinceID = helpers.ValidateString("ProvinceID", rows[i, 4], true);

                        if (ProvinceID.IsValid) rowResult.ProvinceID = ProvinceID.Value;
                        if (!ProvinceID.IsValid) invalidList.Add(ProvinceID.InvalidMessage);

                        // Column ProvinceName
                        var ProvinceName = helpers.ValidateString("ProvinceName", rows[i, 5], true);

                        if (ProvinceName.IsValid) rowResult.ProvinceName = ProvinceName.Value;
                        if (!ProvinceName.IsValid) invalidList.Add(ProvinceName.InvalidMessage);

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
                                cfg.CreateMap<D_Area, AreaInvalid>();
                            });

                            IMapper mapper = config.CreateMapper();
                            var dest = mapper.Map<D_Area, AreaInvalid>(rowResult);
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
                var getDupplidateInFile = valid.GroupBy(x => x.StudyAreaCode.Trim()).Where(x => x.Count() > 1)
                    .Select(x => x.Key).ToList();

                // TODO: Find in valid
                foreach (var item in valid.Where(x => getDupplidateInFile.Any(y => y.Equals(x.StudyAreaCode.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_Area, AreaInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_Area, AreaInvalid>(item);
                    dest.InvalidList = new List<string>() { "Dupplicate column StudyAreaCode in upload file" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                //// TODO: Check Duplicate in database

                //var existingDupplicate = _context.Areas.SqlQuery(
                //    string.Format("SELECT * FROM dbo.Area WHERE StudyAreaCode IN ('{0}')",
                //        string.Join("','", tmpValid.Select(x => x.StudyAreaCode).ToArray()))).ToList();

                //// TODO: Find in exist invalid
                //foreach (var item in invalid.Where(x => existingDupplicate.Any(y => y.StudyAreaCode.Trim().Equals(x.StudyAreaCode.Trim()))).ToList())
                //{
                //    item.InvalidList.Add("Dupplicate column StudyAreaCode in database");
                //}

                //// TODO: Find in valid
                //foreach (var item in valid.Where(x => existingDupplicate.Any(y => y.StudyAreaCode.Trim().Equals(x.StudyAreaCode.Trim()))).ToList())
                //{
                //    // Remove item from valid list
                //    valid.Remove(item);

                //    // Map valid item to invalid
                //    var config = new MapperConfiguration(cfg =>
                //    {
                //        cfg.CreateMap<Area, AreaInvalid>();
                //    });

                //    IMapper mapper = config.CreateMapper();
                //    var dest = mapper.Map<Area, AreaInvalid>(item);
                //    dest.InvalidList = new List<string>() { "Dupplicate column StudyAreaCode in database" };

                //    // Add item to invalid list
                //    invalid.Add(dest);
                //}

                #endregion Check Duplicate

                #region Validate Area

                var notExistArea = helpers.ValidateArea(tmpValid.Where(x => !string.IsNullOrEmpty(x.StudyAreaCode)).Select(x => x.StudyAreaCode.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistArea.Any(y => y.Equals(x.StudyAreaCode.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found StudyAreaCode");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistArea.Any(y => y.Equals(x.StudyAreaCode.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_Area, AreaInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_Area, AreaInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found StudyAreaCode" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Area

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
                        cfg.CreateMap<D_Area, AreaInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_Area, AreaInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found BranchCode" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Branch

                #region Validate Province

                var notExistProvince = helpers.ValidateProvince(tmpValid.Where(x => !string.IsNullOrEmpty(x.ProvinceID)).Select(x => x.ProvinceID.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistProvince.Any(y => y.Equals(x.ProvinceID.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found ProvinceID");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistProvince.Any(y => y.Equals(x.ProvinceID.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_Area, AreaInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_Area, AreaInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found ProvinceID" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Province
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
            var latestPeriodList = _context.D_Area.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_Area.Remove(itm);
                    _context.SaveChanges();
                }
            }

            // Get data from session by datakey
            var valid = (List<D_Area>)Session[dataKey];

            EFBatchOperation.For(_context, _context.D_Area).InsertAll(valid);

            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogManual_Update(periodResult.periodId, "D_Area", user);

            Session[dataKey] = null;

            return Json("บันทึกข้อมูลเรียบร้อย");
        }

        [HttpPost]
        public JsonResult SaveNoData()
        {
            var periodResult = GlobalFunction.GetLatestPeriod();

            var latestPeriodList = _context.D_Area.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_Area.Remove(itm);
                    _context.SaveChanges();
                }
            }

            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            var result = _context.usp_DatasourceImportedLogNoData_Update(periodResult.periodId, "D_Area", user).FirstOrDefault();

            return Json("บันทึกข้อมูลเรียบร้อย");
        }
    }
}