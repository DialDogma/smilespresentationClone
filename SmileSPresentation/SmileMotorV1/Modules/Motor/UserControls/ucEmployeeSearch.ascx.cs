using SmileSMotorClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucEmployeeSearch : System.Web.UI.UserControl
    {
        #region Declare
        private string _employeeCode;
        private Employee _employee;

        public string EmployeeCode
        {
            get
            {
                _employeeCode = txtEmployeeCode.Text.Trim();
                return _employeeCode;
            }
            set
            {
                txtEmployeeCode.Text = value;
                _employeeCode = value;
            }
        }

        public Employee Employee
        {
            get
            {
                DataCenterDB db = new DataCenterDB();
                FunctionManager fm = new FunctionManager();
                List<Employee> lst = new List<Employee>();

                lst = fm.DataTableToList<Employee>(db.GetEmployeeSearch(EmployeeCode));

                _employee = lst[0];

                return _employee;
            }

            set
            {
                _employee = value;
            }
        }

        #endregion
               
        #region RaiseEvent

        //public event EventHandler eSelectedChanged;

        #endregion

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // Set CSS Style
                btnSearch.CssClass = "btn btn-info btn-sm";
                btnClose.CssClass = "btn btn-danger btn-xs";
                
            }
        }

        protected void dgvMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Index Selected
            int index = dgvMain.SelectedIndex;

            // ?
            txtEmployeeCode.Text = HttpUtility.HtmlEncode(dgvMain.Rows[index].Cells[0].Text);
            txtEmployeeCode_TextChanged(new Object(), new EventArgs());
            ClearSelectedIndex();
            txtEmployeeCode.Focus();
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Set Att On Row Click
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(dgvMain, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "คลิกเพื่อเลือกข้อมูล";
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Load Gridview
            LoadGridView();

            // Show Modal
            mpe.Show();

            // Set Visible 
            btnClose.Visible = true;
        }

        public void txtEmployeeCode_TextChanged(object sender, EventArgs e)
        {
            // ถ้า Text เท่ากับ 5 ตัวอักษร
            if (txtEmployeeCode.Text.Trim().Length == 5)
            {
                DataCenterDB db = new DataCenterDB();

                // Get Employee Name
                lblFullName.Text = db.GetEmployeeNameByEmployeeCode(txtEmployeeCode.Text.Trim());
            }

            // ถ้าเกิน
            else
            {
                lblFullName.Text = "";
            }

            txtEmployeeCode.Focus();
        }

        public void loadDefault(Page p)
        {
            DataCenterDB db = new DataCenterDB();
            Employee Emp = new Employee();
            //get Employee Code
            Emp = db.GetEmployeeByEmployeeID(cFunction.GetCurrentEmployeeID(p));

           
            txtEmployeeCode.Text = Emp.EmployeeCode;
            // Get Employee Name
            lblFullName.Text = db.GetEmployeeNameByEmployeeCode(Emp.EmployeeCode);
        }
        #endregion

        #region Method

        /// <summary>
        /// Load Gridview
        /// </summary>
        private void LoadGridView()
        {
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();

            // Get Employee
            dt = db.GetEmployeeSearch(txtSearch.Text.Trim());

            // dt น้อยกว่า 30 รายการ ?
            if (dt.Rows.Count < 30)
            {
                // Load Gridview
                fm.LoadGridview(dgvMain, dt);                
            }

            // dt เกิน 30 รายการ
            else
            {
                cFunction.AlertJavaScript(this.Page,"พบข้อมูลเกิน 30 รายการ");
            }
        }

        /// <summary>
        /// Clear Select Index Gridview
        /// </summary>
        private void ClearSelectedIndex()
        {
            txtSearch.Text = string.Empty;
            dgvMain.SelectedIndex = -1;
            dgvMain.DataBind();

        }
  
        #endregion

    }
}