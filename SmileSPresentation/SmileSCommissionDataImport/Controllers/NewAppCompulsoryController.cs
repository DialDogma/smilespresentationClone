using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using EntityFramework.Utilities;
using OfficeOpenXml;

using Authorization = SmileSCommissionDataImport.Helper.Authorization;
using SmileSCommissionDataImport.Helper;
using SmileSCommissionDataImport.Models;
using SmileSCommissionDataImport.Utils;

namespace SmileSCommissionDataImport.Controllers
{
    [Authorization]
    public class NewAppCompulsoryController : Controller
    {
        #region Declare Variables

        private readonly CommissionCalculateEntities _context;

        public NewAppCompulsoryController()
        {
            // Set entity
            _context = new CommissionCalculateEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // Prepare invalid model and extend model for this import
        public class NewAppCompulsoryInvalid : D_NewAppCompulsory
        {
            public List<string> InvalidList { get; set; }
        }

        #endregion Declare Variables

        // GET: NewAppCompulsory
        public ActionResult Index()
        {
            // Prepare valid and invalid model for initial table
            ViewBag.Valid = new List<D_NewAppCompulsory>();
            ViewBag.ValidJson = "[]";
            ViewBag.Invalid = new List<NewAppCompulsoryInvalid>();
            ViewBag.InvalidJson = "[]";

            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var helpers = new ValidationHelpers();

            // TODO: Prepare valid and invalid model
            var valid = new List<D_NewAppCompulsory>();
            var invalid = new List<NewAppCompulsoryInvalid>();

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
                        "CoverDate",
                        "EmpCode",
                        "Sale_Name",
                        "Team_Id",
                        "Team_Name",
                        "Branch_Id",
                        "Branch_Name",
                        "NetPremium",
                        "DeductCommission",
                        "Commission",
                        "Product",
                        "CustomerFullName",
                        "PayerFullName",
                        "District",
                        "PayerAreaId",
                        "PayerAreaName",
                        "PayerBranchID",
                        "PayerBranchName",
                        "LicensePlate",
                        "Insurance_Company",
                        "DistrictID",
                        "DistrictName",
                    };

                    var missingColumn = helpers.ValidateColumn(rows, columns);

                    if (missingColumn.Count > 0)
                    {
                        ViewBag.isInvalidColumn = true;

                        ViewBag.isInvalidMsg = "ไม่พบ column : " + String.Join(", ", missingColumn);

                        ViewBag.Valid = new List<D_NewAppCompulsory>();
                        ViewBag.ValidJson = "[]";
                        ViewBag.Invalid = new List<NewAppCompulsoryInvalid>();
                        ViewBag.InvalidJson = "[]";

                        return View("Index");
                    }

                    #endregion Validate Column

                    //loop all rows
                    for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row - 1; i++)
                    {
                        // TODO: Prepare valid and invalid model for each row
                        var rowResult = new D_NewAppCompulsory();
                        var invalidList = new List<string>();

                        #region Validate Column

                        // TODO: Check type and require column from document condition

                        // Column Code
                        var Code = helpers.ValidateInt("Code", rows[i, 0], true);

                        if (Code.IsValid) rowResult.Code = Code.Value;
                        if (!Code.IsValid) invalidList.Add(Code.InvalidMessage);

                        // Column CoverDate
                        var CoverDate = helpers.ValidateDate("CoverDate", rows[i, 1], true);

                        if (CoverDate.IsValid) rowResult.CoverDate = CoverDate.Value;
                        if (!CoverDate.IsValid) invalidList.Add(CoverDate.InvalidMessage);

                        // Column EmpCode
                        var EmpCode = helpers.ValidateString("EmpCode", rows[i, 2], true);

                        if (EmpCode.IsValid) rowResult.EmpCode = EmpCode.Value;
                        if (!EmpCode.IsValid) invalidList.Add(EmpCode.InvalidMessage);

                        // Column Sale_Name
                        var Sale_Name = helpers.ValidateString("Sale_Name", rows[i, 3], true);

                        if (Sale_Name.IsValid) rowResult.Sale_Name = Sale_Name.Value;
                        if (!Sale_Name.IsValid) invalidList.Add(Sale_Name.InvalidMessage);

                        // Column Team_Id
                        var Team_Id = helpers.ValidateString("Team_Id", rows[i, 4], true);

                        if (Team_Id.IsValid) rowResult.Team_Id = Team_Id.Value;
                        if (!Team_Id.IsValid) invalidList.Add(Team_Id.InvalidMessage);

                        // Column Team_Name
                        var Team_Name = helpers.ValidateString("Team_Name", rows[i, 5], true);

                        if (Team_Name.IsValid) rowResult.Team_Name = Team_Name.Value;
                        if (!Team_Name.IsValid) invalidList.Add(Team_Name.InvalidMessage);

                        // Column Branch_Id
                        var Branch_Id = helpers.ValidateString("Branch_Id", rows[i, 6], true);

                        if (Branch_Id.IsValid) rowResult.Branch_Id = Branch_Id.Value;
                        if (!Branch_Id.IsValid) invalidList.Add(Branch_Id.InvalidMessage);

                        // Column Branch_Name
                        var Branch_Name = helpers.ValidateString("Branch_Name", rows[i, 7], true);

                        if (Branch_Name.IsValid) rowResult.Branch_Name = Branch_Name.Value;
                        if (!Branch_Name.IsValid) invalidList.Add(Branch_Name.InvalidMessage);

                        // Column NetPremium
                        var NetPremium = helpers.ValidateFloat("NetPremium", rows[i, 8], true);

                        if (NetPremium.IsValid) rowResult.NetPremium = NetPremium.Value;
                        if (!NetPremium.IsValid) invalidList.Add(NetPremium.InvalidMessage);

                        // Column DeductCommission
                        var DeductCommission = helpers.ValidateFloat("DeductCommission", rows[i, 9], true);

                        if (DeductCommission.IsValid) rowResult.DeductCommission = DeductCommission.Value;
                        if (!DeductCommission.IsValid) invalidList.Add(DeductCommission.InvalidMessage);

                        // Column Commission
                        var Commission = helpers.ValidateFloat("Commission", rows[i, 10], true);

                        if (Commission.IsValid) rowResult.Commission = Commission.Value;
                        if (!Commission.IsValid) invalidList.Add(Commission.InvalidMessage);

                        // Column Product
                        var Product = helpers.ValidateString("Product", rows[i, 11], true);

                        if (Product.IsValid) rowResult.Product = Product.Value;
                        if (!Product.IsValid) invalidList.Add(Product.InvalidMessage);

                        // Column CustomerFullName
                        var CustomerFullName = helpers.ValidateString("CustomerFullName", rows[i, 12], true);

                        if (CustomerFullName.IsValid) rowResult.CustomerFullName = CustomerFullName.Value;
                        if (!CustomerFullName.IsValid) invalidList.Add(CustomerFullName.InvalidMessage);

                        // Column PayerFullName
                        var PayerFullName = helpers.ValidateString("PayerFullName", rows[i, 13], true);

                        if (PayerFullName.IsValid) rowResult.PayerFullName = PayerFullName.Value;
                        if (!PayerFullName.IsValid) invalidList.Add(PayerFullName.InvalidMessage);

                        // Column District
                        var District = helpers.ValidateString("District", rows[i, 14], true);

                        if (District.IsValid) rowResult.District = District.Value;
                        if (!District.IsValid) invalidList.Add(District.InvalidMessage);

                        // Column PayerAreaId
                        var PayerAreaId = helpers.ValidateString("PayerAreaId", rows[i, 15], true);

                        if (PayerAreaId.IsValid) rowResult.PayerAreaId = PayerAreaId.Value;
                        if (!PayerAreaId.IsValid) invalidList.Add(PayerAreaId.InvalidMessage);

                        // Column PayerAreaName
                        var PayerAreaName = helpers.ValidateString("PayerAreaName", rows[i, 16], true);

                        if (PayerAreaName.IsValid) rowResult.PayerAreaName = PayerAreaName.Value;
                        if (!PayerAreaName.IsValid) invalidList.Add(PayerAreaName.InvalidMessage);

                        // Column PayerBranchID
                        var PayerBranchID = helpers.ValidateString("PayerBranchID", rows[i, 17], true);

                        if (PayerBranchID.IsValid) rowResult.PayerBranchId = PayerBranchID.Value;
                        if (!PayerBranchID.IsValid) invalidList.Add(PayerBranchID.InvalidMessage);

                        // Column PayerBranchName
                        var PayerBranchName = helpers.ValidateString("PayerBranchName", rows[i, 18], true);

                        if (PayerBranchName.IsValid) rowResult.PayerBranchName = PayerBranchName.Value;
                        if (!PayerBranchName.IsValid) invalidList.Add(PayerBranchName.InvalidMessage);

                        // Column LicensePlate
                        var LicensePlate = helpers.ValidateString("LicensePlate", rows[i, 19], true);

                        if (LicensePlate.IsValid) rowResult.LicensePlate = LicensePlate.Value;
                        if (!LicensePlate.IsValid) invalidList.Add(LicensePlate.InvalidMessage);

                        // Column Insurance_Company
                        var Insurance_Company = helpers.ValidateString("Insurance_Company", rows[i, 20], true);

                        if (Insurance_Company.IsValid) rowResult.Insurance_Company = Insurance_Company.Value;
                        if (!Insurance_Company.IsValid) invalidList.Add(Insurance_Company.InvalidMessage);

                        // Column DistrictID
                        var DistrictID = helpers.ValidateString("DistrictID", rows[i, 21], true);

                        if (DistrictID.IsValid) rowResult.DistrictId = DistrictID.Value;
                        if (!DistrictID.IsValid) invalidList.Add(DistrictID.InvalidMessage);

                        // Column DistrictID
                        var DistrictName = helpers.ValidateString("DistrictName", rows[i, 22], true);

                        if (DistrictName.IsValid) rowResult.DistrictName = DistrictName.Value;
                        if (!DistrictName.IsValid) invalidList.Add(DistrictName.InvalidMessage);

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
                                cfg.CreateMap<D_NewAppCompulsory, NewAppCompulsoryInvalid>();
                            });

                            IMapper mapper = config.CreateMapper();
                            var dest = mapper.Map<D_NewAppCompulsory, NewAppCompulsoryInvalid>(rowResult);
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
                        cfg.CreateMap<D_NewAppCompulsory, NewAppCompulsoryInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewAppCompulsory, NewAppCompulsoryInvalid>(item);
                    dest.InvalidList = new List<string>() { "Dupplicate column MainKey in upload file" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Check Duplicate

                #region Validate EmployeeSS

                var notExistEmployeeSS2 = helpers.ValidateEmployeeSS(tmpValid.Where(x => !string.IsNullOrEmpty(x.EmpCode)).Select(x => x.EmpCode.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistEmployeeSS2.Any(y => y.Equals(x.EmpCode))).ToList())
                {
                    item.InvalidList.Add("Not found MSNID");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistEmployeeSS2.Any(y => y.Equals(x.EmpCode))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_NewAppCompulsory, NewAppCompulsoryInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewAppCompulsory, NewAppCompulsoryInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found MSNID" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate EmployeeSS

                #region Validate Team

                var notExistTeam = helpers.ValidateTeam(tmpValid.Where(x => !string.IsNullOrEmpty(x.Team_Id)).Select(x => x.Team_Id.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistTeam.Any(y => y.Equals(x.Team_Id))).ToList())
                {
                    item.InvalidList.Add("Not found TeamCode");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistTeam.Any(y => y.Equals(x.Team_Id))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_NewAppCompulsory, NewAppCompulsoryInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewAppCompulsory, NewAppCompulsoryInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found TeamCode" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Team

                #region Validate Branch

                var notExistBranch = helpers.ValidateBranch(tmpValid.Where(x => !string.IsNullOrEmpty(x.Branch_Id)).Select(x => x.Branch_Id.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistBranch.Any(y => y.Equals(x.Branch_Id.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found BranchCode");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistBranch.Any(y => y.Equals(x.Branch_Id.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_NewAppCompulsory, NewAppCompulsoryInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewAppCompulsory, NewAppCompulsoryInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found BranchCode" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Branch

                #region Validate Area

                var notExistArea = helpers.ValidateArea(tmpValid.Where(x => !string.IsNullOrEmpty(x.PayerAreaId)).Select(x => x.PayerAreaId.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistArea.Any(y => y.Equals(x.PayerAreaId))).ToList())
                {
                    item.InvalidList.Add("Not found AreaCode");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistArea.Any(y => y.Equals(x.PayerAreaId))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_NewAppCompulsory, NewAppCompulsoryInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewAppCompulsory, NewAppCompulsoryInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found AreaCode" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Area

                #region Validate Payer Branch

                var notExistBranch2 = helpers.ValidateBranch(tmpValid.Where(x => !string.IsNullOrEmpty(x.PayerBranchId)).Select(x => x.PayerBranchId.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistBranch2.Any(y => y.Equals(x.PayerBranchId.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found BranchCode");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistBranch2.Any(y => y.Equals(x.PayerBranchId.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_NewAppCompulsory, NewAppCompulsoryInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewAppCompulsory, NewAppCompulsoryInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found Payer BranchCode" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Payer Branch

                #region Validate District

                var notExistDistrict = helpers.ValidateDistrict(tmpValid.Where(x => !string.IsNullOrEmpty(x.DistrictId)).Select(x => x.DistrictId.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistDistrict.Any(y => y.Equals(x.DistrictId.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found DistrictID");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistDistrict.Any(y => y.Equals(x.DistrictId.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_NewAppCompulsory, NewAppCompulsoryInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_NewAppCompulsory, NewAppCompulsoryInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found DistrictID" };

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
            var latestPeriodList = _context.D_NewAppCompulsory.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_NewAppCompulsory.Remove(itm);
                    _context.SaveChanges();
                }
            }

            // Get data from session by datakey
            var valid = (List<D_NewAppCompulsory>)Session[dataKey];

            EFBatchOperation.For(_context, _context.D_NewAppCompulsory).InsertAll(valid);

            //call sp to update import log
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogManual_Update(periodResult.periodId, "D_NewAppCompulsory", user);

            Session[dataKey] = null;

            return Json("บันทึกข้อมูลเรียบร้อย");
        }

        [HttpPost]
        public JsonResult SaveNoData()
        {
            var periodResult = GlobalFunction.GetLatestPeriod();

            var latestPeriodList = _context.D_NewAppCompulsory.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_NewAppCompulsory.Remove(itm);
                    _context.SaveChanges();
                }
            }

            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogNoData_Update(periodResult.periodId, "D_NewAppCompulsory", user);

            return Json("บันทึกข้อมูลเรียบร้อย");
        }
    }
}