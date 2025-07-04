using SmileSMotorClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmReportNewApplication : System.Web.UI.Page
    {

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Load DDL
                ddlBranch.DoLoadDropDownlist("ทั้งหมด");
                ddlFirstDayOfMonth.DoLoadDropDownList();
            }
        }

        protected void dgvMain_SelectedIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Paging
            dgvMain.PageIndex = e.NewPageIndex;
            LoadGridview();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Load Gridview
            LoadGridview();
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            // Export
            DoExport();
        }
        #endregion

        #region Method


        private void LoadGridview()
        {
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();
            MotorDB db = new MotorDB();
            DateTime firstPeriod;

            firstPeriod = cDate.ToDate(ddlFirstDayOfMonth.ddl.SelectedValue);

            // ถ้าเลือก DDL First Day Of Month
            if (ddlFirstDayOfMonth.ddl.SelectedValue != "0")
            {
                // ถ้าไม่เลือก Branch
                if (ddlBranch.BranchID == -1)
                {
                    // ถ้าไม่ใส่ พนักงาน
                    if (ucEmployeeSearch1.EmployeeCode == "")
                    {
                        dt = db.GetMotorApplicationNewApp(firstPeriod,
                                    null,
                                    null);
                    }
                    // ถ้าใส่ พนักงาน
                    else
                    {
                        dt = db.GetMotorApplicationNewApp(firstPeriod,                                  
                                    null,
                                    ucEmployeeSearch1.Employee.Employee_ID);
                    }

                }
                // ถ้าเลือก Branch
                else
                {
                    // ถ้าไม่ใส่ พนักงาน
                    if (ucEmployeeSearch1.EmployeeCode == "")
                    {
                        dt = db.GetMotorApplicationNewApp(firstPeriod,
                                    ddlBranch.BranchID,
                                    null);
                    }
                    // ถ้าใส่ พนักงาน
                    else
                    {
                        dt = db.GetMotorApplicationNewApp(firstPeriod,
                                    ddlBranch.BranchID,
                                    ucEmployeeSearch1.Employee.Employee_ID);
                    }
                }
            }

            // ถ้า date ว่าง
            else
            {
                // ถ้าไม่เลือก Branch
                if (ddlBranch.BranchID == -1)
                {
                    // ถ้าไม่ใส่ พนักงาน
                    if (ucEmployeeSearch1.EmployeeCode == "")
                    {
                        dt = db.GetMotorApplicationNewApp(null,
                                    null,
                                    null);
                    }
                    // ถ้าใส่ พนักงาน
                    else
                    {
                        dt = db.GetMotorApplicationNewApp(null,
                                    null,
                                    ucEmployeeSearch1.Employee.Employee_ID);
                    }

                }
                // ถ้าเลือก Branch
                else
                {
                    // ถ้าไม่ใส่ พนักงาน
                    if (ucEmployeeSearch1.EmployeeCode == "")
                    {
                        dt = db.GetMotorApplicationNewApp(null,
                                    ddlBranch.BranchID,
                                    null);
                    }
                    // ถ้าใส่ พนักงาน
                    else
                    {
                        dt = db.GetMotorApplicationNewApp(null,
                                    ddlBranch.BranchID,
                                    ucEmployeeSearch1.Employee.Employee_ID);
                    }
                }
            }

            fm.LoadGridview(dgvMain, dt);
        }

        private void DoExport()
        {
            MotorDB db = new MotorDB();
            ExcelManager em = new ExcelManager();
            DataTable dt = new DataTable();
            DateTime firstPeriod;
            firstPeriod = cDate.ToDate(ddlFirstDayOfMonth.ddl.SelectedValue);


            // ถ้าเลือก DDL First Day Of Month
            if (ddlFirstDayOfMonth.ddl.SelectedValue != "0")
            {
                // ถ้าไม่เลือก Branch
                if (ddlBranch.BranchID == -1)
                {
                    // ถ้าไม่ใส่ พนักงาน
                    if (ucEmployeeSearch1.EmployeeCode == "")
                    {
                        dt = db.GetMotorApplicationFullDetail(firstPeriod,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null);
                    }
                    // ถ้าใส่ พนักงาน
                    else
                    {
                        dt = db.GetMotorApplicationFullDetail(firstPeriod,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    ucEmployeeSearch1.Employee.Employee_ID);
                    }

                }
                // ถ้าเลือก Branch
                else
                {
                    // ถ้าไม่ใส่ พนักงาน
                    if (ucEmployeeSearch1.EmployeeCode == "")
                    {
                        dt = db.GetMotorApplicationFullDetail(firstPeriod,
                                    null,
                                    null,
                                    null,
                                    null,
                                    ddlBranch.BranchID,
                                    null);
                    }
                    // ถ้าใส่ พนักงาน
                    else
                    {
                        dt = db.GetMotorApplicationFullDetail(firstPeriod,
                                    null,
                                    null,
                                    null,
                                    null,
                                    ddlBranch.BranchID,
                                    ucEmployeeSearch1.Employee.Employee_ID);
                    }
                }
            }

            // ถ้า date ว่าง
            else
            {
                // ถ้าไม่เลือก Branch
                if (ddlBranch.BranchID == -1)
                {
                    // ถ้าไม่ใส่ พนักงาน
                    if (ucEmployeeSearch1.EmployeeCode == "")
                    {
                        dt = db.GetMotorApplicationFullDetail(null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null);
                    }
                    // ถ้าใส่ พนักงาน
                    else
                    {
                        dt = db.GetMotorApplicationFullDetail(null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    ucEmployeeSearch1.Employee.Employee_ID);
                    }

                }
                // ถ้าเลือก Branch
                else
                {
                    // ถ้าไม่ใส่ พนักงาน
                    if (ucEmployeeSearch1.EmployeeCode == "")
                    {
                        dt = db.GetMotorApplicationFullDetail(null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    ddlBranch.BranchID,
                                    null);
                    }
                    // ถ้าใส่ พนักงาน
                    else
                    {
                        dt = db.GetMotorApplicationFullDetail(null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    ddlBranch.BranchID,
                                    ucEmployeeSearch1.Employee.Employee_ID);
                    }
                }
            }

            em.DatatableToExcel(HttpContext.Current,
                "รายงาน New Application " + " สาขา " + ddlBranch.BranchDetail,
                dt,
                "รายงาน New Application");
        }


        #endregion

    }
}