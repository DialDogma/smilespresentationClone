using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucAddEditAddress : System.Web.UI.UserControl
    {

        #region Declare

        private Address _address;

        public Address Address
        {
            get
            {

                if (hdfAddressID.Value == "")
                {
                    _address = new Address();
                }
                else
                {
                    DataCenterDB db = new DataCenterDB();
                    int addressID = Convert.ToInt32(hdfAddressID.Value);
                    _address = db.GetAddressByAddressID(addressID);
                }

                //Bind Data
               _address.No = txtNo.Text.Trim();
               _address.Moo = txtMoo.Text.Trim();
               _address.VillageName = txtVillageName.Text.Trim();
               _address.BuildingName = txtBuildingName.Text.Trim();
               _address.Floor = txtFloor.Text.Trim();
               _address.Room = txtRoom.Text.Trim();
               _address.Soi = txtSoi.Text.Trim();
               _address.Road = txtRoad.Text.Trim();
               _address.Province_ID = ddlProvince1.Province_ID;
               _address.District_ID = ddlDistrict.District_ID;
               _address.SubDistrict_ID = ddlSubDistrict1.SubDistrict_ID;
                _address.ZipCode = txtZipCode.Text.Trim();

                return _address;
            }
            set
            {
                //TODO : Set ค่าจาก Address เข้า control
                _address = value;
            }
        }

        #endregion

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                // Set DDL Auto Post Back
                this.ddlProvince1.AutoPostback = true;
                this.ddlDistrict.AutoPostback = true;
                this.ddlSubDistrict1.AutoPostback = true;
            }
        }

        protected void ddlProvince1_SelectChanged1(object sender, EventArgs e)
        {
            // Check Province Value
            if (ddlProvince1.Province_ID == "-1")
            {
                ddlDistrict.DoClear();
                ddlSubDistrict1.DoClear();
                txtZipCode.Text = "";
            }
            else
            {
                ddlDistrict.DoLoadDropDownList(ddlProvince1.Province_ID);
                ddlSubDistrict1.DoClear();
                txtZipCode.Text = "";
            }
        }

        protected void ddlDistrict_SelectChanged(object sender, EventArgs e)
        {
            // Check District Value
            if (ddlDistrict.District_ID == "-1")
            {
                ddlSubDistrict1.DoClear();
                txtZipCode.Text = "";
            }
            else
            {
                ddlSubDistrict1.DoLoadDropdownList(ddlDistrict.District_ID);
                txtZipCode.Text = "";
            }
          
        }              

        protected void ddlSubDistrict1_SelectChanged(object sender, EventArgs e)
        {
            // Check SubDistrict Value
            if (ddlSubDistrict1.SubDistrict_ID == "-1")
            {
                txtZipCode.Text = "";
            }
            else
            {
                txtZipCode.Text = ddlSubDistrict1.DoLoadZipCode(Convert.ToInt32(ddlSubDistrict1.SubDistrict_ID));
            }            
        }

        #endregion

        #region Method

        /// <summary>
        /// Do Load Dropdown List
        /// </summary>
        public void DoLoadDropdownList()
        {
            // Check Count Province To Load
            if (ddlProvince1.Province_ID.Count() == 0)
            {
                ddlProvince1.DoLoadDropdownList();
            }

            ddlProvince1_SelectChanged1(new Object(), new EventArgs());
        }


        /// <summary>
        /// Do Clear Text And Dropdown List
        /// </summary>
        public void Doclear()
        {
            hdfAddressID.Value = "";
            txtBuildingName.Text = "";
            txtFloor.Text = "";
            txtMoo.Text = "";
            txtNo.Text = "";
            txtRoad.Text = "";
            txtRoom.Text = "";
            txtSoi.Text = "";
            txtVillageName.Text = "";
            this.ddlProvince1.Province_ID = "-1";
            this.ddlDistrict.DoClear();
            this.ddlSubDistrict1.DoClear();
            txtZipCode.Text = "";
        }


        /// <summary>
        /// Do Load Address Detail
        /// </summary>
        /// <param name="addressID">Address ID</param>
        public void DoLoadByAddressID(int addressID)
        {
            hdfAddressID.Value = addressID.ToString();
            DataCenterDB db = new DataCenterDB();
            SmileSMotorClassLibrary.Address ad;

            ad = db.GetAddressByAddressID(addressID);

            // Check Count Province Value
            if (ddlProvince1.Province_ID.Count() == 0)
            {
                ddlProvince1.DoLoadDropdownList();
            }
            
            // Bind Address Detail To Control
            this.txtNo.Text = ad.No;
            this.txtMoo.Text = ad.Moo;
            this.txtVillageName.Text = ad.VillageName;
            this.txtBuildingName.Text = ad.BuildingName;
            this.txtFloor.Text = ad.Floor;
            this.txtRoom.Text = ad.Room;
            this.txtSoi.Text = ad.Soi;
            this.txtRoad.Text = ad.Road;
            this.ddlProvince1.Province_ID = ad.Province_ID;

            // Force Selected Changed For Province
            ddlProvince1_SelectChanged1(new Object(), new EventArgs());

            this.ddlDistrict.District_ID = ad.District_ID;
            // Force Selected Changed For District
            ddlDistrict_SelectChanged(new Object(), new EventArgs());

            this.ddlSubDistrict1.SubDistrict_ID = ad.SubDistrict_ID;
            this.txtZipCode.Text = ad.ZipCode;

        }


        /// <summary>
        /// Copy Address Detail In Form
        /// </summary>
        /// <param name="_copy">UC Class Copy</param>
        public void Copyform(ucAddEditAddress _copy)
        {
            // Set Value From User Control
            txtNo.Text = _copy.txtNo.Text;
            txtMoo.Text = _copy.txtMoo.Text;
            txtVillageName.Text = _copy.txtVillageName.Text;
            txtBuildingName.Text = _copy.txtBuildingName.Text;
            txtFloor.Text = _copy.txtFloor.Text;
            txtSoi.Text = _copy.txtSoi.Text;
            txtRoom.Text = _copy.txtRoom.Text;
            txtRoad.Text = _copy.txtRoad.Text;

            if (ddlProvince1.Province_ID.Count() == 0)
            {
                ddlProvince1.DoLoadDropdownList();
            }

            ddlProvince1.Province_ID = _copy.ddlProvince1.Province_ID;
            this.ddlProvince1_SelectChanged1(ddlProvince1.Province_ID, new EventArgs());

            this.ddlDistrict.District_ID = _copy.ddlDistrict.District_ID;
            this.ddlDistrict_SelectChanged(ddlDistrict.District_ID,new EventArgs());

            this.ddlSubDistrict1.SubDistrict_ID = _copy.ddlSubDistrict1.SubDistrict_ID;
            this.txtZipCode.Text = _copy.txtZipCode.Text;

        }


        /// <summary>
        /// Do Enabled Text Box
        /// </summary>
        /// <param name="_bool">True/False</param>
        public void IsEnabled(bool _bool)
        {
            txtBuildingName.Enabled = _bool;
            txtVillageName.Enabled = _bool;
            txtNo.Enabled = _bool;
            txtMoo.Enabled = _bool;
            txtFloor.Enabled = _bool;
            txtRoom.Enabled = _bool;
            txtSoi.Enabled = _bool;
            txtRoad.Enabled = _bool;
            //txtZipCode.Enabled = _bool;
            ddlProvince1.IsEnabled(_bool);
            ddlDistrict.IsEnabled(_bool);
            ddlSubDistrict1.IsEnabled(_bool);
        }


        /// <summary>
        /// Validate Address
        /// </summary>
        /// <returns></returns>
        public string ValidateAddress()
        {
            TransactionManager tm = new TransactionManager();
            string result;

            // Valid From Class
            result = tm.Validate_Address(Address);

            return result;

        }
        #endregion

    }
}