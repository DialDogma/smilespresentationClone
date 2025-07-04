using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using ClosedXML.Excel;
using System.Drawing;
using SmileSConvertFile.Model;

namespace MasterTest
{
    public partial class ConvertExcel2 : System.Web.UI.Page
    {
        int countDownload;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //function : import file excel and check file
        public void ImportExcel()
        {
            string filePath = Server.MapPath("~/Files/") + Path.GetFileName(UploadFile.PostedFile.FileName);
            UploadFile.SaveAs(filePath);

            try
            {
                using (XLWorkbook workBook = new XLWorkbook(filePath))
                {
                    IXLWorksheet workSheet = workBook.Worksheet(1);

                    Session["worksheet"] = workSheet;

                    IXLRange range = workSheet.RangeUsed();

                    if (IsValidateImport() == true)
                    {
                        txtRow2.Text = Convert.ToString(range.RowCount());
                        txtColumn2.Text = Convert.ToString(range.ColumnCount());

                        txtStatus2.Text = "OK";
                        txtStatus2.ForeColor = Color.Green;

                        string filename = UploadFile.PostedFile.FileName;
                        Session["FileName"] = filename;
                    }
                    else
                    {
                        txtColumn2.Text = "";
                        txtRow2.Text = "";
                    }
                }
            }
            catch (ArgumentException)
            {
                txtStatus2.Text = "This file format is not supported.";
                //txtStatus2.Text = "Fail";
                txtStatus2.ForeColor = Color.Red;

                txtColumn2.Text = "";
                txtRow2.Text = "";
                //txtStatus2.Text = "";
            }
        }

        //function : read excel to datatable
        public void ExcelToDataTable()
        {
            IXLWorksheet workSheet = (IXLWorksheet)Session["worksheet"];
            IXLRange range = workSheet.RangeUsed();

            DataTable dt = new DataTable();

            int rowCount = 0;
            int columnHead = 0;
            int j = 0;

            //add excel data to datatable
            foreach (IXLRow wsRow in workSheet.Rows())
            {
                columnHead = 0;

                if (rowCount == 2)
                {
                    dt.Columns.AddRange(new DataColumn[] { new DataColumn("A1"), new DataColumn("A2"), new DataColumn("A4") ,new DataColumn("A5")
                                            ,new DataColumn("A6") ,new DataColumn("A10") ,new DataColumn("A13")});

                    foreach (IXLCell cell in wsRow.Cells())
                    {
                        if (columnHead > 3 && columnHead != 13)
                        {
                            dt.Columns.Add(cell.Value.ToString());
                        }

                        columnHead++;
                    }

                    //foreach (IXLCell cell in wsRow.Cells())
                    //{
                    //    dt.Columns.Add(cell.Value.ToString());
                    //}
                }
                else if (rowCount == 3)
                {
                    foreach (IXLCell cell in wsRow.Cells())
                    {
                        if (columnHead < 4)
                        {
                            dt.Columns.Add(cell.Value.ToString());
                        }
                        else if (columnHead == 13)
                        {
                            dt.Columns.Add(cell.Value.ToString());
                        }

                        columnHead++;
                    }

                    //change postion head column for read data to datatable
                    dt.Columns["เดือน"].SetOrdinal(7);
                    dt.Columns["วันที่"].SetOrdinal(8);
                    dt.Columns["รายละเอียด"].SetOrdinal(9);
                    dt.Columns["เลขที่ บส."].SetOrdinal(10);
                    dt.Columns["จ่ายเงินให้"].SetOrdinal(11);
                }
                else if (rowCount > 3)
                {
                    //Add rows to DataTable.
                    dt.Rows.Add();

                    int i = 1;
                    DataRow dr = dt.Rows[j];

                    int ii = range.RowCount();

                    //add all row
                    if ((range.RowCount() - dt.Rows.Count) > 4)
                    {
                        dr["A1"] = "0000";
                        dr["A2"] = "ฝ่ายกองทุนสินไหม";
                    }

                    foreach (IXLCell cell in wsRow.Cells())
                    {
                        //dt.Rows[dt.Rows.Count - 1][i + 6] = cell.Value.ToString();
                        dt.Rows[dt.Rows.Count - 1][i + 6] = cell.Value;

                        i++;
                    }
                    j++;
                }

                rowCount++;

                //GridView1.DataSource = dt;
                //GridView1.DataBind();



            }

            Session["DataTable"] = dt;

        }

        //function : edit data in datatable
        public void EditData()
        {
            DataTable dt = (DataTable)Session["DataTable"];

            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ColumnName.Equals("วันที่"))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!dr[dc].ToString().Equals(""))
                        {
                            //convert double to date
                            DateConvert dateCnv = new DateConvert();
                            dateCnv.Convert(dr[dc].ToString());
                            DateTime datetime = dateCnv.getDate;

                            dr[dc] = String.Format("{0:dd/MM/yy}", datetime);
                        }
                    }
                }
            }

            Session["DataTable"] = dt;

            //GridView1.DataSource = dt;
            //GridView1.DataBind();
        }

        //function : remove columns
        public void RemoveColumn()
        {
            DataTable dt = (DataTable)Session["DataTable"];

            List<string> columnNames = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
            int count = dt.Columns.Count;

            for (int i = 0; i < count; i++)
            {
                if (!(columnNames[i].Equals("A1") || columnNames[i].Equals("A2") || columnNames[i].Equals("วันที่") || columnNames[i].Equals("A4")
                    || columnNames[i].Equals("A5") || columnNames[i].Equals("A6") || columnNames[i].Equals("เลขที่ บส.") || columnNames[i].Equals("รายละเอียด")
                    || columnNames[i].Equals("จ่ายเงินให้") || columnNames[i].Equals("A10") || columnNames[i].Equals("หมายเหตุ") || columnNames[i].Equals("รายการจ่าย")
                    || columnNames[i].Equals("A13")))
                {
                    dt.Columns.Remove(columnNames[i].ToString());
                }
            }

            Session["DataTable"] = dt;

        }

        //function : change columns to true position
        public void ChangePositionColumn()
        {
            DataTable dt = (DataTable)Session["DataTable"];
            try
            {
                // set position column
                dt.Columns["วันที่"].SetOrdinal(2);
                dt.Columns["เลขที่ บส."].SetOrdinal(6);
                dt.Columns["รายละเอียด"].SetOrdinal(7);
                dt.Columns["จ่ายเงินให้"].SetOrdinal(8);
                dt.Columns["หมายเหตุ"].SetOrdinal(10);
                dt.Columns["รายการจ่าย"].SetOrdinal(11);
            }
            catch (NullReferenceException)
            {

            }

            Session["DataTable"] = dt;
        }

        //function : edit columnname to true name
        public void EditColumnName()
        {
            DataTable dt = (DataTable)Session["DataTable"];
            //int countColumn = 0;
            string columnName;

            foreach (DataColumn dc in dt.Columns)
            {
                columnName = dc.ColumnName;

                switch (columnName)
                {
                    case "วันที่":
                        dc.ColumnName = "A3";
                        break;
                    case "เลขที่ บส.":
                        dc.ColumnName = "A7";
                        break;
                    case "รายละเอียด":
                        dc.ColumnName = "A8";
                        break;
                    case "จ่ายเงินให้":
                        dc.ColumnName = "A9";
                        break;
                    case "หมายเหตุ":
                        dc.ColumnName = "A11";
                        break;
                    case "รายการจ่าย":
                        dc.ColumnName = "A12";
                        break;
                }
            }

            Session["DataTable"] = dt;

            //GridView1.DataSource = dt;
            //GridView1.DataBind();
        }

        //function : insert data at blank cell
        public List<string> InsertData()
        {
            DataTable dt = (DataTable)Session["DataTable"];
            int countRows = dt.Rows.Count, count = 0;
            List<string> failList = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                if ((countRows - 1) > count)
                {
                    //insert data in column(A5, A6, A13)
                    List<string> valueList = CompareString(dr["A7"].ToString());

                    dr["A5"] = valueList[0];
                    dr["A6"] = valueList[1];
                    dr["A13"] = valueList[2];

                    //insert data in column(A10)
                    string[] strCode = PayCode(dr["A9"].ToString()).Split('-');

                    if (strCode.Length > 1)
                    {// fail string
                        if (strCode[1].Equals("Fail"))
                        {
                            failList.Add(strCode[0]);
                            dr["A10"] = "0000";
                        }
                    }
                    else if (strCode.Length == 1)
                    {// string is ""
                        if (strCode[0].Equals("FailRow"))
                        {
                            failList.Add("Row " + (count + 4));
                            dr["A10"] = "0000";
                        }
                        else
                        {
                            dr["A10"] = PayCode(dr["A9"].ToString());
                        }
                    }
                    //else
                    //{
                    //    dr["A10"] = PayCode(dr["A9"].ToString());
                    //}

                }
                count++;
            }

            Session["DataTable"] = dt;

            return failList;
        }

        //function : Compare string from data in datatable
        private List<string> CompareString(string s)
        {
            List<string> valueList = new List<string>();

            string codeStr = s.Substring(0, 3);

            string strA5, strA6, strA13;

            strA5 = codeStr.Substring(0, 2) + ".";

            strA6 = codeStr.Substring(0, 2);

            string strA6value = string.Empty;

            switch (strA6)
            {
                case "AI":
                    strA6value = "2-" + strA6 + "001";
                    break;
                case "BU":
                    strA6value = "2-" + codeStr[0] + codeStr[2] + "001";
                    break;
                case "CH":
                    strA6value = "2-" + strA6 + "001";
                    break;
                case "SB":
                    strA6value = "2-" + codeStr[0] + codeStr[2] + "001";
                    break;
                case "SE":
                    strA6value = "2-" + strA6 + "001";
                    break;
                case "UL":
                    strA6value = "2-" + strA6 + "001";
                    break;
            }

            strA13 = "" + codeStr[0];
            string strA13value = string.Empty;

            if (strA13.Equals("U"))
            {
                strA13value = "P" + codeStr[1];
            }
            else
            {
                strA13value = "P" + codeStr[2];
            }

            valueList.Add(strA5);
            valueList.Add(strA6value);
            valueList.Add(strA13value);

            return valueList;
        }

        //function : select code from database
        public string PayCode(string pay)
        {
            CostCenterDBContext db = new CostCenterDBContext();

            string[] strArr = pay.Split('+');

            string strDetail = "", strCode = "";

            string[] strArr2;

            string strChiangrai = "";

            //if (strArr.Length == 0)
            //{
            //    strDetail = strArr[0];
            //}
            if (strArr.Length == 1 || strArr.Length == 2)
            {
                strDetail = strArr[0];

                strArr2 = strDetail.Split('-');

                //string have "เชียงราย"
                if (strArr2.Length > 1 && strArr2[0].Equals("เชียงราย"))
                {
                    strChiangrai = strArr2[1];

                    var code = (from c in db.CostCenterAccounts
                                where c.CostCenterAccountDetail == strChiangrai
                                select new
                                {
                                    c.CostCenterAccountCode,
                                }).ToList();

                    if (code.Count > 0)
                    {
                        strCode = code[0].ToString();
                    }
                    else
                    {
                        //txtExport.Text = strCode + " doesn't have code.";
                        //return "Fail";
                        return strArr2[0] + "-Fail";
                    }
                }
                else if (!strDetail.Equals(""))
                {
                    var code = (from c in db.CostCenterAccounts
                                where c.CostCenterAccountDetail == strDetail
                                select c).FirstOrDefault();

                    if (code != null)
                    {
                        strCode = code.CostCenterAccountCode;
                    }
                    else
                    {
                        //txtExport.Text = strArr2[0] + " doesn't have code.";
                        //return "Fail";
                        return strArr2[0] + "-Fail";
                    }
                }
                else
                {
                    return "FailRow";
                }
            }

            return strCode;

        }

        //function : export datatable to excel  
        public void DatatableToExcel(string fileName, DataTable dt1, string SheetName1 = "SheetName1")
        {
            XLWorkbook wb = new XLWorkbook();
            wb.AddWorksheet(dt1, SheetName1);

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);

            using (MemoryStream tmpMemoryStream = new MemoryStream())
            {
                wb.SaveAs(tmpMemoryStream, false);
                tmpMemoryStream.WriteTo(Response.OutputStream);
                tmpMemoryStream.Close();
            }

            Response.End();
        }

        public void ExportExcel()
        {
            //ExcelToDataTable();
            countDownload = (int)Session["CountDownload"];

            if (countDownload == 0)
            {

                //EditData();
                RemoveColumn();
                ChangePositionColumn();
                EditColumnName();

                //check fail insert data
                List<string> listFail = InsertData();
                List<string> checkList = new List<string>();

                int count = 0;

                if (IsValidateInsertData(listFail) == true)
                {
                    string strError = "";

                    for (int i = 0; i < listFail.Count; i++)
                    {
                        count = 0;

                        for (int j = 0; j < checkList.Count; j++)
                        {
                            if (listFail[i].Equals(checkList[j]))
                            {
                                count++;
                            }
                        }

                        if (count == 0)
                        {
                            checkList.Add(listFail[i]);
                        }

                        //strError += listFail[i] + ", ";
                    }

                    foreach (string s in checkList)
                    {
                        strError += s + ", ";
                    }

                    txtExport.Text = strError + "doesn't have code.";
                }

            }
            //else
            //{
            Session["CountDownload"] = countDownload + 1;

            DataTable dt = (DataTable)Session["DataTable"];
            string fileName = (string)Session["FileName"];

            DatatableToExcel(fileName, dt);
            //}

        }

        protected void Import_Click(object sender, EventArgs e)
        {
            if (!UploadFile.HasFile)
            {
                txtStatus2.Text = "Please select file to upload.";
                txtStatus2.ForeColor = Color.Red;
                txtColumn2.Text = "";
                txtRow2.Text = "";
                txtExport.Text = "";
            }
            else
            {
                ImportExcel();
                txtExport.Text = "";

                if (txtStatus2.Text.Equals("OK"))
                {
                    countDownload = 0;
                    Session["CountDownload"] = countDownload;
                    ExcelToDataTable();
                }
            }
        }

        protected void Export_Click(object sender, EventArgs e)
        {
            if (!txtStatus2.Text.Equals("OK"))
            {

            }
            else
            {



                ExportExcel();

                //ExcelToDataTable();
                //string status = CheckColoumnExport();

                //if(status.Equals("Ok"))
                //{
                //    DataTable dt = (DataTable)Session["LastDataTable"];
                //    string fileName = (string)Session["fileName"];

                //    DatatableToExcel(fileName, dt);
                //}
                //else if(status.Equals("ListFail"))
                //{
                //    List<string> failList = (List<string>)Session["FailList"];
                //    string strError = "";

                //    for (int i = 0; i < failList.Count; i++)
                //    {
                //        strError += failList[i] + ", ";
                //    }

                //    txtExport.Text = strError + "doesn't have code.";
                //}
                ////DataTable dt = (DataTable)Session["LastDataTable"];
                ////string fileName = (string)Session["fileName"];

                ////DatatableToExcel(fileName, dt);
            }
        }

        //function : validate file when upload
        public bool IsValidateImport()
        {
            DataTable dt = (DataTable)Session["Datatable"];

            IXLWorksheet workSheet = (IXLWorksheet)Session["worksheet"];
            //IXLRange range = workSheet.RangeUsed();

            List<string> columnNames = new List<string> { "วันที่", "เลขที่ บส.", "รายละเอียด", "จ่ายเงินให้", "หมายเหตุ", "รายการจ่าย" };
            string columnName = "", columnFail = "";
            int rowCount = 0;
            int columnHead = 0;

            for (int i = 0; i < columnNames.Count; ++i)
            {
                rowCount = 0;

                foreach (IXLRow row in workSheet.Rows())
                {
                    columnHead = 0;

                    if (rowCount == 2)
                    {
                        if (columnNames.Count > 0)
                        {
                            foreach (IXLCell cell in row.Cells())
                            {
                                if (columnHead > 3 && columnHead != 13)
                                {
                                    columnName = cell.Value.ToString();

                                    if (columnNames[i].Equals(columnName))
                                    {
                                        columnNames.RemoveAt(i);
                                        i--;
                                        break;
                                    }
                                }

                                columnHead++;

                            }
                        }

                    }
                    else if (rowCount == 3)
                    {
                        i = 0;

                        if (columnNames.Count > 0)
                        {
                            foreach (IXLCell cell in row.Cells())
                            {
                                if ((columnHead > 0 && columnHead < 4) || columnHead == 13)
                                {
                                    columnName = cell.Value.ToString();

                                    if (columnNames[i].Equals(columnName))
                                    {
                                        columnNames.RemoveAt(i);
                                        i--;
                                        break;
                                    }
                                }

                                columnHead++;

                            }
                        }
                        break;
                    }

                    rowCount++;

                }


            }



            if (columnNames.Count > 0)
            {
                for (int i = 0; i < columnNames.Count; i++)
                {
                    if (i > 0)
                    {
                        columnFail = columnFail + ", " + columnNames[i].ToString();
                    }
                    else
                    {
                        columnFail = columnNames[i].ToString();
                    }
                }

                txtStatus2.Text = "You do not have columns. (" + columnFail + ")";
                txtStatus2.ForeColor = Color.Red;

                return false;
            }

            return true;

        }

        //function : validate listfail
        public bool IsValidateInsertData(List<string> status)
        {
            if (status.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}