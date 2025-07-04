using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;
using System.Drawing;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucAddEditCustomer : System.Web.UI.UserControl
    {
        #region Declare

        private Person _person;
        private string setTextHeader;

        public Person Person
        {
            get
            {
                _person = new Person();
                _person.Title_ID = ddlCustomerTitle.Title_ID;
                _person.FirstName = txtCustomerName.Text.Trim();
                _person.LastName = txtCustomerSurename.Text.Trim();
                _person.NickName = hdfNickname.Value;
                _person.Sex_ID = ddlCustomerSex.SexID;

                _person.Occupation_ID = ddlJobType.Occupation_ID;
                _person.OccupationLevel_ID = ddlJobTypeLevel.OccupationLevel_ID;

                _person.BirthDate = dtCustomerDateOfBirth.DateSelected;
                _person.MaritalStatus_ID = ddlMaritalStatus.MaritalStatus_ID;

                //PersonCard
                PersonCard _identityCard = new PersonCard();
                PersonCard _passport = new PersonCard();
                PersonCard _taxpayerNO = new PersonCard();

                _identityCard.PersonCardType_ID = 2;
                _identityCard.PersonCardDetail = txtCustomerIDCardNumber.Text.Trim();

                _passport.PersonCardType_ID = 3;
                _passport.PersonCardDetail = txtCustomerPassportNumber.Text.ToUpper().Trim();

                _taxpayerNO.PersonCardType_ID = 4; //เลขผู้เสียภาษี
                _taxpayerNO.PersonCardDetail = null;

                List<PersonCard> lstPersonCard = new List<PersonCard>();
                lstPersonCard.Add(_identityCard);
                lstPersonCard.Add(_passport);
                lstPersonCard.Add(_taxpayerNO);

                _person.PersonCard = lstPersonCard;

                //PersonContact
                PersonContact _phone = new PersonContact();
                PersonContact _mobile = new PersonContact();

                _phone.ContactType_ID = 2;
                _phone.ContactDetail = txtCustomerPhoneNumber.Text.Trim();

                _mobile.ContactType_ID = 3;
                _mobile.ContactDetail = txtCustomerMobileNumber.Text.Trim();

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

        public string SetTextHeader
        {
            get
            {
                setTextHeader = lblTextHeader.Text;
                return setTextHeader;
            }

            set
            {
                setTextHeader = value;
                lblTextHeader.Text = value;
            }
        }

        public Button setButtonEditPerson
        {
            get
            {
                return btnEditPerson;
            }
            set
            {
                btnEditPerson = value;
            }
        }

        #endregion Declare

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                // Set Auto Post Back
                this.ddlJobType.AutoPostback = true;
                this.ddlJobTypeLevel.AutoPostback = true;
            }
            // Set Style Upper Case TXT Passport
            txtCustomerPassportNumber.CssClass = "form-control input-sm uppercase";
            // Set Button CSS Style
            btnEditPerson.CssClass = "btn btn-warning btn-xs";
        }

        protected void ddlJobType_SelectChanged(object sender, EventArgs e)
        {
            // Load Job Type Level
            this.ddlJobTypeLevel.AutoPostback = true;
            ddlJobTypeLevel.DoLoadDropdownlist(ddlJobType.Occupation_ID);
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Do Load Dropdownlist
        /// </summary>
        public void DoLoadDropdownList()
        {
            this.ddlCustomerTitle.DoLoadDropdownList();
            this.ddlCustomerSex.DoLoadDropdownList();
            this.ddlMaritalStatus.DoLoadDropdownList();
            this.ddlJobType.DoLoadDropdownList();
        }

        /// <summary>
        /// Do Load Detail / Bind Data To Field
        /// </summary>
        public void DoLoadDetail(int person_ID)
        {
            DataCenterDB db = new DataCenterDB();
            List<Person> lst = new List<Person>();

            lst = db.GetPersonByPersonID(person_ID).ToList();

            txtCustomerName.Text = lst[0].FirstName;
            txtCustomerSurename.Text = lst[0].LastName;
            hdfNickname.Value = lst[0].NickName;

            // วันเกิด
            if (lst[0].BirthDate.HasValue == true)
            {
                dtCustomerDateOfBirth.DateSelected = lst[0].BirthDate.Value;
            }

            ddlCustomerTitle.DoLoadDropdownList();
            ddlCustomerSex.DoLoadDropdownList();
            ddlMaritalStatus.DoLoadDropdownList();
            ddlJobType.DoLoadDropdownList();

            foreach (PersonCard itm in lst[0].PersonCard)
            {
                if (itm.PersonCardType_ID == 2) //บัตรประชาชน
                {
                    txtCustomerIDCardNumber.Text = itm.PersonCardDetail;
                }

                if (itm.PersonCardType_ID == 3) //Passport
                {
                    txtCustomerPassportNumber.Text = itm.PersonCardDetail;
                }
            }

            foreach (PersonContact itm in lst[0].PersonContact)
            {
                if (itm.ContactType_ID == 2) //โทรศัพท์
                {
                    txtCustomerPhoneNumber.Text = itm.ContactDetail;
                }

                if (itm.ContactType_ID == 3) //มือถือ
                {
                    txtCustomerMobileNumber.Text = itm.ContactDetail;
                }
            }

            ddlCustomerTitle.Title_ID = lst[0].Title_ID;
            ddlCustomerSex.SexID = lst[0].Sex_ID;
            ddlMaritalStatus.MaritalStatus_ID = lst[0].MaritalStatus_ID;
            ddlJobType.Occupation_ID = lst[0].Occupation_ID;
            ddlJobType_SelectChanged(ddlJobType.Occupation_ID, new EventArgs());
            ddlJobTypeLevel.OccupationLevel_ID = lst[0].OccupationLevel_ID;
        }

        /// <summary>
        /// Set Enable Text Box And DDL
        /// </summary>
        /// <param name="value">True/False</param>
        public void IsEnabled(bool value)
        {
            // txt
            txtCustomerName.Enabled = value;
            txtCustomerSurename.Enabled = value;
            txtCustomerIDCardNumber.Enabled = value;
            txtCustomerPassportNumber.Enabled = value;
            txtCustomerPhoneNumber.Enabled = value;
            txtCustomerMobileNumber.Enabled = value;

            // ddl
            ddlCustomerTitle.IsEnabled(value);
            ddlCustomerSex.IsEnabled(value);
            ddlJobType.IsEnabled(value);
            ddlJobTypeLevel.IsEnabled(value);
            ddlMaritalStatus.IsEnabled(value);

            // dtp
            dtCustomerDateOfBirth.IsEnabled(value);
        }

        /// <summary>
        /// Do Clear Text
        /// </summary>
        public void DoClear()
        {
            txtCustomerIDCardNumber.Text = string.Empty;
            txtCustomerMobileNumber.Text = string.Empty;
            txtCustomerName.Text = string.Empty;
            txtCustomerPassportNumber.Text = string.Empty;
            txtCustomerPhoneNumber.Text = string.Empty;
            txtCustomerSurename.Text = string.Empty;
            dtCustomerDateOfBirth.DoClear();

            btnEditPerson.Attributes.Clear();
            btnEditPerson.Enabled = false;
        }

        /// <summary>
        /// Set Alert Warning
        /// </summary>
        /// <param name="val">True/False</param>
        public void SetAlertWarning(bool val)
        {
            if (val == true)
            {
                txtCustomerIDCardNumber.CssClass = "input-sm-danger";
                txtCustomerIDCardNumber.Focus();
            }
            else
            {
                txtCustomerIDCardNumber.CssClass = "form-control input-sm";
            }
        }

        /// <summary>
        /// Get ID Card From Text Box
        /// </summary>
        /// <returns></returns>
        public string GetCardDetail()
        {
            string result = txtCustomerIDCardNumber.Text.Trim();
            return result;
        }

        /// <summary>
        /// Get Passpord Card From Text Box
        /// </summary>
        /// <returns></returns>
        public string GetCardPassport()
        {
            string result = txtCustomerPassportNumber.Text.Trim();
            return result;
        }

        /// <summary>
        /// Validate Identity Card
        /// </summary>
        /// <returns></returns>
        public bool IsValidateIdentityCard()
        {
            FunctionManager fm = new FunctionManager();

            // Check Identity Card From Class
            if (fm.IsValidateIdentityCard(txtCustomerIDCardNumber.Text.Trim()) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Validate Customer
        /// </summary>
        /// <returns></returns>
        public string ValidateCustomer()
        {
            TransactionManager tm = new TransactionManager();
            string result;

            // Valid Custommer From Class
            result = tm.Validate_Person(Person);

            return result;
        }

        /// <summary>
        /// Validate Birth Date
        /// </summary>
        /// <returns></returns>
        public bool IsValidateBirthDate(bool IsRequire = false)
        {
            bool result;

            //require
            dtCustomerDateOfBirth.IsRequired = IsRequire;

            // Validate Date
            result = dtCustomerDateOfBirth.IsValidate();

            if (dtCustomerDateOfBirth.DateSelected >= DateTime.Today)
            {
                result = false;
            }

            return result;
        }

        public string ValidateButtonEditPerson(int personID, int userLoginID, int? motorappID = null)
        {
            // check usp validate
            TransactionManager tr = new TransactionManager();

            string result = tr.Validate_PersonMotorApplication_ForEdit(personID, userLoginID, motorappID);

            if (result == string.Empty)
            {
                string EncryptPersonID = cFunction.Base64Encrypt(Convert.ToString(personID));
                string EncryptMotorappID = string.Empty;

                if (motorappID != null)
                {
                    EncryptMotorappID = cFunction.Base64Encrypt(Convert.ToString(motorappID));
                }

                //Enable button
                btnEditPerson.Enabled = true;

                //set attribute open new tab
                btnEditPerson.Attributes.Add("onclick", "window.open('" + "frmManagePersonAddEdit.aspx?" + "personid=" + EncryptPersonID + "&motorid=" + EncryptMotorappID + "')");
            }

            return result;
        }

        public void DisableCardId(bool value)
        {
            txtCustomerIDCardNumber.Enabled = value;
        }

        public void DisablePassportId(bool value)
        {
            txtCustomerPassportNumber.Enabled = value;
        }

        #endregion Method
    }
}