using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmManageProductAddEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Load Dropdown List
                ddlMotorProductType1.DoLoadDropdownList();
                ddlMotorCompanyInsurance1.DoLoadDropdownList();

                // Check Query String
                if (Request.QueryString["id"] != string.Empty && Request.QueryString["id"] != null)
                {
                    FunctionManager fm = new FunctionManager();

                    // Decrypt
                    string id = fm.Base64Decrypt(Request.QueryString["id"]);
                }
            }
        }
    }
}