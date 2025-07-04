using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucAddEditCorporate : System.Web.UI.UserControl
    {
        #region Declare

        private Person _person;

        /// <summary>
        /// Property Get/Set Person Class
        /// </summary>
        public Person Person
        {
            get
            {
                //
                _person = new Person();
                _person.Title_ID = ddlCustomerTitle.Title_ID;
                _person.FirstName = txtCorporateName.Text.Trim();
                _person.LastName = string.Empty;
                _person.NickName = string.Empty;
                _person.Sex_ID = 1;  //1 = n/a

                _person.Occupation_ID = 1; //1 = n/a
                _person.OccupationLevel_ID = 1; // 1 =  n/a

                _person.BirthDate = DateTime.Now;
                _person.MaritalStatus_ID = 1; // 1 =  n/a

                // Get Text To Class
                PersonCard _identityCard = new PersonCard();
                PersonCard _taxpayerNO = new PersonCard();
                PersonCard _passport = new PersonCard();
                _taxpayerNO.PersonCardType_ID = 4; //เลขผู้เสียภาษี
                _taxpayerNO.PersonCardDetail = txtTaxpayerNo.Text.Trim();

                _passport.PersonCardType_ID = 3; //passport
                _passport.PersonCardDetail = null;

                _identityCard.PersonCardType_ID = 2; //บัตประชาชน
                _identityCard.PersonCardDetail = null;
                // Set Class To List
                List<PersonCard> lstPersonCard = new List<PersonCard>();

                lstPersonCard.Add(_identityCard);
                lstPersonCard.Add(_passport);
                lstPersonCard.Add(_taxpayerNO);

                _person.PersonCard = lstPersonCard;

                // Get Text To Class
                PersonContact _phone = new PersonContact();
                PersonContact _mobile = new PersonContact();

                _phone.ContactType_ID = 2;
                _phone.ContactDetail = txtTaxpayerMobileNumber.Text.Trim();

                _mobile.ContactType_ID = 3;
                _mobile.ContactDetail = string.Empty;
                // Set ClassTo List
                List<PersonContact> lstPersonContact = new List<PersonContact>();
                lstPersonContact.Add(_phone);
                lstPersonContact.Add(_mobile);

                _person.PersonContact = lstPersonContact;

                return _person;
            }

            set
            {
                _person = value;
            }
        }

        public Label TextHeader
        {
            get
            {
                return lblTextHeader;
            }
            set
            {
                lblTextHeader = value;
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
        /// Do Load Dropdown List
        /// </summary>
        public void DoLoadDropdownList()
        {
            this.ddlCustomerTitle.DoLoadDropdownList((int)MappingCode.ePersonType.Corporate);
        }

        /// <summary>
        /// Do Load Detail
        /// </summary>
        public void DoLoad(int person_ID)
        {
            DataCenterDB db = new DataCenterDB();
            List<Person> lst = new List<Person>();

            // Get Detail From Person
            lst = db.GetPersonByPersonID(person_ID);

            // Load DDL
            DoLoadDropdownList();

            // Bind Data
            ddlCustomerTitle.Title_ID = lst[0].Title_ID;
            txtCorporateName.Text = lst[0].FirstName;

            foreach (PersonCard itm in lst[0].PersonCard)
            {
                if (itm.PersonCardType_ID == 4) // หมายเลขผู้เสียภาษี
                {
                    txtTaxpayerNo.Text = itm.PersonCardDetail;
                }
            }

            foreach (PersonContact itm in lst[0].PersonContact)
            {
                if (itm.ContactType_ID == 2) // โทรศัพท์
                {
                    txtTaxpayerMobileNumber.Text = itm.ContactDetail;
                }
            }
        }

        /// <summary>
        /// Do Clear
        /// </summary>
        public void DoClear()
        {
            this.ddlCustomerTitle.DoLoadDropdownList((int)MappingCode.ePersonType.Corporate);
            txtCorporateName.Text = "";
            txtTaxpayerNo.Text = "";
            txtTaxpayerMobileNumber.Text = "";
        }

        /// <summary>
        /// Set Enable Text Box And DDL
        /// </summary>
        public void IsEnabled(bool value)
        {
            this.ddlCustomerTitle.IsEnabled(value);
            txtCorporateName.Enabled = value;
            txtTaxpayerNo.Enabled = value;
            txtTaxpayerMobileNumber.Enabled = value;
        }

        public bool IsValidate()
        {
            return true;
        }

        /// <summary>
        /// Validate Customer
        /// </summary>
        /// <returns></returns>
        public string ValidateCorporate()
        {
            TransactionManager tm = new TransactionManager();
            string result;

            // Valid Custommer From Class
            result = tm.Validate_Corporate(Person);

            return result;
        }

        /// <summary>
        /// Set Alert Warning
        /// </summary>
        /// <param name="val">True/False</param>
        public void SetAlertWarning(bool val)
        {
            if (val == true)
            {
                txtTaxpayerNo.CssClass = "input-sm-danger";
                txtTaxpayerNo.Focus();
            }
            else
            {
                txtTaxpayerNo.CssClass = "form-control input-sm";
            }
        }

        /// <summary>
        /// Get TaxpayerNo From Text Box
        /// </summary>
        /// <returns></returns>
        public string GetTaxpayerNo()
        {
            string result = txtTaxpayerNo.Text.Trim();
            return result;
        }

        public void DisableTaxNumber(bool value)
        {
            txtTaxpayerNo.Enabled = value;
        }

        #endregion Method
    }
}