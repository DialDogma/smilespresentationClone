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
    public partial class ucAppDetailOwner : System.Web.UI.UserControl
    {

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }
        #endregion


        #region Method

        /// <summary>
        /// Do Load Detail
        /// </summary>
        /// <param name="_motorApplication_ID"></param>
        public void DoLoad(int _motorApplication_ID)
        {
            MotorDB db = new MotorDB();
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();

            // Get Motor Application Owner
            dt = db.GetMotorApplicationOwner(_motorApplication_ID);

            // Load Gridview
            fm.LoadGridview(dgvMain, dt);
        }
        #endregion
    }
}