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
    public partial class frmReportByApproveDate : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //check role = MotorDeveloper OR MotorUnderwrite
                if (HttpContext.Current.User.IsInRole("MotorDeveloper") || HttpContext.Current.User.IsInRole("MotorUnderwrite"))
                {
                    // Set Text , Load UC
                    ucDatepickerFrom.SetPlaceholder = "จากวันที่";
                    ucDatepickerTo.SetPlaceholder = "ถึงวันที่";
                    ddlBranch.DoLoadDropDownlist();
                    ucDatepickerFrom.DateSelected = DateTime.Now;
                    ucDatepickerTo.DateSelected = DateTime.Now;
                }
                else
                {
                    Response.Redirect("frmSuccess.aspx?msg=2");
                }
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
            // ค้นหา
            LoadGridview();
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            // Export Excel
            DoExport();
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Load Gridview
        /// </summary>
        private void LoadGridview()
        {
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();
            MotorDB db = new MotorDB();
            DateTime dateFrom, dateTo;

            // Get Date From UC
            if (ucDatepickerFrom.DateSelected.HasValue == true)
            {
                dateFrom = cDate.ToDate(ucDatepickerFrom.DateSelected.Value);
            }
            else
            {
                dateFrom = DateTime.Now;
            }

            if (ucDatepickerTo.DateSelected.HasValue == true)
            {
                dateTo = cDate.ToDate(ucDatepickerTo.DateSelected.Value);
            }
            else
            {
                dateTo = DateTime.Now;
            }

            // ถ้า date ไม่เท่ากับค่าว่าง
            if (dateFrom != null && dateTo != null)
            {
                // ถ้าไม่เลือก Branch
                if (ddlBranch.BranchID == -1)
                {
                    // ถ้าไม่ใส่ พนักงาน
                    if (ucEmployeeSearch1.EmployeeCode == "")
                    {
                        dt = db.GetMotorApplicationByApproveDate(
                                    dateFrom,
                                    dateTo,
                                    null,
                                    null);
                    }
                    // ถ้าใส่ พนักงาน
                    else
                    {
                        dt = db.GetMotorApplicationByApproveDate(
                                    dateFrom,
                                    dateTo,
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
                        dt = db.GetMotorApplicationByApproveDate(
                                    dateFrom,
                                    dateTo,
                                    ddlBranch.BranchID,
                                    null);
                    }
                    // ถ้าใส่ พนักงาน
                    else
                    {
                        dt = db.GetMotorApplicationByApproveDate(
                                    dateFrom,
                                    dateTo,
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
                        dt = db.GetMotorApplicationByApproveDate(
                                    null,
                                    null,
                                    null,
                                    null);
                    }
                    // ถ้าใส่ พนักงาน
                    else
                    {
                        dt = db.GetMotorApplicationByApproveDate(
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
                        dt = db.GetMotorApplicationByApproveDate(
                                    null,
                                    null,
                                    ddlBranch.BranchID,
                                    null);
                    }
                    // ถ้าใส่ พนักงาน
                    else
                    {
                        dt = db.GetMotorApplicationByApproveDate(
                                    null,
                                    null,
                                    ddlBranch.BranchID,
                                    ucEmployeeSearch1.Employee.Employee_ID);
                    }
                }
            }

            // โหลด
            fm.LoadGridview(dgvMain, dt);
        }

        /// <summary>
        /// Export Excel
        /// </summary>
        private void DoExport()
        {
            MotorDB db = new MotorDB();
            ExcelManager em = new ExcelManager();
            DataTable dt = new DataTable();
            DateTime dateFrom, dateTo;

            // Get Date From UC
            if (ucDatepickerFrom.DateSelected.HasValue == true)
            {
                dateFrom = cDate.ToDate(ucDatepickerFrom.DateSelected.Value);
            }
            else
            {
                dateFrom = DateTime.Now;
            }

            if (ucDatepickerTo.DateSelected.HasValue == true)
            {
                dateTo = cDate.ToDate(ucDatepickerTo.DateSelected.Value);
            }
            else
            {
                dateTo = DateTime.Now;
            }

            // ถ้า date ไม่เท่ากับค่าว่าง
            if (dateFrom != null && dateTo != null)
            {
                // ถ้าไม่เลือก Branch
                if (ddlBranch.BranchID == -1)
                {
                    // ถ้าไม่ใส่ พนักงาน
                    if (ucEmployeeSearch1.EmployeeCode == "")
                    {
                        dt = db.GetMotorApplicationByApproveDate(
                                    dateFrom,
                                    dateTo,
                                    null,
                                    null);
                    }
                    // ถ้าใส่ พนักงาน
                    else
                    {
                        dt = db.GetMotorApplicationByApproveDate(
                                    dateFrom,
                                    dateTo,
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
                        dt = db.GetMotorApplicationByApproveDate(
                                    dateFrom,
                                    dateTo,
                                    ddlBranch.BranchID,
                                    null);
                    }
                    // ถ้าใส่ พนักงาน
                    else
                    {
                        dt = db.GetMotorApplicationByApproveDate(
                                    dateFrom,
                                    dateTo,
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
                        dt = db.GetMotorApplicationByApproveDate(
                                    null,
                                    null,
                                    null,
                                    null);
                    }
                    // ถ้าใส่ พนักงาน
                    else
                    {
                        dt = db.GetMotorApplicationByApproveDate(
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
                        dt = db.GetMotorApplicationByApproveDate(
                                    null,
                                    null,
                                    ddlBranch.BranchID,
                                    null);
                    }
                    // ถ้าใส่ พนักงาน
                    else
                    {
                        dt = db.GetMotorApplicationByApproveDate(
                                    null,
                                    null,
                                    ddlBranch.BranchID,
                                    ucEmployeeSearch1.Employee.Employee_ID);
                    }
                }
            }

            // Export
            em.DatatableToExcel(HttpContext.Current, "รายงานตามวันที่อนุมัติ " + ucDatepickerFrom.DateSelected + " ถึง " + ucDatepickerTo.DateSelected
                + " สาขา " + ddlBranch.BranchDetail,
                dt,
                "รายงานตามวันที่อนุมัติ");
        }

        #endregion Method
    }
}