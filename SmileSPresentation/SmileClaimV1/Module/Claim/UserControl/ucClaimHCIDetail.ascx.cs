using SmileClaimV1.ClaimSeviceDataCenter;
using SmileClaimV1.HCIService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSGlobalClassLibrary.Class;

namespace SmileClaimV1.Module.Claim.UserControl
{
    public partial class ucClaimHCIDetail : System.Web.UI.UserControl
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            LoadGridview(1, txtSearch.Text.Trim()); //1 = page 1

        }

        protected void dgvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Find Hyperlink
                HyperLink lnkNavigate = (HyperLink)e.Row.FindControl("lnkNav");

                // Get Column Value
                usp_HCI_select_Result dr = e.Row.DataItem as usp_HCI_select_Result;
                int productGroupID = dr.ProductGroupID.Value; //ProductGroupID
                string claimNo = DataBinder.Eval(e.Row.DataItem, "ClaimHeaderID").ToString();

                string EncryptClaimNo = cFunction.Base64Encrypt(claimNo);
                // Check Column Value
                if (productGroupID == 3) // ถ้า PA
                {
                    lnkNavigate.Text = claimNo;
                    lnkNavigate.NavigateUrl = "http://49.231.178.253:81/SSSPA/Modules/Claim/frmClaimPA_New.aspx?clm=" + EncryptClaimNo;
                    lnkNavigate.Target = "_blank";
                }

                // Check Column Value
                if (productGroupID == 2) // ถ้า PH
                {
                    lnkNavigate.Text = claimNo;
                    lnkNavigate.NavigateUrl = "http://49.231.178.253/SSS/Modules/Claim/frmClaimHeader.aspx?clm=" + EncryptClaimNo;
                    lnkNavigate.Target = "_blank";
                }
            }
        }

        protected void dgvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvMain.PageIndex = e.NewPageIndex;
            LoadGridview(e.NewPageIndex + 1, txtSearch.Text.Trim());
        }

        #endregion


        #region Method

        /// <summary>
        /// 
        /// </summary>
        public void DoLoadDropdownList()
        {
            LoadDDLClaimType();
        }

        /// <summary>
        /// Load Gridview Detail
        /// </summary>
        private void LoadGridview(int pageNumber, string txtSearch)
        {
            int defaultRowsPageNumber = 20;
            int? hciStatusID = null;
            int cureTypeID = int.Parse(ddlCureType.SelectedItem.Value);

            if (chkNotClaim.Checked)
            {
                hciStatusID = 2; //สถานะเช็คสิทธิ์เคลม
            }

            HCIServiceClient HCIClient = new HCIServiceClient();
            List<usp_HCI_select_Result> lst = new List<usp_HCI_select_Result>();
            usp_HCI_HeaderInfo_Result headerInfo = new usp_HCI_HeaderInfo_Result();



            //Get HeaderInfo
            headerInfo = HCIClient.GetHCI_HeaderIfo(defaultRowsPageNumber
                                                     , 1 // 1 ทดสอบ id hospital
                                                     , ucDateFrom.DateSelected
                                                     , ucDateTo.DateSelected
                                                     , null
                                                     , cureTypeID
                                                     , hciStatusID
                                                     , txtSearch);
            //Get HCI 
            lst = HCIClient.GetHCI(defaultRowsPageNumber
                                    , pageNumber
                                    , 1  // 1 ทดสอบ id hospital
                                    , ucDateFrom.DateSelected
                                    , ucDateTo.DateSelected
                                    , null
                                    , cureTypeID
                                    , hciStatusID
                                    , txtSearch).ToList();

            dgvMain.VirtualItemCount = headerInfo.TotalRecords.Value;
            dgvMain.PageSize = defaultRowsPageNumber;
            dgvMain.DataSource = lst;
            //dgvMain.DataSource = DataTest(); //อันนี้ทดสอบนะจ๊ะ
            dgvMain.DataBind();
        }

        /// <summary>
        /// Load DDL Claim Type
        /// </summary>
        private void LoadDDLClaimType()
        {

            ClaimServiceClient client = new ClaimServiceClient();

            ddlCureType.DataSource = client.GetClaimAdmidType(null, null, true);
            ddlCureType.DataTextField = "ClaimAdmitTypeDetail";
            ddlCureType.DataValueField = "ClaimAdmitTypeID";
            ddlCureType.DataBind();

        }

        #endregion


        //เอาไว้เทสเฉยๆนะ
        private DataTable DataTest()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("ประเภท");
            dt.Columns.Add("เลขที่การแจ้ง");
            dt.Columns.Add("เลขที่เคลม");
            dt.Columns.Add("ชื่อสกุลผู้เอาประกัน");
            dt.Columns.Add("วันที่แจ้งใช้สิทธิ์");
            dt.Columns.Add("สถานะ");
            dt.Columns.Add("โรงพยาบาล");
            dt.Columns.Add("จังหวัด");

            dt.Rows.Add("PH", "HCI171100000028", "CL5710000057", "นายพงษ์พันธ์ ท้าวยศ", "25-10-2560", "แจ้งใช้สิทธิ์สำเร็จ", "โรงพยาบาลสายไหม", "กรุงเทพ");
            dt.Rows.Add("PA", "HCI171100000029", "CLPA580600000017", "เด็กชายพงษ์พันธ์ ท้าวยศ", "25-09-2560", "แจ้งใช้สิทธิ์สำเร็จ", "โรงพยาบาลสายไหม", "กรุงเทพ");

            return dt;
        }
    }
}