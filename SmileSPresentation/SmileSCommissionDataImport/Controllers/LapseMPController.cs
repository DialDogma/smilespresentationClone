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
    public class LapseMPController : Controller
    {
        #region Declare Variables

        private readonly CommissionCalculateEntities _context;

        public LapseMPController()
        {
            // Set entity
            _context = new CommissionCalculateEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // TODO: Prepare invalid model and extend model for this import
        public class LapseMPInvalid : D_LapseMP
        {
            public List<string> InvalidList { get; set; }
        }

        #endregion Declare Variables

        // GET: LapseMP
        public ActionResult Index()
        {
            // TODO: Prepare valid and invalid model for initial table
            ViewBag.Valid = new List<D_LapseMP>();
            ViewBag.ValidJson = "[]";
            ViewBag.Invalid = new List<LapseMPInvalid>();
            ViewBag.InvalidJson = "[]";
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var helpers = new ValidationHelpers();

            // TODO: Prepare valid and invalid model
            var valid = new List<D_LapseMP>();
            var invalid = new List<LapseMPInvalid>();

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
                        "App_id"
                        ,"Product_id"
                        ,"Product"
                        ,"Premium"
                        ,"StartCoverDate"
                        ,"StartCoverDate_Year"
                        ,"StartCoverDate_Month"
                        ,"StartCoverDate_Day"
                        ,"Status"
                        ,"Custname"
                        ,"CancelDate"
                        ,"CancelDate_Year"
                        ,"CancelDate_Month"
                        ,"CancelDate_Day"
                        ,"PeriodTypeDetail"
                        ,"Age_Cover"
                        ,"Payername"
                        ,"PayerBuildingName"
                        ,"PayerTumbol"
                        ,"PayerAmphoe_id"
                        ,"PayerAmphoe"
                        ,"PayerStudyArea_id"
                        ,"PayerStudyArea"
                        ,"PayerProvince_id"
                        ,"PayerProvince"
                        ,"PayerBranchID"
                        ,"PayerBranch"
                        ,"Sale_id1"
                        ,"SaleName"
                        ,"AppTeamID"
                        ,"AppTeamName"
                        ,"AppBranchID"
                        ,"AppBranchName"
                        ,"Sale_id2"
                        ,"SaleTeam_id2"
                        ,"CreatedBy_id"
                        ,"CreatedDate"
                        ,"ProductGroup_id"
                        ,"ProductGroup"
                        ,"CancelCause"
                        ,"PortfolioPeriod"
                        ,"DateDiff_Portfolio_Cancel"
                        ,"Insured_Title"
                        ,"Insured_First_Name"
                        ,"Insured_Last_Name"
                    };

                    var missingColumn = helpers.ValidateColumn(rows, columns);

                    if (missingColumn.Count > 0)
                    {
                        ViewBag.isInvalidColumn = true;

                        ViewBag.isInvalidMsg = "ไม่พบ column : " + String.Join(", ", missingColumn);

                        ViewBag.Valid = new List<D_LapseMP>();
                        ViewBag.ValidJson = "[]";
                        ViewBag.Invalid = new List<LapseMPInvalid>();
                        ViewBag.InvalidJson = "[]";

                        return View("Index");
                    }

                    #endregion Validate Column

                    //loop all rows
                    for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row - 1; i++)
                    {
                        // TODO: Prepare valid and invalid model for each row
                        var rowResult = new D_LapseMP();
                        var invalidList = new List<string>();

                        #region Validate Column

                        // TODO: Check type and require column from document condition

                        // Column App_id
                        var App_id = helpers.ValidateString("App_id", rows[i, 0], true);

                        if (App_id.IsValid) rowResult.App_id = App_id.Value;
                        if (!App_id.IsValid) invalidList.Add(App_id.InvalidMessage);

                        // Column Product_id
                        var Product_id = helpers.ValidateString("Product_id", rows[i, 1], false);

                        if (Product_id.IsValid) rowResult.Product_id = Product_id.Value;
                        if (!Product_id.IsValid) invalidList.Add(Product_id.InvalidMessage);

                        // Column Product
                        var Product = helpers.ValidateString("Product", rows[i, 2], true);

                        if (Product.IsValid) rowResult.Product = Product.Value;
                        if (!Product.IsValid) invalidList.Add(Product.InvalidMessage);

                        // Column Premium
                        var Premium = helpers.ValidateInt("Premium", rows[i, 3], true);

                        if (Premium.IsValid) rowResult.Premium = Premium.Value;
                        if (!Premium.IsValid) invalidList.Add(Premium.InvalidMessage);

                        // Column StartCoverDate
                        var StartCoverDate = helpers.ValidateDate("StartCoverDate", rows[i, 4], true);

                        if (StartCoverDate.IsValid) rowResult.StartCoverDate = StartCoverDate.Value;
                        if (!StartCoverDate.IsValid) invalidList.Add(StartCoverDate.InvalidMessage);

                        // Column StartCoverDate_Year
                        var StartCoverDate_Year = helpers.ValidateString("StartCoverDate_Year", rows[i, 5], true);

                        if (StartCoverDate_Year.IsValid) rowResult.StartCoverDate_Year = StartCoverDate_Year.Value;
                        if (!StartCoverDate_Year.IsValid) invalidList.Add(StartCoverDate_Year.InvalidMessage);

                        // Column StartCoverDate_Month
                        var StartCoverDate_Month = helpers.ValidateString("StartCoverDate_Month", rows[i, 6], true);

                        if (StartCoverDate_Month.IsValid) rowResult.StartCoverDate_Month = StartCoverDate_Month.Value;
                        if (!StartCoverDate_Month.IsValid) invalidList.Add(StartCoverDate_Month.InvalidMessage);

                        // Column StartCoverDate_Day
                        var StartCoverDate_Day = helpers.ValidateString("StartCoverDate_Day", rows[i, 7], true);

                        if (StartCoverDate_Day.IsValid) rowResult.StartCoverDate_Day = StartCoverDate_Day.Value;
                        if (!StartCoverDate_Day.IsValid) invalidList.Add(StartCoverDate_Day.InvalidMessage);

                        // Column Status
                        var Status = helpers.ValidateString("Status", rows[i, 8], true);

                        if (Status.IsValid) rowResult.Status = Status.Value;
                        if (!Status.IsValid) invalidList.Add(Status.InvalidMessage);

                        // Column Custname
                        var Custname = helpers.ValidateString("Custname", rows[i, 9], false);

                        if (Custname.IsValid) rowResult.Custname = Custname.Value;
                        if (!Custname.IsValid) invalidList.Add(Custname.InvalidMessage);

                        // Column CancelDate
                        var CancelDate = helpers.ValidateDate("CancelDate", rows[i, 10], true);

                        if (CancelDate.IsValid) rowResult.CancelDate = CancelDate.Value;
                        if (!CancelDate.IsValid) invalidList.Add(CancelDate.InvalidMessage);

                        // Column CancelDate_Year
                        var CancelDate_Year = helpers.ValidateString("CancelDate_Year", rows[i, 11], true);

                        if (CancelDate_Year.IsValid) rowResult.CancelDate_Year = CancelDate_Year.Value;
                        if (!CancelDate_Year.IsValid) invalidList.Add(CancelDate_Year.InvalidMessage);

                        // Column CancelDate_Month
                        var CancelDate_Month = helpers.ValidateString("CancelDate_Month", rows[i, 12], true);

                        if (CancelDate_Month.IsValid) rowResult.CancelDate_Month = CancelDate_Month.Value;
                        if (!CancelDate_Month.IsValid) invalidList.Add(CancelDate_Month.InvalidMessage);

                        // Column CancelDate_Day
                        var CancelDate_Day = helpers.ValidateString("CancelDate_Day", rows[i, 13], true);

                        if (CancelDate_Day.IsValid) rowResult.CancelDate_Day = CancelDate_Day.Value;
                        if (!CancelDate_Day.IsValid) invalidList.Add(CancelDate_Day.InvalidMessage);

                        // Column PeriodTypeDetail
                        var PeriodTypeDetail = helpers.ValidateString("PeriodTypeDetail", rows[i, 14], true);

                        if (PeriodTypeDetail.IsValid) rowResult.PeriodTypeDetail = PeriodTypeDetail.Value;
                        if (!PeriodTypeDetail.IsValid) invalidList.Add(PeriodTypeDetail.InvalidMessage);

                        // Column Age_Cover
                        var Age_Cover = helpers.ValidateInt("Age_Cover", rows[i, 15], true);

                        if (Age_Cover.IsValid) rowResult.Age_Cover = Age_Cover.Value;
                        if (!Age_Cover.IsValid) invalidList.Add(Age_Cover.InvalidMessage);

                        // Column Payername
                        var Payername = helpers.ValidateString("Payername", rows[i, 16], false);

                        if (Payername.IsValid) rowResult.Payername = Payername.Value;
                        if (!Payername.IsValid) invalidList.Add(Payername.InvalidMessage);

                        // Column PayerBuildingName
                        var PayerBuildingName = helpers.ValidateString("PayerBuildingName", rows[i, 17], true);

                        if (PayerBuildingName.IsValid) rowResult.PayerBuildingName = PayerBuildingName.Value;
                        if (!PayerBuildingName.IsValid) invalidList.Add(PayerBuildingName.InvalidMessage);

                        // Column PayerTumbol
                        var PayerTumbol = helpers.ValidateString("PayerTumbol", rows[i, 18], true);

                        if (PayerTumbol.IsValid) rowResult.PayerTumbol = PayerTumbol.Value;
                        if (!PayerTumbol.IsValid) invalidList.Add(PayerTumbol.InvalidMessage);

                        // Column PayerAmphoe_id
                        var PayerAmphoe_id = helpers.ValidateString("PayerAmphoe_id", rows[i, 19], true);

                        if (PayerAmphoe_id.IsValid) rowResult.PayerAmphoe_id = PayerAmphoe_id.Value;
                        if (!PayerAmphoe_id.IsValid) invalidList.Add(PayerAmphoe_id.InvalidMessage);

                        // Column PayerAmphoe
                        var PayerAmphoe = helpers.ValidateString("PayerAmphoe", rows[i, 20], true);

                        if (PayerAmphoe.IsValid) rowResult.PayerAmphoe = PayerAmphoe.Value;
                        if (!PayerAmphoe.IsValid) invalidList.Add(PayerAmphoe.InvalidMessage);

                        // Column PayerStudyArea_id
                        var PayerStudyArea_id = helpers.ValidateString("PayerStudyArea_id", rows[i, 21], true);

                        if (PayerStudyArea_id.IsValid) rowResult.PayerStudyArea_id = PayerStudyArea_id.Value;
                        if (!PayerStudyArea_id.IsValid) invalidList.Add(PayerStudyArea_id.InvalidMessage);

                        // Column PayerStudyArea
                        var PayerStudyArea = helpers.ValidateString("PayerStudyArea", rows[i, 22], true);

                        if (PayerStudyArea.IsValid) rowResult.PayerStudyArea = PayerStudyArea.Value;
                        if (!PayerStudyArea.IsValid) invalidList.Add(PayerStudyArea.InvalidMessage);

                        // Column PayerProvince_id
                        var PayerProvince_id = helpers.ValidateString("PayerProvince_id", rows[i, 23], true);

                        if (PayerProvince_id.IsValid) rowResult.PayerProvince_id = PayerProvince_id.Value;
                        if (!PayerProvince_id.IsValid) invalidList.Add(PayerProvince_id.InvalidMessage);

                        // Column PayerProvince
                        var PayerProvince = helpers.ValidateString("PayerProvince", rows[i, 24], true);

                        if (PayerProvince.IsValid) rowResult.PayerProvince = PayerProvince.Value;
                        if (!PayerProvince.IsValid) invalidList.Add(PayerProvince.InvalidMessage);

                        // Column PayerBranchID
                        var PayerBranchID = helpers.ValidateString("PayerBranchID", rows[i, 25], true);

                        if (PayerBranchID.IsValid) rowResult.PayerBranchID = PayerBranchID.Value;
                        if (!PayerBranchID.IsValid) invalidList.Add(PayerBranchID.InvalidMessage);

                        // Column PayerBranch
                        var PayerBranch = helpers.ValidateString("PayerBranch", rows[i, 26], true);

                        if (PayerBranch.IsValid) rowResult.PayerBranch = PayerBranch.Value;
                        if (!PayerBranch.IsValid) invalidList.Add(PayerBranch.InvalidMessage);

                        // Column Sale_id1
                        var Sale_id1 = helpers.ValidateString("Sale_id1", rows[i, 27], true);

                        if (Sale_id1.IsValid) rowResult.Sale_id1 = Sale_id1.Value;
                        if (!Sale_id1.IsValid) invalidList.Add(Sale_id1.InvalidMessage);

                        // Column SaleName
                        var SaleName = helpers.ValidateString("SaleName", rows[i, 28], true);

                        if (SaleName.IsValid) rowResult.SaleName = SaleName.Value;
                        if (!SaleName.IsValid) invalidList.Add(SaleName.InvalidMessage);

                        // Column AppTeamID
                        var AppTeamID = helpers.ValidateString("AppTeamID", rows[i, 29], false);

                        if (AppTeamID.IsValid) rowResult.AppTeamID = AppTeamID.Value;
                        if (!AppTeamID.IsValid) invalidList.Add(AppTeamID.InvalidMessage);

                        // Column AppTeamName
                        var AppTeamName = helpers.ValidateString("AppTeamName", rows[i, 30], false);

                        if (AppTeamName.IsValid) rowResult.AppTeamName = AppTeamName.Value;
                        if (!AppTeamName.IsValid) invalidList.Add(AppTeamName.InvalidMessage);

                        // Column AppBranchID
                        var AppBranchID = helpers.ValidateString("AppBranchID", rows[i, 31], false);

                        if (AppBranchID.IsValid) rowResult.AppBranchID = AppBranchID.Value;
                        if (!AppBranchID.IsValid) invalidList.Add(AppBranchID.InvalidMessage);

                        // Column AppBranchName
                        var AppBranchName = helpers.ValidateString("AppBranchName", rows[i, 32], false);

                        if (AppBranchName.IsValid) rowResult.AppBranchName = AppBranchName.Value;
                        if (!AppBranchName.IsValid) invalidList.Add(AppBranchName.InvalidMessage);

                        // Column Sale_id2
                        var Sale_id2 = helpers.ValidateString("Sale_id2", rows[i, 33], false);

                        if (Sale_id2.IsValid) rowResult.Sale_id2 = Sale_id2.Value;
                        if (!Sale_id2.IsValid) invalidList.Add(Sale_id2.InvalidMessage);

                        // Column SaleTeam_id2
                        var SaleTeam_id2 = helpers.ValidateString("SaleTeam_id2", rows[i, 34], false);

                        if (SaleTeam_id2.IsValid) rowResult.SaleTeam_id2 = SaleTeam_id2.Value;
                        if (!SaleTeam_id2.IsValid) invalidList.Add(SaleTeam_id2.InvalidMessage);

                        // Column CreatedBy_id
                        var CreatedBy_id = helpers.ValidateString("CreatedBy_id", rows[i, 35], false);

                        if (CreatedBy_id.IsValid) rowResult.CreatedBy_id = CreatedBy_id.Value;
                        if (!CreatedBy_id.IsValid) invalidList.Add(CreatedBy_id.InvalidMessage);

                        // Column CreatedDate
                        var CreatedDate = helpers.ValidateDate("CreatedDate", rows[i, 36], true);

                        if (CreatedDate.IsValid) rowResult.CreatedDate = CreatedDate.Value;
                        if (!CreatedDate.IsValid) invalidList.Add(CreatedDate.InvalidMessage);

                        // Column ProductGroup_id
                        var ProductGroup_id = helpers.ValidateString("ProductGroup_id", rows[i, 37], false);

                        if (ProductGroup_id.IsValid) rowResult.ProductGroup_id = ProductGroup_id.Value;
                        if (!ProductGroup_id.IsValid) invalidList.Add(ProductGroup_id.InvalidMessage);

                        // Column ProductGroup
                        var ProductGroup = helpers.ValidateString("ProductGroup", rows[i, 38], false);

                        if (ProductGroup.IsValid) rowResult.ProductGroup = ProductGroup.Value;
                        if (!ProductGroup.IsValid) invalidList.Add(ProductGroup.InvalidMessage);

                        // Column CancelCause
                        var CancelCause = helpers.ValidateString("CancelCause", rows[i, 39], true);

                        if (CancelCause.IsValid) rowResult.CancelCause = CancelCause.Value;
                        if (!CancelCause.IsValid) invalidList.Add(CancelCause.InvalidMessage);

                        // Column PortfolioPeriod
                        var PortfolioPeriod = helpers.ValidateDate("PortfolioPeriod", rows[i, 40], false);

                        if (PortfolioPeriod.IsValid) rowResult.PortfolioPeriod = PortfolioPeriod.Value;
                        if (!PortfolioPeriod.IsValid) invalidList.Add(PortfolioPeriod.InvalidMessage);

                        // Column DateDiff_Portfolio_Cancel
                        var DateDiff_Portfolio_Cancel = helpers.ValidateInt("DateDiff_Portfolio_Cancel", rows[i, 41], false);

                        if (DateDiff_Portfolio_Cancel.IsValid) rowResult.DateDiff_Portfolio_Cancel = DateDiff_Portfolio_Cancel.Value;
                        if (!DateDiff_Portfolio_Cancel.IsValid) invalidList.Add(DateDiff_Portfolio_Cancel.InvalidMessage);

                        // Column Insured_Title
                        var Insured_Title = helpers.ValidateString("Insured_Title", rows[i, 42], true);

                        if (Insured_Title.IsValid) rowResult.Insured_Title = Insured_Title.Value;
                        if (!Insured_Title.IsValid) invalidList.Add(Insured_Title.InvalidMessage);

                        // Column Insured_First_Name
                        var Insured_First_Name = helpers.ValidateString("Insured_First_Name", rows[i, 43], true);

                        if (Insured_First_Name.IsValid) rowResult.Insured_First_Name = Insured_First_Name.Value;
                        if (!Insured_First_Name.IsValid) invalidList.Add(Insured_First_Name.InvalidMessage);

                        // Column Insured_Last_Name
                        var Insured_Last_Name = helpers.ValidateString("Insured_Last_Name", rows[i, 44], true);

                        if (Insured_Last_Name.IsValid) rowResult.Insured_Last_Name = Insured_Last_Name.Value;
                        if (!Insured_Last_Name.IsValid) invalidList.Add(Insured_Last_Name.InvalidMessage);

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
                                cfg.CreateMap<D_LapseMP, LapseMPInvalid>();
                            });

                            IMapper mapper = config.CreateMapper();
                            var dest = mapper.Map<D_LapseMP, LapseMPInvalid>(rowResult);
                            dest.InvalidList = invalidList;

                            // Add item to invalid list
                            invalid.Add((LapseMPInvalid)dest);
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
                var getDupplidateInFile = valid.GroupBy(x => x.App_id).Where(x => x.Count() > 1)
                    .Select(x => x.Key).ToList();

                // TODO: Find in valid
                foreach (var item in valid.Where(x => getDupplidateInFile.Any(y => y.Equals(x.App_id))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_LapseMP, LapseMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_LapseMP, LapseMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Dupplicate column Code in upload file" };

                    // Add item to invalid list
                    invalid.Add((LapseMPInvalid)dest);
                }

                #endregion Check Duplicate

                #region Validate App_id

                var notExistAppId = helpers.ValidateAppId(tmpValid.Where(x => !string.IsNullOrEmpty(x.App_id)).Select(x => x.App_id.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistAppId.Any(y => y.Equals(x.App_id.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found AppID");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistAppId.Any(y => y.Equals(x.App_id.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_LapseMP, LapseMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_LapseMP, LapseMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found AppID" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate App_id

                #region Validate EmployeeSS

                #region Sale_id1

                var notExistEmployeeSS = helpers.ValidateEmployeeSS(tmpValid.Where(x => !string.IsNullOrEmpty(x.Sale_id1)).Select(x => x.Sale_id1.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistEmployeeSS.Any(y => y.Equals(x.Sale_id1.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found Sale_id1");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistEmployeeSS.Any(y => y.Equals(x.Sale_id1.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_LapseMP, LapseMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_LapseMP, LapseMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found Sale_id1" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Sale_id1

                #region Sale_id2

                var notExistEmployeeSS_2 = helpers.ValidateEmployeeSS(tmpValid.Where(x => !string.IsNullOrEmpty(x.Sale_id2)).Select(x => x.Sale_id2.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistEmployeeSS_2.Any(y => y.Equals(x.Sale_id2.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found Sale_id2");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistEmployeeSS_2.Any(y => y.Equals(x.Sale_id2.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_LapseMP, LapseMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_LapseMP, LapseMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found Sale_id2" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Sale_id2

                #region CreateBy_id

                var notExistEmployeeSS_3 = helpers.ValidateEmployeeSS(tmpValid.Where(x => !string.IsNullOrEmpty(x.CreatedBy_id)).Select(x => x.CreatedBy_id.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistEmployeeSS_3.Any(y => y.Equals(x.CreatedBy_id.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found empcode");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistEmployeeSS_3.Any(y => y.Equals(x.CreatedBy_id.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_LapseMP, LapseMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_LapseMP, LapseMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found empcode" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion CreateBy_id

                #endregion Validate EmployeeSS

                #region Validate Team

                var notExistTeam = helpers.ValidateTeam(tmpValid.Where(x => !string.IsNullOrEmpty(x.AppTeamID)).Select(x => x.AppTeamID.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistTeam.Any(y => y.Equals(x.AppTeamID))).ToList())
                {
                    item.InvalidList.Add("Not found AppTeamID");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistTeam.Any(y => y.Equals(x.AppTeamID))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_LapseMP, LapseMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_LapseMP, LapseMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found AppTeamID" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Team

                #region Validate Branch

                #region PayerBranch_id

                var notExistBranch = helpers.ValidateBranch(tmpValid.Where(x => !string.IsNullOrEmpty(x.PayerBranchID)).Select(x => x.PayerBranchID.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistBranch.Any(y => y.Equals(x.PayerBranchID))).ToList())
                {
                    item.InvalidList.Add("Not found PayerBranchID");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistBranch.Any(y => y.Equals(x.PayerBranchID))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_LapseMP, LapseMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_LapseMP, LapseMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found PayerBranchID" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion PayerBranch_id

                #region AppBranchID

                var notExistBranch_2 = helpers.ValidateBranch(tmpValid.Where(x => !string.IsNullOrEmpty(x.AppBranchID)).Select(x => x.AppBranchID.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistBranch_2.Any(y => y.Equals(x.AppBranchID))).ToList())
                {
                    item.InvalidList.Add("Not found AppBranchID");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistBranch_2.Any(y => y.Equals(x.AppBranchID))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_LapseMP, LapseMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_LapseMP, LapseMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found AppBranchID" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion AppBranchID

                #endregion Validate Branch

                #region Validate Area

                var notExistArea = helpers.ValidateArea(tmpValid.Where(x => !string.IsNullOrEmpty(x.PayerStudyArea_id)).Select(x => x.PayerStudyArea_id.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistArea.Any(y => y.Equals(x.PayerStudyArea_id))).ToList())
                {
                    item.InvalidList.Add("Not found PayerStudyArea_id");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistArea.Any(y => y.Equals(x.PayerStudyArea_id.ToString()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_LapseMP, LapseMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_LapseMP, LapseMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found PayerStudyArea_id" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Area

                #region Validate Province

                var notExistBranch2 = helpers.ValidateProvince(tmpValid.Where(x => !string.IsNullOrEmpty(x.PayerProvince_id)).Select(x => x.PayerProvince_id.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistBranch2.Any(y => y.Equals(x.PayerProvince_id))).ToList())
                {
                    item.InvalidList.Add("Not found PayerProvince_id");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistBranch2.Any(y => y.Equals(x.PayerProvince_id))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_LapseMP, LapseMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_LapseMP, LapseMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found PayerProvince_id" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Province

                #region Validate District

                var notExistDistrict = helpers.ValidateDistrict(tmpValid.Where(x => !string.IsNullOrEmpty(x.PayerAmphoe_id)).Select(x => x.PayerAmphoe_id.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistDistrict.Any(y => y.Equals(x.PayerAmphoe_id.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found PayerAmphoe_id");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistDistrict.Any(y => y.Equals(x.PayerAmphoe_id.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_LapseMP, LapseMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_LapseMP, LapseMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found PayerAmphoe_id" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate District
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
            var latestPeriodList = _context.D_LapseMP.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_LapseMP.Remove(itm);
                    _context.SaveChanges();
                }
            }

            // Get data from session by datakey
            var valid = (List<D_LapseMP>)Session[dataKey];

            EFBatchOperation.For(_context, _context.D_LapseMP).InsertAll(valid);

            //call sp to update import log
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogManual_Update(periodResult.periodId, "D_LapseMP", user);

            Session[dataKey] = null;

            return Json("บันทึกข้อมูลเรียบร้อย");
        }

        [HttpPost]
        public JsonResult SaveNoData()
        {
            var periodResult = GlobalFunction.GetLatestPeriod();

            var latestPeriodList = _context.D_LapseMP.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_LapseMP.Remove(itm);
                    _context.SaveChanges();
                }
            }

            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogNoData_Update(periodResult.periodId, "D_LapseMP", user);

            return Json("บันทึกข้อมูลเรียบร้อย");
        }
    }
}