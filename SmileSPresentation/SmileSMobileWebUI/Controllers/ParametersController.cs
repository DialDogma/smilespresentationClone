using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using SmileSMobileWebUI.EmployeeService;

namespace SmileSMobileWebUI.Controllers
{
    public class ParametersController : Controller
    {
        // GET: Parameters
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Rank(string empId)
        {
            //var CurrentUrl = "";
            //if (Request.Url != null)
            //{
            //    CurrentUrl = Regex.Replace(Request.Url.AbsoluteUri, "[&]", "_"); ;
            //}
            //ViewBag.CurrentUrl = CurrentUrl;
            //ViewBag.EmployeeId = employeeId;
            using (var EmployeeService = new EmployeeServiceClient())
            {
                var result = EmployeeService.GetEmployeeByEmployeeCode(empId);
                ViewBag.TeamID = result.EmployeeTeam_Code;
                ViewBag.BranchID = result.Branch_Code;
                ViewBag.APIUrlParameters = Properties.Settings.Default["APIUrlParamerters"].ToString();
            }

            return View();
        }

        public ActionResult RankV2(string empId)
        {
            using (var EmployeeService = new EmployeeServiceClient())
            {
                var result = EmployeeService.GetEmployeeByEmployeeCode(empId);
                ViewBag.TeamID = result.EmployeeTeam_Code;
                ViewBag.BranchID = result.Branch_Code;
                ViewBag.APIUrlParameters = Properties.Settings.Default["APIUrlParamerters"].ToString();
            }

            return View();
        }

        public ActionResult Chart(string empId)
        {
            using (var EmployeeService = new EmployeeServiceClient())
            {
                var result = EmployeeService.GetEmployeeByEmployeeCode(empId);
                ViewBag.TeamID = result.EmployeeTeam_Code;
                ViewBag.BranchID = result.Branch_Code;
                ViewBag.EmpCode = empId;
                ViewBag.APIUrlParameters2 = Properties.Settings.Default["APIUrlParamerters"].ToString();
            }

            return View();
        }

        public ActionResult ChartV2(string empId)
        {
            using (var EmployeeService = new EmployeeServiceClient())
            {
                var result = EmployeeService.GetEmployeeByEmployeeCode(empId);
                ViewBag.TeamID = result.EmployeeTeam_Code;
                ViewBag.BranchID = result.Branch_Code;
                ViewBag.EmpCode = empId;
                ViewBag.APIUrlParameters2 = Properties.Settings.Default["APIUrlParamerters"].ToString();
            }

            return View();
        }
    }
}