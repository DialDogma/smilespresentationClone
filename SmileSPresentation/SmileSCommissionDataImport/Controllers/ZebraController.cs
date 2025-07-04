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
    public class ZebraController : Controller
    {
        #region Declare Variables

        private readonly CommissionCalculateEntities _context;

        public ZebraController()
        {
            // Set entity
            _context = new CommissionCalculateEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // TODO: Prepare invalid model and extend model for this import
        public class ZebraInvalid : D_Zebra
        {
            public List<string> InvalidList { get; set; }
        }

        #endregion Declare Variables

        // GET: Zebra
        public ActionResult Index()
        {
            // TODO: Prepare valid and invalid model for initial table
            ViewBag.Valid = new List<D_Zebra>();
            ViewBag.ValidJson = "[]";
            ViewBag.Invalid = new List<ZebraInvalid>();
            ViewBag.InvalidJson = "[]";
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var helpers = new ValidationHelpers();

            // TODO: Prepare valid and invalid model
            var valid = new List<D_Zebra>();
            var invalid = new List<ZebraInvalid>();

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
                        "ZebraCode",
                        "CarNumber"
                    };

                    var missingColumn = helpers.ValidateColumn(rows, columns);

                    if (missingColumn.Count > 0)
                    {
                        ViewBag.isInvalidColumn = true;

                        ViewBag.isInvalidMsg = "ไม่พบ column : " + String.Join(", ", missingColumn);

                        ViewBag.Valid = new List<D_Zebra>();
                        ViewBag.ValidJson = "[]";
                        ViewBag.Invalid = new List<ZebraInvalid>();
                        ViewBag.InvalidJson = "[]";

                        return View("Index");
                    }

                    #endregion Validate Column

                    //loop all rows
                    for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row - 1; i++)
                    {
                        // TODO: Prepare valid and invalid model for each row
                        var rowResult = new D_Zebra();
                        var invalidList = new List<string>();

                        #region Validate Column

                        // TODO: Check type and require column from document condition

                        // Column ZebraCode
                        var ZebraCode = helpers.ValidateString("ZebraCode", rows[i, 0], true);

                        if (ZebraCode.IsValid) rowResult.ZebraCode = ZebraCode.Value;
                        if (!ZebraCode.IsValid) invalidList.Add(ZebraCode.InvalidMessage);

                        // Column CarNumber
                        var CarNumber = helpers.ValidateString("CarNumber", rows[i, 1], true);

                        if (CarNumber.IsValid) rowResult.CarNumber = CarNumber.Value;
                        if (!CarNumber.IsValid) invalidList.Add(CarNumber.InvalidMessage);

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
                                cfg.CreateMap<D_Zebra, ZebraInvalid>();
                            });

                            IMapper mapper = config.CreateMapper();
                            var dest = mapper.Map<D_Zebra, ZebraInvalid>(rowResult);
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
                var getDupplidateInFile = valid.GroupBy(x => x.ZebraCode.Trim()).Where(x => x.Count() > 1)
                    .Select(x => x.Key).ToList();

                // TODO: Find in valid
                foreach (var item in valid.Where(x => getDupplidateInFile.Any(y => y.Equals(x.ZebraCode.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_Zebra, ZebraInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_Zebra, ZebraInvalid>(item);
                    dest.InvalidList = new List<string>() { "Dupplicate column ZebraCode in upload file" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                //// TODO: Check Duplicate in database

                //var existingDupplicate = _context.Zebras.SqlQuery(
                //    string.Format("SELECT * FROM dbo.Zebra WHERE ZebraCode IN ('{0}')",
                //        string.Join("','", tmpValid.Select(x => x.ZebraCode).ToArray()))).ToList();

                //// TODO: Find in exist invalid
                //foreach (var item in invalid.Where(x => existingDupplicate.Any(y => y.ZebraCode.Trim().Equals(x.ZebraCode.Trim()))).ToList())
                //{
                //    item.InvalidList.Add("Dupplicate column ZebraCode in database");
                //}

                //// TODO: Find in valid
                //foreach (var item in valid.Where(x => existingDupplicate.Any(y => y.ZebraCode.Trim().Equals(x.ZebraCode.Trim()))).ToList())
                //{
                //    // Remove item from valid list
                //    valid.Remove(item);

                //    // Map valid item to invalid
                //    var config = new MapperConfiguration(cfg =>
                //    {
                //        cfg.CreateMap<Zebra, ZebraInvalid>();
                //    });

                //    IMapper mapper = config.CreateMapper();
                //    var dest = mapper.Map<Zebra, ZebraInvalid>(item);
                //    dest.InvalidList = new List<string>() { "Dupplicate column ZebraCode in database" };

                //    // Add item to invalid list
                //    invalid.Add(dest);
                //}

                #endregion Check Duplicate

                #region Validate Zebra

                var notExistZebraId = helpers.ValidateZebra(tmpValid.Where(x => !string.IsNullOrEmpty(x.ZebraCode)).Select(x => x.ZebraCode.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistZebraId.Any(y => y.Equals(x.ZebraCode.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found ZebraCode");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistZebraId.Any(y => y.Equals(x.ZebraCode.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_Zebra, ZebraInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_Zebra, ZebraInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found ZebraCode" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Zebra
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
            var latestPeriodList = _context.D_Zebra.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_Zebra.Remove(itm);
                    _context.SaveChanges();
                }
            }

            // Get data from session by datakey
            var valid = (List<D_Zebra>)Session[dataKey];

            EFBatchOperation.For(_context, _context.D_Zebra).InsertAll(valid);

            //call sp to update import log
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogManual_Update(periodResult.periodId, "D_Zebra", user);

            Session[dataKey] = null;

            return Json("บันทึกข้อมูลเรียบร้อย");
        }

        [HttpPost]
        public JsonResult SaveNoData()
        {
            var periodResult = GlobalFunction.GetLatestPeriod();

            var latestPeriodList = _context.D_Zebra.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_Zebra.Remove(itm);
                    _context.SaveChanges();
                }
            }

            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogNoData_Update(periodResult.periodId, "D_Zebra", user);

            return Json("บันทึกข้อมูลเรียบร้อย");
        }
    }
}