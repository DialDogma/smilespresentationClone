using SmileSMotorClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileMotorV1.Modules.Motor.UserControls.DropdownUserControls
{
    public partial class ucRelation : System.Web.UI.UserControl
    {

        #region Declare
        // Set เป็นบุคลคนเดียวกัน
        string _samePerson = "39";

        private int _relation_ID;
        private string _relationDetail;
        private bool _AutoPostback;

        /// <summary>
        /// Property Get/Set Relation ID
        /// </summary>
        public int Relation_ID
        {
            get
            {
                _relation_ID = Convert.ToInt32(ddlRelation.SelectedValue);
                return _relation_ID;
            }

            set
            {
                _relation_ID = value;
                ddlRelation.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Property Get/Set Relation Detail
        /// </summary>
        public string RelationDetail
        {
            get
            {
                _relationDetail = ddlRelation.SelectedValue;
                return _relationDetail;
            }

            set
            {
                _relationDetail = value;
                ddlRelation.SelectedItem.Text = value;
            }
        }

        /// <summary>
        /// Property Get/Set Auto Postback
        /// </summary>
        public bool AutoPostback
        {
            get
            {
                _AutoPostback = ddlRelation.AutoPostBack;
                return _AutoPostback;
            }

            set
            {
                _AutoPostback = value;
                ddlRelation.AutoPostBack = value;
            }
        }

        #endregion


        #region RaiseEvent
        public event EventHandler eSelectedNotSamePerson;
        public event EventHandler eSelectedSamePerson;

        #endregion


        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }

        }

        protected void ddlRelation_SelectedIndexChanged(object sender, EventArgs e)
        {
            EventHandler handler;

            if (ddlRelation.SelectedValue == _samePerson)
            {
                handler = eSelectedSamePerson;
            }
            else
            {
                handler = eSelectedNotSamePerson;
            }

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion


        #region Method

        /// <summary>
        /// Do Load Dropdown List
        /// </summary>
        public void DoLoadDropdownList()
        {
            FunctionManager fm = new FunctionManager();
            DataCenterDB db = new DataCenterDB();
            DataTable dt = new DataTable();

            // Get Person Relate Type
            dt = db.GetPersonRelationType();

            // Clear
            ddlRelation.Items.Clear();

            // Load Dropdown
            fm.LoadDropdownlist(ddlRelation, dt, "PersonRelationType", "PersonRelationType_ID", true);

        }

        /// <summary>
        /// Do Clear DDL
        /// </summary>
        public void DoClear()
        {
            ddlRelation.Items.Clear();
        }

        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="_bool">True/False</param>
        public void IsEnabled(bool _bool)
        {
            ddlRelation.Enabled = _bool;
        }

        /// <summary>
        /// Validate Relation
        /// </summary>
        /// <returns></returns>
        public string ValidateRelation()
        {
            TransactionManager tm = new TransactionManager();
            string result;

            // Validate From Class
            result = tm.Validate_Relation(Relation_ID);

            return result;
        }

        #endregion


    }
}