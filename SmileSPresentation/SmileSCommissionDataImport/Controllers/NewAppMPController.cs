using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
    public class NewAppMPController : Controller
    {
        #region Declare Variables

        private readonly CommissionCalculateEntities _context;

        public NewAppMPController()
        {
            // Set entity
            _context = new CommissionCalculateEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // TODO: Prepare invalid model and extend model for this import
        public class NewAppMPInvalid : D_NewAppMP
        {
            public List<string> InvalidList { get; set; }
        }

        #endregion Declare Variables

        // GET: NewAppMP
        public ActionResult Index()
        {
            // TODO: Prepare valid and invalid model for initial table
            ViewBag.Valid = new List<D_NewAppMP>();
            ViewBag.ValidJson = "[]";
            ViewBag.Invalid = new List<NewAppMPInvalid>();
            ViewBag.InvalidJson = "[]";

            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var helpers = new ValidationHelpers();

            // TODO: Prepare valid and invalid model
            var valid = new List<D_NewAppMP>();
            var invalid = new List<NewAppMPInvalid>();

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
                        "AppID"
                         ,"Status"
                         ,"ContractTypeDetail"
                         ,"StartCover"
                         ,"StartCover_D"
                         ,"StartCover_M"
                         ,"StartCover_Y"
                         ,"EndCover"
                         ,"EndCover_D"
                         ,"EndCover_M"
                         ,"EndCover_Y"
                         ,"ProductType_ID"
                         ,"ProductTypeDetail"
                         ,"Product_ID"
                         ,"ProductDetail"
                         ,"PremiumPerMonth"
                         ,"PremiumPerYear"
                         ,"PeriodType_ID"
                         ,"PeriodTypeDetail"
                         ,"CustName"
                         ,"PayerName"
                         ,"PayerDistrictID"
                         ,"PayerDistrict"
                         ,"PayerProvinceID"
                         ,"PayerProvince"
                         ,"PayerBranch_ID"
                         ,"PayerBranchDetail"
                         ,"PayerStudyArea_Id"
                         ,"PayerStudyArea"
                         ,"Sale1_Employee_Code"
                         ,"SaleName"
                         ,"Sale1_Branch_ID"
                         ,"Sale1_BranchDetail"
                         ,"ZebraCar_Code"
                         ,"ZebraCar_No"
                         ,"ZebraCarOwner_Id"
                         ,"VehicleRegistrationDetail"
                         ,"VehicleRegistrationNumber"
                         ,"VehicleRegistrationProvince"
                         ,"CreateBy_Employee_Code"
                         ,"ApproveBy_Employee_Code"
                         ,"SaleID2"
                         ,"CreatedDate"
                         ,"CreatedDate_D"
                         ,"CreatedDate_M"
                         ,"CreatedDate_Y"
                         ,"ApproveDate"
                         ,"ApproveDate_D"
                         ,"ApproveDate_M"
                         ,"ApproveDate_Y"
                         ,"SaleID_Service"
                         ,"SaleName_Service"
                         ,"SaleID_NewAppShare"
                         ,"SaleName_NewAppShare"
                         ,"Policy_Issued_Date"
                         ,"Insured_Title"
                         ,"Insured_First_Name"
                         ,"Insured_Last_Name"
                         ,"Payer_Title"
                         ,"Payer_First_Name"
                         ,"Payer_Last_Name"
                         ,"Premium"
                    };

                    var missingColumn = helpers.ValidateColumn(rows, columns);

                    if (missingColumn.Count > 0)
                    {
                        ViewBag.isInvalidColumn = true;

                        ViewBag.isInvalidMsg = "ไม่พบ column : " + String.Join(", ", missingColumn);

                        ViewBag.Valid = new List<D_NewAppMP>();
                        ViewBag.ValidJson = "[]";
                        ViewBag.Invalid = new List<NewAppMPInvalid>();
                        ViewBag.InvalidJson = "[]";

                        return View("Index");
                    }

                    #endregion Validate Column

                    //loop all rows
                    for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row - 1; i++)
                    {
                        // TODO: Prepare valid and invalid model for each row
                        var rowResult = new D_NewAppMP();
                        var invalidList = new List<string>();

                        #region Validate Column

                        // TODO: Check type and require column from document condition

                        // Column AppID
                        var AppID = helpers.ValidateString("AppID", rows[i, 0], true);

                        if (AppID.IsValid) rowResult.AppID = AppID.Value;
                        if (!AppID.IsValid) invalidList.Add(AppID.InvalidMessage);

                        // Column Status
                        var Status = helpers.ValidateString("Status", rows[i, 1], true);

                        if (Status.IsValid) rowResult.Status = Status.Value;
                        if (!Status.IsValid) invalidList.Add(Status.InvalidMessage);

                        // Column ContractTypeDetail
                        var ContractTypeDetail = helpers.ValidateString("ContractTypeDetail", rows[i, 2], true);

                        if (ContractTypeDetail.IsValid) rowResult.ContractTypeDetail = ContractTypeDetail.Value;
                        if (!ContractTypeDetail.IsValid) invalidList.Add(ContractTypeDetail.InvalidMessage);

                        // Column StartCover
                        var StartCover = helpers.ValidateDate("StartCover", rows[i, 3], true);

                        if (StartCover.IsValid) rowResult.StartCover = StartCover.Value;
                        if (!StartCover.IsValid) invalidList.Add(StartCover.InvalidMessage);

                        // Column StartCover_D
                        var StartCover_D = helpers.ValidateInt("StartCover_D", rows[i, 4], false);

                        if (StartCover_D.IsValid) rowResult.StartCover_D = StartCover_D.Value;
                        if (!StartCover_D.IsValid) invalidList.Add(StartCover_D.InvalidMessage);

                        // Column StartCover_M
                        var StartCover_M = helpers.ValidateInt("StartCover_M", rows[i, 5], false);

                        if (StartCover_M.IsValid) rowResult.StartCover_M = StartCover_M.Value;
                        if (!StartCover_M.IsValid) invalidList.Add(StartCover_M.InvalidMessage);

                        // Column StartCover_Y
                        var StartCover_Y = helpers.ValidateInt("StartCover_Y", rows[i, 6], true);

                        if (StartCover_Y.IsValid) rowResult.Premium = StartCover_Y.Value;
                        if (!StartCover_Y.IsValid) invalidList.Add(StartCover_Y.InvalidMessage);

                        // Column EndCover
                        var EndCover = helpers.ValidateDate("EndCover", rows[i, 7], true);

                        if (EndCover.IsValid) rowResult.EndCover = EndCover.Value;
                        if (!EndCover.IsValid) invalidList.Add(EndCover.InvalidMessage);

                        // Column EndCover_D
                        var EndCover_D = helpers.ValidateInt("EndCover_D", rows[i, 8], true);

                        if (EndCover_D.IsValid) rowResult.EndCover_D = EndCover_D.Value;
                        if (!EndCover_D.IsValid) invalidList.Add(EndCover_D.InvalidMessage);

                        // Column EndCover_M
                        var EndCover_M = helpers.ValidateInt("EndCover_M", rows[i, 9], true);

                        if (EndCover_M.IsValid) rowResult.EndCover_M = EndCover_M.Value;
                        if (!EndCover_M.IsValid) invalidList.Add(EndCover_M.InvalidMessage);

                        // Column EndCover_Y
                        var EndCover_Y = helpers.ValidateInt("EndCover_Y", rows[i, 10], true);

                        if (EndCover_Y.IsValid) rowResult.EndCover_Y = EndCover_Y.Value;
                        if (!EndCover_Y.IsValid) invalidList.Add(EndCover_Y.InvalidMessage);

                        // Column ProductType_ID
                        var ProductType_ID = helpers.ValidateString("ProductType_ID", rows[i, 11], false);

                        if (ProductType_ID.IsValid) rowResult.ProductType_ID = ProductType_ID.Value;
                        if (!ProductType_ID.IsValid) invalidList.Add(ProductType_ID.InvalidMessage);

                        // Column ProductTypeDetail
                        var ProductTypeDetail = helpers.ValidateString("ProductTypeDetail", rows[i, 12], true);

                        if (ProductTypeDetail.IsValid) rowResult.ProductTypeDetail = ProductTypeDetail.Value;
                        if (!ProductTypeDetail.IsValid) invalidList.Add(ProductTypeDetail.InvalidMessage);

                        // Column Product_ID
                        var Product_ID = helpers.ValidateString("Product_ID", rows[i, 13], false);

                        if (Product_ID.IsValid) rowResult.Product_ID = Product_ID.Value;
                        if (!Product_ID.IsValid) invalidList.Add(Product_ID.InvalidMessage);

                        // Column ProductDetail
                        var ProductDetail = helpers.ValidateString("ProductDetail", rows[i, 14], true);

                        if (ProductDetail.IsValid) rowResult.ProductDetail = ProductDetail.Value;
                        if (!ProductDetail.IsValid) invalidList.Add(ProductDetail.InvalidMessage);

                        // Column PremiumPerMonth
                        var PremiumPerMonth = helpers.ValidateInt("PremiumPerMonth", rows[i, 15], false);

                        if (PremiumPerMonth.IsValid) rowResult.PremiumPerMonth = PremiumPerMonth.Value;
                        if (!PremiumPerMonth.IsValid) invalidList.Add(PremiumPerMonth.InvalidMessage);

                        // Column PremiumPerYear
                        var PremiumPerYear = helpers.ValidateInt("PremiumPerYear", rows[i, 16], false);

                        if (PremiumPerYear.IsValid) rowResult.PremiumPerYear = PremiumPerYear.Value;
                        if (!PremiumPerYear.IsValid) invalidList.Add(PremiumPerYear.InvalidMessage);

                        // Column PeriodType_ID
                        var PeriodType_ID = helpers.ValidateString("PeriodType_ID", rows[i, 17], false);

                        if (PeriodType_ID.IsValid) rowResult.PeriodType_ID = PeriodType_ID.Value;
                        if (!PeriodType_ID.IsValid) invalidList.Add(PeriodType_ID.InvalidMessage);

                        // Column PeriodTypeDetail
                        var PeriodTypeDetail = helpers.ValidateString("PeriodTypeDetail", rows[i, 18], true);

                        if (PeriodTypeDetail.IsValid) rowResult.PeriodTypeDetail = PeriodTypeDetail.Value;
                        if (!PeriodTypeDetail.IsValid) invalidList.Add(PeriodTypeDetail.InvalidMessage);

                        // Column CustName
                        var CustName = helpers.ValidateString("CustName", rows[i, 19], false);

                        if (CustName.IsValid) rowResult.CustName = CustName.Value;
                        if (!CustName.IsValid) invalidList.Add(CustName.InvalidMessage);

                        // Column PayerName
                        var PayerName = helpers.ValidateString("PayerName", rows[i, 20], false);

                        if (PayerName.IsValid) rowResult.PayerName = PayerName.Value;
                        if (!PayerName.IsValid) invalidList.Add(PayerName.InvalidMessage);

                        // Column PayerDistrictID
                        var PayerDistrictID = helpers.ValidateString("PayerDistrictID", rows[i, 21], true);

                        if (PayerDistrictID.IsValid) rowResult.PayerDistrictID = PayerDistrictID.Value;
                        if (!PayerDistrictID.IsValid) invalidList.Add(PayerDistrictID.InvalidMessage);

                        // Column PayerDistrict
                        var PayerDistrict = helpers.ValidateString("PayerDistrict", rows[i, 22], true);

                        if (PayerDistrict.IsValid) rowResult.PayerDistrict = PayerDistrict.Value;
                        if (!PayerDistrict.IsValid) invalidList.Add(PayerDistrict.InvalidMessage);

                        // Column PayerProvinceID
                        var PayerProvinceID = helpers.ValidateString("PayerProvinceID", rows[i, 23], true);

                        if (PayerProvinceID.IsValid) rowResult.PayerProvinceID = PayerProvinceID.Value;
                        if (!PayerProvinceID.IsValid) invalidList.Add(PayerProvinceID.InvalidMessage);

                        // Column PayerProvince
                        var PayerProvince = helpers.ValidateString("PayerProvince", rows[i, 24], true);

                        if (PayerProvince.IsValid) rowResult.PayerProvince = PayerProvince.Value;
                        if (!PayerProvince.IsValid) invalidList.Add(PayerProvince.InvalidMessage);

                        // Column PayerBranch_ID
                        var PayerBranch_ID = helpers.ValidateString("PayerBranch_ID", rows[i, 25], true);

                        if (PayerBranch_ID.IsValid) rowResult.PayerBranch_ID = PayerBranch_ID.Value;
                        if (!PayerBranch_ID.IsValid) invalidList.Add(PayerBranch_ID.InvalidMessage);

                        // Column PayerBranchDetail
                        var PayerBranchDetail = helpers.ValidateString("PayerBranchDetail", rows[i, 26], true);

                        if (PayerBranchDetail.IsValid) rowResult.PayerBranchDetail = PayerBranchDetail.Value;
                        if (!PayerBranchDetail.IsValid) invalidList.Add(PayerBranchDetail.InvalidMessage);

                        // Column PayerStudyArea_Id
                        var PayerStudyArea_Id = helpers.ValidateString("PayerStudyArea_Id", rows[i, 27], true);

                        if (PayerStudyArea_Id.IsValid) rowResult.PayerStudyArea_Id = PayerStudyArea_Id.Value;
                        if (!PayerStudyArea_Id.IsValid) invalidList.Add(PayerStudyArea_Id.InvalidMessage);

                        // Column PayerStudyArea
                        var PayerStudyArea = helpers.ValidateString("PayerStudyArea", rows[i, 28], true);

                        if (PayerStudyArea.IsValid) rowResult.PayerStudyArea = PayerStudyArea.Value;
                        if (!PayerStudyArea.IsValid) invalidList.Add(PayerStudyArea.InvalidMessage);

                        // Column Sale1_Employee_Code
                        var Sale1_Employee_Code = helpers.ValidateString("Sale1_Employee_Code", rows[i, 29], true);

                        if (Sale1_Employee_Code.IsValid) rowResult.Sale1_Employee_Code = Sale1_Employee_Code.Value;
                        if (!Sale1_Employee_Code.IsValid) invalidList.Add(Sale1_Employee_Code.InvalidMessage);

                        // Column SaleName
                        var SaleName = helpers.ValidateString("SaleName", rows[i, 30], true);

                        if (SaleName.IsValid) rowResult.SaleName = SaleName.Value;
                        if (!SaleName.IsValid) invalidList.Add(SaleName.InvalidMessage);

                        // Column Sale1_Branch_ID
                        var Sale1_Branch_ID = helpers.ValidateString("Sale1_Branch_ID", rows[i, 31], false);

                        if (Sale1_Branch_ID.IsValid) rowResult.Sale1_Branch_ID = Sale1_Branch_ID.Value;
                        if (!Sale1_Branch_ID.IsValid) invalidList.Add(Sale1_Branch_ID.InvalidMessage);

                        // Column Sale1_BranchDetail
                        var Sale1_BranchDetail = helpers.ValidateString("Sale1_BranchDetail", rows[i, 32], true);

                        if (Sale1_BranchDetail.IsValid) rowResult.Sale1_BranchDetail = Sale1_BranchDetail.Value;
                        if (!Sale1_BranchDetail.IsValid) invalidList.Add(Sale1_BranchDetail.InvalidMessage);

                        // Column ZebraCar_Code
                        var ZebraCar_Code = helpers.ValidateString("ZebraCar_Code", rows[i, 33], false);

                        if (ZebraCar_Code.IsValid) rowResult.ZebraCar_Code = ZebraCar_Code.Value;
                        if (!ZebraCar_Code.IsValid) invalidList.Add(ZebraCar_Code.InvalidMessage);

                        // Column ZebraCar_No
                        var ZebraCar_No = helpers.ValidateString("ZebraCar_No", rows[i, 34], false);

                        if (ZebraCar_No.IsValid) rowResult.ZebraCar_No = ZebraCar_No.Value;
                        if (!ZebraCar_No.IsValid) invalidList.Add(ZebraCar_No.InvalidMessage);

                        // Column ZebraCarOwner_Id
                        var ZebraCarOwner_Id = helpers.ValidateString("ZebraCarOwner_Id", rows[i, 35], false);

                        if (ZebraCarOwner_Id.IsValid) rowResult.ZebraCarOwner_Id = ZebraCarOwner_Id.Value;
                        if (!ZebraCarOwner_Id.IsValid) invalidList.Add(ZebraCarOwner_Id.InvalidMessage);

                        // Column VehicleRegistrationDetail
                        var VehicleRegistrationDetail = helpers.ValidateString("VehicleRegistrationDetail", rows[i, 36], true);

                        if (VehicleRegistrationDetail.IsValid) rowResult.VehicleRegistrationDetail = VehicleRegistrationDetail.Value;
                        if (!VehicleRegistrationDetail.IsValid) invalidList.Add(VehicleRegistrationDetail.InvalidMessage);

                        // Column VehicleRegistrationNumber
                        var VehicleRegistrationNumber = helpers.ValidateString("VehicleRegistrationNumber", rows[i, 37], false);

                        if (VehicleRegistrationNumber.IsValid) rowResult.VehicleRegistrationNumber = VehicleRegistrationNumber.Value;
                        if (!VehicleRegistrationNumber.IsValid) invalidList.Add(VehicleRegistrationNumber.InvalidMessage);

                        // Column VehicleRegistrationProvince
                        var VehicleRegistrationProvince = helpers.ValidateString("VehicleRegistrationProvince", rows[i, 38], true);

                        if (VehicleRegistrationProvince.IsValid) rowResult.VehicleRegistrationProvince = VehicleRegistrationProvince.Value;
                        if (!VehicleRegistrationProvince.IsValid) invalidList.Add(VehicleRegistrationProvince.InvalidMessage);

                        // Column CreateBy_Employee_Code
                        var CreateBy_Employee_Code = helpers.ValidateString("CreateBy_Employee_Code", rows[i, 39], false);

                        if (CreateBy_Employee_Code.IsValid) rowResult.CreateBy_Employee_Code = CreateBy_Employee_Code.Value;
                        if (!CreateBy_Employee_Code.IsValid) invalidList.Add(CreateBy_Employee_Code.InvalidMessage);

                        // Column ApproveBy_Employee_Code
                        var ApproveBy_Employee_Code = helpers.ValidateString("ApproveBy_Employee_Code", rows[i, 40], false);

                        if (ApproveBy_Employee_Code.IsValid) rowResult.ApproveBy_Employee_Code = ApproveBy_Employee_Code.Value;
                        if (!ApproveBy_Employee_Code.IsValid) invalidList.Add(ApproveBy_Employee_Code.InvalidMessage);

                        // Column SaleID2
                        var SaleID2 = helpers.ValidateString("SaleID2", rows[i, 41], false);

                        if (SaleID2.IsValid) rowResult.SaleID2 = SaleID2.Value;
                        if (!SaleID2.IsValid) invalidList.Add(SaleID2.InvalidMessage);

                        // Column CreatedDate
                        var CreatedDate = helpers.ValidateDate("CreatedDate", rows[i, 42], true);

                        if (CreatedDate.IsValid) rowResult.CreatedDate = CreatedDate.Value;
                        if (!CreatedDate.IsValid) invalidList.Add(CreatedDate.InvalidMessage);

                        // Column CreatedDate_D
                        var CreatedDate_D = helpers.ValidateInt("CreatedDate_D", rows[i, 43], true);

                        if (CreatedDate_D.IsValid) rowResult.CreatedDate_D = CreatedDate_D.Value;
                        if (!CreatedDate_D.IsValid) invalidList.Add(CreatedDate_D.InvalidMessage);

                        // Column CreatedDate_M
                        var CreatedDate_M = helpers.ValidateInt("CreatedDate_M", rows[i, 44], true);

                        if (CreatedDate_M.IsValid) rowResult.CreatedDate_M = CreatedDate_M.Value;
                        if (!CreatedDate_M.IsValid) invalidList.Add(CreatedDate_M.InvalidMessage);

                        // Column CreatedDate_Y
                        var CreatedDate_Y = helpers.ValidateInt("CreatedDate_Y", rows[i, 45], true);

                        if (CreatedDate_Y.IsValid) rowResult.CreatedDate_Y = CreatedDate_Y.Value;
                        if (!CreatedDate_Y.IsValid) invalidList.Add(CreatedDate_Y.InvalidMessage);

                        // Column ApproveDate
                        var ApproveDate = helpers.ValidateDate("ApproveDate", rows[i, 46], false);

                        if (ApproveDate.IsValid) rowResult.ApproveDate = ApproveDate.Value;
                        if (!ApproveDate.IsValid) invalidList.Add(ApproveDate.InvalidMessage);

                        // Column ApproveDate_D
                        var ApproveDate_D = helpers.ValidateInt("ApproveDate_D", rows[i, 47], true);

                        if (ApproveDate_D.IsValid) rowResult.ApproveDate_D = ApproveDate_D.Value;
                        if (!ApproveDate_D.IsValid) invalidList.Add(ApproveDate_D.InvalidMessage);

                        // Column ApproveDate_M
                        var ApproveDate_M = helpers.ValidateInt("ApproveDate_M", rows[i, 48], true);

                        if (ApproveDate_M.IsValid) rowResult.ApproveDate_M = ApproveDate_M.Value;
                        if (!ApproveDate_M.IsValid) invalidList.Add(ApproveDate_M.InvalidMessage);

                        // Column ApproveDate_Y
                        var ApproveDate_Y = helpers.ValidateInt("ApproveDate_Y", rows[i, 49], true);

                        if (ApproveDate_Y.IsValid) rowResult.ApproveDate_Y = ApproveDate_Y.Value;
                        if (!ApproveDate_Y.IsValid) invalidList.Add(ApproveDate_Y.InvalidMessage);

                        // Column SaleID_Service
                        var SaleID_Service = helpers.ValidateString("SaleID_Service", rows[i, 50], false);

                        if (SaleID_Service.IsValid) rowResult.SaleID_Service = SaleID_Service.Value;
                        if (!SaleID_Service.IsValid) invalidList.Add(SaleID_Service.InvalidMessage);

                        // Column SaleName_Service
                        var SaleName_Service = helpers.ValidateString("SaleName_Service", rows[i, 51], false);

                        if (SaleName_Service.IsValid) rowResult.SaleName_Service = SaleName_Service.Value;
                        if (!SaleName_Service.IsValid) invalidList.Add(SaleName_Service.InvalidMessage);

                        // Column SaleID_NewAppShare
                        var SaleID_NewAppShare = helpers.ValidateString("SaleID_NewAppShare", rows[i, 52], false);

                        if (SaleID_NewAppShare.IsValid) rowResult.SaleID_NewAppShare = SaleID_NewAppShare.Value;
                        if (!SaleID_NewAppShare.IsValid) invalidList.Add(SaleID_NewAppShare.InvalidMessage);

                        // Column SaleName_NewAppShare
                        var SaleName_NewAppShare = helpers.ValidateString("SaleName_NewAppShare", rows[i, 53], false);

                        if (SaleName_NewAppShare.IsValid) rowResult.SaleName_NewAppShare = SaleName_NewAppShare.Value;
                        if (!SaleName_NewAppShare.IsValid) invalidList.Add(SaleName_NewAppShare.InvalidMessage);

                        // Column Policy_Issued_Date
                        var Policy_Issued_Date = helpers.ValidateDate("Policy_Issued_Date", rows[i, 54], true);

                        if (Policy_Issued_Date.IsValid) rowResult.Policy_Issued_Date = Policy_Issued_Date.Value;
                        if (!Policy_Issued_Date.IsValid) invalidList.Add(Policy_Issued_Date.InvalidMessage);

                        // Column Insured_Title
                        var Insured_Title = helpers.ValidateString("Insured_Title", rows[i, 55], true);

                        if (Insured_Title.IsValid) rowResult.Insured_Title = Insured_Title.Value;
                        if (!Insured_Title.IsValid) invalidList.Add(Insured_Title.InvalidMessage);

                        // Column Insured_First_Name
                        var Insured_First_Name = helpers.ValidateString("Insured_First_Name", rows[i, 56], true);

                        if (Insured_First_Name.IsValid) rowResult.Insured_First_Name = Insured_First_Name.Value;
                        if (!Insured_First_Name.IsValid) invalidList.Add(Insured_First_Name.InvalidMessage);

                        // Column Insured_Last_Name
                        var Insured_Last_Name = helpers.ValidateString("Insured_Last_Name", rows[i, 57], true);

                        if (Insured_Last_Name.IsValid) rowResult.Insured_Last_Name = Insured_Last_Name.Value;
                        if (!Insured_Last_Name.IsValid) invalidList.Add(Insured_Last_Name.InvalidMessage);

                        // Column Payer_Title
                        var Payer_Title = helpers.ValidateString("Payer_Title", rows[i, 58], true);

                        if (Payer_Title.IsValid) rowResult.Payer_Title = Payer_Title.Value;
                        if (!Payer_Title.IsValid) invalidList.Add(Payer_Title.InvalidMessage);

                        // Column Payer_First_Name
                        var Payer_First_Name = helpers.ValidateString("Payer_First_Name", rows[i, 59], true);

                        if (Payer_First_Name.IsValid) rowResult.Payer_First_Name = Payer_First_Name.Value;
                        if (!Payer_First_Name.IsValid) invalidList.Add(Payer_First_Name.InvalidMessage);

                        // Column Payer_Last_Name
                        var Payer_Last_Name = helpers.ValidateString("Payer_Last_Name", rows[i, 60], true);

                        if (Payer_Last_Name.IsValid) rowResult.Payer_Last_Name = Payer_Last_Name.Value;
                        if (!Payer_Last_Name.IsValid) invalidList.Add(Payer_Last_Name.InvalidMessage);

                        // Column Premium
                        var Premium = helpers.ValidateInt("Premium", rows[i, 61], true);

                        if (Premium.IsValid) rowResult.Premium = Premium.Value;
                        if (!Premium.IsValid) invalidList.Add(Premium.InvalidMessage);

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
                                cfg.CreateMap<D_NewAppMP, NewAppMPInvalid>();
                            });

                            IMapper mapper = config.CreateMapper();
                            var dest = mapper.Map<D_NewAppMP, NewAppMPInvalid>(rowResult);
                            dest.InvalidList = invalidList;

                            // Add item to invalid list
                            invalid.Add(dest);
                        }
                    }
                }
            }

            if (invalid.Count == 0)
            {
                var tmpValid = valid.ToList();

                #region Check Duplicate

                // TODO: Check Duplicate in File
                var getDupplidateInFile = valid.GroupBy(x => x.AppID.Trim()).Where(x => x.Count() > 1)
                    .Select(x => x.Key).ToList();

                // TODO: Find in valid
                foreach (var item in valid.Where(x => getDupplidateInFile.Any(y => y.Equals(x.AppID.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_NewAppMP, NewAppMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewAppMP, NewAppMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Dupplicate column AppID in upload file" };

                    // Add item to invalid list
                    invalid.Add((NewAppMPInvalid)dest);
                }

                #endregion Check Duplicate

                #region Validate AppId

                var notExistAppId = helpers.ValidateAppId(tmpValid.Where(x => !string.IsNullOrEmpty(x.AppID)).Select(x => x.AppID.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistAppId.Any(y => y.Equals(x.AppID.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found AppID");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistAppId.Any(y => y.Equals(x.AppID.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_NewAppMP, NewAppMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewAppMP, NewAppMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found AppID" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate AppId

                #region Validate EmployeeSS

                #region Sale1_Employee_Code

                var notExistEmployeeSS = helpers.ValidateEmployeeSS(tmpValid.Where(x => !string.IsNullOrEmpty(x.Sale1_Employee_Code)).Select(x => x.Sale1_Employee_Code.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistEmployeeSS.Any(y => y.Equals(x.Sale1_Employee_Code.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found EmpCode");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistEmployeeSS.Any(y => y.Equals(x.Sale1_Employee_Code.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_NewAppMP, NewAppMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewAppMP, NewAppMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found EmpCode" };

                    // Add item to invalid list
                    invalid.Add((NewAppMPInvalid)dest);
                }

                #endregion Sale1_Employee_Code

                #region ZebraCarOwner_Id

                var notExistEmployeeSS_ZebraCarOwner_Id = helpers.ValidateEmployeeSS(tmpValid.Where(x => !string.IsNullOrEmpty(x.ZebraCarOwner_Id)).Select(x => x.ZebraCarOwner_Id.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistEmployeeSS_ZebraCarOwner_Id.Any(y => y.Equals(x.ZebraCarOwner_Id.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found EmpCode");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistEmployeeSS_ZebraCarOwner_Id.Any(y => y.Equals(x.ZebraCarOwner_Id.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_NewAppMP, NewAppMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewAppMP, NewAppMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found EmpCode" };

                    // Add item to invalid list
                    invalid.Add((NewAppMPInvalid)dest);
                }

                #endregion ZebraCarOwner_Id

                #region CreateBy_Employee_Code

                var notExistEmployeeSS_CreateBy_Employee_Code = helpers.ValidateEmployeeSS(tmpValid.Where(x => !string.IsNullOrEmpty(x.CreateBy_Employee_Code)).Select(x => x.CreateBy_Employee_Code.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistEmployeeSS_CreateBy_Employee_Code.Any(y => y.Equals(x.CreateBy_Employee_Code.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found EmpCode");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistEmployeeSS_CreateBy_Employee_Code.Any(y => y.Equals(x.CreateBy_Employee_Code.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_NewAppMP, NewAppMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewAppMP, NewAppMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found EmpCode" };

                    // Add item to invalid list
                    invalid.Add((NewAppMPInvalid)dest);
                }

                #endregion CreateBy_Employee_Code

                #region ApproveBy_Employee_Code

                var notExistEmployeeSS_ApproveBy_Employee_Code = helpers.ValidateEmployeeSS(tmpValid.Where(x => !string.IsNullOrEmpty(x.ApproveBy_Employee_Code)).Select(x => x.ApproveBy_Employee_Code.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistEmployeeSS_ApproveBy_Employee_Code.Any(y => y.Equals(x.ApproveBy_Employee_Code.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found EmpCode");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistEmployeeSS_ApproveBy_Employee_Code.Any(y => y.Equals(x.ApproveBy_Employee_Code.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_NewAppMP, NewAppMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewAppMP, NewAppMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found EmpCode" };

                    // Add item to invalid list
                    invalid.Add((NewAppMPInvalid)dest);
                }

                #endregion ApproveBy_Employee_Code

                #region SaleID2

                var notExistEmployeeSS_SaleID2 = helpers.ValidateEmployeeSS(tmpValid.Where(x => !string.IsNullOrEmpty(x.SaleID2)).Select(x => x.SaleID2.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistEmployeeSS_SaleID2.Any(y => y.Equals(x.SaleID2.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found EmpCode");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistEmployeeSS_SaleID2.Any(y => y.Equals(x.SaleID2.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_NewAppMP, NewAppMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewAppMP, NewAppMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found EmpCode" };

                    // Add item to invalid list
                    invalid.Add((NewAppMPInvalid)dest);
                }

                #endregion SaleID2

                #region SaleID_Service

                var notExistEmployeeSS_SaleID_Service = helpers.ValidateEmployeeSS(tmpValid.Where(x => !string.IsNullOrEmpty(x.SaleID_Service)).Select(x => x.SaleID_Service.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistEmployeeSS_SaleID_Service.Any(y => y.Equals(x.SaleID_Service.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found EmpCode");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistEmployeeSS_SaleID_Service.Any(y => y.Equals(x.SaleID_Service.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_NewAppMP, NewAppMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewAppMP, NewAppMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found EmpCode" };

                    // Add item to invalid list
                    invalid.Add((NewAppMPInvalid)dest);
                }

                #endregion SaleID_Service

                #region SaleID_NewAppShare

                var notExistEmployeeSS_SaleName_NewAppShare = helpers.ValidateEmployeeSS(tmpValid.Where(x => !string.IsNullOrEmpty(x.SaleName_NewAppShare)).Select(x => x.SaleName_NewAppShare.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistEmployeeSS_SaleName_NewAppShare.Any(y => y.Equals(x.SaleName_NewAppShare.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found EmpCode");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistEmployeeSS_SaleName_NewAppShare.Any(y => y.Equals(x.SaleName_NewAppShare.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_NewAppMP, NewAppMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewAppMP, NewAppMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found EmpCode" };

                    // Add item to invalid list
                    invalid.Add((NewAppMPInvalid)dest);
                }

                #endregion SaleID_NewAppShare

                #endregion Validate EmployeeSS

                #region Validate District

                var notExistDistrict = helpers.ValidateDistrict(tmpValid.Where(x => !string.IsNullOrEmpty(x.PayerDistrictID)).Select(x => x.PayerDistrictID.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistDistrict.Any(y => y.Equals(x.PayerDistrictID.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found PayerDistrictID");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistDistrict.Any(y => y.Equals(x.PayerDistrictID.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_NewAppMP, NewAppMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewAppMP, NewAppMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found DistrictID" };

                    // Add item to invalid list
                    invalid.Add((NewAppMPInvalid)dest);
                }

                #endregion Validate District

                #region Validate Province

                var notExistProvince = helpers.ValidateProvince(tmpValid.Where(x => !string.IsNullOrEmpty(x.PayerProvinceID)).Select(x => x.PayerProvinceID.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistProvince.Any(y => y.Equals(x.PayerProvinceID.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found PayerProvinceID");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistProvince.Any(y => y.Equals(x.PayerProvinceID.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_NewAppMP, NewAppMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewAppMP, NewAppMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found PayerProvinceID" };

                    // Add item to invalid list
                    invalid.Add((NewAppMPInvalid)dest);
                }

                #endregion Validate Province

                #region Validate Branch

                #region PayerBranch_ID

                var notExistBranch = helpers.ValidateBranch(tmpValid.Where(x => !string.IsNullOrEmpty(x.PayerBranch_ID)).Select(x => x.PayerBranch_ID.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistBranch.Any(y => y.Equals(x.PayerBranch_ID.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found PayerBranch_ID");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistBranch.Any(y => y.Equals(x.PayerBranch_ID.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_NewAppMP, NewAppMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewAppMP, NewAppMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found PayerBranch_ID" };

                    // Add item to invalid list
                    invalid.Add((NewAppMPInvalid)dest);
                }

                #endregion PayerBranch_ID

                #region Sale1_Branch_ID

                var notExistBranch_Sale1_Branch_ID = helpers.ValidateBranch(tmpValid.Where(x => !string.IsNullOrEmpty(x.Sale1_Branch_ID)).Select(x => x.Sale1_Branch_ID.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistBranch_Sale1_Branch_ID.Any(y => y.Equals(x.Sale1_Branch_ID.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found Sale1_Branch_ID");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistBranch_Sale1_Branch_ID.Any(y => y.Equals(x.Sale1_Branch_ID.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_NewAppMP, NewAppMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewAppMP, NewAppMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found Sale1_Branch_ID" };

                    // Add item to invalid list
                    invalid.Add((NewAppMPInvalid)dest);
                }

                #endregion Sale1_Branch_ID

                #endregion Validate Branch

                #region Validate Area

                var notExistArea = helpers.ValidateArea(tmpValid.Where(x => !string.IsNullOrEmpty(x.PayerStudyArea_Id)).Select(x => x.PayerStudyArea_Id.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistArea.Any(y => y.Equals(x.PayerStudyArea_Id.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found PayerStudyArea_Id");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistArea.Any(y => y.Equals(x.PayerStudyArea_Id.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_NewAppMP, NewAppMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewAppMP, NewAppMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found PayerStudyArea_Id" };

                    // Add item to invalid list
                    invalid.Add((NewAppMPInvalid)dest);
                }

                #endregion Validate Area

                #region Validate Zebra

                var notExistZebraCar = helpers.ValidateZebra(tmpValid.Where(x => !string.IsNullOrEmpty(x.ZebraCar_Code)).Select(x => x.ZebraCar_Code.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistZebraCar.Any(y => y.Equals(x.ZebraCar_Code.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found ZebraCar_Code");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistZebraCar.Any(y => y.Equals(x.ZebraCar_Code.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_NewAppMP, NewAppMPInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewAppMP, NewAppMPInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found ZebraCar_Code" };

                    // Add item to invalid list
                    invalid.Add((NewAppMPInvalid)dest);
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
            var latestPeriodList = _context.D_NewAppMP.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_NewAppMP.Remove(itm);
                    _context.SaveChanges();
                }
            }

            // Get data from session by datakey
            var valid = (List<D_NewAppMP>)Session[dataKey];

            EFBatchOperation.For(_context, _context.D_NewAppMP).InsertAll(valid);

            //call sp to update import log
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogManual_Update(periodResult.periodId, "D_NewAppMP", user);

            Session[dataKey] = null;

            return Json("บันทึกข้อมูลเรียบร้อย");
        }

        [HttpPost]
        public JsonResult SaveNoData()
        {
            var periodResult = GlobalFunction.GetLatestPeriod();

            var latestPeriodList = _context.D_NewAppMP.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_NewAppMP.Remove(itm);
                    _context.SaveChanges();
                }
            }

            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogNoData_Update(periodResult.periodId, "D_NewAppMP", user);

            return Json("บันทึกข้อมูลเรียบร้อย");
        }
    }
}