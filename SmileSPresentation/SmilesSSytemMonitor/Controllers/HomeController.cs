using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using RestSharp;
using SmilesSSytemMonitor.Models;

namespace SmilesSSytemMonitor.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var apiUrl = new Uri(Properties.Settings.Default.APIURL);
            var request = WebRequest.Create(apiUrl);
            request.ContentType = "application/json";
            request.Method = "GET";

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var result = JsonConvert.DeserializeObject<List<GetServerResult>>(responseString);
                ViewBag.ServerCount = result.Count;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return View();
        }

        [HttpGet]
        public ActionResult AnotherLink()
        {
            return View("Index");
        }

        [HttpPost]
        public ActionResult SendNotification(string msg)
        {
            //Call Service Send SMS
            var client = new RestClient("http://operation.siamsmile.co.th:9215/api/sms/SendSMSV2");

            //assign value to object params
            var phone = SmilesSSytemMonitor.Properties.Settings.Default.PhoneNotification;

            var param = new { SMSTypeId = 1, Message = msg, PhoneNo = phone, CreatedById = 1 };

            //Add Json Body to Request
            var request = new RestRequest().AddJsonBody(param);

            //Add Header Token
            request.AddHeader("Authorization", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwcm9qZWN0aWQiOjl9.XX7R_Ik8pydv2e04ZvrDew8tlszSDrjvTEYKdgO4t7A");

            //Post Request
            var response = client.Post(request);

            if (response.IsSuccessful)
            {
                var smsResult = JsonConvert.DeserializeObject<SMSResult>(response.Content);
                return Json(new { IsResult = response.IsSuccessful, SMSStatus = smsResult.Status, Msg = String.Format("ResponseStatus {0};SMS Status {1};Detail {2}", response.ResponseStatus, smsResult.Status, smsResult.Detail) }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //update failed
                return Json(new { IsResult = response.IsSuccessful, Msg = String.Format("ResponseStatus {0};StatusCode {1};Description {2}", response.ResponseStatus, response.StatusCode, response.StatusDescription) }, JsonRequestBehavior.AllowGet);
            }
        }

        private class GetServerResult
        {
            public int Id { get; set; }
            public DateTime CreatedDate { get; set; }
            public string ServerName { get; set; }
            public float Disk_C_Free { get; set; }
            public float Disk_D_Free { get; set; }
            public float Disk_E_Free { get; set; }
            public float Disk_F_Free { get; set; }
            public float Disk_C_Total { get; set; }
            public float Disk_D_Total { get; set; }
            public float Disk_E_Total { get; set; }
            public float Disk_F_Total { get; set; }
            public float RAM { get; set; }
            public float CPU { get; set; }
            public float DiskIO { get; set; }
            public DateTime UpdatedDate { get; set; }
        }
    }
}