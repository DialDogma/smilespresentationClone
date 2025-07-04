using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileMotorV1.Models;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucAppDetailAppReNew : System.Web.UI.UserControl
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
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

        protected void dgvMain_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //GET APP ID FROM DATA KEY
            var Id = dgvMain.DataKeys[dgvMain.SelectedIndex].Values[0].ToString();

            //ENCRYPT TO BASE64
            var IdEncrypt = Convert.ToBase64String(new System.Text.ASCIIEncoding().GetBytes(Id));

            //OPEN NEW WINDOW
            Page.ClientScript.RegisterStartupScript(GetType(), "OpenWindow", "window.open('frmAppDetail?code=" + IdEncrypt + "','_blank');", true);
        }

        #endregion Event

        #region Method

        private void LoadGridView(int motorapplicationID)
        {
            using (var db = new MotorV1Entities())
            {
                var lst = db.usp_MotorApplication_RenewByAppID_Select(motorapplicationID).ToList();

                mFunction.LoadGridviewByList(dgvMain, lst, "ApplicationID");
            }
        }

        public void Doload(int motorApplicationId)
        {
            LoadGridView(motorApplicationId);
        }

        #endregion Method
    }
}