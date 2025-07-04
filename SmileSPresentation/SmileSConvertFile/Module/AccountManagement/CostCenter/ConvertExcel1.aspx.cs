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


namespace MasterTest
{
    public partial class WebForm2 : System.Web.UI.Page
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
                        Session["fileName"] = filename;
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
                txtStatus2.ForeColor = Color.Red;

                txtColumn2.Text = "";
                txtRow2.Text = "";
            }
        }

        //function : read excel to datatable
        public void ExcelToDataTable()
        {
            IXLWorksheet workSheet = (IXLWorksheet)Session["worksheet"];
            IXLRange range = workSheet.RangeUsed();

            DataTable dt = new DataTable();

            bool firstRow = true;

            //add excel data to datatable
            foreach (IXLRow wsRow in workSheet.Rows())
            {
                if (firstRow)
                {
                    //add columns
                    dt.Columns.AddRange(new DataColumn[2] { new DataColumn("A1"), new DataColumn("A4") });

                    //Set AutoIncrement True for the First Column.
                    dt.Columns["A1"].AutoIncrement = true;

                    //Set the Starting or Seed value.
                    dt.Columns["A1"].AutoIncrementSeed = 1;

                    //Set the Increment value.
                    dt.Columns["A1"].AutoIncrementStep = 1;

                    foreach (IXLCell cell in wsRow.Cells())
                    {
                        dt.Columns.Add(cell.Value.ToString());

                    }

                    firstRow = false;
                }
                else
                {
                    //stop auto increment
                    if (dt.Rows.Count == (range.RowCount() - 3))
                    {
                        dt.Columns["A1"].AutoIncrement = false;
                    }

                    //Add rows to DataTable.
                    dt.Rows.Add();

                    int i = 1;

                    foreach (IXLCell cell in wsRow.Cells())
                    {
                        dt.Rows[dt.Rows.Count - 1][i + 1] = cell.Value.ToString();
                        i++;
                    }
                }
            }

            //save last datatable
            Session["DataTable"] = dt;
        }

        //function : edit data in datatable
        public void EditData()
        {
            DataTable dt = (DataTable)Session["DataTable"];

            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ColumnName.Equals("วันที่จ่าย"))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!dr[dc].ToString().Equals(""))
                        {
                            DateConvert dateCnv = new DateConvert();
                            dateCnv.Convert(dr[dc].ToString());
                            DateTime datetime = dateCnv.getDate;

                            dr[dc] = String.Format("{0:dd/MM/yyyy}", datetime);
                        }
                    }
                }
                else if (dc.ColumnName.Equals("ยอดรับชำระ") || dc.ColumnName.Equals("XXX"))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!dr[dc].ToString().Equals(""))
                        {
                            //set format money
                            double d = Convert.ToDouble(dr[dc].ToString());
                            dr[dc] = d.ToString("N2");
                        }
                    }
                }
                else if (dc.ColumnName.Equals("ยอดปฏิเสธ"))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr[dc] = "";
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
                if (!(columnNames[i].Equals("A1") || columnNames[i].Equals("เลขที่ชุดจ่าย") || columnNames[i].Equals("วันที่จ่าย") || columnNames[i].Equals("A4")
                    || columnNames[i].Equals("ยอดรับชำระ") || columnNames[i].Equals("XXX") || columnNames[i].Equals("ยอดปฏิเสธ")))
                {
                    dt.Columns.Remove(columnNames[i].ToString());
                }
            }

            Session["DataTable"] = dt;

            //GridView1.DataSource = dt;
            //GridView1.DataBind();
        }

        //function : change columns to true position
        public void ChangePositionColumn()
        {
            DataTable dt = (DataTable)Session["DataTable"];
            try
            {
                //set positions
                dt.Columns["วันที่จ่าย"].SetOrdinal(1);

                dt.Columns["เลขที่ชุดจ่าย"].SetOrdinal(2);
            }
            catch (NullReferenceException)
            {

            }

            Session["DataTable"] = dt;

            //GridView1.DataSource = dt;
            //GridView1.DataBind();
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
                    case "วันที่จ่าย":
                        dc.ColumnName = "A2";
                        break;
                    case "เลขที่ชุดจ่าย":
                        dc.ColumnName = "A3";
                        break;
                    case "ยอดรับชำระ":
                        dc.ColumnName = "A5";
                        break;
                    case "XXX":
                        dc.ColumnName = "A6";
                        break;
                    case "ยอดปฏิเสธ":
                        dc.ColumnName = "A7";
                        break;
                }
            }

            Session["DataTable"] = dt;

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

            txtColumn2.Text = "";

            Response.End();
        }

        public void ExportExcel()
        {
            //ExcelToDataTable();
            countDownload = (int)Session["CountDownload"];

            if (countDownload == 0)
            {

                EditData();
            RemoveColumn();
            ChangePositionColumn();
            EditColumnName();

            DataTable dt = (DataTable)Session["DataTable"];
            string filename = (string)Session["fileName"];

            DatatableToExcel(filename, dt);
            }

            Session["CountDownload"] = countDownload + 1;
        }

        //button import excel to datatable
        protected void Import_Click(object sender, EventArgs e)
        {
            if (!UploadFile.HasFile)
            {
                txtStatus2.Text = "Please select file to upload.";
                txtStatus2.ForeColor = Color.Red;
                txtColumn2.Text = "";
                txtRow2.Text = "";
            }
            else
            {
                ImportExcel();

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
            }
        }

        //function : validate file when upload
        public bool IsValidateImport()
        {
            DataTable dt = (DataTable)Session["Datatable"];

            IXLWorksheet workSheet = (IXLWorksheet)Session["worksheet"];
            //IXLRange range = workSheet.RangeUsed();

            List<string> columnNames = new List<string> { "เลขที่ชุดจ่าย", "วันที่จ่าย", "ยอดรับชำระ", "XXX", "ยอดปฏิเสธ" };
            string columnName, columnFail = "";
            bool firstRow = true;

            for (int i = 0; i < columnNames.Count; ++i)
            {
                foreach (IXLRow row in workSheet.Rows())
                {
                    if (firstRow == true)
                    {
                        foreach (IXLCell cell in row.Cells())
                        {
                            columnName = cell.Value.ToString();

                            if (columnNames[i].Equals(columnName))
                            {
                                columnNames.RemoveAt(i);
                                i--;
                                break;
                            }
                        }
                    }

                    break;
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
    }
}