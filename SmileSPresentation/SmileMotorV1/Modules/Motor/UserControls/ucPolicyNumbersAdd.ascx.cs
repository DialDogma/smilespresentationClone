using SmileSMotorClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucPolicyNumbersAdd : System.Web.UI.UserControl
    {
        #region Declear
        private int _MotorApplication_ID;

        public int MotorApplication_ID
        {
            get
            {
                return _MotorApplication_ID;
            }

            set
            {
                _MotorApplication_ID = value;
                hdfMotorApplication_ID.Value = value.ToString();
            }
        }
        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    //Validate
        //    if (Isvalidate() == true)
        //    {
        //        DoSave(_MotorApplication_ID);
        //        Doclear();
        //    }
        //}

        //protected void btnCancel_Click(object sender, EventArgs e)
        //{
        //    Doclear();
        //}
        #endregion

        #region Method

        /// <summary>
        /// Do Save เลขที่กรมธรรม์
        /// </summary>
        public void DoSave()
        {
            MotorDB mdb = new MotorDB();

            //Add  motorapp_id ,policytype_id,motorappPolicyDetail,Createbyuser
            mdb.AddMotorApplicationPolicy(Convert.ToInt32(hdfMotorApplication_ID.Value), 2, txtPolicyNumber.Text.Trim(), cFunction.GetCurrentEmployeeID(this.Page)); // 2 คือ กรมธรรม์ปกติ
        }

        /// <summary>
        /// Do Clear Text
        /// </summary>
        public void Doclear()
        {
            txtPolicyNumber.Text = "";
            lblWarning.Text = "";
        }

        /// <summary>
        /// Validate 
        /// </summary>
        /// <returns></returns>
        public bool Isvalidate()
        {   
            // ถ้าไม่กรอก
            if (txtPolicyNumber.Text.Trim() == "")
            {
                lblWarning.Text = "กรุณากรอกเลขที่กรมธรรม์";
                return false;
            }

            return true;
        }
        #endregion

    }
}