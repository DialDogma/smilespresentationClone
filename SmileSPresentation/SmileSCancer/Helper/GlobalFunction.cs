using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web;
using System.IO;
using System.Net;
using System.Web.Mvc;

namespace SmileSIncident.Helper
{
    public class GlobalFunction
    {
        public GlobalFunction()
        {
            ;
        }

        //public static LoginService.LoginDetail GetLoginDetail(HttpContextBase actionContext)
        //{
        //    var result = new LoginService.LoginDetail();
        //    result.UserName = actionContext.@userDetail.LastName;
        //    result.User_ID = Convert.ToInt32(actionContext.Session["User_ID"]);
        //    result.Person_ID = Convert.ToInt32(actionContext.Session["Person_ID"]);
        //    result.Employee_ID = Convert.ToInt32(actionContext.Session["Employee_ID"]);
        //    result.FirstName = actionContext.Session["FirstName"].ToString();
        //    result.LastName = actionContext.Session["LastName"].ToString();
        //    result.EmployeePositionDetail = actionContext.Session["EmployeePositionDetail"].ToString();
        //    result.EmployeeTeamDetail = actionContext.Session["EmployeeTeamDetail"].ToString();
        //    result.BranchDetail = actionContext.Session["BranchDetail"].ToString();
        //    result.DepartmentDetail = actionContext.Session["DepartmentDetail"].ToString();
        //    result.EmployeeTeam_ID = Convert.ToInt32(actionContext.Session["EmployeeTeam_ID"]);
        //    result.Branch_ID = Convert.ToInt32(actionContext.Session["Branch_ID"]);
        //    result.Department_ID = Convert.ToInt32(actionContext.Session["Department_ID"]);
        //    result.EmployeePosition_ID = Convert.ToInt32(actionContext.Session["EmployeePosition_ID"]);
        //    result.Roles = actionContext.Session["Role"].ToString();

        //    return result;
        //}

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        /// <summary>
        /// Convert Value From DatePicker To Datetime (พ.ศ.)
        /// เช่น  28/12/2560
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        /// <summary>
        /// Convert Value From DatePicker To Datetime (พ.ศ.)
        /// เช่น  28/12/2560
        /// </summary>
        /// <param name="strDate"></param>
        /// <param name="strTime"></param>
        /// <returns></returns>
        public static DateTime? ConvertDatePickerToDate_BE(string strDate, string strTime = null)
        {
            DateTime? result;

            var day = 0; //day
            var month = 0; //month
            var year = 0; //year
            var hour = 0;
            var minute = 0;
            var second = 0;

            //Init Result
            result = null;

            //Separator
            string[] separators = { "-", "/", ":" };

            //Split String Date
            if (!String.IsNullOrEmpty(strDate))
            {
                string[] dateSplit = strDate.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                if (dateSplit.Length >= 3)
                {
                    try
                    {
                        //0 = day
                        day = Convert.ToInt32(dateSplit[0]);
                        //1 = month
                        month = Convert.ToInt32(dateSplit[1]);
                        //2 = year
                        year = Convert.ToInt32(dateSplit[2]) - 543;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            //Split String Time
            if (!String.IsNullOrEmpty(strTime))
            {
                string[] timeSplit = strTime.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                if (timeSplit.Length >= 3)
                {
                    try
                    {
                        //hour
                        hour = Convert.ToInt32(timeSplit[0]);
                        //minute
                        minute = Convert.ToInt32(timeSplit[1]);
                        //second
                        second = Convert.ToInt32(timeSplit[2]);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            //Set Result
            result = new DateTime(year, month, day, hour, minute, second);

            return result;
        }

        public static LoginDetail GetLoginDetail(HttpContextBase actionContext)
        {
            var result = Authorization.GetLoginDetail(actionContext);
            return result;
        }

        /// <summary>
        /// Export To Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actionContext">pass this.HttpContext</param>
        /// <param name="lst">list to export</param>
        /// <param name="SheetName">Sheet name</param>
        /// <param name="FileName">File Name</param>
        public static void ExportDatatableToExcel<T>(HttpContextBase actionContext, List<T> lst, string SheetName, string FileName)
        {
            //Convert List To Datatable
            var dt = GlobalFunction.ToDataTable(lst);

            var wb = new ClosedXML.Excel.XLWorkbook();
            wb.AddWorksheet(dt, SheetName);

            actionContext.Response.Clear();
            actionContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            actionContext.Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xlsx");

            using (MemoryStream tmpMemoryStream = new MemoryStream())
            {
                wb.SaveAs(tmpMemoryStream);
                tmpMemoryStream.WriteTo(actionContext.Response.OutputStream);
                tmpMemoryStream.Close();
            }
            actionContext.Response.End();
        }

        public static string ThaiBahtText(string strNumber, bool IsTrillion = false)
        {
            string BahtText = "";
            string strTrillion = "";
            string[] strThaiNumber = { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า", "สิบ" };
            string[] strThaiPos = { "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน" };

            decimal decNumber = 0;
            decimal.TryParse(strNumber, out decNumber);

            if (decNumber == 0)
            {
                return "ศูนย์บาทถ้วน";
            }

            strNumber = decNumber.ToString("0.00");
            string strInteger = strNumber.Split('.')[0];
            string strSatang = strNumber.Split('.')[1];

            if (strInteger.Length > 13)
                throw new Exception("รองรับตัวเลขได้เพียง ล้านล้าน เท่านั้น!");

            bool _IsTrillion = strInteger.Length > 7;
            if (_IsTrillion)
            {
                strTrillion = strInteger.Substring(0, strInteger.Length - 6);
                BahtText = ThaiBahtText(strTrillion, _IsTrillion);
                strInteger = strInteger.Substring(strTrillion.Length);
            }

            int strLength = strInteger.Length;
            for (int i = 0; i < strInteger.Length; i++)
            {
                string number = strInteger.Substring(i, 1);
                if (number != "0")
                {
                    if (i == strLength - 1 && number == "1" && strLength != 1)
                    {
                        BahtText += "เอ็ด";
                    }
                    else if (i == strLength - 2 && number == "2" && strLength != 1)
                    {
                        BahtText += "ยี่";
                    }
                    else if (i != strLength - 2 || number != "1")
                    {
                        BahtText += strThaiNumber[int.Parse(number)];
                    }

                    BahtText += strThaiPos[(strLength - i) - 1];
                }
            }

            if (IsTrillion)
            {
                return BahtText + "ล้าน";
            }

            if (strInteger != "0")
            {
                BahtText += "บาท";
            }

            if (strSatang == "00")
            {
                BahtText += "ถ้วน";
            }
            else
            {
                strLength = strSatang.Length;
                for (int i = 0; i < strSatang.Length; i++)
                {
                    string number = strSatang.Substring(i, 1);
                    if (number != "0")
                    {
                        if (i == strLength - 1 && number == "1" && strSatang[0].ToString() != "0")
                        {
                            BahtText += "เอ็ด";
                        }
                        else if (i == strLength - 2 && number == "2" && strSatang[0].ToString() != "0")
                        {
                            BahtText += "ยี่";
                        }
                        else if (i != strLength - 2 || number != "1")
                        {
                            BahtText += strThaiNumber[int.Parse(number)];
                        }

                        BahtText += strThaiPos[(strLength - i) - 1];
                    }
                }

                BahtText += "สตางค์";
            }

            return BahtText;
        }
    }
}