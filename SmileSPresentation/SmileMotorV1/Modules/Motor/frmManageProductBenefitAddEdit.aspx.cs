using SmileSMotorClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmManageProductBenefitAddEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DoLoad();
        }

        public void DoLoad()
        {
            DataCenterDB db = new DataCenterDB();
            ProductManager pm = new ProductManager();
            DataTable dt = new DataTable();

            dt =  db.GetBenefit();

            dgvMain.DataSource = dt;
            dgvMain.DataBind();
        }
    }
}