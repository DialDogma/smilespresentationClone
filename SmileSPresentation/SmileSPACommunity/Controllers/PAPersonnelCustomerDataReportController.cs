using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Wordprocessing;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using SmileSPACommunity.Helper;
using SmileSPACommunity.Models;
using SmileSPACommunity.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace SmileSPACommunity.Controllers
{
    [Authorization]
    public class PAPersonnelCustomerDataReportController : Controller
    {
        private PACommunityEntities DB;
        public PAPersonnelCustomerDataReportController()
        {
            DB = new PACommunityEntities();
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Organize = DB.usp_Organize_Select(null, null, 2, null, 0, 999999, null, null, null).Where(s => s.OrganizeId == 389190).ToList();
            return View("Index");
        }

        [HttpPost]
        public ActionResult GetPersonalReported(int draw, DateTime startDate, DateTime endDate, int? pageSize = null, int? indexStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var results = DB.usp_PersonnelEndorseCustomerReportGroup_Select(startDate, endDate, indexStart, pageSize, sortField, orderType, search).ToList();

            return Json(new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", results.Count() != 0 ? results.FirstOrDefault().TotalCount : results.Count() },
                {"recordsFiltered", results.Count() != 0 ? results.FirstOrDefault().TotalCount : results.Count() },
                {"data", results.ToList()}
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetPersonalWaitReport(int insuranceId)
        {
            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            try
            {
                var results = DB.usp_PersonnelEndorseCustomerReportGroup_Insert(insuranceId, userId).FirstOrDefault();
                if (results.IsResult == false)
                {
                    return Json(new { valid = false, message = results.Msg });
                }
            }
            catch (Exception error)
            {
                return Json(new { valid = false, message = error.InnerException == null ? error.Message : error.InnerException.Message });
            }

            return Json(new { valid = true });
        }

        [HttpPost]
        public ActionResult loadDataReportCreated(int draw, int insuranceId, int? indexStart = null, int? pageSize = null, string sortField = null, string orderType = null, string search = null)
        {
            var results = DB.usp_PersonnelEndorseCustomerForGenReport_Select(insuranceId, indexStart, pageSize, sortField, orderType, search).ToList();
            return Json(new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", results.Count() != 0 ? results.FirstOrDefault().TotalCount : results.Count() },
                {"recordsFiltered", results.Count() != 0 ? results.FirstOrDefault().TotalCount : results.Count() },
                {"data", results.ToList()}
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public void ExportPersonalDataExcel(int PersonnelEndorseCustomerReportGroupId)
        {
            var data_sheet1 = DB.usp_PersonnelEndorseCustomerReportDetail_Select(PersonnelEndorseCustomerReportGroupId).Select((s, Index) => new
            {
                ลำดับ = Index + 1,
                รหัสรายการ = s.PersonnelEndorseCustomerReportHeaderCode,
                ชื่อสถานศึกษา = s.SchoolName,
                จังหวัด = s.Province,
                AppID = s.PersonnelApplicationCode,
                รหัสสถานศึกษา = s.SchoolId,
                รหัสผู้เอาประกัน = s.PersonnelCustomerDetailCode,
                คำนำหน้าเดิม = s.FromTitle,
                คำนำหน้าใหม่ = s.ToTitle,
                ชื่อเดิม = s.FromFirstName,
                ชื่อใหม่ = s.ToFirstName,
                นามสกุลเดิม = s.FromLastName,
                นามสกุลใหม่ = s.ToLastName,
                เลขบัตรเดิม = s.FromIdCard,
                เลขบัตรใหม่ = s.ToIdCard,
                เลขPassportเดิม = s.FromPassport,
                เลขPassportใหม่ = s.ToPassport,
                ตำแหน่งเดิม = s.FromPosition,
                ตำแหน่งใหม่ = s.ToPosition,
                วันที่ทำรายการ = Convert.ToDateTime(s.ApprovedDate).ToString("dd/MM/yyyy HH:mm:ss"),
                วันที่มีผล = Convert.ToDateTime(s.EffectiveDate).ToString("dd/MM/yyyy")
            }).ToList();

            // set report name
            string reportName = "ED" + data_sheet1.FirstOrDefault().รหัสรายการ;
            var dataTable1 = GlobalFunction.ToDataTable(data_sheet1);
            ExcelManager.ExportToExcel(this.HttpContext, reportName, dataTable1, "รายงานแก้ไขรายชื่อผู้เอาประกัน");
        }

        [HttpPost]
        public ActionResult UpdateEndorseCustomerReportGroup(int statusId, int reportGroupId)
        {
            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            try
            {
                var result = DB.usp_PersonnelEndorseCustomerReportGroup_Update(reportGroupId, statusId, userId).FirstOrDefault();
                if (result.IsResult == false)
                {
                    return Json(new { valid = false, message = result.Msg });
                }
            }
            catch (Exception error)
            {
                return Json(new { valid = false, message = (error.InnerException == null ? error.Message : error.InnerException.Message) });
            }

            return Json(new { valid = true });
        }

    }
}