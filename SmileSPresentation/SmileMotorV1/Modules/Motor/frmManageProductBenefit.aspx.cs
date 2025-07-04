using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmManageProductBenefit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlMotorProduct.DoLoadDropdownList();
                ucBenefit1.DoLoad(0);
                AddAttributeButton();
            }
            
        }

        private void AddAttributeButton()
        {
            string _id = "testID123";
            btnEdit.Attributes.Add("onclick", "javascript:OpenPopup('" + _id + "')");
            btnAdd.Attributes.Add("onclick", "javascript:OpenPopupAdd()");
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            ucBenefit1.DoLoad(ddlMotorProduct.Product_ID);
        }
    }
}