using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmLogDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if query string = tranid ,get tranid ข้อมูลกรมธรรม์
                if (Request.QueryString["tranid"] != null && Request.QueryString["tranid"] != string.Empty)
                {
                    //decrypt tranid to string
                    var qsID = cFunction.Base64Decrypt(Request.QueryString["tranid"]);
                    //convert tranid to int 
                    int qsTID = Convert.ToInt32(qsID);
                    //Load tranid to gridview โหลดข้อมูลการแก้ไขกรมธรรม์
                    LoadtranDGV(qsTID);

                }
                //if query string = ptranid ,get ptranid ข้อมูลลูกค้า หรือผู้ชำระเบี้ย
                if (Request.QueryString["ptranid"] != null && Request.QueryString["ptranid"] != string.Empty)
                {
                    //decrypt ptranid to string
                    var qsID = cFunction.Base64Decrypt(Request.QueryString["ptranid"]);
                    //convert ptranid to int 
                    var qsPTID = Convert.ToInt32(qsID);
                    //load ptranid to gridview โหลดlogการแก้ไขข้อมูลลูกค้า หรือ ผู้ชำระเบี้ย
                    LoadPtranDGV(qsPTID);
                }
            }
        }

        /// <summary>
        /// โหลดข้อมูล log การแก้ไขข้อมูลกรมธรรม์
        /// </summary>
        /// <param name="ID"></param> get tranid
        private void LoadtranDGV(int ID)
        {
            //connect to motor database
            MotorDB mdb = new MotorDB();
            //new datatable
            DataTable dt = new DataTable();
            DataTable logDetailDT = new DataTable();
            //get motor application transaction detail from motor database to datatable "dt"
            dt = mdb.GetMotorApplicationTransactionDetail(ID);
            //check if dt.rows not null
            if (dt.Rows.Count >0)
            {
                //add column "Detail" and "Value" to datatable "logDetailDT"
                logDetailDT.Columns.Add("Detail");
                logDetailDT.Columns.Add("Value");
                //loop in datatable "dt"
                foreach (DataColumn c in dt.Columns)
                {
                    //get column and row data on datatable "dt" to string "value"
                    string value = dt.Rows[0][c.ColumnName].ToString();
                    //add column name and value to row on datatable "logDtailDT"
                    logDetailDT.Rows.Add(c.ColumnName, value);
                }
                //bind data to gridview
                dgvLogDetail.DataSource = logDetailDT;
                dgvLogDetail.DataBind();
            }
            
        }

        /// <summary>
        /// โหลดข้อมูล log การแก้ไขข้อมูลลูกค้า และผู้ชำระเบี้ย
        /// </summary>
        /// <param name="ID"></param>
        private void LoadPtranDGV(int ID)
        {
            //connect to datacenter database
            DataCenterDB ddb = new DataCenterDB();
            //new datatable
            DataTable dt = new DataTable();
            DataTable logDetailDT = new DataTable();
            //get person transaction detail from datacenter database to datatable "dt"
            dt = ddb.GetPersonTransactionDetail(ID);
            //if dt.rows not null
            if (dt.Rows.Count > 0)
            {
                //add column "Detail" and "Value" to datatable "logDetailDT"
                logDetailDT.Columns.Add("Detail");
                logDetailDT.Columns.Add("Value");
                //loop in datatable "dt"
                foreach (DataColumn c in dt.Columns)
                {
                    //get column and row data on datatable "dt" to string "value"
                    string value = dt.Rows[0][c.ColumnName].ToString();
                    //add column name and value to row on datatable "logDtailDT"
                    logDetailDT.Rows.Add(c.ColumnName, value);
                }
                //bind data to gridview
                dgvLogDetail.DataSource = logDetailDT;
                dgvLogDetail.DataBind();
            }
        }
    }
}