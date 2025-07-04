using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucPersonSearchByIdentityNumber : System.Web.UI.UserControl
    {

        #region Declear
        private int? _PersonType_ID;
        private int? _PersonCardType_ID;
        private int? _Person_ID;

        /// <summary>
        /// Property Get/Set
        /// </summary>
        public int? Person_ID
        {
            get
            {
                return _Person_ID;
            }

            set
            {
                _Person_ID = value;
            }
        }

        public int PersonCardType_ID
        {
            get
            {
                return _PersonCardType_ID.Value;
            }

            set
            {
                _PersonCardType_ID = value;
            }
        }

        public int? PersonType_ID
        {
            get
            {
                return _PersonType_ID.Value;
            }

            set
            {
                _PersonType_ID = value;
            }
        }

        #endregion

        #region RiaseEvent
        public event EventHandler eClickClear;
        public event EventHandler eSelectedPersonIDChanged;
        public event EventHandler eClickPersonIDChangednull;
        public event EventHandler eClickPersonIDChangednotnull;

        protected virtual void OnClickClear(object sender, EventArgs e)
        {
            _Person_ID = null;
            txtCustomerIDNumber.Text = string.Empty;
            txtResultPostback1.Text = string.Empty;
            hdfPersonID.Value = string.Empty;
            txtCustomerIDNumber.Enabled = true;

            EventHandler handler = eClickClear;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtCustomerIDNumber.Width = Unit.Pixel(220);
            }
        }

        protected void txtResultPostback1_TextChanged(object sender, EventArgs e)
        {
            // Set Text To Property
            _Person_ID = Convert.ToInt32(txtResultPostback1.Text.Trim());

            // Set Text To HDF
            hdfPersonID.Value = _Person_ID.ToString();
            txtCustomerIDNumber.Enabled = false;
            EventHandler handler = eSelectedPersonIDChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void btnSearch1_Click(object sender, EventArgs e)
        {
            EventHandler handler;


            // ถ้า Text ว่าง
            if (txtCustomerIDNumber.Text == "")
            {
                handler = eClickPersonIDChangednull;
            }

            // ไม่ว่าง
            else
            {
                handler = eClickPersonIDChangednotnull;
            }

            if (handler != null)
            {
                handler(this, e);
            }
        }


        #endregion

        #region Method

        /// <summary>
        /// Set Enable Text / Button
        /// </summary>
        /// <param name="value">True/False</param>
        public void IsEnabled(bool value)
        {
            txtCustomerIDNumber.Enabled = value;
            btnClear.Enabled = value;
            // ปล่อยมันไว้ค่อยมาเล่น
            //btnSearch.Disabled = value;
        }

        /// <summary>
        /// Validate Identity Card
        /// </summary>
        /// <returns></returns>
        public bool IsValidateIdentityCard()
        {
            FunctionManager fm = new FunctionManager();

            // Validate From Class
            if (fm.IsValidateIdentityCard(txtCustomerIDNumber.Text.Trim()) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DoClearTextBoxSearch()
        {
            //txtCustomerIDNumber.Text = string.Empty;
            txtResultPostback1.Text = string.Empty;
            txtCustomerIDNumber.Enabled = true;
        }


        public void SetPersonTypeIDTextbox(string persontype_id)
        {
            txtPersonTypeID.Text = persontype_id;
        }

        public void DoClearTextBoxSearch2()
        {
            txtCustomerIDNumber.Text = string.Empty;
            txtResultPostback1.Text = string.Empty;
            txtCustomerIDNumber.Enabled = true;
        }

        #endregion

    }
}