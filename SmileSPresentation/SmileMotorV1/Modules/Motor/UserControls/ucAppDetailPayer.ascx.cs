using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;
using System.Data;
using System.Drawing;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucAppDetailPayer : System.Web.UI.UserControl
    {
        #region Declare

        private MotorApplication _motorApplication;

        /// <summary>
        /// Property Get/Set Motor Application Class
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

        /// <summary>
        /// Property Get/Set Button Edie Person
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

        protected void dgvHistory_SelectedIndexChanged1(object sender, EventArgs e)
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

        protected void dgvHistory_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ////ส่ง query string
                string ptran_id = dgvHistory.DataKeys[e.Row.RowIndex].Values[0].ToString();

                ////EncryptID
                FunctionManager fm = new FunctionManager();

                string EncryptID = fm.Base64Encrypt(ptran_id);

                //
                e.Row.Attributes.Add("onclick", "window.open('" + "frmLogDetail.aspx?" + "ptranid=" + EncryptID + "')");
                e.Row.ToolTip = "คลิกเพื่อเปิดดูรายละเอียด";
            }
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Load Gridview Payer Contact
        /// </summary>
        /// <param name="_motorApplication_ID">Motor Application ID</param>
        /// <param name="_personRelateAppID">Person Relate ID</param>
        private void LoadGridView(int _motorApplication_ID, int _personRelateAppID)
        {
            MotorDB db = new MotorDB();
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();

            // Get Motor Application Contact
            dt = db.GetMotorAppContactByAppId_PersonRelatedAppTypeId_ContactTypeId(_motorApplication_ID, _personRelateAppID);

            // Load Gridview
            fm.LoadGridview(dgvMain, dt);
        }

        /// <summary>
        /// Do Load Payer Detail
        /// </summary>
        /// <param name="_motorApplication_ID"></param>
        public void DoLoad(int _motorApplication_ID)
        {
            MotorDB mdb = new MotorDB();
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();

            // Set Defualt Person Relate เท่ากับ ผู้ชำระเบี้ย
            int _personRelateAppID = 3;

            // Get Motor Application Detail
            _motorApplication = mdb.GetMotorApplicationByMotorApplicationID(_motorApplication_ID);

            switch (_motorApplication.Payer.Person.PersonType_ID)
            {
                case (int)MappingCode.ePersonType.Individual:   //บุคคลธรรมดา ให้ Bind ตามฟิลด์ของบุคคลธรรมดา

                    BindDataForCustomer(_motorApplication);
                    PnlCorporate.Visible = false;
                    //tblCorporate.Visible = false;
                    break;

                case (int)MappingCode.ePersonType.Corporate:     //นิติ ให้ Bind ตามฟิลด์ของนิติ
                    BindDataForCorporate(_motorApplication);
                    PnlCustomer.Visible = false;
                    //tblCustomer.Visible = false;
                    break;

                default:
                    break;
            }

            // Load Gridview
            LoadGridView(_motorApplication_ID, _personRelateAppID);
            //load gridview modalhistory
            LoadGridViewHistory(_motorApplication.Payer.Person.Person_ID);

            // Set Relate
            PersonRelate person = new PersonRelate();
            person = mdb.GetPersonRelateByMotorApplicationID(_motorApplication_ID, 3);
            int person_ID = person.Person_ID;
            string EncryptPersonID = cFunction.Base64Encrypt(Convert.ToString(person_ID));

            // Set Att
            btnEditPerson.Attributes.Add("onclick", "window.open('" + "frmManagePersonAddEdit.aspx?" + "personid=" + EncryptPersonID + "')");
        }

        private void BindDataForCustomer(MotorApplication _motorApplication)
        {
            DataCenterDB db = new DataCenterDB();

            // Get Person Relate
            PersonRelation personRelation = new PersonRelation();
            personRelation = db.GetPersonRelation(_motorApplication.Payer.Person_ID, _motorApplication.Customer.Person_ID);

            // Set Bind Detail
            //lblID.Text = _motorApplication.Payer.Person.Person_ID.ToString();
            lblFullName.ToolTip = "ID : " + _motorApplication.Payer.Person.Person_ID.ToString();
            lblFullName.Text = _motorApplication.Payer.Person.TitleDetail + _motorApplication.Payer.Person.FirstName + " " + _motorApplication.Payer.Person.LastName;
            lblPayerSex.Text = _motorApplication.Payer.Person.SexDetail;
            //lblPersonRelationTypeDetail.Text = personRelation.PersonRelationType;
            lblRelationTypeDetail.Text = "(ความสัมพันธ์กับผู้เอาประกัน : " + personRelation.PersonRelationType + ")";

            // Set Person Card
            foreach (PersonCard itm in _motorApplication.Payer.Person.PersonCard)
            {
                if (itm.PersonCardType_ID == 2) //บัตรประชาชน
                {
                    lblPayerCardNumber.Text = itm.PersonCardDetail;
                }
                if (itm.PersonCardType_ID == 3) //Passport
                {
                    lblPayerPassportNumber.Text = itm.PersonCardDetail;
                }
            }

            // วันเกิด
            if (_motorApplication.Payer.Person.BirthDate.HasValue == true)
            {
                lblPayerBirthDate.Text = _motorApplication.Payer.Person.BirthDate.Value.ToShortDateString();
            }
            else
            {
                lblPayerBirthDate.Text = string.Empty;
            }

            lblPayerStatus.Text = _motorApplication.Payer.Person.MaritalStatusDetail;
            lblPayerJobType.Text = _motorApplication.Payer.Person.OccupationDetail;
            lblPayerJobDetail.Text = _motorApplication.Payer.Person.OccupationLevelDetail;
        }

        private void BindDataForCorporate(MotorApplication _motorApplication)
        {
            DataCenterDB db = new DataCenterDB();

            // Bind Data
            lblFullCorporateName.ToolTip = "ID : " + _motorApplication.Payer.Person.Person_ID.ToString();
            lblFullCorporateName.Text = _motorApplication.Payer.Person.TitleDetail.Replace("รอข้อมูล", "") + _motorApplication.Payer.Person.FirstName;

            // Get Person Relate
            PersonRelation personRelation = new PersonRelation();
            personRelation = db.GetPersonRelation(_motorApplication.Payer.Person_ID, _motorApplication.Customer.Person_ID);

            lblRelationTypeDetail.Text = "(ความสัมพันธ์กับผู้เอาประกัน : " + personRelation.PersonRelationType + ")";

            foreach (PersonCard itm in _motorApplication.Payer.Person.PersonCard)
            {
                if (itm.PersonCardType_ID == (int)MappingCode.ePersonCardType.TaxpayerNo) //บัตรประชาชน
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

        public int GetPayerID(int motorApplication_ID)
        {
            PersonRelate person = new PersonRelate();
            MotorDB db = new MotorDB();
            person = db.GetPersonRelateByMotorApplicationID(motorApplication_ID, (int)MappingCode.ePersonRelatedApplicationType.Payer);
            int person_ID = person.Person_ID;
            return person_ID;
        }

        #endregion Method
    }
}