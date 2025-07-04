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
    public class NewApp_TA_DomesticController : Controller
    {
        #region Declare Variables

        private readonly CommissionCalculateEntities _context;

        public NewApp_TA_DomesticController()
        {
            // Set entity
            _context = new CommissionCalculateEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // TODO: Prepare invalid model and extend model for this import
        public class NewApp_TA_DomesticInvalid : D_NewApp_TA_Domestic
        {
            public List<string> InvalidList { get; set; }
        }

        #endregion Declare Variables

        // GET: NewApp_TA_Domestic
        public ActionResult Index()
        {
            // TODO: Prepare valid and invalid model for initial table
            ViewBag.Valid = new List<D_NewApp_TA_Domestic>();
            ViewBag.ValidJson = "[]";
            ViewBag.Invalid = new List<NewApp_TA_DomesticInvalid>();
            ViewBag.InvalidJson = "[]";
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var helpers = new ValidationHelpers();

            // TODO: Prepare valid and invalid model
            var valid = new List<D_NewApp_TA_Domestic>();
            var invalid = new List<NewApp_TA_DomesticInvalid>();

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

                        ViewBag.Valid = new List<D_NewApp_TA_Domestic>();
                        ViewBag.ValidJson = "[]";
                        ViewBag.Invalid = new List<NewApp_TA_DomesticInvalid>();
                        ViewBag.InvalidJson = "[]";

                        return View("Index");
                    }

                    #endregion Validate Column

                    //loop all rows
                    for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row - 1; i++)
                    {
                        // TODO: Prepare valid and invalid model for each row
                        var rowResult = new D_NewApp_TA_Domestic();
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
                        var District = helpers.ValidateString("District", rows[i, 4], false);

                        if (District.IsValid) rowResult.District = District.Value;
                        if (!District.IsValid) invalidList.Add(District.InvalidMessage);

                        // Column Province
                        var Province = helpers.ValidateString("Province", rows[i, 5], false);

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
                                cfg.CreateMap<D_NewApp_TA_Domestic, NewApp_TA_DomesticInvalid>();
                            });

                            IMapper mapper = config.CreateMapper();
                            var dest = mapper.Map<D_NewApp_TA_Domestic, NewApp_TA_DomesticInvalid>(rowResult);
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
                        cfg.CreateMap<D_NewApp_TA_Domestic, NewApp_TA_DomesticInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewApp_TA_Domestic, NewApp_TA_DomesticInvalid>(item);
                    dest.InvalidList = new List<string>() { "Dupplicate column Application_ID in upload file" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                //// TODO: Check Duplicate in database

                //var existingDupplicate = _context.NewApp_TA_Domestic.SqlQuery(
                //    string.Format("SELECT * FROM dbo.NewApp_TA_Domestic WHERE Application_ID IN ('{0}')",
                //        string.Join("','", tmpValid.Select(x => x.Application_ID).ToArray()))).ToList();

                //// TODO: Find in exist invalid
                //foreach (var item in invalid.Where(x => existingDupplicate.Any(y => y.Application_ID.Trim().Equals(x.Application_ID.Trim()))).ToList())
                //{
                //    item.InvalidList.Add("Dupplicate column Application_ID in database");
                //}

                //// TODO: Find in valid
                //foreach (var item in valid.Where(x => existingDupplicate.Any(y => y.Application_ID.Trim().Equals(x.Application_ID.Trim()))).ToList())
                //{
                //    // Remove item from valid list
                //    valid.Remove(item);

                //    // Map valid item to invalid
                //    var config = new MapperConfiguration(cfg =>
                //    {
                //        cfg.CreateMap<NewApp_TA_Domestic, NewApp_TA_DomesticInvalid>();
                //    });

                //    IMapper mapper = config.CreateMapper();
                //    var dest = mapper.Map<NewApp_TA_Domestic, NewApp_TA_DomesticInvalid>(item);
                //    dest.InvalidList = new List<string>() { "Dupplicate column Application_ID in database" };

                //    // Add item to invalid list
                //    invalid.Add(dest);
                //}

                #endregion Check Duplicate

                //#region Validate AppID

                //var notExistAppId = helpers.ValidateAppId(tmpValid.Where(x => !string.IsNullOrEmpty(x.Application_ID)).Select(x => x.Application_ID.Trim()).ToList());

                //// TODO: Find in exist invalid
                //foreach (var item in invalid.Where(x => notExistAppId.Any(y => y.Equals(x.Application_ID.Trim()))).ToList())
                //{
                //    item.InvalidList.Add("Not found Application_ID");
                //}

                //// TODO: Find in valid
                //foreach (var item in valid.Where(x => notExistAppId.Any(y => y.Equals(x.Application_ID.Trim()))).ToList())
                //{
                //    // Remove item from valid list
                //    valid.Remove(item);

                //    // Map valid item to invalid
                //    var config = new MapperConfiguration(cfg =>
                //    {
                //        cfg.CreateMap<NewApp_TA_Domestic, NewApp_TA_DomesticInvalid>();
                //    });

                //    IMapper mapper = config.CreateMapper();
                //    var dest = mapper.Map<NewApp_TA_Domestic, NewApp_TA_DomesticInvalid>(item);
                //    dest.InvalidList = new List<string>() { "Not found Application_ID" };

                //    // Add item to invalid list
                //    invalid.Add(dest);
                //}

                //#endregion Validate AppID

                #region Validate EmployeeSS

                var notExistEmployeeSS = helpers.ValidateEmployeeSS(tmpValid.Where(x => !string.IsNullOrEmpty(x.EmpCode)).Select(x => x.EmpCode.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistEmployeeSS.Any(y => y.Equals(x.EmpCode.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found EmpCode");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistEmployeeSS.Any(y => y.Equals(x.EmpCode.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_NewApp_TA_Domestic, NewApp_TA_DomesticInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewApp_TA_Domestic, NewApp_TA_DomesticInvalid>(item);
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
            var latestPeriodList = _context.D_NewApp_TA_Domestic.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_NewApp_TA_Domestic.Remove(itm);
                    _context.SaveChanges();
                }
            }

            // Get data from session by datakey
            var valid = (List<D_NewApp_TA_Domestic>)Session[dataKey];

            EFBatchOperation.For(_context, _context.D_NewApp_TA_Domestic).InsertAll(valid);

            //call sp to update import log
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogManual_Update(periodResult.periodId, "D_NewApp_TA_Domestic", user);

            Session[dataKey] = null;

            return Json("บันทึกข้อมูลเรียบร้อย");
        }

        [HttpPost]
        public JsonResult SaveNoData()
        {
            var periodResult = GlobalFunction.GetLatestPeriod();

            var latestPeriodList = _context.D_NewApp_TA_Domestic.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_NewApp_TA_Domestic.Remove(itm);
                    _context.SaveChanges();
                }
            }

            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogNoData_Update(periodResult.periodId, "D_NewApp_TA_Domestic", user);

            return Json("บันทึกข้อมูลเรียบร้อย");
        }
    }
}