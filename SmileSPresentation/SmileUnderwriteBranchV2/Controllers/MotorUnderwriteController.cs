using ImageResizer;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using RestSharp;
using SmileUnderwriteBranchV2.Helper;
using SmileUnderwriteBranchV2.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SmileUnderwriteBranchV2.Controllers
{
    [Authorization]
    public class MotorUnderwriteController : Controller
    {
        #region Context

        private readonly UnderwriteBranchV2MotorModelContainer _context;
        private readonly UnderwriteBranchV2Entities _contextPH;

        public MotorUnderwriteController()
        {
            _context = new UnderwriteBranchV2MotorModelContainer();
            _contextPH = new UnderwriteBranchV2Entities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            _contextPH.Dispose();
        }

        #endregion Context

        public ActionResult MotorUnderwriteViewDetailV1(string queueId, string hiddenButton)
        {
            int? QueueId = null;
            try
            {
                if (queueId != null && queueId != "")
                {
                    byte[] dataQueue = Convert.FromBase64String(queueId);
                    var decodedQueueIdString = Encoding.UTF8.GetString(dataQueue);
                    QueueId = Convert.ToInt32(decodedQueueIdString);
                }
                else
                {
                    return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "InnerException:" + e.InnerException + ", Message:" + e.Message });
            }

            try
            {
                //Get Role
                var user = GlobalFunction.GetLoginDetail(HttpContext);
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                var accessRole = GlobalFunction.GetAccessRole(roleList);
                ViewBag.AccessRole = accessRole;

                var result = _context.usp_QueueMotorFullDetailByQueueId_Select(QueueId, null).FirstOrDefault();
                ViewBag.QueueMotorFullDetail = result;
                ViewBag.QueueId = result.QueueId;
                ViewBag.AppCode = result.ApplicationCode;
                var applicationCode = result.ApplicationCode;
                var result2 = _context.usp_UnderwriteTypeMotor_Select(null).ToList();
                ViewBag.UnderwriteTypeMotor = result2;

                var result3 = _context.usp_UnderwritePaymentTypeMotor_Select(null).ToList();
                ViewBag.UnderwritePaymentTypeMotor = result3;

                var result4 = _context.usp_CallStatusMotor_Select(null).ToList();
                ViewBag.CallStatusListMotor = result4;

                //check date for edit
                var MotorCMDueDate = Properties.Settings.Default.MotorCMDueDate;
                ViewBag.IsEdit = false;

                var now = DateTime.Now.Date;
                var AppDCR = result.Period.Value;

                // Documents
                var docstorage = Properties.Settings.Default.docstorage;
                ViewBag.docstorage = docstorage;

                #region Documents


                //new Documents from docStorage
                //var documentResult = _context.usp_DocumentUnderwriteByQueueId_Select(QueueId, 2).ToList();

                //if (documentResult != null)
                //{
                //    ViewBag.documentResult = documentResult;
                //    var documentIdList = documentResult.Select(x => x.DocumentId).ToList();

                //    var docstorageApi = Properties.Settings.Default.docstorageApi;

                //    string apiUrl = docstorageApi;

                //    // Create a RestSharp client
                //    var client = new RestClient(apiUrl);

                //    // Create a request with parameters
                //    var request = new RestRequest(Method.GET);
                //    /* request.AddParameter("documentIds", "value1");
                //     request.AddParameter("param2", "value2");*/
                //    foreach (var item in documentIdList)
                //    {
                //        request.AddParameter("documentIds", item);
                //    }


                //    // Execute the request and get the response
                //    IRestResponse response = client.Execute(request);

                //    // Check if the request was successful (statu s code 200)
                //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                //    {
                //        ViewBag.ApiResponse = response.Content;
                //        var dataResponse = JsonConvert.DeserializeObject<DataResponse>(response.Content);
                //        ViewBag.docFile = dataResponse.data.ToList();
                //        var dataResponseMerge = dataResponse.data.ToList();


                //    }

                //}




                #endregion Documents

                // เช็ค("แก้ไขผลการพิจารณา" จะแสดงถึงวันที่ 5 ของเดือนที่คุ้มครอง)
                if ((AppDCR.Month == now.Month && now.Day <= MotorCMDueDate) || (AppDCR > now))
                {
                    ViewBag.IsEdit = true;
                }

                //ปีจดทะเบียน BE
                ViewBag.VehicleRegistrationYear = "-";
                if (!String.IsNullOrEmpty(result.VehicleRegistrationYear))
                {
                    ViewBag.VehicleRegistrationYear = Convert.ToInt32(result.VehicleRegistrationYear) - 543;
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

                //GiveDate
                if (result.GiveDate != null)
                {
                    ViewBag.GiveDate = string.Format("{0}/{1}/{2}", result.GiveDate.Value.Day.ToString("00", null), result.GiveDate.Value.Month.ToString("00", null), result.GiveDate.Value.Year + 543);
                }

                //IssuesPolicyDate
                if (result.IssuesPolicyDate != null)
                {
                    ViewBag.IssuesPolicyDate = string.Format("{0}/{1}/{2} {3}", result.IssuesPolicyDate.Value.Day.ToString("00", null), result.IssuesPolicyDate.Value.Month.ToString("00", null), result.IssuesPolicyDate.Value.Year + 543, result.IssuesPolicyDate.Value.ToString("HH:mm:ss"));
                }

                //InsuredBirthDate
                if (result.InsuredBirthDate != null)
                {
                    ViewBag.InsuredBirthDate = string.Format("{0}/{1}/{2}", result.InsuredBirthDate.Value.Day.ToString("00", null), result.InsuredBirthDate.Value.Month.ToString("00", null), result.InsuredBirthDate.Value.Year + 543);
                }

                //PayerBirthDate
                if (result.PayerBirthDate != null)
                {
                    ViewBag.PayerBirthDate = string.Format("{0}/{1}/{2}", result.PayerBirthDate.Value.Day.ToString("00", null), result.PayerBirthDate.Value.Month.ToString("00", null), result.PayerBirthDate.Value.Year + 543);
                }

                //DriverBirthDate
                if (result.DriverBirthDate != null)
                {
                    ViewBag.DriverBirthDate = string.Format("{0}/{1}/{2}", result.DriverBirthDate.Value.Day.ToString("00", null), result.DriverBirthDate.Value.Month.ToString("00", null), result.DriverBirthDate.Value.Year + 543);
                }

                //เบี้ยร์ประกัน 2 digits
                if (result.Premium != null)
                {
                    ViewBag.Premium = GlobalFunction.NumberFormatInfo(result.Premium.Value.ToString(), 2);
                }

                //check view only
                if (hiddenButton == "Y")
                {
                    ViewBag.hiddenButton = true;
                }
                else
                {
                    ViewBag.hiddenButton = false;
                }

                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult MotorUnderwriteViewDetail(string queueId, string hiddenButton)
        {
            int? QueueId = null;
            try
            {
                if (queueId != null && queueId != "")
                {
                    byte[] dataQueue = Convert.FromBase64String(queueId);
                    var decodedQueueIdString = Encoding.UTF8.GetString(dataQueue);
                    QueueId = Convert.ToInt32(decodedQueueIdString);
                }
                else
                {
                    return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "InnerException:" + e.InnerException + ", Message:" + e.Message });
            }

            try
            {
                //Get Role
                var user = GlobalFunction.GetLoginDetail(HttpContext);
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                var accessRole = GlobalFunction.GetAccessRole(roleList);
                ViewBag.AccessRole = accessRole;

                var result = _context.usp_QueueMotorFullDetailByQueueId_Select(QueueId, null).FirstOrDefault();
                if (result.IsPolicies != true)
                {
                    return RedirectToAction("MotorUnderwriteViewDetailV1", new { queueId = queueId });

                }
                ViewBag.QueueMotorFullDetail = result;
                ViewBag.QueueId = result.QueueId;
                ViewBag.AppCode = result.ApplicationCode;
                var applicationCode = result.ApplicationCode;
                var result2 = _context.usp_UnderwriteTypeMotor_Select(null).ToList();
                ViewBag.UnderwriteTypeMotor = result2;

                var result3 = _context.usp_UnderwritePaymentTypeMotor_Select(null).ToList();
                ViewBag.UnderwritePaymentTypeMotor = result3;

                var result4 = _context.usp_CallStatusMotor_Select(null).ToList();
                ViewBag.CallStatusListMotor = result4;

                //check date for edit
                var MotorCMDueDate = Properties.Settings.Default.MotorCMDueDate;
                ViewBag.IsEdit = false;

                var now = DateTime.Now.Date;
                var AppDCR = result.Period.Value;

                // Documents
                var docstorage = Properties.Settings.Default.docstorage;
                ViewBag.docstorage = docstorage;

                #region Documents


                //new Documents from docStorage
                //var documentResult = _context.usp_DocumentUnderwriteByQueueId_Select(QueueId, 2).ToList();

                //if (documentResult != null)
                //{
                //    ViewBag.documentResult = documentResult;
                //    var documentIdList = documentResult.Select(x => x.DocumentId).ToList();

                //    var docstorageApi = Properties.Settings.Default.docstorageApi;

                //    string apiUrl = docstorageApi;

                //    // Create a RestSharp client
                //    var client = new RestClient(apiUrl);

                //    // Create a request with parameters
                //    var request = new RestRequest(Method.GET);
                //    /* request.AddParameter("documentIds", "value1");
                //     request.AddParameter("param2", "value2");*/
                //    foreach (var item in documentIdList)
                //    {
                //        request.AddParameter("documentIds", item);
                //    }


                //    // Execute the request and get the response
                //    IRestResponse response = client.Execute(request);

                //    // Check if the request was successful (statu s code 200)
                //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                //    {
                //        ViewBag.ApiResponse = response.Content;
                //        var dataResponse = JsonConvert.DeserializeObject<DataResponse>(response.Content);
                //        ViewBag.docFile = dataResponse.data.ToList();
                //        var dataResponseMerge = dataResponse.data.ToList();


                //    }

                //}




                #endregion Documents

                // เช็ค("แก้ไขผลการพิจารณา" จะแสดงถึงวันที่ 5 ของเดือนที่คุ้มครอง)
                if ((AppDCR.Month == now.Month && now.Day <= MotorCMDueDate) || (AppDCR > now))
                {
                    ViewBag.IsEdit = true;
                }

                //ปีจดทะเบียน BE
                ViewBag.VehicleRegistrationYear = "-";
                if (!String.IsNullOrEmpty(result.VehicleRegistrationYear))
                {
                    ViewBag.VehicleRegistrationYear = Convert.ToInt32(result.VehicleRegistrationYear) - 543;
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

                //GiveDate
                if (result.GiveDate != null)
                {
                    ViewBag.GiveDate = string.Format("{0}/{1}/{2}", result.GiveDate.Value.Day.ToString("00", null), result.GiveDate.Value.Month.ToString("00", null), result.GiveDate.Value.Year + 543);
                }

                //IssuesPolicyDate
                if (result.IssuesPolicyDate != null)
                {
                    ViewBag.IssuesPolicyDate = string.Format("{0}/{1}/{2} {3}", result.IssuesPolicyDate.Value.Day.ToString("00", null), result.IssuesPolicyDate.Value.Month.ToString("00", null), result.IssuesPolicyDate.Value.Year + 543, result.IssuesPolicyDate.Value.ToString("HH:mm:ss"));
                }

                //InsuredBirthDate
                if (result.InsuredBirthDate != null)
                {
                    ViewBag.InsuredBirthDate = string.Format("{0}/{1}/{2}", result.InsuredBirthDate.Value.Day.ToString("00", null), result.InsuredBirthDate.Value.Month.ToString("00", null), result.InsuredBirthDate.Value.Year + 543);
                }

                //PayerBirthDate
                if (result.PayerBirthDate != null)
                {
                    ViewBag.PayerBirthDate = string.Format("{0}/{1}/{2}", result.PayerBirthDate.Value.Day.ToString("00", null), result.PayerBirthDate.Value.Month.ToString("00", null), result.PayerBirthDate.Value.Year + 543);
                }

                //DriverBirthDate
                if (result.DriverBirthDate != null)
                {
                    ViewBag.DriverBirthDate = string.Format("{0}/{1}/{2}", result.DriverBirthDate.Value.Day.ToString("00", null), result.DriverBirthDate.Value.Month.ToString("00", null), result.DriverBirthDate.Value.Year + 543);
                }

                //เบี้ยร์ประกัน 2 digits
                if (result.Premium != null)
                {
                    ViewBag.Premium = GlobalFunction.NumberFormatInfo(result.Premium.Value.ToString(), 2);
                }

                //check view only
                if (hiddenButton == "Y")
                {
                    ViewBag.hiddenButton = true;
                }
                else
                {
                    ViewBag.hiddenButton = false;
                }

                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: MotorUnderwrite
        public ActionResult MotorUnderwriteDetail(string queueId)
        {
            int? QueueId = null;
            try
            {
                if (queueId != null && queueId != "")
                {
                    byte[] dataQueue = Convert.FromBase64String(queueId);
                    var decodedQueueIdString = Encoding.UTF8.GetString(dataQueue);
                    QueueId = Convert.ToInt32(decodedQueueIdString);
                }
                else
                {
                    return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "InnerException:" + e.InnerException + ", Message:" + e.Message });
            }

            try
            {
                //Get Role
                var user = GlobalFunction.GetLoginDetail(HttpContext);
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                var accessRole = GlobalFunction.GetAccessRole(roleList);
                ViewBag.AccessRole = accessRole;

                var result = _context.usp_QueueMotorFullDetailByQueueId_Select(QueueId, null).FirstOrDefault();
                ViewBag.QueueMotorFullDetail = result;
                ViewBag.QueueId = result.QueueId;
                ViewBag.AppCode = result.ApplicationCode;
                var applicationCode = result.ApplicationCode;
                var result2 = _context.usp_UnderwriteTypeMotor_Select(null).ToList();
                ViewBag.UnderwriteTypeMotor = result2;

                var result3 = _context.usp_UnderwritePaymentTypeMotor_Select(null).ToList();
                ViewBag.UnderwritePaymentTypeMotor = result3;

                var result4 = _context.usp_CallStatusMotor_Select(null).ToList();
                ViewBag.CallStatusListMotor = result4;

                //เช็คประวัติ หมายเลขตัวถัง
                var count1 = _context.usp_QueueMotorUnderwriteHistory_Select(QueueId, 0, null, null, null).ToList().Count();
                ViewBag.CountChassisNumber = count1;

                //เช็คประวัติ ประวัติการชำระเบี้ยให้กับแอปอื่นๆ
                var count2 = _context.usp_QueueMotorPaymentHistory_Select(QueueId, 0, null, null, null).ToList().Count();
                ViewBag.CountPayment = count2;

                var giveType = _contextPH.usp_GiveType_Select(null).ToList();
                ViewBag.giveType = giveType;

                var docstorage = Properties.Settings.Default.docstorage;
                ViewBag.docstorage = docstorage;

                #region Documents


                //new Documents from docStorage
                //var documentResult = _context.usp_DocumentUnderwriteByQueueId_Select(QueueId, 2).ToList();



                //if (documentResult != null)
                //{
                //    ViewBag.documentResult = documentResult;
                //    var documentIdList = documentResult.Select(x => x.DocumentId).ToList();

                //    var docstorageApi = Properties.Settings.Default.docstorageApi;

                //    string apiUrl = docstorageApi;

                //    // Create a RestSharp client
                //    var client = new RestClient(apiUrl);

                //    // Create a request with parameters
                //    var request = new RestRequest(Method.GET);
                //    /* request.AddParameter("documentIds", "value1");
                //     request.AddParameter("param2", "value2");*/
                //    foreach (var item in documentIdList)
                //    {
                //        request.AddParameter("documentIds", item);
                //    }


                //    // Execute the request and get the response
                //    IRestResponse response = client.Execute(request);

                //    // Check if the request was successful (statu s code 200)
                //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                //    {
                //        ViewBag.ApiResponse = response.Content;
                //        var dataResponse = JsonConvert.DeserializeObject<DataResponse>(response.Content);
                //        ViewBag.docFile = dataResponse.data.ToList();
                //        var dataResponseMerge = dataResponse.data.ToList();


                //    }

                //}




                #endregion Documents

                //ปีจดทะเบียน BE
                ViewBag.VehicleRegistrationYear = "-";
                if (!String.IsNullOrEmpty(result.VehicleRegistrationYear))
                {
                    ViewBag.VehicleRegistrationYear = Convert.ToInt32(result.VehicleRegistrationYear) - 543;
                }

                // StartCoverDate BE 2 ตำแหน่ง
                if (result.StartCoverDate != null)
                {
                    ViewBag.StartCoverDate = string.Format("{0}/{1}/{2}", result.StartCoverDate.Value.Day.ToString("00", null), result.StartCoverDate.Value.Month.ToString("00", null), result.StartCoverDate.Value.Year + 543);
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

                //EndCoverDate BE 2 ตำแหน่ง
                if (result.EndCoverDate != null)
                {
                    ViewBag.EndCoverDate = string.Format("{0}/{1}/{2}", result.EndCoverDate.Value.Day.ToString("00", null), result.EndCoverDate.Value.Month.ToString("00", null), result.EndCoverDate.Value.Year + 543);
                }

                //IssuesPolicyDate
                if (result.IssuesPolicyDate != null)
                {
                    ViewBag.IssuesPolicyDate = string.Format("{0}/{1}/{2} {3}", result.IssuesPolicyDate.Value.Day.ToString("00", null), result.IssuesPolicyDate.Value.Month.ToString("00", null), result.IssuesPolicyDate.Value.Year + 543, result.IssuesPolicyDate.Value.ToString("HH:mm:ss"));
                }

                //InsuredBirthDate
                if (result.InsuredBirthDate != null)
                {
                    ViewBag.InsuredBirthDate = string.Format("{0}/{1}/{2}", result.InsuredBirthDate.Value.Day.ToString("00", null), result.InsuredBirthDate.Value.Month.ToString("00", null), result.InsuredBirthDate.Value.Year + 543);
                }

                //PayerBirthDate
                if (result.PayerBirthDate != null)
                {
                    ViewBag.PayerBirthDate = string.Format("{0}/{1}/{2}", result.PayerBirthDate.Value.Day.ToString("00", null), result.PayerBirthDate.Value.Month.ToString("00", null), result.PayerBirthDate.Value.Year + 543);
                }

                //DriverBirthDate
                if (result.DriverBirthDate != null)
                {
                    ViewBag.DriverBirthDate = string.Format("{0}/{1}/{2}", result.DriverBirthDate.Value.Day.ToString("00", null), result.DriverBirthDate.Value.Month.ToString("00", null), result.DriverBirthDate.Value.Year + 543);
                }

                //เบี้ยร์ประกัน 2 digits
                if (result.Premium != null)
                {
                    ViewBag.Premium = GlobalFunction.NumberFormatInfo(result.Premium.Value.ToString(), 2);
                }

                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public class DataResponse
        {
            public List<subDataResponse> data { get; set; }
            public bool isSuccess { get; set; }
            public DateTime serverDateTime { get; set; }

            // Add properties for other keys as needed
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
                //var models = new List<subDataResponse>();
                //foreach (var item in result)
                //{
                //    models.Add(new subDataResponse
                //    {
                //        documentCode = item.documentCode,
                //        documentId = item.documentId,
                //        documentTypeName = item.documentTypeName,
                //        fileCount = item.fileCount,
                //        mainIndex = item.mainIndex,
                //        searchIndex = item.searchIndex
                //    });
                //}
                //return Content(response.Content, "application/json");

                return dataResponse;
            }
            else
            {
                // Handle the error, for example, display an error message
                ViewBag.ErrorMessage = $"Error: {response.StatusCode} - {response.StatusDescription}";
                return null;
            }


        }

        public ActionResult MotorUnderwriteDetailForUnderwriteOverdue(string queueId)
        {
            int? QueueId = null;
            try
            {
                if (queueId != null && queueId != "")
                {
                    byte[] dataQueue = Convert.FromBase64String(queueId);
                    var decodedQueueIdString = Encoding.UTF8.GetString(dataQueue);
                    QueueId = Convert.ToInt32(decodedQueueIdString);
                }
                else
                {
                    return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "InnerException:" + e.InnerException + ", Message:" + e.Message });
            }

            try
            {
                //Get Role
                var user = GlobalFunction.GetLoginDetail(HttpContext);
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                var accessRole = GlobalFunction.GetAccessRole(roleList);
                ViewBag.AccessRole = accessRole;

                var result = _context.usp_QueueMotorFullDetailByQueueId_Select(QueueId, null).FirstOrDefault();
                ViewBag.QueueMotorFullDetail = result;
                ViewBag.QueueId = result.QueueId;
                ViewBag.AppCode = result.ApplicationCode;
                var applicationCode = result.ApplicationCode;
                var result2 = _context.usp_UnderwriteTypeMotor_Select(null).ToList();
                ViewBag.UnderwriteTypeMotor = result2;

                var result3 = _context.usp_UnderwritePaymentTypeMotor_Select(null).ToList();
                ViewBag.UnderwritePaymentTypeMotor = result3;

                var result4 = _context.usp_CallStatusMotor_Select(null).ToList();
                ViewBag.CallStatusListMotor = result4;

                //เช็คประวัติ หมายเลขตัวถัง
                var count1 = _context.usp_QueueMotorUnderwriteHistory_Select(QueueId, 0, null, null, null).ToList().Count();
                ViewBag.CountChassisNumber = count1;

                //เช็คประวัติ ประวัติการชำระเบี้ยให้กับแอปอื่นๆ
                var count2 = _context.usp_QueueMotorPaymentHistory_Select(QueueId, 0, null, null, null).ToList().Count();
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

                //ปีจดทะเบียน BE
                ViewBag.VehicleRegistrationYear = "-";
                if (!String.IsNullOrEmpty(result.VehicleRegistrationYear))
                {
                    ViewBag.VehicleRegistrationYear = Convert.ToInt32(result.VehicleRegistrationYear) - 543;
                }

                // StartCoverDate BE 2 ตำแหน่ง
                if (result.StartCoverDate != null)
                {
                    ViewBag.StartCoverDate = string.Format("{0}/{1}/{2}", result.StartCoverDate.Value.Day.ToString("00", null), result.StartCoverDate.Value.Month.ToString("00", null), result.StartCoverDate.Value.Year + 543);
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

                //EndCoverDate BE 2 ตำแหน่ง
                if (result.EndCoverDate != null)
                {
                    ViewBag.EndCoverDate = string.Format("{0}/{1}/{2}", result.EndCoverDate.Value.Day.ToString("00", null), result.EndCoverDate.Value.Month.ToString("00", null), result.EndCoverDate.Value.Year + 543);
                }

                //IssuesPolicyDate
                if (result.IssuesPolicyDate != null)
                {
                    ViewBag.IssuesPolicyDate = string.Format("{0}/{1}/{2} {3}", result.IssuesPolicyDate.Value.Day.ToString("00", null), result.IssuesPolicyDate.Value.Month.ToString("00", null), result.IssuesPolicyDate.Value.Year + 543, result.IssuesPolicyDate.Value.ToString("HH:mm:ss"));
                }

                //InsuredBirthDate
                if (result.InsuredBirthDate != null)
                {
                    ViewBag.InsuredBirthDate = string.Format("{0}/{1}/{2}", result.InsuredBirthDate.Value.Day.ToString("00", null), result.InsuredBirthDate.Value.Month.ToString("00", null), result.InsuredBirthDate.Value.Year + 543);
                }

                //PayerBirthDate
                if (result.PayerBirthDate != null)
                {
                    ViewBag.PayerBirthDate = string.Format("{0}/{1}/{2}", result.PayerBirthDate.Value.Day.ToString("00", null), result.PayerBirthDate.Value.Month.ToString("00", null), result.PayerBirthDate.Value.Year + 543);
                }

                //DriverBirthDate
                if (result.DriverBirthDate != null)
                {
                    ViewBag.DriverBirthDate = string.Format("{0}/{1}/{2}", result.DriverBirthDate.Value.Day.ToString("00", null), result.DriverBirthDate.Value.Month.ToString("00", null), result.DriverBirthDate.Value.Year + 543);
                }

                //เบี้ยร์ประกัน 2 digits
                if (result.Premium != null)
                {
                    ViewBag.Premium = GlobalFunction.NumberFormatInfo(result.Premium.Value.ToString(), 2);
                }

                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult MotorUnderwriteEditDetail(string queueId)
        {
            int? QueueId = null;
            try
            {
                if (queueId != null && queueId != "")
                {
                    byte[] dataQueue = Convert.FromBase64String(queueId);
                    var decodedQueueIdString = Encoding.UTF8.GetString(dataQueue);
                    QueueId = Convert.ToInt32(decodedQueueIdString);
                }
                else
                {
                    return RedirectToAction("BadRequest", "Error", new { Msg = "Invalid request parameters" });
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("BadRequest", "Error", new { Msg = "InnerException:" + e.InnerException + ", Message:" + e.Message });
            }

            try
            {
                //Get Role
                var user = GlobalFunction.GetLoginDetail(HttpContext);
                var roleListRaw = OAuth2Helper.GetRoles();
                var roleList = roleListRaw.Split(',');
                var accessRole = GlobalFunction.GetAccessRole(roleList);
                ViewBag.AccessRole = accessRole;

                var result = _context.usp_QueueMotorFullDetailByQueueId_Select(QueueId, null).FirstOrDefault();
                ViewBag.QueueMotorFullDetail = result;
                ViewBag.QueueId = result.QueueId;
                ViewBag.AppCode = result.ApplicationCode;
                var CancerCloseDate = Properties.Settings.Default.CancerCloseDate;
                ViewBag.CancerCloseDate = CancerCloseDate;
                var applicationCode = result.ApplicationCode;

                var result2 = _context.usp_UnderwriteTypeMotor_Select(null).ToList();
                ViewBag.UnderwriteTypeMotor = result2;

                var result3 = _context.usp_UnderwritePaymentTypeMotor_Select(null).ToList();
                ViewBag.UnderwritePaymentTypeMotor = result3;

                var result4 = _context.usp_CallStatusMotor_Select(null).ToList();
                ViewBag.CallStatusListMotor = result4;

                //เช็คประวัติ หมายเลขตัวถัง
                var count1 = _context.usp_QueueMotorUnderwriteHistory_Select(QueueId, 0, null, null, null).ToList().Count();
                ViewBag.CountChassisNumber = count1;

                //เช็คประวัติ ประวัติการชำระเบี้ยให้กับแอปอื่นๆ
                var count2 = _context.usp_QueueMotorPaymentHistory_Select(QueueId, 0, null, null, null).ToList().Count();
                ViewBag.CountPayment = count2;

                var giveType = _contextPH.usp_GiveType_Select(null).ToList();
                ViewBag.giveType = giveType;

                var docstorage = Properties.Settings.Default.docstorage;
                ViewBag.docstorage = docstorage;

                #region Documents


                ////new Documents from docStorage
                //var documentResult = _context.usp_DocumentUnderwriteByQueueId_Select(QueueId, 2).ToList();

                //if (documentResult != null)
                //{
                //    ViewBag.documentResult = documentResult;
                //    var documentIdList = documentResult.Select(x => x.DocumentId).ToList();

                //    var docstorageApi = Properties.Settings.Default.docstorageApi;

                //    string apiUrl = docstorageApi;

                //    // Create a RestSharp client
                //    var client = new RestClient(apiUrl);

                //    // Create a request with parameters
                //    var request = new RestRequest(Method.GET);
                //    /* request.AddParameter("documentIds", "value1");
                //     request.AddParameter("param2", "value2");*/
                //    foreach (var item in documentIdList)
                //    {
                //        request.AddParameter("documentIds", item);
                //    }


                //    // Execute the request and get the response
                //    IRestResponse response = client.Execute(request);

                //    // Check if the request was successful (statu s code 200)
                //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                //    {
                //        ViewBag.ApiResponse = response.Content;
                //        var dataResponse = JsonConvert.DeserializeObject<DataResponse>(response.Content);
                //        ViewBag.docFile = dataResponse.data.ToList();
                //        var dataResponseMerge = dataResponse.data.ToList();


                //    }

                //}




                #endregion Documents

                //ปีจดทะเบียน BE
                ViewBag.VehicleRegistrationYear = "-";
                if (!String.IsNullOrEmpty(result.VehicleRegistrationYear))
                {
                    ViewBag.VehicleRegistrationYear = Convert.ToInt32(result.VehicleRegistrationYear) - 543;
                }

                // StartCoverDate BE 2 ตำแหน่ง
                if (result.StartCoverDate != null)
                {
                    ViewBag.StartCoverDate = string.Format("{0}/{1}/{2}", result.StartCoverDate.Value.Day.ToString("00", null), result.StartCoverDate.Value.Month.ToString("00", null), result.StartCoverDate.Value.Year + 543);
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

                //GiveDate
                if (result.GiveDate != null)
                {
                    ViewBag.GiveDate = string.Format("{0}/{1}/{2}", result.GiveDate.Value.Day.ToString("00", null), result.GiveDate.Value.Month.ToString("00", null), result.GiveDate.Value.Year + 543);
                }

                //EndCoverDate BE 2 ตำแหน่ง
                if (result.EndCoverDate != null)
                {
                    ViewBag.EndCoverDate = string.Format("{0}/{1}/{2}", result.EndCoverDate.Value.Day.ToString("00", null), result.EndCoverDate.Value.Month.ToString("00", null), result.EndCoverDate.Value.Year + 543);
                }

                //IssuesPolicyDate
                if (result.IssuesPolicyDate != null)
                {
                    ViewBag.IssuesPolicyDate = string.Format("{0}/{1}/{2} {3}", result.IssuesPolicyDate.Value.Day.ToString("00", null), result.IssuesPolicyDate.Value.Month.ToString("00", null), result.IssuesPolicyDate.Value.Year + 543, result.IssuesPolicyDate.Value.ToString("HH:mm:ss"));
                }

                //InsuredBirthDate
                if (result.InsuredBirthDate != null)
                {
                    ViewBag.InsuredBirthDate = string.Format("{0}/{1}/{2}", result.InsuredBirthDate.Value.Day.ToString("00", null), result.InsuredBirthDate.Value.Month.ToString("00", null), result.InsuredBirthDate.Value.Year + 543);
                }

                //PayerBirthDate
                if (result.PayerBirthDate != null)
                {
                    ViewBag.PayerBirthDate = string.Format("{0}/{1}/{2}", result.PayerBirthDate.Value.Day.ToString("00", null), result.PayerBirthDate.Value.Month.ToString("00", null), result.PayerBirthDate.Value.Year + 543);
                }

                //DriverBirthDate
                if (result.DriverBirthDate != null)
                {
                    ViewBag.DriverBirthDate = string.Format("{0}/{1}/{2}", result.DriverBirthDate.Value.Day.ToString("00", null), result.DriverBirthDate.Value.Month.ToString("00", null), result.DriverBirthDate.Value.Year + 543);
                }

                //เบี้ยร์ประกัน 2 digits
                if (result.Premium != null)
                {
                    ViewBag.Premium = GlobalFunction.NumberFormatInfo(result.Premium.Value.ToString(), 2);
                }

                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult DowloadDocuments(string queueId)
        {
            #region Documents

            var result = _context.usp_QueueMotorFullDetailByQueueId_Select(Convert.ToInt32(queueId), null).FirstOrDefault();
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

        [HttpGet]
        public ActionResult UnderwriteHistory(int queueId, int draw, int? indexStart, int? pageSize, string sortField, string orderType)
        {
            var result = _context.usp_QueueMotorUnderwriteHistory_Select(queueId, indexStart, pageSize, sortField, orderType).ToList();

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
            var result = _context.usp_QueueMotorPaymentHistory_Select(queueId, indexStart, pageSize, sortField, orderType).ToList();

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
        public ActionResult QueueLog(int queueId, int draw, int? indexStart, int? pageSize, string sortField, string orderType, string search)
        {
            var result = _context.usp_QueueMotorLogByQueueId_Select(queueId, indexStart, pageSize, sortField, orderType, search).ToList();

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
        public ActionResult MotorQuestionUpdate(FormCollection form)
        {
            //สถานะการคัดกรอง
            var underwriteStatus = form["motorUnderwriteStatus"];

            if (!string.IsNullOrEmpty(underwriteStatus))
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var queueId = form["hiddenQueueId"];
                //GROUP สถานะผ่านคัดกรอง
                //ช่องทางการคัดกรอง
                var typeSelect = form["typeSelect"];
                //typeSelectChlid split to array list

                var typeSelectChlid = form["typeSelectChild"];
                if (string.IsNullOrEmpty(typeSelectChlid))
                {
                    typeSelectChlid = form["typeSelectChildCall"];
                }
                var txtCause = form["txtCause"];
                //การชำระเบี้ย
                var paymentTypeMotor = form["paymentTypeMotor"];
                //การคัดกรองด้านสุขภาพ
                var motor = form["motor"];
                var motorChildPass = form["motorChildPass"];
                var IsSpecify = form["IsSpecify"];
                var motorChildPassOtherRemark = form["motorChildPassOtherRemark"];
                //motorChildNotPass split to array list
                var motorChildNotPass = form["motorChildNotPass"];
                var motorChildNotpassOtherRemark = form["motorChildNotpassOtherRemark"];
                //หมายเหตุ/บันทึกเพิ่มเติม
                var remark = form["remark"];

                //GROUP สถานะไม่ผ่านคัดกรอง
                //สถานการโทร
                var callStatus = form["callStatus"];
                //สถานะการเข้าพบ
                var foundCustomer = form["foundCustomer"];

                //DECLARE
                int? userId = Convert.ToInt32(user);
                bool? isUnderwrite = null;
                int? underwriteTypeId = null;
                int? queueStatusId = null;
                int? callStatusId = null;
                string callCauseRemark = null;//
                bool? isNotFoundCustomer = null;
                bool? IsUnderwriteInsured = null;
                bool? isUnderwritePayer = null;//
                int? underwritePaymentTypeId = null;
                bool? isUnderwritePass = null;
                bool? isUnderwritePassStandard = null;//
                bool? isUnderwritePassCondition = null;
                bool? isUnderwritePassIsSpecify = null;
                bool? isUnderwritePassIsSpecifyNotComplete = null;
                string underwritePassSpecifyRemark = null;
                bool? isUnderwriteUnPassVehicleSpec = null;//
                bool? IsUnderwriteUnPassVehicleUseType = null;//
                bool? IsUnderwriteUnPassCantPay = null;
                bool? isUnderwriteUnPassCantContact = null;//
                bool? isUnderwriteUnPassOther = null;//
                string underwriteUnPassOtherRemark = null;//
                string remarkAll = null;

                using (var db = new UnderwriteBranchV2MotorModelContainer())
                {
                    switch (underwriteStatus)
                    {
                        // 1 สถานะคัดกรองแล้ว
                        case "1":
                            isUnderwrite = true;
                            underwriteTypeId = Convert.ToInt32(typeSelect); //ช่องทางการคัดกรอง
                            underwritePaymentTypeId = Convert.ToInt32(paymentTypeMotor); //การชำระเบี้ย
                            isUnderwritePass = (motor == "1"); // การคัดกรองด้านสุขภาพ ผ่าน=true,ไม่ผ่าน=false
                            remarkAll = remark; //หมายเหตุ/บันทึกเพิ่มเติม

                            var arrayTypeSelectChild = typeSelectChlid.Split(','); //ตัวเลือกเพิ่มเติม ช่องทางการคัดกรอง (ผู้ชำระเบี้ย,ผู้เอาประกัน)

                            if (isUnderwritePass.Value)
                            {
                                switch (Convert.ToInt32(typeSelect))
                                {
                                    //โทร - ผ่าน
                                    case 3:
                                        queueStatusId = 3; //โทรคัดกรอง มอบบัตรภายหลัง
                                        break;
                                    //เข้าพบ - ผ่าน
                                    case 2:
                                        queueStatusId = 5; //เข้าพบ มอบบัตรแล้ว
                                        break;

                                    default:
                                        break;
                                }

                                //ผ่าน
                                switch (motorChildPass)
                                {
                                    case "1":
                                        isUnderwritePassStandard = true; //ผ่าน ตามมาตรฐานทุกประการ
                                        break;

                                    case "0":
                                        isUnderwritePassCondition = true; //ผ่าน  ติดเงื่อนไข
                                        underwritePassSpecifyRemark = motorChildPassOtherRemark;

                                        if (!string.IsNullOrEmpty(IsSpecify))
                                        {
                                            switch (IsSpecify)
                                            {
                                                case "1": //ระบุในใบสมัครครบถ้วน
                                                    isUnderwritePassIsSpecify = true;
                                                    isUnderwritePassIsSpecifyNotComplete = null;
                                                    break;

                                                case "0"://ไม่ระบุ
                                                    isUnderwritePassIsSpecify = false;
                                                    isUnderwritePassIsSpecifyNotComplete = null;
                                                    break;
                                                case "2"://ระบุในใบสมัครไม่ครบ
                                                    isUnderwritePassIsSpecify = true;
                                                    isUnderwritePassIsSpecifyNotComplete = true;
                                                    break;

                                                default:
                                                    break;
                                            }
                                        }
                                        break;

                                    default:
                                        return Json(new { IsResult = false, Msg = "การคัดกรองด้านสุขภาพ = null" }, JsonRequestBehavior.AllowGet);
                                }

                                //check not null
                                if (isUnderwritePassStandard == null && isUnderwritePassCondition == null)
                                {
                                    return Json(new { IsResult = false, Msg = "isUnderwritePassStandard = null && isUnderwritePassCondition = null" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                queueStatusId = 6; //ไม่ผ่านดัดกรอง
                                var arraymotorChildNotPass = motorChildNotPass.Split(',');
                                //ไม่ผ่าน
                                foreach (var item in arraymotorChildNotPass)
                                {
                                    switch (item)
                                    {
                                        case "denymotor": //สภาพรถยนต์ไม่ตรงตามใบสมัคร
                                            isUnderwriteUnPassVehicleSpec = true;
                                            break;

                                        case "denyOccupation": //ประเภทการใช้งานผิดประเภท
                                            IsUnderwriteUnPassVehicleUseType = true;
                                            break;

                                        case "denyCantPay": //ไม่สามารถชำระเบี้ยตามเงื่อนไขได้
                                            IsUnderwriteUnPassCantPay = true;
                                            break;

                                        case "denyCantContact": //ติดต่อไม่ได้/ไม่รับสาย/ไม่สะดวกคุย
                                            isUnderwriteUnPassCantContact = true;
                                            break;

                                        case "denyOther": //อื่นๆ
                                            isUnderwriteUnPassOther = true;
                                            underwriteUnPassOtherRemark = motorChildNotpassOtherRemark;
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }

                            //type 3 = โทรคัดกรอง
                            if (Convert.ToInt32(typeSelect) == 3)
                            {
                                callStatusId = 2; //สถานะ รับสาย
                                isNotFoundCustomer = null; //โทรเยี่ยมลูกค้า ให้ isNotFoundCustomer = null
                                callCauseRemark = txtCause;
                                foreach (var item in arrayTypeSelectChild)
                                {
                                    switch (item)
                                    {
                                        case "customer":
                                            IsUnderwriteInsured = true;
                                            break;

                                        case "payer":
                                            isUnderwritePayer = true;
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            //type 2= เข้าพบลูกค้า
                            else if (Convert.ToInt32(typeSelect) == 2)
                            {
                                callStatusId = 1; //สถานะ รอข้อมูล
                                isNotFoundCustomer = false; //เข้าพบลูกค้า ให้ isNotFoundCustomer = false

                                foreach (var item in arrayTypeSelectChild)
                                {
                                    switch (item)
                                    {
                                        case "customer":
                                            IsUnderwriteInsured = true;
                                            break;

                                        case "payer":
                                            isUnderwritePayer = true;
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            break;
                        // 0 ยังไม่คัดกรอง
                        case "0":
                            isUnderwrite = false;
                            queueStatusId = 2; //ยังไม่ได้คัดกรอง
                            callStatusId = Convert.ToInt32(callStatus); //สถานะการโทร
                            isNotFoundCustomer = (foundCustomer == "0");
                            break;

                        default:
                            break;
                    }
                    //var result = 1;
                    var result = db.usp_QueueMotorResultByQueueId_Insert(Convert.ToInt32(queueId),
                                                                    queueStatusId,
                                                                    isUnderwrite,
                                                                    isNotFoundCustomer,
                                                                    underwriteTypeId,
                                                                    callStatusId,
                                                                    callCauseRemark,
                                                                    IsUnderwriteInsured,
                                                                    isUnderwritePayer,
                                                                    underwritePaymentTypeId,
                                                                    isUnderwritePass,
                                                                    isUnderwritePassStandard,
                                                                    isUnderwritePassCondition,
                                                                    isUnderwritePassIsSpecify,
                                                                    isUnderwritePassIsSpecifyNotComplete,
                                                                    underwritePassSpecifyRemark,
                                                                    isUnderwriteUnPassVehicleSpec,
                                                                    IsUnderwriteUnPassVehicleUseType,
                                                                    IsUnderwriteUnPassCantPay,
                                                                    isUnderwriteUnPassCantContact,
                                                                    isUnderwriteUnPassOther,
                                                                    underwriteUnPassOtherRemark,
                                                                    remarkAll,
                                                                    userId).FirstOrDefault();

                    //singnal R
                    var chat = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                    chat.Clients.Group("1234").ReceiveGroupNotice("Success");

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //RETURN LIST
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult MotorQueueApproveUpdate(int QueueId,
                                               int ApproveStatusId,
                                               string ApproveRemark,
                                               bool? CMIsApprovePassCondition,
                                               bool? CMResultPassIsSpecify,
                                               bool? CMDenyMotor,
                                               bool? CMDenyOccupation,
                                               bool? CMDenyCantPay,
                                               bool? CMDenyCantContact,
                                               bool? CMDenyOther,
                                               string CMDenyRemark,
                                               string CMResultPassSpecifyRemark)
        {
            var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

            var nCMDenyRemark = (CMDenyOther == true) ? CMDenyRemark : null;
            //UPDATE
            var resultUpdated = _context.usp_QueueMotorApproveByQueueId_Update(QueueId,
                                                                          ApproveStatusId,
                                                                          userId,
                                                                          ApproveRemark,
                                                                          CMIsApprovePassCondition,
                                                                          CMResultPassIsSpecify,
                                                                          CMResultPassSpecifyRemark,
                                                                          CMDenyMotor,
                                                                          CMDenyOccupation,
                                                                          CMDenyCantPay,
                                                                          CMDenyCantContact,
                                                                          CMDenyOther,
                                                                          nCMDenyRemark).FirstOrDefault();

            //Signal R
            var chat = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            chat.Clients.Group("1234").ReceiveGroupNotice("Success");

            return Json(new { IsResult = true }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult QueueMotorPoliciesInsertV2(int queueId, string isUnderwriteInsured, string isUnderwritePayer, string isBeware, string bewareRemark, string remark, string uRLPath, string physicalPath, string fileName, bool? isPolicies, string giveRemark)
        {
            try
            {
                var BewareData = isBeware == "1" ? bewareRemark : "";
                var isUnderwriteInsuredConvert = isUnderwriteInsured == "insure" ? true : false;
                var isUnderwritePayerConvert = isUnderwritePayer == "payer" ? true : false;
                var isBewareConvert = isBeware == "1" ? true : false;
                isPolicies = true;
                var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                //DECLARE
                int? createdByUserId = Convert.ToInt32(user);

                var result = _context.usp_QueueMotorResultByQueueIdV2_Insert(queueId,
                                                                        isUnderwriteInsuredConvert,
                                                                        isUnderwritePayerConvert,
                                                                        isBewareConvert,
                                                                        BewareData,
                                                                        remark,
                                                                        uRLPath,
                                                                        physicalPath,
                                                                        fileName,
                                                                        giveRemark,
                                                                        createdByUserId).FirstOrDefault();
                //Signal R
                var chat = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                chat.Clients.Group("1234").ReceiveGroupNotice("Success");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                //throw new Exception(e.Message, e.InnerException);
                return Json(new { IsResult = false, Msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /*
                [HttpPost]
                public ActionResult QueueMotorPoliciesInsert(int queueId, string giverDate, string uRLPath, string physicalPath, string fileName, string giveRemark, string giveTypeRemark, int giveTypeId)
                {
                    try
                    {
                        DateTime? giverDateConvert = null;
                        if (giverDate != "")
                        {
                            giverDateConvert = Convert.ToDateTime(giverDate);
                        }

                        var UserDetail = OAuth2Helper.GetLoginDetail();
                        //TODO : give type
                        var result = _context.usp_QueueMotorPolicies_Insert(queueId, UserDetail.User_ID, giverDateConvert, giveTypeId, uRLPath, physicalPath, fileName, UserDetail.User_ID, giveRemark, giveTypeRemark).FirstOrDefault();

                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception e)
                    {
                        //throw new Exception(e.Message, e.InnerException);
                        return Json(new { IsResult = false, Msg = e.Message }, JsonRequestBehavior.AllowGet);
                    }
                }*/

        [Authorization(Roles = "Developer,UnderwriteV2_Chairman")]
        public ActionResult MotorDashboardCM()
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext);

            var branchList = _contextPH.usp_BranchByChairmanUserId_Select(user.User_ID).ToList();

            ViewBag.BranchList = branchList;

            var currentBranch = -1;
            if (branchList.Count > 0) currentBranch = branchList[0].Branch_ID;

            ViewBag.CurrentBranchID = currentBranch;

            //DCR
            var changePeriodDay = Properties.Settings.Default.MotorChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, numberOfMonthToShow);
            ViewBag.PeriodList = periodList;

            var currentDCR = Convert.ToDateTime(periodList[0].Value);
            ViewBag.CurrentDCR = currentDCR;

            return View();
        }

        [Authorization(Roles = "Developer,UnderwriteV2_BusinesPromoteTeam")]
        public ActionResult MotoDashboardBPT()
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext);

            //Get Role
            var roleListRaw = OAuth2Helper.GetRoles();
            var roleList = roleListRaw.Split(',');
            var accessRole = GlobalFunction.GetAccessRole(roleList);
            ViewBag.AccessRole = accessRole;

            int? userId = user.User_ID;
            if (roleList.Contains("Developer"))
            {
                //Dev ให้ส่ง null เพื่อดู --สาขาทั้งหมด--
                userId = null;
            }

            //DCR
            var changePeriodDay = Properties.Settings.Default.MotorChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, numberOfMonthToShow);
            ViewBag.PeriodList = periodList;

            var branchList = _contextPH.usp_BranchByBusinessPromoteTeamUserId_Select(userId).ToList();
            ViewBag.BranchList = branchList;

            var currentDCR = Convert.ToDateTime(periodList[0].Value);
            ViewBag.CurrentDCR = currentDCR;
            return View();
        }

        [Authorization(Roles = "Developer,UnderwriteV2_BusinesPromoteTeam")]
        public ActionResult MotorUnderwriteDashboard()
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext);

            //Get Role
            var roleListRaw = OAuth2Helper.GetRoles();
            var roleList = roleListRaw.Split(',');
            var accessRole = GlobalFunction.GetAccessRole(roleList);
            ViewBag.AccessRole = accessRole;

            int? userId = user.User_ID;
            //DCR
            var changePeriodDay = Properties.Settings.Default.MotorChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;
            var periodList = GlobalFunction.MotorGetPeriodList(changePeriodDay, 4);
            ViewBag.PeriodList = periodList;

            var branchList = _contextPH.usp_BranchByBusinessPromoteTeamUserId_Select(null).ToList();
            ViewBag.BranchList = branchList;

            var currentDCR = Convert.ToDateTime(periodList[0].Value);
            ViewBag.CurrentDCR = currentDCR;

            return View();
        }
        [HttpGet]
        public ActionResult GetQueueStatus(string dcr, int? branchId)
        {
            var cDCR = Convert.ToDateTime(dcr);
            //สถานะคิวงานคัดกรอง
            var objQueueStatus = _context.usp_pivotQueueMotorStatusByBranchId_Select(cDCR, branchId, null, null, null).FirstOrDefault();

            if (objQueueStatus == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(objQueueStatus, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetQueueApprove(string dcr, int? branchId)
        {
            var cDCR = Convert.ToDateTime(dcr);
            //พิจารณาคิวงาน
            var objQueueApprove = _context.usp_pivotQueueMotorApproveByBranchId_Select(cDCR, branchId, null, null, null).FirstOrDefault();

            if (objQueueApprove == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

            return Json(objQueueApprove, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetQueueUnderwritePending(string dcr, int? branchId)
        {
            var cDCR = Convert.ToDateTime(dcr);

            //ยังไม่คัดกรองคนในสาขา
            var objQueueUnderwritePending = _context.usp_pivotQueueMotorUnderwritePendingByBranchId_Select(cDCR, branchId).ToList();

            return Json(objQueueUnderwritePending, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetQueueUnderwritePendingOutsider(string dcr, int? branchId, string userList)
        {
            var cDCR = Convert.ToDateTime(dcr);

            //ยังไม่คัดกรองคนนอกสาขา
            var objQueueUnderwritePendingOutsider = _context.usp_pivotQueueMotorUnderwritePendingOutsiderByBranchId_Select(cDCR, branchId, userList).FirstOrDefault();

            if (objQueueUnderwritePendingOutsider == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

            return Json(objQueueUnderwritePendingOutsider, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetQueueStatusAssignToUser(string dcr, int? assignToUser)
        {
            var cDCR = Convert.ToDateTime(dcr);

            //คิวงานคนในสาขา
            var objQueueStatusAssignToUser = _context.usp_pivotQueueMotorStatusByAssignToUserId_Select(cDCR, assignToUser).FirstOrDefault();

            if (objQueueStatusAssignToUser == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

            return Json(objQueueStatusAssignToUser, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetQueueStatusOutsider(string dcr, int? branchId, string userList)
        {
            var cDCR = Convert.ToDateTime(dcr);

            //คิวงานคนนอกสาขา
            var objQueueStatusOutsider = _context.usp_pivotQueueMotorStatusOutsiderByBranchId_Select(cDCR, branchId, userList).FirstOrDefault();

            if (objQueueStatusOutsider == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

            return Json(objQueueStatusOutsider, JsonRequestBehavior.AllowGet);
        }

        /*    [HttpPost]
            [Obsolete]
            public ActionResult FileUpload(HttpPostedFileBase file, FormCollection form)
            {
                try
                {
                    var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                    var redirectUrlPathFile = "";
                    var redirectPhysicalPathFile = "";
                    //QueueID
                    var queueId = form["queueId"];

                    //Queue Created Date
                    var queueCreatedDate = form["createdDate"];

                    //Year - Month
                    var yearMonth = Convert.ToDateTime(queueCreatedDate).ToString("yyyyMM");
                    //path folder
                    string path = AppDomain.CurrentDomain.BaseDirectory + String.Format("FileImg\\Motor\\{0}\\QueueID_{1}", yearMonth, queueId);
                    //get filename
                    //var fileNameX = Path.GetFileName(file.FileName);
                    //var fileName = Regex.Replace(fileNameX, "[^<>.0-9a-zA-Z]+", "");

                    var fileName = String.Format("{0}_{1}.jpg", queueId, DateTime.Now.ToString("yyyyMMddHHmmss"));
                    //check if directory is exist
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    //full path & file name
                    string fullPath = path + '\\' + String.Format("{0}", fileName);

                    //check file
                    if (!Directory.EnumerateFileSystemEntries(path).Any())
                    {
                        ImageJob img = new ImageJob(file, fullPath, new ResizeSettings("width=2000;height=2000;format=jpg;mode=max"))
                        {
                            CreateParentDirectory = true //Auto-create the uploads directory.
                        };
                        img.Build();

                        //get file path
                        var folder = String.Format("FileImg/Motor/{0}/QueueID_{1}/{2}", yearMonth, queueId, fileName);
                        redirectUrlPathFile = Properties.Settings.Default.AbsolutePathImage + folder;
                        redirectPhysicalPathFile = Server.MapPath("~/" + folder);
                    }
                    else
                    {
                        DirectoryInfo di = new DirectoryInfo(path);
                        foreach (FileInfo fi in di.EnumerateFiles())
                        {
                            fi.Delete();
                        }
                        ImageJob img = new ImageJob(file, fullPath, new ResizeSettings("width=2000;height=2000;format=jpg;mode=max"))
                        {
                            CreateParentDirectory = true //Auto-create the uploads directory.
                        };
                        img.Build();

                        //get file path
                        //get file path
                        var folder = String.Format("FileImg/Motor/{0}/QueueID_{1}/{2}", yearMonth, queueId, fileName);
                        redirectUrlPathFile = Properties.Settings.Default.AbsolutePathImage + folder;
                        redirectPhysicalPathFile = Server.MapPath("~/" + folder);
                    }

                    //return
                    return Json(new
                    {
                        id = 123,
                        success = true,
                        response = "File uploaded.",
                        fileImgName = fileName,
                        urlPath = redirectUrlPathFile,
                        physicalPath = redirectPhysicalPathFile
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
                    var yearMonth = Convert.ToDateTime(queueCreatedDate).ToString("yyyyMM");
                    //path folder
                    string path = AppDomain.CurrentDomain.BaseDirectory + String.Format("FileImg\\Motor\\{0}\\QueueID_{1}", yearMonth, queueId);

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
    */


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
                return Json(new
                {
                    id = 123,
                    success = true,
                    response = "File uploaded.",
                    fileImgName = fileName,
                    urlPath = urlPath,
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


        [HttpGet]
        public ActionResult GetMotorPivotQueueStatus(string dcr, int? branchId)
        {
            var cDCR = Convert.ToDateTime(dcr);
            int? Branch_Id = null;
            if (branchId != -1)
            {
                Branch_Id = branchId;
            }
            //สถานะคิวงานคัดกรอง
            var objQueueStatus = _context.usp_pivotQueueMotorStatus_Select(cDCR, null, Branch_Id, null).FirstOrDefault();

            if (objQueueStatus == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(objQueueStatus, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetPivotMotorQueueFollowUp_dt(int draw, int? indexStart, int? pageSize, string sortField, string orderType, string startCoverDate, int? payerBranchId, string searchDetail)
        {
            try
            {
                DateTime? nStartCoverdate = null;
                if (startCoverDate != "-1" && startCoverDate != null)
                {
                    nStartCoverdate = Convert.ToDateTime(startCoverDate);
                }
                int? BranchId = null;
                if (payerBranchId != -1)
                {
                    BranchId = payerBranchId;
                }
                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _context.usp_pivotQueueMotorFollowup_Select(BranchId, nStartCoverdate, (searchDetail == "" ? null : searchDetail.Trim()), indexStart, pageSize, sortField, orderType).ToList();

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

    }
}