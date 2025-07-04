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
    public partial class ConvertExcel4 : System.Web.UI.Page
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

            //filename
            string filename = UploadFile.PostedFile.FileName;

            if (IsValidateFilename(filename) == true)
            {
                try
                {
                    // read excel file
                    using (XLWorkbook workBook = new XLWorkbook(filePath))
                    {
                        IXLWorksheet workSheet = workBook.Worksheet(1);

                        Session["worksheet"] = workSheet;

                        IXLRange range = workSheet.RangeUsed();

                        if (IsValidateImport() == true)
                        {
                            //count row and columns from excel file
                            txtRow2.Text = Convert.ToString(range.RowCount());
                            txtColumn2.Text = Convert.ToString(range.ColumnCount());

                            txtStatus2.Text = "OK";
                            txtStatus2.ForeColor = Color.Green;

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
            else
            {
                txtStatus2.Text = "This file format is not supported.";
                txtStatus2.ForeColor = Color.Red;
            }



        }

        //function : read excel to datatable
        public void ExcelToDataTable()
        {
            //Read the first Sheet from Excel file.
            IXLWorksheet workSheet = (IXLWorksheet)Session["worksheet"];

            //Create a new DataTable.
            DataTable dt = new DataTable();

            int rowCount = 0;


            foreach (IXLRow wsRow in workSheet.Rows())
            {
                //Use the first row to add columns to DataTable.
                if (rowCount == 4)
                {
                    dt.Columns.AddRange(new DataColumn[] { new DataColumn("Org"), new DataColumn("รถคันที่"), new DataColumn("รหัสพนักงาน"), new DataColumn("ชื่อ-สกุล"), new DataColumn("สาขา") });

                    foreach (IXLCell cell in wsRow.Cells())
                    {
                        dt.Columns.Add(cell.Value.ToString());
                    }
                }
                else if (rowCount > 6)
                {
                    //Add rows to DataTable.
                    dt.Rows.Add();
                    int i = 1;

                    foreach (IXLCell cell in wsRow.Cells())
                    {
                        dt.Rows[dt.Rows.Count - 1][i + 4] = cell.Value.ToString();
                        i++;
                    }
                }

                rowCount++;

                //GridView1.DataSource = dt;
                //GridView1.DataBind();
            }

            Session["DataTable"] = dt;
        }

        //function : edit columnname to true name
        public void EditColumnName()
        {
            DataTable dt = (DataTable)Session["DataTable"];
            int countColumn = 0;

            //loop columns
            foreach (DataColumn dc in dt.Columns)
            {
                //edit column name
                switch (countColumn)
                {
                    case 5:
                        dc.ColumnName = "เลขที่สถานี Merchant ID";
                        break;
                    case 6:
                        dc.ColumnName = "ทะเบียนรถ";
                        break;
                    case 7:
                        dc.ColumnName = "ชื่อสถานี Merchant Name";
                        break;
                    case 8:
                        dc.ColumnName = "ที่ตั้ง Location";
                        break;
                    case 10:
                        dc.ColumnName = "วันที่ทำรายการ Transaction";
                        break;
                    case 11:
                        dc.ColumnName = "เวลา";
                        break;
                    case 12:
                        dc.ColumnName = "เลขที่ ใบกำกับภาษี Invoice No";
                        break;
                    case 16:
                        dc.ColumnName = "จำนวนเงิน (บาท) Amount (Baht)";
                        break;
                }
                countColumn++;
            }

            //GridView1.DataSource = dt;
            //GridView1.DataBind();

            Session["DataTable"] = dt;
        }

        //function : remove columns
        public void RemoveColumn()
        {
            DataTable dt = (DataTable)Session["DataTable"];

            List<string> columnNames = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
            int count = dt.Columns.Count;

            for (int i = 0; i < count; i++)
            {
                if (!(columnNames[i].Equals("Org") || columnNames[i].Equals("วันที่ทำรายการ Transaction") || columnNames[i].Equals("เวลา") || columnNames[i].Equals("เลขที่สถานี Merchant ID")
                    || columnNames[i].Equals("ทะเบียนรถ") || columnNames[i].Equals("รถคันที่") || columnNames[i].Equals("รหัสพนักงาน") || columnNames[i].Equals("ชื่อ-สกุล")
                    || columnNames[i].Equals("สาขา") || columnNames[i].Equals("ชื่อสถานี Merchant Name") || columnNames[i].Equals("ที่ตั้ง Location")
                    || columnNames[i].Equals("เลขที่ ใบกำกับภาษี Invoice No") || columnNames[i].Equals("จำนวนเงิน (บาท) Amount (Baht)")))
                {
                    dt.Columns.Remove(columnNames[i].ToString());
                }
            }

            Session["DataTable"] = dt;

            //GridView1.DataSource = dt;
            //GridView1.DataBind();
        }

        //function : remove rows
        public void RemoveRow()
        {
            DataTable dt = (DataTable)Session["DataTable"];
            int countColumn = 0, countRow = 0;
            string[] strCheck;
            List<int> listRows = new List<int>();

            //add row number to list for remove
            foreach (DataRow dr in dt.Rows)
            {
                countColumn = 0;

                foreach (DataColumn dc in dt.Columns)
                {
                    if (countColumn == 5)
                    {
                        strCheck = dr[dc].ToString().Split('-');

                        if (strCheck.Length != 4)
                        {
                            listRows.Add(countRow);
                        }
                    }

                    countColumn++;
                }
                countRow++;
            }


            //remove row
            int countDelete = 0;

            for (int i = 0; i < listRows.Count; i++)
            {
                if (countDelete == 0)
                {
                    dt.Rows.RemoveAt(listRows[i]);
                    countDelete = 1;
                }
                else
                {
                    dt.Rows.RemoveAt(listRows[i] - countDelete);
                    countDelete++;
                }
            }
            dt.AcceptChanges();

            //GridView1.DataSource = dt;
            //GridView1.DataBind();
        }

        //function : edit data in datatable
        public void EditData()
        {
            DataTable dt = (DataTable)Session["DataTable"];
            string fileName = (string)Session["fileName"];

            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ColumnName.Equals("จำนวนเงิน (บาท) Amount (Baht)"))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        //edit format to money format
                        double d = Convert.ToDouble(dr[dc].ToString());
                        dr[dc] = d.ToString("N2");
                    }
                }
                else if (dc.ColumnName.Equals("Org"))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        //get Org data from filename
                        string ssFilename1 = fileName.Substring(fileName.Length - 12);
                        dr[dc] = ssFilename1.Substring(0, 7);
                    }
                }
            }

            Session["DataTable"] = dt;

            //GridView1.DataSource = dt;
            //GridView1.DataBind();
        }

        //function : change columns to true position
        public void ChangeColumnPosition()
        {
            DataTable dt = (DataTable)Session["DataTable"];
            try
            {
                dt.Columns["วันที่ทำรายการ Transaction"].SetOrdinal(1);
                dt.Columns["เวลา"].SetOrdinal(2);
                dt.Columns["เลขที่สถานี Merchant ID"].SetOrdinal(3);
                dt.Columns["ทะเบียนรถ"].SetOrdinal(4);
            }
            catch (NullReferenceException)
            {

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

            Response.End();
        }

        public void ExportExcel()
        {
            countDownload = (int)Session["CountDownload"];

            if (countDownload == 0)
            {
                //ExcelToDataTable();
                EditColumnName();
                RemoveColumn();
                RemoveRow();
                EditData();
                ChangeColumnPosition();
            }

            Session["CountDownload"] = countDownload + 1;

            DataTable dt = (DataTable)Session["DataTable"];
            string filename = (string)Session["fileName"];

            DatatableToExcel(filename, dt);
        }

        protected void Import_Click(object sender, EventArgs e)
        {
            if (UploadFile.HasFile)
            {

                ImportExcel();

                if (txtStatus2.Text.Equals("OK"))
                {
                    countDownload = 0;
                    Session["CountDownload"] = countDownload;
                    ExcelToDataTable();
                }

            }
            else
            {

                txtStatus2.Text = "Please select file to upload.";
                txtStatus2.ForeColor = Color.Red;
                txtColumn2.Text = "";
                txtRow2.Text = "";

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

        //function : validate file when upload (IsValidateFilename == true)
        public bool IsValidateImport()
        {
            DataTable dt = (DataTable)Session["Datatable"];

            IXLWorksheet workSheet = (IXLWorksheet)Session["worksheet"];
            //IXLRange range = workSheet.RangeUsed();

            List<string> columnNames = new List<string> { "Card No.", "Plate No.", "Station Name"
                                                            , "Location", "Date" ,"Time" ,"Tax Invoice No." ,"Amount\n(Baht)" };
            string columnName, columnFail = "";
            int rowCount = 0;

            for (int i = 0; i < columnNames.Count; ++i)
            {
                rowCount = 0;

                foreach (IXLRow row in workSheet.Rows())
                {
                    if (rowCount == 4)
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

        //function : validate file when upload
        public bool IsValidateFilename(string filename)
        {
            string ssFilename1 = filename.Substring(filename.Length - 12);
            string ssFilename2 = ssFilename1.Substring(0, 7);

            ////แก่ตัวใหญ่
            if (ssFilename2.Equals("tmb-isc"))
            {
                return true;
            }

            return false;
        }
    }
}