using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileMotorV1.CommonUserControls
{
    public partial class ucUppercaseTextbox : System.Web.UI.UserControl
    {

        private string _value;

        public string Value
        {
            get {
                _value = txtUppercase.Text.Trim();
                return _value; 
                }

            set { 
                _value = value;
                txtUppercase.Text = value;
                }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}