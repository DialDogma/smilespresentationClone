using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Wordprocessing;
using SmileSMotorClassLibrary;
using SmileMotorV1.Models;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmAppDetail : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check Query String "code"
                if (Request.QueryString["code"] != string.Empty)
                {
                    FunctionManager fm = new FunctionManager();

                    // Get Query String To Hidden Field
                    hdfMotorApplicationID.Value = cFunction.Base64Decrypt(Request.QueryString["code"]);

                    // Load uc Detail And Setup
                    this.ucAppDetail.DoLoad(Convert.ToInt32(hdfMotorApplicationID.Value), true);
                    IsValidateVisibleButtonEdit();
                    IsValidateVisibleButtonRenew();

                    if (HttpContext.Current.User.IsInRole("MotorDeveloper"))
                    {
                        this.ucAppDetail.DoSetVisibleButtonEditPeron(true);
                    }
                    if (HttpContext.Current.User.IsInRole("MotorUnderwrite"))
                    {
                        this.ucAppDetail.DoSetVisibleButtonEditPeron(true);
                    }
                    if (HttpContext.Current.User.IsInRole("MotorUser"))
                    {
                        this.ucAppDetail.DoSetVisibleButtonEditPeron(false);
                    }
                    if (Request.QueryString["readed"] != null && Request.QueryString["readed"] != "")
                    {
                        using (var db = new MotorV1Entities())
                        {
                            db.usp_MotorApplication_Document_Readed_Update(Convert.ToInt32(hdfMotorApplicationID.Value));
                        }
                    }
                }
            }

            // Set Class Button
            btnRenew.CssClass = "btn btn-info btn-sm";
            btnRenew.Width = Unit.Pixel(200);
            btnEdit.CssClass = "btn btn-warning btn-sm";
            btnEdit.Width = Unit.Pixel(200);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            MotorApplication motorApp = new MotorApplication();

            // Get Motor Application Detail To Class
            motorApp = this.ucAppDetail.GetMotorApplicationByMotorApplicationID(Convert.ToInt32(hdfMotorApplicationID.Value));

            // Set Query String From Class
            string encryptCustomerID = cFunction.Base64Encrypt(motorApp.Customer.Person_ID.ToString());
            string encryptPayerID = cFunction.Base64Encrypt(motorApp.Payer.Person_ID.ToString());
            string encryptAppID = cFunction.Base64Encrypt(motorApp.MotorApplication_ID.ToString());

            btnEdit.Visible = true;
            Response.Redirect("~/Modules/Motor/frmAddEditAppProduct.aspx?" + "&appid=" + encryptAppID);
        }

        protected void btnRenew_Click(object sender, EventArgs e)
        {
            using (var db = new MotorV1Entities())
            {
                int statusId = ucAppDetail.GetMotorApplicationStatus_ID();

                DateTime startCoverDate;

                switch (statusId)
                {
                    case 2://สถานะแอปอนุมัติ
                        startCoverDate = this.ucAppDetail.GetMotorApplicationEndCoverDate();
                        break;

                    default:
                        startCoverDate = DateTime.Today;
                        break;
                }

                var result = db.usp_MotorApplication_Renew_Insert(Convert.ToInt32(hdfMotorApplicationID.Value), startCoverDate, cFunction.GetCurrentLoginUser_ID(this.Page)).FirstOrDefault();
                if (result != null)
                {
                    if (result.IsResult == true)
                    {
                        string encryptAppID = cFunction.Base64Encrypt(result.Result.ToString());

                        Response.Redirect("~/Modules/Motor/frmAddEditAppProduct.aspx?" + "&appid=" + encryptAppID);
                    }
                    else
                    {
                        //TODO Alert ไม่สำเร็จ + MSG
                    }
                }
                else
                {
                    //TODO Alert ไม่สำเร็จ + MSG
                }
            }
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Validate Status For Visible Button Edit
        /// </summary>
        private void IsValidateVisibleButtonEdit()
        {
            // Get Motor Status
            int statusID = ucAppDetail.GetMotorApplicationStatus_ID();
            var appId = Convert.ToInt32(hdfMotorApplicationID.Value);

            // NewApp
            if (statusID == (int)MappingCode.eMotorApplicationStatus.NewApp)
            {
                btnEdit.Visible = true;
            }

            // NewApp
            else if (statusID == (int)MappingCode.eMotorApplicationStatus.WaitEdit)
            {
                btnEdit.Visible = true;
            }

            // Other
            else
            {
                btnEdit.Visible = false;
            }
        }

        /// <summary>
        /// IsValidateVisibleButtonRenew
        /// </summary>
        private void IsValidateVisibleButtonRenew()
        {
            // Get Motor Status
            int statusID = ucAppDetail.GetMotorApplicationStatus_ID();
            DateTime date1 = this.ucAppDetail.GetMotorApplicationEndCoverDate();
            DateTime date2 = DateTime.Today;
            int daysDiff = ((TimeSpan)(date1 - date2)).Days;

            if (this.ucAppDetail.GetMotorApplicationPeriodType_ID() == 3)//ชำระรายปี
            {
                switch (statusID)
                {
                    //อนุมัติ
                    case 2:
                        //TODO validate ให้ต่ออายุล่วงหน้าได้ 3 เดือน appId
                        if (daysDiff >= 0 && daysDiff <= 90)
                        {
                            btnRenew.Visible = true;
                        }
                        break;
                    //สิ้นสุดความคุ้มครอง
                    case 11:
                        //TODO validate หลังจากสิ้นสุดความคุ้มครองไม่เกิน 3 เดือนให้ต่ออายุได้ ถือเป็นการต่ออายุ
                        if (daysDiff <= 0 && daysDiff >= -90)
                        {
                            btnRenew.Visible = true;
                        }
                        break;
                }
            }

            //Response.Write(this.ucAppDetail.GetMotorApplicationPeriodType_ID());
            //Response.Write(appId);
        }

        #endregion Method
    }
}