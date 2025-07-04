using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSSurvey.Models;
using SmileSSurvey.Helper;
using System.Web.Http;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;
using System.Text;


using Serilog;

namespace SmileSSurvey.API
{
    public class SendSurveyController : ApiController
    {
        private readonly SurveyEntities _context;

        public SendSurveyController() => _context = new SurveyEntities();

        //Add Authorize
        //[Authorize]
        [HttpPost]
        [Route("sendsurvey")]
        public IHttpActionResult SendSurvey([FromBody] SendSurveyDTO send)
        {
            send.sendDate = DateTime.Now;
            // concat ค่า reference จาก serveydto ขั้นด้วยลูกน้ำ
            var refList = string.Join(",", send.reference.Select(_ => _.refId).ToArray());

            try
            {
                //ส่ง data survey  ไปที่ store   * call store
                var result = _context.usp_Survey_Insert(send.formId, send.sFCaseId, send.customerId, send.phoneNo, send.employeeCode, refList, send.sendBy, send.sendDate, send.isSendNow).FirstOrDefault();

                //ถัา return  ผลกลับมาเป็นtrue
                if (result.IsResult == true && send.isSendNow == true)
                {
                    //ถ้ัา data survey ให้ส่ง sms 
                    //ส่ง data ข้อความไปsms api 
                   var sms = GlobalFunction.SendSMS(result.Msg, result.SendPhoneNo, 25, send.sendBy);
                    //ถ้า return กลับมา OK
                    var surveyId = Int32.Parse(result.Result);
                    if (sms.Status == "000")
                    {
                        //todo update ผลการส่ง sms 
                        
                        var smsId = Int32.Parse(sms.SmsId);
                        _context.usp_SurveySMS_Update(surveyId, smsId, null);

                    }
                    else
                    {
                        //update ผลการส่ง sms fail
                        _context.usp_SurveySMS_Update(surveyId, null, sms.Detail);
                        result.IsResult = false;
                        result.Result = "Failure";
                        result.Msg = sms.Detail;
                    }


                }

                return Json(result, GlobalObject.carmelSetting());


            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        //Add Authorize
        //[Authorize]
        [HttpPost]
        [Route("sendsurveyclaim")]
        public IHttpActionResult SendSurveyclaim([FromBody] SendSurveyDTO send)
        {
            Log.Information("Start [SendSurveyclaim] at {DateTime} [Input] : {@send}", DateTime.Now, send);
            var refList = "";
            for (int i = 0; i < send.reference.Count; i++)
            {
                refList += i == 0 ? "" : ",";
                refList += send.reference[i].refId;
            }
            try
            {
                Log.Information("[SendSurveyclaim] - [Store procedure]: usp_Survey_InsertV2 [Param] : {@send}", send);
                var result = _context.usp_Survey_InsertV2(send.formId, send.sFCaseId, send.customerId, send.phoneNo, send.employeeCode, refList, send.sendBy, send.sendDate, send.isSendNow).FirstOrDefault();
                Log.Debug("[SendSurveyclaim] - usp_Survey_InsertV2 [Result]: {@result}", result);

                return Json(result, GlobalObject.carmelSetting());
            }
            catch (Exception e)
            {
                Log.Error(e, "[SendSurveyclaim] Error at {DateTime} [Message] : {Message} ", DateTime.Now, e.Message);
                return Json(e.Message);
            }
        }
    }
}