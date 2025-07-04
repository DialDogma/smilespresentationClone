using SmileClaimV1.ClaimSeviceDataCenter;
using SmileClaimV1.HCIService;
using SmileSGlobalClassLibrary.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileClaimV1.Module.Claim.UserControl
{
    public partial class ucHospitalMonitorSearch : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridview();
        }


        #region Method

        /// <summary>
        /// 
        /// </summary>
        public void DoLoadDropdownList()
        {
            LoadDDLClaimAdmitType();
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadGridview()
        {
            HCIServiceClient HCIClient = new HCIServiceClient();
            DataTable dt = new DataTable();

            // Get HCI Monitor Pivot
            


            dgvMain.DataSource = null;
            dgvMain.DataBind();
        }

        private void LoadDDLClaimAdmitType()
        {
            FunctionManager fm = new FunctionManager();
            ClaimServiceClient dataCenterClient = new ClaimServiceClient();

            var lst = dataCenterClient.GetClaimAdmidType(null, null, null).ToList();

            fm.LoadDropdownlist<usp_ClaimAdmitType_Select_Result>(ddlCureType, lst, "ClaimAdmitTypeDetail", "ClaimAdmitTypeID");
        }



        #endregion

        
    }
}