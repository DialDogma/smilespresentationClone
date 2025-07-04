using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using SmileSMobileWebUI.CommunicateService;
using SmileSMobileWebUI.EmployeeService;
using SmileSMobileWebUI.IdentityService;
using SmileSMobileWebUI.Models;
using SmileSMobileWebUI.SMSService;
using SRVTextToImage;

namespace SmileSMobileWebUI.Controllers
{
    public class UserAccountController : Controller
    {
        #region ResetPassword

        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(CaptchaModel cm)
        {
            using (var client = new EmployeeServiceClient())
            {
                var result = client.GetEmployeeByEmployeeCode(cm.EmpCode);
                TempData["data"] = result;
                if (result.FirstName != null)
                {
                    return RedirectToAction("ResetPasswordSubmitOTP", new { cm.EmpCode });
                }
                else
                {
                    ViewBag.Message = "รหัสพนักงานไม่ถูกต้อง!";
                }
            }

            return View(cm);
        }

        [HttpGet]
        public ActionResult ResetPasswordSubmitOTP(CaptchaModel cm)
        {
            var empCode = Request.QueryString["empCode"];
            ViewBag.empCode = empCode;
            if (string.IsNullOrEmpty(empCode))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "not found querystring(empCode)");
                //throw new HttpException(404, "empCode not found.");
            }

            using (var db = new EmployeeServiceClient())
            {
                var userId = db.GetUserIDByEmpCode(empCode);
                var spResult = db.GetEmployeeProfile(userId);
                ViewBag.Item = spResult.Mobile;
                cm.MobileNo = spResult.Mobile;
                @ViewBag.MobileMask = string.Concat("".PadLeft(7, 'X'), cm.MobileNo.Substring(cm.MobileNo.Length - 3));
                cm.EmpCode = empCode;
            }

            return View(cm);
        }

        [HttpPost]
        public ActionResult ResetPasswordSubmitOTP(CaptchaModel cm, string captchaText)
        {
            using (var db = new EmployeeServiceClient())
            {
                var userId = db.GetUserIDByEmpCode(cm.EmpCode);
                var spResult = db.GetEmployeeProfile(userId);
                cm.MobileNo = spResult.Mobile;
            }
            if (this.Session["CaptchaImageText"].ToString() == captchaText)
            {
                return RedirectToAction("CheckOTP", "UserAccount", new { empCode = cm.EmpCode });
            }
            else
            {
                ViewBag.Message = "Catcha Validation Failed!";
            }
            return View(cm);
        }

        [HttpGet]
        public ActionResult CheckOTP(CaptchaModel cm)
        {
            //------Check EmpCode----------------
            var empCode = Request.QueryString["empCode"];
            cm.EmpCode = empCode;
            if (string.IsNullOrEmpty(empCode))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "not found querystring(empCode)");
                //throw new HttpException(404, "empCode not found.");
            }
            //----------Get Mobile------------------
            using (var db = new EmployeeServiceClient())
            {
                var userId = db.GetUserIDByEmpCode(cm.EmpCode);
                var spResult = db.GetEmployeeProfile(userId);
                cm.MobileNo = spResult.Mobile;
            }

            using (var secureClient = new DataCenterV1Entities())
            {
                try
                {
                    //--------------Get otp service--------------------------------
                    if (cm.RefCode.IsNullOrWhiteSpace())
                    {
                        var otpResult = secureClient.usp_OTPGenerate().FirstOrDefault();
                        if (otpResult != null)
                        {
                            cm.RefCode = otpResult.Reference;
                        }
                        using (var smsClient = new SmsServiceClient())
                        {
                            //call sms service
                            string message = "RefCode:" + @cm.RefCode + " OTP:" + otpResult.OTP + " ExpiredDate:" + otpResult.ExpireDate;
                            var smsResult = smsClient.SendSMS_Outbound(1, -1, 2, message, cm.MobileNo);
                            if (smsResult)
                            {
                                @ViewBag.Message = "ระบบได้ทำการส่ง SMS แล้ว หากไม่ได้รับ กรุณาแจ้งไปยัง Callcenter:025333999";
                            }
                        }
                    }
                    else
                    {
                        @ViewBag.Message = "กรุณากรอก OTP อีกครั้ง";
                    }
                }
                catch (Exception e)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "an error occured : Please Contact Addministrator" + e);
                }
            }

            return View(cm);
        }

        [HttpPost]
        public ActionResult CheckOTP(CaptchaModel cm, FormCollection form)
        {
            var RefCode = form["txtRefCode"];
            var OTPCode = form["txtOTP"];
            using (var securityClient = new DataCenterV1Entities())
            {
                var otpResult = securityClient.usp_OTPCheck(RefCode, OTPCode).FirstOrDefault();
                if (otpResult.result.Value)
                {
                    using (var idclient = new SmileIdentityServiceClient())
                    {
                        idclient.Set_AspNetUsers_ResetPassword(cm.EmpCode, cm.EmpCode);
                    }

                    using (var communicateClient = new CommunicateServiceClient())
                    {
                        //ส่ง SMS แจ้งรหัสชั่วคราว
                        communicateClient.Get_Communicate(2, 4, cm.EmpCode); //2 sms /4 ลืมรหัส
                    }
                    return RedirectToAction("Success", "UserAccount");
                }
                ViewBag.Message = "กรอกรหัส OTP ไม่ถูกต้อง! กรุณาลองใหม่อีกครั้ง";
            }
            return View(cm);
        }

        public ActionResult Success(CaptchaModel cm)
        {
            return View();
        }

        //public JsonResult ResetPasswordOTPRequest()
        //{
        //    //TODO: Get OTP
        //    var getResult = "Test 1234";
        //    return Json(getResult);
        //}

        //[HttpPost]
        //public ActionResult ResetPassword(FormCollection form)
        //{
        //    //TODO: Reset Password
        //    return RedirectToAction("ResetPasswordSuccess");
        //}

        //[HttpGet]
        //public ActionResult ResetPasswordSuccess()
        //{
        //    return View();
        //}

        // This action for get Captcha Image
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")] // This is for output cache false
        public FileResult GetCaptchaImage()
        {
            CaptchaRandomImage CI = new CaptchaRandomImage();
            this.Session["CaptchaImageText"] = CI.GetRandomString(5);// here 5 means I want to get 5 char long captcha
            CI.GenerateImage(this.Session["CaptchaImageText"].ToString(), 300, 75, Color.DarkGray, Color.White);
            MemoryStream stream = new MemoryStream();
            CI.Image.Save(stream, ImageFormat.Png);
            stream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(stream, "image/png");
        }

        #endregion ResetPassword

        #region ErrorPage

        public ActionResult ErrorPage(string url, string appidpk)
        {
            ViewBag.CurrentUrl = url;
            ViewBag.appid = appidpk;
            return View();
        }

        #endregion ErrorPage
    }
}