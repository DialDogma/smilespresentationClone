using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using CovertFile.Model;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

namespace SmileSConvertFileV2
{
    public partial class index : System.Web.UI.Page
    {
        private DataTable dt_Branch, dt_Hospital;
        private CultureInfo ThaiCulture = new CultureInfo("th-TH");

        //CultureInfo usCulture = new CultureInfo("en-US");
        private int countNewRow;

        private List<string> lstErrorCode = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            btnAddErrorCode.Disabled = true;

            if (IsPostBack)
            {
                ClientScript.RegisterHiddenField("isPostBack", "1");
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            btnAddErrorCode.Style.Add("background-color", "#fff");
            btnAddErrorCode.Disabled = true;

            CountFile(lstFile.SelectedValue);

            if (lstErrorCode.Count > 0)
            {
                string errorCodeStr = "";

                for (int i = 0; i < lstErrorCode.Count; i++)
                {
                    if (lstErrorCode[i] != "")
                    {
                        if (i == lstErrorCode.Count - 1)
                        {
                            errorCodeStr += lstErrorCode[i];
                        }
                        else
                        {
                            errorCodeStr = errorCodeStr + lstErrorCode[i] + " ";
                        }
                    }
                }

                errorCode.Value = errorCodeStr;

                btnAddErrorCode.Style.Add("background-color", "#f44336");
                btnAddErrorCode.Disabled = false;
            }
            else
            {
                btnAddErrorCode.Style.Add("background-color", "#fff");
                btnAddErrorCode.Disabled = true;
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            ExportToExcel("สาขา");
        }

        protected void btnDownload2_Click(object sender, EventArgs e)
        {
            ExportToExcel("โรงพยาบาล");
        }

        public void CountFile(string typeFile)
        {
            //กำหนดที่อยู่ไฟล์
            //var pathThd = lstPath.SelectedValue + "/04 งานบัญชีกองทุน/สมุดรายวัน-บัญชี กองทุนเคลมสายไหม/3. บันทึกสรุปการจ่ายสำรองสินไหม(บัญชี709).xlsx";
            //var pathFth = lstPath.SelectedValue + "/04 งานบัญชีกองทุน/สมุดรายวัน-บัญชี กองทุนเคลมสายไหม/5. บันทึกสรุปการจ่ายสินไหมโรงพลาบาลวางบิล(บัญชี282).xlsx";

            //var pathThd = "D:/BKK-Funds/04 งานบัญชีกองทุน/สมุดรายวัน-บัญชี กองทุนเคลมสายไหม/3. บันทึกสรุปการจ่ายสำรองสินไหม(บัญชี709).xlsx";
            //var pathFth = "D:/BKK-Funds/04 งานบัญชีกองทุน/สมุดรายวัน-บัญชี กองทุนเคลมสายไหม/5. บันทึกสรุปการจ่ายสินไหมโรงพลาบาลวางบิล(บัญชี282).xlsx";

            //var pathThd = "Z:/04 งานบัญชีกองทุน/สมุดรายวัน-บัญชี กองทุนเคลมสายไหม/3. บันทึกสรุปการจ่ายสำรองสินไหม(บัญชี709).xlsx";
            //var pathFth = "Z:/04 งานบัญชีกองทุน/สมุดรายวัน-บัญชี กองทุนเคลมสายไหม/5. บันทึกสรุปการจ่ายสินไหมโรงพลาบาลวางบิล(บัญชี282).xlsx";

            //var pathThd = "E:/Work/CovertFile บช/New folder/3. บันทึกสรุปการจ่ายสำรองสินไหม(บัญชี709).xlsx";
            //var pathFth = "E:/Work/CovertFile บช/New folder/5. บันทึกสรุปการจ่ายสินไหมโรงพลาบาลวางบิล(บัญชี282).xlsx";

            //var pathThd = CovertFile.Properties.Settings.Default.pathThd;
            //var pathFth = CovertFile.Properties.Settings.Default.pathFth;

            try
            {
                //เช็คว่าต้องการไฟล์ user ต้องการไฟล์ชนิดไหน
                if (typeFile.Equals("RA"))
                {
                    var pathThd_RA = CovertFile.Properties.Settings.Default.pathThd_RA;
                    var pathFth_RA = CovertFile.Properties.Settings.Default.pathFth_RA;

                    //สร้าง datatable สำหรับเก็ยข้อมูลที่อ่านจาก excel
                    //dt_Branch ไฟล์ 3.
                    dt_Branch = new DataTable();
                    //dt_Hospital ไฟล์ 5.
                    dt_Hospital = new DataTable();
                    //กำหนดตัวนับ row ให้เป็น 0
                    countNewRow = 0;

                    for (int i = 0; i < 2; i++)
                    {
                        if (i == 0)
                        {
                            countNewRow = 0;
                            //อ่านไฟล์ 3.
                            ReadFileRA(pathThd_RA, i, tbDate.Text);
                        }
                        else
                        {
                            countNewRow = 0;
                            //อ่านไฟล์ 5.
                            ReadFileRA(pathFth_RA, i, tbDate.Text);
                        }
                    }
                    ShowTable();
                }
                //เช็คว่าต้องการไฟล์ user ต้องการไฟล์ชนิดไหน
                else if (typeFile.Equals("BC"))
                {
                    var pathThd_BC = CovertFile.Properties.Settings.Default.pathThd_BC;
                    var pathFth_BC = CovertFile.Properties.Settings.Default.pathFth_BC;
                    //สร้าง datatable สำหรับเก็ยข้อมูลที่อ่านจาก excel
                    //dt_Branch ไฟล์ 3.
                    dt_Branch = new DataTable();
                    //dt_Hospital ไฟล์ 5.
                    dt_Hospital = new DataTable();
                    //กำหนดตัวนับ row ให้เป็น 0
                    countNewRow = 0;

                    for (int i = 0; i < 2; i++)
                    {
                        if (i == 0)
                        {
                            //อ่านไฟล์ 3.
                            ReadFileBC(pathThd_BC, i, tbDate.Text);
                        }
                        else
                        {
                            //อ่านไฟล์ 5.
                            ReadFileBC(pathFth_BC, i, tbDate.Text);
                        }
                    }
                    ShowTable();
                }
            }
            //จับ catch เมื่อ path ผิด
            catch (DirectoryNotFoundException)
            {
                Response.Write("<script>alert('โปรดตรวจสอบที่อยู่ไฟล์');</script>");
                tbDate.Text = "";
                btnSearch.Enabled = false;
            }
        }

        public void ReadFileRA(string filePath, int countFile, string txtDate)
        {
            try
            {
                //รับไฟล์ excel จาก path

                using (XLWorkbook workbook = new XLWorkbook(filePath))
                {
                    //XLWorkbook workbook = new XLWorkbook(filePath);
                    IXLWorksheet worksheet;

                    if (countFile == 0)
                    {
                        //สร้าง column ให้กับ dt_Branch
                        dt_Branch.Columns.AddRange(new DataColumn[14] { new DataColumn("A1"), new DataColumn("A2"), new DataColumn("A3"), new DataColumn("A4"), new DataColumn("A5"), new DataColumn("A6")
                                                    , new DataColumn("A7"), new DataColumn("A8"), new DataColumn("A9"), new DataColumn("A10"), new DataColumn("A11"), new DataColumn("A12")
                                                    , new DataColumn("A13"), new DataColumn("A14")});

                        //วนลูปตามจำนวน sheet ที่ต้องการอ่าน
                        for (int i = 0; i < 4; i++)
                        {
                            if (i == 0)
                            {
                                //อ่าน sheet ที่ชื่อ ปกติ
                                worksheet = workbook.Worksheet("ปกติ");
                                //ส่ง worksheet ไปทำการอ่านและหาข้อมูลตาม txtDate ที่ user ต้องการ
                                AddDataToDataTable_RA(worksheet, txtDate, "fileThd");
                            }
                            else if (i == 1)
                            {
                                //อ่าน sheet ที่ชื่อ เสียชีวิต
                                worksheet = workbook.Worksheet("เสียชีวิต");
                                //ส่ง worksheet ไปทำการอ่านและหาข้อมูลตาม txtDate ที่ user ต้องการ
                                AddDataToDataTable_RA(worksheet, txtDate, "fileThd");
                            }
                            else if (i == 2)
                            {
                                //อ่าน sheet ที่ชื่อ ความรับผิด
                                worksheet = workbook.Worksheet("ความรับผิด");
                                //ส่ง worksheet ไปทำการอ่านและหาข้อมูลตาม txtDate ที่ user ต้องการ
                                AddDataToDataTable_RA(worksheet, txtDate, "fileThd");
                            }
                            else if (i == 3)
                            {
                                //อ่าน sheet ที่ชื่อ ครอบครัวห่วงใย+ประกันบ้าน
                                worksheet = workbook.Worksheet("ครอบครัวห่วงใย+ประกันบ้าน");
                                //ส่ง worksheet ไปทำการอ่านและหาข้อมูลตาม txtDate ที่ user ต้องการ
                                AddDataToDataTable_RA(worksheet, txtDate, "fileThd");
                            }
                        }
                    }
                    else
                    {
                        //สร้าง column ให้กับ dt_Hospital
                        dt_Hospital.Columns.AddRange(new DataColumn[14] { new DataColumn("A1"), new DataColumn("A2"), new DataColumn("A3"), new DataColumn("A4"), new DataColumn("A5"), new DataColumn("A6")
                                                    , new DataColumn("A7"), new DataColumn("A8"), new DataColumn("A9"), new DataColumn("A10"), new DataColumn("A11"), new DataColumn("A12")
                                                    , new DataColumn("A13"), new DataColumn("A14")});

                        for (int i = 0; i < 2; i++)
                        {
                            if (i == 0)
                            {
                                //อ่าน sheet ที่ชื่อ ปกติ
                                worksheet = workbook.Worksheet("ปกติ");
                                //ส่ง worksheet ไปทำการอ่านและหาข้อมูลตาม txtDate ที่ user ต้องการ
                                AddDataToDataTable_RA(worksheet, txtDate, "fileFth");
                            }
                            else
                            {
                                //อ่าน sheet ที่ชื่อ โรงพยาบาล
                                worksheet = workbook.Worksheet("โรงพยาบาล");
                                //ส่ง worksheet ไปทำการอ่านและหาข้อมูลตาม txtDate ที่ user ต้องการ
                                AddDataToDataTable_RA(worksheet, txtDate, "fileFth");
                            }
                        }
                    }
                }
            }
            catch (IOException)
            {
                Response.Write("<script>alert('ไฟล์กำลังถูกใช้งาน');</script>");
            }
        }

        public void ReadFileBC(string filePath, int countFile, string txtDate)
        {
            try
            {
                using (var workbook = new XLWorkbook(filePath))
                {
                    // XLWorkbook workbook = new XLWorkbook(filePath);
                    IXLWorksheet worksheet;
                    //IXLRange range;

                    TypeConvert tcv = new TypeConvert();

                    if (countFile == 0)
                    {
                        dt_Branch.Columns.AddRange(new DataColumn[13] { new DataColumn("A1"), new DataColumn("A2"), new DataColumn("A3"), new DataColumn("A4"), new DataColumn("A5"), new DataColumn("A6")
                                                    , new DataColumn("A7"), new DataColumn("A8"), new DataColumn("A9"), new DataColumn("A10"), new DataColumn("A11"), new DataColumn("A12", typeof(decimal))
                                                    , new DataColumn("A13")});
                        for (int i = 0; i < 4; i++)
                        {
                            if (i == 0)
                            {
                                worksheet = workbook.Worksheet("ปกติ");

                                AddDataToDataTable_BC(worksheet, txtDate, "fileThd", "");
                            }
                            else if (i == 1)
                            {
                                worksheet = workbook.Worksheet("เสียชีวิต");

                                AddDataToDataTable_BC(worksheet, txtDate, "fileThd", "");
                            }
                            else if (i == 2)
                            {
                                worksheet = workbook.Worksheet("ความรับผิด");

                                AddDataToDataTable_BC(worksheet, txtDate, "fileThd", "");
                            }
                            else if (i == 3)
                            {
                                worksheet = workbook.Worksheet("ครอบครัวห่วงใย+ประกันบ้าน");

                                AddDataToDataTable_BC(worksheet, txtDate, "fileThd", "ครอบครัวห่วงใย+ประกันบ้าน");
                            }
                        }
                    }
                    else
                    {
                        dt_Hospital.Columns.AddRange(new DataColumn[13] { new DataColumn("A1"), new DataColumn("A2"), new DataColumn("A3"), new DataColumn("A4"), new DataColumn("A5"), new DataColumn("A6")
                                                    , new DataColumn("A7"), new DataColumn("A8"), new DataColumn("A9"), new DataColumn("A10"), new DataColumn("A11"), new DataColumn("A12", typeof(decimal))
                                                    , new DataColumn("A13")});

                        for (int i = 0; i < 2; i++)
                        {
                            if (i == 0)
                            {
                                worksheet = workbook.Worksheet("ปกติ");

                                AddDataToDataTable_BC(worksheet, txtDate, "fileFth", "ปกติ");
                            }
                            else
                            {
                                worksheet = workbook.Worksheet("โรงพยาบาล");

                                AddDataToDataTable_BC(worksheet, txtDate, "fileFth", "โรงพยาบาล");
                            }
                        }
                    }
                }
            }
            catch (IOException)
            {
                Response.Write("<script>alert('ไฟล์กำลังถูกใช้งาน');</script>");
            }

            Session["DataTable_Branch"] = dt_Branch;
            Session["DataTable_Hospital"] = dt_Hospital;
        }

        protected void gdvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //จัดการเรื่องเลขหน้าของ gridview

            gdvData.DataSource = Session["DataTable_Branch"];

            gdvData.PageIndex = e.NewPageIndex;
            gdvData.DataBind();
        }

        protected void gdvData2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //จัดการเรื่องเลขหน้าของ gridview

            gdvData2.DataSource = Session["DataTable_Hospital"];

            gdvData2.PageIndex = e.NewPageIndex;
            gdvData2.DataBind();
        }

        public void ShowTable()
        {
            //รับ DataTable จาก Session
            DataTable dt_Branch = (DataTable)Session["DataTable_Branch"];
            DataTable dt_Hospital = (DataTable)Session["DataTable_Hospital"];

            //try
            //{
            //เช็คว่า ใน dt_Branch กับ dt_Hospital มีข้อมูลหรือไม่
            if (dt_Branch.Rows.Count == 0 && dt_Hospital.Rows.Count == 0)
            {
                btnDownload.Enabled = false;
                btnDownload2.Enabled = false;

                gdvData.Visible = false;
                gdvData2.Visible = false;

                tbDate.Text = "";
                Response.Write("<script>alert('ไม่พบข้อมูล');</script>");
            }
            else
            {
                //เช็คว่า dt_Branch มีข้อมูลหรือไม่
                if (dt_Branch.Rows.Count == 0)
                {
                    btnDownload.Enabled = false;
                    gdvData.Visible = false;
                }
                else
                {
                    gdvData.DataSource = dt_Branch;
                    gdvData.DataBind();

                    btnDownload.Enabled = true;
                    btnSearch.Enabled = true;
                    gdvData.Visible = true;
                }

                //เช็คว่า dt_Hospital มีข้อมูลหรือไม่
                if (dt_Hospital.Rows.Count == 0)
                {
                    btnDownload2.Enabled = false;
                    gdvData2.Visible = false;
                }
                else
                {
                    gdvData2.DataSource = dt_Hospital;
                    gdvData2.DataBind();

                    btnDownload2.Enabled = true;
                    btnSearch.Enabled = true;
                    gdvData2.Visible = true;
                }
            }
            //}
            //catch (Exception)
            //{
            //    Server.Transfer("ErrorPage.aspx");
            //}
        }

        public void ExportToExcel(string typeFileName)
        {
            DataTable dt;
            var wb = new XLWorkbook();

            //เช็คว่าเป็นไฟล์ สาขา หรือ โรงพยาบาล
            if (typeFileName.Equals("สาขา"))
            {
                dt = (DataTable)Session["DataTable_Branch"];
                wb.AddWorksheet(dt, "Sheet1");
            }
            else if (typeFileName.Equals("โรงพยาบาล"))
            {
                dt = (DataTable)Session["DataTable_Hospital"];
                wb.AddWorksheet(dt, "Sheet1");
            }

            IXLWorksheet worksheet = wb.Worksheet("Sheet1");

            worksheet.Cell("L1").Style.NumberFormat.Format = "#,##0.00";

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + lstFile.SelectedValue + "-" + typeFileName + ".xlsx");

            using (MemoryStream tmpMemoryStream = new MemoryStream())
            {
                wb.SaveAs(tmpMemoryStream, false);
                tmpMemoryStream.WriteTo(Response.OutputStream);
                tmpMemoryStream.Close();
            }

            Response.End();
        }

        public string[] getColumnId_RA(IXLWorksheet worksheet)
        {
            string[] arrCol = new string[9];

            //วนลูปเพื่อหาตำแหน่งของชื่อ Column ที่จำเป็นต้องใช้
            foreach (IXLRow wsRow in worksheet.Rows(1, 1))
            {
                string headName;
                string colAddr;

                foreach (IXLCell cell in wsRow.Cells())
                {
                    headName = cell.Value.ToString();
                    colAddr = cell.Address.ToString();

                    switch (headName)
                    {
                        case "บ.":
                        case "บริษัท":
                            if (colAddr.Length == 2)
                            {
                                arrCol[0] = colAddr[0].ToString();
                            }
                            else
                            {
                                arrCol[0] = colAddr[0].ToString() + colAddr[1].ToString();
                            }
                            break;

                        case "เลขที่ชุดจ่าย":
                            if (colAddr.Length == 2)
                            {
                                arrCol[1] = colAddr[0].ToString();
                            }
                            else
                            {
                                arrCol[1] = colAddr[0].ToString() + colAddr[1].ToString();
                            }
                            break;

                        case "ยอดจ่าย":
                        case " ยอดจ่าย":
                            if (colAddr.Length == 2)
                            {
                                arrCol[2] = colAddr[0].ToString();
                            }
                            else
                            {
                                arrCol[2] = colAddr[0].ToString() + colAddr[1].ToString();
                            }
                            break;

                        case "วันที่จ่าย":
                            if (colAddr.Length == 2)
                            {
                                arrCol[3] = colAddr[0].ToString();
                            }
                            else
                            {
                                arrCol[3] = colAddr[0].ToString() + colAddr[1].ToString();
                            }
                            break;

                        case "หมายเหตุก่อนส่งวางบิล":
                            if (colAddr.Length == 2)
                            {
                                arrCol[4] = colAddr[0].ToString();
                            }
                            else
                            {
                                arrCol[4] = colAddr[0].ToString() + colAddr[1].ToString();
                            }
                            break;

                        case "ครั้งที่วางบิลบริษัทประกัน":
                            if (colAddr.Length == 2)
                            {
                                arrCol[5] = colAddr[0].ToString();
                            }
                            else
                            {
                                arrCol[5] = colAddr[0].ToString() + colAddr[1].ToString();
                            }
                            break;

                        case "วันที่รับชำระบริษัทประกัน":
                            if (colAddr.Length == 2)
                            {
                                arrCol[6] = colAddr[0].ToString();
                            }
                            else
                            {
                                arrCol[6] = colAddr[0].ToString() + colAddr[1].ToString();
                            }
                            break;

                        case "ยอดรับชำระ":
                            if (colAddr.Length == 2)
                            {
                                arrCol[7] = colAddr[0].ToString();
                            }
                            else
                            {
                                arrCol[7] = colAddr[0].ToString() + colAddr[1].ToString();
                            }
                            break;

                        case "ยอดปฏิเสธ":
                            if (colAddr.Length == 2)
                            {
                                arrCol[8] = colAddr[0].ToString();
                            }
                            else
                            {
                                arrCol[8] = colAddr[0].ToString() + colAddr[1].ToString();
                            }
                            break;
                    }

                    if (arrCol[8] != null)
                    {
                        break;
                    }
                }
            }

            return arrCol;
        }

        public string[] getColumnId_BC(IXLWorksheet worksheet)
        {
            string[] arrCol = new string[8];

            //วนลูปเพื่อหาตำแหน่งของชื่อ Column ที่จำเป็นต้องใช้
            foreach (IXLRow wsRow in worksheet.Rows(1, 1))
            {
                string headName;
                string colAddr;

                foreach (IXLCell cell in wsRow.Cells())
                {
                    headName = cell.Value.ToString();
                    colAddr = cell.Address.ToString();

                    switch (headName)
                    {
                        case "บ.":
                        case "บริษัท":
                            if (colAddr.Length == 2)
                            {
                                arrCol[0] = colAddr[0].ToString();
                            }
                            else
                            {
                                arrCol[0] = colAddr[0].ToString() + colAddr[1].ToString();
                            }
                            break;

                        case "กลุ่มเคลม":
                            if (colAddr.Length == 2)
                            {
                                arrCol[1] = colAddr[0].ToString();
                            }
                            else
                            {
                                arrCol[1] = colAddr[0].ToString() + colAddr[1].ToString();
                            }
                            break;

                        case "สาขา":
                            if (colAddr.Length == 2)
                            {
                                arrCol[2] = colAddr[0].ToString();
                            }
                            else
                            {
                                arrCol[2] = colAddr[0].ToString() + colAddr[1].ToString();
                            }
                            break;

                        case "เลขที่ชุดจ่าย":
                            if (colAddr.Length == 2)
                            {
                                arrCol[3] = colAddr[0].ToString();
                            }
                            else
                            {
                                arrCol[3] = colAddr[0].ToString() + colAddr[1].ToString();
                            }
                            break;

                        case "ยอดจ่าย":
                        case " ยอดจ่าย":
                            if (colAddr.Length == 2)
                            {
                                arrCol[4] = colAddr[0].ToString();
                            }
                            else
                            {
                                arrCol[4] = colAddr[0].ToString() + colAddr[1].ToString();
                            }
                            break;

                        case "วันที่จ่าย":
                            if (colAddr.Length == 2)
                            {
                                arrCol[5] = colAddr[0].ToString();
                            }
                            else
                            {
                                arrCol[5] = colAddr[0].ToString() + colAddr[1].ToString();
                            }
                            break;

                        case "จ่ายโดย":
                            if (colAddr.Length == 2)
                            {
                                arrCol[6] = colAddr[0].ToString();
                            }
                            else
                            {
                                arrCol[6] = colAddr[0].ToString() + colAddr[1].ToString();
                            }
                            break;
                    }

                    if (arrCol[6] != null)
                    {
                        break;
                    }
                }
            }

            return arrCol;
        }

        public void AddDataToDataTable_RA(IXLWorksheet worksheet, string txtDate, string orderFile)
        {
            //try
            //{
            IXLRange range = worksheet.RangeUsed();

            //เก็บจำนวน row ทั้งหมดใน sheet
            var count = range.RowCount();

            //เก็บตำแหน่ง column
            string[] arrHeadColID = getColumnId_RA(worksheet);

            //ประกาศคลาส
            TypeConvert tcv = new TypeConvert();

            //วนลูปเพื่อจัดเก็บข้อมูลลง datatable
            for (int j = 2; j < count; j++)
            {
                string chkRow_DatePay = worksheet.Cell(arrHeadColID[3] + j).Value.ToString();
                string chkRow_PayNo = worksheet.Cell(arrHeadColID[1] + j).Value.ToString();
                string chkDateGetPaid = worksheet.Cell(arrHeadColID[6] + j).Value.ToString();
                string chkCompany = worksheet.Cell(arrHeadColID[0] + j).Value.ToString();

                //เช็คข้อมูลจากววันที่จ่ายกับเลขที่จ่ายว่ามีข้อมูลหรือไม่ (กันพลาด)
                if (!(chkRow_DatePay.Equals("")))
                {
                    if (!(chkRow_DatePay.Equals("") && chkRow_PayNo.Equals("")))
                    {
                        //เช็ควันที่รับชำระ
                        if (!chkDateGetPaid.Equals(""))
                        {
                            string date = tcv.DateConvert(chkDateGetPaid).ToString("d/M/yy", ThaiCulture);

                            //string date = tcv.getDate.ToString("d/M/yy", ThaiCulture);

                            //เช็ควันที่ที่ตรงกับวันที่ user ต้องการ
                            if (date.Equals(txtDate))
                            {
                                //เช็คบริษัทให้ตรงกับที่ user ต้องการ
                                if (chkCompany.Equals(lstCompany.SelectedValue))
                                {
                                    DataRow dr = null;

                                    //เช็คว่าเป็นไฟล์ 3. หรือ 5. เพื่อสร้าง datarow
                                    if (orderFile.Equals("fileThd"))
                                    {
                                        dr = dt_Branch.NewRow();
                                    }
                                    else if (orderFile.Equals("fileFth"))
                                    {
                                        dr = dt_Hospital.NewRow();
                                    }

                                    //บันทึกข้อมูลลง datarow ตามตำแหน่งคอลัมน์
                                    dr[0] = worksheet.Cell(arrHeadColID[0] + j).Value.ToString();
                                    dr[1] = worksheet.Cell(arrHeadColID[1] + j).Value.ToString();
                                    //dr[2] = CostConvert(worksheet.Cell(arrHeadColID[2] + j).Value.ToString());
                                    dr[2] = worksheet.Cell(arrHeadColID[2] + j).GetDouble();
                                    //dr[3] = worksheet.Cell(arrHeadColID[3] + j).Value.ToString();

                                    dr[3] = tcv.DateConvert(worksheet.Cell(arrHeadColID[3] + j).Value.ToString()).ToString("d/M/yy", ThaiCulture);

                                    dr[4] = worksheet.Cell(arrHeadColID[4] + j).Value.ToString();
                                    dr[5] = worksheet.Cell(arrHeadColID[5] + j).Value.ToString();
                                    //dr[6] = worksheet.Cell(arrHeadColID[6] + j).Value.ToString();
                                    dr[6] = date;

                                    try
                                    {
                                        //dr[7] = CostConvert(worksheet.Cell(arrHeadColID[7] + j).ValueCached.ToString());
                                        dr[7] = Convert.ToDouble(worksheet.Cell(arrHeadColID[7] + j).ValueCached.ToString());
                                    }
                                    catch (NullReferenceException)
                                    {
                                        //ถ้าเป็น null ให้ใช้ value ปกติ
                                        //dr[7] = CostConvert(worksheet.Cell(arrHeadColID[7] + j).Value.ToString());

                                        //dr[7] = worksheet.Cell(arrHeadColID[7] + j).GetDouble();
                                        dr[7] = worksheet.Cell(arrHeadColID[7] + j).Value.ToString();
                                    }

                                    dr[8] = worksheet.Cell(arrHeadColID[8] + j).Value.ToString();

                                    //เช็คว่าเป็นไฟล์ 3. หรือ 5. เพื่อ insert datarow
                                    if (orderFile.Equals("fileThd"))
                                    {
                                        dt_Branch.Rows.InsertAt(dr, countNewRow);
                                    }
                                    else if (orderFile.Equals("fileFth"))
                                    {
                                        dt_Hospital.Rows.InsertAt(dr, countNewRow);
                                    }

                                    countNewRow++;
                                }
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            //เก็บ datatable ไว้ใน session เพื่อนำไปใช้ต่อ
            Session["DataTable_Branch"] = dt_Branch;
            Session["DataTable_Hospital"] = dt_Hospital;
            //}
            //catch (Exception)
            //{
            //    Server.Transfer("ErrorPage.aspx");

            //}
        }

        public void AddDataToDataTable_BC(IXLWorksheet worksheet, string txtDate, string orderFile, string sheetName)
        {
            //try
            //{
            IXLRange range = worksheet.RangeUsed();

            //เก็บจำนวน row ทั้งหมดใน sheet
            var count = range.RowCount();

            //เก็บตำแหน่ง column
            string[] arrHeadColID = getColumnId_BC(worksheet);

            //ประกาศคลาส
            TypeConvert tcv = new TypeConvert();

            //วนลูปเพื่อจัดเก็บข้อมูลลง datatable
            for (int j = 2; j < count; j++)
            {
                string chkRow_DatePay = worksheet.Cell(arrHeadColID[5] + j).Value.ToString();
                string chkRow_PayNo = worksheet.Cell(arrHeadColID[3] + j).Value.ToString();

                //เช็คข้อมูลจากววันที่จ่ายกับเลขที่จ่ายว่ามีข้อมูลหรือไม่
                if (!(chkRow_DatePay.Equals("")))
                {
                    if (!(chkRow_DatePay.Equals("") && chkRow_PayNo.Equals("")))
                    {
                        string date = tcv.DateConvert(chkRow_DatePay).ToString("d/M/yy", ThaiCulture);

                        //string date = tcv.getDate.ToString("d/M/yy", ThaiCulture);

                        //เช็ควันที่ที่ตรงกับวันที่ user ต้องการ
                        if (date.Equals(txtDate) && !worksheet.Cell(arrHeadColID[4] + j).IsEmpty())
                        {
                            DataRow dr = null;

                            //เช็คว่าเป็นไฟล์ 3. หรือ 5. เพื่อสร้าง datarow
                            if (orderFile.Equals("fileThd"))
                            {
                                dr = dt_Branch.NewRow();
                            }
                            else if (orderFile.Equals("fileFth"))
                            {
                                dr = dt_Hospital.NewRow();
                            }

                            //บันทึกข้อมูลลง datarow ตามตำแหน่งคอลัมน์
                            dr[0] = "0000";
                            dr[1] = "ฝ่ายกองทุนสินไหม";
                            //dr[2] = worksheet.Cell(arrHeadColID[5] + j).Value.ToString();
                            dr[2] = date;

                            try
                            {
                                dr[4] = worksheet.Cell(arrHeadColID[0] + j).Value.ToString();
                            }
                            catch
                            {
                                dr[4] = "";
                            }

                            dr[5] = CustomerCodeConvert(worksheet.Cell(arrHeadColID[3] + j).Value.ToString());
                            dr[6] = worksheet.Cell(arrHeadColID[3] + j).Value.ToString();
                            dr[7] = PayConvert(worksheet.Cell(arrHeadColID[6] + j).Value.ToString(), orderFile, sheetName);

                            //string[] provinceArrResult = CheckProvince(worksheet.Cell(arrHeadColID[2] + j).Value.ToString());
                            //dr[8] = provinceArrResult[0];
                            //dr[9] = provinceArrResult[1];

                            if (orderFile.Equals("fileThd"))
                            {
                                if (sheetName.Equals("ครอบครัวห่วงใย+ประกันบ้าน"))
                                {
                                    string[] provinceArrResult = CheckProvince(worksheet.Cell(arrHeadColID[1] + j).Value.ToString(), worksheet.Cell(arrHeadColID[2] + j).Value.ToString(), orderFile, sheetName, worksheet.Cell(arrHeadColID[2] + j).Value.ToString());
                                    dr[8] = provinceArrResult[0];
                                    dr[9] = provinceArrResult[1];
                                }
                                else
                                {
                                    string[] provinceArrResult = CheckProvince(worksheet.Cell(arrHeadColID[1] + j).Value.ToString(), worksheet.Cell(arrHeadColID[3] + j).Value.ToString(), orderFile, sheetName, worksheet.Cell(arrHeadColID[2] + j).Value.ToString());
                                    dr[8] = provinceArrResult[0];
                                    dr[9] = provinceArrResult[1];
                                }
                            }
                            else if (orderFile.Equals("fileFth"))
                            {
                                if (sheetName.Equals("ปกติ"))
                                {
                                    string[] provinceArrResult = CheckProvince(worksheet.Cell(arrHeadColID[1] + j).Value.ToString(), worksheet.Cell(arrHeadColID[2] + j).Value.ToString(), orderFile, sheetName, "");
                                    dr[8] = provinceArrResult[0];
                                    dr[9] = provinceArrResult[1];
                                }
                                else
                                {
                                    string[] provinceArrResult = CheckProvince(worksheet.Cell(arrHeadColID[1] + j).Value.ToString(), worksheet.Cell(arrHeadColID[3] + j).Value.ToString(), orderFile, sheetName, worksheet.Cell(arrHeadColID[2] + j).Value.ToString());
                                    dr[8] = provinceArrResult[0];
                                    dr[9] = provinceArrResult[1];
                                }
                            }

                            //dr[11] = CostConvert(worksheet.Cell(arrHeadColID[4] + j).Value.ToString());
                            //dr[11] = Convert.ToDecimal(worksheet.Cell(arrHeadColID[4] + j).GetDouble());

                            if (worksheet.Cell(arrHeadColID[4] + j).Value.ToString() != null)
                            {
                                dr[11] = worksheet.Cell(arrHeadColID[4] + j).GetDouble();
                            }
                            else
                            {
                                dr[11] = worksheet.Cell(arrHeadColID[4] + j).Value.ToString();
                            }

                            dr[12] = ProductGroupConvert(worksheet.Cell(arrHeadColID[1] + j).Value.ToString());

                            //เช็คว่าเป็นไฟล์ 3. หรือ 5. เพื่อ insert datarow
                            if (orderFile.Equals("fileThd"))
                            {
                                dt_Branch.Rows.InsertAt(dr, countNewRow);
                            }
                            else if (orderFile.Equals("fileFth"))
                            {
                                dt_Hospital.Rows.InsertAt(dr, countNewRow);
                            }

                            countNewRow++;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            Session["DataTable_Branch"] = dt_Branch;
            Session["DataTable_Hospital"] = dt_Hospital;
            //}
            //catch (Exception)
            //{
            //    Server.Transfer("ErrorPage.aspx");
            //}
        }

        //แปลงข้อมูลให้เป็นทศนิยม 2 ตำแหน่ง
        public string CostConvert(string strCost)
        {
            string cost = "";

            if (strCost.Equals("0"))
            {
                cost = "-";
            }
            else if (strCost.Equals(""))
            {
                cost = strCost;
            }
            else
            {
                float f = float.Parse(strCost);
                cost = f.ToString("n2");
            }

            return cost;
        }

        //แปลงเลขที่จ่ายให้เป็นรหัสลูกค้า
        public string CustomerCodeConvert(string payNo)
        {
            string customerCode = "";
            string chkPayNo = payNo.Trim().Substring(0, 2);

            switch (chkPayNo)
            {
                case "AI":
                    customerCode = "2-AI001";
                    break;

                case "SE":
                    customerCode = "2-SE001";
                    break;

                case "BU":
                    chkPayNo = payNo.Trim().Substring(0, 3);

                    if (chkPayNo.Equals("BUH"))
                    {
                        customerCode = "2-BH001";
                    }
                    else
                    {
                        customerCode = "2-BA001";
                    }
                    break;

                case "UL":
                    customerCode = "2-UL001";
                    break;

                case "SB":
                    chkPayNo = payNo.Trim().Substring(0, 3);

                    if (chkPayNo.Equals("SBA"))
                    {
                        customerCode = "2-SA001";
                    }
                    else
                    {
                        customerCode = "2-SH001";
                    }
                    break;

                case "VR":
                    chkPayNo = payNo.Trim().Substring(0, 3);

                    if (chkPayNo.Equals("VRH"))
                    {
                        customerCode = "2-VHA001";
                    }
                    else
                    {
                        customerCode = "2-VA001";
                    }
                    break;

                case "CH":
                    chkPayNo = payNo.Trim().Substring(0, 3);

                    if (chkPayNo.Equals("CHH"))
                    {
                        customerCode = "2-CH001";
                    }
                    else
                    {
                        customerCode = "2-CA001";
                    }
                    break;

                case "KA":
                    customerCode = "2-KA001";
                    break;

                case "KP":
                    customerCode = "2-KP001";
                    break;

                case "SH":
                    customerCode = "2-SH001";
                    break;
            }

            return customerCode;
        }

        //แปลงเขตการขายให้เป้นนหัส 01 02 03
        public string PayConvert(string typePay, string orderFile, string sheetName)
        {
            string payCode = "";

            try
            {
                int chkPayNo = Convert.ToInt32(typePay);

                payCode = "01";
            }
            catch (Exception)
            {
                if (orderFile.Equals("fileThd"))
                {
                    if (typePay.Equals("E-Banking-CM"))
                    {
                        payCode = "01";
                    }
                    else if (typePay.Equals("E-Banking") || typePay.Equals("E-Banking-RE") || typePay.Equals("Reclaim"))
                    {
                        payCode = "02";
                    }
                }
                else
                {
                    if (sheetName.Equals("ปกติ"))
                    {
                        payCode = "01";
                    }
                    else if (sheetName.Equals("โรงพยาบาล"))
                    {
                        payCode = "03";
                    }
                }
            }

            return payCode;
        }

        //เช็คชื่อจังหวัดกับรหัสพนักงานขายใน database
        public string[] CheckProvince(string chkClaim, string chkData, string orderFile, string sheetName, string chkBranch)
        {
            string[] provinceArrResult = new string[2];

            SSSPAEntities sSSPAEntities = new SSSPAEntities();

            SSSEntities1 sSSEntities = new SSSEntities1();

            DataCenterV1Entities masterData = new DataCenterV1Entities();

            var branchResult = "";

            CheckProvince_Result provinceResult;

            if (orderFile.Equals("fileThd"))
            {
                if (sheetName.Equals("ครอบครัวห่วงใย+ประกันบ้าน"))
                {
                    provinceResult = masterData.CheckProvince(chkData).FirstOrDefault();

                    if (provinceResult != null)
                    {
                        provinceArrResult[0] = provinceResult.Province;
                        provinceArrResult[1] = provinceResult.SalesmanCode;
                    }
                    else
                    {
                        provinceArrResult[0] = chkData;
                        provinceArrResult[1] = "ไม่พบรหัส";

                        int rptCount = 0;

                        for (int i = 0; i < lstErrorCode.Count; i++)
                        {
                            if (lstErrorCode[i] == branchResult)
                            {
                                rptCount++;
                                break;
                            }
                        }

                        if (rptCount == 0)
                        {
                            lstErrorCode.Add(branchResult);
                        }
                    }
                }
                else
                {
                    branchResult = sSSEntities.CovertFile_CheckDataPH_select(chkData, orderFile).FirstOrDefault();
                    //provinceResult = masterData.CheckProvince(branchResult).FirstOrDefault();

                    if (branchResult != null)
                    {
                        provinceResult = masterData.CheckProvince(branchResult).FirstOrDefault();

                        if (provinceResult != null)
                        {
                            provinceArrResult[0] = provinceResult.Province;
                            provinceArrResult[1] = provinceResult.SalesmanCode;
                        }
                        else
                        {
                            provinceArrResult[0] = branchResult;
                            provinceArrResult[1] = "ไม่พบรหัส";

                            int rptCount = 0;

                            for (int i = 0; i < lstErrorCode.Count; i++)
                            {
                                if (lstErrorCode[i] == branchResult)
                                {
                                    rptCount++;
                                    break;
                                }
                            }

                            if (rptCount == 0)
                            {
                                lstErrorCode.Add(branchResult);
                            }
                        }
                    }
                    else
                    {
                        branchResult = sSSPAEntities.CovertFile_CheckDataPA_select(chkData, orderFile).FirstOrDefault();
                        //provinceResult = masterData.CheckProvince(branchResult).FirstOrDefault();

                        if (branchResult != null)
                        {
                            provinceResult = masterData.CheckProvince(branchResult).FirstOrDefault();

                            if (provinceResult != null)
                            {
                                provinceArrResult[0] = provinceResult.Province;
                                provinceArrResult[1] = provinceResult.SalesmanCode;
                            }
                            else
                            {
                                provinceArrResult[0] = branchResult;
                                provinceArrResult[1] = "ไม่พบรหัส";

                                int rptCount = 0;

                                for (int i = 0; i < lstErrorCode.Count; i++)
                                {
                                    if (lstErrorCode[i] == branchResult)
                                    {
                                        rptCount++;
                                        break;
                                    }
                                }

                                if (rptCount == 0)
                                {
                                    lstErrorCode.Add(branchResult);
                                }
                            }
                        }
                        else
                        {
                            provinceResult = masterData.CheckProvince(chkBranch).FirstOrDefault();

                            if (provinceResult != null)
                            {
                                provinceArrResult[0] = provinceResult.Province;
                                provinceArrResult[1] = provinceResult.SalesmanCode;
                            }
                            else
                            {
                                provinceArrResult[0] = branchResult;
                                provinceArrResult[1] = "ไม่พบรหัส";

                                int rptCount = 0;

                                for (int i = 0; i < lstErrorCode.Count; i++)
                                {
                                    if (lstErrorCode[i] == branchResult)
                                    {
                                        rptCount++;
                                        break;
                                    }
                                }

                                if (rptCount == 0)
                                {
                                    lstErrorCode.Add(branchResult);
                                }
                            }

                            //provinceArrResult[0] = branchResult;
                            //provinceArrResult[1] = "ไม่พบรหัส";
                        }
                    }
                }

                //if (chkClaim.Equals("สุขภาพ") || chkClaim.Equals("PA30"))
                //{
                //    branchResult = sSSEntities.CovertFile_CheckDataPH_select(chkData, orderFile).FirstOrDefault();
                //    provinceResult = masterData.CheckProvince(branchResult).FirstOrDefault();

                //    if (provinceResult != null)
                //    {
                //        provinceArrResult[0] = provinceResult.Province;
                //        provinceArrResult[1] = provinceResult.SalesmanCode;
                //    }
                //    else
                //    {
                //        provinceArrResult[0] = branchResult;
                //        provinceArrResult[1] = "ไม่พบรหัส";
                //    }
                //}
                //else if (chkClaim.Equals("อุบัติเหตุ"))
                //{
                //    branchResult = sSSPAEntities.CovertFile_CheckDataPA_select(chkData, orderFile).FirstOrDefault();
                //    provinceResult = masterData.CheckProvince(branchResult).FirstOrDefault();

                //    if (provinceResult != null)
                //    {
                //        provinceArrResult[0] = provinceResult.Province;
                //        provinceArrResult[1] = provinceResult.SalesmanCode;
                //    }
                //    else
                //    {
                //        provinceArrResult[0] = branchResult;
                //        provinceArrResult[1] = "ไม่พบรหัส";
                //    }
                //}
            }
            else
            {
                if (sheetName.Equals("ปกติ"))
                {
                    provinceResult = masterData.CheckProvince(chkData).FirstOrDefault();

                    if (provinceResult != null)
                    {
                        provinceArrResult[0] = provinceResult.Province;
                        provinceArrResult[1] = provinceResult.SalesmanCode;
                    }
                    else
                    {
                        branchResult = sSSPAEntities.CovertFile_CheckDataPA_select(chkData, orderFile).FirstOrDefault();
                        provinceResult = masterData.CheckProvince(branchResult).FirstOrDefault();

                        if (provinceResult != null)
                        {
                            provinceArrResult[0] = provinceResult.Province;
                            provinceArrResult[1] = provinceResult.SalesmanCode;
                        }
                        else
                        {
                            if (chkData == "สำนักงานใหญ่")
                            {
                                provinceArrResult[0] = chkData;
                                provinceArrResult[1] = "ไม่พบรหัส";

                                int rptCount = 0;

                                for (int i = 0; i < lstErrorCode.Count; i++)
                                {
                                    if (lstErrorCode[i] == chkData)
                                    {
                                        rptCount++;
                                        break;
                                    }
                                }

                                if (rptCount == 0)
                                {
                                    lstErrorCode.Add(chkData);
                                }
                            }
                            else
                            {
                                provinceArrResult[0] = branchResult;
                                provinceArrResult[1] = "ไม่พบรหัส";

                                int rptCount = 0;

                                for (int i = 0; i < lstErrorCode.Count; i++)
                                {
                                    if (lstErrorCode[i] == branchResult)
                                    {
                                        rptCount++;
                                        break;
                                    }
                                }

                                if (rptCount == 0)
                                {
                                    lstErrorCode.Add(branchResult);
                                }
                            }
                        }
                    }
                }
                else if (sheetName.Equals("โรงพยาบาล"))
                {
                    branchResult = sSSEntities.CovertFile_CheckDataPH_select(chkData, orderFile).FirstOrDefault();
                    //provinceResult = masterData.CheckProvince(branchResult).FirstOrDefault();

                    if (branchResult != null)
                    {
                        provinceResult = masterData.CheckProvince(branchResult).FirstOrDefault();

                        if (provinceResult != null)
                        {
                            provinceArrResult[0] = provinceResult.Province;
                            provinceArrResult[1] = provinceResult.SalesmanCode;
                        }
                        else
                        {
                            provinceArrResult[0] = branchResult;
                            provinceArrResult[1] = "ไม่พบรหัส";

                            int rptCount = 0;

                            for (int i = 0; i < lstErrorCode.Count; i++)
                            {
                                if (lstErrorCode[i] == branchResult)
                                {
                                    rptCount++;
                                    break;
                                }
                            }

                            if (rptCount == 0)
                            {
                                lstErrorCode.Add(branchResult);
                            }
                        }
                    }
                    else
                    {
                        branchResult = sSSPAEntities.CovertFile_CheckDataPA_select(chkData, orderFile).FirstOrDefault();
                        //provinceResult = masterData.CheckProvince(branchResult).FirstOrDefault();

                        if (branchResult != null)
                        {
                            provinceResult = masterData.CheckProvince(branchResult).FirstOrDefault();

                            if (provinceResult != null)
                            {
                                provinceArrResult[0] = provinceResult.Province;
                                provinceArrResult[1] = provinceResult.SalesmanCode;
                            }
                            else
                            {
                                provinceArrResult[0] = branchResult;
                                provinceArrResult[1] = "ไม่พบรหัส";

                                int rptCount = 0;

                                for (int i = 0; i < lstErrorCode.Count; i++)
                                {
                                    if (lstErrorCode[i] == branchResult)
                                    {
                                        rptCount++;
                                        break;
                                    }
                                }

                                if (rptCount == 0)
                                {
                                    lstErrorCode.Add(branchResult);
                                }
                            }
                        }
                        else
                        {
                            provinceResult = masterData.CheckProvince(chkBranch).FirstOrDefault();

                            if (provinceResult != null)
                            {
                                provinceArrResult[0] = provinceResult.Province;
                                provinceArrResult[1] = provinceResult.SalesmanCode;
                            }
                            else
                            {
                                provinceArrResult[0] = branchResult;
                                provinceArrResult[1] = "ไม่พบรหัส";

                                int rptCount = 0;

                                for (int i = 0; i < lstErrorCode.Count; i++)
                                {
                                    if (lstErrorCode[i] == branchResult)
                                    {
                                        rptCount++;
                                        break;
                                    }
                                }

                                if (rptCount == 0)
                                {
                                    lstErrorCode.Add(branchResult);
                                }
                            }
                        }
                    }

                    //if (chkClaim.Equals("สุขภาพ") || chkClaim.Equals("PA30"))
                    //{
                    //    branchResult = sSSEntities.CovertFile_CheckDataPH_select(chkData, orderFile).FirstOrDefault();
                    //    provinceResult = masterData.CheckProvince(branchResult).FirstOrDefault();

                    //    if (provinceResult != null)
                    //    {
                    //        provinceArrResult[0] = provinceResult.Province;
                    //        provinceArrResult[1] = provinceResult.SalesmanCode;
                    //    }
                    //    else
                    //    {
                    //        if(branchResult == null)
                    //        {
                    //            provinceArrResult[0] = "-";
                    //            provinceArrResult[1] = "H888-000";
                    //        }
                    //        else
                    //        {
                    //            provinceArrResult[0] = branchResult;
                    //            provinceArrResult[1] = "H888-000";
                    //        }

                    //    }
                    //}
                    //else if (chkClaim.Equals("อุบัติเหตุ"))
                    //{
                    //    branchResult = sSSPAEntities.CovertFile_CheckDataPA_select(chkData, orderFile).FirstOrDefault();
                    //    provinceResult = masterData.CheckProvince(branchResult).FirstOrDefault();

                    //    if (provinceResult != null)
                    //    {
                    //        provinceArrResult[0] = provinceResult.Province;
                    //        provinceArrResult[1] = provinceResult.SalesmanCode;
                    //    }
                    //    else
                    //    {
                    //        if (branchResult == null)
                    //        {
                    //            provinceArrResult[0] = "-";
                    //            provinceArrResult[1] = "H888-000";
                    //        }
                    //        else
                    //        {
                    //            provinceArrResult[0] = branchResult;
                    //            provinceArrResult[1] = "H888-000";
                    //        }

                    //    }
                    //}
                }
            }

            return provinceArrResult;
        }

        //แปลงกลุ่มผลิตภัณฑ์เป็นตัวย่อ
        public string ProductGroupConvert(string productGroup)
        {
            string productGroupCode = "";

            if (productGroup.Equals("อุบัติเหตุ") || productGroup.Equals("PA30") || productGroup.Equals("PAชุมชนห่วงใย"))
            {
                productGroupCode = "PA";
            }
            else if (productGroup.Equals("ประกันชีวิต"))
            {
                productGroupCode = "PL";
            }
            else if (productGroup.Equals("สุขภาพ"))
            {
                productGroupCode = "PH";
            }
            else if (productGroup.Equals("ประกันบ้าน"))
            {
                productGroupCode = "CH";
            }
            else
            {
                productGroupCode = "ไม่ทราบข้อมูล";
            }

            return productGroupCode;
        }

        protected void btnSaveErrorcodes_Click(object sender, EventArgs e)
        {
            DataCenterV1Entities centerV1Entities = new DataCenterV1Entities();

            string province = errorCode.Value;
            string salemanCode = saveCode.Value;

            string[] provinceArr = province.Trim().Split(' ');
            string[] codeArr = salemanCode.Trim().Split(' ');

            if (codeArr.Length == provinceArr.Length)
            {
                for (int i = 0; i < provinceArr.Length; i++)
                {
                    centerV1Entities.usp_MasterConvertData_Insert(provinceArr[i], codeArr[i]);
                }

                Response.Redirect(Request.Url.ToString(), true);
            }
        }

        protected void btnSaveAddcodes_ServerClick(object sender, EventArgs e)
        {
            DataCenterV1Entities centerV1Entities = new DataCenterV1Entities();

            string province = provincetxt.Value;
            string salemanCode = salemancodetxt.Value;

            centerV1Entities.usp_MasterConvertData_Insert(province, salemanCode);

            Response.Redirect(Request.Url.ToString(), true);
        }
    }
}