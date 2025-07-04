using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSConvertFile.Model;

namespace MasterTest
{
    public partial class ManageCost : System.Web.UI.Page
    {
          CostCenterDBContext db = new CostCenterDBContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DoloadGridview();

            }
        }

        public void Button_Insert(Object sender, EventArgs e)
        {
            if (txtCode1.Text == "" || txtDetail.Text == "")
            {
                Response.Write("<script>alert('Can not insert.')</script>");

            }
            else
            {
                var obj = new CostCenterAccount()
                {
                    CostCenterAccountCode = txtCode1.Text,
                    CostCenterAccountDetail = txtDetail.Text,
                    DeptType = txtDept1.Text,
                    IsActive = "true"
                };

                db.CostCenterAccounts.Add(obj);

                db.SaveChanges();

                Response.Redirect("ManageCost.aspx");
            }

        }

        protected void dataGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string data = dataGridView.DataKeys[e.Row.RowIndex].Values[0].ToString();

            }
        }

        protected void dataGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal2", "$('#myModal2').modal();", true);

            string codeSelect = dataGridView.SelectedDataKey[0].ToString();
            string detailSelect = dataGridView.SelectedDataKey[1].ToString();
            string deptSelect = dataGridView.SelectedDataKey[2].ToString();

            txtCodeEdit1.Text = codeSelect;
            txtDetailEdit1.Text = detailSelect;
            //txtDetailEdit.Value = detailSelect;
            txtDeptTypeEdit1.Text = deptSelect;

            Session["Code"] = codeSelect;
            Session["Detail"] = detailSelect;
            Session["DeptType"] = deptSelect;
        }

        public void Delete_Data()
        {
            string strSelect = dataGridView.SelectedDataKey.Value.ToString();

            if (strSelect != "")
            {
                var update = (from c in db.CostCenterAccounts
                              where c.CostCenterAccountCode == strSelect
                              select c).FirstOrDefault();

                if (update != null)
                {
                    update.IsActive = "false";
                }

                db.SaveChanges();

                Response.Redirect("ManageCost.aspx");
            }

            

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Delete_Data();
            DoloadGridview();
            //Response.Redirect("ManageCost.aspx");

        }

        protected void dataGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dataGridView.PageIndex = e.NewPageIndex;

            DoloadGridview();
        }

        private void DoloadGridview()
        {
            var ds = (from c in db.CostCenterAccounts
                      where c.IsActive == "true"
                      select new
                      {
                          c.CostCenterAccountCode,
                          c.CostCenterAccountDetail,
                          c.DeptType,
                      }).ToList();

            if (ds.Count() > 0)
            {

                dataGridView.DataSource = ds;
                dataGridView.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            string strSearch = "";

            strSearch = txtSearch.Value;

            var ds = (from c in db.CostCenterAccounts
                      where c.CostCenterAccountDetail.Contains(strSearch) && c.IsActive == "true"
                      select new
                      {
                          c.CostCenterAccountCode,
                          c.CostCenterAccountDetail,
                          c.DeptType,
                      }).ToList();

            if (ds.Count() > 0)
            {
                dataGridView.DataSource = ds;
                dataGridView.DataBind();
            }
            else
            {
                Response.Write("<script>alert('Not found.')</script>");
                DoloadGridview();
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            string strCode = (string)Session["Code"];
            string strDetail = (string)Session["Detail"];
            string strDept = (string)Session["DeptType"];
            string strCodeEdit = "";
            string strDetailEdit = "";
            string strDeptEdit = "";

            strCodeEdit = txtCodeEdit1.Text;
            strDetailEdit = txtDetailEdit1.Text;
            //strDetailEdit = txtDetailEdit.Value;
            strDeptEdit = txtDeptTypeEdit1.Text;

            if (strCodeEdit != "" && strDetailEdit != "")
            {
                var update = (from c in db.CostCenterAccounts
                              where c.CostCenterAccountCode == strCode && c.CostCenterAccountDetail == strDetail && c.DeptType == strDept
                              select c).FirstOrDefault();

                if (update != null)
                {
                    update.CostCenterAccountCode = strCodeEdit;
                    update.CostCenterAccountDetail = strDetailEdit;
                    update.DeptType = strDeptEdit;
                }

                db.SaveChanges();

                DoloadGridview();
                Response.Redirect("ManageCost.aspx");
            }
        }

        protected void insertbtn_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
        }
    }
}