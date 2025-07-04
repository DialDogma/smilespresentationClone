using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmMotorUnderwrite : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //check role = MotorDeveloper OR MotorUnderwrite
                if (HttpContext.Current.User.IsInRole("MotorDeveloper") || HttpContext.Current.User.IsInRole("MotorUnderwrite"))
                {
                    if (Request.QueryString["branch"] != null && Request.QueryString["branch"] != string.Empty)
                    {
                        string branchID = (Request.QueryString["branch"]);

                        //ถอดรหัส Base64
                        FunctionManager fm = new FunctionManager();
                        string result = fm.Base64Decrypt(branchID);

                        ddlBranch1.DoLoadDropDownlist();
                        ddlBranch1.BranchID = Convert.ToInt32(result);
                        LoadGridview(Convert.ToInt32(result));
                    }
                    else
                    {
                        ddlBranch1.DoLoadDropDownlist();
                    }
                }
                else
                {
                    Response.Redirect("frmSuccess.aspx?msg=2");
                }
            }
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //send query string
                string motor_id = dgvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();

                //Encrypt
                FunctionManager fm = new FunctionManager();
                string EncryptMotorID = fm.Base64Encrypt(motor_id);

                e.Row.Attributes.Add("onclick", "window.open('" + "frmApproveNewApp.aspx?" + "motorid=" + EncryptMotorID + "')");
                e.Row.ToolTip = "คลิกเพื่อเปิดดูรายละเอียด";
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGridview(Convert.ToInt32(ddlBranch1.BranchID));
        }

        #endregion Event

        #region Method

        private void LoadGridview(int branchID)
        {
            FunctionManager fm = new FunctionManager();
            MotorDB mdb = new MotorDB();

            fm.LoadGridview(dgvMain, mdb.GetMotorApplicationWaitApproveByBranchID(branchID), "MotorApplication_ID");
        }

        #endregion Method
    }
}