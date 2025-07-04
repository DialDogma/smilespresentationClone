using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileClaimV1.Module.Claim.UserControl
{
    public partial class ucSearchCustomer : System.Web.UI.UserControl
    {
        #region Declare
        public event EventHandler eSelectedChanged;
        public event EventHandler eButtonClick;
        private bool _AutoPostback;
        private string _ProductTypeID;


        public bool AutoPostback
        {
            get
            {
                _AutoPostback = ddlProductType.AutoPostBack;
                return _AutoPostback;
            }

            set
            {
                _AutoPostback = value;
                ddlProductType.AutoPostBack = value;
            }
        }

        public string ProductTypeID
        {
            get
            {
                return _ProductTypeID;
            }

            set
            {
                _ProductTypeID = value;
                ddlProductType.SelectedValue = value;
            }
        }


        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            EventHandler handler2 = eButtonClick;
            if (handler2 != null)
            {
                handler2(this, e);
            }
        }
        protected void ddlProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ProductTypeID = ddlProductType.SelectedValue;

            EventHandler handler = eSelectedChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion

        #region Method

        public void DoloadDropdownlist()
        {
            //-----ทดสอบ-----//
            DataTable dt = new DataTable();

            dt.Columns.Add("ID");
            dt.Columns.Add("Detail");

            dt.Rows.Add("-1", "---โปรดระบุ---");
            dt.Rows.Add("1", "ประกันส่วนบุคคล");
            dt.Rows.Add("2", "ประกันอุบัติเหตุนักเรียน");
            dt.Rows.Add("3", "ทั้งหมด");


            ddlProductType.DataSource = dt;
            ddlProductType.DataTextField = "Detail"; // property u wanna display
            ddlProductType.DataValueField = "ID"; // property u wanna retrieve value from
            ddlProductType.DataBind();
            ddlProductType.SelectedIndex = -1; // if u want to select the very 1st item by default
        }
        #endregion

        
    }
}