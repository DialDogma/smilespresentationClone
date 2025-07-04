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
    public partial class frmReportSendForCover : System.Web.UI.Page
    {
        private int? dateCase = null;
        private int? periodTypeId = null;
        private DateTime? periodFrom = null;
        private DateTime? periodTo = null;
        private DateTime? startCoverDate = null;
        private DateTime? endCoverDate = null;
        private DateTime? createdDateFrom = null;
        private DateTime? createdDateTo = null;

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
                    ucDatepickerStartCoverDate.DateSelected = Convert.ToDateTime("1" + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString());
                    ucDatepickerEndCoverDate.DateSelected = Convert.ToDateTime(DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString());
                    ucDatepickerCreatedDateFrom.DateSelected = Convert.ToDateTime("1" + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString());
                    ucDatepickerCreatedDateTo.DateSelected = Convert.ToDateTime(DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString());
                    ddlFirstDayOfMonthFrom.DoLoadDropDownList();
                    ddlFirstDayOfMonthTo.DoLoadDropDownList();
                    DateCase();
                }
                else
                {
                    Response.Redirect("frmSuccess.aspx?msg=2");
                }
            }
        }

        protected void btnSearch_OnClick(object sender, EventArgs e)
        {
            DateCase();
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
            DateCase();
            Export();
            //LoadGridview();
        }

        protected void ddlPeriodType1_SelectChanged(object sender, EventArgs e)
        {
            if (ddlPeriodType1.PeriodType_ID == 2)
            {
                trPeriod1.Visible = true;
                trPeriod2.Visible = false;
            }
            else
            {
                trPeriod1.Visible = false;
                trPeriod2.Visible = true;
            }
        }

        protected void rdoDateCase1_CheckedChanged(object sender, EventArgs e)
        {
            DateCase();
        }

        protected void rdoDateCase2_CheckedChanged(object sender, EventArgs e)
        {
            DateCase();
        }

        #endregion Event

        #region Method

        private void LoadGridview()
        {
            using (var db = new MotorV1Entities())
            {
                var lst = db.usp_MotorApplication_Report_SendForCover_Select(dateCase, periodTypeId, periodFrom, periodTo, startCoverDate, endCoverDate, createdDateFrom, createdDateTo).ToList();

                var dt = mFunction.ToDataTable(lst);
                mFunction.LoadGridview(dgvMain, dt);
            }
        }

        private void Export()
        {
            var userId = cFunction.GetCurrentLoginUser_ID(Page);
            using (var db = new MotorV1Entities())
            {
                var lstxx = db.usp_MotorApplication_Report_SendForCoverExcel_Select(dateCase, periodTypeId, periodFrom, periodTo, startCoverDate, endCoverDate, createdDateFrom, createdDateTo).Select(x => new
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
                    เลขบัตร = x.CustomerIDCardNo
                    ,
                    ที่อยู่ปัจจุบัน = x.CustCurrentAddress
                    ,
                    ผู้รับผลประโยชน์ = x.HeirFullName
                    ,
                    ยี่ห้อ = x.VehicleBrand
                    ,
                    รุ่น = x.VehicleModel
                    ,
                    รหัสรถ = x.VehicleSegmentCode
                    ,
                    ป้ายทะเบียน = x.LicencePlateNo
                    ,
                    เลขตัวถัง = x.VehicleChassisNumber
                    ,
                    เลขตัวเครื่อง = x.VehicleEngineNumber
                    ,
                    ปีจดทะเบียน = x.VehicleYear
                    ,
                    CC = x.VehicleCC
                    ,
                    วันที่เริ่มคุ้มครอง = x.StartCoverDate
                    ,
                    วันที่สิ้นสุดความคุ้มครอง = x.EndCoverDate
                    //, งวดคุ้มครอง =x.PeriodFrom
                    //, ถึงงวดคุ้มครอง = x.PeriodTo
                    ,
                    เบี้ย = x.Premium
                    ,
                    แผน = x.ProductDetail
                    //,
                    //ReceiveDetailCode = x.ReceiveDetail_Code
                    //,
                    //TransferID = x.TransferID
                    //,
                    //MatchHeaderID = x.MatchHeaderID
                    //,
                    //Bank = x.BankDetail
                    //,
                    //BankAccountNo = x.BankAccountNo
                    //,
                    //TransactionDateTime = x.TransactionDateTime
                    //,
                    //TransferPremiumAmount = x.TransferPremiumAmount
                }).ToList();
                mFunction.ExportToExcel(HttpContext.Current, lstxx, "รายงานนำส่งขอความคุ้มครอง", "รายงานนำส่งขอความคุ้มครอง_" + DateTime.Now);
            }
        }

        private void DateCase()
        {
            if (rdoDateCase1.Checked == true)
            {
                dateCase = 1;
                periodTypeId = ddlPeriodType1.PeriodType_ID;
                if (periodTypeId == 2)
                {
                    periodFrom = cDate.ToDate(ddlFirstDayOfMonthFrom.ddl.SelectedValue);
                    periodTo = cDate.ToDate(ddlFirstDayOfMonthTo.ddl.SelectedValue);
                    startCoverDate = null;
                    endCoverDate = null;
                    createdDateFrom = null;
                    createdDateTo = null;
                }
                else
                {
                    periodFrom = null;
                    periodTo = null;
                    startCoverDate = ucDatepickerStartCoverDate.DateSelected.Value;
                    endCoverDate = ucDatepickerEndCoverDate.DateSelected.Value;
                    createdDateFrom = null;
                    createdDateTo = null;
                }
            }
            else
            {
                dateCase = 2;
                periodTypeId = null;
                periodFrom = null;
                periodTo = null;
                startCoverDate = null;
                endCoverDate = null;
                createdDateFrom = ucDatepickerCreatedDateFrom.DateSelected.Value;
                createdDateTo = ucDatepickerCreatedDateTo.DateSelected.Value;
            }
        }

        #endregion Method
    }
}