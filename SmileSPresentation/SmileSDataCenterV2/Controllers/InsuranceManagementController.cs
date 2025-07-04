using OfficeOpenXml;
using OfficeOpenXml.Style;
using SmileSDataCenterV2.Helper;
using SmileSDataCenterV2.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SmileSDataCenterV2.Controllers
{
    public class InsuranceManagementController : Controller
    {
        private DataCenterV1Entities _db;

        // GET: InsuranceManagement
        public InsuranceManagementController()
        {
            _db = new DataCenterV1Entities();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        public ActionResult Index()
        {
            return View();
        }

        [Authorization(Roles = "Developer,DataCenter_Supervisor")]
        public ActionResult InsuranceManagement()
        {
            ViewBag.OrganizeType = _db.usp_OrganizeType_Select(null, 3, 0, 99999, null, null, null).ToList();
            ViewBag.Province = _db.usp_Province_Select(null).ToList();
            return View();
        }

        [Authorization(Roles = "Developer,DataCenter_Supervisor")]
        public ActionResult InsuranceDuplicateList(string organizeTypeId, string subDistrictId, string keyword)
        {
            var organizeTypeBase64EncodeBytes = Convert.FromBase64String(organizeTypeId);
            var deCode_organizeTypeId = System.Text.Encoding.UTF8.GetString(organizeTypeBase64EncodeBytes);

            var subDistrictBase64EncodeBypes = Convert.FromBase64String(subDistrictId);
            var deCode_subDistrictId = System.Text.Encoding.UTF8.GetString(subDistrictBase64EncodeBypes);

            var keywordBase64EncodeBypes = Convert.FromBase64String(keyword);
            var deCode_keyword = System.Text.Encoding.UTF8.GetString(keywordBase64EncodeBypes);

            ViewBag.organizeTypeId = deCode_organizeTypeId;
            ViewBag.subDistrictId = deCode_subDistrictId;
            ViewBag.keyword = deCode_keyword;

            return View();
        }

        [Authorization(Roles = "Developer,DataCenter_Supervisor")]
        public ActionResult InsuranceManageusability()
        {
            ViewBag.OrganizeType = _db.usp_OrganizeType_Select(null, 3, 0, 99999, null, null, null).ToList();
            ViewBag.Province = _db.usp_Province_Select(null).ToList();

            return View();
        }

        [Authorization(Roles = "Developer,DataCenter_Supervisor")]
        public ActionResult InformationManagementMonitor()
        {
            ViewBag.OrganizeType = _db.usp_OrganizeType_Select(null, 3, 0, 99999, null, null, null).ToList();
            //Province List
            ViewBag.ProvinceList = _db.usp_Province_Select(null).ToList();

            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.userID = userID;

            return View();
        }

        [Authorization(Roles = "Developer,DataCenter_Supervisor")]
        public ActionResult InformationInsuranceManagement(string organizeID, string mode)
        {
            var organizeIDBase64EncodeBytes = Convert.FromBase64String(organizeID);
            var deCode_organizeId = System.Text.Encoding.UTF8.GetString(organizeIDBase64EncodeBytes);

            var modeBase64EncodeBytes = Convert.FromBase64String(mode);
            var deCode_mode = System.Text.Encoding.UTF8.GetString(modeBase64EncodeBytes);

            ViewBag.BankName = _db.usp_Bank_Select(null).ToList().Where(x => x.BankDetail != "รอข้อมูล");
            ViewBag.OrganizeId = Convert.ToInt32(deCode_organizeId);
            ViewBag.Mode = deCode_mode;
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.userID = userID;

            return View();
        }

        public JsonResult GetInsuranceOrganizeManagement(int? organizeTypeId = null, int? provinceId = null, string keyword = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            organizeTypeId = organizeTypeId == -1 ? null : organizeTypeId;
            provinceId = provinceId == -1 ? null : provinceId;
            keyword = keyword == "" ? null : keyword.Trim();

            var result = _db.usp_InsuranceOrganize_Select(null, organizeTypeId, provinceId, keyword, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveOrganize(int organizeTypeId, string organizeDetail, string no, string moo, string villageName, string floor, string soi, string road, string subDistrictId, string zipCode)
        {
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _db.usp_OrganizeV3_Insert(organizeTypeId, organizeDetail, "", villageName, no, moo, floor, soi, road, subDistrictId, zipCode, userID, null, null).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateOrganize(int organizeId, int organizeTypeId, string organizeDetail, string no, string moo, string villageName, string floor, string soi, string road, string subDistrictId, string zipCode)
        {
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var result = _db.usp_OrganizeV3_Update(organizeId, organizeTypeId, organizeDetail, "", villageName, no, moo, floor, soi, road, subDistrictId, zipCode, userID, null, null).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInsuranceOrganizeById(int organizeId)
        {
            var result = _db.usp_InsuranceOrganize_Select(organizeId, null, null, null, 0, 999, null, null, null).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDuplicateOrganizeCount(int organizeTypeId, string subDistrictId, string keyword)
        {
            var rs = _db.usp_CheckDuplicateOrganizeV2_Select(subDistrictId, organizeTypeId, null, 0, 99999, null, null, keyword).Count();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrganizeDuplicateList(int? organizeTypeId = null, string subDistrictId = null, string keyword = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_CheckDuplicateOrganizeV2_Select(subDistrictId, organizeTypeId, null, pageStart, pageSize, sortField, orderType, keyword).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateActiveInsurance(int orgainzeId, bool isActive)
        {
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var rs = _db.usp_OrganizeActive_Update(orgainzeId, isActive, userID).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInsuranceInformationDataById(int organizeId)
        {
            var rs = _db.usp_OrganizeInformationByOrganizeId_Select(organizeId).Single();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetContractDataById(int organizeId)
        {
            var rs = _db.usp_OrganizeContractDetailByOrganizeId_Select(organizeId).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBankAccountDataById(int organizeId)
        {
            var rs = _db.usp_OrganizeBankAccountByOrganizeId_Select(organizeId).SingleOrDefault();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ContractInsertOrUpdate(int organizeId, bool isContract, string remark = null)
        {
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var rs = _db.usp_OrganizeContractDetail_Insert(organizeId, isContract, null, null, null, null, userID, null, null, null, null, remark).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BankAccountInsertOrUpdate(int organizeId, string ChequeName = null, int? bankId = null, string bankBranch = null, string bankAccountNo = null, string bankAccountName = null)
        {
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            //var rs = _db.usp_OrganizeContractDetail_Insert(organizeId,)

            ChequeName = ChequeName == "" ? null : ChequeName;
            bankId = bankId == -1 ? null : bankId;
            bankBranch = bankBranch == "" ? null : bankBranch;
            bankAccountNo = bankAccountNo == "" ? null : bankAccountNo;
            bankAccountName = bankAccountName == "" ? null : bankAccountName;

            var rs = _db.usp_OrganizeBankAccount_Insert(organizeId, ChequeName, bankId, bankBranch, bankAccountNo, bankAccountName, userID, true).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InformationInsertOrUpdate(int organizeId, string nearByPlace = null, string organizeContactTel = null, string organizeContactFax = null
                                                    , string organizeContactEmail = null, string latitude = null, string longitude = null, string financeContact = null,
                                                    string financeTel = null, string financeEmail = null, string financeFax = null, string alternateFinanceContact = null,
                                                    string alternateFinanceTel = null, string alternateFinanceEmail = null, string alternateFinanceFax = null,
                                                    string accountingContact = null, string accountingTel = null, string accountingEmail = null, string accountingFax = null,
                                                    string marketingContact = null, string marketingTel = null, string marketingEmail = null, string marketingFax = null, string nurseContact = null,
                                                    string nurseTel = null, string nurseEmail = null, string nurseFax = null)
        {
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var contactType_Tel = 3;
            var contactType_Email = 4;
            var contactType_Fax = 12;

            var rs = _db.usp_OrganizeInformation_Insert(organizeId, nearByPlace, organizeContactTel, contactType_Tel, organizeContactFax, contactType_Fax, organizeContactEmail, contactType_Email,
                                                        latitude, longitude, financeContact, financeTel, financeEmail, financeFax, alternateFinanceContact, alternateFinanceTel, alternateFinanceEmail,
                                                        alternateFinanceFax, accountingContact, accountingTel, accountingEmail, accountingFax, marketingContact, marketingTel, marketingEmail, marketingFax, nurseContact,
                                                        nurseTel, nurseEmail, nurseFax, userID).Single();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ExportOrganizeReport(int? organizeTypeId = null, int? provinceId = null, string keyword = null)
        {
            await Task.Yield();
            using (var db = new DataCenterV1Entities())
            {
                organizeTypeId = organizeTypeId == -1 ? null : organizeTypeId;
                provinceId = provinceId == -1 ? null : provinceId;
                keyword = keyword == "" ? null : keyword.Trim();

                var lst = db.usp_InsuranceOrganize_Select(null, organizeTypeId, provinceId, keyword, 0, 999999999, "Organize_ID", "DESC", null).ToList()
                    .Select(x => new
                    {
                        รหัสรายการ = x.Organize_ID,
                        ประเภท = x.OrganizeTypeDetail,
                        ชื่อบริษัทประกันภัย = x.OrganizeDetail,
                        ตำบล = x.SubDistrictDetail,
                        อำเภอ = x.DistrictDetail,
                        จังหวัด = x.ProvinceDetail,
                        คู่สัญญา = x.IsContract == true ? "เป็น" : x.IsContract == false ? "ไม่เป็น" : "ไม่ระบุ",
                        วันที่ทำรายการ = x.OrganizeCreateDate.Value.Date.ToString("dd/MM/yyyy"),
                        สถานะ = x.IsActive == true ? "ใช้งาน" : "ไม่ใช้งาน",
                    });

                var stream = new MemoryStream();

                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Detail");
                    workSheet.Cells.LoadFromCollection(lst, true);

                    // Select only the header cells
                    var headerCells = workSheet.Cells[1, 1, 1, workSheet.Dimension.Columns];
                    // Set their text to bold.
                    headerCells.Style.Font.Bold = true;
                    // Set their background color
                    headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                    // Apply the auto-filter to all the columns
                    var allCells = workSheet.Cells[workSheet.Dimension.Address];
                    allCells.AutoFilter = true;
                    // Auto-fit all the columns
                    allCells.AutoFitColumns();

                    package.Save();

                    stream.Position = 0;
                    Session["DownloadExcel_Organize"] = package.GetAsByteArray();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult DownloadExportOrganize()
        {
            if (Session["DownloadExcel_Organize"] != null)
            {
                byte[] data = Session["DownloadExcel_Organize"] as byte[];
                string excelName = $"Report-InsranceOrganize-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        public JsonResult GetOrganizeTransaction(int organizeId, int? draw, int? pageSize, int? indexStart, string sortField, string orderType, string search)
        {
            var result = _db.usp_OrganizeTransaction_Select(organizeId, indexStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrganizeTransactionDetailById(int? transactionId, int? draw, int? pageSize, int? indexStart, string sortField, string orderType, string search)
        {
            var result = _db.usp_OrganizeTransactionDetail_Select(transactionId, indexStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }
    }
}