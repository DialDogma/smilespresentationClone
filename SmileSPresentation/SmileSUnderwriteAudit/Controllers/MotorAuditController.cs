using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using RestSharp;
using SmileSUnderwriteAudit.Helper;
using SmileSUnderwriteAudit.Models;
using SmileUnderwriteBranchV2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace SmileSUnderwriteAudit.Controllers
{
    [Authorization]
    public class MotorAuditController : Controller
    {
        #region Context

        private readonly UnderwriteMotorAuditEntity _context;
        private readonly UnderwriteBranchV2MotorEntities _contextMotor;
        private readonly UnderwriteBranchV2Entities _contextMotorBranchV2;

        public MotorAuditController()
        {
            _context = new UnderwriteMotorAuditEntity();
            _contextMotor = new UnderwriteBranchV2MotorEntities();
            _contextMotorBranchV2 = new UnderwriteBranchV2Entities();
        }

        #endregion Context

        #region View

        // GET: MotorAudit
        public ActionResult MotorAuditIndex(string motorQueueAuditId)
        {
            var encodeId = motorQueueAuditId;
            if (encodeId != null && encodeId != string.Empty)
            {
                //decode
                byte[] data = Convert.FromBase64String(encodeId);
                string decodedString = Encoding.UTF8.GetString(data);

                //call stored procedure
                var result = _context.usp_QueueMotorAuditFullDetailByQueueAuditId_Select(Convert.ToInt32(decodedString)).FirstOrDefault();
                var OpenGoogleDoc = Properties.Settings.Default.GoogleDocPathMT;
                try
                {
                    var appCode = result.ApplicationCode;
                    var insureName = result.InsuredFirstName + " " + result.InsuredLastName;
                    var payerName = result.PayerFirstName + " " + result.PayerLastName;

                    string appStartCoverDateFormatted = result.Period.HasValue
                                             ? result.Period.Value.ToString("d/M/yyyy", new System.Globalization.CultureInfo("th-TH"))
                                             : string.Empty;

                    string modifiedURL = OpenGoogleDoc.Replace("AppID", result.ApplicationCode)
                                                          .Replace("Name1", insureName)
                                                          .Replace("Name2", payerName)
                                                          .Replace("DCR", appStartCoverDateFormatted);

                    ViewBag.OpenGoogleDoc = modifiedURL;
                }
                catch
                {
                    ViewBag.OpenGoogleDoc = OpenGoogleDoc;
                }
                //ถ้า null return notfouond
                if (result is null) return RedirectToAction("NotFound", "Error", new { Msg = "เลข App NotFound" });
                #region Documents


                #region Documents


                //new Documents from docStorage
                var documentResult = _contextMotorBranchV2.usp_DocumentUnderwriteByQueueId_Select(result.QueueId, 2).ToList();
                var docstorage = Properties.Settings.Default.docstorage;
                ViewBag.docstorage = docstorage;


                if (documentResult != null)
                {
                    ViewBag.documentResult = documentResult;
                    var documentIdList = documentResult.Select(x => x.DocumentId).ToList();

                    var docstorageApi = Properties.Settings.Default.docstorageApi;

                    string apiUrl = docstorageApi;

                    // Create a RestSharp client
                    var client = new RestClient(apiUrl);

                    // Create a request with parameters
                    var request = new RestRequest(Method.GET);
                    /* request.AddParameter("documentIds", "value1");
                     request.AddParameter("param2", "value2");*/
                    foreach (var item in documentIdList)
                    {
                        request.AddParameter("documentIds", item);
                    }


                    // Execute the request and get the response
                    IRestResponse response = client.Execute(request);

                    // Check if the request was successful (statu s code 200)
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.ApiResponse = response.Content;
                        var dataResponse = JsonConvert.DeserializeObject<DataResponse>(response.Content);
                        ViewBag.docFile = dataResponse.data.ToList();
                        var dataResponseMerge = dataResponse.data.ToList();


                    }

                }




                #endregion Documents
                if (documentResult != null)
                {
                    ViewBag.documentResult = documentResult;
                    var documentIdList = documentResult.Select(x => x.DocumentId).ToList();

                    var docstorageApi = Properties.Settings.Default.docstorageApi;

                    string apiUrl = docstorageApi;

                    // Create a RestSharp client
                    var client = new RestClient(apiUrl);

                    // Create a request with parameters
                    var request = new RestRequest(Method.GET);
                    /* request.AddParameter("documentIds", "value1");
                     request.AddParameter("param2", "value2");*/
                    foreach (var item in documentIdList)
                    {
                        request.AddParameter("documentIds", item);
                    }


                    // Execute the request and get the response
                    IRestResponse response = client.Execute(request);

                    // Check if the request was successful (statu s code 200)
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.ApiResponse = response.Content;
                        var dataResponse = JsonConvert.DeserializeObject<DataResponse>(response.Content);
                        ViewBag.docFile = dataResponse.data.ToList();
                        var dataResponseMerge = dataResponse.data.ToList();


                    }

                }

                #endregion Documents



                //Get Role
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                var accessRole = GlobalFunction.GetAccessRoleUnderWriteAudit(roleList);
                ViewBag.AccessRole = accessRole;

                //set app code
                ViewBag.AppCode = result.ApplicationCode;
                ViewBag.MotorQueueAuditId = decodedString;

                //1 = na ให้ set default เป็น 2
                ViewBag.QueueAuditStatusId = (result.QueueAuditStatusId == 1 ? 2 : result.QueueAuditStatusId);
                ViewBag.QueueMotorFullDetail = result;
                var applicationCode = result.ApplicationCode;

                //StartCoverDate BE 2 ตำแหน่ง
                if (result.StartCoverDate != null)
                {
                    ViewBag.StartCoverDate = string.Format("{0}/{1}/{2}", result.StartCoverDate.Value.Day.ToString("00", null), result.StartCoverDate.Value.Month.ToString("00", null), result.StartCoverDate.Value.Year + 543);
                }

                //EndCoverDate BE 2 ตำแหน่ง
                if (result.EndCoverDate != null)
                {
                    ViewBag.EndCoverDate = string.Format("{0}/{1}/{2}", result.EndCoverDate.Value.Day.ToString("00", null), result.EndCoverDate.Value.Month.ToString("00", null), result.EndCoverDate.Value.Year + 543);
                }

                //ปีจดทะเบียน BE
                ViewBag.VehicleRegistrationYear = result.VehicleRegistrationYear;
                if (result.VehicleRegistrationYear != "" || result.VehicleRegistrationYear != null)
                {
                    ViewBag.VehicleRegistrationYear = Convert.ToInt32(result.VehicleRegistrationYear) + 543;
                }

                //IssuesPolicyDate
                if (result.IssuesPolicyDate != null)
                {
                    ViewBag.IssuesPolicyDate = string.Format("{0}/{1}/{2} {3}", result.IssuesPolicyDate.Value.Day.ToString("00", null), result.IssuesPolicyDate.Value.Month.ToString("00", null), result.IssuesPolicyDate.Value.Year + 543, result.IssuesPolicyDate.Value.ToString("HH:mm:ss"));
                }

                //ข้อมูลผู้ใช้รถ (ผู้ที่ใช้รถประจำ/ผู้ใช้รถบ่อยที่สุด)
                if (result.DriverFirstName == result.PayerFirstName && result.DriverLastName == result.PayerLastName)
                {
                    ViewBag.Driver = "ผู้ชำระเบี้ย";
                }
                else if (result.DriverFirstName == result.InsuredFirstName && result.DriverLastName == result.InsuredLastName)
                {
                    ViewBag.Driver = "ผู้เอาประกัน";
                }
                else
                {
                    ViewBag.Driver = "บุคคลอื่น";
                }

                var result2 = _context.usp_QueueMotorAuditStatus_Select(null).ToList();
                ViewBag.QueueAuditStatus = result2;

                var result3 = _context.usp_CallStatusMotor_Select(null).ToList();
                ViewBag.CallStatusListAuditMotor = result3;

                //var result4 = _context.usp_AuditUnderwriteResultStatus_Select(null).ToList();
                //ViewBag.AuditUnderwriteResultStatus = result4;

                var result5 = _context.usp_AuditMotorUnderwriteStatus_Select(null).ToList();
                ViewBag.AuditMotorStatus = result5;

                var result6 = _context.usp_AuditMotorSpecifyStatus_Select(null).ToList();
                ViewBag.AuditMotorSpecifyStatus = result6;

                var result7 = _contextMotor.usp_UnderwriteTypeMotor_Select(null).ToList();
                ViewBag.UnderwriteType = result7;

                var result15 = _contextMotor.usp_UnderwritePaymentTypeMotor_Select(null).ToList();
                ViewBag.UnderwritePaymentTypeMotor = result15;

                var result8 = _context.usp_AuditMotorPaymentStatus_Select(null).ToList();
                ViewBag.AuditMotorPaymentType = result8;

                var result14 = _context.usp_AuditMotorStatus_Select(null).ToList();
                ViewBag.AuditMotorStatus = result14;

                //var result9 = _contextMotor.usp_CallStatusMotor_Select(null).ToList();
                //ViewBag.CallStatusListUDWBranch = result9;
                var result9 = new List<CallStatusMotorUnderWrite>
                {
                    new CallStatusMotorUnderWrite{CallStatusId = 2, CallStatus = "รับสาย" },
                    new CallStatusMotorUnderWrite{CallStatusId = 3, CallStatus = "ไม่รับสาย"},
                    new CallStatusMotorUnderWrite{CallStatusId = 4, CallStatus = "ไม่สะดวกคุย"},
                    new CallStatusMotorUnderWrite{CallStatusId = 5, CallStatus = "ติดต่อไม่ได้"}
                };
                ViewBag.CallStatusListUDWBranch = result9;

                var result10 = _context.usp_AuditMotorInsureStatus_Select(null).ToList();
                ViewBag.AuditInsureStatus = result10;

                var result11 = _context.usp_AuditMotorInsureConsiderStatus_Select(null).ToList();
                ViewBag.AuditInsureConsiderStatus = result11;

                var result12 = _context.usp_AuditMotorUnderwriteStatus_Select(null).ToList();
                ViewBag.AuditUnderwriteStatus = result12;

                var result13 = _context.usp_AuditMotorPoliciesGivenStatus_Select(null).ToList();
                ViewBag.AuditMotorPoliciesGivenStatus = result13;

                //เช็คประวัติ หมายเลขตัวถัง
                var count1 = _contextMotor.usp_QueueMotorUnderwriteHistory_Select(result.QueueId, 0, null, null, null).ToList().Count();
                ViewBag.CountChassisNumber = count1;

                //เช็คประวัติ ประวัติการชำระเบี้ยให้กับแอปอื่นๆ
                var count2 = _contextMotor.usp_QueueMotorPaymentHistory_Select(result.QueueId, 0, null, null, null).ToList().Count();
                ViewBag.CountPayment = count2;

                return View();
            }
            else
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
            }
        }

        // GET: MotorAudit
        public ActionResult MotorAuditIndexV2(string motorQueueAuditId)
        {
            var encodeId = motorQueueAuditId;
            if (encodeId != null && encodeId != string.Empty)
            {
                //decode
                byte[] data = Convert.FromBase64String(encodeId);
                string decodedString = Encoding.UTF8.GetString(data);

                //call stored procedure
                var result = _context.usp_QueueMotorAuditFullDetailByQueueAuditIdV2_Select(Convert.ToInt32(decodedString)).FirstOrDefault();




                //ถ้า null return notfouond
                if (result is null) return RedirectToAction("MotorAuditIndex", new { motorQueueAuditId = motorQueueAuditId });

                var startNewVersionMT = Properties.Settings.Default.StartNewVersionMT;
                DateTime? oldPeriod = Convert.ToDateTime(startNewVersionMT);
                DateTime? nPeriod = Convert.ToDateTime(result.Period);
                if (oldPeriod > nPeriod)
                {
                    return RedirectToAction("MotorAuditIndex", new { motorQueueAuditId = motorQueueAuditId });
                }

                #region Documents


                #region Documents


                //new Documents from docStorage
                var documentResult = _contextMotorBranchV2.usp_DocumentUnderwriteByQueueId_Select(result.QueueId, 2).ToList();
                var docstorage = Properties.Settings.Default.docstorage;
                ViewBag.docstorage = docstorage;


                if (documentResult != null)
                {
                    ViewBag.documentResult = documentResult;
                    var documentIdList = documentResult.Select(x => x.DocumentId).ToList();

                    var docstorageApi = Properties.Settings.Default.docstorageApi;

                    string apiUrl = docstorageApi;

                    // Create a RestSharp client
                    var client = new RestClient(apiUrl);

                    // Create a request with parameters
                    var request = new RestRequest(Method.GET);
                    /* request.AddParameter("documentIds", "value1");
                     request.AddParameter("param2", "value2");*/
                    foreach (var item in documentIdList)
                    {
                        request.AddParameter("documentIds", item);
                    }


                    // Execute the request and get the response
                    IRestResponse response = client.Execute(request);

                    // Check if the request was successful (statu s code 200)
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.ApiResponse = response.Content;
                        var dataResponse = JsonConvert.DeserializeObject<DataResponse>(response.Content);
                        ViewBag.docFile = dataResponse.data.ToList();
                        var dataResponseMerge = dataResponse.data.ToList();


                    }

                }


                #endregion Documents
                if (documentResult != null)
                {
                    ViewBag.documentResult = documentResult;
                    var documentIdList = documentResult.Select(x => x.DocumentId).ToList();

                    var docstorageApi = Properties.Settings.Default.docstorageApi;

                    string apiUrl = docstorageApi;

                    // Create a RestSharp client
                    var client = new RestClient(apiUrl);

                    // Create a request with parameters
                    var request = new RestRequest(Method.GET);
                    foreach (var item in documentIdList)
                    {
                        request.AddParameter("documentIds", item);
                    }
                    // Execute the request and get the response
                    IRestResponse response = client.Execute(request);

                    // Check if the request was successful (statu s code 200)
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.ApiResponse = response.Content;
                        var dataResponse = JsonConvert.DeserializeObject<DataResponse>(response.Content);
                        ViewBag.docFile = dataResponse.data.ToList();
                        var dataResponseMerge = dataResponse.data.ToList();
                    }

                }


                #endregion Documents

                //Get Role
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                var accessRole = GlobalFunction.GetAccessRoleUnderWriteAudit(roleList);
                ViewBag.AccessRole = accessRole;

                //set app code
                ViewBag.AppCode = result.ApplicationCode;
                ViewBag.MotorQueueAuditId = decodedString;

                //1 = na ให้ set default เป็น 2
                ViewBag.QueueAuditStatusId = (result.QueueAuditStatusId == 1 ? 2 : result.QueueAuditStatusId);
                ViewBag.QueueMotorFullDetail = result;
                var applicationCode = result.ApplicationCode;

                /* //StartCoverDate BE 2 ตำแหน่ง
                 if (result.StartCoverDate != null)
                 {
                     ViewBag.StartCoverDate = string.Format("{0}/{1}/{2}", result.StartCoverDate.Value.Day.ToString("00", null), result.StartCoverDate.Value.Month.ToString("00", null), result.StartCoverDate.Value.Year + 543);
                 }

                 //EndCoverDate BE 2 ตำแหน่ง
                 if (result.EndCoverDate != null)
                 {
                     ViewBag.EndCoverDate = string.Format("{0}/{1}/{2}", result.EndCoverDate.Value.Day.ToString("00", null), result.EndCoverDate.Value.Month.ToString("00", null), result.EndCoverDate.Value.Year + 543);
                 }

                 //ปีจดทะเบียน BE
                 ViewBag.VehicleRegistrationYear = result.VehicleRegistrationYear;
                 if (result.VehicleRegistrationYear != "" || result.VehicleRegistrationYear != null)
                 {
                     ViewBag.VehicleRegistrationYear = Convert.ToInt32(result.VehicleRegistrationYear) + 543;
                 }

                 //IssuesPolicyDate
                 if (result.IssuesPolicyDate != null)
                 {
                     ViewBag.IssuesPolicyDate = string.Format("{0}/{1}/{2} {3}", result.IssuesPolicyDate.Value.Day.ToString("00", null), result.IssuesPolicyDate.Value.Month.ToString("00", null), result.IssuesPolicyDate.Value.Year + 543, result.IssuesPolicyDate.Value.ToString("HH:mm:ss"));
                 }

                 //ข้อมูลผู้ใช้รถ (ผู้ที่ใช้รถประจำ/ผู้ใช้รถบ่อยที่สุด)
                 if (result.DriverFirstName == result.PayerFirstName && result.DriverLastName == result.PayerLastName)
                 {
                     ViewBag.Driver = "ผู้ชำระเบี้ย";
                 }
                 else if (result.DriverFirstName == result.InsuredFirstName && result.DriverLastName == result.InsuredLastName)
                 {
                     ViewBag.Driver = "ผู้เอาประกัน";
                 }
                 else
                 {
                     ViewBag.Driver = "บุคคลอื่น";
                 }*/

                var result2 = _context.usp_QueueMotorAuditStatus_Select(null).ToList();
                ViewBag.QueueAuditStatus = result2;

                var result3 = _context.usp_CallStatusMotor_Select(null).ToList();
                ViewBag.CallStatusListAuditMotor = result3;

                var result5 = _context.usp_AuditMotorUnderwriteStatus_Select(null).ToList();
                ViewBag.AuditMotorStatus = result5;

                var result6 = _context.usp_AuditMotorSpecifyStatus_Select(null).ToList();
                ViewBag.AuditMotorSpecifyStatus = result6;

                var result7 = _contextMotor.usp_UnderwriteTypeMotor_Select(null).ToList();
                ViewBag.UnderwriteType = result7;

                var result15 = _contextMotor.usp_UnderwritePaymentTypeMotor_Select(null).ToList();
                ViewBag.UnderwritePaymentTypeMotor = result15;

                var result8 = _context.usp_AuditMotorPaymentStatus_Select(null).ToList();
                ViewBag.AuditMotorPaymentType = result8;

                var result14 = _context.usp_AuditMotorStatus_Select(null).ToList();
                ViewBag.AuditMotorStatus = result14;

                var resultSaleServiceType = _context.usp_SaleServiceType_Select(null, null).ToList();
                ViewBag.SaleServiceType = resultSaleServiceType;

                var resultSaleMethodType = _context.usp_SaleMethodType_Select(null, null).ToList();
                ViewBag.SaleMethodType = resultSaleMethodType;


                var resultUnderwritingBehaviorType = _context.usp_UnderwritingBehaviorType_Select(null, null).ToList();
                ViewBag.UnderwritingBehaviorType = resultUnderwritingBehaviorType;

                var resultNotAuditedCause = _context.usp_NotAuditedCause_Select(null, null).ToList();
                ViewBag.NotAuditedCause = resultNotAuditedCause;

                var VehicleUseType = _context.usp_VehicleUseType_Select(null, null).ToList();
                ViewBag.VehicleUseType = VehicleUseType;

                var PositionDamaged = _context.usp_PositionDamaged_Select(null, null).ToList();
                ViewBag.PositionDamaged = PositionDamaged;

                var ReceivingInsuranceType = _context.usp_ReceivingInsuranceType_Select(null, null).ToList();
                ViewBag.ReceivingInsuranceType = ReceivingInsuranceType;

                var result9 = new List<CallStatusMotorUnderWrite>
                {
                    new CallStatusMotorUnderWrite{CallStatusId = 2, CallStatus = "รับสาย" },
                    new CallStatusMotorUnderWrite{CallStatusId = 3, CallStatus = "ไม่รับสาย"},
                    new CallStatusMotorUnderWrite{CallStatusId = 4, CallStatus = "ไม่สะดวกคุย"},
                    new CallStatusMotorUnderWrite{CallStatusId = 5, CallStatus = "ติดต่อไม่ได้"}
                };
                ViewBag.CallStatusListUDWBranch = result9;

                var result10 = _context.usp_AuditMotorInsureStatus_Select(null).ToList();
                ViewBag.AuditInsureStatus = result10;

                var result11 = _context.usp_AuditMotorInsureConsiderStatus_Select(null).ToList();
                ViewBag.AuditInsureConsiderStatus = result11;

                var result12 = _context.usp_AuditMotorUnderwriteStatus_Select(null).ToList();
                ViewBag.AuditUnderwriteStatus = result12;

                var result13 = _context.usp_AuditMotorPoliciesGivenStatus_Select(null).ToList();
                ViewBag.AuditMotorPoliciesGivenStatus = result13;

                /* //เช็คประวัติ หมายเลขตัวถัง
                 var count1 = _contextMotor.usp_QueueMotorUnderwriteHistory_Select(result.QueueId, 0, null, null, null).ToList().Count();
                 ViewBag.CountChassisNumber = count1;

                 //เช็คประวัติ ประวัติการชำระเบี้ยให้กับแอปอื่นๆ
                 var count2 = _contextMotor.usp_QueueMotorPaymentHistory_Select(result.QueueId, 0, null, null, null).ToList().Count();
                 ViewBag.CountPayment = count2;*/

                return View();
            }
            else
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
            }
        }
        public class DataResponse
        {
            public List<subDataResponse> data { get; set; }
            public bool isSuccess { get; set; }
            public DateTime serverDateTime { get; set; }

            // Add properties for other keys as needed
        }
        private DataResponse DocStorage(List<Guid> documentIdList)
        {
            // Replace the URL with your actual API endpoint
            var docstorageApi = Properties.Settings.Default.docstorageApi;

            string apiUrl = docstorageApi;

            // Create a RestSharp client
            var client = new RestClient(apiUrl);

            // Create a request with parameters
            var request = new RestRequest(Method.GET);
            /* request.AddParameter("documentIds", "value1");
             request.AddParameter("param2", "value2");*/

            foreach (var item in documentIdList)
            {
                request.AddParameter("documentIds", item);
            }


            // Execute the request and get the response
            IRestResponse response = client.Execute(request);

            // Check if the request was successful (statu s code 200)
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ViewBag.ApiResponse = response.Content;
                var dataResponse = JsonConvert.DeserializeObject<DataResponse>(response.Content);
                return dataResponse;
            }
            else
            {
                // Handle the error, for example, display an error message
                ViewBag.ErrorMessage = $"Error: {response.StatusCode} - {response.StatusDescription}";
                return null;
            }


        }

        public class subDataResponse
        {

            public string documentId { get; set; }
            public string documentCode { get; set; }
            public string mainIndex { get; set; }
            public string searchIndex { get; set; }
            public string documentTypeName { get; set; }
            public int fileCount { get; set; }

        }
        public ActionResult MotorAuditView(string motorQueueAuditId)
        {
            var encodeId = motorQueueAuditId;
            if (encodeId != null && encodeId != string.Empty)
            {
                //decode
                byte[] data = Convert.FromBase64String(encodeId);
                string decodedString = Encoding.UTF8.GetString(data);

                //call stored procedure
                var result = _context.usp_QueueMotorAuditFullDetailByQueueAuditId_Select(Convert.ToInt32(decodedString)).FirstOrDefault();

                //ถ้า null return notfouond
                if (result is null) return RedirectToAction("NotFound", "Error", new { Msg = "เลข App NotFound" });

                //Get Role
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                var accessRole = GlobalFunction.GetAccessRoleUnderWriteAudit(roleList);
                ViewBag.AccessRole = accessRole;

                //set app code
                ViewBag.AppCode = result.ApplicationCode;

                //1 = na ให้ set default เป็น 2
                ViewBag.QueueAuditStatusId = (result.QueueAuditStatusId == 1 ? 2 : result.QueueAuditStatusId);
                ViewBag.QueueMotorFullDetail = result;
                var applicationCode = result.ApplicationCode;

                ViewBag.MotorQueueAuditId = decodedString;

                //ปีจดทะเบียน BE
                ViewBag.VehicleRegistrationYear = result.VehicleRegistrationYear;
                if (result.VehicleRegistrationYear != "" || result.VehicleRegistrationYear != null)
                {
                    ViewBag.VehicleRegistrationYear = Convert.ToInt32(result.VehicleRegistrationYear) + 543;
                }

                //IssuesPolicyDate
                if (result.IssuesPolicyDate != null)
                {
                    ViewBag.IssuesPolicyDate = string.Format("{0}/{1}/{2} {3}", result.IssuesPolicyDate.Value.Day.ToString("00", null), result.IssuesPolicyDate.Value.Month.ToString("00", null), result.IssuesPolicyDate.Value.Year + 543, result.IssuesPolicyDate.Value.ToString("HH:mm:ss"));
                }

                //StartCoverDate BE 2 ตำแหน่ง
                if (result.StartCoverDate != null)
                {
                    ViewBag.StartCoverDate = string.Format("{0}/{1}/{2}", result.StartCoverDate.Value.Day.ToString("00", null), result.StartCoverDate.Value.Month.ToString("00", null), result.StartCoverDate.Value.Year + 543);
                }

                //EndCoverDate BE 2 ตำแหน่ง
                if (result.EndCoverDate != null)
                {
                    ViewBag.EndCoverDate = string.Format("{0}/{1}/{2}", result.EndCoverDate.Value.Day.ToString("00", null), result.EndCoverDate.Value.Month.ToString("00", null), result.EndCoverDate.Value.Year + 543);
                }

                //ข้อมูลผู้ใช้รถ (ผู้ที่ใช้รถประจำ/ผู้ใช้รถบ่อยที่สุด)
                if (result.DriverFirstName == result.PayerFirstName && result.DriverLastName == result.PayerLastName)
                {
                    ViewBag.Driver = "ผู้ชำระเบี้ย";
                }
                else if (result.DriverFirstName == result.InsuredFirstName && result.DriverLastName == result.InsuredLastName)
                {
                    ViewBag.Driver = "ผู้เอาประกัน";
                }
                else
                {
                    ViewBag.Driver = "บุคคลอื่น";
                }

                var result2 = _context.usp_QueueMotorAuditStatus_Select(null).ToList();
                ViewBag.QueueAuditStatus = result2;

                var result3 = _context.usp_CallStatusMotor_Select(null).ToList();
                ViewBag.CallStatusListAuditMotor = result3;

                //var result4 = _context.usp_AuditUnderwriteResultStatus_Select(null).ToList();
                //ViewBag.AuditUnderwriteResultStatus = result4;

                var result5 = _context.usp_AuditMotorUnderwriteStatus_Select(null).ToList();
                ViewBag.AuditMotorStatus = result5;

                var result6 = _context.usp_AuditMotorSpecifyStatus_Select(null).ToList();
                ViewBag.AuditMotorSpecifyStatus = result6;

                var result7 = _contextMotor.usp_UnderwriteTypeMotor_Select(null).ToList();
                ViewBag.UnderwriteType = result7;

                var result8 = _context.usp_AuditMotorPaymentStatus_Select(null).ToList();
                ViewBag.AuditMotorPaymentType = result8;

                var result14 = _context.usp_AuditMotorStatus_Select(null).ToList();
                ViewBag.AuditMotorStatus = result14;

                //payment type ใน underWriteMotor
                var result15 = _contextMotor.usp_UnderwritePaymentTypeMotor_Select(null).ToList();
                ViewBag.UnderwritePaymentTypeMotor = result15;

                //var result9 = _context.usp_CallStatusMotor_Select(null).ToList();
                //ViewBag.CallStatusListUDWBranch = result9;

                //ชื่อสโตรใน Audit และ UnderWritrMotor เหมือนกันเลยต้องทำแบปนี้ไปก่อน
                var result9 = new List<CallStatusMotorUnderWrite>
                {
                    new CallStatusMotorUnderWrite{CallStatusId = 2, CallStatus = "รับสาย" },
                    new CallStatusMotorUnderWrite{CallStatusId = 3, CallStatus = "ไม่รับสาย"},
                    new CallStatusMotorUnderWrite{CallStatusId = 4, CallStatus = "ไม่สะดวกคุย"},
                    new CallStatusMotorUnderWrite{CallStatusId = 5, CallStatus = "ติดต่อไม่ได้"}
                };
                ViewBag.CallStatusListUDWBranch = result9;

                var result10 = _context.usp_AuditMotorInsureStatus_Select(null).ToList();
                ViewBag.AuditInsureStatus = result10;

                var result11 = _context.usp_AuditMotorInsureConsiderStatus_Select(null).ToList();
                ViewBag.AuditInsureConsiderStatus = result11;

                var result12 = _context.usp_AuditMotorUnderwriteStatus_Select(null).ToList();
                ViewBag.AuditUnderwriteStatus = result12;

                var result13 = _context.usp_AuditMotorPoliciesGivenStatus_Select(null).ToList();
                ViewBag.AuditMotorPoliciesGivenStatus = result13;

                //เช็คประวัติ หมายเลขตัวถัง
                var count1 = _contextMotor.usp_QueueMotorUnderwriteHistory_Select(result.QueueId, 0, null, null, null).ToList().Count();
                ViewBag.CountChassisNumber = count1;

                //เช็คประวัติ ประวัติการชำระเบี้ยให้กับแอปอื่นๆ
                var count2 = _contextMotor.usp_QueueMotorPaymentHistory_Select(result.QueueId, 0, null, null, null).ToList().Count();
                ViewBag.CountPayment = count2;

                #region Documents

                #region บัตรประชาชน

                ViewBag.idCardAttachedFile = null;
                if ((!String.IsNullOrEmpty(result.IdCardAttachedFile)) && result.IdCardAttachedFile != "N/A")
                {
                    Documents idCardAttachedFile = JsonConvert.DeserializeObject<Documents>(result.IdCardAttachedFile);
                    ViewBag.idCardAttachedFile = idCardAttachedFile.images;
                }
                #region Documents


                //new Documents from docStorage
                var documentResult = _contextMotorBranchV2.usp_DocumentUnderwriteByQueueId_Select(result.QueueId, 2).ToList();
                var docstorage = Properties.Settings.Default.docstorage;
                ViewBag.docstorage = docstorage;


                if (documentResult != null)
                {
                    ViewBag.documentResult = documentResult;
                    var documentIdList = documentResult.Select(x => x.DocumentId).ToList();

                    var docstorageApi = Properties.Settings.Default.docstorageApi;

                    string apiUrl = docstorageApi;

                    // Create a RestSharp client
                    var client = new RestClient(apiUrl);

                    // Create a request with parameters
                    var request = new RestRequest(Method.GET);
                    /* request.AddParameter("documentIds", "value1");
                     request.AddParameter("param2", "value2");*/
                    foreach (var item in documentIdList)
                    {
                        request.AddParameter("documentIds", item);
                    }


                    // Execute the request and get the response
                    IRestResponse response = client.Execute(request);

                    // Check if the request was successful (statu s code 200)
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.ApiResponse = response.Content;
                        var dataResponse = JsonConvert.DeserializeObject<DataResponse>(response.Content);
                        ViewBag.docFile = dataResponse.data.ToList();
                        var dataResponseMerge = dataResponse.data.ToList();


                    }

                }




                #endregion Documents
                #endregion บัตรประชาชน

                #region รายการจดทะเบียน

                ViewBag.regisBookAttachedFile = null;
                if ((!String.IsNullOrEmpty(result.RegisBookAttachedFile)) && result.RegisBookAttachedFile != "N/A")
                {
                    Documents regisBookAttachedFile = JsonConvert.DeserializeObject<Documents>(result.RegisBookAttachedFile);
                    ViewBag.regisBookAttachedFile = regisBookAttachedFile.images;
                }

                #endregion รายการจดทะเบียน

                #region รูปภาพ

                ViewBag.PhotoAttachedFile = null;
                if ((!String.IsNullOrEmpty(result.PhotoAttachedFile)) && result.PhotoAttachedFile != "N/A")
                {
                    Documents photoAttachedFile = JsonConvert.DeserializeObject<Documents>(result.PhotoAttachedFile);
                    ViewBag.PhotoAttachedFile = photoAttachedFile.images;
                }

                #endregion รูปภาพ

                #region ใบเสร็จ

                ViewBag.paymentSlipAttachedFile = null;
                if ((!String.IsNullOrEmpty(result.PaymentSlipAttachedFile)) && result.PaymentSlipAttachedFile != "N/A")
                {
                    Documents paymentSlipAttachedFile = JsonConvert.DeserializeObject<Documents>(result.PaymentSlipAttachedFile);
                    ViewBag.paymentSlipAttachedFile = paymentSlipAttachedFile.images;
                }

                #endregion ใบเสร็จ

                #region หนังสือยินยอม

                ViewBag.debitAuthorizeAttachedFile = null;
                if ((!String.IsNullOrEmpty(result.DebitAuthorizeAttachedFile)) && result.DebitAuthorizeAttachedFile != "N/A")
                {
                    Documents debitAuthorizeAttachedFile = JsonConvert.DeserializeObject<Documents>(result.DebitAuthorizeAttachedFile);
                    ViewBag.debitAuthorizeAttachedFile = debitAuthorizeAttachedFile.images;
                }

                #endregion หนังสือยินยอม

                #region บัญชีธนาคาร

                ViewBag.bankBookAttachedFile = null;
                if ((!String.IsNullOrEmpty(result.BankBookAttachedFile)) && result.BankBookAttachedFile != "N/A")
                {
                    Documents bankBookAttachedFile = JsonConvert.DeserializeObject<Documents>(result.BankBookAttachedFile);
                    ViewBag.bankBookAttachedFile = bankBookAttachedFile.images;
                }

                #endregion บัญชีธนาคาร

                #region อื่นๆ

                ViewBag.OtherAttachedFile = null;
                if ((!String.IsNullOrEmpty(result.OtherAttachedFile)) && result.OtherAttachedFile != "N/A")
                {
                    Documents otherAttachedFile = JsonConvert.DeserializeObject<Documents>(result.OtherAttachedFile);
                    ViewBag.OtherAttachedFile = otherAttachedFile.images;
                }

                #endregion อื่นๆ

                #endregion Documents

                return View();
            }
            else
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
            }
        }


        public ActionResult MotorAuditViewV2(string motorQueueAuditId)
        {
            var encodeId = motorQueueAuditId;
            if (encodeId != null && encodeId != string.Empty)
            {
                //decode
                byte[] data = Convert.FromBase64String(encodeId);
                string decodedString = Encoding.UTF8.GetString(data);

                //call stored procedure
                var result = _context.usp_QueueMotorAuditFullDetailByQueueAuditIdV2_Select(Convert.ToInt32(decodedString)).FirstOrDefault();
                //ถ้า null return notfouond
                if (result is null) RedirectToAction("MotorAuditView", new { motorQueueAuditId = motorQueueAuditId });

                var startNewVersionMT = Properties.Settings.Default.StartNewVersionMT;
                DateTime? oldPeriod = Convert.ToDateTime(startNewVersionMT);
                DateTime? nPeriod = Convert.ToDateTime(result.Period);
                if (oldPeriod > nPeriod)
                {
                    return RedirectToAction("MotorAuditView", new { motorQueueAuditId = motorQueueAuditId });
                }

                if (result.IsPolicies != true)
                {
                    return RedirectToAction("MotorAuditView", new { motorQueueAuditId = motorQueueAuditId });

                }
                var OpenGoogleDoc = Properties.Settings.Default.GoogleDocPathMT;
                try
                {
                    var appCode = result.ApplicationCode;
                    var insureName = result.InsuredName;
                    var payerName = result.PayerName;



                    string modifiedURL = OpenGoogleDoc.Replace("AppID", result.ApplicationCode)
                                                          .Replace("Name1", insureName)
                                                          .Replace("Name2", payerName);

                    ViewBag.OpenGoogleDoc = modifiedURL;
                }
                catch
                {
                    ViewBag.OpenGoogleDoc = OpenGoogleDoc;
                }

                #region Documents


                #region Documents


                //new Documents from docStorage
                var documentResult = _contextMotorBranchV2.usp_DocumentUnderwriteByQueueId_Select(result.QueueId, 2).ToList();
                var docstorage = Properties.Settings.Default.docstorage;
                ViewBag.docstorage = docstorage;


                if (documentResult != null)
                {
                    ViewBag.documentResult = documentResult;
                    var documentIdList = documentResult.Select(x => x.DocumentId).ToList();

                    var docstorageApi = Properties.Settings.Default.docstorageApi;

                    string apiUrl = docstorageApi;

                    // Create a RestSharp client
                    var client = new RestClient(apiUrl);

                    // Create a request with parameters
                    var request = new RestRequest(Method.GET);
                    /* request.AddParameter("documentIds", "value1");
                     request.AddParameter("param2", "value2");*/
                    foreach (var item in documentIdList)
                    {
                        request.AddParameter("documentIds", item);
                    }


                    // Execute the request and get the response
                    IRestResponse response = client.Execute(request);

                    // Check if the request was successful (statu s code 200)
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.ApiResponse = response.Content;
                        var dataResponse = JsonConvert.DeserializeObject<DataResponse>(response.Content);
                        ViewBag.docFile = dataResponse.data.ToList();
                        var dataResponseMerge = dataResponse.data.ToList();


                    }

                }


                #endregion Documents
                if (documentResult != null)
                {
                    ViewBag.documentResult = documentResult;
                    var documentIdList = documentResult.Select(x => x.DocumentId).ToList();

                    var docstorageApi = Properties.Settings.Default.docstorageApi;

                    string apiUrl = docstorageApi;

                    // Create a RestSharp client
                    var client = new RestClient(apiUrl);

                    // Create a request with parameters
                    var request = new RestRequest(Method.GET);
                    foreach (var item in documentIdList)
                    {
                        request.AddParameter("documentIds", item);
                    }
                    // Execute the request and get the response
                    IRestResponse response = client.Execute(request);

                    // Check if the request was successful (statu s code 200)
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.ApiResponse = response.Content;
                        var dataResponse = JsonConvert.DeserializeObject<DataResponse>(response.Content);
                        ViewBag.docFile = dataResponse.data.ToList();
                        var dataResponseMerge = dataResponse.data.ToList();
                    }

                }


                #endregion Documents

                //Get Role
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                var accessRole = GlobalFunction.GetAccessRoleUnderWriteAudit(roleList);
                ViewBag.AccessRole = accessRole;

                //set app code
                ViewBag.AppCode = result.ApplicationCode;
                ViewBag.MotorQueueAuditId = decodedString;

                //1 = na ให้ set default เป็น 2
                ViewBag.QueueAuditStatusId = (result.QueueAuditStatusId == 1 ? 2 : result.QueueAuditStatusId);
                ViewBag.QueueMotorFullDetail = result;
                var applicationCode = result.ApplicationCode;

                /* //StartCoverDate BE 2 ตำแหน่ง
                 if (result.StartCoverDate != null)
                 {
                     ViewBag.StartCoverDate = string.Format("{0}/{1}/{2}", result.StartCoverDate.Value.Day.ToString("00", null), result.StartCoverDate.Value.Month.ToString("00", null), result.StartCoverDate.Value.Year + 543);
                 }

                 //EndCoverDate BE 2 ตำแหน่ง
                 if (result.EndCoverDate != null)
                 {
                     ViewBag.EndCoverDate = string.Format("{0}/{1}/{2}", result.EndCoverDate.Value.Day.ToString("00", null), result.EndCoverDate.Value.Month.ToString("00", null), result.EndCoverDate.Value.Year + 543);
                 }

                 //ปีจดทะเบียน BE
                 ViewBag.VehicleRegistrationYear = result.VehicleRegistrationYear;
                 if (result.VehicleRegistrationYear != "" || result.VehicleRegistrationYear != null)
                 {
                     ViewBag.VehicleRegistrationYear = Convert.ToInt32(result.VehicleRegistrationYear) + 543;
                 }

                 //IssuesPolicyDate
                 if (result.IssuesPolicyDate != null)
                 {
                     ViewBag.IssuesPolicyDate = string.Format("{0}/{1}/{2} {3}", result.IssuesPolicyDate.Value.Day.ToString("00", null), result.IssuesPolicyDate.Value.Month.ToString("00", null), result.IssuesPolicyDate.Value.Year + 543, result.IssuesPolicyDate.Value.ToString("HH:mm:ss"));
                 }

                 //ข้อมูลผู้ใช้รถ (ผู้ที่ใช้รถประจำ/ผู้ใช้รถบ่อยที่สุด)
                 if (result.DriverFirstName == result.PayerFirstName && result.DriverLastName == result.PayerLastName)
                 {
                     ViewBag.Driver = "ผู้ชำระเบี้ย";
                 }
                 else if (result.DriverFirstName == result.InsuredFirstName && result.DriverLastName == result.InsuredLastName)
                 {
                     ViewBag.Driver = "ผู้เอาประกัน";
                 }
                 else
                 {
                     ViewBag.Driver = "บุคคลอื่น";
                 }*/

                var result2 = _context.usp_QueueMotorAuditStatus_Select(null).ToList();
                ViewBag.QueueAuditStatus = result2;

                var result3 = _context.usp_CallStatusMotor_Select(null).ToList();
                ViewBag.CallStatusListAuditMotor = result3;

                var result5 = _context.usp_AuditMotorUnderwriteStatus_Select(null).ToList();
                ViewBag.AuditMotorStatus = result5;

                var result6 = _context.usp_AuditMotorSpecifyStatus_Select(null).ToList();
                ViewBag.AuditMotorSpecifyStatus = result6;

                var result7 = _contextMotor.usp_UnderwriteTypeMotor_Select(null).ToList();
                ViewBag.UnderwriteType = result7;

                var result15 = _contextMotor.usp_UnderwritePaymentTypeMotor_Select(null).ToList();
                ViewBag.UnderwritePaymentTypeMotor = result15;

                var result8 = _context.usp_AuditMotorPaymentStatus_Select(null).ToList();
                ViewBag.AuditMotorPaymentType = result8;

                var result14 = _context.usp_AuditMotorStatus_Select(null).ToList();
                ViewBag.AuditMotorStatus = result14;

                var resultSaleServiceType = _context.usp_SaleServiceType_Select(null, null).ToList();
                ViewBag.SaleServiceType = resultSaleServiceType;

                var resultSaleMethodType = _context.usp_SaleMethodType_Select(null, null).ToList();
                ViewBag.SaleMethodType = resultSaleMethodType;


                var resultUnderwritingBehaviorType = _context.usp_UnderwritingBehaviorType_Select(null, null).ToList();
                ViewBag.UnderwritingBehaviorType = resultUnderwritingBehaviorType;

                var resultNotAuditedCause = _context.usp_NotAuditedCause_Select(null, null).ToList();
                ViewBag.NotAuditedCause = resultNotAuditedCause;

                var VehicleUseType = _context.usp_VehicleUseType_Select(null, null).ToList();
                ViewBag.VehicleUseType = VehicleUseType;

                var PositionDamaged = _context.usp_PositionDamaged_Select(null, null).ToList();
                ViewBag.PositionDamaged = PositionDamaged;

                var ReceivingInsuranceType = _context.usp_ReceivingInsuranceType_Select(null, null).ToList();
                ViewBag.ReceivingInsuranceType = ReceivingInsuranceType;

                var result9 = new List<CallStatusMotorUnderWrite>
                {
                    new CallStatusMotorUnderWrite{CallStatusId = 2, CallStatus = "รับสาย" },
                    new CallStatusMotorUnderWrite{CallStatusId = 3, CallStatus = "ไม่รับสาย"},
                    new CallStatusMotorUnderWrite{CallStatusId = 4, CallStatus = "ไม่สะดวกคุย"},
                    new CallStatusMotorUnderWrite{CallStatusId = 5, CallStatus = "ติดต่อไม่ได้"}
                };
                ViewBag.CallStatusListUDWBranch = result9;

                var result10 = _context.usp_AuditMotorInsureStatus_Select(null).ToList();
                ViewBag.AuditInsureStatus = result10;

                var result11 = _context.usp_AuditMotorInsureConsiderStatus_Select(null).ToList();
                ViewBag.AuditInsureConsiderStatus = result11;

                var result12 = _context.usp_AuditMotorUnderwriteStatus_Select(null).ToList();
                ViewBag.AuditUnderwriteStatus = result12;

                var result13 = _context.usp_AuditMotorPoliciesGivenStatus_Select(null).ToList();
                ViewBag.AuditMotorPoliciesGivenStatus = result13;

                /* //เช็คประวัติ หมายเลขตัวถัง
                 var count1 = _contextMotor.usp_QueueMotorUnderwriteHistory_Select(result.QueueId, 0, null, null, null).ToList().Count();
                 ViewBag.CountChassisNumber = count1;

                 //เช็คประวัติ ประวัติการชำระเบี้ยให้กับแอปอื่นๆ
                 var count2 = _contextMotor.usp_QueueMotorPaymentHistory_Select(result.QueueId, 0, null, null, null).ToList().Count();
                 ViewBag.CountPayment = count2;*/

                return View();
            }
            else
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
            }
        }

        public ActionResult MotorAuditNotPaseView(string motorQueueAuditId)
        {
            var encodeId = motorQueueAuditId;
            if (encodeId != null && encodeId != string.Empty)
            {
                //decode
                byte[] data = Convert.FromBase64String(encodeId);
                string decodedString = Encoding.UTF8.GetString(data);

                //call stored procedure
                var result = _context.usp_QueueMotorAuditFullDetailByQueueAuditId_Select(Convert.ToInt32(decodedString)).FirstOrDefault();

                //ถ้า null return notfouond
                if (result is null) return RedirectToAction("NotFound", "Error", new { Msg = "เลข App NotFound" });

                ViewBag.MotorQueueAuditId = decodedString;

                //Get Role
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                var accessRole = GlobalFunction.GetAccessRoleUnderWriteAudit(roleList);
                ViewBag.AccessRole = accessRole;

                //set app code
                ViewBag.AppCode = result.ApplicationCode;

                //1 = na ให้ set default เป็น 2
                ViewBag.QueueAuditStatusId = (result.QueueAuditStatusId == 1 ? 2 : result.QueueAuditStatusId);
                ViewBag.QueueMotorFullDetail = result;
                var applicationCode = result.ApplicationCode;

                //ปีจดทะเบียน BE
                ViewBag.VehicleRegistrationYear = result.VehicleRegistrationYear;
                if (result.VehicleRegistrationYear != "" || result.VehicleRegistrationYear != null)
                {
                    ViewBag.VehicleRegistrationYear = Convert.ToInt32(result.VehicleRegistrationYear) + 543;
                }

                //IssuesPolicyDate
                if (result.IssuesPolicyDate != null)
                {
                    ViewBag.IssuesPolicyDate = string.Format("{0}/{1}/{2} {3}", result.IssuesPolicyDate.Value.Day.ToString("00", null), result.IssuesPolicyDate.Value.Month.ToString("00", null), result.IssuesPolicyDate.Value.Year + 543, result.IssuesPolicyDate.Value.ToString("HH:mm:ss"));
                }

                //StartCoverDate BE 2 ตำแหน่ง
                if (result.StartCoverDate != null)
                {
                    ViewBag.StartCoverDate = string.Format("{0}/{1}/{2}", result.StartCoverDate.Value.Day.ToString("00", null), result.StartCoverDate.Value.Month.ToString("00", null), result.StartCoverDate.Value.Year + 543);
                }

                //EndCoverDate BE 2 ตำแหน่ง
                if (result.EndCoverDate != null)
                {
                    ViewBag.EndCoverDate = string.Format("{0}/{1}/{2}", result.EndCoverDate.Value.Day.ToString("00", null), result.EndCoverDate.Value.Month.ToString("00", null), result.EndCoverDate.Value.Year + 543);
                }

                //ข้อมูลผู้ใช้รถ (ผู้ที่ใช้รถประจำ/ผู้ใช้รถบ่อยที่สุด)
                if (result.DriverFirstName == result.PayerFirstName && result.DriverLastName == result.PayerLastName)
                {
                    ViewBag.Driver = "ผู้ชำระเบี้ย";
                }
                else if (result.DriverFirstName == result.InsuredFirstName && result.DriverLastName == result.InsuredLastName)
                {
                    ViewBag.Driver = "ผู้เอาประกัน";
                }
                else
                {
                    ViewBag.Driver = "บุคคลอื่น";
                }

                var result7 = _contextMotor.usp_UnderwriteTypeMotor_Select(null).ToList();
                ViewBag.UnderwriteType = result7;

                //payment type ใน underWriteMotor
                var result15 = _contextMotor.usp_UnderwritePaymentTypeMotor_Select(null).ToList();
                ViewBag.UnderwritePaymentTypeMotor = result15;

                //var result9 = _context.usp_CallStatusMotor_Select(null).ToList();
                //ViewBag.CallStatusListUDWBranch = result9;

                //ชื่อสโตรใน Audit และ UnderWritrMotor เหมือนกันเลยต้องทำแบปนี้ไปก่อน
                var result9 = new List<CallStatusMotorUnderWrite>
                {
                    new CallStatusMotorUnderWrite{CallStatusId = 2, CallStatus = "รับสาย"},
                    new CallStatusMotorUnderWrite{CallStatusId = 3, CallStatus = "ไม่รับสาย"},
                    new CallStatusMotorUnderWrite{CallStatusId = 4, CallStatus = "ไม่สะดวกคุย"},
                    new CallStatusMotorUnderWrite{CallStatusId = 5, CallStatus = "ติดต่อไม่ได้"}
                };
                ViewBag.CallStatusListUDWBranch = result9;

                //เช็คประวัติ หมายเลขตัวถัง
                var count1 = _contextMotor.usp_QueueMotorUnderwriteHistory_Select(Convert.ToInt32(decodedString), 0, null, null, null).ToList().Count();
                ViewBag.CountChassisNumber = count1;

                //เช็คประวัติ ประวัติการชำระเบี้ยให้กับแอปอื่นๆ
                var count2 = _contextMotor.usp_QueueMotorPaymentHistory_Select(Convert.ToInt32(decodedString), 0, null, null, null).ToList().Count();
                ViewBag.CountPayment = count2;

                #region Documents

                #region บัตรประชาชน

                ViewBag.idCardAttachedFile = null;
                if ((!String.IsNullOrEmpty(result.IdCardAttachedFile)) && result.IdCardAttachedFile != "N/A")
                {
                    Documents idCardAttachedFile = JsonConvert.DeserializeObject<Documents>(result.IdCardAttachedFile);
                    ViewBag.idCardAttachedFile = idCardAttachedFile.images;
                }

                #endregion บัตรประชาชน

                #region รายการจดทะเบียน

                ViewBag.regisBookAttachedFile = null;
                if ((!String.IsNullOrEmpty(result.RegisBookAttachedFile)) && result.RegisBookAttachedFile != "N/A")
                {
                    Documents regisBookAttachedFile = JsonConvert.DeserializeObject<Documents>(result.RegisBookAttachedFile);
                    ViewBag.regisBookAttachedFile = regisBookAttachedFile.images;
                }

                #endregion รายการจดทะเบียน

                #region รูปภาพ

                ViewBag.PhotoAttachedFile = null;
                if ((!String.IsNullOrEmpty(result.PhotoAttachedFile)) && result.PhotoAttachedFile != "N/A")
                {
                    Documents photoAttachedFile = JsonConvert.DeserializeObject<Documents>(result.PhotoAttachedFile);
                    ViewBag.PhotoAttachedFile = photoAttachedFile.images;
                }

                #endregion รูปภาพ

                #region ใบเสร็จ

                ViewBag.paymentSlipAttachedFile = null;
                if ((!String.IsNullOrEmpty(result.PaymentSlipAttachedFile)) && result.PaymentSlipAttachedFile != "N/A")
                {
                    Documents paymentSlipAttachedFile = JsonConvert.DeserializeObject<Documents>(result.PaymentSlipAttachedFile);
                    ViewBag.paymentSlipAttachedFile = paymentSlipAttachedFile.images;
                }

                #endregion ใบเสร็จ

                #region หนังสือยินยอม

                ViewBag.debitAuthorizeAttachedFile = null;
                if ((!String.IsNullOrEmpty(result.DebitAuthorizeAttachedFile)) && result.DebitAuthorizeAttachedFile != "N/A")
                {
                    Documents debitAuthorizeAttachedFile = JsonConvert.DeserializeObject<Documents>(result.DebitAuthorizeAttachedFile);
                    ViewBag.debitAuthorizeAttachedFile = debitAuthorizeAttachedFile.images;
                }

                #endregion หนังสือยินยอม

                #region บัญชีธนาคาร

                ViewBag.bankBookAttachedFile = null;
                if ((!String.IsNullOrEmpty(result.BankBookAttachedFile)) && result.BankBookAttachedFile != "N/A")
                {
                    Documents bankBookAttachedFile = JsonConvert.DeserializeObject<Documents>(result.BankBookAttachedFile);
                    ViewBag.bankBookAttachedFile = bankBookAttachedFile.images;
                }

                #endregion บัญชีธนาคาร

                #region อื่นๆ

                ViewBag.OtherAttachedFile = null;
                if ((!String.IsNullOrEmpty(result.OtherAttachedFile)) && result.OtherAttachedFile != "N/A")
                {
                    Documents otherAttachedFile = JsonConvert.DeserializeObject<Documents>(result.OtherAttachedFile);
                    ViewBag.OtherAttachedFile = otherAttachedFile.images;
                }

                #endregion อื่นๆ

                #endregion Documents

                return View();
            }
            else
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
            }
        }

        #endregion View

        #region method
        public ActionResult DowloadDocuments(string queueId)
        {
            #region Documents

            var result = _context.usp_QueueMotorAuditFullDetailByQueueAuditId_Select(Convert.ToInt32(queueId)).FirstOrDefault();
            // Get the current directory.
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string redirectUrlPathFile = Properties.Settings.Default.AbsolutePathImage;

            //Year - Month
            var yearMonth = Convert.ToDateTime(result.CreatedDate).ToString("yyyyMM");

            //get file path
            var folder = path + String.Format("FileImg/Motor/temp/{0}/QueueID_{1}/", yearMonth, queueId);

            //get DirectoryInfo
            var chackDirectory = path + "FileImg/Motor/temp/";
            if (Directory.Exists(chackDirectory))
            {
                DirectoryInfo diChack = new DirectoryInfo(chackDirectory);
                DateTime localDate = DateTime.Now;

                //เคลียร์ folder temp ทุกวัน
                if (diChack.CreationTime.Date < localDate.Date)
                {
                    Directory.Delete(chackDirectory, true);
                }
            }

            //check Directory
            //not have is Create
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            // Make a reference to a directory.
            DirectoryInfo di = new DirectoryInfo(folder);
            //delete file in directory befor upload
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            try
            {
                #region บัตรประจำตัวประชาชน

                //บัตรประจำตัวประชาชน
                if ((!String.IsNullOrEmpty(result.IdCardAttachedFile)) && result.IdCardAttachedFile != "N/A")
                {
                    Documents idCardAttachedFile = JsonConvert.DeserializeObject<Documents>(result.IdCardAttachedFile);
                    var partURL = idCardAttachedFile.images;
                    ViewBag.partURL_IdCard = partURL.ToList();
                    var UrlPathFile = redirectUrlPathFile + String.Format("FileImg/Motor/temp/{0}/QueueID_{1}/", yearMonth, queueId);
                    //fileName
                    var fileName = String.Format("{0}_{1}", queueId, "idCardAttachedFile");
                    //list part img
                    List<string> fullPartIdCard = new List<string>();

                    using (var client = new System.Net.WebClient())
                    {
                        for (int i = 0; i < partURL.Count; i++)
                        {
                            try
                            {
                                var fileData = client.DownloadData(partURL[i]);
                                var contentType = client.ResponseHeaders["Content-Type"];
                                switch (contentType)
                                {
                                    case "image/jpeg":
                                        var runing1 = i + 1 + ".jpg";
                                        client.DownloadFile(partURL[i], folder + fileName + runing1);
                                        fullPartIdCard.Add(UrlPathFile + fileName + runing1);
                                        ViewBag.IdCardContentType = "JPG";
                                        break;

                                    case "application/pdf":
                                        var runing2 = i + 1 + ".pdf";
                                        client.DownloadFile(partURL[i], folder + fileName + runing2);
                                        fullPartIdCard.Add(UrlPathFile + fileName + runing2);
                                        ViewBag.IdCardContentType = "PDF";
                                        break;

                                    default:
                                        break;
                                }
                            }
                            catch (Exception)
                            {
                                //url dowload notFound
                                fullPartIdCard.Add(null);
                            }
                        }
                        client.CancelAsync();
                    }

                    ViewBag.IdCardAttachedFile = fullPartIdCard;
                }
                else
                {
                    ViewBag.IdCardAttachedFile = null;
                }

                #endregion บัตรประจำตัวประชาชน

                #region รูปภาพ

                //รูปภาพ
                if ((!String.IsNullOrEmpty(result.PhotoAttachedFile)) && result.PhotoAttachedFile != "N/A")
                {
                    Documents photoAttachedFile = JsonConvert.DeserializeObject<Documents>(result.PhotoAttachedFile);
                    var partURL = photoAttachedFile.images;
                    ViewBag.partURL_Photo = partURL;
                    var UrlPathFile = redirectUrlPathFile + String.Format("FileImg/Motor/temp/{0}/QueueID_{1}/", yearMonth, queueId);
                    //fileName
                    var fileName = String.Format("{0}_{1}-{2}-", queueId, "photoAttachedFile", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    //list part img
                    List<string> fullPartPhoto = new List<string>();
                    using (var client = new System.Net.WebClient())
                    {
                        for (int i = 0; i < partURL.Count; i++)
                        {
                            try
                            {
                                var fileData = client.DownloadData(partURL[i]);
                                var contentType = client.ResponseHeaders["Content-Type"];
                                switch (contentType)
                                {
                                    case "image/jpeg":
                                        var runing1 = i + 1 + ".jpg";
                                        client.DownloadFile(partURL[i], folder + fileName + runing1);
                                        fullPartPhoto.Add(UrlPathFile + fileName + runing1);
                                        ViewBag.PhotoContentType = "JPG";
                                        break;

                                    case "application/pdf":
                                        var runing2 = i + 1 + ".pdf";
                                        client.DownloadFile(partURL[i], folder + fileName + runing2);
                                        fullPartPhoto.Add(UrlPathFile + fileName + runing2);
                                        ViewBag.PhotoContentType = "PDF";
                                        break;

                                    default:
                                        break;
                                }
                            }
                            catch (Exception)
                            {
                                fullPartPhoto.Add(null);
                            }
                        }
                        client.CancelAsync();
                    }

                    ViewBag.PhotoAttachedFile = fullPartPhoto;
                }
                else
                {
                    ViewBag.PhotoAttachedFile = null;
                }

                #endregion รูปภาพ

                #region รายการจดทะเบียน

                //รายการจดทะเบียน
                if ((!String.IsNullOrEmpty(result.RegisBookAttachedFile)) && result.RegisBookAttachedFile != "N/A")
                {
                    Documents regisBookAttachedFile = JsonConvert.DeserializeObject<Documents>(result.RegisBookAttachedFile);
                    var partURL = regisBookAttachedFile.images;
                    ViewBag.partURL_RegisBook = partURL;
                    var UrlPathFile = redirectUrlPathFile + String.Format("FileImg/Motor/temp/{0}/QueueID_{1}/", yearMonth, queueId);
                    //fileName
                    var fileName = String.Format("{0}_{1}-{2}-", queueId, "regisBookAttachedFile", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    //list part img
                    List<string> fullPartRegisBook = new List<string>();
                    using (var client = new System.Net.WebClient())
                    {
                        for (int i = 0; i < partURL.Count; i++)
                        {
                            try
                            {
                                var fileData = client.DownloadData(partURL[i]);
                                var contentType = client.ResponseHeaders["Content-Type"];
                                switch (contentType)
                                {
                                    case "image/jpeg":
                                        var runing1 = i + 1 + ".jpg";
                                        client.DownloadFile(partURL[i], folder + fileName + runing1);
                                        fullPartRegisBook.Add(UrlPathFile + fileName + runing1);
                                        ViewBag.RegisBookContentType = "JPG";
                                        break;

                                    case "application/pdf":
                                        var runing2 = i + 1 + ".pdf";
                                        client.DownloadFile(partURL[i], folder + fileName + runing2);
                                        fullPartRegisBook.Add(UrlPathFile + fileName + runing2);
                                        ViewBag.RegisBookContentType = "PDF";
                                        break;

                                    default:
                                        break;
                                }
                            }
                            catch (Exception)
                            {
                                fullPartRegisBook.Add(null);
                            }
                        }
                        client.CancelAsync();
                    }

                    ViewBag.RegisBookAttachedFile = fullPartRegisBook;
                }
                else
                {
                    ViewBag.RegisBookAttachedFile = null;
                }

                #endregion รายการจดทะเบียน

                #region ใบเสร็จ

                //ใบเสร็จ
                if ((!String.IsNullOrEmpty(result.PaymentSlipAttachedFile)) && result.PaymentSlipAttachedFile != "N/A")
                {
                    Documents paymentSlipAttachedFile = JsonConvert.DeserializeObject<Documents>(result.PaymentSlipAttachedFile);
                    var partURL = paymentSlipAttachedFile.images;
                    ViewBag.partURL_PaymentSlip = partURL;
                    var UrlPathFile = redirectUrlPathFile + String.Format("FileImg/Motor/temp/{0}/QueueID_{1}/", yearMonth, queueId);
                    //fileName
                    var fileName = String.Format("{0}_{1}-{2}-", queueId, "paymentSlipAttachedFile", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    //list part img
                    List<string> fullPartPaymentSlip = new List<string>();
                    using (var client = new System.Net.WebClient())
                    {
                        for (int i = 0; i < partURL.Count; i++)
                        {
                            try
                            {
                                var fileData = client.DownloadData(partURL[i]);
                                var contentType = client.ResponseHeaders["Content-Type"];
                                switch (contentType)
                                {
                                    case "image/jpeg":
                                        var runing1 = i + 1 + ".jpg";
                                        client.DownloadFile(partURL[i], folder + fileName + runing1);
                                        fullPartPaymentSlip.Add(UrlPathFile + fileName + runing1);
                                        ViewBag.PaymentSlipContentType = "JPG";
                                        break;

                                    case "application/pdf":
                                        var runing2 = i + 1 + ".pdf";
                                        client.DownloadFile(partURL[i], folder + fileName + runing2);
                                        fullPartPaymentSlip.Add(UrlPathFile + fileName + runing2);
                                        ViewBag.PaymentSlipContentType = "PDF";
                                        break;

                                    default:
                                        break;
                                }
                            }
                            catch (Exception)
                            {
                                fullPartPaymentSlip.Add(null);
                            }
                        }
                        client.CancelAsync();
                    }

                    ViewBag.PaymentSlipAttachedFile = fullPartPaymentSlip;
                }
                else
                {
                    ViewBag.PaymentSlipAttachedFile = null;
                }

                #endregion ใบเสร็จ

                #region หนังสือยินยอม

                //หนังสือยินยอม
                if ((!String.IsNullOrEmpty(result.DebitAuthorizeAttachedFile)) && result.DebitAuthorizeAttachedFile != "N/A")
                {
                    Documents debitAuthorizeAttachedFile = JsonConvert.DeserializeObject<Documents>(result.DebitAuthorizeAttachedFile);
                    var partURL = debitAuthorizeAttachedFile.images;
                    ViewBag.partURL_DebitAuthorize = partURL;
                    var UrlPathFile = redirectUrlPathFile + String.Format("FileImg/Motor/temp/{0}/QueueID_{1}/", yearMonth, queueId);
                    //fileName
                    var fileName = String.Format("{0}_{1}-{2}-", queueId, "debitAuthorizeAttachedFile", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    //list part img
                    List<string> fullPartDebitAuthorize = new List<string>();
                    using (var client = new System.Net.WebClient())
                    {
                        for (int i = 0; i < partURL.Count; i++)
                        {
                            try
                            {
                                var fileData = client.DownloadData(partURL[i]);
                                var contentType = client.ResponseHeaders["Content-Type"];
                                switch (contentType)
                                {
                                    case "image/jpeg":
                                        var runing1 = i + 1 + ".jpg";
                                        client.DownloadFile(partURL[i], folder + fileName + runing1);
                                        fullPartDebitAuthorize.Add(UrlPathFile + fileName + runing1);
                                        ViewBag.AuthorizeContentType = "JPG";
                                        break;

                                    case "application/pdf":
                                        var runing2 = i + 1 + ".pdf";
                                        client.DownloadFile(partURL[i], folder + fileName + runing2);
                                        fullPartDebitAuthorize.Add(UrlPathFile + fileName + runing2);
                                        ViewBag.AuthorizeContentType = "PDF";
                                        break;

                                    default:
                                        break;
                                }
                            }
                            catch (Exception)
                            {
                                fullPartDebitAuthorize.Add(null);
                            }
                        }
                        client.CancelAsync();
                    }

                    ViewBag.DebitAuthorizeAttachedFile = fullPartDebitAuthorize;
                }
                else
                {
                    ViewBag.DebitAuthorizeAttachedFile = null;
                }

                #endregion หนังสือยินยอม

                #region บัญชีธนาคาร

                //บัญชีธนาคาร
                if ((!String.IsNullOrEmpty(result.BankBookAttachedFile)) && result.BankBookAttachedFile != "N/A")
                {
                    Documents bankBookAttachedFile = JsonConvert.DeserializeObject<Documents>(result.BankBookAttachedFile);
                    var partURL = bankBookAttachedFile.images;
                    ViewBag.partURL_bankBook = partURL;
                    var UrlPathFile = redirectUrlPathFile + String.Format("FileImg/Motor/temp/{0}/QueueID_{1}/", yearMonth, queueId);
                    //fileName
                    var fileName = String.Format("{0}_{1}-{2}-", queueId, "BankBookAttachedFile", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    //list part img
                    List<string> fullPartBankBook = new List<string>();
                    using (var client = new System.Net.WebClient())
                    {
                        for (int i = 0; i < partURL.Count; i++)
                        {
                            try
                            {
                                var fileData = client.DownloadData(partURL[i]);
                                var contentType = client.ResponseHeaders["Content-Type"];
                                switch (contentType)
                                {
                                    case "image/jpeg":
                                        var runing1 = i + 1 + ".jpg";
                                        client.DownloadFile(partURL[i], folder + fileName + runing1);
                                        fullPartBankBook.Add(UrlPathFile + fileName + runing1);
                                        ViewBag.bankBookContentType = "JPG";
                                        break;

                                    case "application/pdf":
                                        var runing2 = i + 1 + ".pdf";
                                        client.DownloadFile(partURL[i], folder + fileName + runing2);
                                        fullPartBankBook.Add(UrlPathFile + fileName + runing2);
                                        ViewBag.bankBookContentType = "PDF";
                                        break;

                                    default:
                                        break;
                                }
                            }
                            catch (Exception)
                            {
                                fullPartBankBook.Add(null);
                            }
                        }
                        client.CancelAsync();
                    }

                    ViewBag.BankBookAttachedFile = fullPartBankBook;
                }
                else
                {
                    ViewBag.BankBookAttachedFile = null;
                }

                #endregion บัญชีธนาคาร

                #region อื่นๆ

                //อื่นๆ
                if ((!String.IsNullOrEmpty(result.OtherAttachedFile)) && result.OtherAttachedFile != "N/A")
                {
                    Documents otherAttachedFile = JsonConvert.DeserializeObject<Documents>(result.OtherAttachedFile);
                    ViewBag.OtherAttachedFile = otherAttachedFile.images;
                    var partURL = otherAttachedFile.images;
                    ViewBag.partURL_Other = partURL;
                    var UrlPathFile = redirectUrlPathFile + String.Format("FileImg/Motor/temp/{0}/QueueID_{1}/", yearMonth, queueId);
                    //fileName
                    var fileName = String.Format("{0}_{1}-{2}-", queueId, "otherAttachedFile", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    //list part img
                    List<string> fullPartOther = new List<string>();
                    using (var client = new System.Net.WebClient())
                    {
                        for (int i = 0; i < partURL.Count; i++)
                        {
                            try
                            {
                                var fileData = client.DownloadData(partURL[i]);
                                var contentType = client.ResponseHeaders["Content-Type"];
                                switch (contentType)
                                {
                                    case "image/jpeg":
                                        var runing1 = i + 1 + ".jpg";
                                        client.DownloadFile(partURL[i], folder + fileName + runing1);
                                        fullPartOther.Add(UrlPathFile + fileName + runing1);
                                        ViewBag.OtherContentType = "JPG";
                                        break;

                                    case "application/pdf":
                                        var runing2 = i + 1 + ".pdf";
                                        client.DownloadFile(partURL[i], folder + fileName + runing2);
                                        fullPartOther.Add(UrlPathFile + fileName + runing2);
                                        ViewBag.OtherContentType = "PDF";
                                        break;

                                    default:
                                        break;
                                }
                            }
                            catch (Exception)
                            {
                                fullPartOther.Add(null);
                            }
                        }
                        client.CancelAsync();
                    }

                    ViewBag.OtherAttachedFile = fullPartOther;
                }
                else
                {
                    ViewBag.OtherAttachedFile = null;
                }

                #endregion อื่นๆ
            }
            catch (Exception e)
            {
                return RedirectToAction("InternalServerError", "Error", new { Msg = "Message:" + e.Message });
            }

            #endregion Documents

            var doc = new Dictionary<string, object>
            {
                {"IsResult",true },
                {"idCardAttachedFile", ViewBag.IdCardAttachedFile },
                {"idCardAttachedFileType", ViewBag.IdCardContentType },
                {"photoAttachedFile", ViewBag.PhotoAttachedFile },
                {"photoAttachedFileType", ViewBag.PhotoContentType },
                {"regisBookAttachedFile", ViewBag.RegisBookAttachedFile },
                {"regisBookAttachedFileType", ViewBag.RegisBookContentType },
                {"paymentSlipAttachedFile", ViewBag.PaymentSlipAttachedFile },
                {"paymentSlipAttachedFileType", ViewBag.PaymentSlipContentType },
                {"debitAuthorizeAttachedFile", ViewBag.DebitAuthorizeAttachedFile },
                {"debitAuthorizeAttachedFileType", ViewBag.AuthorizeContentType },
                {"bankBookAttachedFile", ViewBag.BankBookAttachedFile  },
                {"bankBookAttachedFileType", ViewBag.bankBookContentType },
                {"otherAttachedFile", ViewBag.OtherAttachedFile  },
                {"otherAttachedFileType", ViewBag.OtherContentType },
            };
            return Json(doc, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DowloadDocuments1(string motorQueueAuditId)
        {
            var result = _context.usp_QueueMotorAuditFullDetailByQueueAuditId_Select(Convert.ToInt32(motorQueueAuditId)).FirstOrDefault();
            // Get the current directory.
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string redirectUrlPathFile = Properties.Settings.Default.AbsolutePathImage;

            //Year - Month
            var yearMonth = Convert.ToDateTime(result.CreatedDate).ToString("yyyyMM");

            //get file path
            var folder = path + String.Format("FileImg/MotorAudit/temp/{0}/QueueAuditID_{1}/", yearMonth, motorQueueAuditId);

            //get DirectoryInfo
            var chackDirectory = path + "FileImg/MotorAudit/temp/";
            if (Directory.Exists(chackDirectory))
            {
                DirectoryInfo diChack = new DirectoryInfo(chackDirectory);
                DateTime localDate = DateTime.Now;

                //เคลียร์ folder temp ทุกวัน
                if (diChack.CreationTime.Date < localDate.Date)
                {
                    Directory.Delete(chackDirectory, true);
                }
            }

            //check Directory
            //not have is Create
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            // Make a reference to a directory.
            DirectoryInfo di = new DirectoryInfo(folder);
            //delete file in directory befor upload
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            try
            {
                #region บัตรประจำตัวประชาชน

                //บัตรประจำตัวประชาชน
                if ((!String.IsNullOrEmpty(result.IdCardAttachedFile)) && result.IdCardAttachedFile != "N/A")
                {
                    Documents idCardAttachedFile = JsonConvert.DeserializeObject<Documents>(result.IdCardAttachedFile);
                    var partURL = idCardAttachedFile.images;
                    ViewBag.partURL_IdCard = partURL.ToList();
                    var UrlPathFile = redirectUrlPathFile + String.Format("FileImg/MotorAudit/temp/{0}/QueueAuditID_{1}/", yearMonth, motorQueueAuditId);
                    //fileName
                    var fileName = String.Format("{0}_{1}", motorQueueAuditId, "idCardAttachedFile");
                    //list part img
                    List<string> fullPartIdCard = new List<string>();

                    using (var client = new System.Net.WebClient())
                    {
                        for (int i = 0; i < partURL.Count; i++)
                        {
                            try
                            {
                                var fileData = client.DownloadData(partURL[i]);
                                var contentType = client.ResponseHeaders["Content-Type"];
                                switch (contentType)
                                {
                                    case "image/jpeg":
                                        var runing1 = i + 1 + ".jpg";
                                        client.DownloadFile(partURL[i], folder + fileName + runing1);
                                        fullPartIdCard.Add(UrlPathFile + fileName + runing1);
                                        ViewBag.IdCardContentType = "JPG";
                                        break;

                                    case "application/pdf":
                                        var runing2 = i + 1 + ".pdf";
                                        client.DownloadFile(partURL[i], folder + fileName + runing2);
                                        fullPartIdCard.Add(UrlPathFile + fileName + runing2);
                                        ViewBag.IdCardContentType = "PDF";
                                        break;

                                    default:
                                        break;
                                }
                            }
                            catch (Exception)
                            {
                                //url dowload notFound
                                fullPartIdCard.Add(null);
                            }
                        }
                        client.CancelAsync();
                    }

                    ViewBag.IdCardAttachedFile = fullPartIdCard;
                }
                else
                {
                    ViewBag.IdCardAttachedFile = null;
                }

                #endregion บัตรประจำตัวประชาชน

                #region รูปภาพ

                //รูปภาพ
                if ((!String.IsNullOrEmpty(result.PhotoAttachedFile)) && result.PhotoAttachedFile != "N/A")
                {
                    Documents photoAttachedFile = JsonConvert.DeserializeObject<Documents>(result.PhotoAttachedFile);
                    var partURL = photoAttachedFile.images;
                    ViewBag.partURL_Photo = partURL;
                    var UrlPathFile = redirectUrlPathFile + String.Format("FileImg/MotorAudit/temp/{0}/QueueAuditID_{1}/", yearMonth, motorQueueAuditId);
                    //fileName
                    var fileName = String.Format("{0}_{1}-{2}-", motorQueueAuditId, "photoAttachedFile", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    //list part img
                    List<string> fullPartPhoto = new List<string>();
                    using (var client = new System.Net.WebClient())
                    {
                        for (int i = 0; i < partURL.Count; i++)
                        {
                            try
                            {
                                var fileData = client.DownloadData(partURL[i]);
                                var contentType = client.ResponseHeaders["Content-Type"];
                                switch (contentType)
                                {
                                    case "image/jpeg":
                                        var runing1 = i + 1 + ".jpg";
                                        client.DownloadFile(partURL[i], folder + fileName + runing1);
                                        fullPartPhoto.Add(UrlPathFile + fileName + runing1);
                                        ViewBag.PhotoContentType = "JPG";
                                        break;

                                    case "application/pdf":
                                        var runing2 = i + 1 + ".pdf";
                                        client.DownloadFile(partURL[i], folder + fileName + runing2);
                                        fullPartPhoto.Add(UrlPathFile + fileName + runing2);
                                        ViewBag.PhotoContentType = "PDF";
                                        break;

                                    default:
                                        break;
                                }
                            }
                            catch (Exception)
                            {
                                fullPartPhoto.Add(null);
                            }
                        }
                        client.CancelAsync();
                    }

                    ViewBag.PhotoAttachedFile = fullPartPhoto;
                }
                else
                {
                    ViewBag.PhotoAttachedFile = null;
                }

                #endregion รูปภาพ

                #region รายการจดทะเบียน

                //รายการจดทะเบียน
                if ((!String.IsNullOrEmpty(result.RegisBookAttachedFile)) && result.RegisBookAttachedFile != "N/A")
                {
                    Documents regisBookAttachedFile = JsonConvert.DeserializeObject<Documents>(result.RegisBookAttachedFile);
                    var partURL = regisBookAttachedFile.images;
                    ViewBag.partURL_RegisBook = partURL;
                    var UrlPathFile = redirectUrlPathFile + String.Format("FileImg/MotorAudit/temp/{0}/QueueAuditID_{1}/", yearMonth, motorQueueAuditId);
                    //fileName
                    var fileName = String.Format("{0}_{1}-{2}-", motorQueueAuditId, "regisBookAttachedFile", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    //list part img
                    List<string> fullPartRegisBook = new List<string>();
                    using (var client = new System.Net.WebClient())
                    {
                        for (int i = 0; i < partURL.Count; i++)
                        {
                            try
                            {
                                var fileData = client.DownloadData(partURL[i]);
                                var contentType = client.ResponseHeaders["Content-Type"];
                                switch (contentType)
                                {
                                    case "image/jpeg":
                                        var runing1 = i + 1 + ".jpg";
                                        client.DownloadFile(partURL[i], folder + fileName + runing1);
                                        fullPartRegisBook.Add(UrlPathFile + fileName + runing1);
                                        ViewBag.RegisBookContentType = "JPG";
                                        break;

                                    case "application/pdf":
                                        var runing2 = i + 1 + ".pdf";
                                        client.DownloadFile(partURL[i], folder + fileName + runing2);
                                        fullPartRegisBook.Add(UrlPathFile + fileName + runing2);
                                        ViewBag.RegisBookContentType = "PDF";
                                        break;

                                    default:
                                        break;
                                }
                            }
                            catch (Exception)
                            {
                                fullPartRegisBook.Add(null);
                            }
                        }
                        client.CancelAsync();
                    }

                    ViewBag.RegisBookAttachedFile = fullPartRegisBook;
                }
                else
                {
                    ViewBag.RegisBookAttachedFile = null;
                }

                #endregion รายการจดทะเบียน

                #region ใบเสร็จ

                //ใบเสร็จ
                if ((!String.IsNullOrEmpty(result.PaymentSlipAttachedFile)) && result.PaymentSlipAttachedFile != "N/A")
                {
                    Documents paymentSlipAttachedFile = JsonConvert.DeserializeObject<Documents>(result.PaymentSlipAttachedFile);
                    var partURL = paymentSlipAttachedFile.images;
                    ViewBag.partURL_PaymentSlip = partURL;
                    var UrlPathFile = redirectUrlPathFile + String.Format("FileImg/MotorAudit/temp/{0}/QueueAuditID_{1}/", yearMonth, motorQueueAuditId);
                    //fileName
                    var fileName = String.Format("{0}_{1}-{2}-", motorQueueAuditId, "paymentSlipAttachedFile", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    //list part img
                    List<string> fullPartPaymentSlip = new List<string>();
                    using (var client = new System.Net.WebClient())
                    {
                        for (int i = 0; i < partURL.Count; i++)
                        {
                            try
                            {
                                var fileData = client.DownloadData(partURL[i]);
                                var contentType = client.ResponseHeaders["Content-Type"];
                                switch (contentType)
                                {
                                    case "image/jpeg":
                                        var runing1 = i + 1 + ".jpg";
                                        client.DownloadFile(partURL[i], folder + fileName + runing1);
                                        fullPartPaymentSlip.Add(UrlPathFile + fileName + runing1);
                                        ViewBag.PaymentSlipContentType = "JPG";
                                        break;

                                    case "application/pdf":
                                        var runing2 = i + 1 + ".pdf";
                                        client.DownloadFile(partURL[i], folder + fileName + runing2);
                                        fullPartPaymentSlip.Add(UrlPathFile + fileName + runing2);
                                        ViewBag.PaymentSlipContentType = "PDF";
                                        break;

                                    default:
                                        break;
                                }
                            }
                            catch (Exception)
                            {
                                fullPartPaymentSlip.Add(null);
                            }
                        }
                        client.CancelAsync();
                    }

                    ViewBag.PaymentSlipAttachedFile = fullPartPaymentSlip;
                }
                else
                {
                    ViewBag.PaymentSlipAttachedFile = null;
                }

                #endregion ใบเสร็จ

                #region หนังสือยินยอม

                //หนังสือยินยอม
                if ((!String.IsNullOrEmpty(result.DebitAuthorizeAttachedFile)) && result.DebitAuthorizeAttachedFile != "N/A")
                {
                    Documents debitAuthorizeAttachedFile = JsonConvert.DeserializeObject<Documents>(result.DebitAuthorizeAttachedFile);
                    var partURL = debitAuthorizeAttachedFile.images;
                    ViewBag.partURL_DebitAuthorize = partURL;
                    var UrlPathFile = redirectUrlPathFile + String.Format("FileImg/MotorAudit/temp/{0}/QueueAuditID_{1}/", yearMonth, motorQueueAuditId);
                    //fileName
                    var fileName = String.Format("{0}_{1}-{2}-", motorQueueAuditId, "debitAuthorizeAttachedFile", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    //list part img
                    List<string> fullPartDebitAuthorize = new List<string>();
                    using (var client = new System.Net.WebClient())
                    {
                        for (int i = 0; i < partURL.Count; i++)
                        {
                            try
                            {
                                var fileData = client.DownloadData(partURL[i]);
                                var contentType = client.ResponseHeaders["Content-Type"];
                                switch (contentType)
                                {
                                    case "image/jpeg":
                                        var runing1 = i + 1 + ".jpg";
                                        client.DownloadFile(partURL[i], folder + fileName + runing1);
                                        fullPartDebitAuthorize.Add(UrlPathFile + fileName + runing1);
                                        ViewBag.AuthorizeContentType = "JPG";
                                        break;

                                    case "application/pdf":
                                        var runing2 = i + 1 + ".pdf";
                                        client.DownloadFile(partURL[i], folder + fileName + runing2);
                                        fullPartDebitAuthorize.Add(UrlPathFile + fileName + runing2);
                                        ViewBag.AuthorizeContentType = "PDF";
                                        break;

                                    default:
                                        break;
                                }
                            }
                            catch (Exception)
                            {
                                fullPartDebitAuthorize.Add(null);
                            }
                        }
                        client.CancelAsync();
                    }

                    ViewBag.DebitAuthorizeAttachedFile = fullPartDebitAuthorize;
                }
                else
                {
                    ViewBag.DebitAuthorizeAttachedFile = null;
                }

                #endregion หนังสือยินยอม

                #region บัญชีธนาคาร

                //บัญชีธนาคาร
                if ((!String.IsNullOrEmpty(result.BankBookAttachedFile)) && result.BankBookAttachedFile != "N/A")
                {
                    Documents bankBookAttachedFile = JsonConvert.DeserializeObject<Documents>(result.BankBookAttachedFile);
                    var partURL = bankBookAttachedFile.images;
                    ViewBag.partURL_bankBook = partURL;
                    var UrlPathFile = redirectUrlPathFile + String.Format("FileImg/MotorAudit/temp/{0}/QueueAuditID_{1}/", yearMonth, motorQueueAuditId);
                    //fileName
                    var fileName = String.Format("{0}_{1}-{2}-", motorQueueAuditId, "BankBookAttachedFile", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    //list part img
                    List<string> fullPartBankBook = new List<string>();
                    using (var client = new System.Net.WebClient())
                    {
                        for (int i = 0; i < partURL.Count; i++)
                        {
                            try
                            {
                                var fileData = client.DownloadData(partURL[i]);
                                var contentType = client.ResponseHeaders["Content-Type"];
                                switch (contentType)
                                {
                                    case "image/jpeg":
                                        var runing1 = i + 1 + ".jpg";
                                        client.DownloadFile(partURL[i], folder + fileName + runing1);
                                        fullPartBankBook.Add(UrlPathFile + fileName + runing1);
                                        ViewBag.bankBookContentType = "JPG";
                                        break;

                                    case "application/pdf":
                                        var runing2 = i + 1 + ".pdf";
                                        client.DownloadFile(partURL[i], folder + fileName + runing2);
                                        fullPartBankBook.Add(UrlPathFile + fileName + runing2);
                                        ViewBag.bankBookContentType = "PDF";
                                        break;

                                    default:
                                        break;
                                }
                            }
                            catch (Exception)
                            {
                                fullPartBankBook.Add(null);
                            }
                        }
                        client.CancelAsync();
                    }

                    ViewBag.BankBookAttachedFile = fullPartBankBook;
                }
                else
                {
                    ViewBag.BankBookAttachedFile = null;
                }

                #endregion บัญชีธนาคาร

                #region อื่นๆ

                //อื่นๆ
                if ((!String.IsNullOrEmpty(result.OtherAttachedFile)) && result.OtherAttachedFile != "N/A")
                {
                    Documents otherAttachedFile = JsonConvert.DeserializeObject<Documents>(result.OtherAttachedFile);
                    ViewBag.OtherAttachedFile = otherAttachedFile.images;
                    var partURL = otherAttachedFile.images;
                    ViewBag.partURL_Other = partURL;
                    var UrlPathFile = redirectUrlPathFile + String.Format("FileImg/MotorAudit/temp/{0}/QueueAuditID_{1}/", yearMonth, motorQueueAuditId);
                    //fileName
                    var fileName = String.Format("{0}_{1}-{2}-", motorQueueAuditId, "otherAttachedFile", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    //list part img
                    List<string> fullPartOther = new List<string>();
                    using (var client = new System.Net.WebClient())
                    {
                        for (int i = 0; i < partURL.Count; i++)
                        {
                            try
                            {
                                var fileData = client.DownloadData(partURL[i]);
                                var contentType = client.ResponseHeaders["Content-Type"];
                                switch (contentType)
                                {
                                    case "image/jpeg":
                                        var runing1 = i + 1 + ".jpg";
                                        client.DownloadFile(partURL[i], folder + fileName + runing1);
                                        fullPartOther.Add(UrlPathFile + fileName + runing1);
                                        ViewBag.OtherContentType = "JPG";
                                        break;

                                    case "application/pdf":
                                        var runing2 = i + 1 + ".pdf";
                                        client.DownloadFile(partURL[i], folder + fileName + runing2);
                                        fullPartOther.Add(UrlPathFile + fileName + runing2);
                                        ViewBag.OtherContentType = "PDF";
                                        break;

                                    default:
                                        break;
                                }
                            }
                            catch (Exception)
                            {
                                fullPartOther.Add(null);
                            }
                        }
                        client.CancelAsync();
                    }

                    ViewBag.OtherAttachedFile = fullPartOther;
                }
                else
                {
                    ViewBag.OtherAttachedFile = null;
                }

                #endregion อื่นๆ
            }
            catch (Exception e)
            {
                return RedirectToAction("InternalServerError", "Error", new { Msg = "Message:" + e.Message });
            }

            var doc = new Dictionary<string, object>
            {
                {"IsResult",true },
                {"idCardAttachedFile", ViewBag.IdCardAttachedFile },
                {"idCardAttachedFileType", ViewBag.IdCardContentType },
                {"photoAttachedFile", ViewBag.PhotoAttachedFile },
                {"photoAttachedFileType", ViewBag.PhotoContentType },
                {"regisBookAttachedFile", ViewBag.RegisBookAttachedFile },
                {"regisBookAttachedFileType", ViewBag.RegisBookContentType },
                {"paymentSlipAttachedFile", ViewBag.PaymentSlipAttachedFile },
                {"paymentSlipAttachedFileType", ViewBag.PaymentSlipContentType },
                {"debitAuthorizeAttachedFile", ViewBag.DebitAuthorizeAttachedFile },
                {"debitAuthorizeAttachedFileType", ViewBag.AuthorizeContentType },
                {"bankBookAttachedFile", ViewBag.BankBookAttachedFile  },
                {"bankBookAttachedFileType", ViewBag.bankBookContentType },
                {"otherAttachedFile", ViewBag.OtherAttachedFile  },
                {"otherAttachedFileType", ViewBag.OtherContentType },
            };
            return Json(doc, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_InsureMotor")]
        public ActionResult QuestionAuditInsert(FormCollection form)
        {
            //สถานะการตรวจสอบ
            var queueAuditStatus = form["QueueAuditStatusId"];

            if (!string.IsNullOrEmpty(queueAuditStatus))
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                //queue audit id
                var QueueAuditId = form["QueueAuditId"];
                //สถานการโทร
                var CallStatus = form["CallStatus"];
                var CallRemark = form["remark"];

                //ผลการตรวจสอบ ผอ.บล.
                var AuditUnderwriteResultStatus = form["AuditUnderwriteResultStatus"];

                //ผลการตรวจสอบการคัดกรอง
                var AuditUnderwriteStatus = form["AuditUnderwriteStatus"];

                //ผลการตรวจสอบการมอบบัตร
                var auditPoliciesGivenStatus = form["AuditPoliciesGivenStatus"];
                var AuditRemark = form["AuditRemark"];

                //ผลการตรวจสอบการชำระเบี้ย
                var AuditPaymentMotorStatus = form["AuditPaymentMotorStatus"];
                var AuditPaymentRemark = form["AuditPaymentMotorRemark"];

                var AuditMotorStatus = "";
                //การตรวจสอบข้อมูลการทำประกัน
                if (form["AuditMotorStatus"] != null)
                {
                    AuditMotorStatus = form["AuditMotorStatus"];
                }
                else
                {
                    AuditMotorStatus = form["AuditMotorStatus1"];
                }

                var AuditMotorRemark = form["AuditMotorRemark"];

                //รอพิจารณา -> ระบุในใบสมัคร-ไม่ระบุ
                var AuditMotorSpecifyStatus = form["AuditMotorSpecifyStatus"];
                var AuditMotorSpecifyRemark = form["AuditMotorSpecifyRemark"];

                //DECLARE
                int? userId = Convert.ToInt32(user);
                int? queueAuditStatusId = Convert.ToInt32(queueAuditStatus);
                int? queueAuditId = Convert.ToInt32(QueueAuditId);

                // 1 = na ค่า ตั้งต้น
                int? callStatusId = 1;
                int? auditPaymentStatusId = AuditPaymentMotorStatus != null ? Convert.ToInt32(AuditPaymentMotorStatus) : 1;
                int? auditMotorStatusId = 1;
                int? auditMotorSpecifyStatusId = 1;
                string auditRemark = null;
                string auditPaymentMotorRemark = null;
                string auditMotorRemark = null;
                int? auditUnderwriteStatusId = 1;
                int? auditPoliciesGivenStatusId = 1;

                switch (queueAuditStatusId)
                {
                    //ยังไม่ได้ตรวจสอบ
                    case 2:
                        //สถานะการโทร
                        callStatusId = Convert.ToInt32(CallStatus);
                        //หมายเหตุ *หลัก
                        auditRemark = CallRemark;
                        break;

                    //ตรวจสอบแล้ว
                    case 3:
                        //สถานะการโทร - โทรแล้ว
                        callStatusId = 5;
                        //ผลการตรวจสอบ ผอ.บล.
                        //auditUnderwriteResultStatusId = Convert.ToInt32(AuditUnderwriteResultStatus);
                        //หมายเหตุ ผลการตรวจสอบ ผอ.บล.
                        auditPaymentMotorRemark = AuditPaymentRemark;
                        //หมายเหตุ *หลัก
                        auditRemark = AuditRemark;
                        //ผลการตรวจสอบการคัดกรอง
                        auditUnderwriteStatusId = Convert.ToInt32(AuditUnderwriteStatus);
                        //ผลการตรวจสอบการมอบบัตร
                        auditPoliciesGivenStatusId = Convert.ToInt32(auditPoliciesGivenStatus);

                        //การตรวจสอบข้อมูลการทำประกัน
                        auditMotorStatusId = Convert.ToInt32(AuditMotorStatus);
                        //เช็ค id ผลการตรวจสอบสุขภาพ
                        switch (auditMotorStatusId)
                        {
                            //ผ่าน
                            case 2:
                                //หมายเหตุ-การตรวจสอบด้านสุขภาพ
                                auditMotorRemark = AuditMotorRemark;
                                break;
                            //รอพิจารณา
                            case 3:
                                //รอพิจารณา -> ระบุในใบสมัคร-ไม่ระบุ
                                auditMotorSpecifyStatusId = Convert.ToInt32(AuditMotorSpecifyStatus);
                                //เลือก รอพิจารณา ให้ระบุข้อยกเว้น ใน หมายเหตุ-การตรวจสอบด้านสุขภาพ
                                auditMotorRemark = AuditMotorSpecifyRemark;
                                break;

                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }

                var result = _context.usp_QueueMotorAudit1Result_Insert(queueAuditId: queueAuditId,
                                                                        queueAuditStatusId: queueAuditStatusId,
                                                                        callStatusId: callStatusId,
                                                                        auditUnderwriteStatusId: auditUnderwriteStatusId,
                                                                        auditPoliciesGivenStatusId: auditPoliciesGivenStatusId,
                                                                        auditRemark: auditRemark,
                                                                        auditPaymentStatusId: auditPaymentStatusId,
                                                                        auditPaymentRemark: auditPaymentMotorRemark,
                                                                        auditMotorStatusId: auditMotorStatusId,
                                                                        auditMotorSpecifyStatusId: auditMotorSpecifyStatusId,
                                                                        auditMotorRemark: auditMotorRemark,
                                                                        audit1CreatedByUserId: user).FirstOrDefault();

                //singnal R
                var chat = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                chat.Clients.Group(userId.ToString()).ReceiveGroupNotice("Success");

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //RETURN LIST
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        private bool? QuestionAuditUpdateV2CheckBoolType(string value)
        {
            switch (value)
            {
                case "0":
                    // ไม่มี/ไม่ถูกต้อง
                    return false;
                case "1":
                    // มี/ถูกต้อง
                    return true;
                case "2":
                //checkBox
                case "on":
                    return true;
                default:
                    // จำไม่ได้/null
                    return null;
            }
        }



        [HttpPost]
        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_InsureMotor")]
        public ActionResult QuestionAuditUpdateV2(FormCollection form)
        {
            //สถานะการตรวจสอบ
            var queueAuditStatus = form["QueueAuditStatusId"];
            if (!string.IsNullOrEmpty(queueAuditStatus))
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                var QueueAuditId = form["queueAuditId"];

                var IsUnderwriteInsured = form["IsUnderwriteInsured"];
                var IsUnderwritePayer = form["IsUnderwritePayer"];
                var IsSamePersonPayer = form["IsSamePersonPayer"];
                var IsSamePersonPayerRemark = form["IsSamePersonPayerRemark"];

                /// ยังไม่ได้ตรวจสอบ
                var CallStatusId = form["callStatusId"];
                var NotAuditCauseId = form["notAuditedCause"];          //สาเหตุ
                var AuditRemarkCallStatus = form["auditRemarkCallStatusId"];
                var AuditRemarkNotAuditedCause = form["auditRemarkNotAuditedCause"]; //หมายเหตุ

                /// ตรวจสอบแล้ว

                #region การนำเสนอขาย และบริการหลังการขาย
                // วิธีการขาย
                var SaleMethodTypeId = form["saleMethodType"];
                var SaleMethodTypeRemark = form["saleMethodTypeRemark"];
                // บริการหลังการขาย
                var SaleServiceTypeId = form["saleServiceType"];
                var SaleServiceTypeRemark = form["saleServiceTypeRemark"];
                //ได้รับกรมธรรม์เรียบร้อย
                var IsReceivedInsurance = form["IsReceivedInsurance"];
                var ReceivingInsuranceTypeId = form["ReceivingInsuranceType"];
                //มอบกรมธรรม์ให้กับ
                var InsuranceInsuredIsValid = form["InsuranceInsuredIsValid"];
                var InsuranceInsuredRemark = form["InsuranceInsuredRemark"];
                // ข้อมูล ผอ.บล
                var UnderwriteByUserId = form["underwriteByUserId"];
                var UnderwriteByUserRemark = form["underwriteByUserRemark"];


                // คะแนนความผึงพอใจ
                var FeedbackRate = form["feedbackRate"];
                var FeedbackRemark = form["feedbackRemark"];
                // ผลด้านบริการ
                var ServiceResultIsIssue = form["serviceResultIsIssue"];
                var ServiceResultRemark = form["serviceResultRemark"];

                #endregion

                #region ยืนยันข้อมูลส่วนบุคคล
                // ชื่อ-สกุล ผู้เอาประกัน
                var InsuredNameIsValid = form["insuredNameIsValid"];
                var InsuredNameRemark = form["insuredNameRemark"];
                // ชื่อ-สกุล ผู้ชำระเบี้ย
                var PayerNameIsValid = form["payerNameIsValid"];
                var PayerNameRemark = form["payerNameRemark"];
                // วันเดือนปีเกิด
                var BirthDateIsValid = form["birthDateIsValid"];
                var BirthDateRemark = form["birthDateRemark"];
                //ทะเบียนรถ
                var VehicleRegistrationNumberIsValid = form["VehicleRegistrationNumberIsValid"];
                var VehicleRegistrationNumberRemark = form["VehicleRegistrationNumberRemark"];
                //ยี่ห้อรถ
                var VehicleBrandModelDetailIsValid = form["VehicleBrandModelDetailIsValid"];
                var VehicleBrandModelDetailRemark = form["VehicleBrandModelDetailRemark"];
                //ประเภทการใช้งาน
                var VehicleUseTypeDetailIsValid = form["VehicleUseTypeDetailIsValid"];
                var VehicleUseTypeDetailRemark = form["VehicleUseTypeDetailRemark"];
                #endregion

                #region ประวัติการใช้รถ
                // ลักษณะการใช้รถ
                var VehicleUseTypeId = form["VehicleUseTypeId"];
                var VehicleUseTypeRemark = form["VehicleUseTypeRemark"];
                //สภาพรถ
                var IsDamaged = form["IsDamaged"];
                //สภาพความเสียหาย
                var DamageType = form["DamageType"];
                var DamageTypeRemark = form["DamageTypeRemark"];
                //รายละเอียดความเสียหาย
                var Damage = form["Damage"];
                var DamageRemark = form["DamageRemark"];

                //ตำแหน่งความเสียหาย
                var PositionDamagedId = form["PositionDamagedId"];
                var PositionDamagedRemark = form["PositionDamagedRemark"];

                //อุปกรณ์ตกแต่งเพิ่มเติม 
                var IsAccessories = form["IsAccessories"];
                var AccessoriesRemark = form["AccessoriesRemark"];
                #endregion


                #region ข้อมูลการชำระเบี้ยประกัน
                // อาชีพผู้เอาประกัน
                var PayerOccupationIsValid = form["payerOccupationIsValid"];
                var PayerOccupationRemark = form["payerOccupationRemark"];
                // หน่วยงานผู้ชำระเบี้ย
                var PayerBuildingNameIsValid = form["payerBuildingNameIsValid"];
                var PayerBuildingNameRemark = form["payerBuildingNameRemark"];
                // แผนประกัน
                var ProductIsValid = form["productIsValid"];
                var ProductRemark = form["productRemark"];
                // เบี้ยประกัน
                var PremiumIsValid = form["premiumIsValid"];
                var PremiumRemark = form["premiumRemark"];
                // ประเภทการชำระเบี้ยประกัน
                var PeriodTypeIsValid = form["periodTypeIsValid"];
                var PeriodTypeRemark = form["periodTypeRemark"];

                #endregion

                #region การตรวจสอบ
                // ผลการตรวจสอบ
                var AuditMotorStatusId = form["AuditMotorStatusId"];
                var AuditMotorSpecifyStatusId = form["AuditMotorSpecifyStatusId"];
                // หมายเหตุประวัตการคัดกรอง
                var AuditMotorRemark = form["AuditMotorRemark"];
                // พฤติกรรมการคัดกรอง(MA)
                var UnderwritingBehaviorTypeId = form["underwritingBehaviorTypeId"];
                var UnderwritingBehaviorRemark = form["underwritingBehaviorRemark"];
                // หมายเหตุอื่นๆ
                var AuditOtherMotorRemark = form["AuditOtherMotorRemark"];
                #endregion


                /* int damageType1xx = Array.IndexOf(DamageTypeArray, "DamageType1");
                 int damageType2xx = Array.IndexOf(DamageTypeArray, "DamageType2999");*/

                //DECLARE
                int? userId = Convert.ToInt32(user);
                int? queueAuditId = Convert.ToInt32(QueueAuditId);
                int? queueAuditStatusId = Convert.ToInt32(queueAuditStatus);
                int? callStatusId = Convert.ToInt32(CallStatusId);
                int? notAuditCauseId = Convert.ToInt32(NotAuditCauseId);
                int? auditMotorStatusId = Convert.ToInt32(AuditMotorStatusId);
                int? auditMotorSpecifyStatusId = 1;
                int? receivingInsuranceTypeId = 1;
                string auditRemark = null;

                bool? isUnderwriteInsured = null;
                bool? isUnderwritePayer = null;
                bool? isSamePersonPayer = null;
                string isSamePersonPayerRemark = null;

                #region การนำเสนอขาย และบริการหลังการขาย
                int? saleMethodTypeId = Convert.ToInt32(SaleMethodTypeId);
                int? saleServiceTypeId = Convert.ToInt32(SaleServiceTypeId);
                bool? isReceivedInsurance = null;
                bool? insuranceInsuredIsValid = null;
                int? underwriteByUserId = Convert.ToInt32(UnderwriteByUserId);
                int? feedbackRate = Convert.ToInt32(FeedbackRate);
                bool? serviceResultIsIssue = null;
                //หมายเหตุ
                string saleMethodTypeRemark = null;
                string saleServiceTypeRemark = null;
                string insuranceInsuredRemark = null;
                string underwriteByUserRemark = null;
                string feedbackRemark = null;
                string serviceResultRemark = null;
                #endregion
                #region ยืนยันข้อมูลส่วนบุคคล
                bool? insuredNameIsValid = null;
                bool? payerNameIsValid = null;
                bool? birthDateIsValid = null;
                bool? vehicleRegistrationNumberIsValid = null;
                bool? vehicleBrandModelDetailIsValid = null;
                bool? vehicleUseTypeDetailIsValid = null;

                //หมายเหตุ
                string insuredNameRemark = null;
                string payerNameRemark = null;
                string birthDateRemark = null;
                string vehicleRegistrationNumberRemark = null;
                string vehicleBrandModelDetailRemark = null;
                string vehicleUseTypeDetailRemark = null;
                #endregion
                #region ประวัติการใช้รถ
                int? vehicleUseTypeId = Convert.ToInt32(VehicleUseTypeId);
                bool? isDamaged = null;
                bool? damageType1 = null;
                bool? damageType2 = null;
                bool? damageType3 = null;
                bool? damageType4 = null;
                bool? damage1 = null;
                bool? damage2 = null;
                bool? damage3 = null;
                bool? damage4 = null;
                bool? damage5 = null;
                bool? damage6 = null;
                bool? damage7 = null;
                bool? damage8 = null;
                bool? damage9 = null;
                bool? damage10 = null;
                bool? damage11 = null;
                bool? damage12 = null;
                bool? damage13 = null;
                bool? damage14 = null;
                bool? damage15 = null;
                bool? damage16 = null;
                bool? damage17 = null;
                bool? damage18 = null;
                int? positionDamagedId = Convert.ToInt32(PositionDamagedId);
                bool? isAccessories = null;
                //หมายเหตุ
                string vehicleUseTypeRemark = null;
                string damageTypeRemark = null;
                string damageRemark = null;
                string positionDamagedRemark = null;
                string accessoriesRemark = null;
                #endregion

                #region ข้อมูลการชำระเบี้ยประกัน
                bool? payerOccupationIsValid = null;
                bool? payerBuildingNameIsValid = null;
                bool? productIsValid = null;
                bool? premiumIsValid = null;
                bool? periodTypeIsValid = null;

                //หมายเหตุ
                string payerOccupationRemark = null;
                string payerBuildingNameRemark = null;
                string productRemark = null;
                string premiumRemark = null;
                string periodTypeRemark = null;
                #endregion

                #region การตรวจสอบ
                int? underwritingBehaviorTypeId = Convert.ToInt32(UnderwritingBehaviorTypeId);

                string auditMotorRemark = null;
                string auditOtherMotorRemark = null;
                string underwritingBehaviorRemark = null;
                #endregion




                switch (queueAuditStatusId)
                {
                    //ยังไม่ได้ตรวจสอบ
                    case 2:
                        //สถานะการโทร
                        callStatusId = Convert.ToInt32(CallStatusId);
                        // สาเหตุ
                        // notAuditCauseId = Convert.ToInt32(NotAuditCauseId);
                        // หมายเหตุ
                        auditRemark = AuditRemarkCallStatus.Trim() == "" ? auditRemark : AuditRemarkCallStatus;
                        //ไม่ต้องตรวจสอบ masrter = 1 เพราะไม่ได้เลือก
                        receivingInsuranceTypeId = 1; //n/a
                        saleMethodTypeId = 1; //n/a
                        saleServiceTypeId = 1; //n/a
                        positionDamagedId = 1; //n/a
                        underwritingBehaviorTypeId = 1;
                        feedbackRate = 0;
                        auditMotorStatusId = 1;
                        vehicleUseTypeId = 1;

                        break;

                    //ไม่ต้องตรวจสอบ
                    case 4:
                        //สถานะการโทร
                        //callStatusId = Convert.ToInt32(CallStatusId);
                        // สาเหตุ
                        notAuditCauseId = Convert.ToInt32(NotAuditCauseId);
                        // หมายเหตุ
                        auditRemark = AuditRemarkNotAuditedCause.Trim() == "" ? auditRemark : AuditRemarkNotAuditedCause;
                        //ไม่ต้องตรวจสอบ masrter = 1 เพราะไม่ได้เลือก
                        receivingInsuranceTypeId = 1; //n/a
                        saleMethodTypeId = 1; //n/a
                        saleServiceTypeId = 1; //n/a
                        positionDamagedId = 1; //n/a
                        underwritingBehaviorTypeId = 1;
                        feedbackRate = 0;
                        auditMotorStatusId = 1;
                        vehicleUseTypeId = 1;
                        break;

                    //ตรวจสอบแล้ว
                    case 3:
                        //สถานะการโทร - โทรแล้ว
                        callStatusId = 5;

                        // หมายเหตุเพิ่มเติม
                        #region การนำเสนอขาย และบริการหลังการขาย
                        saleMethodTypeRemark = SaleMethodTypeRemark.Trim() == "" ? saleMethodTypeRemark : SaleMethodTypeRemark;
                        saleServiceTypeRemark = SaleServiceTypeRemark.Trim() == "" ? saleServiceTypeRemark : SaleServiceTypeRemark;
                        underwriteByUserRemark = UnderwriteByUserRemark.Trim() == "" ? underwriteByUserRemark : UnderwriteByUserRemark;
                        feedbackRemark = FeedbackRemark.Trim() == "" ? feedbackRemark : FeedbackRemark;
                        serviceResultRemark = ServiceResultRemark.Trim() == "" ? serviceResultRemark : ServiceResultRemark;
                        insuranceInsuredRemark = InsuranceInsuredRemark.Trim() == "" ? insuranceInsuredRemark : InsuranceInsuredRemark;
                        #endregion
                        #region ยืนยันข้อมูลส่วนบุคคล
                        insuredNameRemark = InsuredNameRemark.Trim() == "" ? insuredNameRemark : InsuredNameRemark;
                        payerNameRemark = PayerNameRemark.Trim() == "" ? payerNameRemark : PayerNameRemark;
                        birthDateRemark = BirthDateRemark.Trim() == "" ? birthDateRemark : BirthDateRemark;
                        vehicleRegistrationNumberRemark = VehicleRegistrationNumberRemark.Trim() == "" ? vehicleRegistrationNumberRemark : VehicleRegistrationNumberRemark;
                        vehicleBrandModelDetailRemark = VehicleBrandModelDetailRemark.Trim() == "" ? vehicleBrandModelDetailRemark : VehicleBrandModelDetailRemark;
                        vehicleUseTypeDetailRemark = VehicleUseTypeDetailRemark.Trim() == "" ? vehicleUseTypeDetailRemark : VehicleUseTypeDetailRemark;
                        #endregion
                        #region ประวัติการใช้รถ
                        vehicleUseTypeRemark = VehicleUseTypeRemark.Trim() == "" ? vehicleUseTypeRemark : VehicleUseTypeRemark;

                        if (IsDamaged == "1") // 1 มีความเสียหาย
                        {
                            damageTypeRemark = DamageTypeRemark.Trim() == "" ? damageTypeRemark : DamageTypeRemark;
                            damageRemark = DamageRemark.Trim() == "" ? damageRemark : DamageRemark;
                            positionDamagedRemark = PositionDamagedRemark.Trim() == "" ? positionDamagedRemark : PositionDamagedRemark;

                        }


                        accessoriesRemark = AccessoriesRemark.Trim() == "" ? accessoriesRemark : AccessoriesRemark;
                        #endregion
                        #region ข้อมูลการชำระเบี้ยประกัน
                        payerOccupationRemark = PayerOccupationRemark.Trim() == "" ? payerOccupationRemark : PayerOccupationRemark;
                        payerBuildingNameRemark = PayerBuildingNameRemark.Trim() == "" ? payerBuildingNameRemark : PayerBuildingNameRemark;
                        productRemark = ProductRemark.Trim() == "" ? productRemark : ProductRemark;
                        premiumRemark = PremiumRemark.Trim() == "" ? premiumRemark : PremiumRemark;
                        periodTypeRemark = PeriodTypeRemark.Trim() == "" ? periodTypeRemark : PeriodTypeRemark;
                        #endregion

                        #region การตรวจสอบ
                        auditMotorRemark = AuditMotorRemark.Trim() == "" ? auditMotorRemark : AuditMotorRemark;
                        underwritingBehaviorRemark = UnderwritingBehaviorRemark.Trim() == "" ? underwritingBehaviorRemark : UnderwritingBehaviorRemark;
                        auditOtherMotorRemark = AuditOtherMotorRemark.Trim() == "" ? auditOtherMotorRemark : AuditOtherMotorRemark;
                        #endregion
                        isSamePersonPayerRemark = IsSamePersonPayerRemark.Trim() == "" ? isSamePersonPayerRemark : IsSamePersonPayerRemark;

                        // Radio box
                        insuranceInsuredIsValid = QuestionAuditUpdateV2CheckBoolType(InsuranceInsuredIsValid);
                        serviceResultIsIssue = QuestionAuditUpdateV2CheckBoolType(ServiceResultIsIssue);
                        insuredNameIsValid = QuestionAuditUpdateV2CheckBoolType(InsuredNameIsValid);
                        payerNameIsValid = QuestionAuditUpdateV2CheckBoolType(PayerNameIsValid);
                        birthDateIsValid = QuestionAuditUpdateV2CheckBoolType(BirthDateIsValid);
                        vehicleRegistrationNumberIsValid = QuestionAuditUpdateV2CheckBoolType(VehicleRegistrationNumberIsValid);
                        vehicleBrandModelDetailIsValid = QuestionAuditUpdateV2CheckBoolType(VehicleBrandModelDetailIsValid);
                        vehicleUseTypeDetailIsValid = QuestionAuditUpdateV2CheckBoolType(VehicleUseTypeDetailIsValid);
                        isDamaged = QuestionAuditUpdateV2CheckBoolType(IsDamaged);
                        if (DamageType != null)
                        {
                            string[] DamageTypeArray = DamageType.Split(',');
                            damageType1 = (Array.IndexOf(DamageTypeArray, "DamageType1") != -1); //QuestionAuditUpdateV2CheckBoolType(DamageType1);
                            damageType2 = (Array.IndexOf(DamageTypeArray, "DamageType2") != -1); //QuestionAuditUpdateV2CheckBoolType(DamageType2);
                            damageType3 = (Array.IndexOf(DamageTypeArray, "DamageType3") != -1); //QuestionAuditUpdateV2CheckBoolType(DamageType3);
                            damageType4 = (Array.IndexOf(DamageTypeArray, "DamageType4") != -1); //QuestionAuditUpdateV2CheckBoolType(DamageType4);
                        }
                        if (Damage != null)
                        {
                            string[] DamageArray = Damage.Split(',');
                            damage1 = (Array.IndexOf(DamageArray, "Damage1") != -1); //QuestionAuditUpdateV2CheckBoolType(Damage1);
                            damage2 = (Array.IndexOf(DamageArray, "Damage2") != -1); //QuestionAuditUpdateV2CheckBoolType(Damage2);
                            damage3 = (Array.IndexOf(DamageArray, "Damage3") != -1); //QuestionAuditUpdateV2CheckBoolType(Damage3);
                            damage4 = (Array.IndexOf(DamageArray, "Damage4") != -1); //QuestionAuditUpdateV2CheckBoolType(Damage4);
                            damage5 = (Array.IndexOf(DamageArray, "Damage5") != -1); //QuestionAuditUpdateV2CheckBoolType(Damage5);
                            damage6 = (Array.IndexOf(DamageArray, "Damage6") != -1); //QuestionAuditUpdateV2CheckBoolType(Damage6);
                            damage7 = (Array.IndexOf(DamageArray, "Damage7") != -1); //QuestionAuditUpdateV2CheckBoolType(Damage7);
                            damage8 = (Array.IndexOf(DamageArray, "Damage8") != -1); //QuestionAuditUpdateV2CheckBoolType(Damage8);
                            damage9 = (Array.IndexOf(DamageArray, "Damage9") != -1); //QuestionAuditUpdateV2CheckBoolType(Damage9);
                            damage10 = (Array.IndexOf(DamageArray, "Damage10") != -1); //QuestionAuditUpdateV2CheckBoolType(Damage10);
                            damage11 = (Array.IndexOf(DamageArray, "Damage11") != -1); //QuestionAuditUpdateV2CheckBoolType(Damage11);
                            damage12 = (Array.IndexOf(DamageArray, "Damage12") != -1); //QuestionAuditUpdateV2CheckBoolType(Damage12);
                            damage13 = (Array.IndexOf(DamageArray, "Damage13") != -1); //QuestionAuditUpdateV2CheckBoolType(Damage13);
                            damage14 = (Array.IndexOf(DamageArray, "Damage14") != -1); //QuestionAuditUpdateV2CheckBoolType(Damage14);
                            damage15 = (Array.IndexOf(DamageArray, "Damage15") != -1); //QuestionAuditUpdateV2CheckBoolType(Damage15);
                            damage16 = (Array.IndexOf(DamageArray, "Damage16") != -1); //QuestionAuditUpdateV2CheckBoolType(Damage16);
                            damage17 = (Array.IndexOf(DamageArray, "Damage17") != -1); //QuestionAuditUpdateV2CheckBoolType(Damage17);
                            damage18 = (Array.IndexOf(DamageArray, "Damage18") != -1); //QuestionAuditUpdateV2CheckBoolType(Damage18);
                        }

                        isAccessories = QuestionAuditUpdateV2CheckBoolType(IsAccessories);
                        payerOccupationIsValid = QuestionAuditUpdateV2CheckBoolType(PayerOccupationIsValid);
                        payerBuildingNameIsValid = QuestionAuditUpdateV2CheckBoolType(PayerBuildingNameIsValid);
                        productIsValid = QuestionAuditUpdateV2CheckBoolType(ProductIsValid);
                        premiumIsValid = QuestionAuditUpdateV2CheckBoolType(PremiumIsValid);
                        periodTypeIsValid = QuestionAuditUpdateV2CheckBoolType(PeriodTypeIsValid);
                        isUnderwriteInsured = QuestionAuditUpdateV2CheckBoolType(IsUnderwriteInsured);
                        isUnderwritePayer = QuestionAuditUpdateV2CheckBoolType(IsUnderwritePayer);
                        isSamePersonPayer = QuestionAuditUpdateV2CheckBoolType(IsSamePersonPayer);

                        //// การนำเสนอขาย และบริการหลังการขาย
                        //ได้รับกรมธรรม์เรียบร้อย
                        isReceivedInsurance = QuestionAuditUpdateV2CheckBoolType(IsReceivedInsurance);
                        receivingInsuranceTypeId = isReceivedInsurance != null && (bool)isReceivedInsurance ? Convert.ToInt32(ReceivingInsuranceTypeId) : receivingInsuranceTypeId; // default receivingManualTypeId = 1

                        //// ผลการตรวจสอบ ด้านสุขภาพ
                        auditMotorStatusId = Convert.ToInt32(AuditMotorStatusId);
                        //เช็ค id ผลการตรวจสอบสุขภาพ
                        switch (auditMotorStatusId)
                        {
                            //ผ่าน
                            case 2:
                                //หมายเหตุ-การตรวจสอบด้านสุขภาพ
                                auditMotorRemark = AuditMotorRemark.Trim() == "" ? auditMotorRemark : AuditMotorRemark;
                                break;
                            //รอพิจารณา
                            case 3:
                                //รอพิจารณา -> ระบุในใบสมัคร-ไม่ระบุ
                                auditMotorSpecifyStatusId = Convert.ToInt32(AuditMotorSpecifyStatusId);

                                //หมายเหตุ-การตรวจสอบด้านสุขภาพ
                                auditMotorRemark = AuditMotorRemark.Trim() == "" ? auditMotorRemark : AuditMotorRemark;
                                break;

                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }

                var result = _context.usp_QueueMotorAudit1ResultV2_Insert(
                    queueAuditId,
                    queueAuditStatusId,
                    callStatusId == -1 ? 1 : callStatusId,
                    notAuditCauseId == -1 ? 1 : notAuditCauseId,
                    auditRemark,
                    isUnderwriteInsured,
                    isUnderwritePayer,
                    isSamePersonPayer,
                    isSamePersonPayerRemark,
                    saleMethodTypeId,
                    saleMethodTypeRemark,
                    saleServiceTypeId,
                    saleServiceTypeRemark,
                    isReceivedInsurance,
                    receivingInsuranceTypeId == -1 ? 1 : receivingInsuranceTypeId,
                    insuranceInsuredIsValid,
                    insuranceInsuredRemark,
                    underwriteByUserId,
                    underwriteByUserRemark,
                    feedbackRate == 6 ? 0 : feedbackRate,
                    feedbackRemark,
                    serviceResultIsIssue,
                    serviceResultRemark,
                    underwritingBehaviorTypeId,
                    underwritingBehaviorRemark,
                    insuredNameIsValid,
                    insuredNameRemark,
                    payerNameIsValid,
                    payerNameRemark,
                    birthDateIsValid,
                    birthDateRemark,
                    vehicleRegistrationNumberIsValid,
                    vehicleRegistrationNumberRemark,
                    vehicleBrandModelDetailIsValid,
                    vehicleBrandModelDetailRemark,
                    vehicleUseTypeDetailIsValid,
                    vehicleUseTypeDetailRemark,
                    vehicleUseTypeId == -1 ? 1 : vehicleUseTypeId,
                    vehicleUseTypeRemark,
                    isDamaged,
                    damageType1,
                    damageType2,
                    damageType3,
                    damageType4,
                    damageTypeRemark,
                   damage1,
                   damage2,
                    damage3,
                    damage4,
                    damage5,
                    damage6,
                    damage7,
                    damage8,
                    damage9,
                    damage10,
                    damage11,
                    damage12,
                    damage13,
                    damage14,
                    damage15,
                    damage16,
                    damage17,
                    damage18,
                    damageRemark,
                    positionDamagedId,
                    positionDamagedRemark,
                    isAccessories,
                    accessoriesRemark,
                    payerOccupationIsValid,
                    payerOccupationRemark,
                    payerBuildingNameIsValid,
                    payerBuildingNameRemark,
                    productIsValid,
                    productRemark,
                    premiumIsValid,
                    premiumRemark,
                    periodTypeIsValid,
                    periodTypeRemark,
                    auditMotorStatusId,
                    auditMotorSpecifyStatusId,
                    auditMotorRemark,
                    auditOtherMotorRemark,
                    userId).FirstOrDefault();

                //singnal R
                var chat = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                chat.Clients.Group(userId.ToString()).ReceiveGroupNotice("Success");

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //RETURN LIST
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpGet]
        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_InsureMotor")]
        public ActionResult QueueMotorAuditLog(int queueAuditId, int draw, int? indexStart, int? pageSize, string sortField, string orderType, string search)
        {
            var result = _context.usp_QueueMotorAuditLogByQueueAuditId_Select(queueAuditId, indexStart, pageSize, sortField, orderType, search).ToList();
            var dt = new Dictionary<string, object>
            {
                 {"draw", draw },
                 {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                 {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                 {"data", result.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_InsureMotor")]
        public ActionResult QuestionAuditInsureUpdate(FormCollection form)
        {
            //queue audit id
            var QueueAuditId = form["QueueAuditId_formInsure"];

            if (!string.IsNullOrEmpty(QueueAuditId))
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var AuditInsureStatus = form["AuditInsureStatus"];
                //แถบพิจารณาแล้วจะ disable AuditInsureStatus ทำให้ AuditInsureStatus ส่งมาเป็น null ต้องใช้ status เดิม คือ AuditInsureStatusId_formInsure
                if (AuditInsureStatus == null)
                {
                    AuditInsureStatus = form["AuditInsureStatusId_formInsure"]; //status เดิม
                }
                //สถานะพิจารณา
                //var AuditInsureStatus = form["AuditInsureStatusId_formInsure"];
                var AuditInsureStatusRemark = form["AuditInsureStatusRemark"];
                //พิจารณาแล้ว --> ผ่าน-ติดเงื่อนไข-ไม่ผ่าน
                var AuditInsureConsiderStatus = form["AuditInsureConsiderStatus"];
                var AuditInsureConsiderStatusRemark = form["AuditInsureConsiderStatusRemark"];

                //DECLARE
                int? userId = Convert.ToInt32(user);
                int? queueAuditId = Convert.ToInt32(QueueAuditId);
                int? auditInsureStatusId = Convert.ToInt32(AuditInsureStatus);

                // 1 = na ค่า ตั้งต้น
                int? auditInsureConsiderStatusId = 1;
                string auditInsureRemark = null;
                string auditInsureConsiderRemark = null;

                switch (auditInsureStatusId)
                {
                    //รอพิจารณา
                    case 2:
                        //หมายเหตุ
                        auditInsureRemark = AuditInsureStatusRemark;
                        break;
                    //พิจารณาแล้ว
                    case 3:
                        //ผลพิจารณา -ผ่าน -ไม่ผ่าน -ผ่านติดเงื่อนไข
                        auditInsureConsiderStatusId = Convert.ToInt32(AuditInsureConsiderStatus);
                        //หมายเหตุ
                        auditInsureConsiderRemark = AuditInsureConsiderStatusRemark;
                        break;

                    default:
                        break;
                }

                var result = _context.usp_QueueMotorAudit2Result_Insert(queueAuditId, auditInsureStatusId, auditInsureRemark, auditInsureConsiderStatusId, auditInsureConsiderRemark, userId).FirstOrDefault();

                //singnal R
                var chat = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                chat.Clients.Group(userId.ToString()).ReceiveGroupNotice("Success");

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //RETURN LIST
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [Authorization(Roles = "Developer,UnderwriteAudit_BusinessPromoteTeam,UnderwriteAudit_InsureMotor")]
        public ActionResult QuestionAuditInsureUpdateV2(FormCollection form)
        {
            //queue audit id
            var QueueAuditId = form["QueueAuditId_formInsure"];

            if (!string.IsNullOrEmpty(QueueAuditId))
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                //สถานะพิจารณา
                var AuditInsureStatus = form["AuditInsureStatus"];
                //พิจารณาแล้ว --> ผ่าน-ติดเงื่อนไข-ไม่ผ่าน
                var AuditInsureConsiderStatus = form["AuditInsureConsiderStatus"];
                var AuditInsureConsiderStatusRemark = form["AuditInsureConsiderStatusRemark"];
                var AuditInsureClose = form["AuditInsureClose"];
                var AuditInsureCloseRemark = form["AuditInsureCloseRemark"];

                //DECLARE
                int? userId = Convert.ToInt32(user);
                int? queueAuditId = Convert.ToInt32(QueueAuditId);
                int? auditInsureStatusId = Convert.ToInt32(AuditInsureStatus);

                // 1 = na ค่า ตั้งต้น
                int? auditInsureConsiderStatusId = 1;
                string auditInsureConsiderRemark = AuditInsureConsiderStatusRemark;
                bool? auditInsureClose = QuestionAuditUpdateV2CheckBoolType(AuditInsureClose);
                string auditInsureCloseRemark = AuditInsureCloseRemark;
                switch (auditInsureStatusId)
                {
                    //พิจารณาแล้ว
                    case 3:
                        //ผลพิจารณา -ผ่าน -ไม่ผ่าน -ผ่านติดเงื่อนไข
                        auditInsureConsiderStatusId = Convert.ToInt32(AuditInsureConsiderStatus);
                        //หมายเหตุ
                        // auditInsureConsiderRemark = AuditInsureConsiderStatusRemark;
                        break;
                    //รอพิจารณา

                    default:
                        break;
                }

                var result = _context.usp_QueueMotorAudit2ResultV2_Insert(
                        queueAuditId,
                        auditInsureStatusId,
                        auditInsureConsiderStatusId,
                        auditInsureConsiderRemark,
                        auditInsureStatusId == 3 ? true : false,
                        auditInsureCloseRemark,
                        userId
                    ).FirstOrDefault();

                //singnal R
                var chat = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                chat.Clients.Group(userId.ToString()).ReceiveGroupNotice("Success");

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //RETURN LIST
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult UnderwriteHistory(int queueId, int draw, int? indexStart, int? pageSize, string sortField, string orderType)
        {
            var result = _contextMotor.usp_QueueMotorUnderwriteHistory_Select(queueId, indexStart, pageSize, sortField, orderType).ToList();

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
        public ActionResult UnderwritePaymentHistory(int queueId, int draw, int? indexStart, int? pageSize, string sortField, string orderType)
        {
            var result = _contextMotor.usp_QueueMotorPaymentHistory_Select(queueId, indexStart, pageSize, sortField, orderType).ToList();

            var dt = new Dictionary<string, object>
            {
                 {"draw", draw },
                 {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                 {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                 {"data", result.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        #endregion method
    }
}