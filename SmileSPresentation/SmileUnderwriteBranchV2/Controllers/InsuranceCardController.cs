using ImageResizer;
using SmileUnderwriteBranchV2.Helper;
using SmileUnderwriteBranchV2.Models;
using SmileUnderwriteBranchV2.Properties;
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
    public class InsuranceCardController : Controller
    {
        #region Context

        private readonly UnderwriteBranchV2Entities _context;

        public InsuranceCardController()
        {
            _context = new UnderwriteBranchV2Entities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Context

        #region view

        // GET: InsuranceCard
        [Authorization(Roles = "Developer,UnderwriteV2_Director")]
        public ActionResult GiveBackInsuranceCardAfterUnderwrite()
        {
            var provinceResult = _context.usp_Province_Select().ToList();
            ViewBag.province = provinceResult;

            var giveType = _context.usp_GiveType_Select(null).ToList();
            ViewBag.giveType = giveType;

            //get give back insurance card date from web config and convert to date
            var givebackInsuranceCardDate = Settings.Default.GiveBackInsuranceCardEditDate;
            DateTime dateCompare = new DateTime(DateTime.Now.Year, DateTime.Now.Month, givebackInsuranceCardDate);
            ViewBag.dateCompare = dateCompare.ToString("dd-MM-yyyy HH:mm:ss", new CultureInfo("en-Us"));

            return View();
        }

        #endregion view

        #region api

        [HttpPost]
        public ActionResult GetGivebackInsuranceCardBeforeUnderwrite_dt(int? provinceId, int? districtId, int? draw, int? indexStart, int? pageSize,
            string sortField, string orderType, string search, string search1, string search2, string appId)
        {
            try
            {
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _context.usp_QueueCoBrandPending_Select(null, null, null, null, userId, indexStart, pageSize, sortField,
                    orderType, search, provinceId, districtId, search1, search2, appId).ToList();

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
        public ActionResult GetGivebackInsuranceCardBeforeUnderwriteV2_dt(int? provinceId, int? districtId, int? draw, int? indexStart, int? pageSize,
            string sortField, string orderType, string search, string search1, string search2, string appId)
        {
            try
            {
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _context.usp_QueueCoBrandPendingV2_Select(null, null, null, null, userId, indexStart, pageSize, sortField,
                    orderType, search, provinceId, districtId, search1, search2, appId).ToList();

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
        public ActionResult GetGivebackInsuranceCardBeforeUnderwriteComplete_dt(int? provinceId, int? districtId,
            int? draw, int? indexStart, int? pageSize, string sortField, string orderType, string search, string search1, string search2, string appId)
        {
            try
            {
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _context.usp_QueueCoBrandComplete_Select(null, null, null, null, userId, indexStart, pageSize, sortField,
                    orderType, search, provinceId, districtId, search1, search2, appId).ToList();

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
        public ActionResult InsertGivebackInsuranceCard(int queueId, string coBrandNo, int giverUserId, int giveType, string giverDate,
            string uRLPath, string physicalPath, string fileName, string trackingNo, string giveTypeRemark, bool isLineOA, string giveRemark)
        {
            try
            {
                DateTime? giverDateConvert = null;
                if (giverDate != "")
                {
                    giverDateConvert = DateTime.ParseExact(giverDate, "dd-MM-yyyy", new CultureInfo("en-Us"));
                }

                var createdByUserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                var result = _context.usp_QueueCoBrand_Insert(queueId, coBrandNo, giverUserId, giverDateConvert, giveType, uRLPath, physicalPath,
                   fileName, createdByUserId, trackingNo, giveTypeRemark, isLineOA, giveRemark).FirstOrDefault();

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
                var result = _context.usp_QueueCoBrandGiver_Select(search).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpGet]
        public ActionResult GetQueueDetail(int queueCobrandId)
        {
            try
            {
                var result = _context.usp_QueueCoBrandByQueueCoBrandId_Select(queueCobrandId).FirstOrDefault();
                ViewBag.TrackingNo = result.TrackingNo;
                ViewBag.GiveTypeRemark = result.GiveTypeRemark;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void DownloadQueuePendingExcelReport(int? provinceId, int? districtId, string search)
        {
            try
            {
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _context.usp_QueueCoBrandPendingReport_Select(null, null, null, null, userId, search, provinceId, districtId, null, null, null).ToList();
                GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", "รายงานมอบบัตรกรณีเกินกำหนด_รอมอบบัตร");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public void DownloadQueueCompleteExcelReport(int? provinceId, int? districtId, string search)
        {
            try
            {
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _context.usp_QueueCoBrandCompleteReport_Select(null, null, null, null, userId, search, provinceId, districtId, null, null, null).ToList();
                GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", "รายงานมอบบัตรกรณีเกินกำหนด_มอบบัตรแล้ว");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file, FormCollection form)
        {
            try
            {
                // ดึง User_ID จาก Session
                var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                // รับค่าจากฟอร์ม
                var queueId = form["queueId"];
                var queueCreatedDate = form["createdDate"];

                // แปลงเป็นปี+เดือน เช่น 202504
                var yearMonth = Convert.ToDateTime(queueCreatedDate).ToString("yyyyMM", new CultureInfo("en-US"));

                // อ่านค่าจาก Web.config (Settings.settings)
                string basePhysicalPath = Properties.Settings.Default.FileImgPath + ("PH");          // เช่น D:\FileImg\
                string baseUrl = Properties.Settings.Default.AbsolutePathImage + ("PH");              // เช่น http://yourdomain.com/FileImg/

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
                string fileUrl = Url.Action("Image", "Underwrite", new { yearMonth = yearMonth, queueId = queueId, fileName = fileName }, Request.Url.Scheme);
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
                string path = Properties.Settings.Default.FileImgPath + String.Format("\\PH\\{0}\\QueueID_{1}", yearMonth, queueId);

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

        #endregion api

        #region common api

        public ActionResult GetProvince()
        {
            var result = _context.usp_Province_Select().ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDistrict(int provinceId)
        {
            var result = _context.usp_DistrictByProvinceId_Select(provinceId).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion common api
    }
}