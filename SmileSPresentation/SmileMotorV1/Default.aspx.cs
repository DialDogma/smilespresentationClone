using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;
using System.Globalization;

namespace SmileMotorV1
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //TextBox1.SkinID = "danger";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                var year = DateTime.Now.Year;
                ucMonitorForBranch.StartDate = new DateTime(year, 01, 01);
                ucMonitorForBranch.EndDate = new DateTime(year, 12, 31);

                ucMonitorForMotor1.StartDate = new DateTime(year, 01, 01);
                ucMonitorForMotor1.EndDate = new DateTime(year, 12, 31);
                //ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.warning, "5555");
                //ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.success, "5555");

                //Set BrachID
                ucMonitorForBranch.BranchID = cFunction.GetCurrentBranchID(this.Page);

                if (HttpContext.Current.User.IsInRole("MotorDeveloper") || HttpContext.Current.User.IsInRole("MotorUnderwrite"))
                {
                    dashboardMotor.Visible = true;
                }
            }
        }
    }
}