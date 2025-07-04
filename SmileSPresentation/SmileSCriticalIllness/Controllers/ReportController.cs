using System;
using System.Collections.Generic;
using System.Globalization;

using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSCriticalIllness.Models;
using SmileSCriticalIllness.Helper;

namespace SmileSCriticalIllness.Controllers
{
    [Authorization]
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult ApplicationReport()
        {
            using (var db = new CriticalIllnessEntities())
            {
                //ViewBag.AppId = appId;
                ViewBag.Branch = db.usp_Branch_Select(null, 0, 999, null, null, null).ToList();
                var UserAuth = GlobalFunction.GetLoginDetail(this.HttpContext);
                ViewBag.UserAuth = UserAuth;

                //UserRolse
                var UserRoles = new SSOService.SSOServiceClient().GetRoleByUserName(UserAuth.UserName);
                if (UserRoles != null)
                {
                    int stridx1 = UserRoles.IndexOf("CriticalIllness_Underwrite");
                    int stridx2 = UserRoles.IndexOf("Developer");

                    if (stridx1 > 0)
                    {
                        ViewBag.IsUserRolesCriticalIllness_Underwrite = "1";
                    }
                    else
                    {
                        ViewBag.IsUserRolesCriticalIllness_Underwrite = "0";
                    }
                    if (stridx2 > 0)
                    {
                        ViewBag.IsUserRolesDeveloper = "1";
                    }
                    else
                    {
                        ViewBag.IsUserRolesDeveloper = "0";
                    }
                    //Response.Write(stridx1 + "<br / >" + stridx2);
                }

                return View();
            }
        }

        //Export Report

        public void ApplicationReport_ExportExcel(int dateTypeId, string dateFrom, string dateTo, int? branchId, string applicationStatusIdList, string employeeCode)
        {
            using (var db = new CriticalIllnessEntities())
            {
                //convert datefrom
                DateTime? dateFromConvert = null;
                if (dateFrom != "")
                {
                    dateFromConvert = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", new CultureInfo("en-Us"));
                }
                //convert dateto
                DateTime? dateToConvert = null;
                if (dateTo != "")
                {
                    dateToConvert = DateTime.ParseExact(dateTo, "dd-MM-yyyy", new CultureInfo("en-Us"));
                }

                //get employeeId
                int? employeeId;
                if (employeeCode == "")
                {
                    employeeId = null;
                }
                else
                {
                    employeeId = new EmployeeService.EmployeeServiceClient().GetEmployeeByEmployeeCode(employeeCode).Employee_ID;
                }

                var lst = db.usp_ReportApplicationFullDetail_Select(dateTypeId, dateFromConvert, dateToConvert, branchId, applicationStatusIdList, employeeId).ToList();

                ExcelManager.ExportToExcel(HttpContext, lst, "Sheet1", "ApplicationReport_" + DateTime.Now);
            }
        }
    }
}