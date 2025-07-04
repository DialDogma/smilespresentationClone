using ImageResizer;
using SmileUnderwriteBranchV2.Helper;
using SmileUnderwriteBranchV2.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmileUnderwriteBranchV2.Controllers
{
    [Authorization]
    public class MotorPoliciesController : Controller
    {
        private readonly UnderwriteBranchV2MotorModelContainer _context;

        public MotorPoliciesController()
        {
            _context = new UnderwriteBranchV2MotorModelContainer();
        }

        // GET: MotorPolicies
        public ActionResult Index()
        {
            return View();
        }

        [Authorization(Roles = "Developer,UnderwriteV2_Director")]
        public ActionResult MotorPoliciesQueue(string isActive, string createDataDT)
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext);
            var results = _context.usp_QueuePoliciesGiver_Select(user.EmployeeCode).ToList();
            if (results.Count() == 1)
            {
                ViewBag.GiverName = results[0].GiverName;
                ViewBag.UserId = results[0].UserId;
            }
            else
            {
                ViewBag.GiverName = null;
                ViewBag.UserId = null;
            }
            var provinceResult = _context.usp_Province_Select().ToList();
            ViewBag.province = provinceResult;

            var giveType = _context.usp_GiveTypeMotor_Select(null).ToList();
            ViewBag.giveType = giveType;

            //tap Active
            ViewBag.isActive = "1";
            if (isActive == "2")
            {
                ViewBag.isActive = "2";
            }

            ViewBag.createDataDT = "f";
            if (createDataDT == "t")
            {
                ViewBag.createDataDT = "t";
            }

            /* //get give back insurance card date from web config and convert to date
             var givebackInsuranceCardDate = Settings.Default.GiveBackInsuranceCardEditDate;
             DateTime dateCompare = new DateTime(DateTime.Now.Year, DateTime.Now.Month, givebackInsuranceCardDate);
             ViewBag.dateCompare = dateCompare.ToString("dd-MM-yyyy HH:mm:ss", new CultureInfo("en-Us"));*/
            return View();
        }

        public ActionResult GetMotorProvince()
        {
            var result = _context.usp_Province_Select().ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMotorDistrict(int? provinceId)
        {
            var DistrictByProvince = _context.usp_DistrictByProvinceId_Select(provinceId).ToList();
            return Json(DistrictByProvince, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetMotorPoliciesQueue_dt(string startCoverDate,
                                                    int? playerBranchId,
                                                    int? payerDistrictId,
                                                    int? payerStudyAreaId,
                                                    int? assignToUserId,
                                                    int? provinceId,
                                                    int? districtId,
                                                    string applicationCode,
                                                    string insuredName,
                                                    string payerName,
                                                    string payerOfficeName,
                                                    int? draw,
                                                    int? indexStart,
                                                    int? pageSize,
                                                    string sortField,
                                                    string orderType)
        {
            try
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext);
                var result = _context.usp_QueueMotorPoliciesPendingV2_Select(null,
                    playerBranchId,
                    payerDistrictId,
                    payerStudyAreaId,
                    user.User_ID,
                    (provinceId == -1 ? null : provinceId),
                    (districtId == -1 ? null : districtId),
                    applicationCode,
                    insuredName,
                    payerName,
                    payerOfficeName,
                    indexStart,
                    pageSize,
                    sortField,
                    orderType).ToList();

                var dt = new Dictionary<string, object>
                {
                    {"draw", draw },
                    {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                    {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                    {"data", result.ToList()}
                };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpPost]
        public ActionResult GetMotorPoliciesQueueComplete_dt(string startCoverDate,
                                                    int? playerBranchId,
                                                    int? payerDistrictId,
                                                    int? payerStudyAreaId,
                                                    int? assignToUserId,
                                                    int? provinceId,
                                                    int? districtId,
                                                    string applicationCode,
                                                    string insuredName,
                                                    string payerName,
                                                    string payerOfficeName,
                                                    int? draw,
                                                    int? indexStart,
                                                    int? pageSize,
                                                    string sortField,
                                                    string orderType)
        {
            try
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext);
                var result = _context.usp_QueueMotorPoliciesComplete_Select(null,
                    playerBranchId,
                    payerDistrictId,
                    payerStudyAreaId,
                    user.User_ID,
                     (provinceId == -1 ? null : provinceId),
                    (districtId == -1 ? null : districtId),
                    applicationCode,
                    insuredName,
                    payerName,
                    payerOfficeName,
                    indexStart,
                    pageSize,
                    sortField,
                    orderType).ToList();

                var dt = new Dictionary<string, object>
                {
                    {"draw", draw },
                    {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                    {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                    {"data", result.ToList()}
                };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpPost]
        public ActionResult InsertGivebackPolicies(int? queueId, int? giverUserId, string giveDate, int? giveTypeId,
        string uRLPath, string physicalPath, string fileName, string giveRemark, string giveTypeRemark)
        {
            try
            {
                DateTime? giverDateConvert = null;
                if (giveDate != "" && giveDate != null)
                {
                    giverDateConvert = DateTime.ParseExact(giveDate, "dd-MM-yyyy", new CultureInfo("en-Us"));
                }

                var createdByUserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                var result = _context.usp_QueueMotorPolicies_Insert(queueId, giverUserId, giverDateConvert, giveTypeId, uRLPath, physicalPath, fileName, createdByUserId, giveRemark, giveTypeRemark).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpGet]
        public ActionResult GetGiverUser(string search)
        {
            try
            {
                var result = _context.usp_QueuePoliciesGiver_Select(search).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpGet]
        public ActionResult GetQueueDetail(int queuePoliciesId)
        {
            try
            {
                var result = _context.usp_QueuePoliciesByQueuePoliciesId_Select(queuePoliciesId).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void DownloadQueueMotorPoliciesPendingExcelReport(
                   int? assignToUserId,
                   int? provinceId,
                   int? districtId,
                   string applicationCode,
                   string insuredName,
                   string payerName,
                   string payerOfficeName
                   )
        {
            try
            {
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _context.usp_QueueMotorPoliciesPendingReport_Select(
                    null,
                    null,
                    null,
                    null,
                    userId,
                    provinceId,
                    districtId,
                    applicationCode,
                    insuredName,
                    payerName,
                    payerOfficeName,
                    null,
                    null,
                    null,
                    null).ToList();
                GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", "รายงานมอบกรมธรรม์กรณีเกินกำหนด_รอมอบกรมธรรม์");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public void DownloadQueueMotorPoliciesCompleteExcelReport(
                   int? assignToUserId,
                   int? provinceId,
                   int? districtId,
                   string applicationCode,
                   string insuredName,
                   string payerName,
                   string payerOfficeName)
        {
            try
            {
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _context.usp_QueueMotorPoliciesCompleteReport_Select(null,
                    null,
                    null,
                    null,
                    userId,
                    provinceId,
                    districtId,
                    applicationCode,
                    insuredName,
                    payerName,
                    payerOfficeName,
                    null,
                    null,
                    null,
                    null).ToList();
                GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", "รายงานมอบกรมธรรม์กรณีเกินกำหนด_มอบกรมธรรม์แล้ว");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }


        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file, int hd_queueId, string hd_queueCreatedDate)
        {
            try
            {
                // ดึง User_ID จาก Session
                var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                // รับค่าจากฟอร์ม
                var queueId = hd_queueId;
                var queueCreatedDate = hd_queueCreatedDate;

                // แปลงเป็นปี+เดือน เช่น 202504
                var yearMonth = Convert.ToDateTime(queueCreatedDate).ToString("yyyyMM", new CultureInfo("en-US"));

                // อ่านค่าจาก Web.config (Settings.settings)
                string basePhysicalPath = Properties.Settings.Default.FileImgPath + ("Motor");           // เช่น D:\FileImg\
                string baseUrl = Properties.Settings.Default.AbsolutePathImage + ("Motor");              // เช่น http://yourdomain.com/FileImg/

                // สร้าง path สำหรับเก็บไฟล์
                string folderPath = Path.Combine(basePhysicalPath, yearMonth, $"QueueID_{queueId}");

                // ตั้งชื่อไฟล์ เช่น 210_20250422104500.jpg
                var fileName = $"{queueId}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.jpg";
                string fullPath = Path.Combine(folderPath, fileName);

                // สร้างไดเรกทอรีถ้ายังไม่มี
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // ลบไฟล์เดิมทั้งหมดก่อนอัปโหลดใหม่
                DirectoryInfo di = new DirectoryInfo(folderPath);
                foreach (FileInfo fi in di.EnumerateFiles())
                {
                    fi.Delete();
                }

                // Resize ภาพก่อนเซฟ (ใช้ ImageResizer)
                ImageJob img = new ImageJob(file, fullPath, new ResizeSettings("width=2000;height=2000;format=jpg;mode=max"))
                {
                    CreateParentDirectory = true
                };
                img.Build();

                // คืนค่า URL ที่จะใช้เปิดรูป และพาธจริง
                string relativePath = $"{yearMonth}/QueueID_{queueId}/{fileName}";
                string urlPath = baseUrl.TrimEnd('/') + "/" + relativePath.Replace("\\", "/");
                string physicalPath = fullPath;
                string fileUrl = Url.Action("Image", "MotorUnderwrite", new { yearMonth = yearMonth, queueId = queueId, fileName = fileName }, Request.Url.Scheme);
                return Json(new
                {
                    id = 123,
                    success = true,
                    response = "File uploaded.",
                    fileImgName = fileName,
                    urlPath = fileUrl,
                    physicalPath = physicalPath
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    success = false,
                    response = e.Message
                });
            }
        }

        [HttpPost]
        public ActionResult File_delete(int queueId, string queueCreatedDate)
        {
            try
            {
                //Year - Month
                var yearMonth = Convert.ToDateTime(queueCreatedDate).ToString("yyyyMM", new CultureInfo("en-Us"));
                //path folder
                string path = Properties.Settings.Default.FileImgPath + String.Format("\\Motor\\{0}\\QueueID_{1}", yearMonth, queueId);

                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo fi in di.EnumerateFiles())
                {
                    fi.Delete();
                }

                return Json(new
                {
                    IsResult = true,
                    Msg = "success"
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    IsResult = false,
                    Msg = e.Message
                });
            }
        }
    }
}