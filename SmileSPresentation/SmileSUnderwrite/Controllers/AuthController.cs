using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmileSUnderwrite.Controllers
{
    public class AuthController:Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username,string password)
        {
            if(username == "" || password == "")
            {
                return View();
            }

            var sssLoginService = new SmileSLoginService.SmileSLoginServiceSoapClient();

            var result = sssLoginService.Login(username,password);

            if(result != null)
            {
                Session["Username"] = result.UserName;
                Session["User_ID"] = result.User_ID;
                Session["Person_ID"] = result.Person_ID;
                Session["Employee_ID"] = result.Employee_ID;
                Session["FirstName"] = result.FirstName;
                Session["LastName"] = result.LastName;
                Session["EmployeePositionDetail"] = result.EmployeePositionDetail;
                Session["EmployeeTeamDetail"] = result.EmployeeTeamDetail;
                Session["BranchDetail"] = result.BranchDetail;
                Session["DepartmentDetail"] = result.DepartmentDetail;
                Session["EmployeeTeam_ID"] = result.EmployeeTeam_ID;
                Session["Branch_ID"] = result.Branch_ID;
                Session["Department_ID"] = result.Department_ID;
                Session["EmployeePosition_ID"] = result.EmployeePosition_ID;
                Session["Role"] = result.Roles;
                Session["FullName"] = result.FirstName + " " + result.LastName;

                return RedirectToAction("QueueCallIndex","QueueCall");
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Login","Auth");
        }

        public ActionResult UnAuthorize()
        {
            return View();
        }
    }
}