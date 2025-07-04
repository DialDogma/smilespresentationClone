using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using SmileSBillPayment.DataCenterAddressService;
using SmileSBillPayment.Helper;
using SmileSBillPayment.Models;
using OfficeOpenXml.Style;
using System.Drawing;
using SmileSBillPayment.SSOService;

namespace SmileSBillPayment.Controllers
{
    [Authorization]
    public class SMSController : Controller
    {
        #region Context

        private readonly SSSCashReceiveEntities _context;

        public SMSController()
        {
            _context = new SSSCashReceiveEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Context

        #region View

        public ActionResult SMSFailPerson()
        {
            var empId = GlobalFunction.GetLoginDetail(HttpContext).Employee_ID;
            ViewBag.empId = empId;

            return View();
        }

        public ActionResult SMSFailBranch()
        {
            //Get Role
            var user = GlobalFunction.GetLoginDetail(HttpContext);
            var roleListRaw = new SSOServiceClient().GetRoleByUserName(user.UserName);
            var _dcService = new AddressServiceClient();
            var arrRole = roleListRaw.Split(',');
            if (arrRole.Contains("Developer") || arrRole.Contains("Billpayment_Premium") || arrRole.Contains("Billpayment_Callcenter"))
            {
                ViewBag.branchList = _dcService.GetBranch(null).ToList();
            }
            else
            {
                ViewBag.branchList = _dcService.GetBranch(user.Branch_ID).ToList();
            }

            _dcService.Close();
            return View();
        }

        /// <summary>
        /// Monitor SMS
        /// </summary>
        /// <returns></returns>
        [Authorization(Roles = "Developer")]
        public ActionResult MonitorSMS()
        {
            return View();
        }

        #endregion View

        #region api

        public ActionResult GetSMSFail_dt(string period, int? empId = null, int? payerBranchId = null
            , int? draw = null, int? indexStart = null, int? pageSize = null,
            string sortField = null, string orderType = null, string searchDetail = null)
        {
            //var saleEmpolyeeId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

            if (payerBranchId == 0)
            {
                payerBranchId = null;
            }

            DateTime? periodConverted = null;
            if (period != "")
            {
                periodConverted = DateTime.ParseExact(period, "dd-MM-yyyy", new CultureInfo("en-Us"));
            }

            //var result = _context.usp_BillPayment_Select(null,periodConverted,payerBranchId,null,empId,null,null,
            //    null,"5,8,9,10",indexStart,pageSize,sortField,orderType,searchDetail).ToList();
            var result = _context.usp_BillPaymentForSMSFail_Select(periodConverted, payerBranchId, null, empId, indexStart,
                pageSize, sortField, orderType, searchDetail).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> ExportSMSFail(string period, int? payerBranchId = null)
        {
            await Task.Yield();

            int? saleEmpolyeeId = null;

            if (payerBranchId == null)
            {
                saleEmpolyeeId = GlobalFunction.GetLoginDetail(HttpContext).Employee_ID;
            }

            DateTime? periodConverted = null;
            if (period != "")
            {
                periodConverted = DateTime.ParseExact(period, "dd-MM-yyyy", new CultureInfo("en-Us"));
            }
            if (payerBranchId == 0)
            {
                payerBranchId = null;
            }

            var result = _context.usp_BillPaymentForSMSFail_Select(periodConverted, payerBranchId, null, saleEmpolyeeId
                , 0, 99999, null, null, null).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(result, true);

                var headerCells = workSheet.Cells[1, 1, 1, workSheet.Dimension.Columns];
                headerCells.Style.Font.Bold = true;
                headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerCells.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                var allCells = workSheet.Cells[workSheet.Dimension.Address];
                allCells.AutoFilter = true;
                allCells.AutoFitColumns();
                package.Save();

                stream.Position = 0;
                //get new GUID
                var dataSessionKey = Guid.NewGuid().ToString();
                //tempData GUID
                TempData["keyReport"] = dataSessionKey;
                //Session Data
                Session[dataSessionKey] = package.GetAsByteArray();

                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DownloadExcelReport()
        {
            var dataKey = TempData["keyReport"].ToString();

            if (Session[dataKey] != null)
            {
                byte[] data = Session[dataKey] as byte[];
                string excelName = $"Report-SMS-fail-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        #endregion api
    }
}