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
    public class UnderwriteConditionController : Controller
    {
        #region Declare Variables

        private readonly CommissionCalculateEntities _context;

        public UnderwriteConditionController()
        {
            // Set entity
            _context = new CommissionCalculateEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // TODO: Prepare invalid model and extend model for this import
        public class UnderwriteConditionInvalid : D_UnderwriteCondition
        {
            public List<string> InvalidList { get; set; }
        }

        #endregion Declare Variables

        // GET: UnderwriteCondition
        public ActionResult Index()
        {
            // TODO: Prepare valid and invalid model for initial table
            ViewBag.Valid = new List<D_UnderwriteCondition>();
            ViewBag.ValidJson = "[]";
            ViewBag.Invalid = new List<UnderwriteConditionInvalid>();
            ViewBag.InvalidJson = "[]";
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var helpers = new ValidationHelpers();

            // TODO: Prepare valid and invalid model
            var valid = new List<D_UnderwriteCondition>();
            var invalid = new List<UnderwriteConditionInvalid>();

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
                        "App_Id",
                        "CustomerFullName",
                        "PayerFullName",
                        "Office",
                        "PayerStudyArea_id",
                        "PayerStudyArea",
                        "PayerProvince",
                        "PayerBranchID",
                        "PayerBranch",
                        "StatusUnderwrite",
                        "Sale_id1",
                        "SaleName",
                        "TeamId",
                        "TeamName",
                        "BranchId",
                        "BranchName",
                        "DateOfReceipt",
                        "ApproveDate",
                        "Description",
                        "District_ID",
                        "District_Name",
                        "PayerProvince_ID",
                        "PayerProvince_Name",
                        "ZebraSaleID",
                        "ZebraSaleNumber"
                    };

                    var missingColumn = helpers.ValidateColumn(rows, columns);

                    if (missingColumn.Count > 0)
                    {
                        ViewBag.isInvalidColumn = true;

                        ViewBag.isInvalidMsg = "ไม่พบ column : " + String.Join(", ", missingColumn);

                        ViewBag.Valid = new List<D_UnderwriteCondition>();
                        ViewBag.ValidJson = "[]";
                        ViewBag.Invalid = new List<UnderwriteConditionInvalid>();
                        ViewBag.InvalidJson = "[]";

                        return View("Index");
                    }

                    #endregion Validate Column

                    //loop all rows
                    for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row - 1; i++)
                    {
                        // TODO: Prepare valid and invalid model for each row
                        var rowResult = new D_UnderwriteCondition();
                        var invalidList = new List<string>();

                        #region Validate Column

                        // TODO: Check type and require column from document condition

                        //App_Id
                        var App_Id = helpers.ValidateString("App_Id", rows[i, 0], true);

                        if (App_Id.IsValid) rowResult.App_Id = App_Id.Value;
                        if (!App_Id.IsValid) invalidList.Add(App_Id.InvalidMessage);

                        //CustomerFullName
                        var CustomerFullName = helpers.ValidateString("CustomerFullName", rows[i, 1], true);

                        if (CustomerFullName.IsValid) rowResult.CustomerFullName = CustomerFullName.Value;
                        if (!CustomerFullName.IsValid) invalidList.Add(CustomerFullName.InvalidMessage);

                        //PayerFullName
                        var PayerFullName = helpers.ValidateString("PayerFullName", rows[i, 2], true);

                        if (PayerFullName.IsValid) rowResult.PayerFullName = PayerFullName.Value;
                        if (!PayerFullName.IsValid) invalidList.Add(PayerFullName.InvalidMessage);

                        //Office
                        var Office = helpers.ValidateString("Office", rows[i, 3], true);

                        if (Office.IsValid) rowResult.Office = Office.Value;
                        if (!Office.IsValid) invalidList.Add(Office.InvalidMessage);

                        //PayerStudyArea_id
                        var PayerStudyArea_id = helpers.ValidateString("PayerStudyArea_id", rows[i, 4], true);

                        if (PayerStudyArea_id.IsValid) rowResult.PayerStudyArea_id = PayerStudyArea_id.Value;
                        if (!PayerStudyArea_id.IsValid) invalidList.Add(PayerStudyArea_id.InvalidMessage);

                        //PayerStudyArea
                        var PayerStudyArea = helpers.ValidateString("PayerStudyArea", rows[i, 5], true);

                        if (PayerStudyArea.IsValid) rowResult.PayerStudyArea = PayerStudyArea.Value;
                        if (!PayerStudyArea.IsValid) invalidList.Add(PayerStudyArea.InvalidMessage);

                        //PayerProvince
                        var PayerProvince = helpers.ValidateString("PayerProvince", rows[i, 6], true);

                        if (PayerProvince.IsValid) rowResult.PayerProvince = PayerProvince.Value;
                        if (!PayerProvince.IsValid) invalidList.Add(PayerProvince.InvalidMessage);

                        //PayerBranchID
                        var PayerBranchID = helpers.ValidateString("PayerBranchID", rows[i, 7], true);

                        if (PayerBranchID.IsValid) rowResult.PayerBranchID = PayerBranchID.Value;
                        if (!PayerBranchID.IsValid) invalidList.Add(PayerBranchID.InvalidMessage);

                        //PayerBranch
                        var PayerBranch = helpers.ValidateString("PayerBranch", rows[i, 8], true);

                        if (PayerBranch.IsValid) rowResult.PayerBranch = PayerBranch.Value;
                        if (!PayerBranch.IsValid) invalidList.Add(PayerBranch.InvalidMessage);

                        //StatusUnderwrite
                        var StatusUnderwrite = helpers.ValidateString("StatusUnderwrite", rows[i, 9], true);

                        if (StatusUnderwrite.IsValid) rowResult.StatusUnderwrite = StatusUnderwrite.Value;
                        if (!StatusUnderwrite.IsValid) invalidList.Add(StatusUnderwrite.InvalidMessage);

                        //Sale_id1
                        var Sale_id1 = helpers.ValidateString("Sale_id1", rows[i, 10], true);

                        if (Sale_id1.IsValid) rowResult.Sale_id1 = Sale_id1.Value;
                        if (!Sale_id1.IsValid) invalidList.Add(Sale_id1.InvalidMessage);

                        //SaleName
                        var SaleName = helpers.ValidateString("SaleName", rows[i, 11], true);

                        if (SaleName.IsValid) rowResult.SaleName = SaleName.Value;
                        if (!SaleName.IsValid) invalidList.Add(SaleName.InvalidMessage);

                        //TeamId
                        var TeamId = helpers.ValidateString("TeamId", rows[i, 12], true);

                        if (TeamId.IsValid) rowResult.TeamId = TeamId.Value;
                        if (!TeamId.IsValid) invalidList.Add(TeamId.InvalidMessage);

                        //TeamName
                        var TeamName = helpers.ValidateString("TeamName", rows[i, 13], true);

                        if (TeamName.IsValid) rowResult.TeamName = TeamName.Value;
                        if (!TeamName.IsValid) invalidList.Add(TeamName.InvalidMessage);

                        //BranchId
                        var BranchId = helpers.ValidateString("BranchId", rows[i, 14], true);

                        if (BranchId.IsValid) rowResult.BranchId = BranchId.Value;
                        if (!BranchId.IsValid) invalidList.Add(BranchId.InvalidMessage);

                        //BranchName
                        var BranchName = helpers.ValidateString("BranchName", rows[i, 15], true);

                        if (BranchName.IsValid) rowResult.BranchName = BranchName.Value;
                        if (!BranchName.IsValid) invalidList.Add(BranchName.InvalidMessage);

                        //DateOfReceipt
                        var DateOfReceipt = helpers.ValidateDate("DateOfReceipt", rows[i, 16], true);

                        if (DateOfReceipt.IsValid) rowResult.DateOfReceipt = DateOfReceipt.Value;
                        if (!DateOfReceipt.IsValid) invalidList.Add(DateOfReceipt.InvalidMessage);

                        //ApproveDate
                        var ApproveDate = helpers.ValidateDate("ApproveDate", rows[i, 17], true);

                        if (ApproveDate.IsValid) rowResult.ApproveDate = ApproveDate.Value;
                        if (!ApproveDate.IsValid) invalidList.Add(ApproveDate.InvalidMessage);

                        //Description
                        var Description = helpers.ValidateString("Description", rows[i, 18], false);

                        if (Description.IsValid) rowResult.Description = Description.Value;
                        if (!Description.IsValid) invalidList.Add(Description.InvalidMessage);

                        //District_ID
                        var District_ID = helpers.ValidateString("District_ID", rows[i, 19], false);

                        if (District_ID.IsValid) rowResult.District_ID = District_ID.Value;
                        if (!District_ID.IsValid) invalidList.Add(District_ID.InvalidMessage);

                        //District_Name
                        var District_Name = helpers.ValidateString("District_Name", rows[i, 20], false);

                        if (District_Name.IsValid) rowResult.District_Name = District_Name.Value;
                        if (!District_Name.IsValid) invalidList.Add(District_Name.InvalidMessage);

                        //PayerProvince_ID
                        var PayerProvince_ID = helpers.ValidateString("PayerProvince_ID", rows[i, 21], false);

                        if (PayerProvince_ID.IsValid) rowResult.PayerProvince_ID = PayerProvince_ID.Value;
                        if (!PayerProvince_ID.IsValid) invalidList.Add(PayerProvince_ID.InvalidMessage);

                        //PayerProvince_Name
                        var PayerProvince_Name = helpers.ValidateString("PayerProvince_Name", rows[i, 22], false);

                        if (PayerProvince_Name.IsValid) rowResult.PayerProvince_Name = PayerProvince_Name.Value;
                        if (!PayerProvince_Name.IsValid) invalidList.Add(PayerProvince_Name.InvalidMessage);

                        //ZebraSaleID
                        var ZebraSaleID = helpers.ValidateString("ZebraSaleID", rows[i, 23], false);

                        if (ZebraSaleID.IsValid) rowResult.ZebraSaleID = ZebraSaleID.Value;
                        if (!ZebraSaleID.IsValid) invalidList.Add(ZebraSaleID.InvalidMessage);

                        //ZebraSaleNumber
                        var ZebraSaleNumber = helpers.ValidateString("ZebraSaleNumber", rows[i, 24], false);

                        if (ZebraSaleNumber.IsValid) rowResult.ZebraSaleNumber = ZebraSaleNumber.Value;
                        if (!ZebraSaleNumber.IsValid) invalidList.Add(ZebraSaleNumber.InvalidMessage);

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
                                cfg.CreateMap<D_UnderwriteCondition, UnderwriteConditionInvalid>();
                            });

                            IMapper mapper = config.CreateMapper();
                            var dest = mapper.Map<D_UnderwriteCondition, UnderwriteConditionInvalid>(rowResult);
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
                var getDupplidateInFile = valid.GroupBy(x => x.App_Id.Trim()).Where(x => x.Count() > 1)
                    .Select(x => x.Key).ToList();

                // TODO: Find in valid
                foreach (var item in valid.Where(x => getDupplidateInFile.Any(y => y.Equals(x.App_Id.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_UnderwriteCondition, UnderwriteConditionInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_UnderwriteCondition, UnderwriteConditionInvalid>(item);
                    dest.InvalidList = new List<string>() { "Dupplicate column App_Id in upload file" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                //// TODO: Check Duplicate in database

                //var existingDupplicate = _context.UnderwriteConditions.SqlQuery(
                //    string.Format("SELECT * FROM dbo.UnderwriteCondition WHERE App_Id IN ('{0}')",
                //        string.Join("','", tmpValid.Select(x => x.App_Id).ToArray()))).ToList();

                //// TODO: Find in exist invalid
                //foreach (var item in invalid.Where(x => existingDupplicate.Any(y => y.App_Id.Trim().Equals(x.App_Id.Trim()))).ToList())
                //{
                //    item.InvalidList.Add("Dupplicate column App_Id in database");
                //}

                //// TODO: Find in valid
                //foreach (var item in valid.Where(x => existingDupplicate.Any(y => y.App_Id.Trim().Equals(x.App_Id.Trim()))).ToList())
                //{
                //    // Remove item from valid list
                //    valid.Remove(item);

                //    // Map valid item to invalid
                //    var config = new MapperConfiguration(cfg =>
                //    {
                //        cfg.CreateMap<UnderwriteCondition, UnderwriteConditionInvalid>();
                //    });

                //    IMapper mapper = config.CreateMapper();
                //    var dest = mapper.Map<UnderwriteCondition, UnderwriteConditionInvalid>(item);
                //    dest.InvalidList = new List<string>() { "Dupplicate column App_Id in database" };

                //    // Add item to invalid list
                //    invalid.Add(dest);
                //}

                #endregion Check Duplicate

                #region Validate AppID

                var notExistAppId = helpers.ValidateAppId(tmpValid.Where(x => !string.IsNullOrEmpty(x.App_Id)).Select(x => x.App_Id.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistAppId.Any(y => y.Equals(x.App_Id.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found App_Id");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistAppId.Any(y => y.Equals(x.App_Id.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_UnderwriteCondition, UnderwriteConditionInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_UnderwriteCondition, UnderwriteConditionInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found App_Id" };

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
                        cfg.CreateMap<D_UnderwriteCondition, UnderwriteConditionInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_UnderwriteCondition, UnderwriteConditionInvalid>(item);
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
                        cfg.CreateMap<D_UnderwriteCondition, UnderwriteConditionInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_UnderwriteCondition, UnderwriteConditionInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found PayerBranchID" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Branch

                #region Validate EmployeeSS

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
                        cfg.CreateMap<D_UnderwriteCondition, UnderwriteConditionInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_UnderwriteCondition, UnderwriteConditionInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found Sale_id1" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate EmployeeSS

                #region Validate Team

                var notExistTeam = helpers.ValidateTeam(tmpValid.Where(x => !string.IsNullOrEmpty(x.TeamId)).Select(x => x.TeamId.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistTeam.Any(y => y.Equals(x.TeamId.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found TeamId");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistTeam.Any(y => y.Equals(x.TeamId.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_UnderwriteCondition, UnderwriteConditionInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_UnderwriteCondition, UnderwriteConditionInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found TeamId" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Team

                #region Validate Branch

                var notExistBranch2 = helpers.ValidateBranch(tmpValid.Where(x => !string.IsNullOrEmpty(x.BranchId)).Select(x => x.BranchId.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistBranch2.Any(y => y.Equals(x.BranchId.Trim()))).ToList())
                {
                    item.InvalidList.Add("Not found BranchId");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistBranch2.Any(y => y.Equals(x.BranchId.Trim()))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_UnderwriteCondition, UnderwriteConditionInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_UnderwriteCondition, UnderwriteConditionInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found BranchId" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Branch

                #region Validate District

                var notExistDistrict = helpers.ValidateDistrict(tmpValid.Where(x => !string.IsNullOrEmpty(x.District_ID)).Select(x => x.District_ID.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistDistrict.Any(y => y.Equals(x.District_ID))).ToList())
                {
                    item.InvalidList.Add("Not found District_ID");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistDistrict.Any(y => y.Equals(x.District_ID))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_UnderwriteCondition, UnderwriteConditionInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_UnderwriteCondition, UnderwriteConditionInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found District_ID" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate District

                #region Validate Province

                var notExistProvince = helpers.ValidateProvince(tmpValid.Where(x => !string.IsNullOrEmpty(x.PayerProvince_ID)).Select(x => x.PayerProvince_ID.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistProvince.Any(y => y.Equals(x.PayerProvince_ID))).ToList())
                {
                    item.InvalidList.Add("Not found PayerProvince_ID");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistProvince.Any(y => y.Equals(x.PayerProvince_ID))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_UnderwriteCondition, UnderwriteConditionInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_UnderwriteCondition, UnderwriteConditionInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found PayerProvince_ID" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Province

                #region Validate Zebra

                var notExistZebraId = helpers.ValidateZebra(tmpValid.Where(x => !string.IsNullOrEmpty(x.ZebraSaleID)).Select(x => x.ZebraSaleID.Trim()).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistZebraId.Any(y => y.Equals(x.ZebraSaleID))).ToList())
                {
                    item.InvalidList.Add("Not found ZebraSaleID");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistZebraId.Any(y => y.Equals(x.ZebraSaleID))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_UnderwriteCondition, UnderwriteConditionInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_UnderwriteCondition, UnderwriteConditionInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found ZebraSaleID" };

                    // Add item to invalid list
                    invalid.Add(dest);
                }

                #endregion Validate Zebra

                #region Validate EmployeeSS

                var notExistEmployeeSS2 = helpers.ValidateEmployeeSS(tmpValid.Select(x => x.ZebraSaleNumber).ToList());

                // TODO: Find in exist invalid
                foreach (var item in invalid.Where(x => notExistEmployeeSS2.Any(y => y.Equals(x.ZebraSaleNumber))).ToList())
                {
                    item.InvalidList.Add("Not found ZebraSaleNumber");
                }

                // TODO: Find in valid
                foreach (var item in valid.Where(x => notExistEmployeeSS2.Any(y => y.Equals(x.ZebraSaleNumber))).ToList())
                {
                    // Remove item from valid list
                    valid.Remove(item);

                    // Map valid item to invalid
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<D_UnderwriteCondition, UnderwriteConditionInvalid>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var dest = mapper.Map<D_UnderwriteCondition, UnderwriteConditionInvalid>(item);
                    dest.InvalidList = new List<string>() { "Not found ZebraSaleNumber" };

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
            var latestPeriodList = _context.D_UnderwriteCondition.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_UnderwriteCondition.Remove(itm);
                    _context.SaveChanges();
                }
            }

            // Get data from session by datakey
            var valid = (List<D_UnderwriteCondition>)Session[dataKey];

            EFBatchOperation.For(_context, _context.D_UnderwriteCondition).InsertAll(valid);

            //call sp to update import log
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogManual_Update(periodResult.periodId, "D_UnderwriteCondition", user);

            Session[dataKey] = null;

            return Json("บันทึกข้อมูลเรียบร้อย");
        }

        [HttpPost]
        public JsonResult SaveNoData()
        {
            var periodResult = GlobalFunction.GetLatestPeriod();

            var latestPeriodList = _context.D_UnderwriteCondition.Where(x => x.Period_Id == periodResult.periodId).ToList();
            if (latestPeriodList.Count != 0)
            {
                foreach (var itm in latestPeriodList)
                {
                    _context.D_UnderwriteCondition.Remove(itm);
                    _context.SaveChanges();
                }
            }

            var user = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            _context.usp_DatasourceImportedLogNoData_Update(periodResult.periodId, "D_UnderwriteCondition", user);

            return Json("บันทึกข้อมูลเรียบร้อย");
        }
    }
}