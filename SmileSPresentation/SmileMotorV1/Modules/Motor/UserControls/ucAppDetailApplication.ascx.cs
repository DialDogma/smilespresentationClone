using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;
using System.Drawing;
using System.Data;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucAppDetailApplication : System.Web.UI.UserControl
    {
        #region Declare

        private int _MotorApplication_ID;
        private int _MotorApplicationStatus_ID;

        /// <summary>
        /// Property Get/Set Motor Application
        /// </summary>
        public int MotorApplication_ID
        {
            get
            {
                return _MotorApplication_ID;
            }

            set
            {
                _MotorApplication_ID = value;
                hdfMotorApplicationID.Value = value.ToString();
            }
        }

        /// <summary>
        /// Property Get/Set Motor Application Status
        /// </summary>
        public int MotorApplicationStatus_ID
        {
            get
            {
                try
                {
                    _MotorApplicationStatus_ID = Convert.ToInt32(hdfMotorAppStatusID.Value);
                }
                catch
                {
                    _MotorApplicationStatus_ID = 0;
                }

                return _MotorApplicationStatus_ID;
            }
            set
            {
                _MotorApplicationStatus_ID = value;
                hdfMotorAppStatusID.Value = value.ToString();
            }
        }

        #endregion Declare

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            //btnshowmodal.CssClass = "btn btn-default btn-xs";
            // Set CSS Style
            btnClose.CssClass = "btn btn-danger btn-xs";
            //btnshowmodal.Width = Unit.Pixel(120);
            //btnClose.Width = Unit.Pixel(10);

            //ถ้าล็อกอินเป็น user
            if (HttpContext.Current.User.IsInRole("MotorUser"))
            {
                //ปิดปุ่มเพิ่ม
                btnAddPolicyNumbers.Visible = false;
            }
        }

        protected void btnshowmodal_Click(object sender, EventArgs e)
        {
            // Show Modal สิทธิประโยชน์
            ModalPopupBenefit.Show();
        }

        protected void btnshowmodalHistory_Click(object sender, EventArgs e)
        {
            // Show Modal ประวัติการทำรายการ
            ModalHistory.Show();
        }

        protected void btnAddPolicyNumbers_Click(object sender, EventArgs e)
        {
            //show เพิ่มเลขที่กรรมธรรม์
            ModalPolicyAddNumbers.Show();
        }

        protected void btnPolicyNumbers_Click(object sender, EventArgs e)
        {
            // Show Modal  เลขที่กรมธรรม์
            ModalPolicyNumbersDetail.Show();
        }

        protected void dgvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ////ส่ง query string
                string tran_id = dgvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();

                ////EncryptID
                FunctionManager fm = new FunctionManager();

                string EncryptID = fm.Base64Encrypt(tran_id);

                e.Row.Attributes.Add("onclick", "window.open('" + "frmLogDetail.aspx?" + "tranid=" + EncryptID + "')");
                e.Row.ToolTip = "คลิกเพื่อเปิดดูรายละเอียด";
            }
        }

        protected void dgvMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in dgvMain.Rows)
            {
                if (row.RowIndex == dgvMain.SelectedIndex)
                {
                    row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    row.ToolTip = "คลิกเพื่อเปิดดูรายละเอียด";
                }
            }
        }

        //Save
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.ucPolicyNumbersAdd.Isvalidate())
            {
                //บันทึก
                this.ucPolicyNumbersAdd.DoSave();
                this.ucPolicyNumbersAdd.Doclear();
                //ซ่อน modal
                ModalPolicyAddNumbers.Hide();
                //โหลด modal detail
                this.ucPolicyNumbersDetail.Doload(Convert.ToInt32(hdfMotorApplicationID.Value));
            }
            else
            {
                ModalPolicyAddNumbers.Show();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.ucPolicyNumbersAdd.Doclear();
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Do Load Detail
        /// </summary>
        /// <param name="application_id">Motor Application ID</param>
        public void DoLoad(int application_id)
        {
            FunctionManager fm = new FunctionManager();
            MotorApplication ma = new MotorApplication();

            // Get Motor Application Detail
            ma = GetMotorApplicationByMotorApplicationID(application_id);

            //Set MotorApplication_ID
            hdfMotorApplicationID.Value = application_id.ToString();

            // Set Bind Detail

            //lblID.Text = application_id.ToString();
            lblMotorApplicationCode.ToolTip = "ID : " + application_id.ToString();
            lblMotorApplicationCode.Text = ma.MotorApplication_Code;
            lblStatus.Text = ma.MotorApplicationStatusDetail;
            lblContract.Text = ma.MotorApplicationContractTypeDetail;
            lblClaimType.Text = ma.MotorApplicationClaimTypeDetail;
            lblStartCoverDate.Text = ma.StartCover.ToShortDateString();
            lblEndCoverDate.Text = ma.EndCover.ToShortDateString();
            lblInsuranceCompany.Text = ma.InsuranceCompanyDetail;

            lblProduct.Text = ma.Product.ProductTypeDetail + "-" + ma.Product.ProductDetail;
            hdfMotorAppStatusID.Value = ma.MotorApplicationStatus_ID.ToString();

            SetHilightStatus(ma.MotorApplicationStatus_ID);

            //load สิทธิประโยชน์
            this.ucBenefit.DoLoad(application_id);

            //โหลด dgv ประวัติการทำรายการ
            LoadGridView(application_id);

            //โหลด รายละเอียดเลขกรมธรรม์
            this.ucPolicyNumbersDetail.Doload(application_id);

            //set MotorApplication_ID
            this.ucPolicyNumbersAdd.MotorApplication_ID = application_id;

            lblHeir.Text = ma.MotorApplicationHeirDetail;
        }

        /// <summary>
        /// Get Motor Application Return Class
        /// </summary>
        /// <param name="application_id">Motor Application ID</param>
        /// <returns></returns>
        public MotorApplication GetMotorApplicationByMotorApplicationID(int application_id)
        {
            MotorDB db = new MotorDB();
            return db.GetMotorApplicationByMotorApplicationID(application_id);
        }

        /// <summary>
        /// Set Hilight Label Status
        /// </summary>
        /// <param name="mortorappStatus_ID">Motor Application ID</param>
        private void SetHilightStatus(int mortorappStatus_ID)
        {
            // Select Motor Application Status
            switch (mortorappStatus_ID)
            {
                // Approve
                case (int)MappingCode.eMotorApplicationStatus.Approve:
                    lblStatus.CssClass = "label label-success";
                    break;

                // NewApp
                case (int)MappingCode.eMotorApplicationStatus.NewApp:
                    lblStatus.CssClass = "label label-info";
                    break;

                // UnApprove
                case (int)MappingCode.eMotorApplicationStatus.UnApprove:
                    lblStatus.CssClass = "label label-danger";
                    break;

                // WaitEdit
                case (int)MappingCode.eMotorApplicationStatus.WaitEdit:
                    lblStatus.CssClass = "label label-warning";
                    break;

                // Cancel
                case (int)MappingCode.eMotorApplicationStatus.Cancel:
                    lblStatus.CssClass = "label label-danger";
                    break;

                // NotAvailable
                case (int)MappingCode.eMotorApplicationStatus.NotAvailable:
                    lblStatus.CssClass = "label label-default";
                    break;

                // WaitApprove
                case (int)MappingCode.eMotorApplicationStatus.WaitApprove:
                    lblStatus.CssClass = "label label-primary";
                    break;

                // EditWaitApprove
                case (int)MappingCode.eMotorApplicationStatus.EditWaitApprove:
                    lblStatus.CssClass = "label label-primary";
                    break;

                // WaitCancel
                case (int)MappingCode.eMotorApplicationStatus.WaitCancel:
                    lblStatus.CssClass = "label label-warning";
                    break;
                // Reverse = รอคืนสภาพ
                case 10:
                    lblStatus.CssClass = "label label-warning";
                    break;
                //สิ้นสุดความคุ้มครอง
                case 11:
                    lblStatus.CssClass = "label label-danger";
                    break;
                //สิ้นสุดความคุ้มครอง
                case 12:
                    lblStatus.CssClass = "label label-warning";
                    break;
            }
        }

        /// <summary>
        /// Get Motor Application Status ID
        /// </summary>
        /// <returns></returns>
        public int GetMotorApplicationStatusID()
        {
            return MotorApplicationStatus_ID;
        }

        /// <summary>
        /// GetMotorApplicationStartCoverDate
        /// </summary>
        /// <returns></returns>
        public DateTime GetMotorApplicationStartCoverDate()
        {
            return Convert.ToDateTime(lblStartCoverDate.Text);
        }

        /// <summary>
        ///GetMotorApplicationEndCoverDate
        /// </summary>
        /// <returns></returns>
        public DateTime GetMotorApplicationEndCoverDate()
        {
            return Convert.ToDateTime(lblEndCoverDate.Text);
        }

        public void LoadGridView(int? appID)
        {
            MotorDB mdb = new MotorDB();
            DataTable dt = new DataTable();
            FunctionManager fm = new FunctionManager();

            dt = mdb.GetMotorApplicationTransaction(appID);

            fm.LoadGridview(dgvMain, dt, "MotorApplicationTransaction_ID");
        }

        #endregion Method

        protected void dgvMain_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvMain.PageIndex = e.NewPageIndex;

            var motor_id = Convert.ToInt32(hdfMotorApplicationID.Value);
            LoadGridView(motor_id);
        }
    }
}