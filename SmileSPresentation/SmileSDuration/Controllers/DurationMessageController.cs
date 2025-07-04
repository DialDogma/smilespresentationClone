using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using SmileSDuration.Models;

namespace SmileSDuration.Controllers
{
    public class DurationMessageController : Controller
    {
        #region Declare

        //**start set context
        private readonly DurationV1Entities _context;

        private readonly CommunicateV1Entities1 _contextCommunicate;

        public DurationMessageController()
        {
            _context = new DurationV1Entities();
            _contextCommunicate = new CommunicateV1Entities1();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            _contextCommunicate.Dispose();
        }

        //**end set context

        #endregion Declare

        #region View

        // GET: DurationMessage
        public ActionResult Index()
        {
            //Get Type
            ViewBag.ComunicateType = _context.usp_ComunicateType_Select(null).ToList();

            return View();
        }

        #endregion View

        #region Method

        public ActionResult SubmitToSendSMS(int tempHeaderId)
        {
            //TODO:
            var tempImportHeaderID = tempHeaderId;
            var type = 0;

            //TODO :INSERT TEMP IMPORT HEADER
            var list = _context.usp_TempImportHeader_Insert(tempImportHeaderID);
            //TODO :
            var listDuration = _context.Comunicate.Where(x => x.TempImportHeaderID == tempImportHeaderID).ToList();

            //TODO: Select Data By HeaderID
            var lstData = new List<SMSQueueHeaderDetail>();
            var lstComunicate = _context.usp_ComunicateByTempImportHeaderID_Select(tempImportHeaderID).ToList();

            if (lstComunicate.Count > 0)
            {
                foreach (var item in lstComunicate)
                {
                    lstData.Add(new SMSQueueHeaderDetail { PhoneNo = item.MobilePhoneNo, Message = item.Message + " " + item.URL });
                }
            }

            var smsDetail = new SMSQueueHeaderDetailViewModel
            {
                ProjectId = 3,
                SMSTypeId = type,
                Remark = "Send From SmileSDuration",
                Total = lstComunicate.Count,
                SendDate = "",
                Data = lstData.ToArray()
            };

            var apiUrl = new Uri(string.Format("http://operation.siamsmile.co.th:9215/api/sms/SendSMSList"));
            var authToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwcm9qZWN0aWQiOjl9.XX7R_Ik8pydv2e04ZvrDew8tlszSDrjvTEYKdgO4t7A";
            var jsonData = JsonConvert.SerializeObject(smsDetail);
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
                var result = JsonConvert.DeserializeObject<SMSResult>(responseString);

                //TODO: UPDATE TransactionID
                if (listDuration.Count > 0)
                {
                    listDuration.ForEach(o =>
                    {
                        o.TransactionID = Convert.ToInt32(result.referenceHeaderID);
                    });
                }
                _context.SaveChanges();
                _context.Dispose();

                //RETURN
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (WebException)
            {
                return null;
            }
        }

        #endregion Method
    }
}