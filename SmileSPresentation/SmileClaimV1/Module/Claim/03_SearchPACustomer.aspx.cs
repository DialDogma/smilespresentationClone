using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileClaimV1.Module.Claim
{
    public partial class _03_SearchPACustomer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string appID = Request.QueryString["appID"];

                this.ucPACustomerDetailSearch.DoLoadCustomer(appID,1,string.Empty);
            }
        }
    }
}