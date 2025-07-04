using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmShowDetailByEmpID : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                this.ddlMotorStatus1.DoLoadDropdownlistAll();
            }

            if (Request.QueryString["empid"] != string.Empty)
            {
                            
                //ถอดรหัส Base64
                FunctionManager fm = new FunctionManager();
                int empID = Convert.ToInt32(fm.Base64Decrypt((Request.QueryString["empid"])));
                int statusID = Convert.ToInt32(ddlMotorStatus1.ddl.SelectedValue);

                lblName.Text = GetEmpID(empID); 
                LoadGridview(empID, statusID);
            }

        }
                
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ////ส่ง query string 2 ตัว
                string motorid = dgvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();

                ////EncryptID
                FunctionManager fm = new FunctionManager();

                string Encrypt = fm.Base64Encrypt(motorid);

                e.Row.Attributes.Add("onclick", "window.open('" + "frmAppDetail.aspx?" + "code" + "=" + Encrypt + "')");
                e.Row.ToolTip = "คลิกเพื่อเปิดดูรายละเอียด Application";
            }
        }

        
        #endregion

        #region Method
        public void LoadGridview(int empid, int statusid)
        {
            FunctionManager fm = new FunctionManager();
            MotorDB mdb = new MotorDB();

            fm.LoadGridview(dgvMain, mdb.GetMotorApplicationByEmployeeIDAndStatusID(empid, statusid),"MotorApplication_ID");
        }

        private string GetEmpID(int empid)
        {
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();
            Employee emp = new Employee();

            //TODO ListEmployee
            emp = db.GetEmployeeByEmployeeID(empid);
            
            return emp.Person.TitleDetail + emp.Person.FirstName + ' ' + emp.Person.LastName;
        }
        #endregion
    }
}