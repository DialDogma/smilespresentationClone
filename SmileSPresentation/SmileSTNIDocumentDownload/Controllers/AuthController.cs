using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmileSTNIDocumentDownload.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            var iuser = form["username"].Trim();
            var ipassword = form["password"].Trim();

            var cuser = Properties.Settings.Default.username;
            var cpassword = Properties.Settings.Default.password;

            if ((iuser == cuser) && (ipassword == cpassword))
            {
                this.Session["IsLoggedIn"] = iuser;
                return RedirectToAction("Index", "DocumentDownload");
            }
            else
            {
                ViewBag.Error = "Incorrect username or password.";
                return View();
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            this.Session["IsLoggedIn"] = "";
            return RedirectToAction("Login", "Auth");
        }
    }
}