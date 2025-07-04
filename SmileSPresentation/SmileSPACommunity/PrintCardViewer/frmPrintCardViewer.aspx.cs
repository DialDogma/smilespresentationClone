using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSPACommunity.Models;
using System.Data;
using SmileSPACommunity.Helper;
using System.Drawing.Printing;

namespace SmileSPACommunity.PrintCardViewer
{
    public partial class frmPrintCardViewer : System.Web.UI.Page
    {

        private PACommunityEntities _db;

        public frmPrintCardViewer()
        {
            _db = new PACommunityEntities();
        }

        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadReport();
            }
        }


        private void LoadReport()
        {
           

            if (Request.QueryString["temCode"] != "")   
            {
                var temcodeBase64EncodedBytes = Convert.FromBase64String(Request.QueryString["temCode"]);
                var temCode = System.Text.Encoding.UTF8.GetString(temcodeBase64EncodedBytes);

                var rs = _db.usp_TicketDetail_Select(temCode).ToList();

                if (rs.Count() > 0)
                {
                    var dt = new DataTable();

                    dt = GlobalFunction.ToDataTable(rs);

                   var c_ProductID =  rs.ToList().Where(x => x.ProductId != 177).Count();

                    bool is_Product632 = false;
                    if (c_ProductID > 0)
                    {
                        is_Product632 = true;
                    }

                    //Delete Tmp
                    var rs_delete = _db.usp_TmpTicketHeader_Delete(temCode).Single();

                    if (rs_delete.IsResult == true)
                    {
                        GenReportViewer(dt, is_Product632);
                    }
                }

            }
        }


        private void GenReportViewer(DataTable dt,bool isProduct632)
        {
            rpvPrintCard.LocalReport.DataSources.Clear();
            rpvPrintCard.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("DataSet", dt));


            //rpvPrintCard.LocalReport.ReportPath = Server.MapPath("~" + "/Views//Report//ReportRDLC//PrintTicketPAC_632Plan.rdlc");
            //var c = dt.Select("ProductId=" + 177).Count();

            if (isProduct632 == false)
            {
                rpvPrintCard.LocalReport.ReportPath = Server.MapPath("~" + "/Views//Report//ReportRDLC//PrintTicketPAC.rdlc");
            }
            else
            {
                rpvPrintCard.LocalReport.ReportPath = Server.MapPath("~" + "/Views//Report//ReportRDLC//PrintTicketPAC_632Product.rdlc");
            }

            rpvPrintCard.DataBind();
            rpvPrintCard.LocalReport.Refresh();
           
        }

    }
}