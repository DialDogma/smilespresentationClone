using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Drawing;
using System.Text;
using ClosedXML.Excel;
using SmileSConvertFile.Model;

namespace MasterTest
{
    public partial class ConvertExcel5 : System.Web.UI.Page
    {
        int countDownload;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //import text file and choose line
        public void ImportTxt()
        {
            string filePath = Server.MapPath("~/Files/") + Path.GetFileName(UploadFile.PostedFile.FileName);
            UploadFile.SaveAs(filePath);

            string filename = UploadFile.PostedFile.FileName;

            List<string> listTxtLine = new List<string>();

            int count = 0;

            if (System.IO.File.Exists(filePath))
            {
                if (IsValidateFilename(filename) == true)
                {
                    using (System.IO.StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding(874)))
                    {
                        // read and choose line , Add line to List
                        string input;
                        while ((input = sr.ReadLine()) != null)
                        {
                            string[] input2 = input.Split(' ');

                            if (input.Length > 0)
                            {
                                string[] a = input2[0].Split('/');

                                if (input2.Length > 8 || a.Length > 2)
                                {
                                    if (input2[8].ToString().Length == 4 || a.Length == 3)
                                    {
                                        listTxtLine.Add(input);
                                    }
                                    else
                                    {
                                        if (input2[9].Length == 2)
                                        {
                                            listTxtLine.Add(input);
                                        }
                                    }
                                }

                            }

                            count++;
                        }

                        Session["ListText"] = listTxtLine;
                    }

                    txtStatus2.Text = "OK";
                    txtStatus2.ForeColor = Color.Green;

                    string[] subFileName = filename.Split('.');

                    Session["fileName"] = subFileName[0];
                }
                else
                {
                    txtStatus2.Text = "This file format is not supported.";
                }
            }
        }

        //choose line for use
        public void ChooseTextList()
        {
            List<string> listText = (List<string>)Session["ListText"];

            // listtext for use
            List<string> listText2 = new List<string>();

            string[] checkLine1, checkLine2, checkLine3;

            bool AccNum;

            for (int i = 0; i < listText.Count; i++)
            {
                checkLine1 = listText[i].Split(' ');

                if (checkLine1.Length > 8)
                {
                    if (checkLine1[8].Length == 4)
                    {
                        //listText2.Add(listText[i]);
                        AccNum = true;

                        for (int j = i + 1; j < listText.Count; j++)
                        {
                            checkLine2 = listText[j].Split(' ');

                            if (checkLine2.Length > 0)
                            {
                                checkLine3 = checkLine2[0].Split('/');

                                if (checkLine3.Length == 3)
                                {
                                    if (AccNum == true)
                                    {
                                        listText2.Add(listText[i]);

                                        AccNum = false;
                                    }

                                    listText2.Add(listText[j]);
                                }
                                else
                                {
                                    string checkDate = checkLine2[0];

                                    if (checkDate.Equals(""))
                                    {
                                        listText2.Add(listText[j]);
                                    }
                                    else
                                    {
                                        i = j - 1;
                                        break;
                                    }

                                }
                            }
                        }
                    }
                }
            }

            Session["ListText"] = listText2;
        }

        //add text to datattable
        public void TextToDataTable()
        {
            List<string> listText = (List<string>)Session["ListText"];

            DataTable dt = new DataTable();

            string[] arrStr;

            string strAccNum = "", strCostcenter = "", strAccName = "", strDate = "", strDocNum = "", strDes = "", strDebit = "";

            //add columns
            dt.Columns.AddRange(new DataColumn[] {new DataColumn("Period") ,new DataColumn("Txn_Date") ,new DataColumn("Cost_Center") ,new DataColumn("Cost_Type") ,new DataColumn("Doc_Number")
                                                  ,new DataColumn("Account_Number") ,new DataColumn("Acount_Name") ,new DataColumn("Descriptions") ,new DataColumn("Debit")
                                                  ,new DataColumn("Credit") ,new DataColumn("Amount") ,new DataColumn("Cost_Center_Name")});

            foreach (string s in listText)
            {
                arrStr = s.Split(' ');

                if (arrStr[8].Length == 4)
                {
                    strAccNum = arrStr[0];
                    strCostcenter = arrStr[8];
                    strAccName = arrStr[9];
                }
                else
                {
                    //add row to datatable
                    dt.Rows.Add();
                    int i = 1;

                    List<string> list = new List<string>();

                    for (int j = 0; j < arrStr.Length; j++)
                    {
                        if (!arrStr[j].Equals(""))
                        {
                            list.Add(arrStr[j]);
                        }
                    }

                    if (list.Count == 6)
                    {
                        strDate = list[0];
                        strDocNum = list[2];
                        strDes = list[3];
                        strDebit = list[4];
                    }
                    else if (list.Count > 6)
                    {
                        strDate = list[0];
                        strDocNum = list[2];
                        strDes = list[3] + list[4];
                        strDebit = list[5];
                    }
                    else
                    {
                        string strDate2 = strDate;
                        strDocNum = list[1];
                        strDes = list[2];
                        strDebit = list[3];
                    }

                    foreach (DataColumn dc in dt.Columns)
                    {
                        //add data by columnname
                        switch (i)
                        {
                            case 2:
                                dt.Rows[dt.Rows.Count - 1][i - 1] = strDate;
                                break;
                            case 3:
                                dt.Rows[dt.Rows.Count - 1][i - 1] = strCostcenter;
                                break;
                            case 5:
                                dt.Rows[dt.Rows.Count - 1][i - 1] = strDocNum;
                                break;
                            case 6:
                                dt.Rows[dt.Rows.Count - 1][i - 1] = strAccNum;
                                break;
                            case 7:
                                dt.Rows[dt.Rows.Count - 1][i - 1] = strAccName;
                                break;
                            case 8:
                                dt.Rows[dt.Rows.Count - 1][i - 1] = strDes;
                                break;
                            case 9:
                                dt.Rows[dt.Rows.Count - 1][i - 1] = strDebit;
                                break;
                        }

                        i++;

                        //dt.Rows[dt.Rows.Count - 1][i] = 
                    }
                }
            }

            //GridView1.DataSource = dt;
            //GridView1.DataBind();

            Session["DataTable"] = dt;

        }

        //function : edit data in datatable
        public List<string> EditData()
        {
            DataTable dt = (DataTable)Session["DataTable"];

            string period = "";

            List<string> failList = new List<string>();

            //loop columns in datatable
            foreach (DataColumn dc in dt.Columns)
            {
                string columnName = dc.ColumnName;

                foreach (DataRow dr in dt.Rows)
                {
                    if (columnName.Equals("Txn_Date"))
                    {
                        //add row in 'Period' Column
                        string date = dr[columnName].ToString();

                        string[] arrDate = date.Split('/');

                        if (arrDate.Length == 3)
                        {
                            int month = Convert.ToInt32(arrDate[1]);

                            if (month > 9)
                            {
                                period = arrDate[2].Substring(arrDate[2].Length - 2) + month;
                            }
                            else
                            {
                                period = arrDate[2].Substring(arrDate[2].Length - 2) + "0" + month;
                            }
                        }

                        dr["Period"] = period;

                    }
                    else if (columnName.Equals("Debit"))
                    {
                        //edit format to money format
                        double d = Convert.ToDouble(dr[columnName]);
                        dr[columnName] = d.ToString("N2");

                    }
                    else if (columnName.Equals("Credit"))
                    {

                        dr[columnName] = 0;

                    }
                    else if (columnName.Equals("Amount"))
                    {
                        //calculate amount by debit and credit
                        double debit = Convert.ToDouble(dr["Debit"].ToString());

                        double credit = Convert.ToDouble(dr["Credit"].ToString());

                        double d = debit - credit;

                        //edit format to money format
                        dr[columnName] = d.ToString("N2");
                    }
                    else if (columnName.Equals("Cost_Center"))
                    {
                        //search costcentername and depttype by code
                        List<string> listCost = SearchCostFromDataBase(dr[columnName].ToString());

                        if (listCost.Count > 0)
                        {
                            if (IsValidateFailCost(listCost) == true)
                            {
                                failList.Add(listCost[1]);
                            }
                            else
                            {
                                dr["Cost_Type"] = listCost[0];
                                dr["Cost_Center_Name"] = listCost[1];

                            }
                        }
                    }
                    else if (columnName.Equals("Account_Number"))
                    {
                        string accNum = dr["Account_Number"].ToString();

                        string[] accNumUse = accNum.Split('E');
                        //////// check
                        dr["Account_Number"] = accNumUse[1];
                    }
                }
            }



            //GridView1.DataSource = dt;
            //GridView1.DataBind();

            Session["DataTable"] = dt;

            return failList;
        }

        public void EditPosition()
        {
            DataTable dt = (DataTable)Session["DataTable"];

            try
            {

                dt.Columns["Cost_Center_Name"].SetOrdinal(3);

            }
            catch (NullReferenceException)
            {

            }

            Session["DataTable"] = dt;
        }

        //function : search costcentername and depttype in database
        public List<string> SearchCostFromDataBase(string code)
        {
            CostCenterDBContext db = new CostCenterDBContext();
            List<string> list = new List<string>();
            List<string> failList = new List<string>();

            var data = (from c in db.CostCenterAccounts
                        where c.CostCenterAccountCode == code
                        select c).FirstOrDefault();

            if (data != null)
            {
                list.Add(data.DeptType);
                list.Add(data.CostCenterAccountDetail);
            }
            else
            {
                failList.Add("Fail");
                failList.Add(code);

                return failList;
            }

            return list;
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
                //ChooseTextList();

                //TextToDataTable();

                List<string> listFail = EditData();
                List<string> checkList = new List<string>();

                EditPosition();

                //check fail data
                int count = 0;

                if (IsValidateToExcel(listFail) == true)
                {
                    string strFail = "";

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

                    }

                    for (int i = 0; i < checkList.Count; i++)
                    {
                        int j = i + 1;

                        if (j == checkList.Count)
                        {
                            strFail += checkList[i];
                        }
                        else
                        {
                            strFail += checkList[i] + ",";
                        }

                    }

                    txtExport.Text = strFail + "doesn't have code.";
                }
                else
                {
                    Session["CountDownload"] = countDownload + 1;

                    DataTable dt = (DataTable)Session["DataTable"];
                    string fileName = (string)Session["fileName"] + ".xlsx";

                    DatatableToExcel(fileName, dt);
                }
            }
            else
            {
                Session["CountDownload"] = countDownload + 1;

                DataTable dt = (DataTable)Session["DataTable"];
                string fileName = (string)Session["fileName"] + ".xlsx";

                DatatableToExcel(fileName, dt);
            }
        }


        protected void Import_Click(object sender, EventArgs e)
        {
            if (UploadFile.HasFile)
            {
                ImportTxt();
                txtExport.Text = "";

                if (txtStatus2.Text.Equals("OK"))
                {
                    countDownload = 0;
                    Session["CountDownload"] = countDownload;
                    ChooseTextList();
                    TextToDataTable();
                }
            }
            else
            {

                txtStatus2.Text = "Please select file to upload.";
                txtStatus2.ForeColor = Color.Red;
                txtExport.Text = "";
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

        public bool IsValidateFilename(string fileName)
        {
            string name = fileName.Substring(0, 8);
            string filetype = fileName.Substring(fileName.Length - 4);

            if (name.Equals("GLLEDGER") && filetype.Equals(".txt"))
            {
                return true;
            }

            return false;
        }

        public bool IsValidateFailCost(List<string> list)
        {
            try
            {
                if (list[0].Equals("Fail"))
                {
                    return true;
                }
            }
            catch (NullReferenceException)
            {
            }


            return false;
        }

        public bool IsValidateToExcel(List<string> list)
        {
            if (list.Count > 0)
            {
                return true;
            }

            return false;
        }

    }
}