using SmileSUnderwrite.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Web;
using static SmileSUnderwrite.Helper.Authorization;

namespace SmileSUnderwrite.Helper
{
    public class GlobalFunction
    {
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

        public static SmileSLoginService.LoginDetail GetLoginDetail(HttpContextBase actionContext)
        {
            var result = new SmileSLoginService.LoginDetail();

            var user = GetUserByUserName(actionContext.Session["Username"].ToString());

            result.UserName = user.UserName;
            result.User_ID = user.User_ID;
            result.Person_ID = user.Person_ID;
            result.Employee_ID = user.Employee_ID;
            result.FirstName = user.FirstName;
            result.LastName = user.LastName;
            result.EmployeePositionDetail = user.EmployeePositionDetail;
            result.EmployeeTeamDetail = user.EmployeeTeamDetail;
            result.BranchDetail = user.BranchDetail;
            result.DepartmentDetail = user.DepartmentDetail;
            result.EmployeeTeam_ID = user.EmployeeTeam_ID;
            result.Branch_ID = user.Branch_ID;
            result.Department_ID = user.Department_ID;
            result.EmployeePosition_ID = user.EmployeePosition_ID;
            result.Roles = actionContext.Session["Role"].ToString();

            return result;
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
                        int yy = Convert.ToInt32(dateSplit[2]);
                        int y;
                        if (yy > 2500)
                        {
                            y = yy - 543;
                        }
                        else
                        {
                            y = yy;
                        }

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

        //public static LoginDetail GetLoginDetail(HttpContextBase actionContext)
        //{
        //    var auth = Authorization.GetLoginDetail(actionContext);

        //    var result = GetUserByUserName(auth.UserName);

        //    return result;
        //}

        private static LoginDetail GetUserByUserName(string username)
        {
            using (SqlConnection conn = new SqlConnection(new UnderwriteDBContext().Database.Connection.ConnectionString))
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