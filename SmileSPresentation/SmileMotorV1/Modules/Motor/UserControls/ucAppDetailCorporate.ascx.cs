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
    public partial class ucAppDetailCorporate : System.Web.UI.UserControl
    {

        #region Declare
        private MotorApplication _motorApplication;

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
        #endregion


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

        #endregion


        #region Method

        /// <summary>
        /// Do Load Detail
        /// </summary>
        public void DoLoad(int _motorApplication_ID, int PersonRelatedApplicationTypeID)
        {
            MotorDB mdb = new MotorDB();
            DataCenterDB db = new DataCenterDB();


            // Get Detail From Person
            _motorApplication = mdb.GetMotorApplicationByMotorApplicationID(_motorApplication_ID);

            switch(PersonRelatedApplicationTypeID)
            {
                case (int)MappingCode.ePersonRelatedApplicationType.Customer:   //ผู้เอาประกัน

                    lblTextHeader.Text = "ข้อมูลผู้เอาประกัน / นิติบุคคล";
                    btnEditPerson.Text = "แก้ไขข้อมูลลูกค้า";
                    // Bind Data
                    lblFullCorporateName.ToolTip = "ID : " + _motorApplication.Customer.Person.Person_ID.ToString();
                    lblFullCorporateName.Text = _motorApplication.Customer.Person.TitleDetail + _motorApplication.Customer.Person.FirstName;

                    foreach (PersonCard itm in _motorApplication.Customer.Person.PersonCard)
                    {
                        if (itm.PersonCardType_ID == (int)MappingCode.ePersonCardType.TaxpayerNo) //บัตรประชาชน
                        {
                            lblTaxpayerNo.Text = itm.PersonCardDetail;
                        }
                    }

                    DoLoadGridView(_motorApplication_ID, PersonRelatedApplicationTypeID);
                    LoadGridViewHistory(_motorApplication.Customer.Person.Person_ID);

                    // Set Relate
                    PersonRelate person = new PersonRelate();
                    person = mdb.GetPersonRelateByMotorApplicationID(_motorApplication_ID, (int)MappingCode.ePersonRelatedApplicationType.Customer);
                    int person_ID = person.Person_ID;
                    string EncryptPersonID = cFunction.Base64Encrypt(Convert.ToString(person_ID));

                    // Set Att Button
                    btnEditPerson.Attributes.Add("onclick", "window.open('" + "frmManagePersonAddEdit.aspx?" + "personid=" + EncryptPersonID + "')");
                    break;

                case (int)MappingCode.ePersonRelatedApplicationType.Payer:  //ผู้ชำระเบี้ย

                    lblTextHeader.Text = "ข้อมูลผู้ชำระเบี้ย";
                    btnEditPerson.Text = "แก้ไขข้อมูลผู้ชำระเบี้ย";
                    // Bind Data
                    lblFullCorporateName.ToolTip = "ID : " + _motorApplication.Payer.Person.Person_ID.ToString();
                    lblFullCorporateName.Text = _motorApplication.Payer.Person.TitleDetail + _motorApplication.Payer.Person.FirstName;

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


                    DoLoadGridView(_motorApplication_ID, PersonRelatedApplicationTypeID);
                    LoadGridViewHistory(_motorApplication.Payer.Person.Person_ID);

                    // Set Relate
                    PersonRelate person1 = new PersonRelate();
                    person1 = mdb.GetPersonRelateByMotorApplicationID(_motorApplication_ID, PersonRelatedApplicationTypeID);
                    int person_ID1 = person1.Person_ID;
                    string EncryptPersonID1 = cFunction.Base64Encrypt(Convert.ToString(person_ID1));

                    // Set Att Button
                    btnEditPerson.Attributes.Add("onclick", "window.open('" + "frmManagePersonAddEdit.aspx?" + "personid=" + EncryptPersonID1 + "')");
                    break;
                default:
                    break;
            }

            

        }

        public void DoLoadGridView(int motorapplicationID,int PersonRelatedApplicationTypeID)
        {
            MotorDB mdb = new MotorDB();
            FunctionManager fm = new FunctionManager();

            // Load Gridview
            fm.LoadGridview(dgvMain, mdb.GetMotorAppContactByAppId_PersonRelatedAppTypeId_ContactTypeId(motorapplicationID, PersonRelatedApplicationTypeID, null));
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
        #endregion

    }
}