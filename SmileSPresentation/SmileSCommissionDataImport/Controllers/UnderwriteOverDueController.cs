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
    public class UnderwriteOverDueController : Controller
    {
        #region Declare Variables

        private readonly CommissionCalculateEntities _context;

        public UnderwriteOverDueController()
        {
            // Set entity
            _context = new CommissionCalculateEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // TODO: Prepare invalid model and extend model for this import
        public class UnderwriteOverDueInvalid : D_UnderwriteOverDue
        {
            public List<string> InvalidList { get; set; }
        }

        #endregion Declare Variables

        // GET: UnderwriteOverDue
        public ActionResult Index()
        {
            // TODO: Prepare valid and invalid model for initial table
            ViewBag.Valid = new List<D_UnderwriteOverDue>();
            ViewBag.ValidJson = "[]";
            ViewBag.Invalid = new List<UnderwriteOverDueInvalid>();
            ViewBag.InvalidJson = "[]";

            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var helpers = new ValidationHelpers();

            // TODO: Prepare valid and invalid model
            var valid = new List<D_UnderwriteOverDue>();
            var invalid = new List<UnderwriteOverDueInvalid>();

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
                        "PayerStudyArea_id",
                        "PayerStudyArea",
                        "PayerBranch_id",
                        "PayerBranch",
                        "QueueOwnerCode",
                        "QueueOwner",
                        "Period",
                        "ApplicationCode",
                        "ApplicationStatus",
                        "CoverDate",
                        "CustomerName",
                        "PayerName",
                        "Office",
                        "CreateBy",
                        "CreateDate",
                        "UpdateResultDate",
                        "DateDiff",
                        "SendDate",
                        "ExprieDate",
                        "UnderwriteHQResult",
                        "UnderwriteHQRemark",
                        "UnderwriteHQDate",
                        "BranchUnderwriteStatus",
                        "QueueStatus",
                        "ChanalType",
                        "CallStatus",
                        "UnderwriteCustomer",
                        "UnderwritePayer",
                        "ChanalStatus",
                        "ConfirmDirectDebitBank",
                        "ConfirmDirectDebitBankRemark",
                        "UnderwriteResult",
                        "UnaprroveHealth",
                        "UnaprroveOccupation",
                        "UnaprrovePremium",
                        "UnaprroveOther",
                        "Remark",
                        "Sale_id",
                        "Sale",
                        "SaleBranch_id",
                        "SaleBranch",
                        "SaleTeam_id",
                        "SaleTeam",
                        "Result"
                    };

                    var missingColumn = helpers.ValidateColumn(rows, columns);

                    if (missingColumn.Count > 0)
                    {
                        ViewBag.isInvalidColumn = true;

                        ViewBag.isInvalidMsg = "ไม่พบ column : " + String.Join(", ", missingColumn);

                        ViewBag.Valid = new List<D_UnderwriteOverDue>();
                        ViewBag.ValidJson = "[]";
                        ViewBag.Invalid = new List<UnderwriteOverDueInvalid>();
                        ViewBag.InvalidJson = "[]";

                        return View("Index");
                    }

                    #endregion Validate Column

                    //loop all rows
                    for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row - 1; i++)
                    {
                        // TODO: Prepare valid and invalid model for each row
                        var rowResult = new D_UnderwriteOverDue();
                        var invalidList = new List<string>();

                        #region Validate Column

                        // TODO: Check type and require column from document condition

                        //PayerStudyArea_id
                        var PayerStudyArea_id = helpers.ValidateString("PayerStudyArea_id", rows[i, 0], true);

                        if (PayerStudyArea_id.IsValid) rowResult.PayerStudyArea_id = PayerStudyArea_id.Value;
                        if (!PayerStudyArea_id.IsValid) invalidList.Add(PayerStudyArea_id.InvalidMessage);

                        //PayerStudyArea
                        var PayerStudyArea = helpers.ValidateString("PayerStudyArea", rows[i, 1], true);

                        if (PayerStudyArea.IsValid) rowResult.PayerStudyArea = PayerStudyArea.Value;
                        if (!PayerStudyArea.IsValid) invalidList.Add(PayerStudyArea.InvalidMessage);

                        //PayerBranch_id
                        var PayerBranch_id = helpers.ValidateString("PayerBranch_id", rows[i, 2], true);

                        if (PayerBranch_id.IsValid) rowResult.PayerBranch_id = PayerBranch_id.Value;
                        if (!PayerBranch_id.IsValid) invalidList.Add(PayerBranch_id.InvalidMessage);

                        //PayerBranch
                        var PayerBranch = helpers.ValidateString("PayerBranch", rows[i, 3], true);

                        if (PayerBranch.IsValid) rowResult.PayerBranch = PayerBranch.Value;
                        if (!PayerBranch.IsValid) invalidList.Add(PayerBranch.InvalidMessage);

                        //QueueOwnerCode
                        var QueueOwnerCode = helpers.ValidateString("QueueOwnerCode", rows[i, 4], true);

                        if (QueueOwnerCode.IsValid) rowResult.QueueOwnerCode = QueueOwnerCode.Value;
                        if (!QueueOwnerCode.IsValid) invalidList.Add(QueueOwnerCode.InvalidMessage);

                        //QueueOwner
                        var QueueOwner = helpers.ValidateString("QueueOwner", rows[i, 5], true);

                        if (QueueOwner.IsValid) rowResult.QueueOwner = QueueOwner.Value;
                        if (!QueueOwner.IsValid) invalidList.Add(QueueOwner.InvalidMessage);

                        //Period
                        var Period = helpers.ValidateDate("Period", rows[i, 6], true);

                        if (Period.IsValid) rowResult.Period = Period.Value;
                        if (!Period.IsValid) invalidList.Add(Period.InvalidMessage);

                        //ApplicationCode
                        var ApplicationCode = helpers.ValidateString("ApplicationCode", rows[i, 7], true);

                        if (ApplicationCode.IsValid) rowResult.ApplicationCode = ApplicationCode.Value;
                        if (!ApplicationCode.IsValid) invalidList.Add(ApplicationCode.InvalidMessage);

                        //ApplicationStatus
                        var ApplicationStatus = helpers.ValidateString("ApplicationStatus", rows[i, 8], true);

                        if (ApplicationStatus.IsValid) rowResult.ApplicationStatus = ApplicationStatus.Value;
                        if (!ApplicationStatus.IsValid) invalidList.Add(ApplicationStatus.InvalidMessage);

                        //CoverDate
                        var CoverDate = helpers.ValidateString("CoverDate", rows[i, 9], true);

                        if (CoverDate.IsValid) rowResult.CoverDate = CoverDate.Value;
                        if (!CoverDate.IsValid) invalidList.Add(CoverDate.InvalidMessage);

                        //CustomerName
                        var CustomerName = helpers.ValidateString("CustomerName", rows[i, 10], true);

                        if (CustomerName.IsValid) rowResult.CustomerName = CustomerName.Value;
                        if (!CustomerName.IsValid) invalidList.Add(CustomerName.InvalidMessage);

                        //PayerName
                        var PayerName = helpers.ValidateString("PayerName", rows[i, 11], true);

                        if (PayerName.IsValid) rowResult.PayerName = PayerName.Value;
                        if (!PayerName.IsValid) invalidList.Add(PayerName.InvalidMessage);

                        //Office
                        var Office = helpers.ValidateString("Office", rows[i, 12], true);

                        if (Office.IsValid) rowResult.Office = Office.Value;
                        if (!Office.IsValid) invalidList.Add(Office.InvalidMessage);

                        //CreateBy
                        var CreateBy = helpers.ValidateString("CreateBy", rows[i, 13], true);

                        if (CreateBy.IsValid) rowResult.CreateBy = CreateBy.Value;
                        if (!CreateBy.IsValid) invalidList.Add(CreateBy.InvalidMessage);

                        //CreateDate
                        var CreateDate = helpers.ValidateDate("CreateDate", rows[i, 14], true);

                        if (CreateDate.IsValid) rowResult.CreateDate = CreateDate.Value;
                        if (!CreateDate.IsValid) invalidList.Add(CreateDate.InvalidMessage);

                        //UpdateResultDate
                        var UpdateResultDate = helpers.ValidateDate("UpdateResultDate", rows[i, 15], false);

                        if (UpdateResultDate.IsValid && UpdateResultDate.Value != null) rowResult.UpdateResultDate = UpdateResultDate.Value;
                        if (!UpdateResultDate.IsValid) invalidList.Add(UpdateResultDate.InvalidMessage);

                        //DateDiff
                        var DateDiff = helpers.ValidateInt("DateDiff", rows[i, 16], false);

                        if (DateDiff.IsValid) rowResult.DateDiff = DateDiff.Value;
                        if (!DateDiff.IsValid) invalidList.Add(DateDiff.InvalidMessage);

                        //SendDate
                        var SendDate = helpers.ValidateDate("SendDate", rows[i, 17], true);

                        if (SendDate.IsValid) rowResult.SendDate = SendDate.Value;
                        if (!SendDate.IsValid) invalidList.Add(SendDate.InvalidMessage);

                        //ExprieDate
                        var ExprieDate = helpers.ValidateDate("ExprieDate", rows[i, 18], true);

                        if (ExprieDate.IsValid) rowResult.ExprieDate = ExprieDate.Value;
                        if (!ExprieDate.IsValid) invalidList.Add(ExprieDate.InvalidMessage);

                        //UnderwriteHQResult
                        var UnderwriteHQResult = helpers.ValidateString("UnderwriteHQResult", rows[i, 19], true);

                        if (UnderwriteHQResult.IsValid) rowResult.UnderwriteHQResult = UnderwriteHQResult.Value;
                        if (!UnderwriteHQResult.IsValid) invalidList.Add(UnderwriteHQResult.InvalidMessage);

                        //UnderwriteHQRemark
                        var UnderwriteHQRemark = helpers.ValidateString("UnderwriteHQRemark", rows[i, 20], false);

                        if (UnderwriteHQRemark.IsValid) rowResult.UnderwriteHQRemark = UnderwriteHQRemark.Value;
                        if (!UnderwriteHQRemark.IsValid) invalidList.Add(UnderwriteHQRemark.InvalidMessage);

                        //UnderwriteHQDate
                        var UnderwriteHQDate = helpers.ValidateString("UnderwriteHQDate", rows[i, 21], false);

                        if (UnderwriteHQDate.IsValid) rowResult.UnderwriteHQDate = UnderwriteHQDate.Value;
                        if (!UnderwriteHQDate.IsValid) invalidList.Add(UnderwriteHQDate.InvalidMessage);

                        //BranchUnderwriteStatus
                        var BranchUnderwriteStatus = helpers.ValidateString("BranchUnderwriteStatus", rows[i, 22], true);

                        if (BranchUnderwriteStatus.IsValid) rowResult.BranchUnderwriteStatus = BranchUnderwriteStatus.Value;
                        if (!BranchUnderwriteStatus.IsValid) invalidList.Add(BranchUnderwriteStatus.InvalidMessage);

                        //QueueStatus
                        var QueueStatus = helpers.ValidateString("QueueStatus", rows[i, 23], true);

                        if (QueueStatus.IsValid) rowResult.QueueStatus = QueueStatus.Value;
                        if (!QueueStatus.IsValid) invalidList.Add(QueueStatus.InvalidMessage);

                        //ChanalType
                        var ChanalType = helpers.ValidateString("ChanalType", rows[i, 24], true);

                        if (ChanalType.IsValid) rowResult.ChanalType = ChanalType.Value;
                        if (!ChanalType.IsValid) invalidList.Add(ChanalType.InvalidMessage);

                        //CallStatus
                        var CallStatus = helpers.ValidateString("CallStatus", rows[i, 25], true);

                        if (CallStatus.IsValid) rowResult.CallStatus = CallStatus.Value;
                        if (!CallStatus.IsValid) invalidList.Add(CallStatus.InvalidMessage);

                        //UnderwriteCustomer
                        var UnderwriteCustomer = helpers.ValidateString("UnderwriteCustomer", rows[i, 26], true);

                        if (UnderwriteCustomer.IsValid) rowResult.UnderwriteCustomer = UnderwriteCustomer.Value;
                        if (!UnderwriteCustomer.IsValid) invalidList.Add(UnderwriteCustomer.InvalidMessage);

                        //UnderwritePayer
                        var UnderwritePayer = helpers.ValidateString("UnderwritePayer", rows[i, 27], true);

                        if (UnderwritePayer.IsValid) rowResult.UnderwritePayer = UnderwritePayer.Value;
                        if (!UnderwritePayer.IsValid) invalidList.Add(UnderwritePayer.InvalidMessage);

                        //ChanalStatus
                        var ChanalStatus = helpers.ValidateString("ChanalStatus", rows[i, 28], false);

                        if (ChanalStatus.IsValid) rowResult.ChanalStatus = ChanalStatus.Value;
                        if (!ChanalStatus.IsValid) invalidList.Add(ChanalStatus.InvalidMessage);

                        //ConfirmDirectDebitBank
                        var ConfirmDirectDebitBank = helpers.ValidateString("ConfirmDirectDebitBank", rows[i, 29], false);

                        if (ConfirmDirectDebitBank.IsValid) rowResult.ConfirmDirectDebitBank = ConfirmDirectDebitBank.Value;
                        if (!ConfirmDirectDebitBank.IsValid) invalidList.Add(ConfirmDirectDebitBank.InvalidMessage);

                        //ConfirmDirectDebitBankRemark
                        var ConfirmDirectDebitBankRemark = helpers.ValidateString("ConfirmDirectDebitBankRemark", rows[i, 30], false);

                        if (ConfirmDirectDebitBankRemark.IsValid) rowResult.ConfirmDirectDebitBankRemark = ConfirmDirectDebitBankRemark.Value;
                        if (!ConfirmDirectDebitBankRemark.IsValid) invalidList.Add(ConfirmDirectDebitBankRemark.InvalidMessage);

                        //UnderwriteResult
                        var UnderwriteResult = helpers.ValidateString("UnderwriteResult", rows[i, 31], false);

                        if (UnderwriteResult.IsValid) rowResult.UnderwriteResult = UnderwriteResult.Value;
                        if (!UnderwriteResult.IsValid) invalidList.Add(UnderwriteResult.InvalidMessage);

                        //UnaprroveHealth
                        var UnaprroveHealth = helpers.ValidateString("UnaprroveHealth", rows[i, 32], false);

                        if (UnaprroveHealth.IsValid) rowResult.UnaprroveHealth = UnaprroveHealth.Value;
                        if (!UnaprroveHealth.IsValid) invalidList.Add(UnaprroveHealth.InvalidMessage);

                        //UnaprroveOccupation
                        var UnaprroveOccupation = helpers.ValidateString("UnaprroveOccupation", rows[i, 33], false);

                        if (UnaprroveOccupation.IsValid) rowResult.UnaprroveOccupation = UnaprroveOccupation.Value;
                        if (!UnaprroveOccupation.IsValid) invalidList.Add(UnaprroveOccupation.InvalidMessage);

                        //UnaprrovePremium
                        var UnaprrovePremium = helpers.ValidateString("UnaprrovePremium", rows[i, 34], false);

                        if (UnaprrovePremium.IsValid) rowResult.UnaprrovePremium = UnaprrovePremium.Value;
                        if (!UnaprrovePremium.IsValid) invalidList.Add(UnaprrovePremium.InvalidMessage);

                        //UnaprroveOther
                        var UnaprroveOther = helpers.ValidateString("UnaprroveOther", rows[i, 35], false);

                        if (UnaprroveOther.IsValid) rowResult.UnaprroveOther = UnaprroveOther.Value;
                        if (!UnaprroveOther.IsValid) invalidList.Add(UnaprroveOther.InvalidMessage);

                        //Remark
                        var Remark = helpers.ValidateString("Remark", rows[i, 36], false);

                        if (Remark.IsValid) rowResult.Remark = Remark.Value;
                        if (!Remark.IsValid) invalidList.Add(Remark.InvalidMessage);

                        //Sale_id
                        var Sale_id = helpers.ValidateString("Sale_id", rows[i, 37], true);

                        if (Sale_id.IsValid) rowResult.Sale_id = Sale_id.Value;
                        if (!Sale_id.IsValid) invalidList.Add(Sale_id.InvalidMessage);

                        //Sale
                        var Sale = helpers.ValidateString("Sale", rows[i, 38], true);

                        if (Sale.IsValid) rowResult.Sale = Sale.Value;
                        if (!Sale.IsValid) invalidList.Add(Sale.InvalidMessage);

                        //SaleBranch_id
                        var SaleBranch_id = helpers.ValidateString("SaleBranch_id", rows[i, 39], true);

                        if (SaleBranch_id.IsValid) rowResult.SaleBranch_id = SaleBranch_id.Value;
                        if (!SaleBranch_id.IsValid) invalidList.Add(SaleBranch_id.InvalidMessage);

                        //SaleBranch
                        var SaleBranch = helpers.ValidateString("SaleBranch", rows[i, 40], true);

                        if (SaleBranch.IsValid) rowResult.SaleBranch = SaleBranch.Value;
                        if (!SaleBranch.IsValid) invalidList.Add(SaleBranch.InvalidMessage);

                        //SaleTeam_id
                        var SaleTeam_id = helpers.ValidateString("SaleTeam_id", rows[i, 41], true);

                        if (SaleTeam_id.IsValid) rowResult.SaleTeam_id = SaleTeam_id.Value;
                        if (!SaleTeam_id.IsValid) invalidList.Add(SaleTeam_id.InvalidMessage);

                        //SaleTeam
                        var SaleTeam = helpers.ValidateString("SaleTeam", rows[i, 42], true);

                        if (SaleTeam.IsValid) rowResult.SaleTeam = SaleTeam.Value;
                        if (!SaleTeam.IsValid) invalidList.Add(SaleTeam.InvalidMessage);

                        //Result
                        var Result = helpers.ValidateString("Result", rows[i, 43], true);

                        if (Result.IsValid) rowResult.Result = Result.Value;
                        if (!Result.IsValid) invalidList.Add(Result.InvalidMessage);

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
                                cfg.CreateMap<D_UnderwriteOverDue, UnderwriteOverDueInvalid>();
                            });

                            IMapper mapper = config.CreateMapper();
                            var dest = mapper.Map<D_UnderwriteOverDue, UnderwriteOverDueInvalid>(rowResult);
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

                //// TODO: Check Duplicate in File
                //var getDupplidateInFile = valid.GroupBy(x => x.QueueOwnerCode.Trim()).Where(x => x.Count() > 1)
                //    .Select(x => x.Key).ToList();

                //// TODO: Find in valid
                //foreach (var item in valid.Where(x => getDupplidateInFile.Any(y => y.Equals(x.QueueOwnerCode.Trim()))).ToList())
                //{
                //    // Remove item from valid list
                //    valid.Remove(item);

                //    // Map valid item to invalid
                //    var config = new MapperConfiguration(cfg =>
                //    {
                //        cfg.CreateMap<UnderwriteOverDue, UnderwriteOverDueInvalid>();
                //    });

                //    IMapper mapper = config.CreateMapper();
                //    var dest = mapper.Map<UnderwriteOverDue, UnderwriteOverDueInvalid>(item);
                //    dest.InvalidList = new List<string>() { "Dupplicate column QueueOwnerCode in upload file" };

                //    // Add item to invalid list
                //    invalid.Add(dest);
                //}

                //// TODO: Check Duplicate in database

                //var existingDupplicate = _context.UnderwriteOverDues.SqlQuery(
                //    string.Format("SELECT * FROM dbo.UnderwriteOverDue WHERE QueueOwnerCode IN ('{0}')",
                //        string.Join("','", tmpValid.Select(x => x.QueueOwnerCode).ToArray()))).ToList();

                //// TODO: Find in exist invalid
                //foreach (var item in invalid.Where(x => existingDupplicate.Any(y => y.QueueOwnerCode.Trim().Equals(x.QueueOwnerCode.Trim()))).ToList())
                //{
                //    item.InvalidList.Add("Dupplicate column QueueOwnerCode in database");
                //}

                //// TODO: Find in valid
                //foreach (var item in valid.Where(x => existingDupplicate.Any(y => y.QueueOwnerCode.Trim().Equals(x.QueueOwnerCode.Trim()))).ToList())
                //{
                //    // Remove item from valid list
                //    valid.Remove(item);

                //    // Map valid item to invalid
                //    var config = new MapperConfiguration(cfg =>
                //    {
                //        cfg.CreateMap<UnderwriteOverDue, UnderwriteOverDueInvalid>();
                //    });

                //    IMapper mapper = config.CreateMapper();
                //    var dest = mapper.Map<UnderwriteOverDue, UnderwriteOverDueInvalid>(item);
                //    dest.InvalidList = new List<string>() { "Dupplicate column QueueOwnerCode in database" };

                //    // Add item to invalid list
                //    invalid.Add(dest);
                //}

                #endregion Check Duplicate

                #region Validate Area

                var notExistArea = helpers.ValidateArea(tmpValid.Where(x => !string.IsNullOrEmpty(x.PayerStudyArea_id)).Select(x => x.PayerStudyArea_id.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistArea.Any(y => y.Equals(x.PayerStudyArea_id.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found PayerStudyArea_id");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistArea.Any(y => y.Equals(x.PayerStudyArea_id.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_UnderwriteOverDue, UnderwriteOverDueInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_UnderwriteOverDue, UnderwriteOverDueInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found PayerStudyArea_id" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Area

                #region Validate Branch

                var notExistBranch = helpers.ValidateBranch(tmpValid.Where(x => !string.IsNullOrEmpty(x.PayerBranch_id)).Select(x => x.PayerBranch_id.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistBranch.Any(y => y.Equals(x.PayerBranch_id.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found PayerBranch_id");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistBranch.Any(y => y.Equals(x.PayerBranch_id.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_UnderwriteOverDue, UnderwriteOverDueInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_UnderwriteOverDue, UnderwriteOverDueInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found PayerBranch_id" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Branch

                #region Validate EmployeeSS

                var notExistEmployeeSS = helpers.ValidateEmployeeSS(tmpValid.Where(x => !string.IsNullOrEmpty(x.QueueOwnerCode)).Select(x => x.QueueOwnerCode.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistEmployeeSS.Any(y => y.Equals(x.QueueOwnerCode.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found QueueOwnerCode");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistEmployeeSS.Any(y => y.Equals(x.QueueOwnerCode.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_UnderwriteOverDue, UnderwriteOverDueInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_UnderwriteOverDue, UnderwriteOverDueInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found QueueOwnerCode" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate EmployeeSS

                #region Validate AppID

                var notExistAppId = helpers.ValidateAppId(tmpValid.Where(x => !string.IsNullOrEmpty(x.ApplicationCode)).Select(x => x.ApplicationCode.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistAppId.Any(y => y.Equals(x.ApplicationCode.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found ApplicationCode");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistAppId.Any(y => y.Equals(x.ApplicationCode.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_UnderwriteOverDue, UnderwriteOverDueInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_UnderwriteOverDue, UnderwriteOverDueInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found ApplicationCode" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate AppID

                #region Validate EmployeeSS

                var notExistEmployeeSS2 = helpers.ValidateEmployeeSS(tmpValid.Where(x => !string.IsNullOrEmpty(x.Sale_id)).Select(x => x.Sale_id.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistEmployeeSS2.Any(y => y.Equals(x.Sale_id.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found Sale_id");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistEmployeeSS2.Any(y => y.Equals(x.Sale_id.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_UnderwriteOverDue, UnderwriteOverDueInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_UnderwriteOverDue, UnderwriteOverDueInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found Sale_id" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate EmployeeSS

                #region Validate Branch

                var notExistBranch2 = helpers.ValidateBranch(tmpValid.Where(x => !string.IsNullOrEmpty(x.SaleBranch_id)).Select(x => x.SaleBranch_id.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistBranch2.Any(y => y.Equals(x.SaleBranch_id.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found SaleBranch_id");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistBranch2.Any(y => y.Equals(x.SaleBranch_id.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_UnderwriteOverDue, UnderwriteOverDueInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_UnderwriteOverDue, UnderwriteOverDueInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found SaleBranch_id" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Branch

                #region Validate Team

                var notExistTeam = helpers.ValidateTeam(tmpValid.Where(x => !string.IsNullOrEmpty(x.SaleTeam_id)).Select(x => x.SaleTeam_id.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistTeam.Any(y => y.Equals(x.SaleTeam_id.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found SaleTeam_id");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistTeam.Any(y => y.Equals(x.SaleTeam_id.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_UnderwriteOverDue, UnderwriteOverDueInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_UnderwriteOverDue, UnderwriteOverDueInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found SaleTeam_id" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Team
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
            var latestPeriodList = _context.D_UnderwriteOverDue.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_UnderwriteOverDue.Remove(itm);
                    _context.SaveChanges();
                }
            }

            // Get data from session by datakey
            var valid = (List<D_UnderwriteOverDue>)Session[dataKey];

            EFBatchOperation.For(_context, _context.D_UnderwriteOverDue).InsertAll(valid);

            //call sp to update import log
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogManual_Update(periodResult.periodId, "D_UnderwriteOverDue", user);

            Session[dataKey] = null;

            return Json("บันทึกข้อมูลเรียบร้อย");
        }

        [HttpPost]
        public JsonResult SaveNoData()
        {
            var periodResult = GlobalFunction.GetLatestPeriod();

            var latestPeriodList = _context.D_UnderwriteOverDue.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_UnderwriteOverDue.Remove(itm);
                    _context.SaveChanges();
                }
            }

            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogNoData_Update(periodResult.periodId, "D_UnderwriteOverDue", user);

            return Json("บันทึกข้อมูลเรียบร้อย");
        }
    }
}