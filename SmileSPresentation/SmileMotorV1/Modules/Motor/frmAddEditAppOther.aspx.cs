using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmAddEditPayment : System.Web.UI.Page
    {

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FunctionManager fm = new FunctionManager();

                //เช็ค query string appid ไม่เท่ากับ null และ ไม่เท่ากับค่าว่าง
                if (Request.QueryString["appid"] != null && Request.QueryString["appid"] != string.Empty)
                {
                    //เก็บ appid ใส่ hdfApp_ID
                    hdfApp_ID.Value = fm.Base64Decrypt(Request.QueryString["appid"]);

                    MotorDB db = new MotorDB();
                    PersonRelate customer = new PersonRelate();
                    PersonRelate payer = new PersonRelate();
                    int _appid = Convert.ToInt32(hdfApp_ID.Value);

                    //get person 
                    customer = db.GetPersonRelateByMotorApplicationID(_appid, 2); // 2 คือ ผู้เอาประกัน
                    payer = db.GetPersonRelateByMotorApplicationID(_appid, 3); // 3 คือ ผู้ชำระเบี้ย

                    //set person_id in hidden field
                    hdfCustomer_ID.Value = customer.Person_ID.ToString();
                    hdfPayer_ID.Value = payer.Person_ID.ToString();

                    //load memo & Document
                    this.ucApplicationMemo.DoLoad(_appid);
                    this.ucDocument.DoLoad(_appid);

                    ///DoLoad
                    this.ucAppDetailApplication.DoLoad(_appid);
                    this.ucPersonDetail_customer.DoLoad(customer.Person_ID);
                    this.ucPersonDetail_payer.DoLoad(payer.Person_ID);
                }
            }
        }

        protected void btnBackward_Click(object sender, EventArgs e)
        {
            FunctionManager fm = new FunctionManager();

            //set id
            int motorAppID = Convert.ToInt32(hdfApp_ID.Value);

            //เข้ารหัส base64
            string encryptCustomerID = fm.Base64Encrypt(hdfCustomer_ID.Value);
            string encryptPayerID = fm.Base64Encrypt(hdfPayer_ID.Value);
            string EncryptMotorID = fm.Base64Encrypt(motorAppID.ToString());

            //redirect
            Response.Redirect("~/Modules/Motor/frmAddEditAppProduct.aspx?" + "cid=" + encryptCustomerID + "&pid=" + encryptPayerID + "&appid=" + EncryptMotorID);

        }

        protected void btnSuccess_Click(object sender, EventArgs e)
        {
            FunctionManager fm = new FunctionManager();

            // Set Motor Application
            int motorAppID = Convert.ToInt32(hdfApp_ID.Value);
            string EncryptMotorID = fm.Base64Encrypt(motorAppID.ToString());

            // Save Status Application
            DoSaveMotorApplicationStatus(motorAppID);

            //alert
            fm.AlertJavaScript(this.Page, "บันทึกเรียบร้อยแล้ว!");

            //redirect
            Response.Redirect("~/Modules/Motor/frmAddEditApplicationComfirmed.aspx?" + "appid=" + EncryptMotorID);
        }

        #endregion

        #region Method
        /// <summary>
        /// บันทึกสถานะ Application
        /// </summary>
        /// <param name="_motorApplication_ID">รับ motorApplication_ID</param>
        /// <returns></returns>
        private int DoSaveMotorApplicationStatus(int _motorApplication_ID)
        {
            FunctionManager fm = new FunctionManager();
            MotorDB db = new MotorDB();
            MotorApplication appDetail = new MotorApplication();
            int result;

            //get application detail
            appDetail = db.GetMotorApplicationByMotorApplicationID(_motorApplication_ID);

            //เช็ค ถ้าสถานะ n/a
            if ((int)MappingCode.eMotorApplicationStatus.NotAvailable == appDetail.MotorApplicationStatus_ID)
            {
                //result รออนุมัติ
                result = db.UpdateMotorApplicationStatus(MappingCode.eMotorApplicationStatus.WaitApprove, _motorApplication_ID, cFunction.GetCurrentLoginUser_ID(this));
            }
            //เช็ค ถ้าสถานะ บันทึกใหม่
            else if ((int)MappingCode.eMotorApplicationStatus.NewApp == appDetail.MotorApplicationStatus_ID)
            {
                //result รออนุมัติ
                result = db.UpdateMotorApplicationStatus(MappingCode.eMotorApplicationStatus.WaitApprove, _motorApplication_ID, cFunction.GetCurrentLoginUser_ID(this));
            }
            //เช็ค ถ้าสถานะ รอแก้ไข
            else if ((int)MappingCode.eMotorApplicationStatus.WaitEdit == appDetail.MotorApplicationStatus_ID)
            {
                //result รออนุมัติ(แก้ไข)
                result = db.UpdateMotorApplicationStatus(MappingCode.eMotorApplicationStatus.EditWaitApprove, _motorApplication_ID, cFunction.GetCurrentLoginUser_ID(this));
            }
            //อื่นๆ
            else
            {
                //result 0
                result = 0;
            }
          
            return result;
        }

        #endregion        
    }
}