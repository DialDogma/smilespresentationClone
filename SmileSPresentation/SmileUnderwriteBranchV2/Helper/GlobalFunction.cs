﻿using SmileUnderwriteBranchV2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using static SmileUnderwriteBranchV2.Helper.Authorization;

namespace SmileUnderwriteBranchV2.Helper
{
    public class GlobalFunction
    {
        /// <summary>
        /// GET IP ADDRESS
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress()
        {
            var ip = HttpContext.Current.Request.UserHostAddress;
            if (ip == "::1" || ip == "127.0.0.1")
            {
                ip = "localhost";
            }

            return ip;
        }

        /// <summary>
        /// Get Access Role > Underwrite Branch V2
        /// </summary>
        /// <returns></returns>
        public static string GetAccessRole(string[] arrRole)
        {
            var accessRole = "";
            if (arrRole.Contains("Developer"))
            {
                accessRole = "DEV"; //พัฒนาระบบ
            }
            if (arrRole.Contains("UnderwriteV2_Chairman") && arrRole.Contains("UnderwriteV2_Director") && arrRole.Contains("UnderwriteV2_BusinesPromoteTeam"))
            {
                accessRole = "CM_DIR_BPT"; //ประธาน ฝ่ายส่งเสริม และ ผอ เป็นคนเดียวกัน
            }
            else if (arrRole.Contains("UnderwriteV2_Chairman") && arrRole.Contains("UnderwriteV2_BusinesPromoteTeam"))
            {
                accessRole = "CM_BPT"; //ประธาน และ ฝ่ายส่งเสริม เป็นคนเดียวกัน
            }
            else if (arrRole.Contains("UnderwriteV2_Director") && arrRole.Contains("UnderwriteV2_BusinesPromoteTeam"))
            {
                accessRole = "DIR_BPT"; //ผอ และ ฝ่ายส่งเสริม เป็นคนเดียวกัน
            }
            else if (arrRole.Contains("UnderwriteV2_Chairman") && arrRole.Contains("UnderwriteV2_Director"))
            {
                accessRole = "CM_DIR"; //ประธาน และ ผอ เป็นคนเดียวกัน
            }
            else if (arrRole.Contains("UnderwriteV2_Chairman"))
            {
                accessRole = "CM"; //ประธาน
            }
            else if (arrRole.Contains("UnderwriteV2_Director"))
            {
                accessRole = "DIR"; //ผอ
            }
            else if (arrRole.Contains("UnderwriteV2_BusinesPromoteTeam"))
            {
                accessRole = "BPT"; //ฝ่ายส่งเสริม
            }
            else if (arrRole.Contains("UnderwriteV2_Manager"))
            {
                accessRole = "MGR";//ผู้จัดการฝ่าย
            }
            else if (arrRole.Contains("UnderwriteV2_InsuranceDept"))
            {
                accessRole = "QC";//ฝ่ายรับประกัน
            }
            else if (arrRole.Contains("UnderwriteV2_AdminAssign"))
            {
                accessRole = "ADMIN_ASSIGN";//  โยกคิวงาน สำหรับผู้ดูแล
            }
            else
            {
                accessRole = "GUEST";//อื่นๆ
            }

            return accessRole;
        }

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

        /// <summary>
        /// Get Period List
        /// changePeriodDay,numberOfMonthToShow ในเว็บ config
        /// </summary>
        /// <returns></returns>
        public static List<Period> MotorGetPeriodList(int changePeriodDay, int numberOfMonthToShow)
        {
            var result = new List<Period>();

            // วันที่เปลี่ยนงวด
            //var changePeriodDay = Properties.Settings.Default.ChangePeriodDay;
            //var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;

            var today = DateTime.Today.Day;

            var defaultDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime addDay;

            //ถ้าวันที่มากกว่า  แสดงเดือนถัดไป
            if (today >= changePeriodDay)
            {
                var addMonths = defaultDay.AddMonths(1);

                addDay = addMonths;

                //Add result
                result.Add(new Period
                {
                    Value = addMonths,
                    Display = $"{addMonths:dd/MM/}" + (addMonths.Year + 543)
                });
            }
            else
            {
                //Add result
                result.Add(new Period
                {
                    Value = defaultDay,
                    Display = $"{defaultDay:dd/MM/}" + (defaultDay.Year + 543)
                });

                addDay = defaultDay;
            }

            //Get Period List ย้อนหลัง
            for (var i = 1; i < numberOfMonthToShow; i++)
            {
                addDay = addDay.AddMonths(-1);
                result.Add(new Period
                {
                    Value = addDay,
                    Display = $"{addDay:dd/MM/}" + (addDay.Year + 543)
                });
            }

            return result;
        }

        /// <summary>
        /// Get Period List
        /// </summary>
        /// <returns></returns>
        public static List<Period> GetPeriodList()
        {
            var result = new List<Period>();

            // วันที่เปลี่ยนงวด
            var changePeriodDay = Properties.Settings.Default.ChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;

            var today = DateTime.Today.Day;

            var defaultDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime addDay;

            //ถ้าวันที่มากกว่า  แสดงเดือนถัดไป
            if (today >= changePeriodDay)
            {
                var addMonths = defaultDay.AddMonths(1);

                addDay = addMonths;

                //Add result
                result.Add(new Period
                {
                    Value = addMonths,
                    Display = $"{addMonths:dd/MM/}" + (addMonths.Year + 543)
                });
            }
            else
            {
                //Add result
                result.Add(new Period
                {
                    Value = defaultDay,
                    Display = $"{defaultDay:dd/MM/}" + (defaultDay.Year + 543)
                });

                addDay = defaultDay;
            }

            //Get Period List ย้อนหลัง
            for (var i = 1; i < numberOfMonthToShow; i++)
            {
                addDay = addDay.AddMonths(-1);
                result.Add(new Period
                {
                    Value = addDay,
                    Display = $"{addDay:dd/MM/}" + (addDay.Year + 543)
                });
            }

            return result;
        }

        /*  public static List<Period> GetDayCheck()
          {
              var result = new List<Period>();

              // วันที่เปิด - ปิดปุ่มแก้ไข
              var OpenEditDay = Properties.Settings.Default.OpenEditDay;
              var CloseEditDay = Properties.Settings.Default.CloseEditDay;

              var today = DateTime.Today.Day;

              var defaultDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
              DateTime addDay;

              //ถ้าวันที่มากกว่า  แสดงเดือนถัดไป
              if (today >= changePeriodDay)
              {
                  var addMonths = defaultDay.AddMonths(1);

                  addDay = addMonths;

                  //Add result
                  result.Add(new Period
                  {
                      Value = addMonths,
                      Display = $"{addMonths:dd/MM/}" + (addMonths.Year + 543)
                  });
              }
              else
              {
                  //Add result
                  result.Add(new Period
                  {
                      Value = defaultDay,
                      Display = $"{defaultDay:dd/MM/}" + (defaultDay.Year + 543)
                  });

                  addDay = defaultDay;
              }

              //Get Period List ย้อนหลัง
              for (var i = 1; i < numberOfMonthToShow; i++)
              {
                  addDay = addDay.AddMonths(-1);
                  result.Add(new Period
                  {
                      Value = addDay,
                      Display = $"{addDay:dd/MM/}" + (addDay.Year + 543)
                  });
              }

              return result;
          }*/

        /// <summary>
        /// OverLoad
        /// </summary>
        /// <param name="ChangePeriodDay"></param>
        /// <param name="NumberOfPeriodToShow"></param>
        /// <returns></returns>
        public static List<Period> GetPeriodList(int ChangePeriodDay, int NumberOfPeriodToShow)
        {
            var result = new List<Period>();

            // วันที่เปลี่ยนงวด
            var changePeriodDay = ChangePeriodDay;
            var numberOfMonthToShow = NumberOfPeriodToShow;

            var today = DateTime.Today.Day;

            var defaultDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime addDay;

            //ถ้าวันที่มากกว่า  แสดงเดือนถัดไป
            if (today >= changePeriodDay)
            {
                var addMonths = defaultDay.AddMonths(1);

                addDay = addMonths;

                //Add result
                result.Add(new Period
                {
                    Value = addMonths,
                    Display = $"{addMonths:dd/MM/}" + (addMonths.Year + 543)
                });
            }
            else
            {
                //Add result
                result.Add(new Period
                {
                    Value = defaultDay,
                    Display = $"{defaultDay:dd/MM/}" + (defaultDay.Year + 543)
                });

                addDay = defaultDay;
            }

            //Get Period List ย้อนหลัง
            for (var i = 1; i < numberOfMonthToShow; i++)
            {
                addDay = addDay.AddMonths(-1);
                result.Add(new Period
                {
                    Value = addDay,
                    Display = $"{addDay:dd/MM/}" + (addDay.Year + 543)
                });
            }

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
        public static void ExportToExcel<T>(HttpContextBase actionContext, List<T> lst, string SheetName, string FileName)
        {
            //Convert List To Datatable
            var dt = ToDataTable(lst);

            var wb = new ClosedXML.Excel.XLWorkbook();
            wb.AddWorksheet(dt, SheetName);

            actionContext.Response.Clear();
            actionContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            actionContext.Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xlsx");

            using (var tmpMemoryStream = new MemoryStream())
            {
                wb.SaveAs(tmpMemoryStream);
                tmpMemoryStream.WriteTo(actionContext.Response.OutputStream);
                tmpMemoryStream.Close();
            }
            actionContext.Response.End();
        }

        /// <summary>
        /// Encode base64
        /// </summary>
        /// <param name="originalString"></param>
        /// <returns></returns>
        public static string Base64StringEncode(string originalString)
        {
            var bytes = Encoding.UTF8.GetBytes(originalString);

            var encodedString = Convert.ToBase64String(bytes);

            return encodedString;
        }

        /// <summary>
        /// Decode base64
        /// </summary>
        /// <param name="encodedString"></param>
        /// <returns></returns>
        public static string Base64StringDecode(string encodedString)
        {
            var bytes = Convert.FromBase64String(encodedString);

            var decodedString = Encoding.UTF8.GetString(bytes);

            return decodedString;
        }

        /// <summary>
        /// NumberFormatInfo
        /// </summary>
        /// <param name="NumberFormatInfo"></param>
        /// <returns></returns>
        public static string NumberFormatInfo(string stringNumber, int digits)
        {
            var number = Int64.Parse(stringNumber);

            // Gets a NumberFormatInfo associated with the en-US culture.
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

            // Displays the same value with four decimal digits.
            nfi.NumberDecimalDigits = digits;
            var numberFormat = number.ToString("N", nfi);

            return numberFormat;
        }

        public static LoginDetail GetLoginDetail(HttpContextBase actionContext)
        {
            return OAuth2Helper.GetLoginDetail();
        }

        private static LoginDetail GetUserByUserName(string username)
        {
            using (SqlConnection conn = new SqlConnection(new UnderwriteBranchV2Entities().Database.Connection.ConnectionString))
            {
                conn.Open();
                // 1.  create a command object identifying the stored procedure
                SqlCommand cmd = new SqlCommand("[DataCenterV1].[Security].[usp_GetUserDetailByUserName]", conn);

                // 2. set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // 3. add parameter to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new SqlParameter("UserName", username));

                var dt = new DataTable("UserDetail");
                // execute the command to datatable
                dt.Load(cmd.ExecuteReader());

                var login = new LoginDetail()
                {
                    Result = LoginResult.SUCCESS,
                    UserName = dt.Rows[0]["UserName"] != DBNull.Value ? dt.Rows[0]["UserName"].ToString() : "",
                    User_ID = dt.Rows[0]["User_ID"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["User_ID"]) : 0,
                    Person_ID = dt.Rows[0]["Person_ID"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Person_ID"]) : 0,
                    Employee_ID = dt.Rows[0]["Employee_ID"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Employee_ID"]) : 0,
                    FirstName = dt.Rows[0]["FirstName"] != DBNull.Value ? dt.Rows[0]["FirstName"].ToString() : "",
                    LastName = dt.Rows[0]["LastName"] != DBNull.Value ? dt.Rows[0]["LastName"].ToString() : "",
                    EmployeePositionDetail = dt.Rows[0]["EmployeePositionDetail"] != DBNull.Value ? dt.Rows[0]["EmployeePositionDetail"].ToString() : "",
                    EmployeeTeamDetail = dt.Rows[0]["EmployeeTeamDetail"] != DBNull.Value ? dt.Rows[0]["EmployeeTeamDetail"].ToString() : "",
                    BranchDetail = dt.Rows[0]["BranchDetail"] != DBNull.Value ? dt.Rows[0]["BranchDetail"].ToString() : "",
                    DepartmentDetail = dt.Rows[0]["DepartmentDetail"] != DBNull.Value ? dt.Rows[0]["DepartmentDetail"].ToString() : "",
                    EmployeeTeam_ID = dt.Rows[0]["EmployeeTeam_ID"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["EmployeeTeam_ID"]) : 0,
                    Department_ID = dt.Rows[0]["Department_ID"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Department_ID"]) : 0,
                    Branch_ID = dt.Rows[0]["Branch_ID"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Branch_ID"]) : 0,
                    EmployeePosition_ID = dt.Rows[0]["EmployeePosition_ID"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["EmployeePosition_ID"]) : 0,
                    FullName = $"{dt.Rows[0]["FirstName"]} {dt.Rows[0]["LastName"]}",
                    Organize_ID = dt.Rows[0]["Organize_ID"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Organize_ID"]) : 0,
                    OrganizeCode = dt.Rows[0]["OrganizeCode"] != DBNull.Value ? dt.Rows[0]["OrganizeCode"].ToString() : "",
                    OrganizeDetail = dt.Rows[0]["OrganizeDetail"] != DBNull.Value ? dt.Rows[0]["OrganizeDetail"].ToString() : "",
                    EmployeeCode = dt.Rows[0]["EmployeeCode"] != DBNull.Value ? dt.Rows[0]["EmployeeCode"].ToString() : "",
                };

                return login;
            }
        }

    }
}

public struct Period
{
    public DateTime Value { get; set; }
    public string Display { get; set; }
}