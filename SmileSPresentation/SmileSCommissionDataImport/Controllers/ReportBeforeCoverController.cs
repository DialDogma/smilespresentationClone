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
    public class ReportBeforeCoverController : Controller
    {
        #region Declare Variables

        private readonly CommissionCalculateEntities _context;

        public ReportBeforeCoverController()
        {
            // Set entity
            _context = new CommissionCalculateEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // TODO: Prepare invalid model and extend model for this import
        public class ReportBeforeCoverInvalid : D_ReportBeforeCover
        {
            public List<string> InvalidList { get; set; }
        }

        #endregion Declare Variables

        // GET: ReportBeforeCover
        public ActionResult Index()
        {
            // TODO: Prepare valid and invalid model for initial table
            ViewBag.Valid = new List<D_ReportBeforeCover>();
            ViewBag.ValidJson = "[]";
            ViewBag.Invalid = new List<ReportBeforeCoverInvalid>();
            ViewBag.InvalidJson = "[]";
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var helpers = new ValidationHelpers();

            // TODO: Prepare valid and invalid model
            var valid = new List<D_ReportBeforeCover>();
            var invalid = new List<ReportBeforeCoverInvalid>();

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
                        "Code",
                        "App_id",
                        "CoverDate",
                        "DateOfDisease",
                        "CongenitalDisease",
                        "Sale_Id",
                        "Sale_Name",
                        "ManagerTeam_Id",
                        "ManagerTeam_Name",
                        "ZebraCarOwnerID",
                        "ZebraCarID",
                        "BranchID",
                        "BranchName"
                    };

                    var missingColumn = helpers.ValidateColumn(rows, columns);

                    if (missingColumn.Count > 0)
                    {
                        ViewBag.isInvalidColumn = true;

                        ViewBag.isInvalidMsg = "ไม่พบ column : " + String.Join(", ", missingColumn);

                        ViewBag.Valid = new List<D_ReportBeforeCover>();
                        ViewBag.ValidJson = "[]";
                        ViewBag.Invalid = new List<ReportBeforeCoverInvalid>();
                        ViewBag.InvalidJson = "[]";

                        return View("Index");
                    }

                    #endregion Validate Column

                    //loop all rows
                    for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row - 1; i++)
                    {
                        // TODO: Prepare valid and invalid model for each row
                        var rowResult = new D_ReportBeforeCover();
                        var invalidList = new List<string>();

                        #region Validate Column

                        // TODO: Check type and require column from document condition

                        // Column Code
                        var Code = helpers.ValidateInt("Code", rows[i, 0], true);

                        if (Code.IsValid) rowResult.Code = Code.Value;
                        if (!Code.IsValid) invalidList.Add(Code.InvalidMessage);

                        // Column App_id
                        var App_id = helpers.ValidateString("App_id", rows[i, 1], true);

                        if (App_id.IsValid) rowResult.App_id = App_id.Value;
                        if (!App_id.IsValid) invalidList.Add(App_id.InvalidMessage);

                        // Column CoverDate
                        var CoverDate = helpers.ValidateDate("CoverDate", rows[i, 2], true);

                        if (CoverDate.IsValid) rowResult.CoverDate = CoverDate.Value;
                        if (!CoverDate.IsValid) invalidList.Add(CoverDate.InvalidMessage);

                        // Column DateOfDisease
                        var DateOfDisease = helpers.ValidateDate("DateOfDisease", rows[i, 3], true);

                        if (DateOfDisease.IsValid) rowResult.DateOfDisease = DateOfDisease.Value;
                        if (!DateOfDisease.IsValid) invalidList.Add(DateOfDisease.InvalidMessage);

                        // Column CongenitalDisease
                        var CongenitalDisease = helpers.ValidateString("CongenitalDisease", rows[i, 4], true);

                        if (CongenitalDisease.IsValid) rowResult.CongenitalDisease = CongenitalDisease.Value;
                        if (!CongenitalDisease.IsValid) invalidList.Add(CongenitalDisease.InvalidMessage);

                        // Column Sale_Id
                        var Sale_Id = helpers.ValidateString("Sale_Id", rows[i, 5], true);

                        if (Sale_Id.IsValid) rowResult.Sale_Id = Sale_Id.Value;
                        if (!Sale_Id.IsValid) invalidList.Add(Sale_Id.InvalidMessage);

                        // Column Sale_Name
                        var Sale_Name = helpers.ValidateString("Sale_Name", rows[i, 6], true);

                        if (Sale_Name.IsValid) rowResult.Sale_Name = Sale_Name.Value;
                        if (!Sale_Name.IsValid) invalidList.Add(Sale_Name.InvalidMessage);

                        // Column ManagerTeam_Id
                        var ManagerTeam_Id = helpers.ValidateString("ManagerTeam_Id", rows[i, 7], false);

                        if (ManagerTeam_Id.IsValid) rowResult.ManagerTeam_Id = ManagerTeam_Id.Value;
                        if (!ManagerTeam_Id.IsValid) invalidList.Add(ManagerTeam_Id.InvalidMessage);

                        // Column ManagerTeam_Name
                        var ManagerTeam_Name = helpers.ValidateString("ManagerTeam_Name", rows[i, 8], false);

                        if (ManagerTeam_Name.IsValid) rowResult.ManagerTeam_Name = ManagerTeam_Name.Value;
                        if (!ManagerTeam_Name.IsValid) invalidList.Add(ManagerTeam_Name.InvalidMessage);

                        // Column ZebraCarOwnerID
                        var ZebraCarOwnerID = helpers.ValidateString("ZebraCarOwnerID", rows[i, 9], true);

                        if (ZebraCarOwnerID.IsValid) rowResult.ZebraCarOwnerID = ZebraCarOwnerID.Value;
                        if (!ZebraCarOwnerID.IsValid) invalidList.Add(ZebraCarOwnerID.InvalidMessage);

                        // Column ZebraCarID
                        var ZebraCarID = helpers.ValidateString("ZebraCarID", rows[i, 10], true);

                        if (ZebraCarID.IsValid) rowResult.ZebraCarID = ZebraCarID.Value;
                        if (!ZebraCarID.IsValid) invalidList.Add(ZebraCarID.InvalidMessage);

                        // Column BranchID
                        var BranchID = helpers.ValidateString("BranchID", rows[i, 11], true);

                        if (BranchID.IsValid) rowResult.BranchID = BranchID.Value;
                        if (!BranchID.IsValid) invalidList.Add(BranchID.InvalidMessage);

                        // Column BranchName
                        var BranchName = helpers.ValidateString("BranchName", rows[i, 12], true);

                        if (BranchName.IsValid) rowResult.BranchName = BranchName.Value;
                        if (!BranchName.IsValid) invalidList.Add(BranchName.InvalidMessage);

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
                                cfg.CreateMap<D_ReportBeforeCover, ReportBeforeCoverInvalid>();
                            });

                            IMapper mapper = config.CreateMapper();
                            var dest = mapper.Map<D_ReportBeforeCover, ReportBeforeCoverInvalid>(rowResult);
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
                var getDupplidateInFile = valid.GroupBy(x => x.App_id.Trim()).Where(x => x.Count() > 1)
                    .Select(x => x.Key).ToList();

                // TODO: Find in valid
                foreach (var item in valid.Where(x => getDupplidateInFile.Any(y => y.Equals(x.App_id.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_ReportBeforeCover, ReportBeforeCoverInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_ReportBeforeCover, ReportBeforeCoverInvalid>(item);
                    dest.InvalidList = new List<string>() { "Dupplicate column App_id in upload file" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                //// TODO: Check Duplicate in database

                //var existingDupplicate = _context.ReportBeforeCovers.SqlQuery(
                //    string.Format("SELECT * FROM dbo.ReportBeforeCover WHERE App_id IN ('{0}')",
                //        string.Join("','", tmpValid.Select(x => x.App_id).ToArray()))).ToList();

                //// TODO: Find in exist invalid
                //foreach (var item in invalid.Where(x => existingDupplicate.Any(y => y.App_id.Trim().Equals(x.App_id.Trim()))).ToList())
                //{
                //    item.InvalidList.Add("Dupplicate column App_id in database");
                //}

                //// TODO: Find in valid
                //foreach (var item in valid.Where(x => existingDupplicate.Any(y => y.App_id.Trim().Equals(x.App_id.Trim()))).ToList())
                //{
                //    // Remove item from valid list
                //    valid.Remove(item);

                //    // Map valid item to invalid
                //    var config = new MapperConfiguration(cfg =>
                //    {
                //        cfg.CreateMap<ReportBeforeCover, ReportBeforeCoverInvalid>();
                //    });

                //    IMapper mapper = config.CreateMapper();
                //    var dest = mapper.Map<ReportBeforeCover, ReportBeforeCoverInvalid>(item);
                //    dest.InvalidList = new List<string>() { "Dupplicate column App_id in database" };

                //    // Add item to invalid list
                //    invalid.Add(dest);
                //}

                #endregion Check Duplicate

                #region Validate AppID

                var notExistAppId = helpers.ValidateAppId(tmpValid.Where(x => !string.IsNullOrEmpty(x.App_id)).Select(x => x.App_id.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistAppId.Any(y => y.Equals(x.App_id.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found App_id");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistAppId.Any(y => y.Equals(x.App_id.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_ReportBeforeCover, ReportBeforeCoverInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_ReportBeforeCover, ReportBeforeCoverInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found App_id" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate AppID

                #region Validate EmployeeSS

                var notExistEmployeeSS = helpers.ValidateEmployeeSS(tmpValid.Where(x => !string.IsNullOrEmpty(x.Sale_Id)).Select(x => x.Sale_Id.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistEmployeeSS.Any(y => y.Equals(x.Sale_Id.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found Sale_Id");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistEmployeeSS.Any(y => y.Equals(x.Sale_Id.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_ReportBeforeCover, ReportBeforeCoverInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_ReportBeforeCover, ReportBeforeCoverInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found Sale_Id" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate EmployeeSS

                #region Validate EmployeeSS

                var notExistEmployeeSS2 = helpers.ValidateEmployeeSS(tmpValid.Where(x => !string.IsNullOrEmpty(x.ManagerTeam_Id)).Select(x => x.ManagerTeam_Id.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistEmployeeSS2.Any(y => y.Equals(x.ManagerTeam_Id.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found ManagerTeam_Id");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistEmployeeSS2.Any(y => y.Equals(x.ManagerTeam_Id.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_ReportBeforeCover, ReportBeforeCoverInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_ReportBeforeCover, ReportBeforeCoverInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found ManagerTeam_Id" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate EmployeeSS

                //#region Validate Team

                //var notExistTeam = helpers.ValidateTeam(tmpValid.Where(x => !string.IsNullOrEmpty(x.ManagerTeam_Id)).Select(x => x.ManagerTeam_Id.Trim()).ToList());

                //// TODO: Find in exist invalid
                //foreach (var item in invalid.Where(x => notExistTeam.Any(y => y.Equals(x.ManagerTeam_Id.Trim()))).ToList())
                //{
                //    item.InvalidList.Add("Not found ManagerTeam_Id");
                //}

                //// TODO: Find in valid
                //foreach (var item in valid.Where(x => notExistTeam.Any(y => y.Equals(x.ManagerTeam_Id.Trim()))).ToList())
                //{
                //    // Remove item from valid list
                //    valid.Remove(item);

                //    // Map valid item to invalid
                //    var config = new MapperConfiguration(cfg =>
                //    {
                //        cfg.CreateMap<ReportBeforeCover, ReportBeforeCoverInvalid>();
                //    });

                //    IMapper mapper = config.CreateMapper();
                //    var dest = mapper.Map<ReportBeforeCover, ReportBeforeCoverInvalid>(item);
                //    dest.InvalidList = new List<string>() { "Not found ManagerTeam_Id" };

                //    // Add item to invalid list
                //    invalid.Add(dest);
                //}

                //#endregion Validate Team

                #region Validate Zebra

                var notExistZebraId = helpers.ValidateZebra(tmpValid.Where(x => !string.IsNullOrEmpty(x.ZebraCarID)).Select(x => x.ZebraCarID.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistZebraId.Any(y => y.Equals(x.ZebraCarID.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found ZebraCarID");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistZebraId.Any(y => y.Equals(x.ZebraCarID.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_ReportBeforeCover, ReportBeforeCoverInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_ReportBeforeCover, ReportBeforeCoverInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found ZebraCarID" };

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
            var latestPeriodList = _context.D_ReportBeforeCover.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_ReportBeforeCover.Remove(itm);
                    _context.SaveChanges();
                }
            }
            // Get data from session by datakey
            var valid = (List<D_ReportBeforeCover>)Session[dataKey];

            EFBatchOperation.For(_context, _context.D_ReportBeforeCover).InsertAll(valid);

            //call sp to update import log
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogManual_Update(periodResult.periodId, "D_ReportBeforeCover", user);

            Session[dataKey] = null;

            return Json("บันทึกข้อมูลเรียบร้อย");
        }

        [HttpPost]
        public JsonResult SaveNoData()
        {
            var periodResult = GlobalFunction.GetLatestPeriod();

            var latestPeriodList = _context.D_ReportBeforeCover.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_ReportBeforeCover.Remove(itm);
                    _context.SaveChanges();
                }
            }

            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogNoData_Update(periodResult.periodId, "D_ReportBeforeCover", user);

            return Json("บันทึกข้อมูลเรียบร้อย");
        }
    }
}