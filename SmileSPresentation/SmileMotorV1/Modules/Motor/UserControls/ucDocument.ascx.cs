using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileMotorV1.Models;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucDocument : System.Web.UI.UserControl
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            Button btnScan;
            Button btnOpenDoc;
            string docID;
            string typeID;
            FunctionManager fm = new FunctionManager();
            string encode;
            DocumentManager dm = new DocumentManager();

            using (var db = new SSSCashReceiveEntities())
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // Get Doc ID From Row
                    docID = e.Row.Cells[1].Text;

                    //get doc type
                    typeID = e.Row.Cells[3].Text;

                    // Encoding Doc ID
                    encode = fm.Base64Encrypt(docID);

                    // Find Button In Row
                    btnScan = e.Row.FindControl("btnScan") as Button;
                    btnOpenDoc = e.Row.FindControl("btnOpenDoc") as Button;

                    if (typeID == "5")
                    {
                        var result = db.usp_Validate_NewApp_ScanDocument_Select(hdfAppID.Value).First();
                        if (result == false)
                        {
                            //btnScan.Enabled = false;
                            btnScan.Text = "ยังไม่ชำระเงิน";
                            btnScan.Attributes.Add("disabled", "true");
                        }
                    }
                    // Set Text Button And Count Doc
                    btnOpenDoc.Text = "จำนวนเอกสาร :" + dm.GetDocCount(docID);

                    // Set Att Button
                    var scanPDF = Properties.Settings.Default["ScanPDF"].ToString();
                    var openPDF = Properties.Settings.Default["OpenPDF"].ToString();

                    btnScan.Attributes.Add("onclick", "window.open('" + scanPDF + "dcid=" + encode + "&prjid=SSSMotor');");
                    btnOpenDoc.Attributes.Add("onclick", "window.open('" + openPDF + "dcid=" + encode + "&prjid=SSSMotor');");
                }
            }
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Do Load Detail
        /// </summary>
        /// <param name="motorapplicationID">Motor Application ID</param>
        public void DoLoad(int motorapplicationID)
        {
            MotorDB db = new MotorDB();
            DataTable dt = new DataTable();
            FunctionManager fm = new FunctionManager();

            // Get Document Detail
            dt = db.GetDocumentDetail(motorapplicationID);

            // Load Gridview
            fm.LoadGridview(dgvMain, dt, "CustomerDocument_ID");
            dgvMain.Columns[3].Visible = false;
        }

        /// <summary>
        /// Do Delete Document
        /// </summary>
        /// <param name="customerdocumentID">Cust Doc ID</param>
        public void DoDelete(int customerdocumentID)
        {
            MotorDB db = new MotorDB();
            FunctionManager fm = new FunctionManager();

            // Delete Document
            db.DeleteDocument(customerdocumentID, cFunction.GetCurrentLoginUser_ID(this.Page));
        }

        /// <summary>
        /// Do Add Document
        /// </summary>
        /// <param name="motorapplicationID">Motor Application ID</param>
        /// <param name="documentType">Doc Type ID</param>
        /// <returns></returns>
        public DataTable DoAddDocument(int motorapplicationID, int documentType)
        {
            MotorDB db = new MotorDB();
            FunctionManager fm = new FunctionManager();
            DataTable dt = new DataTable();

            //db.AddDocument();
            dt = fm.ToDataTable(db.AddDocument(motorapplicationID, documentType, cFunction.GetCurrentLoginUser_ID(this.Page)));

            return dt;
        }

        /// <summary>
        /// Set Enable Scan Button
        /// </summary>
        /// <param name="value">True/False</param>
        public void IsEnabled(bool value)
        {
            // Set Enable In Row
            foreach (GridViewRow gvr in dgvMain.Rows)
            {
                Button btnScan = gvr.FindControl("btnScan") as Button;
                btnScan.Enabled = value;
            }
        }

        #endregion Method

        public void SetAppID(string appId)
        {
            hdfAppID.Value = appId;
        }
    }
}