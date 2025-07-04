using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileMotorV1.CommonUserControls
{
    public partial class ucProgressbar_ascx : System.Web.UI.UserControl
    {
        DateTime dtPageStartTime = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
        
        }
        private void Page_PreRender(object sender, System.EventArgs e)
        {
            lblProgressTime.Text = "ใช้เวลาโหลด " + (DateTime.Now - dtPageStartTime).TotalSeconds.ToString("N2") + " วินาที";
        }
    }
}