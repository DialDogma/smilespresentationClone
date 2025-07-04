using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;
using System.Drawing;
using System.Data;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucAppDetailCustomer : System.Web.UI.UserControl
    {
        #region Declare

        private MotorApplication _motorApplication;

        private Person _person;

        private int personID;

        /// <summary>
        /// Property Get/Set Motor Application
        /// </summary>
        public MotorApplication MotorApplication
        {
            get
            {
                return _motorApplication;
            }
            set
            {
                _motorApplication = value;
            }
        }

        public Person Person
        {
            get
            {
                return _person;
            }
            set
            {
                _person = value;
            }
        }

        /// <summary>
        /// Property Get/Set Button Edit Person
        /// </summary>
        public Button ButtonEditPerson
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
            // Set Button CSS Style
            btnEditPerson.CssClass = "btn btn-warning btn-xs";
        }

        protected void btnshowmodalHistory_Click(object sender, EventArgs e)
        {
            // Show Modal
            ModalHistory.Show();
        }

        protected void dgvHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ////ส่ง query string
                string ptran_id = dgvHistory.DataKeys[e.Row.RowIndex].Values[0].ToString();

                ////EncryptID
                FunctionManager fm = new FunctionManager();

                string EncryptID = fm.Base64Encrypt(ptran_id);

                e.Row.Attributes.Add("onclick", "window.open('" + "frmLogDetail.aspx?" + "ptranid=" + EncryptID + "')");
                e.Row.ToolTip = "คลิกเพื่อเปิดดูรายละเอียด";
            }
        }

        protected void dgvHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in dgvHistory.Rows)
            {
                if (row.RowIndex == dgvHistory.SelectedIndex)
                {
                    row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    row.ToolTip = "คลิกเพื่อเปิดดูรายละเอียด";
                }
            }
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Load Gridview
        /// </summary>
        /// <param name="motorapplicationID">Motor Application ID</param>
        private void LoadGridView(int motorapplicationID)
        {
            MotorDB mdb = new MotorDB();
            FunctionManager fm = new FunctionManager();

            // Load Gridview
            fm.LoadGridview(dgvMain, mdb.GetMotorAppContactByAppId_PersonRelatedAppTypeId_ContactTypeId(motorapplicationID, (int)MappingCode.ePersonRelatedApplicationType.Customer, null));
        }

        /// <summary>
        /// Do Load บุคคลธรรมดา
        /// </summary>
        /// <param name="_motorApplication_ID">Motor Application ID</param>
        public void DoLoad(int _motorApplication_ID)
        {
            FunctionManager fm = new FunctionManager();
            MotorDB db = new MotorDB();

            // Get Motor Application Detail
            _motorApplication = db.GetMotorApplicationByMotorApplicationID(_motorApplication_ID);

            switch (_motorApplication.Customer.Person.PersonType_ID)
            {
                case (int)MappingCode.ePersonType.Individual: //บุคคลธรรมดา
                    BindDataForIndividual(_motorApplication);
                    panCorporate.Visible = false;
                    break;

                case (int)MappingCode.ePersonType.Corporate: //นิติฯ
                    BindDataForCorporate(_motorApplication);
                    panCustomer.Visible = false;
                    break;

                default:
                    break;
            }

            LoadGridView(_motorApplication_ID);
            LoadGridViewHistory(_motorApplication.Customer.Person.Person_ID);

            // Set Relate
            PersonRelate person = new PersonRelate();
            person = db.GetPersonRelateByMotorApplicationID(_motorApplication_ID, (int)MappingCode.ePersonRelatedApplicationType.Customer);
            int person_ID = person.Person_ID;

            personID = person_ID;
            string EncryptPersonID = cFunction.Base64Encrypt(Convert.ToString(person_ID));

            // Set Att Button
            btnEditPerson.Attributes.Add("onclick", "window.open('" + "frmManagePersonAddEdit.aspx?" + "personid=" + EncryptPersonID + "')");
        }

        /// <summary>
        /// Do Load by PersonId
        /// </summary>
        /// <param name="_person_ID"></param>
        public void DoLoadByPersonId(int _person_ID)
        {
            FunctionManager fm = new FunctionManager();
            DataCenterDB db = new DataCenterDB();

            _person = db.GetPersonByPersonID(_person_ID).FirstOrDefault();

            switch (_person.PersonType_ID)
            {
                case 2: //บุคคลธรรมดา
                    BindDataForIndividualByPersonId(_person);
                    panCustomer.Visible = true;
                    panCorporate.Visible = false;
                    break;

                case 3: //นิติฯ
                    BindDataForCorporateByPersonId(_person);
                    panCustomer.Visible = false;
                    panCorporate.Visible = true;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// ข้อมูลบุคคลธรรมดา
        /// </summary>
        /// <param name="_motorApplication"></param>
        private void BindDataForIndividual(MotorApplication _motorApplication)
        {
            MotorDB db = new MotorDB();
            // Set Bind Motor Application Detail
            //lblID.Text = _motorApplication.Customer.Person.Person_ID.ToString();

            lblTextHeader.Text = "ข้อมูลผู้เอาประกัน";
            lblFullname.ToolTip = "ID : " + _motorApplication.Customer.Person.Person_ID.ToString();
            lblFullname.Text = _motorApplication.Customer.Person.TitleDetail + _motorApplication.Customer.Person.FirstName + " " + _motorApplication.Customer.Person.LastName;
            lblSex.Text = _motorApplication.Customer.Person.SexDetail;
            int _motorApplication_ID = _motorApplication.MotorApplication_ID;

            // Set Person Card
            foreach (PersonCard itm in _motorApplication.Customer.Person.PersonCard)
            {
                if (itm.PersonCardType_ID == (int)MappingCode.ePersonCardType.IdentityNumber) //บัตรประชาชน
                {
                    lblCardNumber.Text = itm.PersonCardDetail;
                }
                if (itm.PersonCardType_ID == (int)MappingCode.ePersonCardType.Passport) //Passport
                {
                    lblPassportNumber.Text = itm.PersonCardDetail;
                }
            }

            // วันเกิด
            if (_motorApplication.Customer.Person.BirthDate.HasValue == true)
            {
                lblBirthDate.Text = _motorApplication.Customer.Person.BirthDate.Value.ToShortDateString();
            }
            else
            {
                lblBirthDate.Text = string.Empty;
            }

            lblMaritalStatusDetail.Text = _motorApplication.Customer.Person.MaritalStatusDetail;
            lblOccupationDetail.Text = _motorApplication.Customer.Person.OccupationDetail;
            lblOccupationLevelDetail.Text = _motorApplication.Customer.Person.OccupationLevelDetail;
        }

        /// <summary>
        /// ข้อมูลนิติบุคคล
        /// </summary>
        /// <param name="_motorApplication"></param>
        private void BindDataForCorporate(MotorApplication _motorApplication)
        {
            MotorDB db = new MotorDB();
            // Set Bind Motor Application Detail
            lblTextHeader.Text = "ข้อมูลผู้เอาประกัน / นิติบุคคล";
            int _motorApplication_ID = _motorApplication.MotorApplication_ID;

            // Bind Data
            lblFullCorporateName.ToolTip = "ID : " + _motorApplication.Customer.Person.Person_ID.ToString();
            lblFullCorporateName.Text = _motorApplication.Customer.Person.TitleDetail.Replace("รอข้อมูล", "") + _motorApplication.Customer.Person.FirstName;

            foreach (PersonCard itm in _motorApplication.Customer.Person.PersonCard)
            {
                if (itm.PersonCardType_ID == (int)MappingCode.ePersonCardType.TaxpayerNo) //บัตรประชาชน
                {
                    lblTaxpayerNo.Text = itm.PersonCardDetail;
                }
            }
        }

        private void BindDataForIndividualByPersonId(Person _person)
        {
            // Set Bind Motor Application Detail
            //lblID.Text = _motorApplication.Customer.Person.Person_ID.ToString();

            lblTextHeader.Text = "ข้อมูลผู้เอาประกัน";
            lblFullname.ToolTip = "ID : " + _person.Person_ID.ToString();
            lblFullname.Text = _person.TitleDetail + _person.FirstName + " " + _person.LastName;
            lblSex.Text = _person.SexDetail;
            int _motorApplication_ID = _person.Person_ID;

            // Set Person Card
            foreach (PersonCard itm in _person.PersonCard)
            {
                if (itm.PersonCardType_ID == 2) //บัตรประชาชน
                {
                    lblCardNumber.Text = itm.PersonCardDetail;
                }
                if (itm.PersonCardType_ID == 3) //Passport
                {
                    lblPassportNumber.Text = itm.PersonCardDetail;
                }
            }

            // วันเกิด
            if (_person.BirthDate.HasValue == true)
            {
                lblBirthDate.Text = _person.BirthDate.Value.ToShortDateString();
            }
            else
            {
                lblBirthDate.Text = string.Empty;
            }

            lblMaritalStatusDetail.Text = _person.MaritalStatusDetail;
            lblOccupationDetail.Text = _person.OccupationDetail;
            lblOccupationLevelDetail.Text = _person.OccupationLevelDetail;
        }

        private void BindDataForCorporateByPersonId(Person _person)
        {
            DataCenterDB dataCenterDB = new DataCenterDB();
            // Set Bind Motor Application Detail
            lblTextHeader.Text = "ข้อมูลผู้เอาประกัน / นิติบุคคล";
            int _person_ID = _person.Person_ID;

            // Bind Data
            lblFullCorporateName.ToolTip = "ID : " + _person.Person_ID.ToString();
            lblFullCorporateName.Text = _person.TitleDetail + _person.FirstName;

            foreach (PersonCard itm in _person.PersonCard)
            {
                if (itm.PersonCardType_ID == 4) //เลขประจำตัวผู้เสียภาษี
                {
                    lblTaxpayerNo.Text = itm.PersonCardDetail;
                }
            }
        }

        //load gridview modal history
        public void LoadGridViewHistory(int? PersonID)
        {
            DataCenterDB db = new DataCenterDB();
            DataTable dt = new DataTable();
            FunctionManager fm = new FunctionManager();

            dt = db.GetPersonTransaction(PersonID);

            fm.LoadGridview(dgvHistory, dt, "PersonTransaction_ID");
        }

        public void DisableButtonEditAndAddress(bool value)
        {
            btnEditPerson.Visible = value;
            btnshowmodalHistory.Visible = value;
            dgvMain.Visible = value;
        }

        public int GetCustomerID(int motorApplication_ID)
        {
            PersonRelate person = new PersonRelate();
            MotorDB db = new MotorDB();
            person = db.GetPersonRelateByMotorApplicationID(motorApplication_ID, (int)MappingCode.ePersonRelatedApplicationType.Customer);
            int person_ID = person.Person_ID;
            return person_ID;
        }

        #endregion Method
    }
}