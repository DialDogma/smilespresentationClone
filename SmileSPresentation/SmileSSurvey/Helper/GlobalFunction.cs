using Newtonsoft.Json;
using RestSharp;
using SmileSSurvey.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;

namespace SmileSSurvey.Helper
{
    public class GlobalFunction
    {
        /// <summary>
        /// Get IP Adress
        /// </summary
        /// <returns></returns>
        ///

        public static SMS_Result SendSMS(string msg, string phoneNo, int smsType, string user)
        {
            //Call Service Send SMS "http://operation.siamsmile.co.th:9215/api/sms/SendSMSV2"
            var client = new RestClient(Properties.Settings.Default.sms_api_URL);

            var param = new { SMSTypeId = smsType, Message = msg, PhoneNo = phoneNo, CreatedById = user };
            //Add Json Body to Request
            var request = new RestRequest().AddJsonBody(param);
            //Add Header Token
            request.AddHeader("Authorization", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwcm9qZWN0aWQiOjl9.XX7R_Ik8pydv2e04ZvrDew8tlszSDrjvTEYKdgO4t7A");
            //Post Request
            var response = client.Post(request);
            //Response
            //Deserialize JSON
            SMS_Result sms = JsonConvert.DeserializeObject<SMS_Result>(response.Content);

            return sms;
        }

        public static string GetIPAddress()
        {
            var userRequest = HttpContext.Current.Request;

            // Get IP Address
            var ipAddress = userRequest.UserHostAddress;

            if (ipAddress != null)
            {
                Int64 macinfo = new Int64();
                string macSrc = macinfo.ToString("X");
                if (macSrc == "0")
                {
                    if (ipAddress == "::1" || ipAddress == "127.0.0.1")
                    {
                        ipAddress = "localhost";
                    }
                    else
                    {
                        ipAddress = ipAddress.ToString();
                    }
                }
            }

            return ipAddress;
        }

        public static string GetUserAgent()
        {
            var userRequest = HttpContext.Current.Request;

            // Get User-Agent (to identify browser or device type)
            var userAgent = userRequest.UserAgent;

            return userAgent;
        }

        /// <summary>
        /// To Datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
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
        /// Export to Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actionContext"></param>
        /// <param name="lst"></param>
        /// <param name="SheetName"></param>
        /// <param name="FileName"></param>
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

        public static LoginDetail GetLoginDetail(HttpContextBase actionContext)
        {
            var auth = Authorization.GetLoginDetail(actionContext);

            var result = GetUserByUserName(auth.UserName);

            return auth;
        }

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

        private static LoginDetail GetUserByUserName(string username)
        {
            using (SqlConnection conn = new SqlConnection(new SurveyEntities().Database.Connection.ConnectionString))
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

        public static (bool IsValid, string Message) ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return (false, "Invalid: Phone number is null or empty.");
            }

            // Trim ช่องว่างรอบ ๆ หมายเลข
            phoneNumber = phoneNumber.Trim();

            if (phoneNumber.Length != 10)
            {
                return (false, "Invalid: Phone number must be exactly 10 characters.");
            }

            if (!phoneNumber.All(char.IsDigit))
            {
                return (false, "Invalid: Phone number contains non-numeric characters.");
            }

            if (!phoneNumber.StartsWith("0"))
            {
                return (false, "Invalid: Phone number must start with 0.");
            }

            if (phoneNumber == "0000000000")
            {
                return (false, "Invalid: Phone number cannot be all zeros.");
            }

            return (true, "Valid: Phone number is correct.");
        }

        public static bool IsValidBase64(string base64String, out int decodedNumber)
        {
            decodedNumber = 0;

            if (string.IsNullOrEmpty(base64String) ||
                !Regex.IsMatch(base64String, @"^[A-Za-z0-9+/]*={0,2}$"))
            {
                return false;
            }

            try
            {
                var base64 = Base64UrlExtensions.FromBase64Url(base64String);

                return int.TryParse(base64, out decodedNumber);
            }
            catch
            {
                return false;
            }
        }
    }
}