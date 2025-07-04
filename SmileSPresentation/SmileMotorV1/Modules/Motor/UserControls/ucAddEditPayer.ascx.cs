using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucAddEditPayer : System.Web.UI.UserControl
    {
        #region Declare
        private Person _person;
        private int _relationType_ID;
        private string _relationTypeDetail;

        /// <summary>
        /// Property With Person Class
        /// </summary>
        public Person Person
        {
            get
            {
                // Person Detail
                _person = new Person();
                _person.Title_ID = ddlPayerTitle.Title_ID;
                _person.FirstName = txtPayerName.Text.Trim();
                _person.LastName = txtPayerSurename.Text.Trim();
                _person.BirthDate = dtPayerDateOfBirth.DateSelected;
                _person.Sex_ID = ddlPayerSex.SexID;
                _person.MaritalStatus_ID = ddlMaritalStatus.MaritalStatus_ID;
                _person.Occupation_ID = ddlPayerJobType.Occupation_ID;
                _person.OccupationLevel_ID = ddlJobTypeLevel.OccupationLevel_ID;

                //PersonCard
                PersonCard _identityCard = new PersonCard();
                PersonCard _passport = new PersonCard();
                PersonCard _taxpayerNO = new PersonCard();

                _identityCard.PersonCardType_ID = 2;
                _identityCard.PersonCardDetail = txtPayerIDCardNumber.Text.Trim();

                _passport.PersonCardType_ID = 3;
                _passport.PersonCardDetail = txtPayerPassportNumber.Text.Trim();

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
                _phone.ContactDetail = txtPayerPhoneNumber.Text.Trim();

                _mobile.ContactType_ID = 3;
                _mobile.ContactDetail = txtPayerMobileNumber.Text.Trim();

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

        /// <summary>
        /// Property Relation Type ID
        /// </summary>
        public int RelationType_
        {
            get
            {
                //TODO : รับค่าจาก ddl
                return _relationType_ID;
            }
            set
            {
                //TODO : รับค่าจาก ddl
                _relationType_ID = value;
            }
        }

        /// <summary>
        /// Property Relation Type Detail
        /// </summary>
        public string RelationTypeDetail
        {
            get
            {
                //TODO : รับค่าจาก ddl
                return _relationTypeDetail;
            }
            set
            {
                //TODO : รับค่าจาก ddl
                _relationTypeDetail = value;
            }
        }

        //ตัวอย่างนะจ๊ะ 
        public TextBox Txttest
        {
            get
            {
                return txtPayerName;
            }

            set
            {
               txtPayerName = value;
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

        #endregion

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                // Set Auto Postback DDL
                this.ddlPayerJobType.AutoPostback = true;
                this.ddlJobTypeLevel.AutoPostback = true;
            }

            // Set Style Upper Case TXT Passport
            txtPayerPassportNumber.CssClass = "form-control input-sm uppercase";
            // Set Button CSS Style
            btnEditPerson.CssClass = "btn btn-warning btn-xs";
        }

        protected void ddlPayerJobType_SelectChanged(object sender, EventArgs e)
        {
            // Set And Load DDL
            this.ddlJobTypeLevel.AutoPostback = true;
            ddlJobTypeLevel.DoLoadDropdownlist(ddlPayerJobType.Occupation_ID);
        }

        #endregion

        #region Method

        /// <summary>
        /// Do Load Dropdownlist
        /// </summary>
        public void DoLoadDropdownList()
        {
            // Load DDL
            ddlPayerTitle.DoLoadDropdownList();
            ddlPayerSex.DoLoadDropdownList();
            ddlMaritalStatus.DoLoadDropdownList();
            ddlPayerJobType.DoLoadDropdownList();
        }

        /// <summary>
        /// Do Load Detail / Bind Data To Field
        /// </summary>
        public void DoloadDetail(int _person_ID)
        {
            DataCenterDB db = new DataCenterDB();
            List<Person> lst = new List<Person>();

            // Get Person
            lst = db.GetPersonByPersonID(_person_ID);

            txtPayerName.Text = lst[0].FirstName;
            txtPayerSurename.Text = lst[0].LastName;

            // วันเกิด
            if (lst[0].BirthDate.HasValue == true)
            {
                dtPayerDateOfBirth.DateSelected = lst[0].BirthDate.Value;
            }

            ddlPayerTitle.DoLoadDropdownList();
            ddlPayerSex.DoLoadDropdownList();
            ddlMaritalStatus.DoLoadDropdownList();
            ddlPayerJobType.DoLoadDropdownList();

            // Get Person Card
            foreach (PersonCard itm in lst[0].PersonCard)
            {
                if (itm.PersonCardType_ID == 2) //บัตรประชาชน
                {
                    txtPayerIDCardNumber.Text = itm.PersonCardDetail;
                }

                if (itm.PersonCardType_ID == 3) //Passport
                {
                    txtPayerPassportNumber.Text = itm.PersonCardDetail;
                }
            }

            // Get Person Contact
            foreach (PersonContact itm in lst[0].PersonContact)
            {
                if (itm.ContactType_ID == 2) //โทรศัพท์
                {
                    txtPayerPhoneNumber.Text = itm.ContactDetail;
                }

                if (itm.ContactType_ID == 3) //มือถือ
                {
                    txtPayerMobileNumber.Text = itm.ContactDetail;
                }
            }

            // Bind Detail
            ddlPayerTitle.Title_ID = lst[0].Title_ID;
            ddlPayerSex.SexID = lst[0].Sex_ID;
            ddlMaritalStatus.MaritalStatus_ID = lst[0].MaritalStatus_ID;
            ddlPayerJobType.Occupation_ID = lst[0].Occupation_ID;
            ddlPayerJobType_SelectChanged(ddlPayerJobType.Occupation_ID, new EventArgs());
            ddlJobTypeLevel.OccupationLevel_ID = lst[0].OccupationLevel_ID;
        }

        /// <summary>
        /// Do Clear Data Field
        /// </summary>
        public void DoClear()
        {
            // Clear Text
            ddlPayerTitle.DoLoadDropdownList();
            txtPayerName.Text = "";
            txtPayerSurename.Text = "";
            ddlPayerSex.DoLoadDropdownList();
            txtPayerIDCardNumber.Text = "";
            txtPayerPassportNumber.Text = "";
            ddlMaritalStatus.DoLoadDropdownList();
            txtPayerPhoneNumber.Text = "";
            txtPayerMobileNumber.Text = "";
            ddlPayerJobType.DoLoadDropdownList();
            ddlJobTypeLevel.DoLoadDropdownList();
            dtPayerDateOfBirth.DoClear();

            btnEditPerson.Attributes.Clear();
            btnEditPerson.Enabled = false;
        }

        /// <summary>
        /// Set Enable
        /// </summary>
        /// <param name="value">True/False</param>
        public void IsEnabled(bool value)
        {
            ddlPayerTitle.IsEnabled(value);
            ddlPayerSex.IsEnabled(value);
            ddlMaritalStatus.IsEnabled(value);
            ddlPayerJobType.IsEnabled(value);
            ddlJobTypeLevel.IsEnabled(value);
            dtPayerDateOfBirth.IsEnabled(value);

            txtPayerName.Enabled = value;
            txtPayerSurename.Enabled = value;
            txtPayerIDCardNumber.Enabled = value;
            txtPayerPassportNumber.Enabled = value;
            txtPayerPhoneNumber.Enabled = value;
            txtPayerMobileNumber.Enabled = value;
        }

        /// <summary>
        /// Set Alert Warning
        /// </summary>
        /// <param name="val">True/False</param>
        public void SetAlertWarning(bool val)
        {
            // Set CSS Style
            if (val == true)
            {
                txtPayerIDCardNumber.CssClass = "input-sm-danger";
                txtPayerIDCardNumber.Focus();
            }
            else
            {
                txtPayerIDCardNumber.CssClass = "form-control input-sm";
            }

        }

        /// <summary>
        /// Get Card Detail
        /// </summary>
        /// <returns></returns>
        public string GetCardDetail()
        {
            string result = txtPayerIDCardNumber.Text.Trim();
            return result;
        }

        /// <summary>
        /// Get Card Password
        /// </summary>
        /// <returns></returns>
        public string GetCardPassport()
        {
            string result = txtPayerPassportNumber.Text.Trim();
            return result;
        }

        /// <summary>
        /// Validate Identity Card
        /// </summary>
        /// <returns></returns>
        public bool IsValidateIdentityCard()
        {
            FunctionManager fm = new FunctionManager();

            // valid
            if (fm.IsValidateIdentityCard(txtPayerIDCardNumber.Text.Trim()) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Validate Payer
        /// </summary>
        /// <returns></returns>
        public string ValidatePayer()
        {
            TransactionManager tm = new TransactionManager();
            string result;

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
            dtPayerDateOfBirth.IsRequired = IsRequire;

            result = dtPayerDateOfBirth.IsValidate();


            // ถ้า Birth Date เท่ากับวันนี้
            if (dtPayerDateOfBirth.DateSelected >= DateTime.Today )
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
        #endregion   
    }
}