using OfficeOpenXml;
using OfficeOpenXml.Style;
using SmileSDataCenterV2.Helper;
using SmileSDataCenterV2.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmileSDataCenterV2.Controllers
{
    [Authorization]
    public class WorkplaceController : Controller
    {
        private DataCenterV1Entities _db;

        // GET: Workplace
        public WorkplaceController()
        {
            _db = new DataCenterV1Entities();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        public ActionResult WorkplaceManagement()
        {
            ViewBag.Province = _db.usp_Province_Select(null).ToList();
            ViewBag.OrganizeSubType = _db.usp_OrganizeTypeXOrganizeSubType_Select(24, 0, 9999, null, null, null).ToList();

            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.userID = userID;

            return View();
        }

        public ActionResult WorkplaceEditAndCreate(string workplaceId)
        {
            string msg = "";

            byte[] idByte = Convert.FromBase64String(workplaceId);
            string id = System.Text.Encoding.UTF8.GetString(idByte);

            if (id == "CreateNew")
            {
                msg = "Create";
                ViewBag.Title = "เพิ่มรายชื่อหน่วยงาน";
            }
            else
            {
                msg = id;
                var referenceId = Int32.Parse(id);
                var user = GlobalFunction.GetLoginDetail(HttpContext);
                var insertDoc = _db.usp_DocumentOrganize_Insert(referenceId, "39,40,41", null, user.User_ID).FirstOrDefault();

                ViewBag.Title = "แก้ไขรายชื่อหน่วยงาน";
            }
            ViewBag.Msg = msg;
            ViewBag.OrganizeType = _db.usp_OrganizeType_Select(null, 9, null, 9999, null, null, null).ToList();

            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.userID = userID;

            return View();
        }

        public ActionResult WorkplaceManageUsability()
        {
            ViewBag.Province = _db.usp_Province_Select(null).ToList();
            ViewBag.OrganizeSubType = _db.usp_OrganizeTypeXOrganizeSubType_Select(24, 0, 9999, null, null, null).ToList();
            return View();
        }

        public ActionResult WorkplaceInformationMonitor()
        {
            ViewBag.Province = _db.usp_Province_Select(null).ToList();
            ViewBag.OrganizeSubType = _db.usp_OrganizeTypeXOrganizeSubType_Select(24, 0, 9999, null, null, null).ToList();
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.userID = userID;

            return View();
        }

        public ActionResult WorkplaceDataDetail(string organizeID)
        {
            var OrganizeIDBase64EncodeBytes = Convert.FromBase64String(organizeID);
            var deCode_organizeID = System.Text.Encoding.UTF8.GetString(OrganizeIDBase64EncodeBytes);
            ViewBag.organizeID = deCode_organizeID;

            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.userID = userID;

            return View();
        }

        public ActionResult WorkplaceInformationManagement(string organizeID)
        {
            var OrganizeIDBase64EncodeBytes = Convert.FromBase64String(organizeID);
            var deCode_organizeID = System.Text.Encoding.UTF8.GetString(OrganizeIDBase64EncodeBytes);
            ViewBag.organizeID = deCode_organizeID;

            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.userID = userID;

            return View();
        }

        public JsonResult GetWorkplaceMonitor(int? organizeGroupId, int? organizeSubTypeId, int? provinceId, int indexStart, int draw, int pageSize, string sortField, string orderType, string searchDetail)
        {
            var list = _db.usp_OrganizeWorkPlace_Select(organizeGroupId, null, provinceId, indexStart, pageSize, sortField, orderType, searchDetail, organizeSubTypeId).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
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

        public JsonResult GetOrganizeTransactionDetail(int? organizeTransactionId = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_OrganizeTransactionDetail_Select(organizeTransactionId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWorkplaceDocument(int? referenceId, string documentTypeIdList, int indexStart, int draw, int pageSize, string sortField, string orderType, string searchDetail)
        {
            var list = _db.usp_DocumentOrganize_Select(referenceId, documentTypeIdList, indexStart, pageSize, sortField, orderType, searchDetail).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrganizeDetailById(int? organizeId)
        {
            var Organize = _db.usp_OrganizeDetailByOrganizeId_Select(organizeId).SingleOrDefault();
            return Json(Organize, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InsertOrganizeWorkplace(int? organizeTypeId, string organizeDetail, string buildingName, string villageName, string no, string moo, string floor, string soi, string road, string subDistrictCode, string zipCode, int? organizeSubTypeId, string taxNumber)
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext);

            var result = _db.usp_OrganizeV3_Insert(organizeTypeId, organizeDetail, buildingName, villageName, no, moo,
                                                   floor, soi, road, subDistrictCode, zipCode, user.User_ID, organizeSubTypeId, taxNumber).FirstOrDefault();

            if (result.IsResult == true)
            {
                var referenceId = Int32.Parse(result.Result);
                var insertDoc = _db.usp_DocumentOrganize_Insert(referenceId, "39,40,41", null, user.User_ID).FirstOrDefault();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateOrganizeWorkplace(int? organizeId, int? organizeTypeId, string organizeDetail, string buildingName, string villageName, string no, string moo, string floor, string soi, string road, string subDistrictCode, string zipCode, int? organizeSubTypeId, string taxNumber)
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext);

            var result = _db.usp_OrganizeV3_Update(organizeId, organizeTypeId, organizeDetail, buildingName, villageName, no, moo,
                                                   floor, soi, road, subDistrictCode, zipCode, user.User_ID, organizeSubTypeId, taxNumber).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckDuplicateWorkplaceName(int? organizeId, string subDistrictCode, int? organizeTypeId, int? organizeSubTypeId, string searchDetail)
        {
            var duplicateList = _db.usp_CheckDuplicateOrganizeV2_Select(subDistrictCode, organizeTypeId, organizeSubTypeId, 0, 99999, null, null, searchDetail).ToList();
            var duplcateResult = duplicateList.Where(x => x.Organize_ID != organizeId).ToList();
            return Json(duplcateResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDuplicateWorkplaceNameDetail(int? organizeId, string subDistrictCode, int? organizeTypeId, int? organizeSubTypeId, int indexStart, int draw, int pageSize, string sortField, string orderType, string searchDetail)
        {
            var list = _db.usp_CheckDuplicateOrganizeV2_Select(subDistrictCode, organizeTypeId, organizeSubTypeId, indexStart, pageSize, sortField, orderType, searchDetail).Where(x => x.Organize_ID != organizeId).ToList();
            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
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

        public JsonResult GetOrganizeSubTypeByOrganizeTypeId(int? organizeTypeId)
        {
            var result = _db.usp_OrganizeTypeXOrganizeSubType_Select(organizeTypeId, 0, 9999, null, null, null).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InsertOrganizeDocument(int? referenceId)
        {
            var createdByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var insertDoc = _db.usp_DocumentOrganize_Insert(referenceId, "39,40,41", null, createdByUserId).FirstOrDefault();
            return Json(insertDoc, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateUsabilityWorkplace(int? organizeId, bool isActive)
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext);

            var result = _db.usp_OrganizeActive_Update(organizeId, isActive, user.User_ID).FirstOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportWorkplaceMonitor(int? organizeSubTypeId, int? provinceId, string searchDetail)
        {
            using (var db = new DataCenterV1Entities())
            {
                var list = _db.usp_OrganizeWorkPlace_Select(9, 24, provinceId, 0, 99999, "Organize_ID", "desc", searchDetail, organizeSubTypeId)
            .Select(x => new
            {
                รหัสรายการ = x.OrganizeCode,
                ประเภทย่อย = x.OrganizeSubType,
                ชื่อหน่วยงาน = x.OrganizeDetail,
                เลขประจำตัวผู้เสียภาษีอากร = x.TaxNumber,
                ตำบล = x.SubDistrictDetail,
                อำเภอ = x.DistrictDetail,
                จังหวัด = x.ProvinceDetail,
                วันที่ทำรายการ = x.UpdatedDate.Value.Date.ToString("dd/MM/yyyy"),
                สถานะ = x.IsActive == true ? "ใช้งาน" : "ไม่ใช้งาน",
            }).ToList();
                var stream = new MemoryStream();

                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Detail");
                    workSheet.Cells.LoadFromCollection(list, true);

                    var headerCells = workSheet.Cells[1, 1, 1, workSheet.Dimension.Columns];
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
                    Session["DownloadExcel_Workplace"] = package.GetAsByteArray();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult ExportWorkplaceDownload()
        {
            if (Session["DownloadExcel_Workplace"] != null)
            {
                byte[] data = Session["DownloadExcel_Workplace"] as byte[];
                string excelName = $"Report-Workplace-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }
    }
}