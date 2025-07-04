using SmileSBillPayment.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SmileSBillPayment.Helper;

using SmileSBillPayment.Models;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace SmileSBillPayment.Controllers
{
    [Authorization]
    public class ReserveAccountController : Controller
    {
        #region Context

        private readonly SSSCashReceiveEntities _context;

        public ReserveAccountController()
        {
            _context = new SSSCashReceiveEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Context

        #region View

        // GET: ReserveAccount
        [Authorization(Roles = "Developer,Billpayment_Premium")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: ReserveAccountDetail
        public ActionResult Detail(string reserveAccountId)
        {
            try
            {
                if (reserveAccountId != null && reserveAccountId != "")
                {
                    var base64EncodedBytes = Convert.FromBase64String(reserveAccountId);

                    reserveAccountId = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

                    var result = _context.usp_ReserveAccount_Select(Convert.ToInt32(reserveAccountId), null, null, null, null, null, null, null, null).SingleOrDefault();
                    if (result != null)
                    {
                        ViewBag.ReserveAccount = result;
                    }
                    else
                    {
                        TempData["errormsg"] = "ไม่พบรายละเอียด AccountID นี้";
                        return RedirectToAction("InternalServerError", "Error");
                    }
                }
                else

                {
                    TempData["errormsg"] = "ไม่พบรายละเอียด AccountID นี้";
                    return RedirectToAction("InternalServerError", "Error");
                }
                return View();
            }
            catch (Exception e)
            {
                TempData["errormsg"] = e.Message;
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        #endregion View

        #region Datatable

        /// <summary>
        /// GetReserveAccount
        /// </summary>
        /// <param name="reserveAccountId"></param>
        /// <param name="reserveAccountTypeId"></param>
        /// <param name="isDummy"></param>
        /// <param name="isBalance"></param>
        /// <param name="searchDetail"></param>
        /// <param name="draw"></param>
        /// <param name="indexStart"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetReserveAccount(int? reserveAccountId, int? reserveAccountTypeId, bool? isDummy, bool? isBalance, string searchDetail = null, int? draw = null, int? indexStart = 0, int? pageSize = 10, string sortField = null, string orderType = null)
        {
            ////var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            //if (branchId == 0)
            //{
            //    branchId = null;
            //}
            //if (appStatusIdList == "")
            //{
            //    appStatusIdList = null;
            //}
            //if (appUdwStatusIdList == "")
            //{
            //    appUdwStatusIdList = null;
            //}
            ////check if null
            //DateTime? startCoverDateConvert = null;
            //if (startCoverDate != "")
            //{
            //    startCoverDateConvert = DateTime.ParseExact(startCoverDate, "dd-MM-yyyy", new CultureInfo("en-Us"));
            //}

            //DateTime? cancelDateConvert = null;
            //if (cancelDate != "")
            //{
            //    cancelDateConvert = DateTime.ParseExact(cancelDate, "dd-MM-yyyy", new CultureInfo("en-Us"));
            //}

            //DateTime? endCoverDateConvert = null;
            //if (endCoverDate != "")
            //{
            //    endCoverDateConvert = DateTime.ParseExact(endCoverDate, "dd-MM-yyyy", new CultureInfo("en-Us"));
            //}
            if (isBalance == false)
            {
                isBalance = null;
            }

            var result = _context.usp_ReserveAccountShort_Select(reserveAccountId,
                                                            reserveAccountTypeId,
                                                            isBalance,
                                                            indexStart,
                                                            pageSize,
                                                            sortField,
                                                            orderType,
                                                            searchDetail).ToList();

            var dt = new Dictionary<string, object>
                {
                    {"draw",draw },
                    {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                    {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                    {"data", result.ToList()}
                };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetReserveAccountTransaction
        /// </summary>
        /// <param name="reserveAccountId"></param>
        /// <param name="draw"></param>
        /// <param name="indexStart"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetReserveAccountTransaction(int reserveAccountId, int? draw = null, int? indexStart = null, int? pageSize = null, string sortField = null, string orderType = null)
        {
            var result = _context.usp_ReserveAccountTransaction_Select(null, reserveAccountId, indexStart,
                pageSize, sortField, orderType, null).ToList();

            var dt = new Dictionary<string, object>
                {
                    {"draw",draw },
                    {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                    {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                    {"data", result.ToList()}
                };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        #endregion Datatable

        #region Export

        /// <summary>
        /// ExportExcelReserveAccount
        /// </summary>
        /// <param name="reserveAccountId"></param>
        /// <param name="reserveAccountTypeId"></param>
        /// <param name="isDummy"></param>
        /// <param name="isBalance"></param>
        /// <param name="searchDetail"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorization(Roles = "Developer,Billpayment_Premium")]
        public async Task<ActionResult> ExportExcelReserveAccount(int? reserveAccountId, int? reserveAccountTypeId, bool? isDummy, bool? isBalance, string searchDetail = null)
        {
            await Task.Yield();
            var lst = _context.usp_ReserveAccount_Select(reserveAccountId, reserveAccountTypeId, isDummy, isBalance, 0,
             999999999, null, null, searchDetail).ToList();//.Select(x => new { xxx = x.ReserveAccountCode, กกก = x.ReferenceDetail })

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet1 = package.Workbook.Worksheets.Add("ReserveAccount");

                workSheet1.Cells.LoadFromCollection(lst, true);

                // Select only the header cells
                var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                // Set their text to bold.
                headerCells1.Style.Font.Bold = true;
                // Set their background color
                headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerCells1.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(54, 127, 169));
                headerCells1.Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                // Apply the auto-filter to all the columns
                var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                allCells1.AutoFilter = true;
                // Auto-fit all the columns
                allCells1.AutoFitColumns();

                package.Save();

                stream.Position = 0;

                //get new GUID
                var dataSessionKey = Guid.NewGuid().ToString();
                //tempData GUID
                TempData["dataKey"] = dataSessionKey;
                //Session Data
                Session[dataSessionKey] = package.GetAsByteArray();

                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Download Export Excel File
        /// </summary>
        /// <returns></returns>
        public ActionResult DownloadExcelReserveAccount()
        {
            var dataKey = TempData["dataKey"].ToString();

            if (Session[dataKey] != null)
            {
                byte[] data = Session[dataKey] as byte[];
                string excelName = $"Report-ReserveAccount-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        #endregion Export
    }
}