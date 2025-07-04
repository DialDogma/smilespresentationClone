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
    public class AcademyManagementController : Controller
    {
        // GET: Academy
        private DataCenterV1Entities _db;

        // GET: InsuranceManagement
        public AcademyManagementController()
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
        public ActionResult AcademyManagement()
        {
            ViewBag.OrganizeType = _db.usp_OrganizeType_Select(null, 6, 0, 99999, null, null, null).ToList();
            ViewBag.Province = _db.usp_Province_Select(null).ToList();
            ViewBag.OrganizeTitle = _db.usp_Title_Select(null, 1).ToList().Where(x => x.TitleDetail != "รอข้อมูล");
            return View();
        }

        [Authorization(Roles = "Developer,DataCenter_Supervisor")]
        public ActionResult AcademyDuplicateList(string organizeTypeId, string subDistrictId, string keyword, string organizeTitleId)
        {
            var organizeTypeBase64EncodeBytes = Convert.FromBase64String(organizeTypeId);
            var deCode_organizeTypeId = System.Text.Encoding.UTF8.GetString(organizeTypeBase64EncodeBytes);

            var subDistrictBase64EncodeBypes = Convert.FromBase64String(subDistrictId);
            var deCode_subDistrictId = System.Text.Encoding.UTF8.GetString(subDistrictBase64EncodeBypes);

            var keywordBase64EncodeBypes = Convert.FromBase64String(keyword);
            var deCode_keyword = System.Text.Encoding.UTF8.GetString(keywordBase64EncodeBypes);

            var organizeTitleIdBase64EncodeBypes = Convert.FromBase64String(organizeTitleId);
            var deCode_organizeTitleId = System.Text.Encoding.UTF8.GetString(organizeTitleIdBase64EncodeBypes);

            ViewBag.organizeTypeId = deCode_organizeTypeId;
            ViewBag.subDistrictId = deCode_subDistrictId;
            ViewBag.keyword = deCode_keyword;
            ViewBag.organizeTitleId = deCode_organizeTitleId;

            return View();
        }

        [Authorization(Roles = "Developer,DataCenter_Supervisor")]
        public ActionResult AcademyManageusability()
        {
            ViewBag.OrganizeType = _db.usp_OrganizeType_Select(null, 6, 0, 99999, null, null, null).ToList();
            ViewBag.Province = _db.usp_Province_Select(null).ToList();
            return View();
        }

        public JsonResult GetAcademyOrganizeManagement(int? organizeTypeId = null, int? provinceId = null, string keyword = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            organizeTypeId = organizeTypeId == -1 ? null : organizeTypeId;
            provinceId = provinceId == -1 ? null : provinceId;
            keyword = keyword == "" ? null : keyword;

            var result = _db.usp_AcademyOrganize_Select(null, organizeTypeId, provinceId, keyword, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAcademyOrganizeById(int organizeId)
        {
            var result = _db.usp_AcademyOrganize_Select(organizeId, null, null, null, 0, 999, null, null, null).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveOrganize(int organizeTypeId, string organizeDetail, string no, string moo, string villageName, string floor, string soi, string road, string subDistrictId, string zipCode, int organizeTitleId)
        {
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _db.usp_OrganizeAcademyV2_Insert(organizeTypeId, organizeTitleId, organizeDetail, "", villageName, no, moo, floor, soi, road, subDistrictId, zipCode, userID).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateOrganize(int organizeId, int organizeTypeId, string organizeDetail, string no, string moo, string villageName, string floor, string soi, string road, string subDistrictId, string zipCode, int organizeTitleId)
        {
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var result = _db.usp_OrganizeAcademyV2_Update(organizeId, organizeTypeId, organizeTitleId, organizeDetail, "", villageName, no, moo, floor, soi, road, subDistrictId, zipCode, userID).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDuplicateOrganizeCount(int organizeTypeId, string subDistrictId, string keyword, int orgainizeTitleId)
        {
            var rs = _db.usp_CheckDuplicate_OrganizeAcademy_Select(organizeTypeId, orgainizeTitleId, subDistrictId, keyword, 0, 999999999, null, null, null).Count();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrganizeDuplicateList(int? organizeTypeId = null, string subDistrictId = null, string keyword = null, int? orgainizeTitleId = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_CheckDuplicate_OrganizeAcademy_Select(organizeTypeId, orgainizeTitleId, subDistrictId, keyword, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateActiveAcademy(int orgainzeId, bool isActive)
        {
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var rs = _db.usp_OrganizeActive_Update(orgainzeId, isActive, userID).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ExportOrganizeReport(int? organizeTypeId = null, int? provinceId = null, string keyword = null)
        {
            await Task.Yield();
            using (var db = new DataCenterV1Entities())
            {
                organizeTypeId = organizeTypeId == -1 ? null : organizeTypeId;
                provinceId = provinceId == -1 ? null : provinceId;
                keyword = keyword == "" ? null : keyword;

                var lst = db.usp_AcademyOrganize_Select(null, organizeTypeId, provinceId, keyword, 0, 999999999, "Organize_ID", "DESC", null).ToList()
                    .Select(x => new
                    {
                        รหัสรายการ = x.Organize_ID,
                        ประเภท = x.OrganizeTypeDetail,
                        คำนำหน้า = x.TitleDetail,
                        ชื่อสถานศึกษา = x.OrganizeDetail,
                        ตำบล = x.SubDistrictDetail,
                        อำเภอ = x.DistrictDetail,
                        จังหวัด = x.ProvinceDetail,
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
                string excelName = $"Report-AcademyOrganize-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        public ActionResult AcademyInformationMonitor()
        {
            ViewBag.Province = _db.usp_Province_Select(null).ToList();
            ViewBag.OrganizeType = _db.usp_OrganizeType_Select(null, 6, 0, 9999, null, null, null).ToList();
            return View();
        }

        public ActionResult AcademyInformationDetail(string organizeID)
        {
            var OrganizeIDBase64EncodeBytes = Convert.FromBase64String(organizeID);
            var deCode_organizeID = System.Text.Encoding.UTF8.GetString(OrganizeIDBase64EncodeBytes);
            ViewBag.organizeID = deCode_organizeID;
            return View();
        }

        public ActionResult AcademyInformationManagement(string organizeID)
        {
            var OrganizeIDBase64EncodeBytes = Convert.FromBase64String(organizeID);
            var deCode_organizeID = System.Text.Encoding.UTF8.GetString(OrganizeIDBase64EncodeBytes);
            ViewBag.organizeID = deCode_organizeID;
            return View();
        }

        public JsonResult UpsertOrganizeContact(int? organizeId, string primaryPhone, string secordaryPhone = null, string otherPhone = null, string email = null, string lineId = null, string facebookId = null, string twitterId = null, string instagramId = null, string youtubeId = null)
        {
            var createdByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _db.usp_OrganizeContact_Upsert(organizeId, primaryPhone, secordaryPhone, otherPhone, email, lineId, facebookId, twitterId, instagramId, youtubeId, createdByUserId).Single();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrganizeContact(int? organizeId)
        {
            var result = _db.usp_OrganizeContact_Select(organizeId, 0, 9999, null, null, null).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrganizeTransaction(int? organizeId, int indexStart, int draw, int pageSize, string sortField, string orderType, string searchDetail)
        {
            var list = _db.usp_OrganizeTransaction_Select(organizeId, indexStart, pageSize, sortField, orderType, searchDetail).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrganizeTransactionDetail(int? organizeTransactionId = null, int? draw = null, int? pageSize = null, int? indexStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_OrganizeTransactionDetail_Select(organizeTransactionId, indexStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrganizeDetailById(int organizeId)
        {
            var Organize = _db.usp_OrganizeDetailByOrganizeId_Select(organizeId).SingleOrDefault();
            return Json(Organize, JsonRequestBehavior.AllowGet);
        }

        //public void ExportAcademyeMonitor(int? organizeTypeId, int? provinceId, string searchDetail)
        //{
        //    var list = _db.usp_AcademyOrganize_Select(null, organizeTypeId, provinceId, searchDetail, 0, 999999999, "Organize_ID", "DESC", null).ToList();

        //    var Data = list.Select(x => new
        //    {
        //        รหัสรายการ = x.Organize_ID,
        //        ประเภท = x.OrganizeTypeDetail,
        //        คำนำหน้า = x.TitleDetail,
        //        ชื่อหน่วยสถานศึกษา = x.OrganizeDetail,
        //        เลขประจำตัวผู้เสียภาษีอากร = x.,
        //        ตำบล = x.SubDistrictDetail,
        //        อำเภอ = x.DistrictDetail,
        //        จังหวัด = x.ProvinceDetail,
        //        วันที่ทำรายการ = x.OrganizeCreateDate.Value.Date.ToString("dd/MM/yyyy"),
        //        สถานะ = x.IsActive == true ? "ใช้งาน" : "ไม่ใช้งาน",
        //    }).ToList();

        //    ExcelManager.ExportToExcel(this.HttpContext, Data, "sheet1", "Organization_Information_Report_" + DateTime.Now.ToString());
        //}
    }
}