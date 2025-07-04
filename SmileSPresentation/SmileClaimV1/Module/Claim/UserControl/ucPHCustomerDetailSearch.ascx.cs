using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileClaimV1.ApplicationService_PH;
using SmileSGlobalClassLibrary.Class;
using System.Drawing;

namespace SmileClaimV1.Module.Claim.UserControl
{
    public partial class ucPHCustomerDetailSearch : System.Web.UI.UserControl
    {
        #region Declare




        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //load dgv page 1
            if (txtSearch.Text.Trim() != string.Empty)
            {
                dgvMain.PageIndex = 0; //Set defaultPageIndex = page 1 เสมอ
                DoLoadCustomer(1, txtSearch.Text.Trim());
                
            }
            else
            {
                cFunction.AlertJavaScript(Page, "ระบุคำค้นหา");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {          
            if (txtSearch.Text.Trim() != string.Empty)
            {
                dgvMain.PageIndex = e.NewPageIndex;
                DoLoadCustomer(e.NewPageIndex + 1, txtSearch.Text.Trim());
            }
            else
            {
                cFunction.AlertJavaScript(Page, "ระบุคำค้นหา");
            }
        }

        protected void dgvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string statusDetail = "ยกเลิก";

                string Status = e.Row.Cells[5].Text;
 
                // Hilight แดง
                if (statusDetail == Status)
                {
                    e.Row.Cells[5].ForeColor = Color.Red;
                }
            }
        }
        #endregion

        #region Method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="searchText"></param>
        public void DoLoadCustomer(int pageNumber, string searchText)
        {
            int defaultItemPerPage = 20;

            ApplicationServiceClient clientPH = new ApplicationServiceClient();
            List<usp_CustomerSearch_Select_Result> lst = new List<usp_CustomerSearch_Select_Result>();

            usp_CustomerSearch_HeaderInfo_Result headerInfo = clientPH.CustomerSearchHeaderInfo(20, searchText);

            lst = clientPH.CustomerSearch(searchText, defaultItemPerPage, pageNumber).ToList();

            dgvMain.VirtualItemCount = headerInfo.TotalRecords.Value;
            dgvMain.PageSize = defaultItemPerPage;
            dgvMain.DataSource = lst;
            dgvMain.DataBind();
        }


        #endregion

        
    }
}