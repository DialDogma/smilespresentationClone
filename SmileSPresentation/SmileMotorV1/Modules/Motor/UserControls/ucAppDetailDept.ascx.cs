using SmileSMotorClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucAppDetailDept : System.Web.UI.UserControl
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnLinkCashReceive.Width = Unit.Pixel(200);
            }
        }

        protected void dgvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Select ค่าใน Cell ที่ 8 ของแต่ละ Row
                string Status = e.Row.Cells[7].Text.Trim();

                // เป็น Text ที่ไม่เท่ากับ... ?
                if (Status != "ชำระแล้ว")
                {
                    e.Row.ForeColor = Color.Red;
                }

                // Set Att
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(dgvMain, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "คลิกเพื่อดูรายละเอียด";
            }
        }

        protected void dgvMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Select Data Key Value
            int dk = Convert.ToInt32(dgvMain.SelectedDataKey.Value);

            // Load UC With Data Key
            this.ucDebtRecieveDetail.DoLoad(dk);

            // Show Modal
            mpe.Show();
        }

        protected void dgvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Paging
            dgvMain.PageIndex = e.NewPageIndex;
            dgvMain.DataBind();
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Do Load Detail
        /// </summary>
        /// <param name="motorApplication_ID">Motor Application ID</param>
        public void DoLoad(int motorApplication_ID)
        {
            // Set Motor Application ID To Hidden Field
            hdfApp_ID.Value = motorApplication_ID.ToString();
            MotorDB db = new MotorDB();
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();
            List<MotorApplication> lst = new List<MotorApplication>();
            string app_code;

            dt = db.GetDebtByMotorApplicationID(motorApplication_ID);
            lst = fm.DataTableToList<MotorApplication>(dt);

            if (lst.Count > 0)
            {
                //set app_code
                app_code = lst[0].MotorApplication_Code;

                // Load Gridview
                fm.LoadGridview(dgvMain, dt, "Debt_ID");

                //btnLinkCashReceive.Attributes.Add("onclick", "window.open('" + "http://49.231.178.253:84/SSSCashReceive/Modules/CashReceive/frmReceiveDetailDisplay.aspx?" + "app=" + app_code + "')");

                btnLinkCashReceive.Attributes.Add("onclick", "window.open('" + "http://operation.siamsmile.co.th:9103/Modules/CashReceive/frmReceiveDetailDisplay.aspx?" + "app=" + app_code + "')");
            }
            else
            {
                fm.LoadGridview(dgvMain, dt, "Debt_ID");
                btnLinkCashReceive.Visible = false;
            }
        }

        #endregion Method
    }
}