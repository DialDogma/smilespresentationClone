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
    public class ZebraManagementController : Controller
    {
        private DataCenterV1Entities _db;

        // GET: InsuranceManagement
        public ZebraManagementController()
        {
            _db = new DataCenterV1Entities();

            //Miw
            //Ploy
            //Gig
            //eiei
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        // GET: ZebraManagement
        public ActionResult Index()
        {
            return View();
        }

        [Authorization(Roles = "Developer,DataCenter_Zebra_Operation,DataCenter_Zebra_Supervisor")]
        public ActionResult ZebraManagement()
        {
            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = new SSOService.SSOServiceClient().GetRoleByUserName(username);
            var rolelist = role.Split(',').ToList();

            var roleZebraOperation = new[] { "DataCenter_Zebra_Operation" }; //Operation

            //ถ้าเป็น Role = Operation ให้ viewbag.operation = true
            if (rolelist.Intersect(roleZebraOperation).Count() > 0)
            {
                ViewBag.Operation = 1;
            }
            else
            {
                ViewBag.Operation = 0;
            }

            ViewBag.CarType = _db.usp_ZebraType_Select(0, 999999, null, null, null).ToList();
            ViewBag.ZebraOrganize = _db.usp_ZebraOrganize_Select(0, 9999999, null, null, null).ToList();
            ViewBag.CarBrand = _db.usp_ZebraBrand_Select(0, 99999999, null, null, null).ToList();
            ViewBag.CarProvince = _db.usp_Province_Select(null).ToList();
            return View();
        }

        [Authorization(Roles = "Developer,DataCenter_Zebra_Operation,DataCenter_Zebra_Supervisor")]
        public ActionResult ZebraCaretakerManagementMonitor()
        {
            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = new SSOService.SSOServiceClient().GetRoleByUserName(username);
            var rolelist = role.Split(',').ToList();

            var roleZebraOperation = new[] { "DataCenter_Zebra_Operation" }; //Operation

            //ถ้าเป็น Role = Operation ให้ viewbag.operation = true
            if (rolelist.Intersect(roleZebraOperation).Count() > 0)
            {
                ViewBag.Operation = 1;
            }
            else
            {
                ViewBag.Operation = 0;
            }

            ViewBag.CarType = _db.usp_ZebraType_Select(0, 999999, null, null, null).ToList();
            ViewBag.userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            return View();
        }

        [Authorization(Roles = "Developer,DataCenter_Zebra_Supervisor")]
        public ActionResult ZebraCaretakerManagement(string zebraId)
        {
            var ZebraIdBase64EncodeBytes = Convert.FromBase64String(zebraId);
            var deCode_zebraId = System.Text.Encoding.UTF8.GetString(ZebraIdBase64EncodeBytes);

            ViewBag.ZebraId = deCode_zebraId;
            ViewBag.CarType = _db.usp_ZebraType_Select(0, 999999, null, null, null).ToList();
            ViewBag.userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            return View();
        }

        [Authorization(Roles = "Developer,DataCenter_Zebra_Supervisor")]
        public ActionResult ZebraManageusability()
        {
            ViewBag.CarType = _db.usp_ZebraType_Select(0, 999999, null, null, null).ToList();
            return View();
        }

        [Authorization(Roles = "Developer,DataCenter_Zebra_Supervisor")]
        public ActionResult ZebraConfirmManagement()
        {
            return View();
        }

        [Authorization(Roles = "Developer,DataCenter_Zebra_Operation,DataCenter_Zebra_Supervisor")]
        public ActionResult ZebraCaretakerManagementDetail(string zebraId)
        {
            var ZebraIdBase64EncodeBytes = Convert.FromBase64String(zebraId);
            var deCode_zebraId = System.Text.Encoding.UTF8.GetString(ZebraIdBase64EncodeBytes);

            ViewBag.ZebraId = deCode_zebraId;

            return View();
        }

        public JsonResult GetCarModel(string brancdId)
        {
            var rs = _db.usp_ZebraModel_Select(brancdId, 0, 9999999, null, null, null).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DoSaveZebra(int companyId, string zebraCarNo, string carColor, int brandId, int modelId, string subModel
                                     , bool isNewCar, string vinNo, string engineNo, string pickupDate, string plateNo = null, int? plateProvinceId = null)
        {
            DateTime d_pickupDate;

            d_pickupDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(pickupDate));
            plateNo = plateNo == "" ? null : plateNo;
            plateProvinceId = plateProvinceId == -1 ? null : plateProvinceId;

            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_Zebra_Insert(companyId, zebraCarNo, carColor, brandId, modelId, subModel, isNewCar, plateNo, plateProvinceId, vinNo, engineNo, d_pickupDate, userID).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DoUpdateZebra(int zebraId, int companyId, string carColor, int brandId, int modelId, string subModel
                                     , bool isNewCar, string engineNo, string pickupDate, string plateNo = null, int? plateProvinceId = null)
        {
            DateTime d_pickupDate;

            d_pickupDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(pickupDate));
            plateNo = plateNo == "" ? null : plateNo;
            plateProvinceId = plateProvinceId == -1 ? null : plateProvinceId;

            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_Zebra_Update(zebraId, companyId, carColor, brandId, modelId, subModel, isNewCar, plateNo, plateProvinceId, engineNo, d_pickupDate, userID).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetZebraManagement(int? CarTypeId = null, int? IsActive = null, int? IsOwner = null, string keyword = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            bool? b_IsActive = null;
            bool? b_IsOwner = null;

            keyword = keyword == "" ? null : keyword.Trim();
            CarTypeId = CarTypeId == -1 ? null : CarTypeId;

            switch (IsActive)
            {
                case 1:
                    b_IsActive = true;
                    break;

                case 0:
                    b_IsActive = false;
                    break;

                default:
                    break;
            }

            switch (IsOwner)
            {
                case 1:
                    b_IsOwner = true;
                    break;

                case 0:
                    b_IsOwner = false;
                    break;

                default:
                    break;
            }

            var result = _db.usp_ZebraCar_Select(null, CarTypeId, b_IsActive, b_IsOwner, null, pageStart, pageSize, sortField, orderType, keyword).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetZebraCaretakerManagement(int? CarTypeId = null, int? IsOwner = null, string ownerCode = null, string keyword = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            bool? b_IsOwner = null;

            keyword = keyword == "" ? null : keyword.Trim();
            CarTypeId = CarTypeId == -1 ? null : CarTypeId;
            ownerCode = ownerCode == "" ? null : ownerCode;

            switch (IsOwner)
            {
                case 1:
                    b_IsOwner = true;
                    break;

                case 0:
                    b_IsOwner = false;
                    break;

                default:
                    break;
            }

            var result = _db.usp_ZebraOwner_Select(CarTypeId, b_IsOwner, ownerCode, pageStart, pageSize, sortField, orderType, keyword).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOwnerData(string q)
        {
            var rs = _db.usp_EmployeeSearch_Select(q).ToList();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeByEmployeeCode(string employeeCode)
        {
            var rs = _db.usp_EmployeeByEmployeeCode_Select(employeeCode).Single();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetZebraCaretakerManagementById(int zebraId)
        {
            var rs = _db.usp_ZebraCar_Select(zebraId, null, null, null, null, 0, 99999, null, null, null).Single();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CaretakerSaveOrUpdate(int zebraId, int carTypeId, string applyDate, string employeeCode = null, string remark = null)
        {
            DateTime d_applyDate;

            d_applyDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(applyDate));
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            employeeCode = employeeCode == "" ? null : employeeCode == "null" ? null : employeeCode;

            var rs = _db.usp_ZebraOwner_Insert(zebraId, carTypeId, d_applyDate, employeeCode, userID, remark).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTransaction(int? zebraId = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_ZebraTransaction_Select(zebraId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTransactionDetail(int? transactionId = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_ZebraTransactionDetail_Select(transactionId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateActive(int zebraId, bool isActive)
        {
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var rs = _db.usp_ZebraStatus_Update(zebraId, isActive, userID).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetZebraConfirmManagement(int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _db.usp_ZebraOwnerConfirm_Select(pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ConfirmZebraUpdate(string[] zebraIdArray)
        {
            var zebraIdList = "";

            if (zebraIdArray != null)
            {
                zebraIdList = string.Join(",", zebraIdArray);
            }
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _db.usp_ZebraOwner_Update(zebraIdList, userID).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        #region "Export"

        public async Task<ActionResult> ExportZebraManagementReport(int? CarTypeId = null, int? IsActive = null, int? IsOwner = null, string keyword = null)
        {
            await Task.Yield();
            using (var db = new DataCenterV1Entities())
            {
                bool? b_IsActive = null;
                bool? b_IsOwner = null;

                keyword = keyword == "" ? null : keyword.Trim();
                CarTypeId = CarTypeId == -1 ? null : CarTypeId;

                switch (IsActive)
                {
                    case 1:
                        b_IsActive = true;
                        break;

                    case 0:
                        b_IsActive = false;
                        break;

                    default:
                        break;
                }

                switch (IsOwner)
                {
                    case 1:
                        b_IsOwner = true;
                        break;

                    case 0:
                        b_IsOwner = false;
                        break;

                    default:
                        break;
                }

                var lst = _db.usp_ZebraCar_Select(null, CarTypeId, b_IsActive, b_IsOwner, null, 0, 999999999, null, null, keyword).ToList()
                            .Select(x => new
                            {
                                รหัสรายการ = x.Zebra_ID,
                                ประเภทรถ = x.ZebraType,
                                สังกัดบริษัท = x.Abbreviation,
                                เบอร์รถ = x.Zebra_No,
                                ยี่ห้อรถ = x.Brand,
                                รุ่นรถ = x.Model,
                                รุ่นย่อย = x.SubModel,
                                เลขเครื่องยนต์ = x.EngineNo,
                                เลขตัวถัง = x.VINno,
                                ทะเบียนรถ = x.PlateNo,
                                จังหวัดทะเบียนรถ = x.ProvinceDetail,
                                ผู้ดูแลรถ = x.OwnerEmployeeFullName,
                                สถานะรถ = x.CarStatus,
                                สถานะการใช้งาน = x.CarOwnerStatus,
                                วันที่ทำรายการ = x.CreatedDate != null ? x.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm") : null
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
                    Session["DownloadExcel_ZebraManagement"] = package.GetAsByteArray();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public async Task<ActionResult> ExportZebraCaretakerManagementReport(int? CarTypeId = null, int? IsOwner = null, string ownerCode = null, string keyword = null)
        {
            await Task.Yield();
            using (var db = new DataCenterV1Entities())
            {
                bool? b_IsActive = null;
                bool? b_IsOwner = null;

                keyword = keyword == "" ? null : keyword.Trim();
                CarTypeId = CarTypeId == -1 ? null : CarTypeId;
                ownerCode = ownerCode == "" ? null : ownerCode;

                switch (IsOwner)
                {
                    case 1:
                        b_IsOwner = true;
                        break;

                    case 0:
                        b_IsOwner = false;
                        break;

                    default:
                        break;
                }

                var lst = _db.usp_ZebraOwner_Select(CarTypeId, b_IsOwner, ownerCode, 0, 999999999, null, null, keyword).ToList()
                            .Select(x => new
                            {
                                รหัสรายการ = x.Zebra_ID,
                                ประเภทรถ = x.ZebraType,
                                สังกัดบริษัท = x.Abbreviation,
                                เบอร์รถ = x.Zebra_No,
                                ยี่ห้อรถ = x.Brand,
                                รุ่นรถ = x.Model,
                                รุ่นย่อย = x.SubModel,
                                เลขเครื่องยนต์ = x.EngineNo,
                                เลขตัวถัง = x.VINno,
                                ทะเบียนรถ = x.PlateNo,
                                จังหวัดทะเบียนรถ = x.ProvinceDetail,
                                ผู้ดูแลรถ = x.OwnerEmployeeFullName,
                                ผู้ดูแลรถใหม่ = x.NewOwnerEmployeefullName,
                                สถานะรถ = x.CarStatus,
                                สถานะการใช้งาน = x.CarOwnerStatus,
                                วันที่ทำรายการ = x.CreatedDate != null ? x.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm") : null
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
                    Session["DownloadExcel_ZebraCaretakerManagement"] = package.GetAsByteArray();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult DownloadExportZebraManagement()
        {
            if (Session["DownloadExcel_ZebraManagement"] != null)
            {
                byte[] data = Session["DownloadExcel_ZebraManagement"] as byte[];
                string excelName = $"Report-ZebraManagement-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else if (Session["DownloadExcel_ZebraCaretakerManagement"] != null)
            {
                byte[] data = Session["DownloadExcel_ZebraCaretakerManagement"] as byte[];
                string excelName = $"Report-ZebraCaretakerManagement-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        #endregion "Export"
    }
}