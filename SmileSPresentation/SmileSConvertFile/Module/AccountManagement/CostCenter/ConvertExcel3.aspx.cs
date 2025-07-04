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
    public partial class ConvertExcel3 : System.Web.UI.Page
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

            string filename = UploadFile.PostedFile.FileName;

            if (IsValidateFilename(filename) == true)
            {
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

                            //string filename = UploadFile.PostedFile.FileName;
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
            int j = 0;
            string plate = "";

            foreach (IXLRow wsRow in workSheet.Rows())
            {
                //Use the first row to add columns to DataTable.
                if (rowCount == 11)
                {
                    dt.Columns.AddRange(new DataColumn[] { new DataColumn("Org"), new DataColumn("เวลา"), new DataColumn("ทะเบียนรถ") ,new DataColumn("รถคันที่")
                                            ,new DataColumn("รหัสพนักงาน") ,new DataColumn("ชื่อ-สกุล") ,new DataColumn("สาขา")});

                    foreach (IXLCell cell in wsRow.Cells())
                    {
                        dt.Columns.Add(cell.Value.ToString());
                    }
                }
                else if (rowCount > 16)
                {
                    //Add rows to DataTable.
                    dt.Rows.Add();
                    int i = 1;

                    foreach (IXLCell cell in wsRow.Cells())
                    {

                        dt.Rows[dt.Rows.Count - 1][i + 6] = cell.Value.ToString();
                        i++;
                    }
                }

                rowCount++;

                //GridView1.DataSource = dt;
                //GridView1.DataBind();
            }

            Session["DataTable"] = dt;
        }

        //function : Insert ทะเบียนรถ
        public void InsertPlate()
        {
            DataTable dt = (DataTable)Session["DataTable"];
            int countColumn = 0;
            string strPlate = "", plateNo = "";

            foreach (DataRow dr in dt.Rows)
            {
                countColumn = 0;
                foreach (DataColumn dc in dt.Columns)
                {
                    if (countColumn == 13)
                    {
                        strPlate = dr[dc].ToString();

                        if (strPlate.Equals("Plate No."))
                        {
                            Session["PlateNo"] = "PlateNo";
                        }
                    }
                    else
                    {
                        if (countColumn == 14)
                        {

                            if (Session["PlateNo"] != null && Session["PlateNo"].Equals("PlateNo"))
                            {
                                plateNo = dr[dc].ToString();
                                Session["PlateNo"] = null;
                            }
                        }
                    }
                    countColumn++;

                }

                dr["ทะเบียนรถ"] = plateNo;

            }

            Session["DataTable"] = dt;
        }

        //function : edit columnname to true name
        public void EditColumnName()
        {
            DataTable dt = (DataTable)Session["DataTable"];
            int countColumn = 0;

            foreach (DataColumn dc in dt.Columns)
            {
                switch (countColumn)
                { ////// check
                    case 7:
                        dc.ColumnName = "วันที่ทำรายการ Transaction";
                        break;
                    case 9:
                        dc.ColumnName = "เลขที่สถานี Merchant ID";
                        break;
                    case 10:
                        dc.ColumnName = "ชื่อสถานี Merchant Name";
                        break;
                    case 11:
                        dc.ColumnName = "ที่ตั้ง Location";
                        break;
                    case 13:
                        dc.ColumnName = "เลขที่ ใบกำกับภาษี Invoice No";
                        break;
                    case 16:
                        dc.ColumnName = "จำนวนเงิน (บาท) Amount (Baht)";
                        break;
                }
                countColumn++;
            }

            Session["DataTable"] = dt;
        }

        //function : remove rows
        public void RemoveRow()
        {
            DataTable dt = (DataTable)Session["DataTable"];
            int countColumn = 0, countRow = 0;
            string strCheck = "";
            List<int> listRows = new List<int>();

            foreach (DataRow dr in dt.Rows)
            {
                countColumn = 0;

                foreach (DataColumn dc in dt.Columns)
                {
                    if (countColumn == 7)
                    {
                        strCheck = dr[dc].ToString();

                        if (strCheck.Length > 3)
                        {
                            strCheck = strCheck.Substring(strCheck.Length - 3);
                        }
                        //strCheck = strCheck.Substring(strCheck.Length-3);
                    }

                    countColumn++;
                }

                if (!(strCheck.Equals(" AM") || strCheck.Equals(" PM")))
                {
                    listRows.Add(countRow);
                }

                countRow++;
            }

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

        //function : edit data in datatable
        public void EditData()
        {
            DataTable dt = (DataTable)Session["DataTable"];
            string fileName = (string)Session["fileName"];

            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ColumnName.Equals("เลขที่สถานี Merchant ID"))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr[dc] = Convert.ToInt32(dr[dc].ToString());
                    }
                }
                else if (dc.ColumnName.Equals("จำนวนเงิน (บาท) Amount (Baht)"))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        double d = Convert.ToDouble(dr[dc].ToString());
                        dr[dc] = d.ToString("N2");
                    }
                }
                else if (dc.ColumnName.Equals("วันที่ทำรายการ Transaction"))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        //convert double to date
                        string[] dTime = dr[dc].ToString().Split();
                        DateTime date = DateTime.Parse(dTime[0]);
                        dr[dc] = date.ToString("dd/MM/yyyy");

                        DateTime time = DateTime.Parse(dTime[1] + " " + dTime[2]);
                        dr["เวลา"] = time.ToString("HH:mm:ss");
                    }
                }
                else if (dc.ColumnName.Equals("Org"))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        //string amount = dr[dc].ToString();
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
                dt.Columns["เลขที่สถานี Merchant ID"].SetOrdinal(3);
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
                InsertPlate();
                EditColumnName();
                RemoveRow();
                RemoveColumn();
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

        //function : validate file when upload (IsValidateFilename == true)
        public bool IsValidateImport()
        {
            DataTable dt = (DataTable)Session["Datatable"];

            IXLWorksheet workSheet = (IXLWorksheet)Session["worksheet"];
            //IXLRange range = workSheet.RangeUsed();

            List<string> columnNames = new List<string> { "\nวันที่ทำรายการ\nTramsaction Date", "\nเลขที่สถานี\nMerchant ID", "\nชื่อสถานี\nMerchant Name"
                                                            , "\nที่ตั้ง\nLocation", "เลขที่\nใบกำกับภาษี\nInvoice No" ,"จำนวนเงิน\n(บาท)\nAmount\n(Baht)" };
            string columnName, columnFail = "";
            int rowCount = 0;

            for (int i = 0; i < columnNames.Count; ++i)
            {
                rowCount = 0;

                foreach (IXLRow row in workSheet.Rows())
                {
                    if (rowCount == 11)
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

            if (ssFilename2.Equals("ktb-sbt") || ssFilename2.Equals("ktb-isc") || ssFilename2.Equals("ktb-tpa") || ssFilename2.Equals("ktb-ssl"))
            {
                return true;
            }

            return false;
        }
    }
}