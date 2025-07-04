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
    public class ClaimMotorController : Controller
    {
        #region Declare Variables

        private readonly CommissionCalculateEntities _context;

        public ClaimMotorController()
        {
            // Set entity
            _context = new CommissionCalculateEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // TODO: Prepare invalid model and extend model for this import
        public class ClaimMotorInvalid : D_ClaimMotor
        {
            public List<string> InvalidList { get; set; }
        }

        #endregion Declare Variables

        // GET: ClaimMotor
        public ActionResult Index()
        {
            // TODO: Prepare valid and invalid model for initial table
            ViewBag.Valid = new List<D_ClaimMotor>();
            ViewBag.ValidJson = "[]";
            ViewBag.Invalid = new List<ClaimMotorInvalid>();
            ViewBag.InvalidJson = "[]";
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var helpers = new ValidationHelpers();

            // TODO: Prepare valid and invalid model
            var valid = new List<D_ClaimMotor>();
            var invalid = new List<ClaimMotorInvalid>();

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
                        "Status",
                        "CustName",
                        "App_id",
                        "Date_Happen",
                        "Total",
                        "Create_Date",
                        "Product",
                        "InsuranceCompany",
                        "license_car",
                        "Engine_no",
                        "Chassis_no",
                        "CoverDate",
                        "Premium",
                        "PayerName",
                        "PayerStudyArea_id",
                        "PayerStudyArea",
                        "PayerBranchID",
                        "PayerBranchName",
                        "ApprovedBy_id",
                        "ApprovedBy_Name",
                        "ApprovedDate",
                        "Garage_Name",
                        "ProductGroup_id",
                        "ProductGroup"
                    };

                    var missingColumn = helpers.ValidateColumn(rows, columns);

                    if (missingColumn.Count > 0)
                    {
                        ViewBag.isInvalidColumn = true;

                        ViewBag.isInvalidMsg = "ไม่พบ column : " + String.Join(", ", missingColumn);

                        ViewBag.Valid = new List<D_ClaimMotor>();
                        ViewBag.ValidJson = "[]";
                        ViewBag.Invalid = new List<ClaimMotorInvalid>();
                        ViewBag.InvalidJson = "[]";

                        return View("Index");
                    }

                    #endregion Validate Column

                    //loop all rows
                    for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row - 1; i++)
                    {
                        // TODO: Prepare valid and invalid model for each row
                        var rowResult = new D_ClaimMotor();
                        var invalidList = new List<string>();

                        #region Validate Column

                        // TODO: Check type and require column from document condition

                        // Column Code
                        var Code = helpers.ValidateInt("Code", rows[i, 0], true);

                        if (Code.IsValid) rowResult.Code = Code.Value;
                        if (!Code.IsValid) invalidList.Add(Code.InvalidMessage);

                        // Column Status
                        var Status = helpers.ValidateString("Status", rows[i, 1], true);

                        if (Status.IsValid) rowResult.Status = Status.Value;
                        if (!Status.IsValid) invalidList.Add(Status.InvalidMessage);

                        // Column CustName
                        var CustName = helpers.ValidateString("CustName", rows[i, 2], true);

                        if (CustName.IsValid) rowResult.CustName = CustName.Value;
                        if (!CustName.IsValid) invalidList.Add(CustName.InvalidMessage);

                        // Column App_id
                        var App_id = helpers.ValidateString("App_id", rows[i, 3], true);

                        if (App_id.IsValid) rowResult.App_id = App_id.Value;
                        if (!App_id.IsValid) invalidList.Add(App_id.InvalidMessage);

                        // Column Date_Happen
                        var Date_Happen = helpers.ValidateDate("Date_Happen", rows[i, 4], true);

                        if (Date_Happen.IsValid) rowResult.Date_Happen = Date_Happen.Value;
                        if (!Date_Happen.IsValid) invalidList.Add(Date_Happen.InvalidMessage);

                        // Column Total
                        var Total = helpers.ValidateInt("Total", rows[i, 5], true);

                        if (Total.IsValid) rowResult.Total = Total.Value;
                        if (!Total.IsValid) invalidList.Add(Total.InvalidMessage);

                        // Column Create_Date
                        var Create_Date = helpers.ValidateDate("Create_Date", rows[i, 6], true);

                        if (Create_Date.IsValid) rowResult.Create_Date = Create_Date.Value;
                        if (!Create_Date.IsValid) invalidList.Add(Create_Date.InvalidMessage);

                        // Column Product
                        var Product = helpers.ValidateString("Product", rows[i, 7], true);

                        if (Product.IsValid) rowResult.Product = Product.Value;
                        if (!Product.IsValid) invalidList.Add(Product.InvalidMessage);

                        // Column InsuranceCompany
                        var InsuranceCompany = helpers.ValidateString("InsuranceCompany", rows[i, 8], true);

                        if (InsuranceCompany.IsValid) rowResult.InsuranceCompany = InsuranceCompany.Value;
                        if (!InsuranceCompany.IsValid) invalidList.Add(InsuranceCompany.InvalidMessage);

                        // Column license_car
                        var license_car = helpers.ValidateString("license_car", rows[i, 9], true);

                        if (license_car.IsValid) rowResult.license_car = license_car.Value;
                        if (!license_car.IsValid) invalidList.Add(license_car.InvalidMessage);

                        // Column Engine_no
                        var Engine_no = helpers.ValidateString("Engine_no", rows[i, 10], true);

                        if (Engine_no.IsValid) rowResult.Engine_no = Engine_no.Value;
                        if (!Engine_no.IsValid) invalidList.Add(Engine_no.InvalidMessage);

                        // Column Chassis_no
                        var Chassis_no = helpers.ValidateString("Chassis_no", rows[i, 11], true);

                        if (Chassis_no.IsValid) rowResult.Chassis_no = Chassis_no.Value;
                        if (!Chassis_no.IsValid) invalidList.Add(Chassis_no.InvalidMessage);

                        // Column CoverDate
                        var CoverDate = helpers.ValidateDate("CoverDate", rows[i, 12], true);

                        if (CoverDate.IsValid) rowResult.CoverDate = CoverDate.Value;
                        if (!CoverDate.IsValid) invalidList.Add(CoverDate.InvalidMessage);

                        // Column Premium
                        var Premium = helpers.ValidateInt("Premium", rows[i, 13], true);

                        if (Premium.IsValid) rowResult.Premium = Premium.Value;
                        if (!Premium.IsValid) invalidList.Add(Premium.InvalidMessage);

                        // Column PayerName
                        var PayerName = helpers.ValidateString("PayerName", rows[i, 14], true);

                        if (PayerName.IsValid) rowResult.PayerName = PayerName.Value;
                        if (!PayerName.IsValid) invalidList.Add(PayerName.InvalidMessage);

                        // Column PayerStudyArea_id
                        var PayerStudyArea_id = helpers.ValidateString("PayerStudyArea_id", rows[i, 15], true);

                        if (PayerStudyArea_id.IsValid) rowResult.PayerStudyArea_id = PayerStudyArea_id.Value;
                        if (!PayerStudyArea_id.IsValid) invalidList.Add(PayerStudyArea_id.InvalidMessage);

                        // Column PayerStudyArea
                        var PayerStudyArea = helpers.ValidateString("PayerStudyArea", rows[i, 16], true);

                        if (PayerStudyArea.IsValid) rowResult.PayerStudyArea = PayerStudyArea.Value;
                        if (!PayerStudyArea.IsValid) invalidList.Add(PayerStudyArea.InvalidMessage);

                        // Column PayerBranchID
                        var PayerBranchID = helpers.ValidateString("PayerBranchID", rows[i, 17], true);

                        if (PayerBranchID.IsValid) rowResult.PayerBranchID = PayerBranchID.Value;
                        if (!PayerBranchID.IsValid) invalidList.Add(PayerBranchID.InvalidMessage);

                        // Column PayerBranchName
                        var PayerBranchName = helpers.ValidateString("PayerBranchName", rows[i, 18], true);

                        if (PayerBranchName.IsValid) rowResult.PayerBranchName = PayerBranchName.Value;
                        if (!PayerBranchName.IsValid) invalidList.Add(PayerBranchName.InvalidMessage);

                        // Column ApprovedBy_id
                        var ApprovedBy_id = helpers.ValidateString("ApprovedBy_id", rows[i, 19], true);

                        if (ApprovedBy_id.IsValid) rowResult.ApprovedBy_id = ApprovedBy_id.Value;
                        if (!ApprovedBy_id.IsValid) invalidList.Add(ApprovedBy_id.InvalidMessage);

                        // Column ApprovedBy_Name
                        var ApprovedBy_Name = helpers.ValidateString("ApprovedBy_Name", rows[i, 20], true);

                        if (ApprovedBy_Name.IsValid) rowResult.ApprovedBy_Name = ApprovedBy_Name.Value;
                        if (!ApprovedBy_Name.IsValid) invalidList.Add(ApprovedBy_Name.InvalidMessage);

                        // Column ApprovedDate
                        var ApprovedDate = helpers.ValidateDate("ApprovedDate", rows[i, 21], true);

                        if (ApprovedDate.IsValid) rowResult.ApprovedDate = ApprovedDate.Value;
                        if (!ApprovedDate.IsValid) invalidList.Add(ApprovedDate.InvalidMessage);

                        // Column Garage_Name
                        var Garage_Name = helpers.ValidateString("Garage_Name", rows[i, 22], false);

                        if (Garage_Name.IsValid) rowResult.Garage_Name = Garage_Name.Value;
                        if (!Garage_Name.IsValid) invalidList.Add(Garage_Name.InvalidMessage);

                        // Column ProductGroup_id
                        var ProductGroup_id = helpers.ValidateString("ProductGroup_id", rows[i, 23], true);

                        if (ProductGroup_id.IsValid) rowResult.ProductGroup_id = ProductGroup_id.Value;
                        if (!ProductGroup_id.IsValid) invalidList.Add(ProductGroup_id.InvalidMessage);

                        // Column ProductGroup
                        var ProductGroup = helpers.ValidateString("ProductGroup", rows[i, 24], true);

                        if (ProductGroup.IsValid) rowResult.ProductGroup = ProductGroup.Value;
                        if (!ProductGroup.IsValid) invalidList.Add(ProductGroup.InvalidMessage);

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
                                cfg.CreateMap<D_ClaimMotor, ClaimMotorInvalid>();
                            });

                            IMapper mapper = config.CreateMapper();
                            var dest = mapper.Map<D_ClaimMotor, ClaimMotorInvalid>(rowResult);
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
                var getDupplidateInFile = valid.GroupBy(x => x.Code).Where(x => x.Count() > 1)
                    .Select(x => x.Key).ToList();

                // TODO: Find in valid
                foreach (var item in valid.Where(x => getDupplidateInFile.Any(y => y.Equals(x.Code))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_ClaimMotor, ClaimMotorInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_ClaimMotor, ClaimMotorInvalid>(item);
                    dest.InvalidList = new List<string>() { "Dupplicate column Code in upload file" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                //// TODO: Check Duplicate in database

                //var existingDupplicate = _context.ClaimMotors.SqlQuery(
                //    string.Format("SELECT * FROM dbo.ClaimMotor WHERE Code IN ('{0}')",
                //        string.Join("','", tmpValid.Select(x => x.Code).ToArray()))).ToList();

                //// TODO: Find in exist invalid
                //foreach (var item in invalid.Where(x => existingDupplicate.Any(y => y.Code.Equals(x.Code))).ToList())
                //{
                //    item.InvalidList.Add("Dupplicate column Code in database");
                //}

                //// TODO: Find in valid
                //foreach (var item in valid.Where(x => existingDupplicate.Any(y => y.Code.Equals(x.Code))).ToList())
                //{
                //    // Remove item from valid list
                //    valid.Remove(item);

                //    // Map valid item to invalid
                //    var config = new MapperConfiguration(cfg =>
                //    {
                //        cfg.CreateMap<ClaimMotor, ClaimMotorInvalid>();
                //    });

                //    IMapper mapper = config.CreateMapper();
                //    var dest = mapper.Map<ClaimMotor, ClaimMotorInvalid>(item);
                //    dest.InvalidList = new List<string>() { "Dupplicate column Code in database" };

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
                        cfg.CreateMap<D_ClaimMotor, ClaimMotorInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_ClaimMotor, ClaimMotorInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found App_id" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate AppID

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
                        cfg.CreateMap<D_ClaimMotor, ClaimMotorInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_ClaimMotor, ClaimMotorInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found PayerStudyArea_id" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Area

                #region Validate Branch

                var notExistBranch = helpers.ValidateBranch(tmpValid.Where(x => !string.IsNullOrEmpty(x.PayerBranchID)).Select(x => x.PayerBranchID.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistBranch.Any(y => y.Equals(x.PayerBranchID.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found PayerBranchID");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistBranch.Any(y => y.Equals(x.PayerBranchID.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_ClaimMotor, ClaimMotorInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_ClaimMotor, ClaimMotorInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found PayerBranchID" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Branch

                #region Validate EmployeeSS

                var notExistEmployeeSS = helpers.ValidateEmployeeSS(tmpValid.Where(x => !string.IsNullOrEmpty(x.ApprovedBy_id)).Select(x => x.ApprovedBy_id.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistEmployeeSS.Any(y => y.Equals(x.ApprovedBy_id.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found ApprovedBy_id");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistEmployeeSS.Any(y => y.Equals(x.ApprovedBy_id.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_ClaimMotor, ClaimMotorInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_ClaimMotor, ClaimMotorInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found ApprovedBy_id" };

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
            var latestPeriodList = _context.D_ClaimMotor.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_ClaimMotor.Remove(itm);
                    _context.SaveChanges();
                }
            }

            // Get data from session by datakey
            var valid = (List<D_ClaimMotor>)Session[dataKey];

            EFBatchOperation.For(_context, _context.D_ClaimMotor).InsertAll(valid);

            //call sp to update import log
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogManual_Update(periodResult.periodId, "D_ClaimMotor", user);

            Session[dataKey] = null;

            return Json("บันทึกข้อมูลเรียบร้อย");
        }

        [HttpPost]
        public JsonResult SaveNoData()
        {
            var periodResult = GlobalFunction.GetLatestPeriod();

            var latestPeriodList = _context.D_ClaimMotor.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_ClaimMotor.Remove(itm);
                    _context.SaveChanges();
                }
            }

            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogNoData_Update(periodResult.periodId, "D_ClaimMotor", user);

            return Json("บันทึกข้อมูลเรียบร้อย");
        }
    }
}