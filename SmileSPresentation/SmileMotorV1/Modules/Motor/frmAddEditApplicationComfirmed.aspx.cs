using SmileSMotorClassLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmAddEditApplicationComfirmed : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FunctionManager fm = new FunctionManager();

                //เช็ค query string appid ไม่เท่ากับ null และ ไม่เท่ากับค่าว่าง
                if (Request.QueryString["appid"] != null && Request.QueryString["appid"] != "")
                {
                    hdfApp_ID.Value = fm.Base64Decrypt(Request.QueryString["appid"]);
                    int _appid = Convert.ToInt32(hdfApp_ID.Value);

                    //โหลด detail
                    this.ucAppDetail.DoLoad(_appid);

                    //ซ่อนปุ่มแก้ไขข้อมูลบุคคล
                    this.ucAppDetail.DoSetVisibleButtonEditPeron(false);

                    //alert แจ้งเตือนบันทึกเรียบร้อยแล้ว
                    this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.success, "บันทึกข้อมูล Application เรียบร้อยแล้ว");

                    string cashReceiveLink = Properties.Settings.Default["CashreceiveLink"].ToString();
                    //add attributes ไปเปิดโปรแกรม CashReceive
                    btnOpenCashReceive.Attributes.Add("onclick", "window.open('" + cashReceiveLink + "')");
                }
                else
                {
                    //alert ไม่พบข้อมูล
                    this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "ไม่พบข้อมูล Application");
                }
                if (Request.QueryString["disbtn"] != null && Request.QueryString["disabled"] != "")
                {
                    btnContinueNewApp.Visible = false;
                    btnOpenCashReceive.Visible = false;
                }
            }
        }

        protected void btnContinueNewApp_Click(object sender, EventArgs e)
        {
            //Redirect เปิดหน้าบันทึก app อีกครั้ง
            Response.Redirect("~/Modules/Motor/frmAddEditAppCustomer.aspx");
        }
    }

    #endregion Event
}