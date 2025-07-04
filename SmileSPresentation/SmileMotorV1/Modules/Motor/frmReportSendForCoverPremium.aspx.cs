using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileMotorV1.Models;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmReportSendForCoverPremium : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //check role = MotorDeveloper OR MotorUnderwrite
                if (HttpContext.Current.User.IsInRole("MotorDeveloper") || HttpContext.Current.User.IsInRole("MotorUnderwrite"))
                {
                    ddlPeriodType1.DoLoadDropdownList("ทั้งหมด");
                    ddlPeriodType1.FindByValue("2");
                    ddlPeriodType1.IsEnabled(false);
                    //ucDatepickerPeriod.DateSelected = Convert.ToDateTime("1" + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString());
                    ddlFirstDayOfMonth.DoLoadDropDownList();
                    DoloadCoverType();
                }
                else
                {
                    Response.Redirect("frmSuccess.aspx?msg=2");
                }
            }
        }

        protected void btnSearch_OnClick(object sender, EventArgs e)
        {
            LoadGridview();
        }

        protected void dgvMain_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Paging
            dgvMain.PageIndex = e.NewPageIndex;
            LoadGridview();
        }

        protected void btnExportExcel_OnClick(object sender, EventArgs e)
        {
            Export();
        }

        #endregion Event

        #region Method

        private void DoloadCoverType()
        {
            ddlCoverType.Items.Clear();
            ddlCoverType.Items.Insert(0, new ListItem("ทั้งหมด", "-1"));
            ddlCoverType.Items.Insert(1, new ListItem("ลูกค้าต่ออายุ", "3"));
            ddlCoverType.Items.Insert(2, new ListItem("ลูกค้าใหม่", "2"));
        }

        private void LoadGridview()
        {
            int periodTypeId = ddlPeriodType1.PeriodType_ID;
            int motorApplicationContractTypeId = Convert.ToInt32(ddlCoverType.SelectedItem.Value);
            DateTime? period = cDate.ToDate(ddlFirstDayOfMonth.ddl.SelectedValue);
            if (periodTypeId != 2)
            {
                period = null;
            }
            using (var db = new MotorV1Entities())
            {
                var CoverTypeId = ddlCoverType.SelectedItem.Value;
                var lst = db.usp_MotorApplication_Report_SendForCoverPremium_Select(periodTypeId, period, motorApplicationContractTypeId).ToList();
                //switch (CoverTypeId)
                //{
                //    case "2":
                //        lst = lst.Where(x => x.RealStartCover == cDate.ToDate(ddlFirstDayOfMonth.ddl.SelectedValue)).ToList();
                //        break;

                //    case "3":
                //        lst = lst.Where(x => x.RealStartCover < cDate.ToDate(ddlFirstDayOfMonth.ddl.SelectedValue)).ToList();

                //        break;

                //    default:
                //        break;
                //}

                var dt = mFunction.ToDataTable(lst);
                mFunction.LoadGridview(dgvMain, dt);
                //if (ddlPeriodType1.PeriodType_ID == 2)
                //{
                //    dgvMain.Columns[3].Visible = true;
                //    dgvMain.Columns[4].Visible = false;
                //}
                //else if (ddlPeriodType1.PeriodType_ID == 3)
                //{
                //    dgvMain.Columns[3].Visible = false;
                //    dgvMain.Columns[4].Visible = true;
                //}
                //else
                //{
                //    dgvMain.Columns[3].Visible = true;
                //    dgvMain.Columns[4].Visible = true;
                //}
            }
        }

        private void Export()
        {
            int periodTypeId = ddlPeriodType1.PeriodType_ID;
            int motorApplicationContractTypeId = Convert.ToInt32(ddlCoverType.SelectedItem.Value);
            DateTime? period = cDate.ToDate(ddlFirstDayOfMonth.ddl.SelectedValue);
            if (periodTypeId != 2)
            {
                period = null;
            }
            using (var db = new MotorV1Entities())
            {
                var lstchk = db.usp_MotorApplication_Report_SendForCoverPremiumExcel_Select(periodTypeId, period, motorApplicationContractTypeId).Count();
                if (lstchk > 0)
                {
                    var lst = db.usp_MotorApplication_Report_SendForCoverPremiumExcel_Select(periodTypeId, period, motorApplicationContractTypeId).Select(x => new
                    {
                        เลขที่นำส่ง = x.SendForCoverHeader_Code
                        ,
                        ประเภทสัญญา = x.MotorApplicationContractTypeDetail
                        ,
                        เลขที่อ้างอิง = x.SendForCoverDetail_Code
                        ,
                        MotorApplicationCode = x.MotorApplication_Code
                        ,
                        ผู้เอาประกันภัย = x.CustomerFullName
                        ,
                        วันที่เริ่มคุ้มครอง = x.StartCoverDate
                        ,
                        วันที่สิ้นสุดความคุ้มครอง = x.EndCoverDate
                                                ,
                        เบี้ย = x.Premium
                        ,
                        แผน = x.ProductDetail
                        ,
                        ReceiveDetailCode = x.ReceiveDetail_Code
                        ,
                        TransferID = x.TransferID
                        ,
                        MatchHeaderID = x.MatchHeaderID
                        ,
                        Bank = x.BankDetail
                        ,
                        BankAccountNo = x.BankAccountNo
                        ,
                        TransactionDateTime = x.TransactionDateTime
                        ,
                        TransferPremiumAmount = x.TransferPremiumAmount
                    }).ToList();
                    mFunction.ExportToExcel(HttpContext.Current, lst, "เบี้ยประกันนำส่งขอความคุ้มครอง", "รายงานเบี้ยประกันนำส่งขอความคุ้มครอง_" + DateTime.Now);
                }
            }
        }

        #endregion Method

        protected void ddlPeriodType1_SelectChanged(object sender, EventArgs e)
        {
            if (ddlPeriodType1.PeriodType_ID == 2)
            {
                trPeriod.Visible = true;
            }
            else
            {
                trPeriod.Visible = false;
            }
        }
    }
}