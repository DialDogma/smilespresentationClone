using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileMotorV1.Models;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucAppDetail : System.Web.UI.UserControl
    {
        #region Declare

        private string motorapplicationCode;

        public string MotorapplicationCode
        {
            get
            {
                return motorapplicationCode;
            }

            set
            {
                motorapplicationCode = value;
            }
        }

        #endregion Declare

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        /// <summary>
        /// BUTTON PRINT COVER NOTE CLICK!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrint_OnClick(object sender, EventArgs e)
        {
            var encryptID = cFunction.Base64Encrypt(hdfMotorAppID.Value);
            //Redirect to print covernote
            btnPrint.Attributes.Add("href", "../Motor/frmReportApplicationCoverNote?motorid=" + encryptID);
            btnPrint.Attributes.Add("target", "_blank");
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Do Load Detail
        /// </summary>
        /// <param name="motorapplicationID">Motor Application ID</param>
        /// <param name="visibleButtonMemo">True/False</param>
        public void DoLoad(int motorapplicationID, bool visibleButtonMemo = false)
        {
            MotorApplication motor = new MotorApplication();

            motor = GetMotorApplicationByMotorApplicationID(motorapplicationID);

            // Load UC
            this.ucAppDetailApplication.DoLoad(motorapplicationID);

            ////Check ถ้าเป็นนิติให้โหลด นิติ แต่ถ้าไม่เป็น ให้โหลดข้อมูลธรรมดา ผู้เอาประกัน
            //switch (motor.Customer.Person.PersonType_ID)
            //{
            //    case (int)MappingCode.ePersonType.Individual:           // 2 คือบุคคลธรรมดา
            //        this.ucAppDetailCustomer.DoLoad(motorapplicationID);

            //        this.ucAppDetailCorporate.Visible = false;

            //        break;
            //    case (int)MappingCode.ePersonType.Corporate:            // 3 คือนิติบุคคล
            //        this.ucAppDetailCorporate.DoLoad(motorapplicationID,(int)MappingCode.ePersonRelatedApplicationType.Customer);

            //        this.ucAppDetailCustomer.Visible = false;

            //        break;
            //    default:
            //        break;
            //}

            ////check ผู้ชำระเบี้ย
            //switch (motor.Payer.Person.PersonType_ID)
            //{
            //    case (int)MappingCode.ePersonType.Individual:   // 2 คือบุคคลธรรมดา
            //        this.ucAppDetailPayer.DoLoad(motorapplicationID);
            //        this.ucAppDetailCorporate1.Visible = false;
            //        break;
            //    case (int)MappingCode.ePersonType.Corporate:    // 3 คือนิติบุคคล
            //        this.ucAppDetailCorporate1.DoLoad(motorapplicationID, (int)MappingCode.ePersonRelatedApplicationType.Payer);
            //        this.ucAppDetailPayer.Visible = false;
            //        break;
            //    default:
            //        break;

            //}
            SetAppCode(motorapplicationID);
            //SET MOTOR CODE - MT61xxxxxxxx
            ucDocument.SetAppID(motorapplicationCode);

            ucAppDetailCustomer.DoLoad(motorapplicationID);
            ucAppDetailPayer.DoLoad(motorapplicationID);

            ucAppDetailAddressCustomer.DoLoad(motorapplicationID, MappingCode.ePersonRelatedApplicationType.Customer);

            ucAppDetailAddressPayer.DoLoad(motorapplicationID, MappingCode.ePersonRelatedApplicationType.Payer);

            ucAppDetailVehicle.DoLoad(motorapplicationID);
            ucAppDetailPayMethod.DoLoad(motorapplicationID);

            ucAppDetailOwner.DoLoad(motorapplicationID);
            ucDocument.DoLoad(motorapplicationID);
            ucDocument.IsEnabled(true); //เปิดปิดปุ่มสแกนเอกสาร
            ucApplicationMemo.DoLoad(motorapplicationID);
            ucApplicationMemo.IsVisible(visibleButtonMemo);
            ucAppDetailDept.DoLoad(motorapplicationID);
            ucAppDetailAppReNew.Doload(motorapplicationID);

            //this.ucBenefit.DoLoad(motorapplicationID);

            //SET MOTOR ID TO  HIDDEN FIELD
            hdfMotorAppID.Value = motorapplicationID.ToString();

            //CHECK VISIBLE BUTTON PRINT
            CountData();
        }

        /// <summary>
        /// Set Text Application Code
        /// </summary>
        /// <param name="motorapplicationID"></param>
        private void SetAppCode(int motorapplicationID)
        {
            MotorDB db = new MotorDB();
            FunctionManager fm = new FunctionManager();
            MotorApplication mad = new MotorApplication();

            // Get Application Detail
            mad = db.GetMotorApplicationByMotorApplicationID(motorapplicationID);

            string motorAppCode = mad.MotorApplication_Code;
            // Set Code
            lblAppCode.Text = motorAppCode;
            motorapplicationCode = motorAppCode;
        }

        /// <summary>
        /// Get Motor Application Status ID
        /// </summary>
        /// <returns></returns>
        public int GetMotorApplicationStatus_ID()
        {
            // Get Status From UC
            return ucAppDetailApplication.MotorApplicationStatus_ID;
        }

        public int GetMotorApplicationPeriodType_ID()
        {
            // Get Status From UC
            return ucAppDetailPayMethod.GetMotorApplicationPeriodType_ID();
        }

        /// <summary>
        /// GetMotorApplicationStartCoverDate
        /// </summary>
        /// <returns></returns>
        public DateTime GetMotorApplicationStartCoverDate()
        {
            // Get Status EndCoverDate From UC
            return ucAppDetailApplication.GetMotorApplicationStartCoverDate();
        }

        /// <summary>
        /// GetMotorApplicationEndCoverDate
        /// </summary>
        /// <returns></returns>
        public DateTime GetMotorApplicationEndCoverDate()
        {
            // Get Status EndCoverDate From UC
            return ucAppDetailApplication.GetMotorApplicationEndCoverDate();
        }

        /// <summary>
        /// Get Motor Application Return Class
        /// </summary>
        /// <param name="application_id">Motor Application Class</param>
        /// <returns></returns>
        public MotorApplication GetMotorApplicationByMotorApplicationID(int application_id)
        {
            return this.ucAppDetailApplication.GetMotorApplicationByMotorApplicationID(application_id);
        }

        /// <summary>
        /// Do Set Visible Edit Person Button
        /// </summary>
        /// <param name="value">True/False</param>
        public void DoSetVisibleButtonEditPeron(bool value)
        {
            ucAppDetailCustomer.ButtonEditPerson.Visible = value;
            ucAppDetailPayer.ButtonEditPerson.Visible = value;
        }

        private void CountData()
        {
            var encryptID = cFunction.Base64Encrypt(hdfMotorAppID.Value);
            using (var db = new MotorV1Entities())
            {
                var lst = db.usp_MotorApplication_CoverNote_Select(Convert.ToInt32(hdfMotorAppID.Value)).Count();

                btnPrint.Visible = lst > 0;
                if (lst > 0)
                {
                    //hplCoverNoteManual.Visible = true;
                    //hplCoverNoteManualPart2.Visible = true;
                    TRCoverNoteManual.Visible = true;
                }
            }
        }

        public int GetCustomerId(int morId)
        {
            return this.ucAppDetailCustomer.GetCustomerID(morId);
        }

        public int GetPayerId(int morId)
        {
            return this.ucAppDetailPayer.GetPayerID(morId);
        }

        #endregion Method
    }
}