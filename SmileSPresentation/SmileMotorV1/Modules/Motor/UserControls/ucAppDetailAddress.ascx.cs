using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucAppDetailAddressCustomer : System.Web.UI.UserControl
    {
        #region Declare

        private string _HeaderText;

        /// <summary>
        /// Property Set/Get Text UC Header
        /// </summary>
        public string HeaderText
        {
            get
            {
                _HeaderText = lblHeader.Text.Trim();
                return _HeaderText;
            }
            set
            {
                _HeaderText = value;
                lblHeader.Text = value;
            }
        }

        #endregion Declare

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Load Gridview
        /// </summary>
        /// <param name="motorapplicationID">Motor Application ID</param>
        /// <param name="relateTypeID">Relate Type ID</param>
        private void LoadGridView(int motorapplicationID, int relateTypeID)
        {
            MotorDB mdb = new MotorDB();
            FunctionManager fm = new FunctionManager();

            fm.LoadGridview(dgvMain, mdb.GetMotorApplicationAddressBy(motorapplicationID, relateTypeID, null));
        }

        /// <summary>
        /// Do Load Detail
        /// </summary>
        /// <param name="motorapplicationID">Motor Application ID</param>
        /// <param name="relateType">Relate Type ID</param>
        public void DoLoad(int motorapplicationID, MappingCode.ePersonRelatedApplicationType relateType)
        {
            int relateTypeID;

            // Select Case Relate Type
            switch (relateType)
            {
                // ลูกค้า
                case MappingCode.ePersonRelatedApplicationType.Customer:
                    relateTypeID = (int)MappingCode.ePersonRelatedApplicationType.Customer;
                    HeaderText = "ข้อมูลที่อยู่ผู้เอาประกัน";
                    break;
                // ผู้ชำระเบี้ย
                case MappingCode.ePersonRelatedApplicationType.Payer:
                    relateTypeID = (int)MappingCode.ePersonRelatedApplicationType.Payer;
                    HeaderText = "ข้อมูลที่อยู่ผู้ชำระเบี้ย";
                    break;
                // Defualt
                default:
                    relateTypeID = 0;
                    HeaderText = "";
                    break;
            }

            LoadGridView(motorapplicationID, relateTypeID);
        }

        public void DoLoad2(int motorapplicationID, int relateType)
        {
            int relateTypeID;

            // Select Case Relate Type
            switch (relateType)
            {
                // ลูกค้า
                case 2:
                    relateTypeID = 2;
                    HeaderText = "ข้อมูลที่อยู่ผู้เอาประกัน";
                    break;
                // ผู้ชำระเบี้ย
                case 3:
                    relateTypeID = 3;
                    HeaderText = "ข้อมูลที่อยู่ผู้ชำระเบี้ย";
                    break;
                // Defualt
                default:
                    relateTypeID = 0;
                    HeaderText = "";
                    break;
            }

            LoadGridView(motorapplicationID, relateTypeID);
        }

        #endregion Method
    }
}