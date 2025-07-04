using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;
using System.Data;
using System.Drawing;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmImportFilePolicy : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            //check role = MotorDeveloper OR MotorUnderwrite
            if (HttpContext.Current.User.IsInRole("MotorDeveloper") || HttpContext.Current.User.IsInRole("MotorUnderwrite"))
            {
                // Setup uc
                this.ucAppDetailApplication1.Visible = false;
                this.ucDocument.Visible = false;

                //ถ้ามีการเลือกข้อมูล
                if (dgvMain.SelectedIndex != -1)
                {
                    dgvMain_SelectedIndexChanged(new object(), new EventArgs());
                }
            }
            else
            {
                Response.Redirect("frmSuccess.aspx?msg=2");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //โหลด grigview
            ClearSelectIndex();
            LoadGridView();
        }

        protected void dgvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //เปลี่ยนหน้า paging
            dgvMain.PageIndex = e.NewPageIndex;
            ClearSelectIndex();
            LoadGridView();
        }

        protected void dgvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //attributes onclick Select on row
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(dgvMain, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "คลิกเพื่อเลือกข้อมูล";
            }
        }

        protected void dgvMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            //foreach row ใน dgvMain
            foreach (GridViewRow row in dgvMain.Rows)
            {
                //ถ้า rowIndex เท่ากับ SelectIndex ที่เลือก
                if (row.RowIndex == dgvMain.SelectedIndex)
                {
                    //set สีเป็นสีแดงๆ set tooltip = string.empty
                    row.BackColor = ColorTranslator.FromHtml("#CC3333");
                    row.ToolTip = string.Empty;

                    // เก็บ applicationID จาก dataKeys ไว้ใน app_id
                    string app_id = dgvMain.DataKeys[row.RowIndex].Values[0].ToString();

                    //โหลด ucappdatail โดย appid
                    this.ucAppDetailApplication1.DoLoad(Convert.ToInt32(app_id));

                    //โหลด ucDoc โดย appid
                    this.ucDocument.DoLoad(Convert.ToInt32(app_id));
                    //แสดง ucapp & ucDoc
                    this.ucAppDetailApplication1.Visible = true;
                    this.ucDocument.Visible = true;
                }
                else
                {
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    row.ToolTip = "คลิกเพื่อเลือกข้อมูล";
                }
            }
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Load GridView
        /// </summary>
        private void LoadGridView()
        {
            MotorDB db = new MotorDB();
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();

            //function get โดยคำค้นหา เก็บไว้ใน data table
            dt = db.GetMotorAppByNoPolicyDocument(null, txtSearch.Text.Trim());

            //นับ row result
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    int count = dt.Rows.IndexOf(row);
                    count = count + 1;
                    lblCountResult.Text = Convert.ToString(count);
                }
            }
            else
            {
                lblCountResult.Text = "0";
            }

            //โหลด gridview โดย function
            fm.LoadGridview(dgvMain, dt, "MotorApplication_ID");
        }

        /// <summary>
        /// Clear Select Index DGV
        /// </summary>
        private void ClearSelectIndex()
        {
            dgvMain.SelectedIndex = -1;
            this.ucAppDetailApplication1.Visible = false;
            this.ucDocument.Visible = false;
        }

        #endregion Method
    }
}