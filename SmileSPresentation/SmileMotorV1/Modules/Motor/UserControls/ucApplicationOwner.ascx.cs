using SmileSMotorClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucApplicationOwner : System.Web.UI.UserControl
    {
        #region Declare

        private int _zebraID;
        private int _ownerTypeID;
        private Employee _employee;

        /// <summary>
        /// Property Get/Set Owner Type ID
        /// </summary>
        public int OwnerTypeID
        {
            get
            {
                _ownerTypeID = this.ddlOwnerType.OwnerType_ID;
                return _ownerTypeID;
            }

            set
            {
                _ownerTypeID = value;
                this.ddlOwnerType.OwnerType_ID = value;
            }
        }

        /// <summary>
        ///  Property Get/Set Zebra ID
        /// </summary>
        public int ZebraID
        {
            get
            {
                _zebraID = this.ddlZebra.Zebra_ID;
                return _zebraID;
            }

            set
            {
                _zebraID = value;
                this.ddlZebra.Zebra_ID = value;
            }
        }

        /// <summary>
        /// Property Get/Set Employee Class
        /// </summary>
        public Employee Employee
        {
            get
            {
                DataCenterDB db = new DataCenterDB();
                FunctionManager fm = new FunctionManager();
                List<Employee> lst = new List<Employee>();

                lst = fm.DataTableToList<Employee>(db.GetEmployeeSearch(this.ucEmployeeSearch1.EmployeeCode));

                _employee = lst[0];

                return _employee;
            }

            set
            {
                _employee = value;
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

        #endregion Event

        #region Method

        /// <summary>
        /// Do Load Detail
        /// </summary>
        /// <param name="_motorApplication_ID">Motor Application ID</param>
        public void DoLoad(int _motorApplication_ID)
        {
            MotorDB db = new MotorDB();
            FunctionManager fm = new FunctionManager();
            List<Employee> lst = new List<Employee>();

            // Get Motor Application Owner To List
            lst = fm.DataTableToList<Employee>(db.GetMotorApplicationOwner(_motorApplication_ID));

            // Set Bind Data
            ucEmployeeSearch1.EmployeeCode = lst[0].EmployeeCode;
            ucEmployeeSearch1.txtEmployeeCode_TextChanged(new object(), new EventArgs());
            ddlOwnerType.OwnerType_ID = lst[0].MotorApplicationOwnerType_ID;
            ddlZebra.Zebra_ID = lst[0].Zebra_ID;
        }

        /// <summary>
        /// Do Load Dropdown List
        /// </summary>
        public void DoloadDropdownlist()
        {
            ddlOwnerType.DoLoadDropdownList();
            ddlZebra.DoLoadDropdownList();
        }

        /// <summary>
        /// Do Set Fix Type Owner
        /// </summary>
        public void DoSetTypeOwner()
        {
            ddlOwnerType.OwnerType_ID = 2; // 2 คือ เจ้าของผลงาน
            ddlOwnerType.OwnerTypeEnable.Enabled = false;
        }

        /// <summary>
        /// Validate Owner
        /// </summary>
        /// <returns></returns>
        public string ValidateOwner()
        {
            string result;
            TransactionManager tm = new TransactionManager();

            // Valid From Class
            result = tm.Validate_Owner(OwnerTypeID.ToString(), this.ucEmployeeSearch1.EmployeeCode, ddlZebra.Zebra_ID.ToString());

            return result;
        }

        public void LoadOwnerDefault()
        {
            this.ucEmployeeSearch1.loadDefault(this.Page);
        }

        #endregion Method
    }
}