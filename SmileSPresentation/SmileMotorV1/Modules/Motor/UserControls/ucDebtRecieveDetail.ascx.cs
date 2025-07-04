using SmileSMotorClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucDebtRecieveDetail : System.Web.UI.UserControl
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        protected void dgvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnLinkOpen = (Button)e.Row.FindControl("btnLinkOpen");
                string mth_ID = e.Row.Cells[3].Text.Trim();

                btnLinkOpen.CommandArgument = mth_ID;
            }
        }

        protected void dgvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Open":

                    //string url = "http://49.231.178.253:84/SSSCashReceive/Modules/CashReceive/frmReceiveDetailDisplay.aspx?";
                    string url = "http://operation.siamsmile.co.th:9103/Modules/CashReceive/frmReceiveDetailDisplay.aspx?";
                    string script = "window.open('" + url + "mthID=" + e.CommandArgument.ToString() + "','_blank');";
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", script, true);

                    break;
            }
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Do Load Detail
        /// </summary>
        /// <param name="_debt_ID"></param>
        public void DoLoad(int _debt_ID)
        {
            MotorDB db = new MotorDB();
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();

            // Set Debt ID To HDF
            hdfDebt_ID.Value = _debt_ID.ToString();

            // Get Debt Receive Detail
            dt = db.GetDebtReCeiveByDebtID(_debt_ID);

            // Load Gridview
            fm.LoadGridview(dgvMain, dt);
        }

        #endregion Method
    }
}