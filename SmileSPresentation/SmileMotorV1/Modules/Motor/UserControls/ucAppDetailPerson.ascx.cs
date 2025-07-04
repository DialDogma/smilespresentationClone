using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucAppDetailPerson : System.Web.UI.UserControl
    {
        #region Declare
        private MotorApplication _motorApplication;

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

            btnEditPerson.CssClass = "btn btn-warning btn-xs";
        }

        protected void btnshowmodalHistory_Click(object sender, EventArgs e)
        {
            ModalHistory.Show();
        }
        #endregion

        #region Method

        public void DoLoad(int personID)
        {

            DataCenterDB db = new DataCenterDB();
            List<Person> lst = new List<Person>();
            
            lst = db.GetPersonByPersonID(personID).ToList();
           
            //lblID.Text = _motorApplication.Customer.Person.Person_ID.ToString();
            lblFullname.ToolTip = "ID : " + lst[0].Person_ID.ToString();
            lblFullname.Text = lst[0].FullName;
            lblSex.Text = lst[0].SexDetail;

            foreach (PersonCard itm in lst[0].PersonCard)
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

            foreach (PersonContact itm in lst[0].PersonContact)
            {
                if (itm.ContactType_ID == (int)MappingCode.ePersonContactType.Phone) 
                {
                    lblPhoneNumber.Text = itm.ContactDetail;
                }
                if (itm.ContactType_ID == (int)MappingCode.ePersonContactType.Mobile)
                {
                    lblMobileNumber.Text = itm.ContactDetail;
                }
            }

            // วันเกิด
            if (lst[0].BirthDate.HasValue == true)
            {
                lblBirthDate.Text = lst[0].BirthDate.Value.ToShortDateString();
            }
            else
            {
                lblBirthDate.Text = string.Empty;
            }
            
            lblMaritalStatusDetail.Text = lst[0].MaritalStatusDetail;
            lblOccupationDetail.Text = lst[0].OccupationDetail;
            lblOccupationLevelDetail.Text = lst[0].OccupationLevelDetail;
     
            
            string EncryptPersonID = cFunction.Base64Encrypt(personID.ToString());

            btnEditPerson.Attributes.Add("onclick", "window.open('" + "frmManagePersonAddEdit.aspx?" + "personid=" + EncryptPersonID + "')");
        }
        #endregion


    }
}