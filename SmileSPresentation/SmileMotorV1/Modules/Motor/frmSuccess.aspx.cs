using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmSuccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string msg = string.Empty;

            if (!IsPostBack)
            {
                string QSmsg = Request.QueryString["msg"];
                switch (QSmsg)
                {
                    case "":
                        msg = "บันทึกเรียบร้อยแล้ว";
                        break;

                    case "1":
                        msg = "ดำเนินการเรียบร้อยแล้ว";
                        break;

                    case "2":
                        msg = "ไม่สามารถเข้าใช้งานได้";
                        divImg.Visible = true;
                        break;

                    default:
                        msg = "สำเร็จ";
                        break;
                }
                lblMessage.Text = msg;
            }
        }
    }
}