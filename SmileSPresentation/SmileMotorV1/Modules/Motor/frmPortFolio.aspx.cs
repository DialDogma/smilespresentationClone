using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using SmileSMotorClassLibrary;
using System.Web.UI.WebControls;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmPortFolio : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //check role = MotorDeveloper OR MotorUnderwrite
                if (HttpContext.Current.User.IsInRole("MotorDeveloper"))
                {
                    //load
                    Doload();
                    ucDatepicker1.DateSelected = DateTime.Now;
                }
                else
                {
                    Response.Redirect("frmSuccess.aspx?msg=2");
                }
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (Isvalidate())
            {
                ExportToExcel();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            MotorDB mdb = new MotorDB();

            if (Isvalidate())
            {
                mdb.AddMotorPortfolio(ucDatepicker1.DateSelected.Value, txtRemark.Text.Trim(), cFunction.GetCurrentEmployeeID(Page));

                //clear textremark
                txtRemark.Text = string.Empty;

                //load
                Doload();

                //alert แจ้งเตือนบันทึกเรียบร้อยแล้ว
                this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.success, "บันทึกข้อมูลการคิดผลงานเรียบร้อยแล้ว");
            }
        }

        #endregion Event

        #region Method

        private void ExportToExcel()
        {
            ExcelManager ex = new ExcelManager();
            MotorDB mdb = new MotorDB();
            DataTable dt = new DataTable();

            dt = mdb.GetMotorApplicationNoPortfolio(null, null);
            ex.DatatableToExcel(HttpContext.Current, "รายละเอียด App คิดผลงาน", dt, "Detail");
        }

        private void Doload()
        {
            MotorDB mdb = new MotorDB();
            DataTable dt = new DataTable();

            dt = mdb.GetMotorApplicationNoPortfolio(null, null);

            if (dt.Rows.Count != 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    int count = dt.Rows.IndexOf(row);
                    count = count + 1;
                    lblResult.Text = Convert.ToString(count);
                }
            }
            else
            {
                lblResult.Text = "0";
            }

            //dgvMain.DataSource = dt;
            //dgvMain.DataBind();
        }

        private bool Isvalidate()
        {
            MotorDB mdb = new MotorDB();
            DataTable dt = new DataTable();

            dt = mdb.GetMotorApplicationNoPortfolio(null, null);

            if (dt.Rows.Count == 0)
            {
                this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "ไม่พบข้อมูล Application");

                return false;
            }

            return true;
        }

        #endregion Method
    }
}