using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;
using SmileMotorV1.Models;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmReportApplicationCoverNote : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //check role = MotorDeveloper OR MotorUnderwrite
                if (HttpContext.Current.User.IsInRole("MotorDeveloper") || HttpContext.Current.User.IsInRole("MotorUnderwrite") || HttpContext.Current.User.IsInRole("MotorUser"))
                {
                    // Check Query String "motorid"
                    if (Request.QueryString["motorid"] != string.Empty && Request.QueryString["motorid"] != null)
                    {
                        var motorId = GetMotorAppID();
                        GetMotorApplicationCoveNote(motorId);
                    }
                    else
                    {
                        Response.Redirect("frmSuccess.aspx?msg=2");
                    }
                }
                else
                {
                    Response.Redirect("frmSuccess.aspx?msg=2");
                }
            }
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Get Motor App ID
        /// </summary>
        /// <returns></returns>
        private int GetMotorAppID()
        {
            int result;

            // Check Query String "motorid"
            if (Request.QueryString["motorid"] != string.Empty && Request.QueryString["motorid"] != null)
            {
                // Get Query String "motorid"
                var code = (Request.QueryString["motorid"]);

                // ถอดรหัส Base64
                result = Convert.ToInt32(cFunction.Base64Decrypt(code));
            }
            else
            {
                result = 0;
            }

            return result;
        }

        private void GetMotorApplicationCoveNote(int motorId)
        {
            using (var db = new MotorV1Entities())
            {
                var lst = db.usp_MotorApplication_CoverNote_Select(motorId).FirstOrDefault();

                if (lst == null) return;
                var arr_text = lst.LicencePlate.Split(' ');
                //HEADER
                txt_customerName.InnerText = lst.CustomerFullName;

                //DETAIL
                txt_applicationCode.InnerText = lst.MotorApplication_Code;
                txt_customerFullname.InnerText = lst.CustomerFullName;
                txt_address.InnerText = lst.AddressFullDetail;
                //txt_carddetail.InnerText = lst.PersonCardDetail;
                txt_startCoverDate.InnerText = lst.StartCover.Value.ToString("dd MMMM yyyy");
                // txt_contactdetail.InnerText = lst.ContactDetail;

                //IN TABLE
                txt_vehicleSegmentCode.InnerText = lst.VehicleSegmentCode;
                txt_vehicleBrandModel.InnerText = lst.VehicleBrandModel;
                txt_vehicleLicenePlate.InnerText = arr_text[0] + " " + arr_text[1];
                txt_vehicleLicenePlate2.InnerText = arr_text[2];
                txt_vehicleChassisNumber.InnerText = lst.VehicleChassisNumber;
                txt_vehicleYear.InnerText = lst.VehicleYear;
                txt_vehicleSeatWeight.InnerText = lst.VehicleSeatWeight;
            }
        }

        #endregion Method
    }
}