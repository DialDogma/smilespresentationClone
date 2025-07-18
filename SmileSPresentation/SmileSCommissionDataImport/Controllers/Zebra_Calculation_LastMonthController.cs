﻿using System;
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
    public class Zebra_Calculation_LastMonthController : Controller
    {
        #region Declare Variables

        private readonly CommissionCalculateEntities _context;

        public Zebra_Calculation_LastMonthController()
        {
            // Set entity
            _context = new CommissionCalculateEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // TODO: Prepare invalid model and extend model for this import
        public class Zebra_Calculation_LastMonthInvalid : D_Zebra_Calculation_LastMonth
        {
            public List<string> InvalidList { get; set; }
        }

        #endregion Declare Variables

        // GET: Zebra_Calculation_LastMonth
        public ActionResult Index()
        {
            // TODO: Prepare valid and invalid model for initial table
            ViewBag.Valid = new List<D_Zebra_Calculation_LastMonth>();
            ViewBag.ValidJson = "[]";
            ViewBag.Invalid = new List<Zebra_Calculation_LastMonthInvalid>();
            ViewBag.InvalidJson = "[]";

            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var helpers = new ValidationHelpers();

            // TODO: Prepare valid and invalid model
            var valid = new List<D_Zebra_Calculation_LastMonth>();
            var invalid = new List<Zebra_Calculation_LastMonthInvalid>();

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
                        "ZebraNumber",
                        "Lapse_Adjust_PH_LastMonth",
                        "Lapse_Adjust_PH_InProcess_LastMonth",
                        "Best_Loss_Claim_Zebra"
                    };

                    var missingColumn = helpers.ValidateColumn(rows, columns);

                    if (missingColumn.Count > 0)
                    {
                        ViewBag.isInvalidColumn = true;

                        ViewBag.isInvalidMsg = "ไม่พบ column : " + String.Join(", ", missingColumn);

                        ViewBag.Valid = new List<D_Zebra_Calculation_LastMonth>();
                        ViewBag.ValidJson = "[]";
                        ViewBag.Invalid = new List<Zebra_Calculation_LastMonthInvalid>();
                        ViewBag.InvalidJson = "[]";

                        return View("Index");
                    }

                    #endregion Validate Column

                    //loop all rows
                    for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row - 1; i++)
                    {
                        // TODO: Prepare valid and invalid model for each row
                        var rowResult = new D_Zebra_Calculation_LastMonth();
                        var invalidList = new List<string>();

                        #region Validate Column

                        // TODO: Check type and require column from document condition

                        //Column Mainkey validate
                        var MainKey = helpers.ValidateString("MainKey", rows[i, 0], true);
                        if (MainKey.IsValid) rowResult.MainKey = MainKey.Value;
                        if (!MainKey.IsValid) invalidList.Add(MainKey.InvalidMessage);

                        //Column ZebraNumber validate
                        var ZebraNumber = helpers.ValidateString("ZebraNumber", rows[i, 1], true);
                        if (ZebraNumber.IsValid) rowResult.ZebraNumber = ZebraNumber.Value;
                        if (!ZebraNumber.IsValid) invalidList.Add(ZebraNumber.InvalidMessage);

                        //Column Lapse_Adjust_PH_LastMonth validate
                        var Lapse_Adjust_PH_LastMonth = helpers.ValidateFloat("Lapse_Adjust_PH_LastMonth", rows[i, 2], true);
                        if (Lapse_Adjust_PH_LastMonth.IsValid) rowResult.Lapse_Adjust_PH_LastMonth = Lapse_Adjust_PH_LastMonth.Value;
                        if (!Lapse_Adjust_PH_LastMonth.IsValid) invalidList.Add(Lapse_Adjust_PH_LastMonth.InvalidMessage);

                        //Column Lapse_Adjust_PH_InProcess_LastMonth validate
                        var Lapse_Adjust_PH_InProcess_LastMonth = helpers.ValidateFloat("Lapse_Adjust_PH_LastMonth", rows[i, 3], true);
                        if (Lapse_Adjust_PH_InProcess_LastMonth.IsValid) rowResult.Lapse_Adjust_PH_InProcess_LastMonth = Lapse_Adjust_PH_InProcess_LastMonth.Value;
                        if (!Lapse_Adjust_PH_InProcess_LastMonth.IsValid) invalidList.Add(Lapse_Adjust_PH_InProcess_LastMonth.InvalidMessage);

                        //Column ZebraNumber validate
                        var Best_Loss_Claim_Personal = helpers.ValidateFloat("Best_Lost_Claim_Zebra", rows[i, 4], true);
                        if (Best_Loss_Claim_Personal.IsValid) rowResult.Best_Loss_Claim_Zebra = Best_Loss_Claim_Personal.Value;
                        if (!Best_Loss_Claim_Personal.IsValid) invalidList.Add(Best_Loss_Claim_Personal.InvalidMessage);

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
                                cfg.CreateMap<D_Zebra_Calculation_LastMonth, Zebra_Calculation_LastMonthInvalid>();
                            });

                            IMapper mapper = config.CreateMapper();
                            var dest = mapper.Map<Zebra_Calculation_LastMonthInvalid>(rowResult);
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

                #region Validate EmployeeSS

                var notExistEmployeeSS = helpers.ValidateEmployeeSS(tmpValid.Where(x => !string.IsNullOrEmpty(x.MainKey)).Select(x => x.MainKey.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistEmployeeSS.Any(y => y.Equals(x.MainKey.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found SS_ID");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistEmployeeSS.Any(y => y.Equals(x.MainKey.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_Zebra_Calculation_LastMonth, Zebra_Calculation_LastMonthInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_Zebra_Calculation_LastMonth, Zebra_Calculation_LastMonthInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found SS_ID" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate EmployeeSS

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
                        cfg.CreateMap<D_Zebra_Calculation_LastMonth, Zebra_Calculation_LastMonthInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_Zebra_Calculation_LastMonth, Zebra_Calculation_LastMonthInvalid>(item);
                    dest.InvalidList = new List<string>() { "Dupplicate column MainKey in upload file" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Check Duplicate

                #region Validate Zebra

                var notExistZebraId = helpers.ValidateZebra(tmpValid.Where(x => !string.IsNullOrEmpty(x.ZebraNumber)).Select(x => x.ZebraNumber.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistZebraId.Any(y => y.Equals(x.ZebraNumber.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found ZebraCode");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistZebraId.Any(y => y.Equals(x.ZebraNumber.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_Zebra_Calculation_LastMonth, Zebra_Calculation_LastMonthInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_Zebra_Calculation_LastMonth, Zebra_Calculation_LastMonthInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found ZebraCode" };

                    // Add item to invalid list
                    invalid.Add((Zebra_Calculation_LastMonthInvalid)dest);
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
            var latestPeriodList = _context.D_Zebra_Calculation_LastMonth.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_Zebra_Calculation_LastMonth.Remove(itm);
                    _context.SaveChanges();
                }
            }

            // Get data from session by datakey
            var valid = (List<D_Zebra_Calculation_LastMonth>)Session[dataKey];

            EFBatchOperation.For(_context, _context.D_Zebra_Calculation_LastMonth).InsertAll(valid);

            //call sp to update import log
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogManual_Update(periodResult.periodId, "D_Zebra_Calculation_LastMonth", user);

            Session[dataKey] = null;

            return Json("บันทึกข้อมูลเรียบร้อย");
        }

        [HttpPost]
        public JsonResult SaveNoData()
        {
            var periodResult = GlobalFunction.GetLatestPeriod();

            var latestPeriodList = _context.D_Zebra_Calculation_LastMonth.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_Zebra_Calculation_LastMonth.Remove(itm);
                    _context.SaveChanges();
                }
            }

            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogNoData_Update(periodResult.periodId, "D_Zebra_Calculation_LastMonth", user);

            return Json("บันทึกข้อมูลเรียบร้อย");
        }
    }
}