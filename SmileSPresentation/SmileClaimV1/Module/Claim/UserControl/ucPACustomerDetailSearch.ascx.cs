using SmileClaimV1.ApplicationService_PA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileClaimV1.Module.Claim.UserControl
{
    public partial class ucPACustomerDetailSearch : System.Web.UI.UserControl
    {


        #region Declare
        private string applicationID;

        public string ApplicationID
        {
            get
            {
                return applicationID;
            }

            set
            {
                applicationID = value;
            }
        }

        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearchStudent_Click(object sender, EventArgs e)
        {
            dgvMain.PageIndex = 0; //set defaultPageIndex = 0 เสมอ
            DoLoadCustomer(hdfApplicationID.Value, 1, txtStudentName.Text.Trim()); // 1 = page 1 
          
        }

        protected void dgvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            dgvMain.PageIndex = e.NewPageIndex;
            DoLoadCustomer(hdfApplicationID.Value, e.NewPageIndex + 1, txtStudentName.Text.Trim());
        }

        #endregion

        #region Method

        /// <summary>
        /// 
        /// </summary>
        public void DoLoadCustomer(string appID, int pageNumber, string searchText)
        {

            int defaultItemPerPage = 20;

            // Set
            hdfApplicationID.Value = appID;

            // Create Constructor Client PA
            ApplicationServiceClient clientPA = new ApplicationServiceClient();
            List<usp_CustomerSearch_Select_Result> lst = new List<usp_CustomerSearch_Select_Result>();
            usp_ApplicationSearch_Select_Result app = new usp_ApplicationSearch_Select_Result();

            usp_CustomerSearch_HeaderInfo_Result headerInfo = clientPA.GetCustomerSearchHeaderInfo(20, hdfApplicationID.Value, searchText);
            dgvMain.VirtualItemCount = headerInfo.TotalRecords.Value;

            // Get Application Detail
            // 1 = หน้าแรกเท่านั้นเพื่อดึงค่าไปแสดงใน lblApplicationDetail และ lblProduct
            app = clientPA.ApplicationSearch(defaultItemPerPage, 1, hdfApplicationID.Value, null, null, null, null, null).FirstOrDefault();

            // Bind Detail
            lblApplicationDetail.Text = "Application ID:" + app.ApplicationID + " " + app.School;
            lblProduct.Text = app.Product;

            // Get Customer

            lst = clientPA.CustomerSearch(appID, defaultItemPerPage, pageNumber, searchText).ToList(); //ส่ง text

            dgvMain.PageSize = defaultItemPerPage;
            dgvMain.DataSource = lst;
            dgvMain.DataBind();
        }

        #endregion


    }
}