using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;
using System.Data;

namespace SmileMotorV1.Modules.Motor.UserControls.DropdownUserControls
{
    public partial class ddlSubDistrict : System.Web.UI.UserControl
    {

        #region Declare
        private string _SubDistrict_ID;
        private string _SubDistrictDetail;
        private bool _AutoPostback;
        private string _ZipCode;

        /// <summary>
        /// Property Get/Set Zip Code
        /// </summary>
        public string ZipCode
        {
            get { return _ZipCode; }
            set { _ZipCode = value; }
        }

        /// <summary>
        /// Property Get/Set SubDistrict ID
        /// </summary>
        public string SubDistrict_ID
        {
            get
            {
                _SubDistrict_ID = ddlSubDistrict1.SelectedValue;
                return _SubDistrict_ID;
            }
            set
            {
                _SubDistrict_ID = value;
                ddlSubDistrict1.SelectedValue = value;
            }
        }

        /// <summary>
        /// Property Get/Set SubDistrict Detail
        /// </summary>
        public string SubDistrictDetail
        {
            get
            {
                try
                {
                    _SubDistrictDetail = ddlSubDistrict1.SelectedItem.Text;
                    return _SubDistrictDetail;
                }
                catch
                { return string.Empty; }

            }
            set
            {
                _SubDistrictDetail = value;
                ddlSubDistrict1.SelectedItem.Text = value;
            }
        }

        /// <summary>
        /// Property Get/Set Auto Postback
        /// </summary>
        public bool AutoPostback
        {
            get
            {
                _AutoPostback = ddlSubDistrict1.AutoPostBack;
                return _AutoPostback;
            }
            set
            {
                _AutoPostback = value;
                ddlSubDistrict1.AutoPostBack = value;
            }
        }

        #endregion


        #region RaiseEvent
        public event EventHandler SelectChanged;

        protected virtual void OnSelectChanged(object sender, EventArgs e)
        {
            EventHandler handler = SelectChanged;

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

            }
        }
        #endregion


        #region Method

        /// <summary>
        /// Do Load Dropdown List
        /// </summary>
        void DoLoadDropdownList()
        {
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();

            // Clear
            ddlSubDistrict1.Items.Clear();

            // Load Dropdown
            fm.LoadDropdownlist(ddlSubDistrict1, db.GetSubDistrict(), "SubDistrictDetail", "SubDistrict_ID");
        }

        /// <summary>
        /// Do Load Zip Code
        /// </summary>
        /// <param name="subdistrict_id">Subdistrict ID</param>
        /// <returns>Zip Code</returns>
        public string DoLoadZipCode(int subdistrict_id)
        {
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();
            List<Address> lst = new List<Address>();
            string ZipCode;

            // Get SubDistrict
            dt = db.GetSubDistrict(subdistrict_id);

            // Convert To List
            lst = fm.DataTableToList<Address>(dt);

            // Get Zip Code
            ZipCode = lst[0].ZipCode;

            return ZipCode;
        }

        /// <summary>
        /// Do Load Dropdown List
        /// </summary>
        /// <param name="district_id"></param>
        public void DoLoadDropdownList(string district_id)
        {
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();

            // Get SubDistrict
            dt = db.GetSubDistrictByDistrict(Convert.ToInt32(district_id));

            // Clear
            ddlSubDistrict1.Items.Clear();

            // Load Dropdown
            fm.LoadDropdownlist(ddlSubDistrict1, dt, "SubDistrictDetail", "SubDistrict_ID", true);

        }

        /// <summary>
        /// DDL Select Fix Value
        /// </summary>
        public void DDL_SelectedValue()
        {
            ddlSubDistrict1.SelectedValue = "-1";
        }

        /// <summary>
        /// Do Clear DDL
        /// </summary>
        public void DoClear()
        {
            ddlSubDistrict1.Items.Clear();
        }

        /// <summary>
        /// Set Enable DDL
        /// </summary>
        /// <param name="_bool"></param>
        public void IsEnabled(bool _bool)
        {
            ddlSubDistrict1.Enabled = _bool;
        }
        #endregion

    }
}