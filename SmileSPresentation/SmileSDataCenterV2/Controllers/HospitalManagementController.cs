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
    public class HospitalManagementController : Controller
    {
        private DataCenterV1Entities _db;

        // GET: HospitalManagement
        public HospitalManagementController()
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
        public ActionResult HospitalManagement()
        {
            ViewBag.OrganizeType = _db.usp_OrganizeType_Select(null, 5, 0, 99999, null, null, null).ToList();
            ViewBag.Province = _db.usp_Province_Select(null).ToList();
            return View();
        }

        [Authorization(Roles = "Developer,DataCenter_Supervisor")]
        public ActionResult HospitalDuplicateList(string organizeTypeId, string subDistrictId, string keyword)
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
        public ActionResult HospitalManageusability()
        {
            ViewBag.OrganizeType = _db.usp_OrganizeType_Select(null, 5, 0, 99999, null, null, null).ToList();
            ViewBag.Province = _db.usp_Province_Select(null).ToList();

            return View();
        }

        public JsonResult GetHospitalOrganizeManagement(int? organizeTypeId = null, int? provinceId = null, string keyword = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = "Organize_ID", string orderType = "ASC", string search = null)
        {
            organizeTypeId = organizeTypeId == -1 ? null : organizeTypeId;
            provinceId = provinceId == -1 ? null : provinceId;
            keyword = keyword == "" ? null : keyword;

            var result = _db.usp_HospitalOrganize_Select(null, organizeTypeId, provinceId, keyword, pageStart, pageSize, sortField, orderType, search).ToList();

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

        public JsonResult GetHospitalOrganizeById(int organizeId)
        {
            var result = _db.usp_HospitalOrganize_Select(organizeId, null, null, null, 0, 999, null, null, null).Single();

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

        public JsonResult UpdateActiveHospital(int orgainzeId, bool isActive)
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

                var lst = db.usp_HospitalOrganize_Select(null, organizeTypeId, provinceId, keyword, 0, 999999999, "Organize_ID", "DESC", null).ToList()
                    .Select(x => new
                    {
                        รหัสรายการ = x.Organize_ID,
                        ประเภท = x.OrganizeTypeDetail,
                        ชื่อสถานพยาบาล = x.OrganizeDetail,
                        ตำบล = x.SubDistrictDetail,
                        อำเภอ = x.DistrictDetail,
                        จังหวัด = x.ProvinceDetail,
                        คู่สัญญา = x.IsContract == true ? "เป็น" : x.IsContract == false ? "ไม่เป็น" : "ไม่ระบุ",
                        วันที่ทำรายการ = x.OrganizeCreateDate.Value.Date.ToString("dd/MM/yyyy"),
                        สถานะ = x.IsActive == true ? "ใช้งาน" : "ไม่ใช้งาน"
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
                string excelName = $"Report-HospitalOrganize-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }
    }
}