using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileClaimV1.CommonUserControl
{
    public partial class ucDatepicker : System.Web.UI.UserControl
    {
        #region  Declare
        private DateTime _dateSelected;
        private bool _IsRequired;
        private string _setPlaceholder;

        public bool IsRequired
        {
            get
            {
                return _IsRequired;
            }
            set
            {
                _IsRequired = value;

            }
        }

        public DateTime DateSelected
        {
            get
            {
                return ToDate(txtDate.Text);
            }
            set
            {
                _dateSelected = value;
                txtDate.Text = _dateSelected.ToString("dd/MM/yyyy");
            }
        }

        /// <summary>
        /// Auto postback text date
        /// </summary>
        public bool AutoPostback { get; set; }

        public string SetPlaceholder
        {
            get
            {
                return _setPlaceholder;
            }

            set
            {
                _setPlaceholder = value;
                txtDate.Attributes.Add("placeholder", _setPlaceholder);
            }
        }
        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            txtDate.AutoPostBack = AutoPostback;

            if (txtDate.Text == "")
            {
                DateSelected = DateTime.Today;
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// String to Date
        /// </summary>
        /// <param name="txt"></param>
        /// <returns>Date </returns>
        /// <remarks>(ค.ศ.)</remarks>
        public static DateTime ToDate(string txt)
        {
            DateTime dtp;

            if (IsValidate(txt) == true)
            {
                dtp = DateTime.Parse(txt);

                int yy = dtp.Year;
                int mm = dtp.Month;
                int dd = dtp.Day;

                if (yy > 2500)
                {
                    yy -= 543;
                }

                if (yy < 1700)
                {
                    yy += 543;
                }

                return new DateTime(yy, mm, dd);
            }

            return new DateTime();
        }

        /// <summary>
        /// Date (พ.ศ.) to Date (ค.ศ.)
        /// </summary>
        /// <param name="dtp"></param>
        /// <returns>Date </returns>
        /// <remarks>(ค.ศ.)</remarks>
        public static DateTime ToDate(DateTime dtp)
        {
            int yy = dtp.Year;
            int mm = dtp.Month;
            int dd = dtp.Day;

            if (yy > 2500)
            {
                yy -= 543;
            }

            if (yy < 1700)
            {
                yy += 543;
            }

            return new DateTime(yy, mm, dd);
        }

        public static bool IsValidate(string strDate)
        {
            if (strDate.Trim() == "")
            {
                return false;
            }

            if (IsValidateStringToDatetime(strDate) == false)
            {
                return false;
            }

            DateTime dtp = DateTime.Parse(strDate);

            if (IsValidate_InRange(dtp) == false)
            {
                return false;
            }

            return true;
        }

        public static bool IsValidate(DateTime dtp)
        {
            if (dtp == null)
            {
                return false;
            }

            if (IsValidate_InRange(dtp) == false)
            {
                return false;
            }

            if (IsValidate_InRange(dtp) == false)
            {
                return false;
            }

            return true;
        }

        public static bool IsValidateStringToDatetime(string strDate)
        {
            DateTime temp;
            if (DateTime.TryParse(strDate, out temp))
            { //yay
                return true;
            }
            else
            { //:(
                return false;
            }
        }

        public static bool IsValidate_InRange(DateTime dtp)
        {
            DateTime sDate = new DateTime(1000, 01, 01);
            DateTime eDate = new DateTime(3000, 12, 31);

            if (dtp < sDate)
            {
                return false;
            }

            if (dtp > eDate)
            {
                return false;
            }

            return true;
        }


        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            if (IsValidate() == true)
            {
                DateSelected = ToDate(txtDate.Text);
            }
        }

        public bool IsValidate()
        {
            bool result;
            string strDate = txtDate.Text.Trim();
            int len;

            //get len
            len = strDate.Length;

            if (len > 0)
            {
                // มีการกรอกค่า
                result = IsValidateStringToDatetime(strDate);
            }
            else
            {
                // ไม่มีการกรอกค่า
                if (IsRequired)
                {
                    //Required
                    result = false;
                }
                else
                {
                    //Not Required
                    result = true;
                }
            }
            return result;
        }

        public void DoClear()
        {
            txtDate.Text = "";
        }
        public void IsEnabled(bool _bool)
        {
            txtDate.Enabled = _bool;
        }

        public DateTime GetFirstDateOfMonth(DateTime dtp)
        {
            return GetFirstDateOfMonth(dtp);
        }
        #endregion


    }
}