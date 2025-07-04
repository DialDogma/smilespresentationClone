using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucAddEditApplication : System.Web.UI.UserControl
    {
        #region Declare

        private MotorApplication _motorApplication;
        private Product _product;

        private int _Product_ID;
        private int _ProductType_ID;

        public int ProductID
        {
            get
            {
                try
                {
                    _Product_ID = ddlMotorProduct.Product_ID;
                }
                catch
                {
                    _Product_ID = 0;
                }

                return _Product_ID;
            }
            set
            {
                _Product_ID = value;
                ddlMotorProduct.Product_ID = value;
            }
        }

        public int ProductTypeID
        {
            get
            {
                _ProductType_ID = ddlMotorProductType.ProductType_ID;
                return _ProductType_ID;
            }

            set
            {
                _ProductType_ID = value;
                ddlMotorProductType.ProductType_ID = value;
            }
        }

        public MotorApplication MotorApplication
        {
            get
            {
                _motorApplication = new MotorApplication();
                _motorApplication.Product = new Product();

                _motorApplication.MotorApplication_Code = txtMotorAppCode.Text.Trim();

                _motorApplication.InsuranceCompany_ID = ddlMotorCompanyInsurance.InsuranceCompany_ID;
                _motorApplication.StartCover = dtpStartCoverDate.DateSelected.Value;
                _motorApplication.EndCover = dtpStartCoverDate.DateSelected.Value;
                //_motorApplication.EndCover = cDate.DateAdd(DateInterval.Year, dtpStartCoverDate.DateSelected.Value, 1);
                _motorApplication.Product.ProductType_ID = ddlMotorProductType.ProductType_ID;

                //เช็คว่า dropdownlist มี item อยู่รึเปล่า?
                if (ddlMotorProduct.ddlProduct.Items.Count == 0)
                {
                    _motorApplication.Product.Product_ID = 0;
                }
                else
                {
                    _motorApplication.Product.Product_ID = ddlMotorProduct.Product_ID;
                }

                _motorApplication.MotorApplicationContractType_ID = ddlContractType1.ContractType_ID;
                _motorApplication.MotorApplicationClaimType_ID = ddlMotorClaimType1.MotorClaimType_ID;
                _motorApplication.MotorApplicationHeirDetail = txtHeir.Text.Trim();

                //TODO Assign value

                return _motorApplication;
            }
            set
            {
                _motorApplication = value;
            }
        }

        public Product Product
        {
            get
            {
                _product = GetProduct(ddlMotorProduct.Product_ID);
                return _product;
            }
            set
            {
                _product = value;
            }
        }

        #endregion Declare

        #region RaiseEvent

        public event EventHandler eMotorCompanyInsuranceSelectChanged;

        public event EventHandler eProductSelectChanged;

        public event EventHandler eProductTypeSelectChanged;

        #endregion RaiseEvent

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                // Set Auto Post Back
                this.ddlMotorCompanyInsurance.AutoPostback = true;
                this.ddlMotorProductType.AutoPostback = true;
            }
        }

        protected void ddlMotorCompanyInsurance_SelectChanged(object sender, EventArgs e)
        {
            // Clear And Load DDL
            this.ddlMotorProduct.DoClear();
            this.ddlMotorProduct.DoLoadDropdownList(ddlMotorCompanyInsurance.InsuranceCompany_ID, ddlMotorProductType.ProductType_ID);

            EventHandler handler = eMotorCompanyInsuranceSelectChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void ddlMotorProductType_SelectChanged(object sender, EventArgs e)
        {
            // Clear And Load DDL
            this.ddlMotorProduct.DoClear();
            this.ddlMotorProduct.DoLoadDropdownList(ddlMotorCompanyInsurance.InsuranceCompany_ID, ddlMotorProductType.ProductType_ID);
            EventHandler handler;

            _ProductType_ID = ddlMotorProductType.ProductType_ID;

            handler = eProductTypeSelectChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void ddlMotorProduct_SelectChanged(object sender, EventArgs e)
        {
            // Set And Get Product
            EventHandler handler;
            _Product_ID = ddlMotorProduct.Product_ID;

            if (_Product_ID != -1)
            {
                Product = GetProduct(_Product_ID);
            }

            handler = eProductSelectChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        //เมื่อมีการเลือกประเภทสัญญา
        protected void ddlContractType1_eContractTypeSelectChanged(object sender, EventArgs e)
        {
            this.ddlMotorClaimType1.SetDefault(); // set ให้เป็นซ่อมอู่
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Get Product
        /// </summary>
        /// <param name="_Product_ID">Product ID</param>
        /// <returns></returns>
        private Product GetProduct(int _Product_ID)
        {
            Product p = new Product();
            DataCenterDB db = new DataCenterDB();

            p = cData.DataTableToList<Product>(db.GetProduct(_Product_ID))[0];

            return p;
        }

        /// <summary>
        /// Do Load Dropdown List
        /// </summary>
        public void DoloadDropdownList()
        {
            ddlMotorCompanyInsurance.DoLoadDropdownList();
            ddlMotorProductType.DoLoadDropdownList();
            ddlMotorClaimType1.DoLoadDropdownList();
            ddlContractType1.DoLoadDropDownList();
        }

        /// <summary>
        /// Do Load Motor Detail
        /// </summary>
        /// <param name="motorapplicationID">Motor Application ID</param>
        public void DoLoad(int motorapplicationID)
        {
            MotorApplication mt = new MotorApplication();
            MotorDB db = new MotorDB();

            // Get Motor App Detail
            mt = db.GetMotorApplicationByMotorApplicationID(motorapplicationID);

            // Load Dropdown Before Bind Data
            ddlContractType1.DoLoadDropDownList();
            ddlMotorCompanyInsurance.DoLoadDropdownList();
            ddlMotorClaimType1.DoLoadDropdownList();
            ddlMotorProductType.DoLoadDropdownList();

            // Set Detail
            txtMotorAppCode.Text = mt.MotorApplication_Code;
            ddlContractType1.ContractType_ID = mt.MotorApplicationContractType_ID;
            ddlContractType1.IsEnabled(false);
            ddlMotorClaimType1.MotorClaimType_ID = mt.MotorApplicationClaimType_ID;
            dtpStartCoverDate.DateSelected = mt.StartCover;

            ddlMotorCompanyInsurance.InsuranceCompany_ID = mt.InsuranceCompany_ID;

            ddlMotorProductType.ProductType_ID = mt.Product.ProductType_ID;
            ddlMotorProductType_SelectChanged(new Object(), new EventArgs());
            ddlMotorProduct.Product_ID = mt.Product.Product_ID;
            txtHeir.Text = mt.MotorApplicationHeirDetail;
        }

        public void ValidateContractType(int contractTypeId)

        {
            if (contractTypeId == 3)
            {
            }
        }

        /// <summary>
        /// Set Enable Text Box And DDL
        /// </summary>
        /// <param name="value">True/False</param>
        public void IsEnabled(bool value)
        {
            txtMotorAppCode.Enabled = value;
            ddlContractType1.IsEnabled(value);
            ddlMotorClaimType1.IsEnabled(value);
            ddlMotorCompanyInsurance.IsEnabled(value);
            ddlMotorProduct.IsEnabled(value);
            ddlMotorProductType.IsEnabled(value);
            dtpStartCoverDate.IsEnabled(value);
            txtHeir.Enabled = value;
        }

        /// <summary>
        /// Set Enable Text Box Motor App
        /// </summary>
        /// <param name="value">True/False</param>
        public void IsEnabledMotorAppCode(bool value)
        {
            txtMotorAppCode.Enabled = value;
        }

        /// <summary>
        /// Validate Motor Application
        /// </summary>
        /// <returns></returns>
        public string ValidateApplication()
        {
            string result;
            TransactionManager tm = new TransactionManager();

            result = tm.Validate_Application(MotorApplication);

            return result;
        }

        /// <summary>
        /// Validate Start Cover
        /// </summary>
        /// <returns></returns>
        public bool IsValidateStartCover()
        {
            bool result;

            result = dtpStartCoverDate.IsValidate();

            return result;
        }

        ///// <summary>
        ///// Set Date Picker This Today
        ///// </summary>
        //public void SetDateSelectedToday()
        //{
        //    //หากเกินวันที่10มีผลคุ้มครองวันที่1เดือนถัดไป
        //    DateTime d1;
        //    if (DateTime.Today.Day > 10)
        //    {
        //        var d2 = DateTime.Today.AddMonths(1);
        //        d1 = Convert.ToDateTime("1/" + d2.Month + "/" + (d2.Year + 543));
        //    }
        //    else
        //    {
        //        d1 = Convert.ToDateTime("1/" + DateTime.Today.Month + "/" + (DateTime.Today.Year + 543));
        //    }

        //    this.dtpStartCoverDate.DateSelected = d1;
        //    //this.dtpStartCoverDate.DateSelected = DateTime.Today;
        //}

        //TODO : 28-2-2562
        /// <summary>
        /// Set Date Picker
        /// </summary>
        public void SetDateSelectedToday()
        {
            //หาก;วันที่เป็นวันที่ 1 ถึง 25 ให้แสดงเป็นวันที่ 1 ของเดือนถัดไป
            DateTime d1;
            if (DateTime.Today.Day >= 1 && DateTime.Today.Day <= 25)
            {
                var d2 = DateTime.Today.AddMonths(1);
                d1 = Convert.ToDateTime("1/" + d2.Month + "/" + (d2.Year + 543));
            }
            //หาก;วันที่เป็นวันที่ 26 ถึง n ให้แสดงเป็นวันที่ 1 ของ2เดือนถัดไป
            else
            {
                var d2 = DateTime.Today.AddMonths(2);
                d1 = Convert.ToDateTime("1/" + d2.Month + "/" + (d2.Year + 543));
            }

            this.dtpStartCoverDate.DateSelected = d1;
            //this.dtpStartCoverDate.DateSelected = DateTime.Today;
        }

        //comment 27-2-2561 boom
        ///// <summary>
        ///// Set Date Picker This Today
        ///// </summary>
        //public void SetDateSelectedToday()
        //{
        //    //หากเกินวันที่10มีผลคุ้มครองวันที่1เดือนถัดไป

        //    var d1 = Convert.ToDateTime(DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + (DateTime.Today.Year + 543));

        //    this.dtpStartCoverDate.DateSelected = d1;
        //    //this.dtpStartCoverDate.DateSelected = DateTime.Today;
        //}

        /// <summary>
        /// สำหรับเทส
        /// </summary>
        public void SetDataTest()
        {
            ddlContractType1.ContractType_ID = 2;
            ddlMotorClaimType1.MotorClaimType_ID = 2;
            ddlMotorCompanyInsurance.InsuranceCompany_ID = 5;

            ddlMotorCompanyInsurance_SelectChanged(new Object(), new EventArgs());
            ddlMotorProductType.ProductType_ID = 4;

            ddlMotorProductType_SelectChanged(new Object(), new EventArgs());
            ddlMotorProduct.Product_ID = 7;
            txtHeir.Text = "ทดสอบผู้รับผลประโยชน์";
        }

        public void RemoveContractTypeItemIndex(byte val)
        {
            ddlContractType1.RemoveItemIndex(val);
        }

        #endregion Method
    }
}