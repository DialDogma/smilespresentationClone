using Newtonsoft.Json;
using OfficeOpenXml;
using PayTransferAPI.Contract;
using RestSharp;
using Serilog;
using SmileSClaimOnLine.DTOs;
using SmileSClaimOnLine.EmployeeService;
using SmileSClaimOnLine.Helper;
using SmileSClaimOnLine.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.IO;
using OfficeOpenXml.Style;
using DocumentFormat.OpenXml.Spreadsheet;
using SmileSClaimOnLine.Consumers;
using System.Data.Entity;
using MassTransit;

namespace SmileSClaimOnLine.Controllers
{
    [Authorization]
    public class ClaimOnLineController : Controller
    {
        #region dbContext

        private ClaimOnLineDBContext _context;

        private LoginDetail _loginDetail;
        private const string _controllerName = nameof(ClaimOnLineController);
        private const string _exportToExcelSuccessFul = "Export To Excel สำเร็จ";
        private const string _dataNotFound = "ไม่พบข้อมูล";
        public ClaimOnLineController()
        {
            _context = new ClaimOnLineDBContext();
            //_loginDetail = GlobalFunction.GetLoginDetail(this.HttpContext);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion dbContext

        // GET: ClaimOnLine
        public ActionResult Index()
        {
            return View();
        }

        #region "View_NewClaimonLine"

        public ActionResult ConsiderTransferPremium(string claimOnlineId = null, string claimOnLineCode = null)
        {
            var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;
            var role = OAuth2Helper.GetRoles();
            var rolelist = role.Split(',').ToList();

            var roleOperation = new[] { "SmileSClaimOnLine_Operation" }; //MO
            var roleDeveloper = new[] { "Developer" }; //Dev

            if (rolelist.Intersect(roleOperation).Count() > 0)
                ViewBag.RoleOperation = true;
            else
                ViewBag.RoleOperation = false;

            if (rolelist.Intersect(roleDeveloper).Count() > 0)
                ViewBag.RoleDeveloper = true;
            else
                ViewBag.RoleDeveloper = false;

            string ClaimOnLineID = null;
            if (claimOnLineCode != null)
            {
                //DeCode
                var ClaimOnlineCodeBase64EncodedBytes = Convert.FromBase64String(claimOnLineCode);
                claimOnLineCode = System.Text.Encoding.UTF8.GetString(ClaimOnlineCodeBase64EncodedBytes);
                ClaimOnLineID = _context.ClaimOnLine.Where(_ => _.ClaimOnLineCode == claimOnLineCode).Select(_ => _.ClaimOnLineId).FirstOrDefault().ToString();
            }
            else
            {
                //DeCode
                var ClaimOnlineIdBase64EncodedBytes = Convert.FromBase64String(claimOnlineId);
                ClaimOnLineID = System.Text.Encoding.UTF8.GetString(ClaimOnlineIdBase64EncodedBytes);
            }

            var resultBankAccount = _context.BankAccount
                                                                              .OrderByDescending(ba => ba.UpdatedDate)
                                                                              .Select(ba => new
                                                                              {
                                                                                  ba.Balance,
                                                                                  ba.UpdatedDate
                                                                              })
                                                                              .FirstOrDefault();

            ViewBag.ClaimOnLine_Id = Convert.ToInt32(ClaimOnLineID);
            ViewBag.userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            ViewBag.CancelCause = _context.usp_CancelCause_Select(null, true).ToList();
            ViewBag.AccountBalance = ViewBag.RoleOperation == true ? "0.00" : resultBankAccount.Balance.ToString("#,##0.00");
            ViewBag.AccountBalanceLastUpdate = resultBankAccount.UpdatedDate.ToString("dd/MM/yyyy HH:mm:ss");
            ViewBag.TransferAmountLimit = _context.TransferAmountLimit.FirstOrDefault().TransferAmountLimit1.Value;

            ViewBag.AllowTransferAmountLimit = CheckAllowTransferAmountLimit();

            return View();
        }

        public bool CheckAllowTransferAmountLimit()
        {
            var employeeCode = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            var env = Properties.Settings.Default.Environment;
            return _context.EmployeeXTransferAmountLimit.Any(x => x.IsActive == true && x.EmployeeCode == employeeCode);
        }

        #endregion "View_NewClaimonLine"

        public ActionResult ClaimOnLine()
        {
            int employeeID = _loginDetail.Employee_ID;

            // ลดการ Run store ซ้ำ 2 ครั้ง โดยไม่จำเป็น
            var zoneXEmployee = _context.usp_ZoneXEmployee_Select(employeeID, true);

            int c = zoneXEmployee.Count();

            // ใช้ if แค่รอบเดียว
            int? zoneId = null;
            if (c > 0) zoneId = Convert.ToInt32(zoneXEmployee.SingleOrDefault().ZoneId);

            ViewBag.Branch = _context.usp_Branch_Select(null, zoneId, true).ToList();

            ViewBag.PayeeType = _context.usp_PayeeType_Select(true).ToList().Where(x => x.PayeeTypeId != 2);

            // ย้าย store product type ไป รวมที่เดียว ใช้ Config เดียว
            ViewBag.ProductType = ProductTypeSelect();

            ViewBag.Bank = BankSelect();

            return View();
        }

        private List<usp_ProductType_Select_Result> ProductTypeSelect()
        {
            return _context.usp_ProductType_Select(null, null).ToList().Where(x => x.ProductType_ID == 6 || x.ProductType_ID == 7 || x.ProductType_ID == 8 || x.ProductType_ID == 26).ToList();
        }

        public ActionResult MonitorClaimOnLine()
        {
            ViewBag.TransferType = _context.usp_TransferType_Select(true).ToList().Where(x => x.IsWithdraw == true);

            // Standard CSharp ตัวแปร ขึ้นต้นด้วยตัวเลขทุกครั้ง
            int employeeId = GlobalFunction.GetLoginDetail(this.HttpContext).Employee_ID;

            // รัน Store ซ้ำ โดยไม่จำเป็น
            // ทำงานซ้ำ สร้าง Method แยก
            ViewBag.Account = ClaimOnlineAccountSelectByEmployeeId(employeeId);

            ViewBag.Zone = _context.usp_Zone_Select(null, true).ToList();

            ViewBag.Bank = BankSelect();

            return View();
        }

        private List<usp_ClaimOnLineAccount_Select_Result> ClaimOnlineAccountSelectByEmployeeId(int employeeId)
        {
            var result = _context.usp_ClaimOnLineAccount_Select(null, employeeId, true).ToList();

            if (!result.Any()) result = _context.usp_ClaimOnLineAccount_Select(null, null, true).ToList();

            return result;
        }

        public ActionResult MonitorForEditClaimOnLine()
        {
            ViewBag.TransferType = _context.usp_TransferType_Select(true).ToList().Where(x => x.IsWithdraw == true);

            int EmployeeId = GlobalFunction.GetLoginDetail(this.HttpContext).Employee_ID;

            ViewBag.Account = ClaimOnlineAccountSelectByEmployeeId(EmployeeId);

            ViewBag.Zone = _context.usp_Zone_Select(null, true).ToList();

            return View();
        }

        public ActionResult CancelClaimOnLine()
        {
            ViewBag.Zone = _context.usp_Zone_Select(null, true).ToList();

            ViewBag.CancelCause = _context.usp_CancelCause_Select(null, true).ToList();

            return View();
        }

        [Authorization(Roles = "Developer")]
        public ActionResult TransferEndorse()
        {
            //new

            //old code
            ViewBag.TransferType = _context.usp_TransferType_Select(true).ToList();
            ViewBag.Zone = _context.usp_Zone_Select(null, true).ToList();
            ViewBag.Bank = BankSelect();

            return View();
        }

        //ยกเลิกรับเงินคืนจากกองทุน
        public ActionResult CancelClaimOnLineTransferItem()
        {
            return View();
        }

        [Authorization(Roles = "Developer")]
        public ActionResult COLEndorse()
        {
            return View();
        }

        [Authorization(Roles = "Developer,SmileSClaimOnLine_Manager,SmileSClaimOnLine_Supervisor")]
        public ActionResult ClaimOnLineMonitoring()
        {
            var user = GlobalFunction.GetLoginDetail(this.HttpContext);
            var username = user.UserName;
            var userId = user.User_ID;
            var role = OAuth2Helper.GetRoles();
            var rolelist = role.Split(',').ToList();
            var roleOperation = new[] { "SmileSClaimOnLine_Operation" }; //MO
            var roleDeveloper = new[] { "Developer" }; //Dev

            if (rolelist.Intersect(roleOperation).Count() > 0)
                ViewBag.RoleOperation = true;
            else
                ViewBag.RoleOperation = false;

            if (rolelist.Intersect(roleDeveloper).Count() > 0)
                ViewBag.RoleDeveloper = true;
            else
                ViewBag.RoleDeveloper = false;

            //06074 20220429 Add bank
            ViewBag.Bank = BankSelect();
            ViewBag.Zone = _context.usp_Area_Select(null, 0, 999999, null, null, null).ToList();

            var resultArea = _context.usp_AreaXEmployeeUserId_Select(userId, 0, 999999, null, null, null).SingleOrDefault();
            var resultBankAccount = _context.BankAccount
                      .OrderByDescending(ba => ba.UpdatedDate)
                      .Select(ba => new
                      {
                          ba.Balance,
                          ba.UpdatedDate
                      })
                      .FirstOrDefault();

            ViewBag.DefaultZone = 1;
            ViewBag.AccountBalance = resultBankAccount.Balance.ToString("#,##0.00");
            ViewBag.AccountBalanceLastUpdate = resultBankAccount.UpdatedDate.ToString("dd/MM/yyyy HH:mm:ss");
            var TransferAmountLimit = _context.TransferAmountLimit.FirstOrDefault().TransferAmountLimit1;
            ViewBag.TransferAmountLimit = TransferAmountLimit;
            ViewBag.TextTransferAmountLimit = TransferAmountLimit.Value.ToString("#,##0.00");
            ViewBag.AllowTransferAmountLimit = CheckAllowTransferAmountLimit();

            if (resultArea != null) ViewBag.DefaultZone = resultArea.AreaId;

            return View();
        }

        private IEnumerable<usp_Bank_Select_Result> BankSelect()
        {
            return _context.usp_Bank_Select(null, null, true).ToList().Where(x => x.Bank_ID != 2);
        }

        #region "Method"

        public ActionResult GetZebraCar(string q)
        {
            var rs = _context.usp_GetEmployeeDetailINZebraCar_Select(null, 0, 9999, null, null, q).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveForClaimOnLineRemarkDetail(int claimOnLineId, string remark, int remarkByUserId)
        {
            //int claimOnLineId,string remark,int remarkByUserId

            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            string[] rs = new string[3];

            var result = _context.usp_ClaimOnLineRemark_Insert(claimOnLineId, remark, remarkByUserId, userId).Single();

            rs[0] = result.IsResult.Value.ToString();
            rs[1] = result.Result;
            rs[2] = result.Msg;

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClaimOnLineRemark(string claimOnLineId)
        {
            var con_claimonlineID = Convert.ToInt32(claimOnLineId);

            // Run store ซ้ำ โดยไม่จำเป็น
            var claimOnlineRemarks = _context.usp_ClaimOnLineRemark_Select(con_claimonlineID, 0, 999, null, null, null);

            if (claimOnlineRemarks.Any()) return Json(false, JsonRequestBehavior.AllowGet);

            var rs = claimOnlineRemarks.Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployeeService2Detail(int q)
        {
            var rs = _context.usp_GetEmployeeDetailAll_Select(q, 0, 999, null, null, null).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployeeService2(string q)
        {
            var rs = _context.usp_GetEmployeeDetailAll_Select(null, 0, 20, null, null, q).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBranchByZoneId(int? zoneId)
        {
            if (zoneId == -1)
            {
                zoneId = null;
            }

            var result = _context.usp_Branch_Select(null, zoneId, true).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetZoneByEmployee()
        {
            int employeeID = GlobalFunction.GetLoginDetail(this.HttpContext).Employee_ID;

            var zoneXEmployee = _context.usp_ZoneXEmployee_Select(employeeID, true);

            // ใช้ if แบบไม่ต้องใช้ else
            var result = 0;
            if (zoneXEmployee.Any()) result = Convert.ToInt32(zoneXEmployee.SingleOrDefault().ZoneId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditClaimOnLine(int claimOnlineId)
        {
            ViewBag.ClaimOnLineId = claimOnlineId;

            ViewBag.ClaimOnLineCode = _context.usp_ClaimOnLine_Select(claimOnlineId, true).SingleOrDefault().ClaimOnLineCode;

            ViewBag.Branch = _context.usp_Branch_Select(null, null, true).ToList();

            ViewBag.PayeeType = _context.usp_PayeeType_Select(true).ToList().Where(x => x.PayeeTypeId != 2);

            ViewBag.ProductType = ProductTypeSelect();

            ViewBag.Bank = BankSelect();

            return View();
        }

        public ActionResult UpdateClaimOnLine(int productGroupID, string detail, int claimCount, int branchId, int serviceByUserId, int noticeByUserId, string accountNo, string accountName, int payeeTypeId, int claimOnlineId, int bankId, int serviceByEmpId, int zebraCarOwnerEmpId)
        {
            int userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            try
            {
                var claimOnLineCode = _context.usp_ClaimOnLine_Update(claimOnlineId, productGroupID, detail, claimCount, branchId, serviceByUserId, noticeByUserId, serviceByEmpId, payeeTypeId, bankId, accountNo, accountName, userID, zebraCarOwnerEmpId).SingleOrDefault().ClaimOnLineCode;

                return Json(claimOnLineCode, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetBranchByAreaId(int? areaId)
        {
            if (areaId == -1) areaId = null;

            var result = _context.usp_BranchByAreaId_Select(null, areaId, null, 0, 9999999, null, null, null).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TransferEndorseAmount(string amount, int COLTransferId)
        {
            try
            {
                //collect data
                var amountResult = Convert.ToDouble(amount);

                var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

                var result = _context.usp_ClaimOnLineTransfer_Amount_Update(COLTransferId, amountResult, userID);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                // คืออะไร??
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorization(Roles = "Developer")]
        public JsonResult SaveClaimOnLine(int productGroupID, string detail, int claimCount, int branchId, int serviceByUserId, int noticeByUserId, string accountNo, string accountName, int payeeTypeId, int bankId, int serviceByEmpId, int zebarCarOwnerByEmpId)
        {
            int userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            try
            {
                var claimOnLineCode = _context.usp_ClaimOnLine_Insert(productGroupID, detail, claimCount, branchId, serviceByUserId, noticeByUserId, serviceByEmpId, payeeTypeId, bankId, accountNo, accountName, userID, zebarCarOwnerByEmpId).SingleOrDefault().ClaimOnLineCode;

                return Json(claimOnLineCode, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult CancelClaimOnLine(FormCollection form)
        {
            int userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var claimOnLineId = Convert.ToInt32(form["hdfClaimOnLineId"]);

            var cancelCauseId = Convert.ToInt32(form["ddlCancelCause"]);

            var remark = form["txtRemark"];

            try
            {
                _context.usp_ClaimOnLineCancel_Update(claimOnLineId, cancelCauseId, remark, userID);

                return RedirectToAction("CancelClaimOnLine", "ClaimOnLine");
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorization(Roles = "Developer")]
        public ActionResult SaveClaimOnLineTransfer(string f_claimOnlineId, string claimOnLineAccount, string transferTypeId, string bankId, string accountNo, string accountName
                                                    , string amount, string f_transferDate, string transferTime)
        {
            // transferType
            var intTransferType = Convert.ToInt32(transferTypeId);

            // tfType ตั้งชื่อให้สื่อ
            var transferType = _context.usp_TransferType_Select(true).Where(x => x.TransferTypeId == intTransferType).SingleOrDefault();

            var payeeBankId = Convert.ToInt32(bankId);
            var payeeType = Convert.ToInt32(transferType.PayeeTypeId);

            // reassign โดยไม่จำเป็น
            //var payeeAccountNo = accountNo;
            //var payeeAccountName = accountName;

            var convertAmount = Convert.ToDouble(amount.Replace(",", ""));
            //var tdate = f_transferDate;

            // ข้อมูลเข้ามาเฉพาะ ชม:นาที เพิ่ม วินาที
            transferTime += ":00";

            var transferDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(f_transferDate, transferTime));

            var claimOnLineId = Convert.ToInt32(f_claimOnlineId);

            int payerType = Convert.ToInt32(transferType.PayerTypeId);

            int claimOnLineAccountId = Convert.ToInt32(claimOnLineAccount);

            var claimAccount = _context.usp_ClaimOnLineAccount_Select(claimOnLineAccountId, null, true).SingleOrDefault();

            var payerBankId = claimAccount.BankId;

            string fromAccountNo = claimAccount.AccountNo;
            string fromAccountName = claimAccount.AccountName;

            bool isActive = true;

            int userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            try
            {
                //Save
                var result = _context.usp_ClaimOnLineTransfer_Insert(claimOnLineId, intTransferType, payerType, payerBankId, fromAccountNo, fromAccountName, payeeType, payeeBankId, accountNo, accountName, transferDate, convertAmount, claimOnLineAccountId, isActive, userID, null);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetClaimOnlineMonitorVersion1(string dateFrom, string dateTo, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null, int? branchId = null, int? zoneId = null, string claimOnLineCode = null)
        {
            DateTime? d_dateFrom = GlobalFunction.ConvertDatePickerToDate_BE(dateFrom);
            DateTime? d_dateTo = GlobalFunction.ConvertDatePickerToDate_BE(dateTo);

            var result = _context.usp_ClaimOnLineMonitorVersion1_New_Select(d_dateFrom, d_dateTo, zoneId == -1 ? null : zoneId, branchId == -1 ? null : branchId, claimOnLineCode, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMonitorClaimOnLine(string dateFrom, string dateTo, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null, int? branchId = null, int? zoneId = null)
        {
            DateTime? d_dateFrom = GlobalFunction.ConvertDatePickerToDate_BE(dateFrom);
            DateTime? d_dateTo = GlobalFunction.ConvertDatePickerToDate_BE(dateTo);

            if (branchId == -1) branchId = null;
            if (zoneId == -1) zoneId = null;

            var result = _context.usp_ClaimOnLineMonitor_Select(null, d_dateFrom, d_dateTo, zoneId, branchId, true, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMonitorClaimOnLineV2(string statusId = null,
            int? claimOnlineId = null,
            bool isShowAll = true,
            bool isShowBranchData = false,
            int? bankId = null, //06074 20220429 Add Bank parameter.
            int? branchId = null,
            int? areaId = null,
            int? draw = null,
            int? pageSize = null,
            int? pageStart = null,
            string sortField = null,
            string orderType = null,
            string search = null,
            string dateSearchMode = null,
            string dateFrom = null,
            string dateTo = null)
        {
            //isShowAll คือ ถ้าแสดงของทุกสาขาใช่ไหม defult คือ true ถ้า false คือแสดงสาขาตัวเองโดยใส่ useridไป
            int? userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            int? mode = null;
            DateTime? cDateFrom = null;
            DateTime? cDateTo = null;

            // ไม่ได้ใช้แล้ว
            if (dateSearchMode != null) mode = Convert.ToInt32(dateSearchMode);
            if (dateFrom != null && dateFrom != "-1") cDateFrom = Convert.ToDateTime(dateFrom);
            if (dateTo != null && dateTo != "-1") cDateTo = Convert.ToDateTime(dateTo);
            if (isShowBranchData)
            {
                isShowAll = true;
                branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
            }

            //06958 2022-09-14 implement storedprocedure
            var result = _context.usp_ClaimOnLineVersion2_1_Select(mode,
                                                                 cDateFrom,
                                                                 cDateTo,
                                                                 statusId == "" ? null : statusId,
                                                                 branchId == -1 ? null : branchId,
                                                                 areaId == -1 ? null : areaId,
                                                                 bankId == -1 ? null : bankId,
                                                                 isShowAll == true ? null : userId,
                                                                 pageStart,
                                                                 pageSize,
                                                                 sortField,
                                                                 orderType,
                                                                 search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMonitorClaimOnLineV3(string statusId = null,
            int? claimOnlineId = null,
            bool isShowAll = true,
            bool isShowBranchData = false,
            int? bankId = null,
            int? branchId = null,
            int? areaId = null,
            int? draw = null,
            int? pageSize = null,
            int? pageStart = null,
            string sortField = null,
            string orderType = null,
            string search = null,
            string dateSearchMode = null,
            string dateFrom = null,
            string dateTo = null,
            string optionsRadiosOnProcess = null)
        {
            //isShowAll คือ ถ้าแสดงของทุกสาขาใช่ไหม defult คือ true ถ้า false คือแสดงสาขาตัวเองโดยใส่ useridไป
            int? userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var employeeCode = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            var env = Properties.Settings.Default.Environment;

            int? mode = null;
            DateTime? cDateFrom = null;
            DateTime? cDateTo = null;

            if (dateSearchMode != null) mode = Convert.ToInt32(dateSearchMode);
            if (dateFrom != null && dateFrom != "-1") cDateFrom = Convert.ToDateTime(dateFrom);
            if (dateTo != null && dateTo != "-1") cDateTo = Convert.ToDateTime(dateTo);
            if (isShowBranchData)
            {
                isShowAll = true;
                branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
            }

            bool isShowTransferAmountLimit = false;
            //optionsRadiosOnProcess == 1 คือ แสดงยอด ≥ จำนวนเงินโอนที่ตั้งไว้
            if (optionsRadiosOnProcess == "1") isShowTransferAmountLimit = true;


            var result = usp_ClaimOnLineVersion3_1_Select(mode, cDateFrom, cDateTo, statusId == "" ? null : statusId, branchId == -1 ? null : branchId, areaId == -1 ? null : areaId, bankId == -1 ? null : bankId, isShowAll == true ? null : userId, isShowTransferAmountLimit, pageStart, pageSize, sortField, orderType, search.Trim());

            //sum ยอดเงินค้างโอน , statusId 2 รอดำเนินการ
            ActionResult resultTransferAmountTotal = null;
            if (statusId == "2")
            {
                resultTransferAmountTotal = GetClaimOnLineTransfer_OutstandingTotal(mode, cDateFrom, cDateTo, statusId, branchId, areaId, bankId, isShowAll, isShowTransferAmountLimit, pageStart, pageSize, search.Trim());
            }

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()},
                {"transferAmountTotal", resultTransferAmountTotal}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        private List<usp_ClaimOnLineVersion3_1_Select_Result> usp_ClaimOnLineVersion3_1_Select(int? mode, DateTime? cDateFrom, DateTime? cDateTo, string statusId, int? branchId, int? areaId, int? bankId, int? isShowAll, bool? isShowTransferAmountLimit, int? pageStart, int? pageSize, string sortField, string orderType, string search)
        {
            return _context.usp_ClaimOnLineVersion3_1_Select(mode,
                                                   cDateFrom,
                                                   cDateTo,
                                                   statusId,
                                                   branchId,
                                                   areaId,
                                                   bankId,
                                                   isShowAll,
                                                   isShowTransferAmountLimit,
                                                   pageStart,
                                                   pageSize,
                                                   sortField,
                                                   orderType,
                                                   search).ToList();
        }

        public JsonResult GetMonitorTransferLog(int claimOnLineId)
        {
            var result = _context.usp_ClaimOnLineTransfer_Select(null, claimOnLineId, true).ToList();

            var dt = new Dictionary<string, object>
            {
                //{"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.ToList().Count : result.ToList().Count()},
                {"recordsFiltered", result.Count() != 0 ? result.ToList().Count : result.ToList().Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClaimOnLineSelect(int claimOnLineId)
        {
            var result = _context.usp_ClaimOnLine_Select(claimOnLineId, true).SingleOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClaimOnLineAccountById(int ClaimOnLineAccountId)
        {
            string result = "0";
            if (ClaimOnLineAccountId != -1)
                result = _context.usp_ClaimOnLineAccount_Select(ClaimOnLineAccountId, null, true).SingleOrDefault().Balance.ToString();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchCOL(string COLCOde)
        {
            COLCOde = COLCOde.Trim();

            //get COL id
            var result = _context.usp_ClaimOnLineByCode_Select(COLCOde).Single();

            if (result == null) return null;

            return GetMonitorTransferLog(result.ClaimOnLineId);
        }

        public JsonResult GetTransferDetail(int transferId)
        {
            var result = _context.usp_ClaimOnLineTransfer_Select(transferId, null, true).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteTransfer(int transferId)
        {
            try
            {
                var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                var result = _context.usp_ClaimOnLineTransfer_Delete(transferId, userID).FirstOrDefault();

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SearchTransferItemVersion1(string colCode)
        {
            var result = _context.usp_ClaimOnLineTransferItemVersion1_Select(colCode, colCode, true).ToList();

            var dt = new Dictionary<string, object>
            {
                //{"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.ToList().Count : result.ToList().Count()},
                {"recordsFiltered", result.Count() != 0 ? result.ToList().Count : result.ToList().Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchTransferItem(string colCode)
        {
            var result = _context.usp_ClaimOnLineTransferItem_Select(colCode, colCode, true).ToList();

            var dt = new Dictionary<string, object>
            {
                //{"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.ToList().Count : result.ToList().Count()},
                {"recordsFiltered", result.Count() != 0 ? result.ToList().Count : result.ToList().Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteCOLItem(string claimHeaderGroupCode)
        {
            try
            {
                var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                var result = _context.usp_ClaimOnLineTransferItem_Delete(claimHeaderGroupCode, userID);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult COLEndorsement(string claimOnlineCode, string claimOnlineHeaderGroupCode, string oldClaimOnLineCode)
        {
            try
            {
                //set to uppercase
                claimOnlineCode = claimOnlineCode.ToUpper();
                var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                var result = _context.usp_Endorse_ClaimOnlineCodeinClaim_Update(claimOnlineHeaderGroupCode, oldClaimOnLineCode, claimOnlineCode, userID).FirstOrDefault();

                if (result != null)
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetSSSPAAccountNo(string txt)
        {
            var result = _context.usp_SSSPAAccountNoByApplicationCode(txt).FirstOrDefault();

            if (result != null) return Json(result, JsonRequestBehavior.AllowGet);

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClaimOnLineDetailV2(int claimOnlineId)
        {
            var rs = _context.usp_ClaimOnLineVersion3_Select(claimOnlineId, null, null, null, null, 0, 9999, null, null, null, null, null, null).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClaimDetail_DT(int claimOnlineId, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _context.usp_ClaimOnLineTmp_Select(claimOnlineId, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDocumentDetail_DT(int claimOnlineId, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _context.usp_Document_Select(claimOnlineId, null, null, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [Authorization(Roles = "Developer")]
        public ActionResult ValidateClaimOnlineTransferVersion3(int claimonLineId, string reference = null, string accountNo = null, string remark = null)
        {
            Log.Debug("{_controllerName} - Start ValidateClaimOnlineTransferVersion3 [claimOnLineId = {claimOnLineId}]", _controllerName, claimonLineId);
            var user = GlobalFunction.GetLoginDetail(this.HttpContext);

            int? User_ID = user.User_ID;
            if (User_ID.HasValue)
            {
                Log.Debug("{_controllerName} - ValidateClaimOnlineTransferVersion3 Get User Successful [claimOnLineId = {claimOnLineId},User  = {User_ID}]", _controllerName, claimonLineId, User_ID);
            }
            else
            {
                Log.Debug("{_controllerName} - ValidateClaimOnlineTransferVersion3 Get User Fail [claimOnLineId = {claimOnLineId},User  = {User_ID}]", _controllerName, claimonLineId, User_ID);
            }

            //validate before insert
            var valid = _context.usp_ClaimOnLineTransferValidate2_Select(claimonLineId, reference, accountNo, remark, user.User_ID).Single();

            string _validMsg = valid.Msg;
            if (valid.IsResult.Value)
            {
                Log.Debug("{_controllerName} - ValidateClaimOnlineTransferVersion3 Successful : usp_ClaimOnLineTransferValidate2_Select : [claimOnLineId = {claimOnLineId},reference = {reference},accountNo = {accountNo},remark = {remark},user = {User_ID},valid = {_validMsg}]", _controllerName, claimonLineId, reference, accountNo, remark, User_ID, _validMsg);
            }
            else
            {
                Log.Debug("{_controllerName} - ValidateClaimOnlineTransferVersion3 Fail : usp_ClaimOnLineTransferValidate2_Select : [claimOnLineId = {claimOnLineId},reference = {reference},accountNo = {accountNo},remark = {remark},user = {User_ID},valid = {_validMsg}]", _controllerName, claimonLineId, reference, accountNo, remark, User_ID, _validMsg);
            }

            //invalid return false
            if (!valid.IsResult.Value) return Json(ResponseResult.Failure<string>(message: valid.Msg), JsonRequestBehavior.AllowGet);

            //valid equal true
            try
            {
                Log.Debug("{_controllerName} - Start GetCustomerDetail : usp_PersonContactPhoneNoById_Select [claimOnLineId = {claimOnLineId}]", _controllerName, claimonLineId);
                //get contact from stored procedure
                var contact = _context.usp_PersonContactPhoneNoById_Select(claimonLineId).First();

                if (contact.PersonContactPhoneNo != "" && contact.PayeeAccountNo != "" && contact.PayeeAccountName != "")
                {
                    Log.Debug("{_controllerName} - GetCustomerDetail Successful : usp_PersonContactPhoneNoById_Select [claimOnLineId = {claimOnLineId},PayeeAccountNo = {PayeeAccountNo},PayeeAccountName = {PayeeAccountName},PersonContactPhoneNo = {PersonContactPhoneNo}]", _controllerName, claimonLineId, contact.PayeeAccountNo, contact.PayeeAccountName, contact.PersonContactPhoneNo);
                }
                else
                {
                    Log.Debug("{_controllerName} - GetCustomerDetail Fail : usp_PersonContactPhoneNoById_Select [claimOnLineId = {claimOnLineId},PayeeAccountNo = {PayeeAccountNo},PayeeAccountName = {PayeeAccountName},PersonContactPhoneNo = {PersonContactPhoneNo}]", _controllerName, claimonLineId, contact.PayeeAccountNo, contact.PayeeAccountName, contact.PersonContactPhoneNo);
                }

                //regex pattern
                var reg = new Regex(@"[!""#฿$%&'()*+,-./:;<=>?@A-Zก-๙\[\\\]^_`a-z{|}~]");

                var phoneNumber = "";
                //เช็ค Environment ถ้าเป็น PROD ให้ส่งข้อมูลจริง / UAT ส่งเบอร์สำหรับทดสอบ
                var env = Properties.Settings.Default.Environment;
                if (env == "PROD")
                {
                    phoneNumber = reg.Replace(contact.PersonContactPhoneNo.Trim(), "");
                    if (phoneNumber != "")
                    {
                        Log.Debug("{_controllerName} - env PROD PhoneNumber Successful [claimOnLineId = {claimOnLineId},phoneNumber = {phoneNumber}]", _controllerName, claimonLineId, phoneNumber);
                    }
                    else
                    {
                        Log.Debug("{_controllerName} - env PROD PhoneNumber Fail [claimOnLineId = {claimOnLineId},phoneNumber = {phoneNumber}]", _controllerName, claimonLineId, phoneNumber);
                    }
                }
                else if (env == "UAT")
                {
                    //เบอร์โทรสำหรับทดสอบ ตั้งต่าใน config
                    var forTest = Properties.Settings.Default.PhonNumberForTest.Trim();
                    phoneNumber = reg.Replace(forTest, "");
                    if (phoneNumber != "")
                    {
                        Log.Debug("{_controllerName} - env UAT PhoneNumber Successful [claimOnLineId = {claimOnLineId},phoneNumber = {phoneNumber}]", _controllerName, claimonLineId, phoneNumber);
                    }
                    else
                    {
                        Log.Debug("{_controllerName} - env UAT PhoneNumber Fail [claimOnLineId = {claimOnLineId},phoneNumber = {phoneNumber}]", _controllerName, claimonLineId, phoneNumber);
                    }
                }
                else
                {
                    //return fail -- env ไม่ได้ set value
                    Log.Debug("{_controllerName} - Not Set ENV [claimOnLineId = {claimOnLineId}]", _controllerName, claimonLineId);
                    return Json(ResponseResult.Failure<string>(message: "Environment does not exist"), JsonRequestBehavior.AllowGet);
                }

                if (phoneNumber.Length != 10)
                {
                    //return เบอร์โทรไม่ถูกต้อง
                    Log.Error("{_controllerName} - ValidateClaimOnlineTransferVersion3 - Please Check Phone Number {0} - claimOnLineId={1}", _controllerName, contact.PersonContactPhoneNo, claimonLineId);
                    return Json(ResponseResult.Failure<string>(message: String.Format("กรุณาตรวจสอบหมายเลขโทรศัพท์ {0}", contact.PersonContactPhoneNo)), JsonRequestBehavior.AllowGet);
                }

                contact.PersonContactPhoneNo = phoneNumber;
                Log.Debug("{_controllerName} - End Of ValidateClaimOnlineTransferVersion3 Successful  [claimOnLineId = {claimOnLineId}]", _controllerName, claimonLineId);
                return Json(ResponseResult.Success<usp_PersonContactPhoneNoById_Select_Result>(contact), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - ValidateClaimOnlineTransferVersion3 [claimonLineId: {claimonLineId},Error: {Message}]", _controllerName, claimonLineId, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        private class CheckClaimTransferedModel
        {
            public bool? IsAllowReTransfer { get; set; } = true;
            public int? ClaimOnLineStatusId { get; set; }
        }

        [HttpGet]
        public JsonResult CheckClaimTransfered(int claimonLineId, int? ClaimOnLineTransferId = null)
        {
            //เช็คว่าเคลมนี้ถูกโอนเงินไปแล้วหรือยัง 3 = โอนแล้ว 6 = ไม่สำเร็จ
            var response = new CheckClaimTransferedModel();
            if (ClaimOnLineTransferId == null)
            {
                response.ClaimOnLineStatusId = _context.ClaimOnLine.Where(x => x.ClaimOnLineId == claimonLineId).FirstOrDefault().ClaimOnLineStatusId;
            }
            else
            {
                var result = _context.ClaimOnLineTransfer.Where(x => x.ClaimOnLineTransferId == ClaimOnLineTransferId).GroupJoin(_context.ClaimOnLine, tct => tct.ClaimOnLineId, col => col.ClaimOnLineId, (ct, joinGroup) => new
                {
                    ct.IsAllowReTransfer,
                    ClaimOnLineStatusId = joinGroup.Select(col => col.ClaimOnLineStatusId).FirstOrDefault()
                }).FirstOrDefault();

                response.ClaimOnLineStatusId = result.ClaimOnLineStatusId;
                response.IsAllowReTransfer = result.IsAllowReTransfer;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Authorization(Roles = "Developer")]
        public ActionResult SaveTmpClaimOnlineTransfer(int claimonLineId, int transferTypeID, int payerTypeId, int payeeTypeId, string reference, double transferAmountTotal, string remark, string personContactPhoneNo, string payeeAccountName, string payeeAccountNo, int? payeeBankId, int transferFromMenu, int? claimOnLineAccountId = null, int? payAutoTypeId = null, string claimOnLineCode = null, bool? isClaimAI = false)
        {
            Log.Debug("{_controllerName} Start SaveTmpClaimOnlineTransfer [claimOnLineId = {claimOnLineId}, transferTypeID = {transferTypeID} payerTypeId = {payerTypeId}, payeeTypeId = {payeeTypeId}, reference = {reference}, transferAmountTotal = {transferAmountTotal}, remark = {remark}, personContactPhoneNo = {personContactPhoneNo}, payeeAccountName = {payeeAccountName}, payeeAccountNo = {payeeAccountNo}, payeeBankId = {payeeBankId}, transferFromMenu = {transferFromMenu}, claimOnLineAccountId = {claimOnLineAccountId}, payAutoTypeId = {payAutoTypeId}]", _controllerName, claimonLineId, transferTypeID, payerTypeId, payeeTypeId, reference, transferAmountTotal, remark, personContactPhoneNo, payeeAccountName, payeeAccountNo, payeeBankId, transferFromMenu, claimOnLineAccountId, payAutoTypeId);
            var user = GlobalFunction.GetLoginDetail(this.HttpContext);
            if (isClaimAI == true)
            {
                // 00000 (5) คุณสำนักงาน
                user.EmployeeCode = "00000";
                user.User_ID = 5;
            }

            try
            {
                string msg = "";
                bool flgSendSMSWithSurvey = false;
                string MsgSurvey = "";

                try
                {
                    var PayListHeaderId = Guid.NewGuid();
                    var fromBankId = 7; //BBL
                    var fromAccountNo = Properties.Settings.Default.PayAutoBankAccNo;
                    var fromAccountName = Properties.Settings.Default.PayAutoBankAccName;
                    Log.Debug($"{_controllerName} - SaveTmpClaimOnlineTransfer [Store procedure]: usp_TmpClaimOnLineTransfer_Insert_v2 [ claimonLineId = {claimonLineId}, transferTypeID = {transferTypeID}, payerTypeId = {payerTypeId}, payeeTypeId = {payeeTypeId}, payListHeaderId = {PayListHeaderId}, reference = {reference}, transferAmountTotal = {transferAmountTotal}, remark = {remark}, createByUserId = {user.User_ID}, createdByCode = {user.EmployeeCode}, sMSPhoneNo = {personContactPhoneNo}, sMSMessage = {msg}, isSMSSurveyLink = {flgSendSMSWithSurvey}, msgSurvey = {MsgSurvey}, transferFromMenu = {transferFromMenu}, claimOnLineAccountId = {claimOnLineAccountId}, fromBankId = {fromBankId}, fromAccountNo = {fromAccountNo}, fromAccountName = {fromAccountName}, payAutoTypeId = {payAutoTypeId}]");
                    var rs = _context.usp_TmpClaimOnLineTransfer_Insert_v2(claimonLineId
                                                                                                        , transferTypeID
                                                                                                        , payerTypeId
                                                                                                        , payeeTypeId
                                                                                                        , PayListHeaderId
                                                                                                        , reference
                                                                                                        , transferAmountTotal
                                                                                                        , remark
                                                                                                        , user.User_ID
                                                                                                        , user.EmployeeCode
                                                                                                        , personContactPhoneNo
                                                                                                        , msg
                                                                                                        , flgSendSMSWithSurvey
                                                                                                        , MsgSurvey
                                                                                                        , transferFromMenu
                                                                                                        , claimOnLineAccountId
                                                                                                        , fromBankId
                                                                                                        , fromAccountNo
                                                                                                        , fromAccountName
                                                                                                        , payAutoTypeId).Single();
                    Log.Debug("{_controllerName} - SaveTmpClaimOnlineTransfer claimonLineId = {claimonLineId} [Result]: {@rs}", _controllerName, claimonLineId, rs);

                    Log.Information("{_controllerName} - SaveTmpClaimOnlineTransfer Successful", _controllerName);

                    if (rs.IsResult == true)
                    {
                        return new PayAutoController().PublishPayTransferCreate(claimonLineId, payeeBankId, payeeAccountNo, payeeAccountName, transferAmountTotal, PayListHeaderId, claimOnLineCode);
                    }
                    else
                    {
                        return Json(ResponseResult.Failure<string>(message: rs.Msg), JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "{_controllerName} - SaveTmpClaimOnlineTransfer claimonLineId = {claimonLineId} Insert claimonline transfer failed Error: {Message}", _controllerName, claimonLineId, ex.Message);
                    return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                //return exception
                Log.Error(ex, "{_controllerName} - SaveTmpClaimOnlineTransfer claimonLineId = {claimonLineId} Error: {Message}", _controllerName, claimonLineId, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult RejectClaimOnLine(int claimOnLineId, int cancelCauseId, string remark)
        {
            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var rs = _context.usp_ClaimOnLineRejectVersion2_Insert(claimOnLineId, cancelCauseId, remark, userId).Single();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClaimOnlineStatusCount()
        {
            var result = _context.usp_pivotClaimOnlineStatusVersion2_Select().SingleOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDateNotTranfer(int zoneId, int branchId)
        {
            // 06074 Open ZoneId = -1 {ทั้งหมด}
            var result = _context.usp_ClaimOnLineDateNotTranfer_Select_V2(zoneId, branchId, null, null, null, null, null).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDateNotTranferMonitor(int zoneId, int branchId,
            int? draw,
            int? pageSize = null,
            int? pageStart = null,
            string sortField = null,
            string orderType = null,
            string search = null)
        {
            var result = _context.usp_ClaimOnLineDateNotTranfer_Select_V2(zoneId, branchId, pageStart, pageSize, sortField, orderType, search).ToList();
            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DuplicateClaimOnLine(string productGroup, string appId, string date)
        {
            try
            {
                string applicationCode = appId;
                int? productGroupId = null;
                DateTime? claimOnLineDate = null;

                switch (productGroup)
                {
                    case "PH":
                        productGroupId = 2;
                        break;

                    case "PA":
                        productGroupId = 3;
                        break;

                    default:
                        break;
                }

                if (!String.IsNullOrEmpty(date)) claimOnLineDate = Convert.ToDateTime(date);

                var result = _context.usp_DuplicateClaimOnLineCountByApplicationCode_Select(productGroupId, applicationCode, claimOnLineDate).Single();

                return Json(new { IsResult = true, Count = result, Msg = String.Format("Duplicate {0}", result) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { IsResult = false, Count = 0, Msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetCustomerAccountDetail_ClaimOnLineTransfer(int claimOnLineTransferId)
        {
            var result = _context.usp_ClaimOnLineTransfer_Select(claimOnLineTransferId, null, true).FirstOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClaimOnLineTransfer_OutstandingTotal(int? mode = null, DateTime? cDateFrom = null, DateTime? cDateTo = null, string statusId = null, int? branchId = null, int? areaId = null, int? bankId = null, bool? isShowAll = null, bool? transferAmountOverLimit = null, int? pageStart = null, int? pageSize = null, string search = null)
        {
            int? userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _context.usp_GetClaimOnLineTransfer_OutstandingTotal(mode,
                                                                 cDateFrom,
                                                                 cDateTo,
                                                                 statusId == "" ? null : statusId,
                                                                 branchId == -1 ? null : branchId,
                                                                 areaId == -1 ? null : areaId,
                                                                 bankId == -1 ? null : bankId,
                                                                 null,
                                                                 transferAmountOverLimit,
                                                                 pageStart,
                                                                 pageSize,
                                                                 search).FirstOrDefault().TransferAmountTotal;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClaimOnLineTransfer_OutstandingTotalAll()
        {
            var result = _context.usp_GetClaimOnLineTransfer_OutstandingAll().FirstOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClaimOnLineMonitor_Unsuccessful(
            int? branchId = null,
            int? draw = null,
            int? pageSize = null,
            int? pageStart = null,
            string sortField = null,
            string orderType = null,
            string search = null,
            string dateFrom = null,
            string dateTo = null,
            string optionsRadiosOnProcess = null)
        {
            DateTime? cDateFrom = null;
            DateTime? cDateTo = null;

            if (dateFrom != null && dateFrom != "-1") cDateFrom = Convert.ToDateTime(dateFrom);
            if (dateTo != null && dateTo != "-1") cDateTo = Convert.ToDateTime(dateTo);
            branchId = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;

            bool isShowTransferAmountLimit = false;
            //optionsRadiosOnProcess == 1 คือ แสดงยอด ≥ จำนวนเงินโอนที่ตั้งไว้
            if (optionsRadiosOnProcess == "1") isShowTransferAmountLimit = true;

            var result = _context.usp_ClaimOnLineVersion2_Status6_Select(cDateFrom, cDateTo, branchId, isShowTransferAmountLimit, pageStart, pageSize, sortField, orderType, search.Trim()).ToList();

            //sum ยอดเงินค้างโอน , statusId 6 ไม่สำเร็จ
            var resultTransferAmountTotal = GetClaimOnLineTransfer_OutstandingTotal(null, cDateFrom, cDateTo, "6", null, null, null, null, isShowTransferAmountLimit, null, null, search);

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()},
                {"transferAmountTotal", resultTransferAmountTotal}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClaimOnLineMonitor_WaitingTransfer(int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _context.usp_ClaimOnLineVersion3_1_Status5_Select(pageStart, pageSize, sortField, orderType, search.Trim()).ToList();

            var modelList = new List<GetClaimOnLineStatus5ResponseDto>();
            foreach (var item in result)
            {
                var model = new GetClaimOnLineStatus5ResponseDto
                {
                    ClaimOnLinePayAuto_TransactionId = item.ClaimOnLinePayAuto_TransactionId,
                    ClaimOnLinePayAuto_TransactionCode = item.ClaimOnLinePayAuto_TransactionCode,
                    CreatedDate = item.CreatedDate,
                    PayAutoTypeId = item.PayAutoTypeId,
                    CreatedByUserId = item.CreatedByUserId,
                    PaymentDate = item.PaymentDate,
                    PaymentAmount = item.PaymentAmount,
                    ToBankId = item.ToBankId,
                    ToAccountNo = item.ToAccountNo,
                    ToAccountName = item.ToAccountName,
                    TransRefNo = item.TransRefNo,
                    PayTransferResponse = item.PayTransferResponse,
                    ClaimOnLineId = item.ClaimOnLineId,
                    PayListHeaderId = item.PayListHeaderId,
                    ClaimOnLineTransferId = item.ClaimOnLineTransferId,
                    PremiumSourceStatusId = item.PremiumSourceStatusId,
                    ClaimOnLineCode = item.ClaimOnLineCode,
                    SMSPhoneNo = item.SMSPhoneNo,
                    SMSMessageSurvey = item.SMSMessageSurvey,
                    TransferFromMenu = item.TransferFromMenu,
                    TotalCount = item.TotalCount,
                    ToBankName = item.ToBankName
                };

                if (item.PayListHeaderId != null)
                {
                    // Post api PayTransferAPI
                    var client = new RestClient(Properties.Settings.Default.PayTransferApiURL.Trim());
                    // สร้าง request และกำหนด URL ของ API
                    var request = new RestRequest("/api/PayTransfer/PayListHeader/" + item.PayListHeaderId, Method.GET);
                    // ส่งคำขอไปยัง API และรอรับคำตอบ
                    Log.Debug("{_controllerName} - GetClaimOnLineMonitor_WaitingTransfer - Start Post PayTransferAPI [request = {request}]", _controllerName, client.BaseUrl + request.Resource);

                    IRestResponse response = client.Execute(request);

                    // ตรวจสอบสถานะการตอบกลับ
                    if (response.IsSuccessful)
                    {
                        // อ่านข้อมูลที่ได้รับจาก API
                        string responseBody = response.Content;
                        Log.Debug("{_controllerName} - GetClaimOnLineMonitor_WaitingTransfer PayTransferAPI response [response = {responseBody}]", _controllerName, responseBody);
                        // Deserialize the JSON string
                        GetPayListHeaderResponseDto PayTransferResponse = JsonConvert.DeserializeObject<GetPayListHeaderResponseDto>(responseBody);

                        if (PayTransferResponse.isSuccess)
                        {
                            model.PayListStatusId = PayTransferResponse.data.payListStatusId;
                            model.PayListStatusName = PayTransferResponse.data.payListStatusName;
                            model.Amount = PayTransferResponse.data.amount;
                        }
                    }
                    else
                    {
                        Log.Warning("{_controllerName} - GetClaimOnLineMonitor_WaitingTransfer เกิดข้อผิดพลาด PayTransferAPI : {ErrorMessage}", _controllerName, response.ErrorMessage);
                    }
                }


                modelList.Add(model);
            }

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", modelList.Count() != 0 ? modelList.FirstOrDefault()?.TotalCount : modelList.Count()},
                {"recordsFiltered", modelList.Count() != 0 ? modelList.FirstOrDefault()?.TotalCount : modelList.Count()},
                {"data", modelList.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RepairClaimOnLineTransferStatus5ByClaimOnLineId(int claimOnLineId, string claimOnLineCode)
        {
            try
            {
                Log.Debug("{_controllerName} Start RepairClaimOnLineTransferStatus5ByClaimOnLineId [claimOnLineId = {claimOnLineId}]", _controllerName, claimOnLineId);
                var dataStatus5 = _context.usp_ClaimOnLineVersion3_1_Status5_Select(null, null, null, null, claimOnLineCode).FirstOrDefault();
                Log.Debug("{_controllerName} [claimOnLineId = {claimOnLineId}] [Result] : {@dataStatus5} ", _controllerName, claimOnLineId, dataStatus5);

                if (dataStatus5 == null)
                {
                    Log.Warning("{_controllerName} - RepairClaimOnLineTransferStatus5ByClaimOnLineId [claimOnLineId = {claimOnLineId}] ไม่พบ PayListHeaderId", _controllerName, claimOnLineId);
                    return Json(ResponseResult.Failure<string>(message: "ไม่พบ PayListHeaderId"), JsonRequestBehavior.AllowGet);
                }
                
                Guid? PayListHeaderId = dataStatus5.PayListHeaderId;

                if (!string.IsNullOrEmpty(dataStatus5.PayTransferResponse))
                {
                    // Deserialize the JSON string
                    PayListResult PayListResult = JsonConvert.DeserializeObject<PayListResult>(dataStatus5.PayTransferResponse);
                    var insertResult = new Consumers.PayListResultConsumer().ClaimOnLineTransfer_PayAuto_Insert(claimOnLineId, PayListResult.TransferDate, (double)dataStatus5.PaymentAmount, dataStatus5.CreatedByUserId, null, null, null, PayListHeaderId, 3, dataStatus5.TransferFromMenu.Value, PayListResult.StatusBank);
                    if (insertResult.IsResult.Value)
                    {
                        Log.Information("{_controllerName} - RepairClaimOnLineTransferStatus5ByClaimOnLineId [claimOnLineId = {claimOnLineId}] Successful", _controllerName, claimOnLineId);
                        return Json(ResponseResult.Success<string>(message: insertResult.Msg), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Log.Information("{_controllerName} - RepairClaimOnLineTransferStatus5ByClaimOnLineId [claimOnLineId = {claimOnLineId}] failed", _controllerName, claimOnLineId);
                        return Json(ResponseResult.Failure<string>(message: insertResult.Msg), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var client = new RestClient(Properties.Settings.Default.PayTransferApiURL.Trim());
                    // สร้าง request และกำหนด URL ของ API
                    var request = new RestRequest("/api/PayTransfer/PayListHeader/" + PayListHeaderId, Method.GET);
                    // ส่งคำขอไปยัง API และรอรับคำตอบ
                    Log.Debug("{_controllerName} - RepairClaimOnLineTransferStatus5ByClaimOnLineId - Start Post PayTransferAPI [claimOnLineId = {claimOnLineId}, request = {request}]", _controllerName, claimOnLineId, client.BaseUrl + request.Resource);
                    IRestResponse response = client.Execute(request);
                    // อ่านข้อมูลที่ได้รับจาก API
                    string responseBody = response.Content;
                    Log.Debug("{_controllerName} - RepairClaimOnLineTransferStatus5ByClaimOnLineId PayTransferAPI response [claimOnLineId = {claimOnLineId}, response = {responseBody}]", _controllerName, claimOnLineId, responseBody);
                    // Deserialize the JSON string
                    GetPayListHeaderResponseDto PayListHeaderResponse = JsonConvert.DeserializeObject<GetPayListHeaderResponseDto>(responseBody);

                    // Check PayAutoTransaction ว่ามีประวัตการอนุมัติ และ โอนสำเร็จหรือยัง
                    var transaction = _context.usp_ClaimOnLinePayAuto_Transaction_Select(claimOnLineId, null, 999, null, null, null).ToList();
                    var havePayAutoType2 = transaction.Any(_ => _.PayAutoTypeId == 2); // อนุมัติ
                    var havePayAutoType3 = transaction.Any(_ => _.PayAutoTypeId == 3 && _.TransRefNo == PayListHeaderResponse.data.bankTranferNo); // โอนเงินสำเร็จ

                    int? userId = dataStatus5.CreatedByUserId;
                    decimal? transferAmountTotal = dataStatus5.PaymentAmount == null ? 0 : dataStatus5.PaymentAmount;

                    if (!havePayAutoType2)
                    {
                        var col = _context.usp_TmpClaimOnLineTransfer_Select(PayListHeaderId).FirstOrDefault();
                        if (col == null)
                        {
                            return Json(ResponseResult.Failure<string>(message: string.Format("PayListHeaderId: {0} ไม่มีข้อมูลใน TmpClaimOnLineTransfer", PayListHeaderId)), JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            userId = col.CreateByUserId;
                            transferAmountTotal = (decimal?)col.Amount;
                            new PayListResultConsumer().ClaimOnLinePayAuto_Transaction_Insert(1, 2, userId, null, (decimal)transferAmountTotal, null, null, claimOnLineId, PayListHeaderId, PayListHeaderResponse.data.bankStatusCode);
                        }
                    }

                    if (!havePayAutoType3)
                    {
                        // ตรวจสอบสถานะการตอบกลับ
                        if (response.IsSuccessful)
                        {
                            new PayListResultConsumer().ClaimOnLinePayAuto_Transaction_Insert(1, 3, userId, PayListHeaderResponse.data.bankTransferDate, (decimal)transferAmountTotal, PayListHeaderResponse.data.bankTranferNo, JsonConvert.SerializeObject(PayListHeaderResponse.data), claimOnLineId, PayListHeaderId, PayListHeaderResponse.data.bankStatusCode);
                        }
                        else
                        {
                            Log.Warning("{_controllerName} - RepairClaimOnLineTransferStatus5ByClaimOnLineId [claimOnLineId = {claimOnLineId}] เกิดข้อผิดพลาด PayTransferAPI : {ErrorMessage}", _controllerName, claimOnLineId, response.ErrorMessage);
                            return Json(ResponseResult.Failure<string>(message: response.ErrorMessage), JsonRequestBehavior.AllowGet);
                        }
                    }

                    var insertResult = new Consumers.PayListResultConsumer().ClaimOnLineTransfer_PayAuto_Insert(claimOnLineId, PayListHeaderResponse.data.bankTransferDate, (double)transferAmountTotal, userId, null, null, null, PayListHeaderId, 3, dataStatus5.TransferFromMenu.Value, PayListHeaderResponse.data.bankStatusCode);
                    if (insertResult.IsResult.Value)
                    {
                        Log.Information("{_controllerName} - RepairClaimOnLineTransferStatus5ByClaimOnLineId [claimOnLineId = {claimOnLineId}] Successful", _controllerName, claimOnLineId);
                        return Json(ResponseResult.Success<string>(message: insertResult.Msg), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Log.Information("{_controllerName} - RepairClaimOnLineTransferStatus5ByClaimOnLineId [claimOnLineId = {claimOnLineId}] failed", _controllerName, claimOnLineId);
                        return Json(ResponseResult.Failure<string>(message: insertResult.Msg), JsonRequestBehavior.AllowGet);
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - RepairClaimOnLineTransferStatus5ByClaimOnLineId [claimOnLineId = {claimOnLineId}] Error: {Message}", _controllerName, claimOnLineId, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCountClaimUnsuccess()
        {
            var count = _context.ClaimOnLineTransfer
                            .Join(_context.ClaimOnLine,
                                ct => ct.ClaimOnLineId,
                                c => c.ClaimOnLineId,
                                (ct, c) => new { ClaimOnLineTransfer = ct, ClaimOnLine = c })
                            .Where(x => x.ClaimOnLineTransfer.ClaimOnLineStatusId == 6 &&
                                        x.ClaimOnLine.ClaimOnLineStatusId != 4).Take(1).Count();
            return Json(count, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCountClaimWaitingTransfer()
        {
            DateTime targetDateTime = DateTime.Now.AddMinutes(-5);
            var count = _context.ClaimOnLine.Where(_ => _.ClaimOnLineStatusId == 5 && _.UpdateDate < targetDateTime).Take(1).Count();
            return Json(count, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOverAndUnderTransferAmountCount(int? zoneId = null, int? branchId = null, int? bankId = null, string dateFrom = null, string dateTo = null, int? statusId = null)
        {
            DateTime? cDateFrom = null;
            DateTime? cDateTo = null;
            if (dateFrom != null && dateFrom != "-1") cDateFrom = Convert.ToDateTime(dateFrom);
            if (dateTo != null && dateTo != "-1") cDateTo = Convert.ToDateTime(dateTo);
            var result = _context.usp_GetOverAndUnderTransferAmountCount(zoneId == -1 ? null : zoneId, branchId == -1 ? null : branchId, bankId == -1 ? null : bankId, cDateFrom, cDateTo, statusId).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RevertClaimOnLineStatus(int claimOnLineId)
        {
            int? userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var result = _context.usp_RevertClaimOnLineStatusToStatus2_Update(claimOnLineId, userId).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorization(Roles = "Developer")]
        public ActionResult WhiteListImport()
        {
            return View();
        }

        public JsonResult GetWhiteList(int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            var result = _context.usp_WhiteList_Select(pageStart, pageSize, sortField, orderType, search.Trim()).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertWhiteList(string employeeCode)
        {
            try
            {
                Serilog.Log.Debug("{_controllerName} Start InsertWhiteList [employeeCode = {employeeCode}]", _controllerName, employeeCode);
                int createByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

                Serilog.Log.Debug($"{_controllerName} - InsertWhiteList [Store procedure]: usp_WhiteList_Insert [employeeCode = {employeeCode}, userId = {createByUserId}]");
                var result = _context.usp_WhiteList_Insert(null, employeeCode, createByUserId).FirstOrDefault();
                Serilog.Log.Debug("{_controllerName} - InsertWhiteList [Store procedure]: usp_WhiteList_Insert [Result]: {@result}", _controllerName, result);

                if (result.IsResult.Value)
                {
                    Serilog.Log.Information("{_controllerName} - InsertWhiteList Successful", _controllerName);
                    return Json(ResponseResult.Success<string>(message: result.Msg), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Serilog.Log.Information("{_controllerName} - InsertWhiteList failed", _controllerName);
                    return Json(ResponseResult.Failure<string>(message: result.Msg), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "{_controllerName} - InsertWhiteList Error: {Message}", _controllerName, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteWhiteList(string employeeCode)
        {
            try
            {
                int createByUserId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                Serilog.Log.Debug("{_controllerName} Start DeleteWhiteList [employeeCode = {employeeCode}]", _controllerName, employeeCode);

                ClaimOnLineDBContext dataBase = new ClaimOnLineDBContext();
                var x = dataBase.WhiteList.Where(_ => _.EmployeeCode == employeeCode && _.IsActive == true).FirstOrDefault();
                x.IsActive = false;
                x.UpdatedDate = DateTime.Now;
                x.UpdateByUserId = createByUserId;
                dataBase.SaveChanges();

                Serilog.Log.Information("{_controllerName} - DeleteWhiteList Successful", _controllerName);
                return Json(ResponseResult.Success<string>(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "{_controllerName} - DeleteWhiteList Error: {Message}", _controllerName, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult WhiteListImportFile(HttpPostedFileBase file)
        {
            try
            {
                Serilog.Log.Debug("{_controllerName} Start WhiteListImportFile", _controllerName);

                var lstValidate = new string[4];

                var tmCode = new ObjectParameter("Result", typeof(string));

                var employeeCodeList = GetExcelImportWhiteList(file);

                if (employeeCodeList == null)
                {
                    return Json(ResponseResult.Failure<string>(message: "กรุณาตรวจสอบไฟล์นำเข้า"), JsonRequestBehavior.AllowGet);
                }

                var createByUserId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                using (var db = new ClaimOnLineDBContext())
                {
                    //Gen Code Tmp
                    db.usp_GenerateCode("TWL", 6, tmCode);
                }
                var tmp_Code = tmCode.Value.ToString();

                bool success = true;
                usp_TmpWhiteList_Insert_Result result = new usp_TmpWhiteList_Insert_Result();
                //Loop Insert Data
                foreach (var employeeCode in employeeCodeList)
                {
                    Log.Debug($"{_controllerName} - WhiteListImportFile Call Store procedure: usp_WhiteList_Insert [tmp_Code = {tmp_Code}, employeeCode = {employeeCode}, createByUserId = {createByUserId}]");
                    result = _context.usp_TmpWhiteList_Insert(tmp_Code, employeeCode, createByUserId).FirstOrDefault();

                    if (result.IsResult.Value == false)
                    {
                        Serilog.Log.Warning("{_controllerName} - WhiteListImportFile failed [message: {Msg}]", _controllerName, result.Msg.ToString());
                        success = false;
                        break;
                    }
                }

                if (!success)
                    return Json(ResponseResult.Failure<string>(message: result.Msg.ToString()), JsonRequestBehavior.AllowGet);

                Log.Debug($"{_controllerName} - WhiteListImportFile Call Store procedure: usp_TmpWhiteList_Validate [tmp_Code = {tmp_Code}]");
                var ValidateResult = _context.usp_TmpWhiteList_Validate(tmp_Code).FirstOrDefault();
                Serilog.Log.Debug("{_controllerName} - WhiteListImportFile [Store procedure]: usp_TmpWhiteList_Validate [Result]: {@ValidateResult}", _controllerName, ValidateResult);

                if (ValidateResult.IsResult.Value && ValidateResult.Result == "1")
                    return Json(ResponseResult.Success<string>(data: tmp_Code), JsonRequestBehavior.AllowGet);
                else if (ValidateResult.IsResult.Value && ValidateResult.Result == "0")
                    return Json(ResponseResult.Success<string>(data: tmp_Code, message: ValidateResult.Msg.ToString()), JsonRequestBehavior.AllowGet);
                else
                    return Json(ResponseResult.Failure<string>(message: ValidateResult.Msg.ToString()), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - WhiteListImportFile Error: {Message}", _controllerName, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        public List<string> GetExcelImportWhiteList(HttpPostedFileBase file)
        {
            List<string> EmployeeCodeList = new List<string>();

            Log.Debug("{_controllerName} Start GetExcelImportWhiteList", _controllerName);
            using (var package = new ExcelPackage(file.InputStream))
            {
                var cs = package.Workbook.Worksheets;
                var ws = cs.First();
                var nr = ws.Dimension.End.Row;

                try
                {
                    for (var ri = 2; ri <= nr; ri++)
                    {
                        if (ws.Cells[ri, 1].Value == null || ws.Cells[ri, 1].Value?.ToString() == "")
                        {
                            ws.DeleteRow(nr);
                        }
                        else
                        {
                            var employeeCode = ws.Cells[ri, 1]?.Value?.ToString() ?? "";
                            if (employeeCode.Length != 5) return null;
                            EmployeeCodeList.Add(employeeCode);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "{_controllerName} - GetExcelImportWhiteList Error: {Message}", _controllerName, ex.Message);
                    throw;
                }
            }

            return EmployeeCodeList;
        }

        public JsonResult GetWhiteListImportValidatePreview(string tmpCode,
          int? draw = null,
          int? pageSize = null,
          int? pageStart = null,
          string sortField = null,
          string orderType = null,
          string search = null)
        {
            Log.Debug("{_controllerName} Start GetWhiteListImportValidatePreview", _controllerName);

            var result = _context.usp_TmpWhiteList_Preview(tmpCode, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            Log.Information("{_controllerName} - GetWhiteListImportValidatePreview Successful", _controllerName);
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveWhiteListImport(string tmpCode)
        {
            Log.Debug("{_controllerName} Start SaveWhiteListImport [tmpCode = {tmpCode}]", _controllerName, tmpCode);

            int userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            try
            {
                Log.Debug($"{_controllerName} - SaveWhiteListImport Call Store procedure: usp_WhiteList_Insert [tmp_Code = {tmpCode}]");
                var result = _context.usp_WhiteList_Insert(tmpCode, null, userID).FirstOrDefault();
                Log.Debug("{_controllerName} - SaveWhiteListImport [Store procedure]: usp_WhiteList_Insert [Result]: {@result}", _controllerName, result);

                if (result.IsResult.Value)
                {
                    Serilog.Log.Information("{_controllerName} - SaveWhiteListImport Successful", _controllerName);
                    return Json(ResponseResult.Success<string>(), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Serilog.Log.Information("{_controllerName} - SaveWhiteListImport failed", _controllerName);
                    return Json(ResponseResult.Failure<string>(message: result.Msg), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - SaveWhiteListImport Error: {Message}", _controllerName, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ValidateForPostClaimAI(string ApplicationCode, int BankId, string AccountNo, string ClaimAdmitType, string EmployeeCode, string ClaimCode, int ClaimOnLineId, string HospitalId, string Amount)
        {
            try
            {
                decimal? cAmount;
                Log.Debug("{_controllerName} Start ValidateForPostClaimAI [ApplicationCode = {ApplicationCode}, BankId = {BankId}, AccountNo = {AccountNo}, ClaimAdmitType = {ClaimAdmitType}, EmployeeCode = {EmployeeCode}, ClaimCode = {ClaimCode}, ClaimOnLineId = {ClaimOnLineId}]", _controllerName, ApplicationCode, BankId, AccountNo, ClaimAdmitType, EmployeeCode, ClaimCode, ClaimOnLineId);

                var isAllowClaimAIPayAuto = _context.usp_ClaimOnlineOfficeHours_Select().FirstOrDefault().IsActive;
                if (isAllowClaimAIPayAuto == false)
                {
                    Log.Information("{_controllerName} - ValidateForPostClaimAI [isAllowClaimAIPayAuto == false]", _controllerName);
                    return Json(ResponseResult.Failure<string>(""), JsonRequestBehavior.AllowGet);
                }

                cAmount = decimal.Parse(Amount);

                Log.Debug($"{_controllerName} - ValidateForPostClaimAI Call Store procedure: usp_ClaimOnLineValidateForPostClaimAI_Select [ProductGroupId = {2}, ApplicationCode = {ApplicationCode}, BankId = {BankId}, AccountNo = {AccountNo}, ClaimAdmitType = {ClaimAdmitType}, EmployeeCode = {EmployeeCode}, HospitalId = {HospitalId}, Amount = {Amount}]");
                var result = _context.usp_ClaimOnLineValidateForPostClaimAI_Select(2, ApplicationCode, BankId, AccountNo, ClaimAdmitType, EmployeeCode, HospitalId, cAmount).FirstOrDefault();
                Log.Debug("{_controllerName} - ValidateForPostClaimAI [Store procedure]: usp_ClaimOnLineValidateForPostClaimAI_Select [Result]: {@result}", _controllerName, result);

                if (result.IsResult.Value)
                {
                    Log.Information("{_controllerName} - ValidateForPostClaimAI pass", _controllerName);
                    //return GetDataClaimByCodeForClaimAI(ClaimCode, ClaimOnLineId);
                    return GetDataClaimByCodeForClaimAIV2(ClaimCode, ClaimOnLineId, cAmount);
                }
                else
                {
                    Log.Information("{_controllerName} - ValidateForPostClaimAI not pass", _controllerName);
                    return Json(ResponseResult.Failure<string>(""), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - ValidateForPostClaimAI Error: {Message}", _controllerName, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult GetDataClaimByCodeForClaimAI(string ClaimCode, int? ClaimOnLineId = null)
        {
            try
            {
                Log.Debug("{_controllerName} Start GetDataClaicmByCodeForClaimAI [ClaimCode = {ClaimCode}, ClaimOnLineId = {ClaimOnLineId}]", _controllerName, ClaimCode, ClaimOnLineId);

                Log.Debug($"{_controllerName} - GetDataClaimByCodeForClaimAI Call Store procedure: usp_GetDataClaimByCodeForClaimAI_Select [ClaimCode = {ClaimCode}]");
                var DataClaim = _context.usp_GetDataClaimByCodeForClaimAI_Select(ClaimCode).FirstOrDefault();

                var PersonModel = new person
                {
                    sex = DataClaim.Person_Sex,
                    birth_date = DataClaim.Person_BirthDate.Value.ToString("dd/MM/yyyy"),
                    age = DataClaim.Person_Age.ToString(),
                    occupation = DataClaim.Person_Occupation,
                };

                var PolicyModel = new policy
                {
                    product_name = DataClaim.Policy_ProductName,
                    start_cover_date = DataClaim.Policy_StartCoverDate.Value.ToString("dd/MM/yyyy"),
                    age_policy = DataClaim.Policy_AgePolicy.ToString(),
                    memo = DataClaim.Policy_Memo,
                    loss_claim = DataClaim.Policy_LossClaim
                };

                var ClaimDataModel = new claim_data
                {
                    hospital_id = DataClaim.Claim_HospitalId,
                    hospital_name = DataClaim.Claim_HospitalName,
                    age = DataClaim.Claim_Age.ToString(),
                    birth_date = DataClaim.Claim_BirthDate.Value.ToString("dd/MM/yyyy"),
                    date_happen = DataClaim.Claim_DateHappen.Value.ToString("dd/MM/yyyy"),
                    insurer_code = DataClaim.Claim_InsurerCode,
                    claim_admit_type = DataClaim.Claim_ClaimAdmitType,
                    follow_up = DataClaim.Claim_FollowUp,
                    icd10_1 = DataClaim.Claim_ICD101,
                    icd10_2 = DataClaim.Claim_ICD102,
                    icd10_3 = DataClaim.Claim_ICD103,
                    code = DataClaim.Claim_Code,
                    opd_compensation = DataClaim.Claim_OpdCompensation,
                    opd_claim = DataClaim.Claim_OpdClaim,
                    province = DataClaim.Claim_Province,
                    chief_complain = DataClaim.Claim_ChiefComplain,
                    gender = DataClaim.Claim_Gender
                };

                var RequectModel = new GetDataClaimRequestDto
                {
                    trans_no = DataClaim.TransactionNo,
                    customer_guid = DataClaim.CustomerGuid,
                    app_id = DataClaim.ApplicationCode,
                    person = PersonModel,
                    policy = PolicyModel,
                    claim_data = ClaimDataModel
                };

                // Serialize JSON
                var JsonBody = JsonConvert.SerializeObject(RequectModel);
                JsonBody = JsonBody.Substring(0, JsonBody.Length - 1);
                JsonBody = JsonBody + ",\"" + "customParams" + "\":" + DataClaim.JsonCustomParams + "}";

                #region API Claim AI
                // Post api claim ai
                var client = new RestClient(Properties.Settings.Default.ClaimAI_API_URL.Trim());
                // สร้าง request และกำหนด URL ของ API
                var request = new RestRequest(Method.POST);


                // Add the Authorization header with the token value
                var token = OAuth2Helper.GetCookie();
                request.AddHeader("Authorization", $"Bearer {token.Value}");



                // กำหนด JsonBody
                request.AddJsonBody(JsonBody);
                // ส่งคำขอไปยัง API และรอรับคำตอบ
                Log.Debug("{_controllerName} - GetDataClaimByCodeForClaimAI - Start Post iclaim [request = {JsonBody}]", _controllerName, JsonBody);
                IRestResponse response = client.Post(request);

                // ตรวจสอบสถานะการตอบกลับ
                if (response.IsSuccessful)
                {
                    // อ่านข้อมูลที่ได้รับจาก API
                    string responseBody = response.Content;
                    Log.Debug("{_controllerName} - GetDataClaimByCodeForClaimAI iclaim response [response = {responseBody}]", _controllerName, responseBody);

                    // Deserialize the JSON string
                    IClaimResponseDto claimAIResponse = JsonConvert.DeserializeObject<IClaimResponseDto>(responseBody);
                    string concatenatedMessages = string.Join(", ", claimAIResponse.error_description.Select(e => e.message));
                    Log.Debug($"{_controllerName} - GetDataClaimByCodeForClaimAI Call Store procedure: usp_ClaimAIAuditResult_Insert [ClaimOnLineId = {ClaimOnLineId}, JsonBody = {JsonBody}, ClaimAIResponse = {responseBody}, CreatedByUserId = {5}]");
                    var result = _context.usp_ClaimAIAuditResult_Insert(ClaimOnLineId, JsonBody, responseBody, 5, claimAIResponse.is_valid, claimAIResponse.is_valid == true ? null : concatenatedMessages).FirstOrDefault();
                    Log.Debug("{_controllerName} - GetDataClaimByCodeForClaimAI [Store procedure]: usp_ClaimAIAuditResult_Insert [Result]: {@result}", _controllerName, result);

                    // After success then insert data to ClaimAIAuditResult
                    if (claimAIResponse.is_valid && result.IsResult.Value)
                        return Json(ResponseResult.Success<string>(), JsonRequestBehavior.AllowGet);
                    else
                        return Json(ResponseResult.Failure<string>(""), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Log.Debug($"{_controllerName} - GetDataClaimByCodeForClaimAI Call Store procedure: usp_ClaimAIAuditResult_Insert [ClaimOnLineId = {ClaimOnLineId}, JsonBody = {JsonBody}, ClaimAIResponse = {null}, CreatedByUserId = {5}]");
                    var result = _context.usp_ClaimAIAuditResult_Insert(ClaimOnLineId, JsonBody, null, 5, null, response.ErrorMessage).FirstOrDefault();
                    Log.Debug("{_controllerName} - GetDataClaimByCodeForClaimAI [Store procedure]: usp_ClaimAIAuditResult_Insert [Result]: {@result}", _controllerName, result);

                    Log.Warning("{_controllerName} - GetDataClaimByCodeForClaimAI เกิดข้อผิดพลาด iclaim : {ErrorMessage}", _controllerName, response.ErrorMessage);
                    return Json(ResponseResult.Failure<string>(""), JsonRequestBehavior.AllowGet);
                }
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - GetDataClaimByCodeForClaimAI Error: {Message}", _controllerName, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetDataClaimByCodeForClaimAIV2(string ClaimCode, int? ClaimOnLineId = null, decimal? Amount = null)
        {
            try
            {
                Log.Debug("{_controllerName} Start GetDataClaimByCodeForClaimAIV2 [ClaimCode = {ClaimCode}, ClaimOnLineId = {ClaimOnLineId}]", _controllerName, ClaimCode, ClaimOnLineId);

                Log.Debug($"{_controllerName} - GetDataClaimByCodeForClaimAIV2 Call Store procedure: usp_GetDataClaimByCodeForClaimAI_Select [ClaimCode = {ClaimCode}]");
                var DataClaim = _context.usp_GetDataClaimByCodeForClaimAI_Select(ClaimCode).FirstOrDefault();

                var PersonModel = new person
                {
                    sex = DataClaim.Person_Sex,
                    birth_date = DataClaim.Person_BirthDate.Value.ToString("dd/MM/yyyy"),
                    age = DataClaim.Person_Age.ToString(),
                    occupation = DataClaim.Person_Occupation,
                };

                var PolicyModel = new policy
                {
                    product_name = DataClaim.Policy_ProductName,
                    start_cover_date = DataClaim.Policy_StartCoverDate.Value.ToString("dd/MM/yyyy"),
                    age_policy = DataClaim.Policy_AgePolicy.ToString(),
                    memo = DataClaim.Policy_Memo,
                    loss_claim = DataClaim.Policy_LossClaim
                };

                var ClaimDataModel = new claim_data
                {
                    hospital_id = DataClaim.Claim_HospitalId,
                    hospital_name = DataClaim.Claim_HospitalName,
                    age = DataClaim.Claim_Age.ToString(),
                    birth_date = DataClaim.Claim_BirthDate.Value.ToString("dd/MM/yyyy"),
                    date_happen = DataClaim.Claim_DateHappen.Value.ToString("dd/MM/yyyy"),
                    insurer_code = DataClaim.Claim_InsurerCode,
                    claim_admit_type = DataClaim.Claim_ClaimAdmitType,
                    follow_up = DataClaim.Claim_FollowUp,
                    icd10_1 = DataClaim.Claim_ICD101,
                    icd10_2 = DataClaim.Claim_ICD102,
                    icd10_3 = DataClaim.Claim_ICD103,
                    code = DataClaim.Claim_Code,
                    opd_compensation = DataClaim.Claim_OpdCompensation,
                    opd_claim = DataClaim.Claim_OpdClaim,
                    province = DataClaim.Claim_Province,
                    chief_complain = DataClaim.Claim_ChiefComplain,
                    gender = DataClaim.Claim_Gender
                };

                var RequectModel = new GetDataClaimRequestDto
                {
                    trans_no = DataClaim.TransactionNo,
                    customer_guid = DataClaim.CustomerGuid,
                    app_id = DataClaim.ApplicationCode,
                    person = PersonModel,
                    policy = PolicyModel,
                    claim_data = ClaimDataModel
                };

                // Serialize JSON
                var JsonBody = JsonConvert.SerializeObject(RequectModel);
                JsonBody = JsonBody.Substring(0, JsonBody.Length - 1);
                JsonBody = JsonBody + ",\"" + "customParams" + "\":" + DataClaim.JsonCustomParams + "}";

                #region New Store Claim AI
                // Post to Store claim ai
                Log.Debug($"{_controllerName} - GetDataClaimByCodeForClaimAIV2 Call Store procedure: usp_ClaimOnlineCheckClaimPolicy_Select [ClaimCode = {ClaimCode} Amount = {Amount}]");
                var client = _context.usp_ClaimOnlineCheckClaimPolicy_Select(ClaimCode, Amount).FirstOrDefault();
                Log.Debug("{_controllerName} - GetDataClaimByCodeForClaimAIV2 [Store procedure]: usp_ClaimOnlineCheckClaimPolicy_Select [client]: {@client}", _controllerName, client);

                if (client.is_valid.Value == true)
                {

                    // อ่านข้อมูลที่ได้รับจาก Store
                    Log.Debug("{_controllerName} - GetDataClaimByCodeForClaimAIV2 iclaim response [response = {responseBody}]", _controllerName, client.Msg);

                    // Deserialize the JSON string
                    Log.Debug($"{_controllerName} - GetDataClaimByCodeForClaimAIV2 Call Store procedure: usp_ClaimAIAuditResult_Insert [ClaimOnLineId = {ClaimOnLineId}, JsonBody = {JsonBody}, ClaimAIResponse = {null}, CreatedByUserId = {5}]");
                    var result = _context.usp_ClaimAIAuditResult_Insert(ClaimOnLineId, JsonBody, null, 5, client.is_valid, client.Msg).FirstOrDefault();
                    Log.Debug("{_controllerName} - GetDataClaimByCodeForClaimAIV2 [Store procedure]: usp_ClaimAIAuditResult_Insert [Result]: {@result}", _controllerName, result);

                    // After success then insert data to ClaimAIAuditResult
                    if (result.IsResult.Value)
                        return Json(ResponseResult.Success<string>(), JsonRequestBehavior.AllowGet);
                    else
                        return Json(ResponseResult.Failure<string>(""), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Log.Debug($"{_controllerName} - GetDataClaimByCodeForClaimAIV2 Call Store procedure: usp_ClaimAIAuditResult_Insert [ClaimOnLineId = {ClaimOnLineId}, JsonBody = {null}, ClaimAIResponse = {null}, CreatedByUserId = {5}]");
                    var result = _context.usp_ClaimAIAuditResult_Insert(ClaimOnLineId, JsonBody, null, 5, client.is_valid, client.Msg).FirstOrDefault();
                    Log.Debug("{_controllerName} - GetDataClaimByCodeForClaimAIV2 [Store procedure]: usp_ClaimAIAuditResult_Insert [Result]: {@result}", _controllerName, result);

                    Log.Warning("{_controllerName} - GetDataClaimByCodeForClaimAIV2 เกิดข้อผิดพลาด iclaim : {ErrorMessage}", _controllerName, client.Msg);
                    return Json(ResponseResult.Failure<string>(""), JsonRequestBehavior.AllowGet);
                }
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - GetDataClaimByCodeForClaimAIV2 Error: {Message}", _controllerName, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        private class ClaimAIResponseModel
        {
            public bool is_valid { get; set; }
            public string employeeName { get; set; }
            public bool claimAIApproved { get; set; } = false;
            public string error_description { get; set; }
        }
        public JsonResult GetClaimAIAuditResultByClaimOnLineId(int claimOnlineId)
        {
            var result = _context.usp_GetClaimAIAuditResultByClaimOnLineId_Select(claimOnlineId).ToList();

            var rs = new ClaimAIResponseModel();
            //string s = "";
            if (result.Count > 0)
            {
                // Deserialize the JSON string
                //IClaimResponseDto claimAIResponse = JsonConvert.DeserializeObject<IClaimResponseDto>(result[0].ClaimAIResponse);
                //foreach (ErrorDescription error in claimAIResponse.error_description)
                //{
                //    s += error.message + ", ";
                //}
                //string error_str = "";
                //if (s != "")
                //    error_str = s.Substring(0, s.Length - 2);

                rs.is_valid = result[0].Isvalid.Value;
                rs.employeeName = result[0].EmployeeName;
                rs.claimAIApproved = true;
                rs.error_description = result[0].ErrorDescription;
            }

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        [Authorization(Roles = "Developer")]
        public ActionResult ClaimAIPayAutoSettings()
        {
            var IsAllowClaimAI = CheckAllowClaimAIByEmployeeCode();
            if (IsAllowClaimAI == false)
                return RedirectToAction("Forbidden", "Auth");
            else
                return View();
        }

        public JsonResult GetCurrentAllowedClaimAI()
        {
            var result = _context.usp_ClaimOnlineOfficeHours_Select().ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AllowClaimAI_PayAuto(int allow)
        {
            Log.Debug("{_controllerName} Start AllowClaimAI_PayAuto [allow = {tmpCallowode}]", _controllerName, allow);

            bool _allow = allow == 1 ? true : false;

            int userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            try
            {
                Log.Debug($"{_controllerName} - AllowClaimAI_PayAuto Call Store procedure: usp_ClaimOnlineOfficeHours_Insert [allow = {_allow}, createdByUserId = {userID}]");
                var result = _context.usp_ClaimOnlineOfficeHours_Insert(_allow, userID).FirstOrDefault();
                Log.Debug("{_controllerName} - AllowClaimAI_PayAuto [Store procedure]: usp_ClaimOnlineOfficeHours_Insert [Result]: {@result}", _controllerName, result);

                if (result.IsResult.Value)
                {
                    Serilog.Log.Information("{_controllerName} - AllowClaimAI_PayAuto Successful", _controllerName);
                    return Json(ResponseResult.Success<string>(), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Serilog.Log.Information("{_controllerName} - AllowClaimAI_PayAuto failed", _controllerName);
                    return Json(ResponseResult.Failure<string>(message: result.Msg), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - SaveWhiteListImport Error: {Message}", _controllerName, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        public bool CheckAllowClaimAIByEmployeeCode()
        {
            var employeeCode = GlobalFunction.GetLoginDetail(this.HttpContext).EmployeeCode;
            return _context.ExployeeXAllowClaimAI.Any(x => x.EmployeeCode == employeeCode && x.IsActive == true);
        }
        #endregion "Method"

        #region "Service"

        public ActionResult GetEmployee(string q)
        {
            using (var client = new EmployeeServiceClient())
            {
                var result = client.GetEmployeeSearch(q).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetUserIdByEmployeeCode(string empCode)
        {
            using (var client = new EmployeeServiceClient())
            {
                var result = client.GetUserIDByEmpCode(empCode);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetEmployeeByEmployeeCode(string empcode)
        {
            using (var client = new EmployeeServiceClient())
            {
                var result = client.GetEmployeeByEmployeeCode(empcode);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion "Service"
    }
}