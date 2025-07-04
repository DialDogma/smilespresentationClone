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
    public partial class ucAppDetailPersonRelatedApplication : System.Web.UI.UserControl
    {
        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected void dgvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Paging
            dgvMain.PageIndex = e.NewPageIndex;
            LoadGridview();
        }

        #endregion

        #region Method

        /// <summary>
        /// Do Load Person Related Application
        /// </summary>
        /// <param name="personID">Peron ID</param>
        public void DoLoad(int personID)
        {
            // Set Value To HDF
            hdfPerson_ID.Value = personID.ToString();

            DataTable dt = new DataTable();
            MotorDB db = new MotorDB();
            FunctionManager fm = new FunctionManager();

            // Get Person Relate Application
            dt = db.GetPersonRelateApplicationByPersonID(personID);

            // Load Gridview
            fm.LoadGridview(dgvMain, dt);
        }

        /// <summary>
        /// Load Gridview
        /// </summary>
        private void LoadGridview()
        {
            DataTable dt = new DataTable();
            MotorDB db = new MotorDB();
            FunctionManager fm = new FunctionManager();

            // Get Person Relate Application
            dt = db.GetPersonRelateApplicationByPersonID(Convert.ToInt32(hdfPerson_ID.Value));

            // Load Gridview
            fm.LoadGridview(dgvMain, dt);
        }

        #endregion

        
    }
}