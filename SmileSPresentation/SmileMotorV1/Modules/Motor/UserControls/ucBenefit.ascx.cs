using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucBenefit : System.Web.UI.UserControl
    {

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #endregion

        #region Method

        /// <summary>
        /// Do Load Product Benefit From Application
        /// </summary>
        /// <param name="motorApplicationID">รหัส Application</param>
        public void DoLoad(int motorApplicationID)
        {
            MotorDB db = new MotorDB();
            ProductManager pm = new ProductManager();
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();

            // Get Motor Application Benefit Coverange
            dt = db.GetBenefitCoverageByMotorApplicationID(motorApplicationID);

            // Load Gridview
            fm.LoadGridview(dgvMain, dt);
        }

        /// <summary>
        /// Set Enable DGV
        /// </summary>
        /// <param name="value">True/False</param>
        public void IsEnabled(bool value)
        {
            this.dgvMain.Enabled = value;
        }

        #endregion

    }
}