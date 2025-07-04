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
    public class ClaimNonMotor_OnLineController : Controller
    {
        #region Declare Variables

        private readonly CommissionCalculateEntities _context;

        public ClaimNonMotor_OnLineController()
        {
            // Set entity
            _context = new CommissionCalculateEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // TODO: Prepare invalid model and extend model for this import
        public class ClaimNonMotor_OnLineInvalid : D_ClaimNonMotor_OnLine
        {
            public List<string> InvalidList { get; set; }
        }

        #endregion Declare Variables

        // GET: ClaimNonMotor_OnLine
        public ActionResult Index()
        {
            // TODO: Prepare valid and invalid model for initial table
            ViewBag.Valid = new List<D_ClaimNonMotor_OnLine>();
            ViewBag.ValidJson = "[]";
            ViewBag.Invalid = new List<ClaimNonMotor_OnLineInvalid>();
            ViewBag.InvalidJson = "[]";
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var helpers = new ValidationHelpers();

            // TODO: Prepare valid and invalid model
            var valid = new List<D_ClaimNonMotor_OnLine>();
            var invalid = new List<ClaimNonMotor_OnLineInvalid>();

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
                        "RewardHistoryId"
                        ,"RewardId"
                        ,"ClaimOnLineId"
                        ,"ClaimOnLineCode"
                        ,"ClaimHeaderCode"
                        ,"ClaimCode"
                        ,"Pay"
                        ,"UnPay"
                        ,"ProductTypeId"
                        ,"ProductType"
                        ,"Detail"
                        ,"Zone"
                        ,"Branch"
                        ,"ServiceByEmpCode"
                        ,"ServiceByEmpName"
                        ,"TransferDateStart"
                        ,"TransferDateStart_D"
                        ,"TransferDateStart_M"
                        ,"TransferDateStart_Y"
                        ,"ReturnDateFinal"
                        ,"ReturnDateFinal_D"
                        ,"ReturnDateFinal_M"
                        ,"ReturnDateFinal_Y"
                        ,"OperateDays"
                        ,"CreatedDate"
                        ,"CreatedDate_D"
                        ,"CreatedDate_M"
                        ,"CreatedDate_Y"
                        ,"DateReceiveDocMO"
                        ,"DateReceiveDocMO_D"
                        ,"DateReceiveDocMO_M"
                        ,"DateReceiveDocMO_Y"
                        ,"DateReceiveDocBO"
                        ,"DateReceiveDocBO_D"
                        ,"DateReceiveDocBO_M"
                        ,"DateReceiveDocBO_Y"
                        ,"DateDiff_Transfer_DateReceiveDocMO"
                        ,"DateDiff_DateReceiveDocMO_DateReceiveDocBO"
                    };

                    var missingColumn = helpers.ValidateColumn(rows, columns);

                    if (missingColumn.Count > 0)
                    {
                        ViewBag.isInvalidColumn = true;

                        ViewBag.isInvalidMsg = "ไม่พบ column : " + String.Join(", ", missingColumn);

                        ViewBag.Valid = new List<D_ClaimNonMotor_OnLine>();
                        ViewBag.ValidJson = "[]";
                        ViewBag.Invalid = new List<ClaimNonMotor_OnLineInvalid>();
                        ViewBag.InvalidJson = "[]";

                        return View("Index");
                    }

                    #endregion Validate Column

                    //loop all rows
                    for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row - 1; i++)
                    {
                        // TODO: Prepare valid and invalid model for each row
                        var rowResult = new D_ClaimNonMotor_OnLine();
                        var invalidList = new List<string>();

                        #region Validate Column

                        // TODO: Check type and require column from document condition

                        // Column RewardHistoryId
                        var RewardHistoryId = helpers.ValidateString("RewardHistoryId", rows[i, 0], false);

                        if (RewardHistoryId.IsValid) rowResult.RewardHistoryId = RewardHistoryId.Value;
                        if (!RewardHistoryId.IsValid) invalidList.Add(RewardHistoryId.InvalidMessage);

                        // Column RewardId
                        var RewardId = helpers.ValidateDate("RewardId", rows[i, 1], false);

                        if (RewardId.IsValid) rowResult.RewardId = RewardId.Value;
                        if (!RewardId.IsValid) invalidList.Add(RewardId.InvalidMessage);

                        // Column ClaimOnLineId
                        var ClaimOnLineId = helpers.ValidateInt("ClaimOnLineId", rows[i, 2], false);

                        if (ClaimOnLineId.IsValid) rowResult.ClaimOnLineId = ClaimOnLineId.Value;
                        if (!ClaimOnLineId.IsValid) invalidList.Add(ClaimOnLineId.InvalidMessage);

                        // Column ClaimOnLineCode
                        var ClaimOnLineCode = helpers.ValidateString("ClaimOnLineCode", rows[i, 3], true);

                        if (ClaimOnLineCode.IsValid) rowResult.ClaimOnLineCode = ClaimOnLineCode.Value;
                        if (!ClaimOnLineCode.IsValid) invalidList.Add(ClaimOnLineCode.InvalidMessage);

                        // Column ClaimHeaderCode
                        var ClaimHeaderCode = helpers.ValidateString("ClaimHeaderCode", rows[i, 4], false);

                        if (ClaimHeaderCode.IsValid) rowResult.ClaimHeaderCode = ClaimHeaderCode.Value;
                        if (!ClaimHeaderCode.IsValid) invalidList.Add(ClaimHeaderCode.InvalidMessage);

                        // Column ClaimCode
                        var ClaimCode = helpers.ValidateString("ClaimCode", rows[i, 5], true);

                        if (ClaimCode.IsValid) rowResult.ClaimCode = ClaimCode.Value;
                        if (!ClaimCode.IsValid) invalidList.Add(ClaimCode.InvalidMessage);

                        // Column Pay
                        var Pay = helpers.ValidateString("Pay", rows[i, 6], false);

                        if (Pay.IsValid) rowResult.Pay = Pay.Value;
                        if (!Pay.IsValid) invalidList.Add(Pay.InvalidMessage);

                        // Column UnPay
                        var UnPay = helpers.ValidateString("UnPay", rows[i, 7], false);

                        if (UnPay.IsValid) rowResult.UnPay = UnPay.Value;
                        if (!UnPay.IsValid) invalidList.Add(UnPay.InvalidMessage);

                        // Column ProductTypeId
                        var ProductTypeId = helpers.ValidateInt("ProductTypeId", rows[i, 8], false);

                        if (ProductTypeId.IsValid) rowResult.ProductTypeId = ProductTypeId.Value;
                        if (!ProductTypeId.IsValid) invalidList.Add(ProductTypeId.InvalidMessage);

                        // Column ProductType
                        var ProductType = helpers.ValidateString("ProductType", rows[i, 9], false);

                        if (ProductType.IsValid) rowResult.ProductType = ProductType.Value;
                        if (!ProductType.IsValid) invalidList.Add(ProductType.InvalidMessage);

                        // Column Detail
                        var Detail = helpers.ValidateString("Detail", rows[i, 10], true);

                        if (Detail.IsValid) rowResult.Detail = Detail.Value;
                        if (!Detail.IsValid) invalidList.Add(Detail.InvalidMessage);

                        // Column Zone
                        var Zone = helpers.ValidateString("Zone", rows[i, 11], false);

                        if (Zone.IsValid) rowResult.Zone = Zone.Value;
                        if (!Zone.IsValid) invalidList.Add(Zone.InvalidMessage);

                        // Column Branch
                        var Branch = helpers.ValidateString("Branch", rows[i, 12], true);

                        if (Branch.IsValid) rowResult.Branch = Branch.Value;
                        if (!Branch.IsValid) invalidList.Add(Branch.InvalidMessage);

                        // Column ServiceByEmpCode
                        var ServiceByEmpCode = helpers.ValidateString("ServiceByEmpCode", rows[i, 13], true);

                        if (ServiceByEmpCode.IsValid) rowResult.ServiceByEmpCode = ServiceByEmpCode.Value;
                        if (!ServiceByEmpCode.IsValid) invalidList.Add(ServiceByEmpCode.InvalidMessage);

                        // Column ServiceByEmpCode
                        var ServiceByEmpName = helpers.ValidateString("ServiceByEmpName", rows[i, 14], true);

                        if (ServiceByEmpName.IsValid) rowResult.ServiceByEmpName = ServiceByEmpName.Value;
                        if (!ServiceByEmpName.IsValid) invalidList.Add(ServiceByEmpName.InvalidMessage);

                        // Column TransferDateStart
                        var TransferDateStart = helpers.ValidateDate("TransferDateStart", rows[i, 15], false);

                        if (TransferDateStart.IsValid) rowResult.TransferDateStart = TransferDateStart.Value;
                        if (!TransferDateStart.IsValid) invalidList.Add(TransferDateStart.InvalidMessage);

                        // Column TransferDateStart_D
                        var TransferDateStart_D = helpers.ValidateString("TransferDateStart_D", rows[i, 16], false);

                        if (TransferDateStart_D.IsValid) rowResult.TransferDateStart_D = TransferDateStart_D.Value;
                        if (!TransferDateStart_D.IsValid) invalidList.Add(TransferDateStart_D.InvalidMessage);

                        // Column TransferDateStart_M
                        var TransferDateStart_M = helpers.ValidateString("TransferDateStart_M", rows[i, 17], false);

                        if (TransferDateStart_M.IsValid) rowResult.TransferDateStart_M = TransferDateStart_M.Value;
                        if (!TransferDateStart_M.IsValid) invalidList.Add(TransferDateStart_M.InvalidMessage);

                        // Column TransferDateStart_Y
                        var TransferDateStart_Y = helpers.ValidateString("TransferDateStart_Y", rows[i, 18], false);

                        if (TransferDateStart_Y.IsValid) rowResult.TransferDateStart_Y = TransferDateStart_Y.Value;
                        if (!TransferDateStart_Y.IsValid) invalidList.Add(TransferDateStart_Y.InvalidMessage);

                        // Column ReturnDateFinal
                        var ReturnDateFinal = helpers.ValidateDate("ReturnDateFinal", rows[i, 19], false);

                        if (ReturnDateFinal.IsValid) rowResult.ReturnDateFinal = ReturnDateFinal.Value;
                        if (!ReturnDateFinal.IsValid) invalidList.Add(ReturnDateFinal.InvalidMessage);

                        // Column ReturnDateFinal_D
                        var ReturnDateFinal_D = helpers.ValidateString("ReturnDateFinal_D", rows[i, 20], false);

                        if (ReturnDateFinal_D.IsValid) rowResult.ReturnDateFinal_D = ReturnDateFinal_D.Value;
                        if (!ReturnDateFinal_D.IsValid) invalidList.Add(ReturnDateFinal_D.InvalidMessage);

                        // Column ReturnDateFinal_M
                        var ReturnDateFinal_M = helpers.ValidateString("ReturnDateFinal_M", rows[i, 21], false);

                        if (ReturnDateFinal_M.IsValid) rowResult.ReturnDateFinal_M = ReturnDateFinal_M.Value;
                        if (!ReturnDateFinal_M.IsValid) invalidList.Add(ReturnDateFinal_M.InvalidMessage);

                        // Column ReturnDateFinal_Y
                        var ReturnDateFinal_Y = helpers.ValidateString("ReturnDateFinal_Y", rows[i, 22], false);

                        if (ReturnDateFinal_Y.IsValid) rowResult.ReturnDateFinal_Y = ReturnDateFinal_Y.Value;
                        if (!ReturnDateFinal_Y.IsValid) invalidList.Add(ReturnDateFinal_Y.InvalidMessage);

                        // Column OperateDays
                        var OperateDays = helpers.ValidateInt("OperateDays", rows[i, 23], false);

                        if (OperateDays.IsValid) rowResult.OperateDays = OperateDays.Value;
                        if (!OperateDays.IsValid) invalidList.Add(OperateDays.InvalidMessage);

                        // Column CreatedDate
                        var CreatedDate = helpers.ValidateDate("CreatedDate", rows[i, 24], false);

                        if (CreatedDate.IsValid) rowResult.CreatedDate = CreatedDate.Value;
                        if (!CreatedDate.IsValid) invalidList.Add(CreatedDate.InvalidMessage);

                        // Column CreatedDate_D
                        var CreatedDate_D = helpers.ValidateString("CreatedDate_D", rows[i, 25], false);

                        if (CreatedDate_D.IsValid) rowResult.CreatedDate_D = CreatedDate_D.Value;
                        if (!CreatedDate_D.IsValid) invalidList.Add(CreatedDate_D.InvalidMessage);

                        // Column CreatedDate_M
                        var CreatedDate_M = helpers.ValidateString("CreatedDate_M", rows[i, 26], false);

                        if (CreatedDate_M.IsValid) rowResult.CreatedDate_M = CreatedDate_M.Value;
                        if (!CreatedDate_M.IsValid) invalidList.Add(CreatedDate_M.InvalidMessage);

                        // Column CreatedDate_Y
                        var CreatedDate_Y = helpers.ValidateString("CreatedDate_Y", rows[i, 27], false);

                        if (CreatedDate_Y.IsValid) rowResult.CreatedDate_Y = CreatedDate_Y.Value;
                        if (!CreatedDate_Y.IsValid) invalidList.Add(CreatedDate_Y.InvalidMessage);

                        // Column DateReceiveDocMO
                        var DateReceiveDocMO = helpers.ValidateDate("DateReceiveDocMO", rows[i, 28], true);

                        if (DateReceiveDocMO.IsValid) rowResult.DateReceiveDocMO = DateReceiveDocMO.Value;
                        if (!DateReceiveDocMO.IsValid) invalidList.Add(DateReceiveDocMO.InvalidMessage);

                        // Column DateReceiveDocMO_D
                        var DateReceiveDocMO_D = helpers.ValidateString("DateReceiveDocMO_D", rows[i, 29], true);

                        if (DateReceiveDocMO_D.IsValid) rowResult.DateReceiveDocMO_D = DateReceiveDocMO_D.Value;
                        if (!DateReceiveDocMO_D.IsValid) invalidList.Add(DateReceiveDocMO_D.InvalidMessage);

                        // Column DateReceiveDocMO_M
                        var DateReceiveDocMO_M = helpers.ValidateString("CreatedDate_M", rows[i, 30], true);

                        if (DateReceiveDocMO_M.IsValid) rowResult.DateReceiveDocMO_M = DateReceiveDocMO_M.Value;
                        if (!DateReceiveDocMO_M.IsValid) invalidList.Add(DateReceiveDocMO_M.InvalidMessage);

                        // Column DateReceiveDocMO_Y
                        var DateReceiveDocMO_Y = helpers.ValidateString("DateReceiveDocMO_Y", rows[i, 31], true);

                        if (DateReceiveDocMO_Y.IsValid) rowResult.DateReceiveDocMO_Y = DateReceiveDocMO_Y.Value;
                        if (!DateReceiveDocMO_Y.IsValid) invalidList.Add(DateReceiveDocMO_Y.InvalidMessage);

                        // Column DateReceiveDocBO
                        var DateReceiveDocBO = helpers.ValidateDate("DateReceiveDocBO", rows[i, 32], true);

                        if (DateReceiveDocBO.IsValid) rowResult.DateReceiveDocBO = DateReceiveDocBO.Value;
                        if (!DateReceiveDocBO.IsValid) invalidList.Add(DateReceiveDocBO.InvalidMessage);

                        // Column DateReceiveDocBO_D
                        var DateReceiveDocBO_D = helpers.ValidateString("DateReceiveDocBO_D", rows[i, 33], true);

                        if (DateReceiveDocBO_D.IsValid) rowResult.DateReceiveDocBO_D = DateReceiveDocBO_D.Value;
                        if (!DateReceiveDocBO_D.IsValid) invalidList.Add(DateReceiveDocBO_D.InvalidMessage);

                        // Column DateReceiveDocBO_M
                        var DateReceiveDocBO_M = helpers.ValidateString("DateReceiveDocBO_M", rows[i, 34], true);

                        if (DateReceiveDocBO_M.IsValid) rowResult.DateReceiveDocBO_M = DateReceiveDocBO_M.Value;
                        if (!DateReceiveDocBO_M.IsValid) invalidList.Add(DateReceiveDocBO_M.InvalidMessage);

                        // Column DateReceiveDocBO_Y
                        var DateReceiveDocBO_Y = helpers.ValidateString("DateReceiveDocBO_Y", rows[i, 35], true);

                        if (DateReceiveDocBO_Y.IsValid) rowResult.DateReceiveDocBO_Y = DateReceiveDocBO_Y.Value;
                        if (!DateReceiveDocBO_Y.IsValid) invalidList.Add(DateReceiveDocBO_Y.InvalidMessage);

                        // Column DateDiff_Transfer_DateReceiveDocMO
                        var DateDiff_Transfer_DateReceiveDocMO = helpers.ValidateInt("DateDiff_Transfer_DateReceiveDocMO", rows[i, 36], true);

                        if (DateDiff_Transfer_DateReceiveDocMO.IsValid) rowResult.DateDiff_Transfer_DateReceiveDocMO = DateDiff_Transfer_DateReceiveDocMO.Value;
                        if (!DateDiff_Transfer_DateReceiveDocMO.IsValid) invalidList.Add(DateDiff_Transfer_DateReceiveDocMO.InvalidMessage);

                        // Column DateDiff_DateReceiveDocMO_DateReceiveDocBO
                        var DateDiff_DateReceiveDocMO_DateReceiveDocBO = helpers.ValidateInt("DateDiff_DateReceiveDocMO_DateReceiveDocBO", rows[i, 37], true);

                        if (DateDiff_DateReceiveDocMO_DateReceiveDocBO.IsValid) rowResult.DateDiff_DateReceiveDocMO_DateReceiveDocBO = DateDiff_DateReceiveDocMO_DateReceiveDocBO.Value;
                        if (!DateDiff_DateReceiveDocMO_DateReceiveDocBO.IsValid) invalidList.Add(DateDiff_DateReceiveDocMO_DateReceiveDocBO.InvalidMessage);

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
                                cfg.CreateMap<D_ClaimNonMotor_OnLine, ClaimNonMotor_OnLineInvalid>();
                            });

                            IMapper mapper = config.CreateMapper();
                            var dest = mapper.Map<D_ClaimNonMotor_OnLine, ClaimNonMotor_OnLineInvalid>(rowResult);
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
                var getDupplidateInFile = valid.GroupBy(x => x.ClaimCode.Trim()).Where(x => x.Count() > 1)
                    .Select(x => x.Key).ToList();

                // TODO: Find in valid
                foreach (var item in valid.Where(x => getDupplidateInFile.Any(y => y.Equals(x.ClaimCode.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_ClaimNonMotor_OnLine, ClaimNonMotor_OnLineInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_ClaimNonMotor_OnLine, ClaimNonMotor_OnLineInvalid>(item);
                    dest.InvalidList = new List<string>() { "Dupplicate column ClaimCode in upload file" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Check Duplicate

                #region Validate Claim Id

                var notExistArea = helpers.ValidateClaimCode(tmpValid.Where(x => !string.IsNullOrEmpty(x.ClaimCode)).Select(x => x.ClaimCode.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistArea.Any(y => y.Equals(x.ClaimCode.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found ClaimCode");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistArea.Any(y => y.Equals(x.ClaimCode.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_ClaimNonMotor_OnLine, ClaimNonMotor_OnLineInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_ClaimNonMotor_OnLine, ClaimNonMotor_OnLineInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found ClaimCode" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Claim Id
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
            var latestPeriodList = _context.D_ClaimNonMotor_OnLine.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_ClaimNonMotor_OnLine.Remove(itm);
                    _context.SaveChanges();
                }
            }

            // Get data from session by datakey
            var valid = (List<D_ClaimNonMotor_OnLine>)Session[dataKey];

            EFBatchOperation.For(_context, _context.D_ClaimNonMotor_OnLine).InsertAll(valid);

            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogManual_Update(periodResult.periodId, "D_ClaimNonMotor_OnLine", user);

            Session[dataKey] = null;

            return Json("บันทึกข้อมูลเรียบร้อย");
        }

        [HttpPost]
        public JsonResult SaveNoData()
        {
            var periodResult = GlobalFunction.GetLatestPeriod();

            var latestPeriodList = _context.D_ClaimNonMotor_OnLine.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_ClaimNonMotor_OnLine.Remove(itm);
                    _context.SaveChanges();
                }
            }

            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            var result = _context.usp_DatasourceImportedLogNoData_Update(periodResult.periodId, "D_ClaimNonMotor_OnLine", user).FirstOrDefault();

            return Json("บันทึกข้อมูลเรียบร้อย");
        }
    }
}