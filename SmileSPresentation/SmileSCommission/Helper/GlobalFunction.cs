using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SmileSCommission.Models;
using static System.String;

namespace SmileSCommission.Helper
{
    public class GlobalFunction
    {
        public static JsonSerializerSettings carmelSetting()
        {
            return new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
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

        /// <summary>
        /// GET CURRENT URL BY CUTEFUL 14-12-2018
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentURL()
        {
            var url = HttpContext.Current.Request.Url.AbsoluteUri;
            var decodedUrl = HttpUtility.UrlDecode(url);

            return decodedUrl;
        }

        public static bool IsNumeric(string s)
        {
            float output;
            return float.TryParse(s, out output);
        }

        /// <summary>
        /// Convert Value From DatePicker To Datetime (พ.ศ.)
        /// เช่น  28/12/2560
        /// </summary>
        /// <param name="strDate"></param>
        /// <param name="strTime"></param>
        /// <returns></returns>
        public static DateTime? ConvertDatePickerToDate_BE(string strDate, string strTime = null)
        {
            var day = 0; //day
            var month = 0; //month
            var year = 0; //year
            var hour = 0;
            var minute = 0;
            var second = 0;

            //Init Result
            DateTime? result = null;

            //Separator
            string[] separators = { "-", "/", ":" };

            //Split String Date
            if (!IsNullOrEmpty(strDate))
            {
                var dateSplit = strDate.Split(separators, StringSplitOptions.RemoveEmptyEntries);

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
            if (!IsNullOrEmpty(strTime))
            {
                var timeSplit = strTime.Split(separators, StringSplitOptions.RemoveEmptyEntries);

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

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public static LoginDetail GetLoginDetail(HttpContextBase actionContext)
        {
            var auth = Authorization.GetLoginDetail(actionContext);

            var result = GetUserByUserName(auth.UserName);

            return result;
        }

        private static LoginDetail GetUserByUserName(string username)
        {
            using (SqlConnection conn = new SqlConnection(new CommissionCalculateEntities().Database.Connection.ConnectionString))
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