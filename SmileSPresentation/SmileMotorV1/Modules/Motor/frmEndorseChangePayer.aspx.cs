using SmileSMotorClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileMotorV1.Models;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmEndorseChangePayer : System.Web.UI.Page
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

                    ddlPersonType1.DoLoadDropdownList();
                }
                else
                {
                    Response.Redirect("frmSuccess.aspx?msg=2");
                }
            }
        }

        protected void ddlPersonType1_eSelectChanged(object sender, EventArgs e)
        {
            int personTypeID = ddlPersonType1.PersonTypeID;

            //Visible uc application customerChange
            ucAppDetailCustomerChange.Visible = false;

            if (personTypeID == 3) // 3 นิติบุคคล
            {
                //clear detail
                ucPersonSearchByIdentityCardNumber.DoClearTextBoxSearch2();

                hdfPerson_ID.Value = "";

                ddlRelation.Relation_ID = -1;
                ddlRelation.IsEnabled(true);

                ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.clear, "");
            }
            else if (personTypeID == 2) // 2 บุคคลธรรมดา
            {
                //clear detail
                ucPersonSearchByIdentityCardNumber.DoClearTextBoxSearch2();

                hdfPerson_ID.Value = "";

                ddlRelation.Relation_ID = -1;
                ddlRelation.IsEnabled(true);

                ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.clear, "");
            }
            else
            {
                //clear detail
                ucPersonSearchByIdentityCardNumber.DoClearTextBoxSearch2();

                hdfPerson_ID.Value = "";

                ddlRelation.Relation_ID = -1;
                ddlRelation.IsEnabled(true);

                ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.clear, "");
            }

            ucPersonSearchByIdentityCardNumber.SetPersonTypeIDTextbox(ddlPersonType1.PersonTypeID.ToString());
        }

        protected void ucPersonSearchByIdentityCardNumber_eSelectedPersonIDChanged(object sender, EventArgs e)
        {
            DataCenterDB dbc = new DataCenterDB();

            // Get ค่า person_id ไว้ใน hdfPerson_ID
            hdfPerson_ID.Value = ucPersonSearchByIdentityCardNumber.Person_ID.ToString();
            int customer_id = Convert.ToInt32(hdfPerson_ID.Value);

            //get card type
            int cardtype_id = dbc.GetPersonCardByPersonID(customer_id).Select(o => o.PersonCardType_ID).FirstOrDefault();

            if (cardtype_id == 2 || cardtype_id == 3)
            {
                //โหลด Detail  ucCustomer บุคคลธรรมดา
                //panelDetail.Visible = true;
                ucAppDetailCustomerChange.Visible = true;
                ucAppDetailCustomerChange.DisableButtonEditAndAddress(false);
                DoloadCustomerDetailByPersonID(customer_id);
                ddlPersonType1.PersonTypeID = 2; //set dropdownlist เป็นบุคคลธรรมดา
            }
            else
            {
                ucAppDetailCustomerChange.Visible = true;
                ucAppDetailCustomerChange.DisableButtonEditAndAddress(false);
                DoloadCustomerDetailByPersonID(customer_id);
                ddlPersonType1.PersonTypeID = 3; //set dropdownlist
            }

            //เช็คถ้าเป็นบุคคลคนเดียวกัน
            if (hdfPerson_ID.Value == hdfCustomer_ID.Value)
            {
                ddlRelation.Relation_ID = 39;
                ddlRelation.IsEnabled(false);
            }
        }

        protected void ucPersonSearchByIdentityCardNumber_eClickPersonIDChangednull1(object sender, EventArgs e)
        {
            //alert
            ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.info, "กรอกคำค้น");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DoSave();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            cFunction.CloseBrowser(Page);
        }

        #endregion Event

        #region Method

        private void DoLoad(int motor_id)
        {
            //2 ผู้เอาประกัน
            //3 ผู้ชำระเบี้ย

            ucAppDetailApplication.DoLoad(motor_id);
            ucAppDetailVehicle.DoLoad(motor_id);
            ucAppDetailCustomer.DoLoad(motor_id);
            ucAppDetailAddressCustomer.DoLoad2(motor_id, 2);
            ucAppDetailPayer.DoLoad(motor_id);
            ucAppDetailAddressPayer.DoLoad2(motor_id, 3);
            ucAppDetailPayMethod.DoLoad(motor_id);
            ddlRelation.DoLoadDropdownList();
            ucAppDetailCustomerChange.Visible = false;

            hdfApp_ID.Value = motor_id.ToString();
            hdfOldPerson_ID.Value = ucAppDetailPayer.MotorApplication.Payer.Person_ID.ToString();
            hdfCustomer_ID.Value = ucAppDetailCustomer.MotorApplication.Customer.Person_ID.ToString();
        }

        /// <summary>
        /// Load Customer Detail by Person_id
        /// </summary>
        /// <param name="person_id">ส่ง Person_id เข้ามา</param>
        private void DoloadCustomerDetailByPersonID(int person_id)
        {
            ucAppDetailCustomerChange.Visible = true;

            ucAppDetailCustomerChange.DoLoadByPersonId(person_id);
        }

        private void DoInsertCustomerApplication(int appId, int personId, string custType)
        {
            var userID = cFunction.GetCurrentLoginUser_ID(this.Page);
            using (var db = new MotorV1Entities())
            {
                var rs = db.usp_CustomerApplication_Motor_Person_InsertOrUpdate(appId, personId, custType, userID);
            }
        }

        private void DoSave()
        {
            if (hdfPerson_ID.Value != "")
            {
                if (hdfPerson_ID.Value != hdfOldPerson_ID.Value)
                {
                    using (MotorV1Entities motorV1entities = new MotorV1Entities())
                    {
                        var app_Id = Convert.ToInt32(hdfApp_ID.Value);
                        var person_Id = Convert.ToInt32(hdfPerson_ID.Value);

                        var result = motorV1entities.usp_EndorseChangePayer_Update(app_Id, ddlRelation.Relation_ID, person_Id, cFunction.GetCurrentLoginUser_ID(Page)).FirstOrDefault();

                        if (result.IsResult.Value)
                        {
                            DoInsertCustomerApplication(app_Id, person_Id, "P");

                            //alert
                            ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.success, result.Msg);

                            var encrypt_appId = cFunction.Base64Encrypt(app_Id.ToString());

                            Server.Transfer("frmAddEditApplicationComfirmed.aspx?appid=" + encrypt_appId + "&disbtn=0");
                        }
                        else
                        {
                            ucPersonSearchByIdentityCardNumber.DoClearTextBoxSearch2();

                            hdfPerson_ID.Value = "";

                            ucAppDetailCustomerChange.Visible = false;

                            //alert
                            ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, result.Msg);
                        }
                    }
                }
                else
                {
                    ucPersonSearchByIdentityCardNumber.DoClearTextBoxSearch2();

                    hdfPerson_ID.Value = "";

                    ucAppDetailCustomerChange.Visible = false;

                    //alert
                    ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "กรุณาเลือกผู้ชำระเบี้ยคนใหม่");
                }
            }
            else
            {
                ucPersonSearchByIdentityCardNumber.DoClearTextBoxSearch2();

                hdfPerson_ID.Value = "";

                ucAppDetailCustomerChange.Visible = false;

                //alert
                ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "กรุณาเลือกผู้ชำระเบี้ย");
            }
        }

        #endregion Method
    }
}