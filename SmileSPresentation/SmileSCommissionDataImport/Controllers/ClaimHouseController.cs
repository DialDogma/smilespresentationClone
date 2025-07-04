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
    public class ClaimHouseController : Controller
    {
        #region Declare Variables

        private readonly CommissionCalculateEntities _context;

        public ClaimHouseController()
        {
            // Set entity
            _context = new CommissionCalculateEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // TODO: Prepare invalid model and extend model for this import
        public class ClaimHouseInvalid : D_ClaimHouse
        {
            public List<string> InvalidList { get; set; }
        }

        #endregion Declare Variables

        // GET: ClaimHouse
        public ActionResult Index()
        {
            // TODO: Prepare valid and invalid model for initial table
            ViewBag.Valid = new List<D_ClaimHouse>();
            ViewBag.ValidJson = "[]";
            ViewBag.Invalid = new List<ClaimHouseInvalid>();
            ViewBag.InvalidJson = "[]";

            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var helpers = new ValidationHelpers();

            // TODO: Prepare valid and invalid model
            var valid = new List<D_ClaimHouse>();
            var invalid = new List<ClaimHouseInvalid>();

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
                        "AppID",
                        "CustomerName",
                        "PaidCode",
                        "BranchName",
                        "EmpCodeApprove",
                        "EmpNameApprove",
                        "ClaimRequestTotal",
                        "ClaimApproveTotal",
                        "PayClaimDate"
                    };

                    var missingColumn = helpers.ValidateColumn(rows, columns);

                    if (missingColumn.Count > 0)
                    {
                        ViewBag.isInvalidColumn = true;

                        ViewBag.isInvalidMsg = "ไม่พบ column : " + String.Join(", ", missingColumn);

                        ViewBag.Valid = new List<D_ClaimHouse>();
                        ViewBag.ValidJson = "[]";
                        ViewBag.Invalid = new List<ClaimHouseInvalid>();
                        ViewBag.InvalidJson = "[]";

                        return View("Index");
                    }

                    #endregion Validate Column

                    //loop all rows
                    for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row - 1; i++)
                    {
                        // TODO: Prepare valid and invalid model for each row
                        var rowResult = new D_ClaimHouse();
                        var invalidList = new List<string>();

                        #region Validate Column

                        // TODO: Check type and require column from document condition

                        // Column AppID
                        var AppID = helpers.ValidateString("AppID", rows[i, 0], true);

                        if (AppID.IsValid) rowResult.AppID = AppID.Value;
                        if (!AppID.IsValid) invalidList.Add(AppID.InvalidMessage);

                        // Column CustomerName
                        var CustomerName = helpers.ValidateString("CustomerName", rows[i, 1], true);

                        if (CustomerName.IsValid) rowResult.CustomerName = CustomerName.Value;
                        if (!CustomerName.IsValid) invalidList.Add(CustomerName.InvalidMessage);

                        // Column PaidCode
                        var PaidCode = helpers.ValidateString("PaidCode", rows[i, 2], true);

                        if (PaidCode.IsValid) rowResult.PaidCode = PaidCode.Value;
                        if (!PaidCode.IsValid) invalidList.Add(PaidCode.InvalidMessage);

                        // Column BranchName
                        var BranchName = helpers.ValidateString("BranchName", rows[i, 3], true);

                        if (BranchName.IsValid) rowResult.BranchName = BranchName.Value;
                        if (!BranchName.IsValid) invalidList.Add(BranchName.InvalidMessage);

                        // Column EmpCodeApprove
                        var EmpCodeApprove = helpers.ValidateString("EmpCodeApprove", rows[i, 4], true);

                        if (EmpCodeApprove.IsValid) rowResult.EmpCodeApprove = EmpCodeApprove.Value;
                        if (!EmpCodeApprove.IsValid) invalidList.Add(EmpCodeApprove.InvalidMessage);

                        // Column EmpNameApprove
                        var EmpNameApprove = helpers.ValidateString("EmpNameApprove", rows[i, 5], true);

                        if (EmpNameApprove.IsValid) rowResult.EmpNameApprove = EmpNameApprove.Value;
                        if (!EmpNameApprove.IsValid) invalidList.Add(EmpNameApprove.InvalidMessage);

                        // Column ClaimRequestTotal
                        var ClaimRequestTotal = helpers.ValidateInt("ClaimRequestTotal", rows[i, 6], true);

                        if (ClaimRequestTotal.IsValid) rowResult.ClaimRequestTotal = ClaimRequestTotal.Value;
                        if (!ClaimRequestTotal.IsValid) invalidList.Add(ClaimRequestTotal.InvalidMessage);

                        // Column ClaimApproveTotal
                        var ClaimApproveTotal = helpers.ValidateInt("ClaimApproveTotal", rows[i, 7], true);

                        if (ClaimApproveTotal.IsValid) rowResult.ClaimApproveTotal = ClaimApproveTotal.Value;
                        if (!ClaimApproveTotal.IsValid) invalidList.Add(ClaimApproveTotal.InvalidMessage);

                        // Column PayClaimDate
                        var PayClaimDate = helpers.ValidateDate("PayClaimDate", rows[i, 8], true);

                        if (PayClaimDate.IsValid) rowResult.PayClaimDate = PayClaimDate.Value;
                        if (!PayClaimDate.IsValid) invalidList.Add(PayClaimDate.InvalidMessage);

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
                                cfg.CreateMap<D_ClaimHouse, ClaimHouseInvalid>();
                            });

                            IMapper mapper = config.CreateMapper();
                            var dest = mapper.Map<D_ClaimHouse, ClaimHouseInvalid>(rowResult);
                            dest.InvalidList = invalidList;

                            // Add item to invalid list
                            invalid.Add(dest);
                        }
                    }
                }
            }

            // TODO: Validate Zone

            if (invalid.Count == 0)
            {
                var tmpValid = valid.ToList();

                #region Check Duplicate

                // TODO: Check Duplicate in File
                var getDupplidateInFile = valid.GroupBy(x => x.PaidCode.Trim()).Where(x => x.Count() > 1)
                    .Select(x => x.Key).ToList();

                // TODO: Find in valid
                foreach (var item in valid.Where(x => getDupplidateInFile.Any(y => y.Equals(x.PaidCode.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_ClaimHouse, ClaimHouseInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_ClaimHouse, ClaimHouseInvalid>(item);
                    dest.InvalidList = new List<string>() { "Dupplicate column PaidCode in upload file" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                //// TODO: Check Duplicate in database

                //var existingDupplicate = _context.ClaimHouses.SqlQuery(
                //    string.Format("SELECT * FROM dbo.ClaimHouse WHERE PaidCode IN ('{0}')",
                //        string.Join("','", tmpValid.Select(x => x.PaidCode).ToArray()))).ToList();

                //// TODO: Find in exist invalid
                //foreach (var item in invalid.Where(x => existingDupplicate.Any(y => y.PaidCode.Trim().Equals(x.PaidCode.Trim()))).ToList())
                //{
                //    item.InvalidList.Add("Dupplicate column PaidCode in database");
                //}

                //// TODO: Find in valid
                //foreach (var item in valid.Where(x => existingDupplicate.Any(y => y.PaidCode.Trim().Equals(x.PaidCode.Trim()))).ToList())
                //{
                //    // Remove item from valid list
                //    valid.Remove(item);

                //    // Map valid item to invalid
                //    var config = new MapperConfiguration(cfg =>
                //    {
                //        cfg.CreateMap<ClaimHouse, ClaimHouseInvalid>();
                //    });

                //    IMapper mapper = config.CreateMapper();
                //    var dest = mapper.Map<ClaimHouse, ClaimHouseInvalid>(item);
                //    dest.InvalidList = new List<string>() { "Dupplicate column PaidCode in database" };

                //    // Add item to invalid list
                //    invalid.Add(dest);
                //}

                #endregion Check Duplicate

                #region Validate AppID

                var notExistAppId = helpers.ValidateAppId(tmpValid.Where(x => !string.IsNullOrEmpty(x.AppID)).Select(x => x.AppID.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistAppId.Any(y => y.Equals(x.AppID.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found AppId");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistAppId.Any(y => y.Equals(x.AppID.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_ClaimHouse, ClaimHouseInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_ClaimHouse, ClaimHouseInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found AppId" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate AppID

                #region Validate EmployeeSS

                var notExistEmployeeSS = helpers.ValidateEmployeeSS(tmpValid.Where(x => !string.IsNullOrEmpty(x.EmpCodeApprove)).Select(x => x.EmpCodeApprove.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistEmployeeSS.Any(y => y.Equals(x.EmpCodeApprove.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found EmpCodeApprove");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistEmployeeSS.Any(y => y.Equals(x.EmpCodeApprove.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_ClaimHouse, ClaimHouseInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_ClaimHouse, ClaimHouseInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found EmpCodeApprove" };

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
            var latestPeriodList = _context.D_ClaimHouse.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_ClaimHouse.Remove(itm);
                    _context.SaveChanges();
                }
            }

            // Get data from session by datakey
            var valid = (List<D_ClaimHouse>)Session[dataKey];

            EFBatchOperation.For(_context, _context.D_ClaimHouse).InsertAll(valid);

            //call sp to update import log
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogManual_Update(periodResult.periodId, "D_ClaimHouse", user);

            Session[dataKey] = null;

            return Json("บันทึกข้อมูลเรียบร้อย");
        }

        [HttpPost]
        public JsonResult SaveNoData()
        {
            var periodResult = GlobalFunction.GetLatestPeriod();

            var latestPeriodList = _context.D_ClaimHouse.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_ClaimHouse.Remove(itm);
                    _context.SaveChanges();
                }
            }

            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogNoData_Update(periodResult.periodId, "D_ClaimHouse", user);

            return Json("บันทึกข้อมูลเรียบร้อย");
        }
    }
}