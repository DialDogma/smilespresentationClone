using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;
using SmileMotorV1.Models;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmEndorseMotor : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["etype"] != "" && Request.QueryString["etype"] != null)
                {
                    var eType = Request.QueryString["etype"];
                    LoadDropdownList();
                    //ddlEndorseType.Items.FindByValue(eType).Selected = true;
                    ddlEndorseType.SelectedValue = eType;
                }
                else
                {
                    Response.Redirect("frmSuccess.aspx?msg=2");
                }
            }
        }

        protected void dgvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvMain.PageIndex = e.NewPageIndex;

            LoadGridview();
        }

        protected void dgvMain_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var valueEndorseType = ddlEndorseType.SelectedValue;

            var id = dgvMain.DataKeys[dgvMain.SelectedIndex].Values[0].ToString();

            var idEncode = Convert.ToBase64String(new System.Text.ASCIIEncoding().GetBytes(id));

            switch (valueEndorseType)
            {
                case "2":
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('frmEndorseChangePayer.aspx?mtid=" + idEncode + "','_blank');", true);
                    //Response.Redirect("frmEndorseChangePayer.aspx?mtid=MzIyOQ==");
                    break;

                case "3":
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('frmEndorseChangeSource.aspx?mtid=" + idEncode + "','_blank');", true);
                    //Server.Transfer("frmAppDetail.aspx?code=MzIyOQ==");
                    break;

                case "4":
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('frmEndorseChangePeriodType.aspx?mtid=" + idEncode + "','_blank');", true);
                    //Response.Redirect("frmAppDetail.aspx?code=MzIyOQ==");
                    break;
            }
        }

        protected void dgvMain_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //attributes onclick Select on row
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(dgvMain, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "คลิกเพื่อเลือกข้อมูล";
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridview();
        }

        #endregion Event

        #region Method

        public void LoadGridview()
        {
            using (MotorV1Entities motorV1Entities = new MotorV1Entities())
            {
                FunctionManager fm = new FunctionManager();

                DataTable dt = new DataTable();

                var result = motorV1Entities.usp_MotorApplication_Search_SelectV2(2, txtSearch.Text.Trim()).ToList();

                dt = fm.ToDataTable(result);

                mFunction.LoadGridview(dgvMain, dt, "ApplicationID");
            }
        }

        public void LoadDropdownList()
        {
            using (MotorV1Entities motorV1Entities = new MotorV1Entities())
            {
                var dt = motorV1Entities.usp_MotorApplication_Endorse_Select(null);

                ddlEndorseType.DataSource = dt;
                ddlEndorseType.DataTextField = "MotorApplicationEndorseDetail";
                ddlEndorseType.DataValueField = "MotorApplicationEndorse_ID";

                ddlEndorseType.DataBind();
            }
        }

        #endregion Method
    }
}