using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls.DropdownUserControls
{
    public partial class ddlFirstDayOfMonth : System.Web.UI.UserControl
    {
        #region Declare

        /// <summary>
        /// Property Get/Set Dropdown List
        /// </summary>
        public DropDownList ddl
        {
            get
            {
                return ddlFirstDayOfMonth1;
            }
            set
            {
                ddlFirstDayOfMonth1 = value;
            }
        }

        #endregion Declare

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Do Load Dropdown List
        /// </summary>
        /// <param name="previousMonth"></param>
        /// <param name="nextMonth"></param>
        public void DoLoadDropDownList(int previousMonth = 60, int nextMonth = 12)
        {
            var ylst = new List<ListItem>();

            // Get Date
            var dtp = cDate.GetFirstDateOfMonth(DateTime.Now);
            var sDate = cDate.DateAdd(DateInterval.Month, dtp, previousMonth * -1);

            int count = previousMonth + nextMonth;

            if (ddlFirstDayOfMonth1.Items.Count == 0)
            {
                //Year
                var li = new ListItem
                {
                    Text = "งวด",
                    Value = "0"
                };

                ddlFirstDayOfMonth1.Items.Insert(0, li);

                for (var i = 0; i <= count; i++)
                {
                    sDate = cDate.DateAdd(DateInterval.Month, sDate, 1);

                    li = new ListItem
                    {
                        Text = sDate.ToString("dd MMMM yyyy"),
                        Value = sDate.ToString()
                    };

                    ylst.Add(li);
                }

                var j = 1;
                foreach (var itm in ylst)
                {
                    ddlFirstDayOfMonth1.Items.Insert(j, itm);
                    j++;
                }

                ddlFirstDayOfMonth1.SelectedValue = dtp.ToString(); ;
            }
        }

        #endregion Method
    }
}