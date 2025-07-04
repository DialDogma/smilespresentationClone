using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;
using System.Data;


namespace SmileMotorV1.Modules.Motor
{
    public partial class frmPersonSearchResult : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (Request.QueryString["txtsearch"] != string.Empty && Request.QueryString["PersonTypeID"] != string.Empty)
                {
                    string txtsearch = Request.QueryString["txtsearch"];
                    string personTypeID = Request.QueryString["PersonTypeID"];

                    LoadGridView(txtsearch, Convert.ToInt32(personTypeID));
                }

               
            }
         
        }

        private void LoadGridView(string personcardDetail,int persontype_id)
        {
            DataCenterDB db = new DataCenterDB();
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();

            dt =   db.GetPersonSearchByCardDetail(personcardDetail, null,persontype_id);


            fm.LoadGridview(dgvMain, dt, "Person_ID");
        }

        protected void dgvMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedID;
            int index = dgvMain.SelectedIndex;

            selectedID = dgvMain.DataKeys[index].Value.ToString();

            // Add script on page startup
            Page.ClientScript.RegisterStartupScript(GetType(), "MyKey", "ReturnValue(" + selectedID + ");", true);
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(dgvMain, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "คลิกเพื่อเลือกข้อมูล";
            }
        }
    }
}