using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSClaimOnLine.EmployeeService;
using SmileSClaimOnLine.DataCenterMobileService;
using SmileSClaimOnLine.Models;
using SmileSClaimOnLine.Helper;
using System.Text.RegularExpressions;
using Serilog;
using System.Data.Entity.Core.Objects;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace SmileSClaimOnLine.Controllers
{
    [Authorization]
    public class PremiumController : Controller
    {
        #region dbContext

        private ClaimOnLineDBContext _context;
        private const string _controllerName = nameof(PremiumController);
        private const string _exportToExcelSuccessFul = "Export To Excel สำเร็จ";
        public PremiumController()

        {
            _context = new ClaimOnLineDBContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion dbContext

        // GET: Premium        
        public ActionResult Index()
        {
            return View();
        }

        [Authorization(Roles = "Developer")]
        public ActionResult TransferPremium()
        {
            //ViewBag.ClaimOnLineId = claimOnLineId;
            ViewBag.TransferType = _context.usp_TransferType_Select(true).ToList().Where(x => x.IsWithdraw == true && x.TransferTypeId != 7);

            int EmployeeId;

            EmployeeId = GlobalFunction.GetLoginDetail(this.HttpContext).Employee_ID;

            var c = _context.usp_ClaimOnLineAccount_Select(null, EmployeeId, true).Count();

            ViewBag.Account = Properties.Settings.Default.PayAutoBankAccNo + " - " + Properties.Settings.Default.PayAutoBankAccName;

            ViewBag.Zone = _context.usp_Zone_Select(null, true).ToList();

            ViewBag.Bank = _context.usp_Bank_Select(null, null, true).ToList().Where(x => x.Bank_ID != 2);

            var TransferAmountLimit = _context.TransferAmountLimit.FirstOrDefault().TransferAmountLimit1;
            ViewBag.TransferAmountLimit = TransferAmountLimit;

            ViewBag.AllowTransferAmountLimit = new ClaimOnLineController().CheckAllowTransferAmountLimit();

            var role = OAuth2Helper.GetRoles();
            var rolelist = role.Split(',').ToList();

            var roleOperation = new[] { "SmileSClaimOnLine_Operation" }; //MO

            if (rolelist.Intersect(roleOperation).Count() > 0)
            {
                ViewBag.RoleOperation = true;
            }
            else
            {
                ViewBag.RoleOperation = false;
            }

            return View();
        }

        public ActionResult ReturnPremium()
        {
            //ViewBag.ClaimOnLineId = claimOnLineId;
            ViewBag.TransferType = _context.usp_TransferType_Select(true).ToList().Where(x => x.IsDeposit == true);

            int EmployeeId;

            EmployeeId = GlobalFunction.GetLoginDetail(this.HttpContext).Employee_ID;

            var c = _context.usp_ClaimOnLineAccount_Select(null, EmployeeId, true).Count();

            List<usp_ClaimOnLineAccount_Select_Result> result = new List<usp_ClaimOnLineAccount_Select_Result>();

            if (c == 0)
            {
                result = _context.usp_ClaimOnLineAccount_Select(null, null, true).ToList();
            }
            else
            {
                result = _context.usp_ClaimOnLineAccount_Select(null, EmployeeId, true).ToList();
            }

            ViewBag.Account = result;

            ViewBag.Zone = _context.usp_Zone_Select(null, true).ToList();

            ViewBag.Bank = _context.usp_Bank_Select(null, null, true).ToList().Where(x => x.Bank_ID != 2);

            ViewBag.Refund = _context.usp_RefundCause_Select().ToList();

            return View();
        }

        public ActionResult FundTransfer()
        {
            ViewBag.TransferType = _context.usp_TransferType_Select(true).ToList().Where(x => x.TransferTypeId == 4);

            int EmployeeId;

            EmployeeId = GlobalFunction.GetLoginDetail(this.HttpContext).Employee_ID;

            var c = _context.usp_ClaimOnLineAccount_Select(null, EmployeeId, true).Count();

            List<usp_ClaimOnLineAccount_Select_Result> result = new List<usp_ClaimOnLineAccount_Select_Result>();

            if (c == 0)
            {
                result = _context.usp_ClaimOnLineAccount_Select(null, null, true).ToList();
            }
            else
            {
                result = _context.usp_ClaimOnLineAccount_Select(null, EmployeeId, true).ToList();
            }

            ViewBag.Account = result;

            return View();
        }

        public ActionResult ReceivePremium()
        {
            int EmployeeId;

            EmployeeId = GlobalFunction.GetLoginDetail(this.HttpContext).Employee_ID;

            var c = _context.usp_ClaimOnLineAccount_Select(null, EmployeeId, true).Count();

            List<usp_ClaimOnLineAccount_Select_Result> result = new List<usp_ClaimOnLineAccount_Select_Result>();

            if (c == 0)
            {
                result = _context.usp_ClaimOnLineAccount_Select(null, null, true).ToList();
            }
            else
            {
                result = _context.usp_ClaimOnLineAccount_Select(null, EmployeeId, true).ToList();
            }

            ViewBag.Account = result;

            ViewBag.TransferType = _context.usp_TransferType_Select(true).ToList().Where(x => x.TransferTypeId == 4);

            return View();
        }

        public ActionResult FundTransferImport()
        {
            return View();
        }
        #region "Method"

        public JsonResult GetDefaultAreaByUserId()
        {
            var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

            var c = _context.usp_AreaXEmployeeUserId_Select(userId, 0, 9999, null, null, null).Count();

            if (c > 0)
            {
                var rs = _context.usp_AreaXEmployeeUserId_Select(userId, 0, 9999, null, null, null).Single();
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetBranchByAreaId(int? areaId = null, int? branchId = null)
        {
            var rs = _context.usp_BranchByAreaId_Select(branchId == -1 ? null : branchId, areaId == -1 ? null : areaId, true, 0, 99999, null, null, null).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAreaVersion1(int? areaId = null)
        {
            var rs = _context.usp_Area_Select(areaId == -1 ? null : areaId, 0, 99999, null, null, null).ToList();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAreaVersion2()
        {
            var user = GlobalFunction.GetLoginDetail(this.HttpContext);
            var role = OAuth2Helper.GetRoles();
            var rolelist = role.Split(',').ToList();

            var roleDev = new[] { "Developer" }; //ฝ่ายพัฒนาระบบ
            var roleClaimonLine = new[] { "SmileSClaimOnLine_Manager" }; //ฝ่าย PA Underwrite
            var roleSupervisor = new[] { "SmileSClaimOnLine_Supervisor" };

            //ถ้าเป็น สิทธิ์ของ Dev และ PA ให้แสดงทั้งหมด
            if (rolelist.Intersect(roleClaimonLine).Count() > 0 || rolelist.Intersect(roleDev).Count() > 0)
            {
                var lstArea = _context.usp_Area_Select(null, 0, 99999, null, null, null).ToList();
                return Json(lstArea, JsonRequestBehavior.AllowGet);
            }
            //ถ้าเป็น สิทธิ์ของ Claim Online ให้โหลดภาคที่ตัวเองดูแล
            else if (rolelist.Intersect(roleSupervisor).Count() > 0)
            {
                var lstAreaEmployee = _context.usp_AreaXEmployeeUserId_Select(user.User_ID, 0, 9999, null, null, null).ToList();
                return Json(lstAreaEmployee, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetZoneVersion1(int? zoneId = null)
        {
            var rs = _context.usp_Zone_Select(zoneId == -1 ? null : zoneId, true).ToList();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPremiumMonitorAll(string claimHeaderGroupCode = null, string claimOnlineCode = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string[] claimList = null)
        {
            //int userID;

            //userID = Convert.ToInt32(Session["User_ID"]);

            double? sumPay = 0;

            if (claimList == null)
            {
                sumPay = 0;
            }
            else
            {
                foreach (var lst in claimList)
                {
                    if (lst != "on")
                    {
                        sumPay += Convert.ToDouble(_context.usp_ClaimAllMonitor_New_Select(claimHeaderGroupCode, claimOnlineCode, pageStart, pageSize, sortField, orderType, lst).SingleOrDefault().Pay);
                    }
                }
            }

            return Json(sumPay, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMonitorAccountTransaction(int AccountId, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            //int userID;

            //userID = Convert.ToInt32(Session["User_ID"]);

            var result = _context.usp_ClaimOnLineAccountTransactionMonitor_Select(AccountId, null, true, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
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
            int employeeID;

            employeeID = GlobalFunction.GetLoginDetail(this.HttpContext).Employee_ID;

            int c;

            c = _context.usp_ZoneXEmployee_Select(employeeID, true).Count();

            int? result;

            if (c > 0)
            {
                result = Convert.ToInt32(_context.usp_ZoneXEmployee_Select(employeeID, true).SingleOrDefault().ZoneId);
            }
            else
            {
                result = 0;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMonitorClaimOnlineVersion2(string dateFrom, string dateTo, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null, int? branchId = null, int? zoneId = null)
        {
            DateTime? d_dateFrom;
            DateTime? d_dateTo;

            d_dateFrom = GlobalFunction.ConvertDatePickerToDate_BE(dateFrom);
            d_dateTo = GlobalFunction.ConvertDatePickerToDate_BE(dateTo);

            if (branchId == -1)
            {
                branchId = null;
            }

            if (zoneId == -1)
            {
                zoneId = null;
            }

            var result = _context.usp_ClaimOnLineMonitorVersion2_Select(null, d_dateFrom, d_dateTo, zoneId, branchId, true, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMonitorClaimOnlineVersion1(string dateFrom, string dateTo, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null, int? branchId = null, int? zoneId = null)
        {
            DateTime? d_dateFrom;
            DateTime? d_dateTo;

            d_dateFrom = GlobalFunction.ConvertDatePickerToDate_BE(dateFrom);
            d_dateTo = GlobalFunction.ConvertDatePickerToDate_BE(dateTo);

            if (branchId == -1)
            {
                branchId = null;
            }

            if (zoneId == -1)
            {
                zoneId = null;
            }

            var result = _context.usp_ClaimOnLineMonitorVersion1_Select(null, d_dateFrom, d_dateTo, zoneId, branchId, true, pageStart, pageSize, sortField, orderType, search).ToList();

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
            //int userID;

            //userID = Convert.ToInt32(Session["User_ID"]);

            DateTime? d_dateFrom;
            DateTime? d_dateTo;

            d_dateFrom = GlobalFunction.ConvertDatePickerToDate_BE(dateFrom);
            d_dateTo = GlobalFunction.ConvertDatePickerToDate_BE(dateTo);

            if (branchId == -1)
            {
                branchId = null;
            }

            if (zoneId == -1)
            {
                zoneId = null;
            }

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

        public JsonResult GetMonitorClaimOnLineAll(string claimHeaderGroupCode = null, string claimOnlineCode = null, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            //int userID;

            //userID = Convert.ToInt32(Session["User_ID"]);

            var result = _context.usp_ClaimAllMonitor_New_Select(claimHeaderGroupCode, claimOnlineCode, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClaimOnLineSelect(int claimOnLineId)
        {
            var result = _context.usp_ClaimOnLine_Select(claimOnLineId, true).SingleOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorization(Roles = "Developer")]
        public ActionResult SaveReceivePremium(int account, int transferTypeID, string amount, string transferDate, string timetransfer)
        {
            var transferType = transferTypeID;
            var tfType = _context.usp_TransferType_Select(true).Where(x => x.TransferTypeId == transferType).SingleOrDefault();

            var payeeType = Convert.ToInt32(tfType.PayeeTypeId);
            int claimOnLineAccountId = account;

            var claimAccount = _context.usp_ClaimOnLineAccount_Select(claimOnLineAccountId, null, true).SingleOrDefault();

            var payeeBankId = claimAccount.BankId;
            var payeeAccountNo = claimAccount.AccountNo;
            var payeeAccountName = claimAccount.AccountName;

            if (amount != null)
            {
                amount = amount.Replace(",", "");
            }

            var Amount = Convert.ToDouble(amount);
            var tdate = transferDate;
            var timetrans = timetransfer;
            DateTime TransferDate;

            timetrans += ":00";

            TransferDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(tdate, timetrans));

            var claimOnLineId = 1;

            int payerType = Convert.ToInt32(tfType.PayerTypeId);

            var claimAmountFund = _context.usp_ClaimOnLineAccount_Select(1, null, null).SingleOrDefault();

            var fromBankId = claimAmountFund.BankId;
            string fromAccountNo = claimAmountFund.AccountNo;
            string fromAccountName = claimAmountFund.AccountName;

            bool isActive = true;

            int userID;

            userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            try
            {
                _context.usp_ClaimOnLineTransfer_Insert(claimOnLineId, transferType, payerType, fromBankId, fromAccountNo, fromAccountName, payeeType, payeeBankId, payeeAccountNo, payeeAccountName, TransferDate, Amount, claimOnLineAccountId, isActive, userID, null);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
                //throw;
            }
        }

        private class ClaimOnLineTransferPremiumModel
        {
            public int TransferTypeId { get; set; }
            public int PayerTypeId { get; set; }
            public int PayeeTypeId { get; set; }
            public int? FromBankId { get; set; }
            public string FromAccountNo { get; set; }
            public string FromAccountName { get; set; }
            public int ClaimOnLineAccountId { get; set; }
            public string PersonContactPhoneNo { get; set; }
        }

        public ActionResult GetClaimOnLineTransferPremiumAccount(int claimonLineId, string transferTypeId)
        {
            var transferType = Convert.ToInt32(transferTypeId);

            var tfType = _context.usp_TransferType_Select(true).Where(x => x.TransferTypeId == transferType).SingleOrDefault();

            var payeeType = Convert.ToInt32(tfType.PayeeTypeId);
            int payerType = Convert.ToInt32(tfType.PayerTypeId);

            var payerBankId = 3;
            string fromAccountNo = Properties.Settings.Default.PayAutoBankAccNo.Trim();
            string fromAccountName = Properties.Settings.Default.PayAutoBankAccName.Trim();

            //get contact from stored procedure
            var contact = _context.usp_PersonContactPhoneNoById_Select(claimonLineId).First();

            //regex pattern
            var reg = new Regex(@"[!""#฿$%&'()*+,-./:;<=>?@A-Zก-๙\[\\\]^_`a-z{|}~]");

            var phoneNumber = "";
            //เช็ค Environment ถ้าเป็น PROD ให้ส่งข้อมูลจริง / UAT ส่งเบอร์สำหรับทดสอบ
            var env = Properties.Settings.Default.Environment;
            if (env == "PROD")
            {
                phoneNumber = reg.Replace(contact.PersonContactPhoneNo.Trim(), "");
            }
            else if (env == "UAT")
            {
                //เบอร์โทรสำหรับทดสอบ ตั้งต่าใน config
                var forTest = Properties.Settings.Default.PhonNumberForTest.Trim();
                phoneNumber = reg.Replace(forTest, "");
            }
            else
            {
                //return fail -- env ไม่ได้ set value
                return Json(ResponseResult.Failure<string>(message: "Environment does not exist"), JsonRequestBehavior.AllowGet);
            }

            if (phoneNumber.Length != 10)
            {
                //return เบอร์โทรไม่ถูกต้อง
                Log.Error("{_controllerName} - GetClaimOnLineTransferPremiumAccount - Please Check Phone Number {0}", _controllerName, contact.PersonContactPhoneNo);
                return Json(ResponseResult.Failure<string>(message: String.Format("กรุณาตรวจสอบหมายเลขโทรศัพท์ {0}", contact.PersonContactPhoneNo)), JsonRequestBehavior.AllowGet);
            }

            contact.PersonContactPhoneNo = phoneNumber;

            var model = new ClaimOnLineTransferPremiumModel
            {
                TransferTypeId = transferType,
                PayerTypeId = payerType,
                PayeeTypeId = payeeType,
                FromBankId = payerBankId,
                FromAccountNo = fromAccountNo,
                FromAccountName = fromAccountName,
                PersonContactPhoneNo = contact.PersonContactPhoneNo
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [Authorization(Roles = "Developer")]
        public ActionResult SaveFundTransfer(int account, int transferTypeID, string amount, string transferDate, string timetransfer, string[] claimList = null)
        {
            var transferType = transferTypeID;
            var tfType = _context.usp_TransferType_Select(true).Where(x => x.TransferTypeId == transferType).SingleOrDefault();

            var payeeType = Convert.ToInt32(tfType.PayeeTypeId);
            int claimOnLineAccountId = account;

            var claimAccount = _context.usp_ClaimOnLineAccount_Select(claimOnLineAccountId, null, true).SingleOrDefault();

            var payeeBankId = claimAccount.BankId;
            var payeeAccountNo = claimAccount.AccountNo;
            var payeeAccountName = claimAccount.AccountName;
            if (amount != null)
            {
                amount = amount.Replace(",", "");
            }

            var Amount = Convert.ToDouble(amount);
            var tdate = transferDate;
            var timetrans = timetransfer;
            DateTime TransferDate;

            timetrans += ":00";

            TransferDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(tdate, timetrans));

            var claimOnLineId = 1;

            int payerType = Convert.ToInt32(tfType.PayerTypeId);

            var claimAmountFund = _context.usp_ClaimOnLineAccount_Select(1, null, null).SingleOrDefault();

            var fromBankId = claimAmountFund.BankId;
            string fromAccountNo = claimAmountFund.AccountNo;
            string fromAccountName = claimAmountFund.AccountName;

            bool isActive = true;

            int userID;

            userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            string result = "";

            var index = 0;

            if (claimList == null)
            {
                result = null;
            }
            else
            {
                foreach (var lst in claimList)
                {
                    result = result + ',' + lst;
                    index += 1;
                }

                result = result.Substring(1, result.Length - 1);
            }

            try
            {
                _context.usp_ClaimOnLineTransfer_Insert(claimOnLineId, transferType, payerType, fromBankId, fromAccountNo, fromAccountName, payeeType, payeeBankId, payeeAccountNo, payeeAccountName, TransferDate, Amount, claimOnLineAccountId, isActive, userID, result);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
                //throw;
            }
        }

        [Authorization(Roles = "Developer")]
        public ActionResult SaveClaimOnLineReturnPremium(string f_claimOnlineId, string claimOnLineAccount, string transferTypeId, string bankId, string accountNo, string accountName
                                                    , string amount, string f_transferDate, string transferTime, string refundCauseId, string remark)
        {
            var transferType = Convert.ToInt32(transferTypeId);

            var tfType = _context.usp_TransferType_Select(true).Where(x => x.TransferTypeId == transferType).SingleOrDefault();

            var payeeType = Convert.ToInt32(tfType.PayeeTypeId);
            int claimOnLineAccountId = Convert.ToInt32(claimOnLineAccount);

            var claimAccount = _context.usp_ClaimOnLineAccount_Select(claimOnLineAccountId, null, true).SingleOrDefault();

            var payeeBankId = claimAccount.BankId;

            var payeeAccountNo = claimAccount.AccountNo;
            var payeeAccountName = claimAccount.AccountName;
            var sAmount = amount;

            if (sAmount != null)
            {
                sAmount = sAmount.Replace(",", "");
            }

            var Amount = Convert.ToDouble(sAmount);
            var tdate = f_transferDate;
            var timetransfer = transferTime;
            DateTime transferDate;

            timetransfer += ":00";

            transferDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(tdate, timetransfer));

            var claimOnLineId = Convert.ToInt32(f_claimOnlineId);

            int payerType = Convert.ToInt32(tfType.PayerTypeId);

            var payerBankId = Convert.ToInt32(bankId);

            var _refundCauseId = Convert.ToInt32(refundCauseId);


            string fromAccountNo = accountNo;
            string fromAccountName = accountName;

            bool isActive = true;

            int userID;

            userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            try
            {
                //Save
                _context.usp_ClaimOnLineTransfer_InsertV2(claimOnLineId, transferType, payerType, payerBankId, fromAccountNo, fromAccountName, payeeType, payeeBankId, payeeAccountNo, payeeAccountName, transferDate, Amount, claimOnLineAccountId, isActive, userID, null, _refundCauseId, remark);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //throw;

                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetClaimOnLineAccountById(int ClaimOnLineAccountId)
        {
            string result;
            if (ClaimOnLineAccountId == -1)
            {
                result = "0";
            }
            else
            {
                result = _context.usp_ClaimOnLineAccount_Select(ClaimOnLineAccountId, null, true).SingleOrDefault().Balance.ToString();
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClaimOnLineAccountDetail(int ClaimOnLineAccountId)
        {
            var result = _context.usp_ClaimOnLineAccount_Select(ClaimOnLineAccountId, null, true).SingleOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion "Method"

        #region "ImportFile"
        public ActionResult FundTransferImportFile(HttpPostedFileBase file, string payDate = null)
        {
            Log.Debug($"{_controllerName} Start FundTransferImportFile ", _controllerName);

            var lstArr = new string[3];
            var lstValidate = new string[4];

            var tmCode = new ObjectParameter("Result", typeof(string));

            var lst = GetExcelImportFundTransfer(file);

            DateTime cPayDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(payDate));

            if (lst == null)
            {
                lstValidate[0] = "False";
                lstValidate[1] = "0";
                lstValidate[2] = "ข้อมูลไม่ถูกต้องกรุณาตรวจสอบไฟล์";
                lstValidate[3] = "";
                return Json(lstValidate, JsonRequestBehavior.AllowGet);
            }

            var lst_ClaimHeaderGroup = lst.Item1;
            var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

            using (var db = new ClaimOnLineDBContext())
            {
                //Gen Code Tmp
                db.usp_GenerateCode("ColDoc", 6, tmCode);
            }
            var tmp_Code = tmCode.Value.ToString();

            //Loop Insert Data
            foreach (var item in lst_ClaimHeaderGroup)
            {
                try
                {
                    Log.Debug($"{_controllerName} - FundTransferImportFile Call Store procedure: usp_TmpFundTransferImport_Insert [tmp_Code = {tmp_Code}, ClaimHeaderGroupCode = {item.ClaimHeaderGroupCode}, ItemCount = {item.ItemCount}, Total_Amount = {item.Total_Amount}, cPayDate = {cPayDate}, Branch = {item.Branch} ]");

                    var rs = _context.usp_TmpFundTransferImport_Insert(tmp_Code,
                                                                            item.Branch,
                                                                            item.Zone,
                                                                            item.ClaimHeaderGroupCode,
                                                                            item.Product,
                                                                            Convert.ToInt32(item.ItemCount),
                                                                             Decimal.Parse(item.Total_Amount),
                                                                             item.ClaimType,
                                                                             DateTime.Parse(item.SendDate),
                                                                            DateTime.Parse(item.PayDate),
                                                                            userId
                                                                            ).FirstOrDefault();

                    string message = rs.Msg.ToString(); 
                    lstArr[0] = rs.IsResult.Value.ToString();
                    lstArr[1] = rs.Result.ToString();
                    lstArr[2] = message;

                    if (lstArr[0] == "False")
                    {
                        Log.Warning(lstArr[0], "{_controllerName} - FundTransferImportFile Error: {Message}", _controllerName, lstArr[2]);
                        return Json(lstArr, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "{_controllerName} - FundTransferImportFile Error: {Message}", _controllerName, ex.Message);
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }

            Log.Debug($"{_controllerName} - FundTransferImportFile Call Store procedure: usp_TmpFundTransferImport_Validate [tmp_Code = {tmp_Code}]");
            //Insert Validate
            var rs_validate = _context.usp_TmpFundTransferImport_Validate(tmp_Code, cPayDate).FirstOrDefault();
            lstValidate[0] = rs_validate.IsResult.ToString();
            lstValidate[1] = rs_validate.Result.ToString();
            lstValidate[2] = rs_validate.Msg.ToString();
            lstValidate[3] = tmp_Code;

            Log.Information($"{_controllerName} - FundTransferImportFile Successful", _controllerName);
            return Json(lstValidate, JsonRequestBehavior.AllowGet);
        }

        public Tuple<List<ImportFundTransferRequestModel>> GetExcelImportFundTransfer(HttpPostedFileBase file)
        {
            List<ImportFundTransferRequestModel> FundTransferList = new List<ImportFundTransferRequestModel>();

            Log.Debug($"{_controllerName} Start GetExcelImportFundTransfer ", _controllerName);
            using (var package = new ExcelPackage(file.InputStream))
            {
                var cs = package.Workbook.Worksheets;
                var ws = cs.First();
                var nr = ws.Dimension.End.Row;

                try
                {
                    for (var ri = 2; ri <= nr; ri++)
                    {
                        if (ws.Cells[ri, 1].Value == null)
                        {
                            ws.DeleteRow(nr);
                        }
                        else
                        {
                            var Branch = ws.Cells[ri, 1].Value;
                            string branch_str = "";
                            if (Branch != null) { branch_str = ws.Cells[ri, 1].Value.ToString(); }
                            var Zone = ws.Cells[ri, 2].Value;
                            string zone_str = "";
                            if (Zone != null) { zone_str = ws.Cells[ri, 2].Value.ToString(); }
                            var ClaimHeaderGroupCode = ws.Cells[ri, 3].Value;
                            string claimHeaderGroupCode_str = "";
                            if (ClaimHeaderGroupCode != null) { claimHeaderGroupCode_str = ws.Cells[ri, 3].Value.ToString(); }
                            var Product = ws.Cells[ri, 4].Value;
                            string product_str = "";
                            if (Product != null) { product_str = ws.Cells[ri, 4].Value.ToString(); }
                            var ItemCount = ws.Cells[ri, 5].Value;
                            string itemCount_str = "";
                            if (ItemCount != null) { itemCount_str = ws.Cells[ri, 5].Value.ToString(); }
                            var Total_Amount = ws.Cells[ri, 6].Value;
                            string amount_str = "";
                            if (Total_Amount != null) { amount_str = ws.Cells[ri, 6].Value.ToString(); }
                            var ClaimType = ws.Cells[ri, 7].Value;
                            string claimType_str = "";
                            if (ClaimType != null) { claimType_str = ws.Cells[ri, 7].Value.ToString(); }
                            var SendDate = ws.Cells[ri, 8].Value;
                            string sendDate_str = "";
                            if (SendDate != null) { sendDate_str = ws.Cells[ri, 8].Value.ToString(); }
                            var PayDate = ws.Cells[ri, 9].Value;
                            string payDate_str = "";
                            if (PayDate != null) { payDate_str = ws.Cells[ri, 9].Value.ToString(); }
                            var InturanceCompany = ws.Cells[ri, 10].Value;
                            string inturanceCompany_str = "";
                            if (InturanceCompany != null) { inturanceCompany_str = ws.Cells[ri, 10].Value.ToString(); }

                            ImportFundTransferRequestModel item = new ImportFundTransferRequestModel();
                            item.Branch = branch_str;
                            item.Zone = zone_str;
                            item.ClaimHeaderGroupCode = claimHeaderGroupCode_str;
                            item.Product = product_str;
                            item.ItemCount = itemCount_str;
                            item.Total_Amount = amount_str;
                            item.ClaimType = claimType_str;
                            item.SendDate = sendDate_str;
                            item.PayDate =  payDate_str;
                            item.InturanceCompany = inturanceCompany_str;
                            FundTransferList.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "{_controllerName} - GetExcelImportFundTransfer Error: {Message}", _controllerName, ex.Message);
                    throw;
                }
            }

            return System.Tuple.Create(FundTransferList);
        }

        public JsonResult GetTmpFundTransferImportOverView(string tmpCode,
           int? draw = null,
           int? pageSize = null,
           int? pageStart = null,
           string sortField = null,
           string orderType = null,
           string search = null)
        {
            Log.Debug($"{_controllerName} Start GetTmpFundTransferImportOverView ", _controllerName);       
            var result = _context.usp_TmpFundTransferImport_Preview(tmpCode, pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            Log.Information($"{_controllerName} - GetTmpFundTransferImportOverView Successful", _controllerName);
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetImportFundTransferTmpCodeCount(string tmpCode)
        {
            Log.Debug($"{_controllerName} Start GetImportFundTransferTmpCodeCount ", _controllerName);
            Log.Debug($"{_controllerName} - GetImportFundTransferTmpCodeCount Call Store procedure: usp_TmpFundTransferImportCount_Select [tmp_Code = {tmpCode}]");
            var rs = _context.usp_TmpFundTransferImportCount_Select(tmpCode).FirstOrDefault();

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportFundTransferImportValidateToExcel(string tmpID)
        {
            //await Task.Yield();
            Log.Debug($"{_controllerName} Start ExportFundTransferImportValidateToExcel ", _controllerName);
            Session.Remove("DownloadExcel_FileFileFundTransferImportValidateReport");
            try
            {
                Log.Debug($"{_controllerName} - ExportFundTransferImportValidateToExcel Call Store procedure: usp_TmpFundTransferImport_Preview [tmp_Code = {tmpID}]");
                var data_sheet1 = _context.usp_TmpFundTransferImport_Preview(tmpID, null, 9999, null, null, null).Select(x => new
                {
                    บริษัท = x.InsuranceCompanyName,
                    วันที่กองทุนโอน = x.PayDate,
                    สาขา = x.BranchName,
                    สาขาโอนออนไลน์ =x.ClaimonlineBranch,
                    Product = x.ProductGroup,
                    เลขที่ชุดเคลม = x.ClaimHeaderGroupCode,
                    รายจากไฟล์กองทุน = x.TotalClaim,
                    รายโอนออนไลน์ =x.ClaimOnlineCount,
                    ยอดเงินจากไฟล์กองทุน = x.Amount,
                    ยอดเงินโอนออนไลน์ = x.TransferAmountTotal,
                    ผลต่าง = x.AmountDifference,
                    Error_Message = x.ValidateResult,
                }).ToList();

                var stream = new MemoryStream();

                using (var package = new ExcelPackage(stream))
                {
                    var workSheet1 = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet1.Cells.LoadFromCollection(data_sheet1, true);

                    // Select only the header cells
                    var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                    // Set their text to bold.
                    headerCells1.Style.Font.Bold = true;
                    // Set their background color
                    headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    // Apply the auto-filter to all the columns
                    var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                    allCells1.AutoFilter = true;
                    // Auto-fit all the columns
                    allCells1.AutoFitColumns();

                    package.Save();

                    stream.Position = 0;
                    Session["DownloadExcel_FileFileFundTransferImportValidateReport"] = package.GetAsByteArray();
                    //return Json("", JsonRequestBehavior.AllowGet);
                    return Json(ResponseResult.Success<string>(_exportToExcelSuccessFul), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Session.Remove("DownloadExcel_FileFileFundTransferImportValidateReport");
                Log.Error(ex, "{_controllerName} - ClaimOnLineReserveBranchExport Error: {Message}", _controllerName, ex.Message);
                //return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult DownloadFundTransferImportValidate()
        {
            Log.Debug($"{_controllerName} Start DownloadFundTransferImportValidate ", _controllerName);
            if (Session["DownloadExcel_FileFileFundTransferImportValidateReport"] != null)
            {
                byte[] data = Session["DownloadExcel_FileFileFundTransferImportValidateReport"] as byte[];
                string excelName = $"{DateTime.Now.ToString("ddMMyyyy")}__รายงานตัดรับชำระที่ Error.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }
        public ActionResult SaveFundTransferImport(string tmpCode)
        {
            Log.Debug($"{_controllerName} Start SaveFundTransferImport ", _controllerName);
            
            var lstArr = new string[3];
            int userID;
            userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            Log.Debug($"{_controllerName} - SaveFundTransferImport Call Store procedure: usp_ClaimOnLineTransferImport_Insert [tmp_Code = {tmpCode}]");
            try
            {
                var rs = _context.usp_ClaimOnLineTransferImport_Insert(tmpCode, userID).FirstOrDefault();

                lstArr[0] = rs.IsResult.Value.ToString();
                lstArr[1] = rs.Result.ToString();
                lstArr[2] = rs.Msg.ToString();

                return Json(lstArr, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{_controllerName} - SaveFundTransferImport Error: usp_ClaimOnLineTransferImport_Insert");
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion "ImportFile"
    }
}