using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmSearchApplication : System.Web.UI.Page
    {
        #region Declare
        private int _InsuranceCompany_ID;

        public int InsuranceCompany_ID
        {
            get
            {
                return _InsuranceCompany_ID;
            }

            set
            {
                _InsuranceCompany_ID = value;
            }
        }
        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {               
                ddlMotorCompanyInsurance.DoLoadDropdownListAll();
            }
        }

        protected void dgvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvMain.PageIndex = e.NewPageIndex;

            LoadGridview();
        }

        protected void dgvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string id = dgvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();
                
                string EncryptID = cFunction.Base64Encrypt(id);

                e.Row.Attributes.Add("onclick", "window.open('" + "frmAppDetail.aspx?" + "code=" + EncryptID + "')");
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

        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridview();
        }
        #endregion

        #region Method
        public void LoadGridview()
        {
            MotorDB db = new MotorDB();
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();

            dt = db.GetMotorApplicationByInsuranceCompanyID(ddlMotorCompanyInsurance.InsuranceCompany_ID, txtSearch.Text.Trim());

            fm.LoadGridview(dgvMain, dt, "ApplicationID");

        }
        #endregion

    }
}