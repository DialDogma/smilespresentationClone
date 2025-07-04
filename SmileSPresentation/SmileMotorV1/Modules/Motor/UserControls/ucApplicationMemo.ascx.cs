using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using SmileSMotorClassLibrary;


namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucApplicationMemo : System.Web.UI.UserControl
    {

#region Declare

        private int _motorApplication_ID;

        /// <summary>
        /// Property Get/Set Motor Application ID
        /// </summary>
        public int MotorApplication_ID
        {
            get 
            {
                _motorApplication_ID = Convert.ToInt32(hdfApp_id.Value);
                return _motorApplication_ID;
            }
            set 
            {
                _motorApplication_ID = value;
                hdfApp_id.Value = Convert.ToString(_motorApplication_ID);
            }
        }


#endregion



#region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                // Load DDL
                this.ddlMemoType1.DoLoadDropDownlistAll();
                this.ddlMemoType2.DoLoadDropDownlist();
            }

            // Load Gridview
            LoadGridview();

            // Set Width Button
            this.btnAdd.Width = Unit.Percentage(30);
            this.btnSave.Width = Unit.Percentage(30);
            this.btnCancel.Width = Unit.Percentage(30);
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            // Load Gridview
            LoadGridview();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Valid
            if (Isvalidate() == true)
            {
                // Save Memo
                DoSave();
                Doclear();
                LoadGridview();
            }
            else
            {
                // Show Modal
                mpe.Show();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Clear And Hide Modal
            Doclear();
            mpe.Hide();
        }


#endregion



#region Method
        
        /// <summary>
        /// Do Load Detail
        /// </summary>
        /// <param name="motorAppID">Motor Application ID</param>
        public void DoLoad(int motorAppID)
        {
            // Set Motor Application
            MotorApplication_ID = motorAppID;

            // Load Gridview
            LoadGridview();
        }

        /// <summary>
        /// Load Gridview
        /// </summary>
        private void LoadGridview()
        {
            FunctionManager fm = new FunctionManager();
            MotorDB mdb = new MotorDB();
            int appID ;
            int? typeID = null;

            // Get HDF 
            appID = Convert.ToInt32(hdfApp_id.Value);
            
            // Check Memo Type ID
            if (ddlMemoType1.MemoTypeID != -1)
            {
                typeID = ddlMemoType1.MemoTypeID;
            }

            // Load Gridview
            fm.LoadGridview(dgvMain, mdb.GetApplicationMemo(appID, typeID),"MotorApplicationMemo_ID" );
        }

        /// <summary>
        /// Validate Text Memo
        /// </summary>
        /// <returns></returns>
        private bool Isvalidate()
        {
            // Memo Type ถ้าไม่ระบุ
            if (ddlMemoType2.MemoTypeID == 1)
            {
                lblWarning.Text = "กรุณาเลือกประเภท";
                return false;
            }

            // ถ้าไม่กรอก ข้อความ Remark
            if (txtRemark.Text.Trim() == "")
            {
                lblWarning.Text = "กรุณากรอกข้อความ";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Do Save Memo
        /// </summary>
        private void DoSave()
        {
            int appID;
            int typeID;
            string remark;

            MotorDB mdb = new MotorDB();        

            // Get Value From HDF
            appID =Convert.ToInt32(hdfApp_id.Value);
            typeID = Convert.ToInt32(ddlMemoType2.MemoTypeID);
            remark = txtRemark.Text.Trim();

            // Add Motor Application Memo
            mdb.AddMotorApplicationMemo(appID, typeID, remark, cFunction.GetCurrentLoginUser_ID(this.Page));
        }

        /// <summary>
        /// Do Clear Text
        /// </summary>
        private void Doclear()
        {
            txtRemark.Text = "";
            lblWarning.Text = "";
        }

        /// <summary>
        /// Set Visible Button
        /// </summary>
        /// <param name="value">True/False</param>
        public void IsVisible(bool value)
        {
            btnAdd.Visible = value;
            ModalPanel.Visible = value;
        }
        #endregion

    }

}