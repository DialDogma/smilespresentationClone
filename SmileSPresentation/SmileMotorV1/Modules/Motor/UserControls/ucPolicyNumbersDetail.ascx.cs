using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;
using System.Data;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucPolicyNumbersDetail : System.Web.UI.UserControl
    {
        #region Declear

        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #endregion

        #region Method

        /// <summary>
        /// โหลด GridView ข้อมูลรายละเอียดเลขกรมธรรม์
        /// </summary>
        /// <param name="MotorApplication_id"></param>
        public void Doload(int MotorApplication_id)
        {
            MotorDB mdb = new MotorDB();
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();

            //get ค่า จากเลข app id เก็บไว้ใน datatable
            dt = mdb.GetMotorApplicationPolicy(null, MotorApplication_id);

            //if (dt.Rows.Count > 0)
            //{
            //    //load
            //    fm.LoadGridview(dgvMain, dt);
            //}

            //load
            fm.LoadGridview(dgvMain, dt);

        }
        #endregion

    }
}