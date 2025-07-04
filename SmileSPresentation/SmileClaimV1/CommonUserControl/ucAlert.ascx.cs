using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileClaimV1.CommonUserControl
{
    public partial class ucAlert : System.Web.UI.UserControl
    {
        #region Declare
        public enum AlertStatus
        {
            success,
            info,
            warning,
            danger,
            clear
        }
        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SetMessage(AlertStatus status, string txt)
        {
            lblAlert.Visible = true;
            switch (status.ToString())
            {
                case "success":
                    lblAlert.CssClass = "alert alert-success";
                    break;
                case "info":
                    lblAlert.CssClass = "alert alert-info";
                    break;
                case "warning":
                    lblAlert.CssClass = "alert alert-warning";
                    break;
                case "danger":
                    lblAlert.CssClass = "alert alert-danger";
                    break;
                case "clear":
                    lblAlert.Visible = false;
                    break;
            }
            lblAlert.Text = "<button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>";
            lblAlert.Text += txt;
        }
        #endregion

    }
}