using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Serilog;
using SmileSCommunicate.Helper;
using SmileSCommunicate.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SmileSCommunicate.Controllers
{
    [Helper.Authorization]
    public class SMSController : Controller
    {
        #region Context

        private readonly CommunicateV1Entities _context;
        private readonly DataCenterV1Entities _contextDataCenter;

        public SMSController()

        {
            _context = new CommunicateV1Entities();
            _contextDataCenter = new DataCenterV1Entities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            _contextDataCenter.Dispose();
        }

        #endregion Context

        #region Old

        [Helper.Authorization(Roles = "Developer,Communicate_Admin")]
        public ActionResult SearchSmsByPhoneNumberResult()
        {
            using (var db = new CommunicateV1Entities())
            {
                ViewBag.SMSType = db.usp_GetListSMSType_Select().ToList();
            }
            return View();
        }

        [Helper.Authorization(Roles = "Developer,Communicate_Admin")]
        public ActionResult CreditRemain()
        {
            using (var db = new CommunicateV1Entities())
            {
                ViewBag.SMSType = db.usp_GetListSMSType_Select().ToList();
            }
            return View();
        }

        [Helper.Authorization(Roles = "Developer,Communicate_Admin")]
        public ActionResult SearchSMSByCriteria()
        {
            using (var db = new CommunicateV1Entities())
            {
                ViewBag.SMSType = db.usp_GetListSMSType_Select().ToList();
            }
            return View();
        }

        public ActionResult SMSDetail()
        {
            return View();
        }

        [Helper.Authorization(Roles = "Developer,Communicate_Admin")]
        public ActionResult SendSMSNow()
        {
            ViewBag.userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            using (var db = new CommunicateV1Entities())
            {
                ViewBag.SMSType = db.usp_GetListSMSType_Select().ToList();
            }
            return View();
        }

        [Helper.Authorization(Roles = "Developer,Communicate_Admin,Communicate_ClaimMotor")]
        public ActionResult SMSClaimCars()
        {
            ViewBag.userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            using (var db = new DataCenterV1Entities())
            {
                ViewBag.Provinces = db.Provinces.ToList();
            }
            using (var db = new CommunicateV1Entities())
            {
                // 46 is แจ้งอุบัติเหตุรถยนต์
                ViewBag.SMSType = db.usp_GetListSMSType_Select().Where(x => x.SMSTypeID == 46).SingleOrDefault();
            }
            return View();
        }

        [Helper.Authorization(Roles = "Developer,Communicate_Admin")]
        public ActionResult Search()
        {
            using (var db = new CommunicateV1Entities())
            {
                ViewBag.SMSType = db.usp_GetListSMSType_Select().ToList();
                ViewBag.TransactionStatus = db.usp_TransactionStatus_Select().ToList();
                ViewBag.TransactionDetailStatus = db.usp_TransactionDetailStatus_Select().ToList();
            }
            return View();
        }

        public JsonResult Searching(string criteria, string dateF, string dateT, int? smsType,
            int? transactionStatus, int? transactionStatusDetail,
            int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null)
        {
            var dateTo = Convert.ToDateTime(dateT);
            var dateFrom = Convert.ToDateTime(dateF);

            using (var db = new CommunicateV1Entities())
            {
                var result = db.usp_GetListSMSReport_Select(dateFrom, dateTo, smsType, transactionStatus, transactionStatusDetail
                    , criteria, pageStart, pageSize, sortField, orderType).ToList();

                var dt = new Dictionary<string, object>
                {
                    //{"draw", draw },
                    {"recordsTotal", result.Count() != 0 ? result.ToList().Count : result.ToList().Count()},
                    {"recordsFiltered", result.Count() != 0 ? result.ToList().Count : result.ToList().Count()},
                    {"data", result.ToList()}
                };
                return Json(dt, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion Old

        #region NewView

        [Obsolete]
        [Helper.Authorization(Roles = "Developer")]
        public ActionResult SMSMain()
        {
            using (var db = new CommunicateV1Entities())
            {
                var result = db.usp_SMSTypeForSemiAuto_Select(0, 999, null, null, null).ToList();

                ViewBag.SMSType = result;
            }
            return View();
        }

        [Helper.Authorization(Roles = "Developer,Communicate_Admin,Communicate_Premium")]
        public ActionResult SMSPremium()
        {
            using (var db = new CommunicateV1Entities())
            {
                var result = db.usp_SMSTypeForSemiAuto_Select(0, 999, null, null, null).ToList();

                ViewBag.SMSType = result;
            }
            return View();
        }

        public ActionResult SMSPremiumThankyou()
        {
            return View();
        }

        private List<usp_GetListSMSType_Select_Result> GetSMSTypeByPermissions(List<usp_GetListSMSType_Select_Result> snsTypes, List<RolePermission> rolePermission, params string[] permissions)
        {
            var values = new List<int>();

            foreach (var permission in permissions)
            {
                var lstSMSType = rolePermission.FirstOrDefault(x => x.PermissionName == permission);
                if (lstSMSType != null)
                    values.AddRange(lstSMSType.Value.Split(',').Select(int.Parse));
            }

            List<usp_GetListSMSType_Select_Result> smsTypeListResult = new List<usp_GetListSMSType_Select_Result>();
            foreach (var i in values)
            {
                var smsType = snsTypes.FirstOrDefault(x => x.SMSTypeID == i);
                if (smsType != null)
                    smsTypeListResult.Add(smsType);
            }
            return smsTypeListResult;
        }

        private void AddItems(List<usp_GetListSMSType_Select_Result> sourceList, List<usp_GetListSMSType_Select_Result> resultList)
        {
            foreach (var itm in sourceList)
            {
                var hasValue = resultList.Any(x => x.SMSTypeID == itm.SMSTypeID);
                if (!hasValue)
                    resultList.Add(itm);
            }
        }

        [Helper.Authorization(Roles = "Developer,Communicate_Admin,Communicate_Premium,Communicate_Claim,Communicate_Fund,Communicate_CallCenter,Communicate_ClaimMotor,Communicate_ClaimMotor_Report,Communicate_SSSAdmin,Communicate_PH,Communicate_Insurance3")]
        public ActionResult SMSReport()
        {
            var userId = GlobalFunction.GetLoginDetail(HttpContext);

            var roleList = OAuth2Helper.GetRoles();
            var lstRoles = roleList.Split(',').ToList();

            using (var db = new CommunicateV1Entities())
            {
                if (lstRoles.Any())
                {
                    var SMSType = db.usp_GetListSMSType_Select().ToList();
                    var rolePermission = db.RolePermission.ToList();

                    List<usp_GetListSMSType_Select_Result> smsTypeResult = new List<usp_GetListSMSType_Select_Result>();

                    if (lstRoles.Contains("Developer") || lstRoles.Contains("Communicate_Admin"))
                    {
                        var all = new usp_GetListSMSType_Select_Result() { SMSTypeID = -1, SMSTypeDetail = "---ทั้งหมด---" };
                        SMSType.Add(all);
                        smsTypeResult = SMSType.OrderBy(x => x.SMSTypeID).ToList();
                    }
                    else
                    {
                        if (lstRoles.Contains("Communicate_Premium"))
                        {
                            var result = GetSMSTypeByPermissions(SMSType, rolePermission, "SMSType.Premium");
                            if (result.Count > 0)
                                AddItems(result, smsTypeResult);
                        }

                        if (lstRoles.Contains("Communicate_Claim"))
                        {
                            var result = GetSMSTypeByPermissions(SMSType, rolePermission, "SMSType.Claim");
                            if (result.Count > 0)
                                AddItems(result, smsTypeResult);
                        }
                        if (lstRoles.Contains("Communicate_CallCenter") && lstRoles.Contains("Communicate_ClaimMotor_Report"))
                        {
                            var result = GetSMSTypeByPermissions(SMSType, rolePermission, "SMSType.CallCenter", "SMSType.ClaimMotor");
                            if (result.Count > 0)
                                AddItems(result, smsTypeResult);
                        }
                        if (lstRoles.Contains("Communicate_CallCenter"))
                        {
                            var result = GetSMSTypeByPermissions(SMSType, rolePermission, "SMSType.CallCenter");
                            if (result.Count > 0)
                                AddItems(result, smsTypeResult);
                        }
                        if (lstRoles.Contains("Communicate_ClaimMotor") || lstRoles.Contains("Communicate_ClaimMotor_Report"))
                        {
                            var result = GetSMSTypeByPermissions(SMSType, rolePermission, "SMSType.ClaimMotor");
                            if (result.Count > 0)
                                AddItems(result, smsTypeResult);
                        }
                        if (lstRoles.Contains("Communicate_Fund"))
                        {
                            var result = GetSMSTypeByPermissions(SMSType, rolePermission, "SMSType.Fund");
                            if (result.Count > 0)
                                AddItems(result, smsTypeResult);
                        }
                        if (lstRoles.Contains("Communicate_SSSAdmin"))
                        {
                            var result = GetSMSTypeByPermissions(SMSType, rolePermission, "SMSType.SSSAdmin");
                            if (result.Count > 0)
                                AddItems(result, smsTypeResult);
                        }
                        if (lstRoles.Contains("Communicate_PH"))
                        {
                            var result = GetSMSTypeByPermissions(SMSType, rolePermission, "SMSType.PH");
                            if (result.Count > 0)
                                AddItems(result, smsTypeResult);
                        }
                        if (lstRoles.Contains("Communicate_Insurance3"))
                        {
                            var result = GetSMSTypeByPermissions(SMSType, rolePermission, "SMSType.Insurance3");
                            if (result.Count > 0)
                                AddItems(result, smsTypeResult);
                        }
                    }
                    ViewBag.SMSType = smsTypeResult;
                }
            }

            return View();
        }

        [Helper.Authorization(Roles = "Developer,Communicate_Admin,Communicate_Premium,Communicate_Fund,Communicate_SSSAdmin,Communicate_PH,Communicate_Insurance3,Communicate_CallCenter")]
        public ActionResult SMSImportExcel()
        {
            ViewBag.excelData = new List<ImportExcel>();

            return View();
        }

        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength <= 0)
            {
                return RedirectToAction("InternalServerError", "Error");
            }
            if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("InternalServerError", "Error");
            }

            var excelList = new List<ImportExcelV2>();

            Stream fileContent = file.InputStream;

            using (ExcelPackage excelPackage = new ExcelPackage(fileContent))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];
                var rowCount = worksheet.Dimension.End.Row;

                for (int i = 2; i <= rowCount; i++)
                {
                    var cellValuePhoneNo = worksheet.Cells[i, 1].Value?.ToString()?.Trim();
                    var cellValueMsg = worksheet.Cells[i, 2].Value?.ToString()?.Trim();
                    var cellValueRef = worksheet.Cells[i, 3].Value?.ToString()?.Trim();

                    if (string.IsNullOrWhiteSpace(cellValuePhoneNo) || string.IsNullOrWhiteSpace(cellValueMsg))
                    {
                        break; // Exit the loop if PhoneNo or Msg is empty
                    }

                    var newEntry = new ImportExcelV2
                    {
                        PhoneNo = cellValuePhoneNo,
                        Message = cellValueMsg,
                        Ref = string.IsNullOrWhiteSpace(cellValueRef) ? $"{DateTime.Now:yyyyMMddHHmmss}{new Random().Next(00, 99)}{i - 1}" : cellValueRef
                    };

                    excelList.Add(newEntry);
                }
            }

            ViewBag.excelData = excelList;
            ViewBag.excelDataJson = JsonConvert.SerializeObject(excelList);
            var userId = GlobalFunction.GetLoginDetail(HttpContext);
            ViewBag.userId = userId.User_ID;

            var dataKey = Guid.NewGuid().ToString();
            ViewBag.DataKey = dataKey;

            Session[dataKey] = excelList;

            Session.Timeout = 30;

            var roleList = OAuth2Helper.GetRoles();
            var lstRoles = roleList.Split(',').ToList();

            using (var db = new CommunicateV1Entities())
            {
                if (lstRoles.Any())
                {
                    var SMSType = db.usp_GetListSMSType_Select().ToList();
                    var rolePermission = db.RolePermission.ToList();

                    List<usp_GetListSMSType_Select_Result> smsTypeResult = new List<usp_GetListSMSType_Select_Result>();

                    if (lstRoles.Contains("Communicate_Fund"))
                    {
                        var result = GetSMSTypeByPermissions(SMSType, rolePermission, "SMSType.Fund");
                        if (result.Count > 0)
                            AddItems(result, smsTypeResult);
                    }

                    if (lstRoles.Contains("Communicate_SSSAdmin"))
                    {
                        var result = GetSMSTypeByPermissions(SMSType, rolePermission, "SMSType.SSSAdmin");
                        if (result.Count > 0)
                            AddItems(result, smsTypeResult);
                    }

                    if (lstRoles.Contains("Communicate_PH"))
                    {
                        var result = GetSMSTypeByPermissions(SMSType, rolePermission, "SMSType.PH");
                        if (result.Count > 0)
                            AddItems(result, smsTypeResult);
                    }

                    if (lstRoles.Contains("Communicate_Insurance3"))
                    {
                        var result = GetSMSTypeByPermissions(SMSType, rolePermission, "SMSType.Insurance3");
                        if (result.Count > 0)
                            AddItems(result, smsTypeResult);
                    }

                    if (lstRoles.Contains("Communicate_CallCenter"))
                    {
                        var result = GetSMSTypeByPermissions(SMSType, rolePermission, "SMSType.CallCenter");
                        if (result.Count > 0)
                            AddItems(result, smsTypeResult);
                    }

                    if (smsTypeResult.Count > 0)
                        ViewBag.SMSType = smsTypeResult;
                    else
                        ViewBag.SMSType = SMSType;
                }
            }

            return View("SMSImportExcel");
        }

        public JsonResult Save(string dataKey, int smsTypeId, string sendDate, string sendTime)
        {
            var userId = GlobalFunction.GetLoginDetail(HttpContext);
            Log.Information("[SMSImportExcel][Upload] - Started: {date} : User_ID {user_ID}, EmpCode {empCode}, FullName {fullName}, BranchDetail {branchDetail}, DepartmentDetail {departmentDetail}", DateTime.Now, userId.User_ID, userId.EmpCode, userId.FullName, userId.BranchDetail, userId.DepartmentDetail);

            string sendDateTime = null;
            //string datetime = null;
            if (sendDate != "")
            {
                var nDate = sendDate;
                var nTimeSplit = sendTime.Split(' ');
                var datetime = nDate + " " + nTimeSplit[0];
                var dateTimeConvert = Convert.ToDateTime(datetime);
                sendDateTime = dateTimeConvert.ToString("dd/MM/yyyy HH:mm:ss");
            }

            var excelList = (List<ImportExcelV2>)Session[dataKey];
            ImportExcelV2[] excelArray = excelList.ToArray();

            var smsData = new SMSSendV2
            {
                ProjectId = 15,
                SMSTypeId = smsTypeId,
                Remark = "Send from Communicate",
                Total = excelList.Count(),
                SendDate = sendDateTime,
                Data = excelArray
            };
            var apiUrl = new Uri("http://operation.siamsmile.co.th:9215/api/sms/SendSMSListV2");
            var authToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwcm9qZWN0aWQiOjl9.XX7R_Ik8pydv2e04ZvrDew8tlszSDrjvTEYKdgO4t7A";
            var jsonData = JsonConvert.SerializeObject(smsData);
            var request = WebRequest.Create(apiUrl);
            var byteData = Encoding.UTF8.GetBytes(jsonData);
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Headers.Add("Authorization", authToken);

            try
            {
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(byteData, 0, byteData.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var result = JsonConvert.DeserializeObject<SMSResultV2>(responseString);

                Log.Information("[SMSImportExcel][Upload] - Upload By {empCode} {firstName} - Success Reference id {referenceId}.", userId.EmpCode, userId.FirstName, result.referenceHeaderID);
                return Json(new { IsResult = true, Message = result.detail + " reference id:" + result.referenceHeaderID });
            }
            catch (Exception e)
            {
                Log.Error(e, "[SMSImportExcel][Upload] - Upload By {empCode} {firstName} - An error occurred.", userId.EmpCode, userId.FirstName);
                return Json(new { IsResult = false, e.Message });
            }
        }

        [Helper.Authorization(Roles = "Developer,Communicate_Admin,Communicate_Accounting")]
        public ActionResult CreditUtilizationReport()
        {
            return View();
        }

        [Helper.Authorization(Roles = "Developer,Communicate_Admin")]
        public ActionResult ManageProvider()
        {
            var provider = _context.Provider.Where(p => p.IsActive == true).ToList();
            ViewBag.Provider = provider;

            var defaultId = _context.ProgramConfig.FirstOrDefault(pc => pc.ParameterName == "ProviderDefault").ValueNumber;
            ViewBag.ProviderDefault = provider.FirstOrDefault(p => p.ProviderId == (int)defaultId).DisplayName;

            return View();
        }

        #endregion NewView

        #region Method

        [HttpPost]
        [Helper.Authorization(Roles = "Developer,Communicate_Admin")]
        public async Task<JsonResult> SendSMSV2(string phoneNo, string message, int smsTypeId, int createdById)
        {
            var response = await GlobalFunction.SendSMSV2(phoneNo, message, smsTypeId, createdById);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Helper.Authorization(Roles = "Developer,Communicate_Admin,Communicate_ClaimMotor")]
        public async Task<JsonResult> SendSMSWithRef(string phoneNo, string @ref, string message, int smsTypeId, int createdById)
        {
            var response = await GlobalFunction.SendSMSWithRef(phoneNo, message, @ref, smsTypeId, createdById);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Send / Insert
        /// </summary>
        /// <param name="headerId"></param>
        /// <param name="sendtype"></param>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TmpSMSToSMSQueue(int headerId, string sendtype, string date, string time)
        {
            var UserDetail = GlobalFunction.GetLoginDetail(HttpContext);
            var SendType = sendtype;
            var TmpSMSHeaderId = headerId;
            DateTime? SendDate = null;

            if (SendType == "0")
            {
                var nDate = date;
                var nTime = time;
                var nTimeSplit = time.Split(' ');
                var datetime = nDate + " " + nTimeSplit[0];
                SendDate = Convert.ToDateTime(datetime);
            }

            using (var db = new CommunicateV1Entities())
            {
                var result = db.usp_TmpSMSToSMSQueue_Insert(TmpSMSHeaderId, SendDate, "", UserDetail.User_ID).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="tmpSMSHeaderI"></param>
        /// <returns></returns>
        [HttpDelete]
        public ActionResult TmpSMSDelete(int tmpSMSHeaderI)
        {
            using (var db = new CommunicateV1Entities())
            {
                var UserDetail = GlobalFunction.GetLoginDetail(this.HttpContext);

                var result = db.usp_TmpSMS_Delete(tmpSMSHeaderI, UserDetail.User_ID).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Helper.Authorization(Roles = "Developer,Communicate_Admin")]
        public ActionResult UpdateDefaultProvider(int providerId)
        {
            var user = GlobalFunction.GetLoginDetail(this.HttpContext);

            try
            {
                var existingProvider = _context.ProgramConfig.FirstOrDefault(pc => pc.ParameterName == "ProviderDefault");
                existingProvider.ValueNumber = providerId;
                existingProvider.UpdatedByUserId = user.User_ID;
                existingProvider.UpdatedDate = DateTime.Now;
                _context.SaveChanges();
                return Json(new { IsSuccess = true, Msg = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Log.Error(e, "[UpdateDefaultProvider] - An error occurred.");
                return Json(new { IsSuccess = false, Msg = "Failure" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// NEW SMSDetailSEMI
        /// </summary>
        /// <param name="period"></param>
        /// <param name="SMSTypeId"></param>
        /// <param name="PaymethodId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SMSDetailSEMI(string period, int SMSTypeId, int PaymethodId, string remark)
        {
            await Task.Yield();

            try
            {
                using (var db = new CommunicateV1Entities())
                {
                    var UserDetail = GlobalFunction.GetLoginDetail(this.HttpContext);

                    var nPeriod = Convert.ToDateTime(period);
                    int timeout = 300000; //milliseconds

                    var result = InsertSMSDetailAsync(nPeriod, SMSTypeId, PaymethodId, remark, UserDetail.User_ID);

                    if (await Task.WhenAny(result, Task.Delay(timeout)) == result)
                    {
                        // task completed within timeout
                        return Json(result.Result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        // timeout logic
                        return Json("ErrorCode : " + HttpStatusCode.RequestTimeout, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<usp_InsertSMSDetail_SEMI_Insert_Result> InsertSMSDetailAsync(DateTime period, int SMSTypeId, int PaymethodId, string remark, int userId)
        {
            using (var db = new CommunicateV1Entities())
            {
                var result = await Task.Run(() => db.usp_InsertSMSDetail_SEMI_Insert(period, SMSTypeId, PaymethodId, remark, userId).FirstOrDefault());

                return result;
            }
        }

        /// <summary>
        /// Get PaymethodType
        /// </summary>
        /// <param name="smsTypeId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SMSTypeXPaymethodType(int smsTypeId)
        {
            using (var db = new CommunicateV1Entities())
            {
                var result = db.usp_SMSTypeXPaymethodType_Select(smsTypeId, 0, 999, null, null, null).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Get Data for Datatables
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="period"></param>
        /// <param name="indexStart"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TmpSMSHeader(int draw, string period, int? indexStart, int? pageSize, string sortField, string orderType, string search)
        {
            using (var db = new CommunicateV1Entities())
            {
                var nPeriod = Convert.ToDateTime(period);
                var lst = db.usp_TmpSMSHeader_Select(null, nPeriod, indexStart, pageSize, sortField, orderType, search).ToList();

                var dt = new Dictionary<string, object>
                {
                    {"draw",draw },
                    {"recordsTotal", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"recordsFiltered", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"data", lst.ToList()}
                };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Get Data for Datatables Transaction
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="smsTypeId"></param>
        /// <param name="pageStart"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetTransaction(int draw, string startDate, string endDate, int? smsTypeId, int? pageStart, int? pageSize, string sortField, string orderType, string search)
        {
            using (var db = new CommunicateV1Entities())
            {
                var nStartDate = Convert.ToDateTime(startDate);

                var nEndDate = Convert.ToDateTime(endDate);
                int? smsType = null;
                if (smsTypeId != -1)
                {
                    smsType = smsTypeId;
                }
                var lst = db.usp_Transaction_select(nStartDate, nEndDate, smsType, search, pageStart, pageSize, sortField, orderType).ToList();

                var dt = new Dictionary<string, object>
                {
                    {"draw",draw },
                    {"recordsTotal", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"recordsFiltered", lst.Count() != 0 ? lst.FirstOrDefault()?.TotalCount : lst.Count()},
                    {"data", lst.ToList()}
                };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetReportSentSMS(string dateFrom, string dateTo)
        {
            using (var db = new CommunicateV1Entities())
            {
                var nDateFrom = Convert.ToDateTime(dateFrom);

                var nDateTo = Convert.ToDateTime(dateTo);

                var report = db.usp_zReportSentSMS_Select(nDateFrom, nDateTo).ToList();

                var dt = new Dictionary<string, object>
                {
                    {"data", report.ToList()}
                };
                return Json(dt, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion Method

        #region Excel

        /// <summary>
        /// Export Excel แบบเก่า
        /// </summary>
        /// <param name="headerId"></param>
        public void ExportToExcel(int headerId)
        {
            using (var db = new CommunicateV1Entities())
            {
                var d = DateTime.Now;
                var lst1 = db.usp_Report_SMSDetail_Select(headerId).ToList();
                var lst2 = db.usp_Report_SMSSubDetail_Select(headerId).ToList();

                var dt1 = GlobalFunction.ToDataTable(lst1);
                var dt2 = GlobalFunction.ToDataTable(lst2);

                GlobalFunction.ExportToExcel(HttpContext, "ReportSMS_" + d, dt1, "SMS", dt2, "Detail");
            }
        }

        /// <summary>
        /// Export Excel SMS Detail
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="smsTypeId"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ExportSMSDetail(int headerId)
        {
            await Task.Yield();
            using (var db = new CommunicateV1Entities())
            {
                var lst1 = db.usp_Report_SMSDetail_Select(headerId).ToList();
                var lst2 = db.usp_Report_SMSSubDetail_Select(headerId).ToList();

                var stream = new MemoryStream();

                using (var package = new ExcelPackage(stream))
                {
                    var workSheet1 = package.Workbook.Worksheets.Add("SMS");
                    var workSheet2 = package.Workbook.Worksheets.Add("Detail");
                    workSheet1.Cells.LoadFromCollection(lst1, true);
                    workSheet2.Cells.LoadFromCollection(lst2, true);

                    // Select only the header cells
                    var headerCells1 = workSheet1.Cells[1, 1, 1, workSheet1.Dimension.Columns];
                    // Set their text to bold.
                    headerCells1.Style.Font.Bold = true;
                    // Set their background color
                    headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells1.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                    // Apply the auto-filter to all the columns
                    var allCells1 = workSheet1.Cells[workSheet1.Dimension.Address];
                    allCells1.AutoFilter = true;
                    // Auto-fit all the columns
                    allCells1.AutoFitColumns();

                    // Select only the header cells
                    var headerCells2 = workSheet2.Cells[1, 1, 1, workSheet2.Dimension.Columns];
                    // Set their text to bold.
                    headerCells2.Style.Font.Bold = true;
                    // Set their background color
                    headerCells2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells2.Style.Fill.BackgroundColor.SetColor(Color.DeepSkyBlue);
                    // Apply the auto-filter to all the columns
                    var allCells2 = workSheet2.Cells[workSheet2.Dimension.Address];
                    allCells2.AutoFilter = true;
                    // Auto-fit all the columns
                    allCells2.AutoFitColumns();

                    package.Save();

                    stream.Position = 0;

                    //get new GUID
                    var dataSessionKey = Guid.NewGuid().ToString();
                    //tempData GUID
                    TempData["keySMSDetail"] = dataSessionKey;
                    //Session Data
                    Session[dataSessionKey] = package.GetAsByteArray();

                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// Download File
        /// </summary>
        /// <returns></returns>
        public ActionResult DownloadExportSMSDetail()
        {
            var dataKey = TempData["keySMSDetail"].ToString();

            if (Session[dataKey] != null)
            {
                byte[] data = Session[dataKey] as byte[];
                string excelName = $"Report-SMS-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        /// <summary>
        /// Export Excel Report
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="smsTypeId"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ExportReport(string startDate, string endDate, int? smsTypeId, string search)
        {
            await Task.Yield();
            using (var db = new CommunicateV1Entities())
            {
                var nStartDate = Convert.ToDateTime(startDate);

                var nEndDate = Convert.ToDateTime(endDate);
                int? smsType = null;
                if (smsTypeId != -1)
                {
                    smsType = smsTypeId;
                }

                var lst = db.usp_Transaction_select(nStartDate, nEndDate, smsType, search, 0, 999999, null, null).ToList();

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
                    //get new GUID
                    var dataSessionKey = Guid.NewGuid().ToString();
                    //tempData GUID
                    TempData["keyReport"] = dataSessionKey;
                    //Session Data
                    Session[dataSessionKey] = package.GetAsByteArray();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// Download file
        /// </summary>
        /// <returns></returns>
        public ActionResult DownloadExportReport()
        {
            var dataKey = TempData["keyReport"].ToString();

            if (Session[dataKey] != null)
            {
                byte[] data = Session[dataKey] as byte[];
                string excelName = $"Report-SMS-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpGet]
        public async Task<ActionResult> ExportReportSentSMS(string dateFrom, string dateTo)
        {
            await Task.Yield();
            using (var db = new CommunicateV1Entities())
            {
                var nDateFrom = Convert.ToDateTime(dateFrom);

                var nDateTo = Convert.ToDateTime(dateTo);

                var report = db.usp_zReportSentSMS_Select(nDateFrom, nDateTo).ToList();

                var stream = new MemoryStream();

                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(report, true);

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
                    //get new GUID
                    var dataSessionKey = Guid.NewGuid().ToString();
                    //tempData GUID
                    TempData["CreditUtilization"] = dataSessionKey;
                    //Session Data
                    Session[dataSessionKey] = package.GetAsByteArray();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult DownloadExportReportSentSMS()
        {
            var dataKey = TempData[key: "CreditUtilization"].ToString();

            if (Session[dataKey] != null)
            {
                byte[] data = Session[dataKey] as byte[];
                string excelName = $"Report-Sent-SMS-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        #endregion Excel

        public class SMSSend
        {
            public int ProjectId { get; set; }
            public int SMSTypeId { get; set; }
            public string Remark { get; set; }
            public int Total { get; set; }
            public string SendDate { get; set; }
            public ImportExcel[] Data { get; set; }
        }

        public class ImportExcel
        {
            public String PhoneNo { get; set; }
            public string Message { get; set; }
        }

        public class SMSResult
        {
            public string status { get; set; }
            public string detail { get; set; }
            public string referenceHeaderID { get; set; }
        }

        public class SMSSendV2
        {
            public int ProjectId { get; set; }
            public int SMSTypeId { get; set; }
            public string Remark { get; set; }
            public int Total { get; set; }
            public string SendDate { get; set; }
            public ImportExcelV2[] Data { get; set; }
        }

        public class ImportExcelV2
        {
            public string Ref { get; set; }
            public String PhoneNo { get; set; }
            public string Message { get; set; }
        }

        public class SMSResultV2
        {
            public string status { get; set; }
            public string detail { get; set; }
            public string referenceHeaderID { get; set; }
        }
    }
}