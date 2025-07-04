using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

namespace SmileSMobileWebUI.Controllers
{
    public class PortalsController : Controller
    {
        // GET: Portals
        public void Link()
        {
            var serviceNo = Request.QueryString["service"];
            var appIDPK = Request.QueryString["AppIDPK"];
            var ClaimCode = Request.QueryString["ClaimCode"];
            var FeedBackID = Request.QueryString["FeedBackID"];
            var EmpCode = Request.QueryString["EmpCode"];
            var CurrentUrl = "";
            if (Request.Url != null)
            {
                //CurrentUrl = Request.Url.PathAndQuery;
                CurrentUrl = Regex.Replace(Request.Url.AbsoluteUri, "[&]", "_"); ;
                //CurrentUrl = Request.Url.AbsoluteUri;
            }

            switch (serviceNo)
            {
                //Module Application
                case "0101": //Application Detail (PA30/PH/PL)
                    Response.Redirect("http://uat.siamsmile.co.th:9112/application/application?appid=" + appIDPK);

                    break;

                case "0102": //Application Editor (PA30/PH/PL)
                    Response.Redirect("http://uat.siamsmile.co.th:9112/application/application?appid=" + appIDPK);

                    break;

                case "0103": //Application Detail (House)
                    Response.Redirect("http://uat.siamsmile.co.th:9112/ApplicationHouse/Application?appid=" + appIDPK);
                    break;

                case "0104"://Application Editor (House)
                    Response.Redirect("http://uat.siamsmile.co.th:9112/ApplicationHouse/Application?appid=" + appIDPK);
                    break;

                case "0105"://Application Detail (PA Community)
                    Response.Redirect("http://uat.siamsmile.co.th:9112/community/addpacommunity?appid=" + appIDPK);

                    break;

                case "0106"://Application Editor (PA Community)
                    Response.Redirect("http://uat.siamsmile.co.th:9112/community/addpacommunity?appid=" + appIDPK);

                    break;

                //Module Claim
                case "0201": //Claim Detail
                    Response.Redirect("http://uat.siamsmile.co.th:9112/application/application?appid=" + ClaimCode);

                    break;

                case "0202": //Claim Editor
                    Response.Redirect("http://uat.siamsmile.co.th:9112/application/application?appid=" + ClaimCode);

                    break;

                case "0203": //Claim Feedback
                    Response.Redirect("http://uat.siamsmile.co.th:9112/application/application?appid=" + FeedBackID);

                    break;

                case "0204": // Claim Feedback Total
                    Response.Redirect("http://uat.siamsmile.co.th:9112/Claim/ShowClaimFeedback?empcode=" + EmpCode);

                    break;

                case "0205": // Claim Benefit
                    Response.Redirect("http://uat.siamsmile.co.th:9112/application/application?appid=" + appIDPK);

                    break;

                //Module Premium
                //case "0301": // Premium Detail
                //    //Response.Redirect("http://uat.siamsmile.co.th:9112/application/application?appid=" + appIDPK);

                //    break;

                //case "0302": //Premium Editor
                //    //Response.Redirect("http://uat.siamsmile.co.th:9112/application/application?appid=" + appIDPK);

                //    break;

                ////Module Prospect
                //case "0401": //Prospect Detail
                //    //Response.Redirect("http://uat.siamsmile.co.th:9112/application/application?appid=" + appIDPK);

                //    break;

                //case "0402": // Prospect Editor
                //    //Response.Redirect("http://uat.siamsmile.co.th:9112/application/application?appid=" + appIDPK);

                //    break;

                //Module Rank
                case "0501": // Rank Detail
                    Response.Redirect("http://uat.siamsmile.co.th:9112/Parameters/rankV2?empId=" + EmpCode);

                    break;

                //Module Statistic
                case "0601": // Statistic Detail
                    Response.Redirect("http://uat.siamsmile.co.th:9112/Parameters/chartV2?empId=" + EmpCode);

                    break;

                //Module Main/ผลงาน
                case "0701": //Main Page
                    Response.Redirect("http://uat.siamsmile.co.th:9112/FirstPage/indexV2?empID=" + EmpCode);

                    break;

                //Module CounterService
                //case "0801": //CounterService
                //    //Response.Redirect("http://uat.siamsmile.co.th:9112/application/application?appid=" + appIDPK);

                //    break;

                ////Module Profile Edit
                case "0901": // Profile Editor
                    Response.Redirect("http://uat.siamsmile.co.th:9198/Modules/document/frmEmployeeProfile.aspx?empCode=" + EmpCode);
                    //Response.Redirect("http://49.231.178.252:81/SSSDoc/Modules/document/frmEmployeeProfile.aspx?empCode=" + EmpCode);//ProductionSSSDoce

                    break;

                ////Module Media Product
                //case "1001": // Media And Product Detail
                //    //Response.Redirect("http://uat.siamsmile.co.th:9112/media/index);

                //    break;

                //Module ResetPassword
                case "1101": // Reset Password Form
                    Response.Redirect("http://uat.siamsmile.co.th:9112/UserAccount/ResetPassword");

                    break;

                default:
                    Response.Redirect("http://uat.siamsmile.co.th:9112/UserAccount/ErrorPage?url=" + CurrentUrl);
                    break;
            };
        }
    }
}