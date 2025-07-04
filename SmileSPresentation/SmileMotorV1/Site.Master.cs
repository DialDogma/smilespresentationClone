using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Claims;
using System.Security.Principal;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;
using SmileMotorV1.Models;
using SmileSMotorClassLibrary;

namespace SmileMotorV1
{
    public partial class Site : System.Web.UI.MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }
            Page.PreLoad += master_Page_Preload;
        }

        protected void master_Page_Preload(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //ManageDataMenu.Visible = false;
            ApplicationMenu.Visible = false;
            MotorMenu.Visible = false;
            ReportMenu.Visible = false;
            EndorseMenu.Visible = false;
            //ReportMenuForUser.Visible = false;

            listStatus8.Visible = false;
            listNoticeEndCover.Visible = false;
            listNoticePolicy.Visible = false;
            listNoticeCashReceive.Visible = false;
            listNoticeCashReceiveSuccess.Visible = false;
            listReverse.Visible = false;

            //ROLE DEVELOPER
            if (HttpContext.Current.User.IsInRole("MotorDeveloper"))
            {
                //ManageDataMenu.Visible = true;
                ApplicationMenu.Visible = true;
                MotorMenu.Visible = true;
                ReportMenu.Visible = true;
                EndorseMenu.Visible = true;
                EndorseManual.Visible = true;

                listStatus8.Visible = true;
                listNoticeEndCover.Visible = true;
                listNoticePolicy.Visible = true;
                listNoticeCashReceive.Visible = true;
                listNoticeCashReceiveSuccess.Visible = true;
                listReverse.Visible = true;

                var db = new MotorV1Entities();
                var resultStatus8 = db.usp_MotorApplication_CountStatus_Select(null, 8).FirstOrDefault();
                var resultStatus4 = db.usp_MotorApplication_CountStatus_Select(null, 4).FirstOrDefault();
                var resultStatus10 = db.usp_MotorApplication_Report_StatusNotice_Select(null, null, 10).Count();
                //description argument 2 = สเตตัสอนุมัติ,2งวดรายเดือน,3=งวดชำระรายปี,7=product ชั้น 3 + พิเศษ,null=สาขา(ทั้งหมด),60=จำนวนวัน
                var resultNoticeEndCover = db.usp_MotorApplication_Report_EndCoverNotice_Select(2, null, 7, null, 30).Count();
                var resultPolicy = db.usp_MotorApplication_Report_DocumentNotice_Select(null).Count();
                var resultNewappNotice = db.usp_MotorApplication_Report_NewAppNotice_Select(null).Count();
                var resultCashReceiveNotice = db.usp_MotorApplication_Report_NewAppMatchedNotice_Select(null).Count();
                var sum = resultStatus4.CountStatus + resultStatus8.CountStatus + resultPolicy + resultNewappNotice + resultStatus10 + resultCashReceiveNotice;

                lblNotification0.Text = resultNoticeEndCover.ToString();
                lblNotification.Text = sum.ToString();

                numberNotice.Text = resultStatus4.CountStatus.ToString();
                detailNotice.Text = "รอการอนุมัติ";
                detailNotice.CssClass = "label label-info";

                numberNotice2.Text = resultStatus8.CountStatus.ToString();
                detailNotice2.Text = "แก้ไขรอการอนุมัติ";
                detailNotice2.CssClass = "label label-primary";

                numberNotice3.Text = resultNoticeEndCover.ToString();
                detailNotice3.Text = "App ใกล้หมดอายุ";
                detailNotice3.CssClass = "label label-danger";

                numberNotice4.Text = resultPolicy.ToString();
                detailNotice4.Text = "เอกสารกรมธรรม์";
                detailNotice4.CssClass = "label label-success";

                numberNotice5.Text = resultNewappNotice.ToString();
                detailNotice5.Text = "รอชำระเบี้ย";
                detailNotice5.CssClass = "label label-warning";

                numberNotice6.Text = resultStatus10.ToString();
                detailNotice6.Text = "รอคืนสภาพ";
                detailNotice6.CssClass = "label label-warning";

                numberNotice7.Text = resultCashReceiveNotice.ToString();
                detailNotice7.Text = "ชำระเบี้ย";
                detailNotice7.CssClass = "label label-success";
            }
            //ROLE UNDERWRITE
            if (HttpContext.Current.User.IsInRole("MotorUnderwrite"))
            {
                //ManageDataMenu.Visible = true;
                ApplicationMenu.Visible = true;
                MotorMenu.Visible = true;
                ReportMenu.Visible = true;
                EndorseMenu.Visible = true;
                EndorseManual.Visible = true;

                listStatus8.Visible = true;
                listNoticeEndCover.Visible = true;
                //listNoticePolicy.Visible = true;
                listReverse.Visible = true;
                listNoticeCashReceive.Visible = true;
                listNoticeCashReceiveSuccess.Visible = true;

                var db = new MotorV1Entities();
                var resultStatus8 = db.usp_MotorApplication_CountStatus_Select(null, 8).FirstOrDefault();
                var resultStatus4 = db.usp_MotorApplication_CountStatus_Select(null, 4).FirstOrDefault();
                var resultStatus10 = db.usp_MotorApplication_Report_StatusNotice_Select(null, null, 10).Count();
                //description argument 2 = สเตตัสอนุมัติ,2งวดรายเดือน,3=งวดชำระรายปี,7=proกuct ชั้น 3 + พิเศษ,null=สาขา(ทั้งหมด),60=จำนวนวัน
                var resultNoticeEndCover = db.usp_MotorApplication_Report_EndCoverNotice_Select(2, null, 7, null, 60).Count();
                //var resultPolicy = db.usp_MotorApplication_Report_DocumentNotice_Select(null).Count();
                var resultNewappNotice = db.usp_MotorApplication_Report_NewAppNotice_Select(null).Count();
                var resultCashReceiveNotice = db.usp_MotorApplication_Report_NewAppMatchedNotice_Select(null).Count();
                var sum = resultStatus4.CountStatus + resultStatus8.CountStatus + resultNewappNotice + resultStatus10 + resultCashReceiveNotice;

                lblNotification0.Text = resultNoticeEndCover.ToString();
                lblNotification.Text = sum.ToString();

                numberNotice.Text = resultStatus4.CountStatus.ToString();
                detailNotice.Text = "รอการอนุมัติ";
                detailNotice.CssClass = "label label-info";

                numberNotice2.Text = resultStatus8.CountStatus.ToString();
                detailNotice2.Text = "แก้ไขรอการอนุมัติ";
                detailNotice2.CssClass = "label label-primary";

                numberNotice3.Text = resultNoticeEndCover.ToString();
                detailNotice3.Text = "App ใกล้หมดอายุ";
                detailNotice3.CssClass = "label label-danger";

                //numberNotice4.Text = resultPolicy.ToString();
                //detailNotice4.Text = "เอกสารกรมธรรม์";
                //detailNotice4.CssClass = "label label-success";

                numberNotice5.Text = resultNewappNotice.ToString();
                detailNotice5.Text = "รอชำระเบี้ย";
                detailNotice5.CssClass = "label label-warning";

                numberNotice6.Text = resultStatus10.ToString();
                detailNotice6.Text = "รอคืนสภาพ";
                detailNotice6.CssClass = "label label-warning";

                numberNotice7.Text = resultCashReceiveNotice.ToString();
                detailNotice7.Text = "ชำระเบี้ย";
                detailNotice7.CssClass = "label label-success";
            }
            //ROLE USER
            if (HttpContext.Current.User.IsInRole("MotorUser"))
            {
                ApplicationMenu.Visible = true;
                listNoticeEndCover.Visible = true;
                listNoticePolicy.Visible = true;
                listNoticeCashReceive.Visible = true;
                listNoticeCashReceiveSuccess.Visible = true;
                //ReportMenuForUser.Visible = true;

                var db = new MotorV1Entities();
                var resultStatus6 = db.usp_MotorApplication_CountStatus_Select(cFunction.GetCurrentBranchID(Page), 6).FirstOrDefault();
                //description argument 2 = สเตตัสอนุมัติ,2งวดรายเดือน,3=งวดชำระรายปี,7=proกuct ชั้น 3 + พิเศษ,null=สาขา(ทั้งหมด),60=จำนวนวัน
                var resultNoticeEndCover = db.usp_MotorApplication_Report_EndCoverNotice_Select(2, null, 7, cFunction.GetCurrentBranchID(Page), 60).Count();
                var resultPolicy = db.usp_MotorApplication_Report_DocumentNotice_Select(cFunction.GetCurrentBranchID(Page)).Count();
                var resultNewappNotice = db.usp_MotorApplication_Report_NewAppNotice_Select(cFunction.GetCurrentBranchID(Page)).Count();
                var resultCashReceiveNotice = db.usp_MotorApplication_Report_NewAppMatchedNotice_Select(cFunction.GetCurrentBranchID(Page)).Count();
                var sum = resultStatus6.CountStatus + resultPolicy + resultNewappNotice + resultCashReceiveNotice;

                lblNotification0.Text = resultNoticeEndCover.ToString();
                lblNotification.Text = sum.ToString();
                numberNotice.Text = resultStatus6.CountStatus.ToString();
                detailNotice.Text = "รอการแก้ไข";
                detailNotice.CssClass = "label label-warning";

                numberNotice3.Text = resultNoticeEndCover.ToString();
                detailNotice3.Text = "App ใกล้หมดอายุ";
                detailNotice3.CssClass = "label label-danger";

                numberNotice4.Text = resultPolicy.ToString();
                detailNotice4.Text = "เอกสารกรมธรรม์";
                detailNotice4.CssClass = "label label-success";

                numberNotice5.Text = resultNewappNotice.ToString();
                detailNotice5.Text = "รอชำระเบี้ย";
                detailNotice5.CssClass = "label label-warning";

                numberNotice7.Text = resultCashReceiveNotice.ToString();
                detailNotice7.Text = "ชำระเบี้ย";
                detailNotice7.CssClass = "label label-success";
            }
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut();
        }
    }
}