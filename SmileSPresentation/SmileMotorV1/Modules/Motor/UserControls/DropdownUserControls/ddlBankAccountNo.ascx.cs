using SmileSMotorClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileMotorV1.Modules.Motor.UserControls.DropdownUserControls
{
    public partial class ddlBankAccountNo : System.Web.UI.UserControl
    {

        #region Declare

        private int _PersonBankAccount_ID;

        public int PersonBankAccountID
        {
            get
            {
                try
                {
                    _PersonBankAccount_ID = Convert.ToInt32(ddlBankAccountNo1.SelectedValue);
                    return _PersonBankAccount_ID;
                }
                catch
                {
                    return -1;
                }
                
            }

            set
            {
                _PersonBankAccount_ID = value;
                ddlBankAccountNo1.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// ประกาศ type เป็น Dropdownlist 
        /// </summary>
        public DropDownList ddl
        {
            get
            {
                return ddlBankAccountNo1;
            }

            set
            {
                ddlBankAccountNo1 = value;
            }
        }
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        #region Method
        /// <summary>
        /// load dropdownlist
        /// </summary>
        /// <param name="_person_ID"></param>
        /// <param name="_bank_ID"></param>
        public void DoloadDropdownList(MappingCode.eProductGroup eProductGroup, int _person_ID, int? _bank_ID = null)
        {
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();

            int? productGroupID;

            switch (eProductGroup)
            {
                case MappingCode.eProductGroup.NotAvailable:
                    productGroupID = null;
                    break;
                default:
                    productGroupID = (int)eProductGroup;
                    break;
            }
            //clear dropdownlist
            ddlBankAccountNo1.Items.Clear();


            //function LoadDropdownlist
            fm.LoadDropdownlist(ddlBankAccountNo1, db.GetBankAccountByPersonIDandBankID(_person_ID, _bank_ID, null, null, null, productGroupID), "PersonBankAccountNo", "PersonBankAccount_ID", true);

        }
        /// <summary>
        /// method เปิด ปิด dropdownlist
        /// </summary>
        /// <param name="value"></param>
        public void IsEnabled(bool value)
        {
            ddlBankAccountNo1.Enabled = value;
            
        }


        #endregion
    }
}