using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.CommonUserControls
{
    public partial class ucDatepicker_DisablePastDates : System.Web.UI.UserControl
    {
        private DateTime? _dateSelected;
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

        public DateTime? DateSelected
        {
            get
            {
                if (txtDate.Text != string.Empty)
                {
                    return cDate.ToDate(txtDate.Text);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _dateSelected = value;
                if (_dateSelected.HasValue == true)
                {
                    txtDate.Text = _dateSelected.Value.ToString("dd/MM/yyyy");
                }
                else
                {
                    txtDate.Text = string.Empty;
                }
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

        /// <summary>
        /// Visible property
        /// </summary>
        //public override bool Visible { get; set; }

        /// <summary>
        /// selected date changed event
        /// </summary>
        public EventHandler eSelectedDateChanged;

        /// <summary>
        /// selected not validate date event
        /// </summary>
        public EventHandler eSelectedNotValidate;

        protected void Page_Load(object sender, EventArgs e)
        {
            txtDate.AutoPostBack = AutoPostback;
            txtDate.Attributes.Add("readonly", "readonly");
        }

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            if (IsValidate() == true)
            {
                DateSelected = cDate.ToDate(txtDate.Text);
            }

            //TODO: Raise Event Date Changed
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
                result = cDate.IsValidateStringToDatetime(strDate);
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

        //public DateTime? SelectedDate()
        //{
        //    DateManager dm = new DateManager();
        //    string strDate = txtDate.Text.Trim();
        //    DateTime dResult;
        //    //Validate
        //    if (dm.IsValidateDateFormat_Buddism(strDate))
        //    {
        //        //Return Selected Date
        //        dResult = dm.GetDateFromStringBuddism(strDate);
        //        return dResult;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// Clear textbox datetime
        /// </summary>
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
            return cDate.GetFirstDateOfMonth(dtp);
        }
    }
}