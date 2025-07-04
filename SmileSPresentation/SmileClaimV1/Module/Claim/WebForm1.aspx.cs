using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSGlobalClassLibrary.Class;


namespace SmileClaimV1.Module.Claim
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ucOpenClaim.DoLoadDropdownList(1);
                //DoloadSelector();
            }


        }

        private void DoloadSelector()
        {
            //ClaimSeviceDataCenter.ClaimServiceClient dtcenter = new ClaimSeviceDataCenter.ClaimServiceClient();

            //var lst = dtcenter.GetClaimAdmidType(null, null, true);

            //selector.DataSource = lst;
            //selector.DataTextField = "ClaimAdmitTypeDetail";
            //selector.DataValueField = "ClaimAdmitTypeID";

            //ListItem requiredList;
            ////Add Required list item
            //requiredList = new ListItem();
            //requiredList.Text = "--โปรดระบุ--";
            //requiredList.Value = "-1";
            //selector.Items.Insert(0, requiredList);


        }

        protected void bbbb_Click(object sender, EventArgs e)
        {
            var sss = selector.SelectedIndex;
            Response.Write(sss);
        }
    }
}