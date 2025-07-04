using SmileSDuration.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SmileSDuration.Helper
{
    public class GlobalFunction
    {
        public GlobalFunction()
        {
        }

        /// <summary>
        /// GET IP ADDRESS BY CUTEFUL 14-12-2018
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
        public static DateTime? ConvertDatePickerToDate_BE(string strDate)
        {
            DateTime? result;

            //Init Result
            result = null;

            //Separator
            string[] separators = { "-", "/" };

            //Split String
            if (!String.IsNullOrEmpty(strDate))
            {
                string[] dateSplit = strDate.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                if (dateSplit.Length >= 3)
                {
                    try
                    {
                        //0 = day
                        var d = Convert.ToInt32(dateSplit[0]);
                        //1 = month
                        var m = Convert.ToInt32(dateSplit[1]);
                        //2 = year
                        var y = Convert.ToInt32(dateSplit[2]) - 543;

                        result = new DateTime(y, m, d);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Baht text
        /// </summary>
        /// <param name="strNumber"></param>
        /// <param name="IsTrillion"></param>
        /// <returns></returns>
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

        public static LoginDetail GetLoginDetail(HttpContextBase actionContext)
        {
            var auth = Authorization.GetLoginDetail(actionContext);

            var result = GetUserByUserName(auth.UserName);

            return result;
        }

        private static LoginDetail GetUserByUserName(string username)
        {
            using (SqlConnection conn = new SqlConnection(new DurationV1Entities().Database.Connection.ConnectionString))
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