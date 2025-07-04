using SmileClaimV1.ApplicationService_PA;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSGlobalClassLibrary.Class;

namespace SmileClaimV1.Module.Claim.UserControl
{
    public partial class ucPASchoolDetailSearch : System.Web.UI.UserControl
    {

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSchoolNameSearch.Text.Trim() != string.Empty)
            {
                dgvSchool.PageIndex = 0; //set defaultPageIndex = 0 เสมอ
                DoSearchSchool(1); // 1 คือ pageNuber
               
            }
            else
            {
                cFunction.AlertJavaScript(Page, "กรุณาระบุคำค้น!");
            }
        }


        protected void dgvSchool_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvSchool.PageIndex = e.NewPageIndex;
            DoSearchSchool(e.NewPageIndex + 1);
        }


        protected void dgvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string statusDetail = "ยกเลิกกรมธรรม์";

                string Status = e.Row.Cells[6].Text;

                // Hilight แดง
                if (statusDetail == Status)
                {
                    e.Row.Cells[6].ForeColor = Color.Red;
                }
            }
        }
        #endregion


        #region Method
        /// <summary>
        /// 
        /// </summary>
        public void DoSearchSchool(int pageNumber)
        {
            int year = int.Parse(ddlSchoolYear.SelectedItem.Value);
            int defaultItemPerPage = 20;

            // Create Constructor Client PA
            ApplicationServiceClient clientPA = new ApplicationServiceClient();
            List<usp_ApplicationSearch_Select_Result> lst = new List<usp_ApplicationSearch_Select_Result>();
            usp_ApplicationSearch_HeaderInfo_Result headerInfo = clientPA.ApplicationSearchHeaderInfo(defaultItemPerPage, txtSchoolNameSearch.Text.Trim(), year, null, null, null, null);

            dgvSchool.VirtualItemCount = headerInfo.TotalRecords.Value;
          
            // Get School
            lst = clientPA.ApplicationSearch(defaultItemPerPage, pageNumber,txtSchoolNameSearch.Text.Trim()
                , year
                , null, null, null, null).ToList();

            dgvSchool.PageSize = defaultItemPerPage;
            dgvSchool.DataSource = lst;
            dgvSchool.DataBind();
        }
        #endregion


    }
}