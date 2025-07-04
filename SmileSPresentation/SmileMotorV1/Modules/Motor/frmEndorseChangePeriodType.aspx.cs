using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmEndorseChangePeriodType : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["mtid"] != "" && Request.QueryString["mtid"] != null)
                {
                    var motorEncode = Request.QueryString["mtid"];
                    var motorDecode = new ASCIIEncoding().GetString(Convert.FromBase64String(motorEncode));
                    var motorId = int.Parse(motorDecode);
                    DoLoad(motorId);

                    ddlPeriodType.DoLoadDropdownList();
                }
                else
                {
                    Response.Redirect("frmSuccess.aspx?msg=2");
                }
            }
        }

        #endregion Event

        #region Method

        private void DoLoad(int motor_id)
        {
            ucAppDetailApplication.DoLoad(motor_id);
            ucAppDetailVehicle.DoLoad(motor_id);
            ucAppDetailCustomer.DoLoad(motor_id);
            ucAppDetailAddressCustomer.DoLoad2(motor_id, 2);
            ucAppDetailPayer.DoLoad(motor_id);
            ucAppDetailAddressPayer.DoLoad2(motor_id, 3);
            ucAppDetailPayMethod.DoLoad(motor_id);
        }

        #endregion Method
    }
}