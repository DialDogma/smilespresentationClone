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
    public class NewApp_PAGroupController : Controller
    {
        #region Declare Variables

        private readonly CommissionCalculateEntities _context;

        public NewApp_PAGroupController()
        {
            // Set entity
            _context = new CommissionCalculateEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // Prepare invalid model and extend model for this import
        public class NewApp_PAGroupInvalid : D_NewApp_PAGroup
        {
            public List<string> InvalidList { get; set; }
        }

        #endregion Declare Variables

        // GET: NewApp_PAGroup
        public ActionResult Index()
        {
            // Prepare valid and invalid model for initial table
            ViewBag.Valid = new List<D_NewApp_PAGroup>();
            ViewBag.ValidJson = "[]";
            ViewBag.Invalid = new List<D_NewApp_PAGroup>();
            ViewBag.InvalidJson = "[]";

            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var helpers = new ValidationHelpers();

            // TODO: Prepare valid and invalid model
            var valid = new List<D_NewApp_PAGroup>();
            var invalid = new List<NewApp_PAGroupInvalid>();

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
                       "Application_ID",
                       "CoverDate",
                       "CustomerName",
                       "Branch",
                       "District",
                       "Province",
                       "Premium",
                       "EmpCode"
                    };

                    var missingColumn = helpers.ValidateColumn(rows, columns);

                    if (missingColumn.Count > 0)
                    {
                        ViewBag.isInvalidColumn = true;

                        ViewBag.isInvalidMsg = "ไม่พบ column : " + String.Join(", ", missingColumn);

                        ViewBag.Valid = new List<D_NewApp_PAGroup>();
                        ViewBag.ValidJson = "[]";
                        ViewBag.Invalid = new List<NewApp_PAGroupInvalid>();
                        ViewBag.InvalidJson = "[]";

                        return View("Index");
                    }

                    #endregion Validate Column

                    //loop all rows
                    for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row - 1; i++)
                    {
                        // TODO: Prepare valid and invalid model for each row
                        var rowResult = new D_NewApp_PAGroup();
                        var invalidList = new List<string>();

                        #region Validate Column

                        // TODO: Check type and require column from document condition

                        // Column Application_ID
                        var Application_ID = helpers.ValidateString("Application_ID", rows[i, 0], true);

                        if (Application_ID.IsValid) rowResult.Application_ID = Application_ID.Value;
                        if (!Application_ID.IsValid) invalidList.Add(Application_ID.InvalidMessage);

                        // Column CoverDate
                        var CoverDate = helpers.ValidateDate("CoverDate", rows[i, 1], true);

                        if (CoverDate.IsValid) rowResult.CoverDate = CoverDate.Value;
                        if (!CoverDate.IsValid) invalidList.Add(CoverDate.InvalidMessage);

                        // Column CustomerName
                        var CustomerName = helpers.ValidateString("CustomerName", rows[i, 2], true);

                        if (CustomerName.IsValid) rowResult.CustomerName = CustomerName.Value;
                        if (!CustomerName.IsValid) invalidList.Add(CustomerName.InvalidMessage);

                        // Column Branch
                        var Branch = helpers.ValidateString("Branch", rows[i, 3], true);

                        if (Branch.IsValid) rowResult.Branch = Branch.Value;
                        if (!Branch.IsValid) invalidList.Add(Branch.InvalidMessage);

                        // Column District
                        var District = helpers.ValidateString("District", rows[i, 4], true);

                        if (District.IsValid) rowResult.District = District.Value;
                        if (!District.IsValid) invalidList.Add(District.InvalidMessage);

                        // Column Province
                        var Province = helpers.ValidateString("Province", rows[i, 5], true);

                        if (Province.IsValid) rowResult.Province = Province.Value;
                        if (!Province.IsValid) invalidList.Add(Province.InvalidMessage);

                        // Column Premium
                        var Premium = helpers.ValidateFloat("Premium", rows[i, 6], true);

                        if (Premium.IsValid) rowResult.Premium = Premium.Value;
                        if (!Premium.IsValid) invalidList.Add(Premium.InvalidMessage);

                        // Column EmpCode
                        var EmpCode = helpers.ValidateString("EmpCode", rows[i, 7], true);

                        if (EmpCode.IsValid) rowResult.EmpCode = EmpCode.Value;
                        if (!EmpCode.IsValid) invalidList.Add(EmpCode.InvalidMessage);

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
                                cfg.CreateMap<D_NewApp_PAGroup, NewApp_PAGroupInvalid>();
                            });

                            IMapper mapper = config.CreateMapper();
                            var dest = mapper.Map<D_NewApp_PAGroup, NewApp_PAGroupInvalid>(rowResult);
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
                var getDupplidateInFile = valid.GroupBy(x => x.Application_ID.Trim()).Where(x => x.Count() > 1)
                    .Select(x => x.Key).ToList();

                // TODO: Find in valid
                foreach (var item in valid.Where(x => getDupplidateInFile.Any(y => y.Equals(x.Application_ID.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_NewApp_PAGroup, NewApp_PAGroupInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewApp_PAGroup, NewApp_PAGroupInvalid>(item);
                    dest.InvalidList = new List<string>() { "Dupplicate column MainKey in upload file" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Check Duplicate

                #region Validate EmployeeSS

                var notExistEmployeeSS2 = helpers.ValidateEmployeeSS(tmpValid.Where(x => !string.IsNullOrEmpty(x.EmpCode)).Select(x => x.EmpCode.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistEmployeeSS2.Any(y => y.Equals(x.EmpCode))).ToList())
                {
                    item.InvalidList.Add("Not found MSNID");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistEmployeeSS2.Any(y => y.Equals(x.EmpCode))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_NewApp_PAGroup, NewApp_PAGroupInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewApp_PAGroup, NewApp_PAGroupInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found EmpCode" };

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
            var latestPeriodList = _context.D_NewApp_PAGroup.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_NewApp_PAGroup.Remove(itm);
                    _context.SaveChanges();
                }
            }

            // Get data from session by datakey
            var valid = (List<D_NewApp_PAGroup>)Session[dataKey];

            EFBatchOperation.For(_context, _context.D_NewApp_PAGroup).InsertAll(valid);

            //call sp to update import log
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogManual_Update(periodResult.periodId, "D_NewApp_PAGroup", user);

            Session[dataKey] = null;

            return Json("บันทึกข้อมูลเรียบร้อย");
        }

        [HttpPost]
        public JsonResult SaveNoData()
        {
            var periodResult = GlobalFunction.GetLatestPeriod();

            var latestPeriodList = _context.D_NewApp_PAGroup.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_NewApp_PAGroup.Remove(itm);
                    _context.SaveChanges();
                }
            }

            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogNoData_Update(periodResult.periodId, "D_NewApp_PAGroup", user);

            return Json("บันทึกข้อมูลเรียบร้อย");
        }
    }
}