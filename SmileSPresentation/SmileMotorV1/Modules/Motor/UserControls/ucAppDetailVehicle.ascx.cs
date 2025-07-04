using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;
namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucAppDetailVehicle : System.Web.UI.UserControl
    {

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }
        #endregion

        #region Method

        /// <summary>
        /// Do Load Detail
        /// </summary>
        /// <param name="application_id">Motor Application ID</param>
        public void DoLoad(int application_id)
        {

            MotorDB db = new MotorDB();
            FunctionManager fm = new FunctionManager();
            List<MotorApplication> lstDetail = new List<MotorApplication>();
            MotorApplication appid = new MotorApplication();
            Address addr = new Address();
            DataCenterDB mdb = new DataCenterDB();

            // Get Motor Application Vehicle Detail
            appid = db.GetMotorApplicationVehicleDetailByApplicationID(application_id);

            // Get Vehicle Registration Province
            addr = mdb.GetProvinceByProvinceID(Convert.ToInt32(appid.Vehicle.VehicleRegistrationProvince_ID));

            // Set Bind Vehicle Detail
            lblVehicleBrand.Text = appid.Vehicle.VehicleBrandDetail;
            lblVehicleType.Text = appid.Vehicle.VehicleTypeDetail;
            lblVehicleSegment.Text = appid.Vehicle.VehicleSegmentDetail;
            lblVehicleModel.Text = appid.Vehicle.VehicleModelDetail;
            lblVehicleCC.Text = appid.Vehicle.VehicleCC;
            lblWeightVehicle.Text = appid.Vehicle.VehicleWeight;
            lblLicensePlate.Text = appid.Vehicle.VehicleRegistrationDetail + " " + appid.Vehicle.VehicleRegistrationNumber;
            lblVehicleYear.Text = appid.Vehicle.VehicleYear;
            lblSeat.Text = appid.Vehicle.VehicleSeat;
            lblEngineNumber.Text = appid.Vehicle.VehicleEngineNumber;
            lblChassisNumber.Text = appid.Vehicle.VehicleChassisNumber;
            lblVehicleProvince.Text = addr.ProvinceDetail;
            lblFuelType.Text = "(" + appid.Vehicle.FuelTypeDetail + ")";
        }
        #endregion

    }
}