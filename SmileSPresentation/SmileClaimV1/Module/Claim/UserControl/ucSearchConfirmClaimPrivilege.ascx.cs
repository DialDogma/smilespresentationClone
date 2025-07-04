using SmileClaimV1.HCIService;
using SmileSGlobalClassLibrary.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using SmileSMotorClassLibrary;

namespace SmileClaimV1.Module.Claim.UserControl
{
    public partial class ucSearchConfirmClaimPrivilege : System.Web.UI.UserControl
    {
        #region Declare

        private string _title;

        /// <summary>
        /// Set Text Header
        /// </summary>
        public string Title
        {
            get
            {
                _title = lblTextHeader.Text.Trim();
                return _title;
            }

            set
            {
                lblTextHeader.Text = value;
            }
        }

        #endregion Declare

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnCancelPrivilege.Enabled = false;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DoClearAlert();
            dgvMain.PageIndex = 0; //Set defaultPageIndex = page 1 เสมอ
            DoSearchHCI(1, null, txtSearch.Text.Trim()); //1 = page 1
        }

        protected void btnCancelPrivilege_Click(object sender, EventArgs e)
        {
            DoClearAlert();
        }

        protected void btnSumitCancel_Click(object sender, EventArgs e)
        {
            DoClearAlert();
            SetHCIClaimCancelStatus();
            //DoSearch - load dgv
            DoSearchHCI(1, null, txtSearch.Text.Trim()); //1 = page 1
        }

        protected void dgvMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            DoClearAlert();
            btnCancelPrivilege.Enabled = true;
            hdfHospitalClaimInformID.Value = dgvMain.SelectedDataKey.Value.ToString();
        }

        protected void dgvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DoClearAlert();
            dgvMain.PageIndex = e.NewPageIndex;
            DoSearchHCI(e.NewPageIndex + 1, null, txtSearch.Text.Trim());
        }

        protected void dgvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                usp_HCI_select_Result dr = e.Row.DataItem as usp_HCI_select_Result;

                if (dr.HCIStatusID.ToString() == "4") // 4 คือ สถานะยกเลิก
                {
                    e.Row.Cells[9].ForeColor = Color.Red;
                    e.Row.Cells[1].Enabled = false;
                }
            }
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Set HCI Claim Cancel Status
        /// </summary>
        private void SetHCIClaimCancelStatus()
        {
            HCIServiceClient HCIClient = new HCIServiceClient();

            int Status = Convert.ToInt32(ddlRemarkCancel.SelectedItem.Value);

            var result = HCIClient.SetHCIClaimCancelStatus(int.Parse(hdfHospitalClaimInformID.Value),
                                                        1, //TODO:Create by ID
                                                        Status);

            if (result != null)
            {
                ucAlert.SetMessage(CommonUserControl.ucAlert.AlertStatus.success, "บันทึกยกเลิกการใช้สิทธิ์เรียบร้อยแล้ว");
            }
            else
            {
                ucAlert.SetMessage(CommonUserControl.ucAlert.AlertStatus.danger, "เกิดข้อผิดพลาด!");
            }

            //if (result == "Success")
            //{
            //    // ทำรายการสำเร็จ
            //}
        }

        /// <summary>
        /// Do Search HCI
        /// </summary>
        private void DoSearchHCI(int pageNamber, int? hciStatusID, string txtSearch)
        {
            int defaultRowsPageNumber = 20;

            HCIServiceClient HCIClient = new HCIServiceClient();
            List<usp_HCI_select_Result> lst = new List<usp_HCI_select_Result>();
            usp_HCI_HeaderInfo_Result headerInfo = new usp_HCI_HeaderInfo_Result();

            // Get Header Info
            headerInfo = HCIClient.GetHCI_HeaderIfo(defaultRowsPageNumber
                                   , 1 // 1 ทดสอบ id hospital
                                   , ucDateFrom.DateSelected
                                   , ucDateTo.DateSelected
                                   , null
                                   , null
                                   , hciStatusID
                                   , txtSearch);

            // Get HCI
            lst = HCIClient.GetHCI(defaultRowsPageNumber
                                   , pageNamber
                                   , 1 // 1 ทดสอบ id hospital
                                   , ucDateFrom.DateSelected
                                   , ucDateTo.DateSelected
                                   , null
                                   , null
                                   , hciStatusID
                                   , txtSearch).ToList();

            if (lst.Count > 0)
            {
                dgvMain.VirtualItemCount = headerInfo.TotalRecords.Value;
                dgvMain.PageSize = defaultRowsPageNumber;
                dgvMain.DataSource = lst;
                dgvMain.DataBind();
            }
            else
            {
                dgvMain.DataBind();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void DoLoadDrropdowList()
        {
            LoadDDLCancelRemark();
        }

        /// <summary>
        ///
        /// </summary>
        private void LoadDDLCancelRemark()
        {
            FunctionManager fm = new FunctionManager();
            HCIServiceClient HCIClient = new HCIServiceClient();

            var lst = HCIClient.GetHCICancelCause().ToList();
            fm.LoadDropdownlist(ddlRemarkCancel, lst, "CancelCauseDetail", "CancelCauseID");
        }

        private void DoClearAlert()
        {
            ucAlert.SetMessage(CommonUserControl.ucAlert.AlertStatus.clear, "");
        }

        #endregion Method
    }
}