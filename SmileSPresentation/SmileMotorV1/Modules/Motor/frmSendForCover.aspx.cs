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
    public partial class frmSendForCover : System.Web.UI.Page
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
            ExportPreview();
        }

        protected void dgvMain_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Paging
            dgvMain.PageIndex = e.NewPageIndex;
            LoadGridview();
        }

        protected void btnExportExcel_OnClick(object sender, EventArgs e)
        {
            LoadGridview();
            Export();
        }

        #endregion Event

        #region Method

        private void DoloadCoverType()
        {
            ddlCoverType.Items.Clear();
            ddlCoverType.Items.Insert(0, new ListItem("ลูกค้าต่ออายุ", "3"));
            ddlCoverType.Items.Insert(1, new ListItem("ลูกค้าใหม่", "2"));
        }

        private void LoadGridview()
        {
            int periodTypeId = ddlPeriodType1.PeriodType_ID;
            DateTime? period = cDate.ToDate(ddlFirstDayOfMonth.ddl.SelectedValue);
            if (periodTypeId != 2)
            {
                period = null;
            }
            using (var db = new MotorV1Entities())
            {
                var CoverTypeId = ddlCoverType.SelectedItem.Value;
                var lst = db.usp_MotorApplicationSendForCover_Select(periodTypeId, period).ToList();
                switch (CoverTypeId)
                {
                    case "2":
                        lst = lst.Where(x => x.RealStartCover == cDate.ToDate(ddlFirstDayOfMonth.ddl.SelectedValue)).ToList();
                        break;

                    case "3":
                        lst = lst.Where(x => x.RealStartCover < cDate.ToDate(ddlFirstDayOfMonth.ddl.SelectedValue)).ToList();
                        break;

                    default:
                        break;
                }

                var dt = mFunction.ToDataTable(lst);
                mFunction.LoadGridview(dgvMain, dt);
                if (ddlPeriodType1.PeriodType_ID == 2)
                {
                    dgvMain.Columns[3].Visible = true;
                    dgvMain.Columns[4].Visible = false;
                }
                else if (ddlPeriodType1.PeriodType_ID == 3)
                {
                    dgvMain.Columns[3].Visible = false;
                    dgvMain.Columns[4].Visible = true;
                }
                else
                {
                    dgvMain.Columns[3].Visible = true;
                    dgvMain.Columns[4].Visible = true;
                }
            }
        }

        private void ExportPreview()
        {
            int periodTypeId = ddlPeriodType1.PeriodType_ID;
            DateTime period = cDate.ToDate(ddlFirstDayOfMonth.ddl.SelectedValue);
            var userId = cFunction.GetCurrentLoginUser_ID(Page);
            using (var db = new MotorV1Entities())
            {
                var lstchk = db.usp_MotorApplicationSendForCover_Select(periodTypeId, period).Count();
                if (lstchk > 0)
                {
                    var CoverTypeId = ddlCoverType.SelectedItem.Value;
                    var lst = db.usp_MotorApplicationSendForCover_Select(periodTypeId, period).ToList();
                    switch (CoverTypeId)
                    {
                        case "2":
                            lst = lst.Where(x => x.RealStartCover == cDate.ToDate(ddlFirstDayOfMonth.ddl.SelectedValue)).ToList();
                            break;

                        case "3":
                            lst = lst.Where(x => x.RealStartCover < cDate.ToDate(ddlFirstDayOfMonth.ddl.SelectedValue)).ToList();
                            break;

                        default:
                            break;
                    }

                    var lstResult = lst.Select(x => new
                    {
                        MotorApplicationCode = x.MotorApplication_Code,
                        ผู้เอาประกันภัย = x.CustomerFullName
                             ,
                        เลขบัตร = x.CustIdentityCard
                             ,
                        ที่อยู่ปัจจุบัน = x.CusCurrentAddress_FullDetail
                             ,
                        ผู้รับผลประโยชน์ = x.MotorApplicationHeirDetail
                             ,
                        ยี่ห้อ = x.VehicleBrandDetail
                             ,
                        รุ่น = x.VehicleModelDetail
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
                        วันที่เริ่มคุ้มครอง = x.StartCover
                             ,
                        วันที่สิ้นสุดความคุ้มครอง = x.EndCover
                             //, งวดคุ้มครอง =x.PeriodFrom
                             //, ถึงงวดคุ้มครอง = x.PeriodTo
                             ,
                        เบี้ย = x.PremiumPerMonth
                             ,
                        แผน = x.ProductDetail
                    }).ToList();
                    if (lstResult.Count > 0)
                    {
                        mFunction.ExportToExcel(HttpContext.Current, lstResult, "ตรวจสอบขอความคุ้มครอง" + ddlPeriodType1.PeriodTypeDetail, "ตรวจสอบขอความคุ้มครอง_" + DateTime.Now);
                    }
                }
            }
        }

        private void Export()
        {
            int periodTypeId = ddlPeriodType1.PeriodType_ID;
            DateTime period = cDate.ToDate(ddlFirstDayOfMonth.ddl.SelectedValue);
            var userId = cFunction.GetCurrentLoginUser_ID(Page);
            int coverTypeId = Convert.ToInt32(ddlCoverType.SelectedItem.Value);
            using (var db = new MotorV1Entities())
            {
                db.Database.CommandTimeout = 600;
                var lstchk = db.usp_MotorApplicationSendForCover_Select(periodTypeId, period).Count();
                if (lstchk > 0)
                {
                    var SendForCoverHeader_ID = db.usp_MotorApplicationSendForCoverV2_Insert(periodTypeId, period, userId, coverTypeId).FirstOrDefault().Value;
                    if (SendForCoverHeader_ID != 0)
                    {
                        var lst = db.usp_MotorApplicationSendForCoverExcel_Select(SendForCoverHeader_ID).Select(x => new
                        {
                            เลขที่นำส่ง = x.SendForCoverHeader_Code
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
                        }).ToList();

                        mFunction.ExportToExcel(HttpContext.Current, lst, "นำส่งขอความคุ้มครอง" + ddlPeriodType1.PeriodTypeDetail, "นำส่งขอความคุ้มครอง_" + DateTime.Now);
                    }
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