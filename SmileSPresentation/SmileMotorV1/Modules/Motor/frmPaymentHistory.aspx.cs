using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmPaymentHistory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            dgvMain.DataSource = dt();
            dgvMain.DataBind();

        }

        private DataTable dt()
        {
            DataTable dt;
            dt = new DataTable();

            dt.Columns.Add("ID");
            dt.Columns.Add("Product");
            dt.Columns.Add("ClaimType");
            dt.Columns.Add("CustomerName");
            dt.Columns.Add("StartCoverDate");
            dt.Columns.Add("EndCoverDate");
            dt.Columns.Add("VehicleLicensePlate");

            dt.Rows.Add("1", "ประกัน1", "อู่", "สมชาย", "1-1-60", "1-1-61", "กย1234");
            dt.Rows.Add("2", "ประกัน2", "ศูนย์", "สมหญิง", "26-5-60", "26-5-61", "กย3456");
            dt.Rows.Add("3", "ประกัน3", "อู่", "สมาย", "5-1-60", "5-1-61", "1กย2234");
            dt.Rows.Add("4", "ประกัน4", "ศูนย์", "Test1", "1-6-60", "1-6-61", "2กย4534");

            return dt;
        }
    }
}