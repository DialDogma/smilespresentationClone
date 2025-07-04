using SmileSLogin.Helper;
using SmileSLogin.Models;
using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Linq;
using Microsoft.Owin;
using AuthorizationServer.Contacts;
using System.Web;

namespace SmileSLogin.Controllers
{
    public class AuthController:Controller
    {
        private string tokenName = "";

        //ctor
        public AuthController()
        {
            tokenName = Properties.Settings.Default.TokenName;
        }

        //dispose
        protected override void Dispose(bool disposing)
        {
        }

        // GET: Auth
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            var url = Request.QueryString["url"];
            //Check if already has token return url with token
            //Get Cookie
            var tokenName = Properties.Settings.Default.TokenName;
            var c = this.HttpContext.Request.Cookies[tokenName];

            //If Has Cookie
            if(c != null)
            {
                //If cookie validate return to requested url with token name
                var isKeyValidate = new Services.SSOService().ValidateKey(c.Value);
                if(isKeyValidate)
                {

                    if(!string.IsNullOrEmpty(url))
                    {
                        url = AddQueryStringByKey(url, "token", c.Value);
                        return Redirect(url);
                    } else
                    {
                        //Login success without return url
                        return RedirectToAction("Index", "Home");
                    }
                    
                }
                else //key not validate
                {
                    //clear cookie
                    Response.Cookies[tokenName].Expires = DateTime.Now.AddDays(-1);

                    //return to login page
                    ViewBag.ReturnURL = url;
                    return View();
                }
            }
            else
            {
                //return to login page
                ViewBag.ReturnURL = url;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Login(string username,string password,string ReturnURL)
        {
            //Decode encoded URL
            ReturnURL = Server.UrlDecode(ReturnURL);

            if(username == "" || password == "")
            {
                ReturnURL = Server.UrlEncode(ReturnURL);
                return RedirectToAction("Login",new { ErrorText = "1",url = ReturnURL });
            }

            //Login via login service
            var result = AuthMethod.Login(username,password);

            if(result != null)
            {
                var t = new TokenManager().CreateToken(result,HttpContext);

                if(string.IsNullOrEmpty(ReturnURL))
                {
                    //Login success without return url
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    //Login success with Return URL with token
                    var token = this.HttpContext.Request.Cookies[tokenName];

                    // Remove Query String
                    ReturnURL = AddQueryStringByKey(ReturnURL, "token", token.Value);
                    
                    return Redirect(ReturnURL);
                }
            }
            else
            {
                ReturnURL = Server.UrlEncode(ReturnURL);
                return RedirectToAction("Login",new { ErrorText = "1",url = ReturnURL });
            }
        }


        private string RemoveQueryStringByKey(string url, string key)
        {
            var uri = new Uri(url);
            var newQueryString = HttpUtility.ParseQueryString(uri.Query);
            newQueryString.Remove(key);
            var pagePathWithoutQueryString = uri.GetLeftPart(UriPartial.Path);
            return (newQueryString.Count > 0) ? string.Format("{0}?{1}", pagePathWithoutQueryString, newQueryString) : pagePathWithoutQueryString;
        }
        private string AddQueryStringByKey(string url, string key, string value)
        {
            var uri = new Uri(url);
            var newQueryString = HttpUtility.ParseQueryString(uri.Query);
            newQueryString.Remove(key);
            newQueryString.Add(key, value);
            var pagePathWithoutQueryString = uri.GetLeftPart(UriPartial.Path);
            return (newQueryString.Count > 0) ? string.Format("{0}?{1}", pagePathWithoutQueryString, newQueryString) : pagePathWithoutQueryString;
        }

        public ActionResult Logout()
        {
            //get return url
            var reurnURL = Request.QueryString["url"];

            if (Request.Cookies[tokenName] != null)
            {
                //Delete database token
                var db = new Models.DataCenterV1Entities();
                db.usp_DeleteToken(Request.Cookies[tokenName].Value);
                db.Dispose();

                //clear cookie
                Response.Cookies[tokenName].Expires = DateTime.Now.AddDays(-1);
            }


            return RedirectToAction("Login","Auth",new { url = reurnURL });
        }

        /// <summary>
        /// Logout All Defices
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOutAll()
        {
            //Clear Cookie
            if(Request.Cookies[tokenName] != null)
            {
                //Get user name
                var username = GlobalFunction.GetLoginDetail(this.HttpContext).UserName;

                //Delete Token in database
                var db = new Models.DataCenterV1Entities();
                db.usp_DeleteToken_ByUserName(username);
                db.Dispose();

                //clear cookie
                Response.Cookies[tokenName].Expires = DateTime.Now.AddDays(-1);
            }

            return RedirectToAction("Index","Home");
        }

        public ActionResult ChangePassword()
        {
            var reurnURL = Request.QueryString["url"];
            ViewBag.url = reurnURL;
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(System.Web.Mvc.FormCollection form)
        {
            //check username
            var username = form["Username"];

            //Check old password
            var oldPassword = form["OldPassword"];
            var newPassword = form["NewPassword"];
            var returnURL = form["ReturnURL"];
            var tryLogin = AuthMethod.Login(username,oldPassword);

            if (tryLogin != null)
            {
                //old password correct
                //Reset Password SmileS
                if (!AuthMethod.ChangePassword(username, newPassword))
                {
                    ViewBag.ErrorText = "เกิดข้อผิดพลาด ไม่สามารถเปลี่ยนรหัสผ่าน SmileS ได้";
                    return View(new { url = returnURL });
                }

                ////Reset Password SSS
                //if(!AuthMethod.ChangePassword_SSS(username,newPassword))
                //{
                //    ViewBag.ErrorText = "เกิดข้อผิดพลาด ไม่สามารถเปลี่ยนรหัสผ่าน SSS ได้";
                //    return View(new { url = returnURL });
                //}

                ////Reset Password Auth Server
                //if (!AuthMethod.ChangePassword_AuthServer(username, newPassword))
                //{
                //    ViewBag.ErrorText = "เกิดข้อผิดพลาด ไม่สามารถเปลี่ยนรหัสผ่าน AuthServer ได้";
                //    return View(new { url = returnURL });
                //}

                return RedirectToAction("ChangePasswordSuccess", "Auth", new { url = returnURL });
            }
            else
            {
                //old password incorrect!
                ViewBag.ErrorText = "รหัสผ่านเดิมไม่ถูกต้อง";
            }

            ViewBag.url = returnURL;
            return View(new { url = returnURL });
        }

        public ActionResult ChangePasswordSuccess()
        {
            var reurnURL = Request.QueryString["url"];
            ViewBag.url = Server.UrlEncode(reurnURL);
            return View();
        }
    }
}