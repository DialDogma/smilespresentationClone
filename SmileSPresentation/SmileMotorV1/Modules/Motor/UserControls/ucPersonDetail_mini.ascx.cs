using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucPersonDetail_mini : System.Web.UI.UserControl
    {

        #region Declare
        private Person _person;

        /// <summary>
        /// Set Enum Relate Type Text
        /// </summary>
        public enum PersonRelateTypeText
        {
            ผู้เอาประกัน,
            ผู้ชำระเบี้ย
        }

        /// <summary>
        /// Property Get/Set Person Class
        /// </summary>
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
        /// Do Load Detail
        /// </summary>
        /// <param name="personID">Person ID</param>
        public void DoLoad(int personID)
        {
            // ถ้า Person ไม่เท่ากับ 0
            if (personID != 0)
            {
                DataCenterDB db = new DataCenterDB();
                _person = new Person();

                // Get Person
                _person = db.GetPersonByPersonID(personID).Single();

                // Set Detail To Label
                //lblPersonID.Text = _person.Person_ID.ToString();
                lblFullName.Text = _person.TitleDetail + _person.FirstName + " " + _person.LastName;
            }
            else
            {
                //lblPersonID.Text = "0";
                lblFullName.Text = "ไม่พบข้อมูล";
            }           
        }

        //public void SetGroupingText(PersonRelateTypeText txt)
        //{
        //    this.Panel1.GroupingText = txt.ToString(); ;
        //}
        #endregion
        
    }
}