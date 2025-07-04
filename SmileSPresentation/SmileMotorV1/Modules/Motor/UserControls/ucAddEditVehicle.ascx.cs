using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using SmileMotorV1.Modules.Motor.UserControls.DropdownUserControls;
using SmileSMotorClassLibrary;
using SmileMotorV1.Models;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucAddEditVehicle : System.Web.UI.UserControl
    {
        #region Declare

        private Vehicle _vehicle;

        /// <summary>
        /// Property Get/Set Verhicle Class
        /// </summary>
        public Vehicle Vehicle
        {
            get
            {
                _vehicle = new Vehicle();

                // Get Verhicle Detail
                _vehicle.VehicleBrand_ID = ddlMotorVehicleBrand.VehicleBrand_ID;
                _vehicle.VehicleType_ID = ddlMotorVehicleType.VehicleType_ID;
                _vehicle.VehicleSegment_ID = ddlMotorVehicleSegment.VehicleSegment_ID;
                _vehicle.VehicleModel_ID = ddlMotorVehicleModel.VehicleModel_ID;
                _vehicle.VehicleCC = txtVehicleCC.Text.Trim();
                _vehicle.VehicleWeight = txtWeightVehicle.Text.Trim();
                _vehicle.VehicleRegistrationDetail = txtRegistrationDetail.Text.Trim();
                _vehicle.VehicleRegistrationNumber = txtRegistrationNumber.Text.Trim();
                _vehicle.VehicleYear = ddlVehicleYear.VehicleYear;
                _vehicle.VehicleSeat = txtVehicleSeat.Text.Trim();
                _vehicle.VehicleEngineNumber = txtEngineNo.Text.ToUpper().Trim();
                _vehicle.VehicleChassisNumber = txtChassis.Text.ToUpper().Trim();
                _vehicle.VehicleRegistrationProvince_ID = ddlMotorProvince.Province_ID;
                _vehicle.FuelType_ID = ddlFuelType.FuelType_ID;

                //
                if (txtVehiclePrice.Text != "")
                {
                    _vehicle.VehiclePrice = Convert.ToDouble(txtVehiclePrice.Text);
                }
                else
                {
                    _vehicle.VehiclePrice = 0;
                }

                return _vehicle;
            }
            set
            {
                _vehicle = value;
            }
        }

        #endregion Declare

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                // Set TXT Class Style
                this.txtEngineNo.CssClass = "form-control input-sm uppercase";
                this.txtChassis.CssClass = "form-control input-sm uppercase";
                this.txtChassis.Width = Unit.Percentage(75);

                //this.txtRegistrationDetail.CssClass = "form-control input-sm";
                //this.txtRegistrationNumber.CssClass = "form-control input-sm";
                //this.txtRegistrationDetail.Width = Unit.Percentage(30);
                //this.txtRegistrationNumber.Width = Unit.Percentage(45);
            }
        }

        protected void ddlMotorVehicleType_SelectChanged(object sender, EventArgs e)
        {
            // Clear And Load DDL
            ddlMotorVehicleSegment.DoClear();
            ddlMotorVehicleSegment.DoLoadDropdownListByVehicleTypeID(ddlMotorVehicleType.VehicleType_ID);
            //ddlMotorVehicleSegment_SelectChanged(new object(), new EventArgs());
        }

        protected void ddlMotorVehicleSegment_SelectChanged(object sender, EventArgs e)
        {
            // Clear And Load DDL
            ddlMotorVehicleModel.Doclear();
            ddlMotorVehicleModel.DoLoadDropdownListByVehicleSegmentandVehicleBrand(ddlMotorVehicleSegment.VehicleSegment_ID, ddlMotorVehicleBrand.VehicleBrand_ID);
        }

        protected void ddlMotorVehicleBrand_SelectChanged(object sender, EventArgs e)
        {
            // Clear And Load DDL
            ddlMotorVehicleModel.Doclear();
            ddlMotorVehicleModel.DoLoadDropdownListByVehicleSegmentandVehicleBrand(ddlMotorVehicleSegment.VehicleSegment_ID, ddlMotorVehicleBrand.VehicleBrand_ID);
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Do Load Dropdown List
        /// </summary>
        public void DoloadDropDownList()
        {
            // Load DDL
            this.ddlMotorVehicleBrand.DoLoadDropdownList();
            this.ddlMotorVehicleType.DoLoadDropdownList();
            this.ddlMotorProvince.DoLoadDropdownList();
            this.ddlVehicleYear.DoLoadDropdownList();
            this.ddlFuelType.DoLoadDropdownList();
        }

        /// <summary>
        /// Do Load Detail
        /// </summary>
        /// <param name="motorapplicationID">Motor Application ID</param>
        public void DoLoad(int motorapplicationID)
        {
            Vehicle vc = new Vehicle();
            MotorDB db = new MotorDB();

            // Get Vehicle Detail
            vc = db.GetVehicleByMotorApplicationID(motorapplicationID);

            // Set Bind Vehicle Detail
            ddlMotorVehicleType.VehicleType_ID = vc.VehicleType_ID;
            ddlMotorVehicleType_SelectChanged(new Object(), new EventArgs());

            if (ddlMotorVehicleType.VehicleType_ID != -1)
            {
                ddlMotorVehicleSegment.VehicleSegment_ID = vc.VehicleSegment_ID;
                //ddlMotorVehicleSegment_SelectChanged(new Object(), new EventArgs());
            }

            ddlMotorVehicleBrand.VehicleBrand_ID = vc.VehicleBrand_ID;
            ddlMotorVehicleBrand_SelectChanged(new Object(), new EventArgs());

            ddlMotorVehicleModel.VehicleModel_ID = vc.VehicleModel_ID;

            txtVehicleCC.Text = vc.VehicleCC;
            txtWeightVehicle.Text = vc.VehicleWeight;
            txtRegistrationNumber.Text = vc.VehicleRegistrationNumber;
            ddlVehicleYear.VehicleYear = vc.VehicleYear;
            ddlMotorProvince.Province_ID = vc.VehicleRegistrationProvince_ID;
            ddlFuelType.FuelType_ID = vc.FuelType_ID;
            txtVehicleSeat.Text = vc.VehicleSeat;
            txtVehiclePrice.Text = vc.VehiclePrice.ToString();
            txtEngineNo.Text = vc.VehicleEngineNumber;
            txtChassis.Text = vc.VehicleChassisNumber;
            txtRegistrationDetail.Text = vc.VehicleRegistrationDetail;
        }

        /// <summary>
        /// Validate Vehicle
        /// </summary>
        /// <returns></returns>
        public string ValidateVehicle()
        {
            string result;
            TransactionManager tm = new TransactionManager();

            // Valid
            result = tm.Validate_Vehicle(Vehicle);

            return result;
        }

        public bool ValidateFocus()
        {
            //Brand
            if (ddlMotorVehicleBrand.VehicleBrand_ID == 1 || ddlMotorVehicleBrand.VehicleBrand_ID == -1)
            {
                ddlMotorVehicleBrand.Focus();
                return false;
            }

            //VehicleType
            if (ddlMotorVehicleType.VehicleType_ID == 1 || ddlMotorVehicleType.VehicleType_ID == -1)
            {
                ddlMotorVehicleType.Focus();
                return false;
            }
            //VehicleSegment
            if (ddlMotorVehicleSegment.VehicleSegment_ID == 1 || ddlMotorVehicleSegment.VehicleSegment_ID == -1)
            {
                ddlMotorVehicleSegment.Focus();
                return false;
            }
            //Model
            if (ddlMotorVehicleModel.VehicleModel_ID == 1 || ddlMotorVehicleModel.VehicleModel_ID == -1)
            {
                ddlMotorVehicleModel.Focus();
                return false;
            }
            //ขนาดเครื่องยนต์
            if (txtVehicleCC.Text.Trim() == "")
            {
                txtVehicleCC.Focus();
                txtVehicleCC.BackColor = Color.LightPink;
                return false;
            }
            else
            {
                txtVehicleCC.BackColor = Color.Empty;
            }

            //น้ำหนักรถ
            if (txtWeightVehicle.Text.Trim() == "")
            {
                txtWeightVehicle.Focus();
                txtWeightVehicle.BackColor = Color.LightPink;
                return false;
            }
            else
            {
                txtWeightVehicle.BackColor = Color.Empty;
            }

            if (this.ddlFuelType.FuelType_ID == 1 || this.ddlFuelType.FuelType_ID == -1)
            {
                this.ddlFuelType.Focus();
                return false;
            }

            //อักษรทะเบียนรถ
            if (txtRegistrationDetail.Text.Trim() == "")
            {
                txtRegistrationDetail.Focus();
                txtRegistrationDetail.BackColor = Color.LightPink;
                return false;
            }
            else
            {
                txtRegistrationDetail.BackColor = Color.Empty;
            }

            //ตัวเลขทะเบียนรถ
            if (txtRegistrationNumber.Text.Trim() == "")
            {
                txtRegistrationNumber.Focus();
                txtRegistrationNumber.BackColor = Color.LightPink;
                return false;
            }
            else
            {
                txtRegistrationNumber.BackColor = Color.Empty;
            }

            //จังหวัด
            if (ddlMotorProvince.Province_ID == "-1")
            {
                ddlMotorProvince.Focus();
                return false;
            }

            //ที่นั่ง
            if (txtVehicleSeat.Text.Trim() == "")
            {
                txtVehicleSeat.Focus();
                txtVehicleSeat.BackColor = Color.LightPink;
                return false;
            }
            else
            {
                txtVehicleSeat.BackColor = Color.Empty;
            }

            //เลขเครื่องยนต์
            if (txtEngineNo.Text.Trim() == "")
            {
                txtEngineNo.Focus();
                txtEngineNo.BackColor = Color.LightPink;
                return false;
            }
            else
            {
                txtEngineNo.BackColor = Color.Empty;
            }

            //เลขตัวถัง
            if (txtChassis.Text.Trim() == "")
            {
                txtChassis.Focus();
                txtChassis.BackColor = Color.LightPink;
                return false;
            }
            else
            {
                txtChassis.BackColor = Color.Empty;
            }

            return true;
        }

        /// <summary>
        /// ข้อมูล Test
        /// </summary>
        public void SetDataTest()
        {
            ddlMotorVehicleBrand.VehicleBrand_ID = 2;
            ddlMotorVehicleType.VehicleType_ID = 2;
            ddlMotorVehicleType_SelectChanged(new Object(), new EventArgs());
            ddlMotorVehicleSegment.VehicleSegment_ID = 2;
            ddlMotorVehicleSegment_SelectChanged(new Object(), new EventArgs());
            ddlMotorVehicleModel.VehicleModel_ID = 3;
            txtVehicleCC.Text = "1600";
            txtWeightVehicle.Text = "456";
            ddlFuelType.FuelType_ID = 2;
            txtRegistrationNumber.Text = "KM2587";
            ddlMotorProvince.Province_ID = "10";
            ddlVehicleYear.VehicleYear = "2015";
            txtVehicleSeat.Text = "6";
            txtVehiclePrice.Text = "15000";
            txtEngineNo.Text = "KMNBG6012";
            txtChassis.Text = "KKJ120000";
            txtRegistrationDetail.Text = "กจ";
        }

        public bool ValidateDuplicate()
        {
            var db = new MotorV1Entities();
            var result = db.usp_MotorApplication_VehicleDetailValidate_Select(txtRegistrationDetail.Text.Trim(),
                                                                                txtRegistrationNumber.Text.Trim(),
                                                                                Convert.ToInt32(ddlMotorProvince.Province_ID),
                                                                                txtChassis.Text.Trim(),
                                                                                txtEngineNo.Text.Trim()).FirstOrDefault();

            if (result != null && result.ReturnResult == false)
            {
                txtRegistrationDetail.Focus();
                txtRegistrationDetail.BackColor = Color.LightPink;

                txtRegistrationNumber.BackColor = Color.LightPink;
                txtChassis.BackColor = Color.LightPink;
                txtEngineNo.BackColor = Color.LightPink;
            }
            return result.ReturnResult.Value;
        }

        public void IsEnabled(bool value)
        {
            // txt
            txtVehicleCC.Enabled = value;
            txtWeightVehicle.Enabled = value;
            txtRegistrationDetail.Enabled = value;
            txtRegistrationNumber.Enabled = value;
            txtVehicleSeat.Enabled = value;
            txtVehiclePrice.Enabled = value;
            txtEngineNo.Enabled = value;
            txtChassis.Enabled = value;

            // ddl
            ddlMotorVehicleBrand.IsEnabled(value);
            ddlMotorVehicleType.IsEnabled(value);
            ddlMotorVehicleSegment.IsEnabled(value);
            ddlMotorVehicleModel.IsEnabled(value);
            ddlFuelType.IsEnabled(value);
            ddlMotorProvince.IsEnabled(value);
            ddlVehicleYear.IsEnabled(value);
        }

        #endregion Method
    }
}