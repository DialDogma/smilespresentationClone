using SmileSBankDirectDebit.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Web;

namespace SmileSBankDirectDebit.Helper
{
    public class GlobalFunction
    {
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

        public static string CreateBackupFile(HttpPostedFileBase file)
        {
            string result;
            try
            {
                //CreateDirectory
                DateTime now = DateTime.Now;

                var sourcePath = Properties.Settings.Default.PathBackupFile;
                string Year = now.Year.ToString();
                string Month = now.Month.ToString("00");
                string Day = now.Day.ToString("00");

                sourcePath = sourcePath + "\\" + Year + "\\" + Month + "\\In";

                if (!Directory.Exists(sourcePath))
                {
                    Directory.CreateDirectory(sourcePath);
                }

                //GenFileName
                var originalFileName = Path.GetFileName(file.FileName);
                var fileName = "\\" + (DateTime.Now.ToShortDateString()).Replace("/", "-") + "_" + originalFileName;

                var path = sourcePath + fileName;

                file.SaveAs(path);

                result = path;
            }
            catch (Exception e)
            {
                result = "";
            }

            return result;
        }

        /// <summary>
        /// Export To Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst">list to export</param>
        /// <param name="SheetName">Sheet name</param>
        /// <param name="FileName">File Name</param>
        public static string ExportToExcel<T>(List<T> lst, string SheetName, string FileName, string folderName)
        {
            try
            {
                //Convert List To Datatable
                var dt = GlobalFunction.ToDataTable(lst);

                var wb = new ClosedXML.Excel.XLWorkbook();
                wb.AddWorksheet(dt, SheetName);

                var now = DateTime.Now;
                var sourcePath = Properties.Settings.Default.PathBackupFile;
                var Year = now.Year.ToString();
                var Month = now.Month.ToString("00");
                var Day = now.Day.ToString("00");

                sourcePath = sourcePath + "\\" + Year + "\\" + Month + "\\out\\" + folderName;

                if (!Directory.Exists(sourcePath))
                {
                    Directory.CreateDirectory(sourcePath);
                }

                //var fileNameNew = "\\" + (DateTime.Now.ToShortDateString()).Replace("/","-") + "_" + FileName;

                var fileNameNew = "\\" + (DateTime.Today.ToString("yyyyMMdd")) + "-" + FileName;

                var filePath = sourcePath + fileNameNew + ".xlsx";

                wb.SaveAs(filePath);

                return filePath;
            }
            catch (Exception e)
            {
                return e.Message;
                throw;
            }
        }

        /// <summary>
        /// Add To Zip  If Error Return Error Message, If Success Return nothing
        /// </summary>
        /// <param name="sourceDirectory">eg. c:\example\start</param>
        /// <param name="targetDirectory">eg. c:\example\result.zip</param>
        /// <returns></returns>
        public static string AddToZip(string sourceDirectory, string targetDirectory)
        {
            //Dont forget to add reference
            //System.IO.Compression.FileSystem
            //Example source : https://docs.microsoft.com/en-us/dotnet/api/system.io.compression.zipfile?redirectedfrom=MSDN&view=netframework-4.7.2
            try
            {
                System.IO.Compression.ZipFile.CreateFromDirectory(sourceDirectory, targetDirectory);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
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

        /// <summary>
        /// Export To Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actionContext">pass this.HttpContext</param>
        /// <param name="lst">list to export</param>
        /// <param name="SheetName">Sheet name</param>
        /// <param name="FileName">File Name</param>
        public static void ExportToExcel2<T>(HttpContextBase actionContext, List<T> lst, string SheetName, string FileName)
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
            return OAuth2Helper.GetLoginDetail();
        }

        private static LoginDetail GetUserByUserName(string username)
        {
            using (SqlConnection conn = new SqlConnection(new BankDirectDebitDBContext().Database.Connection.ConnectionString))
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

                if (dt.Rows.Count == 0)
                {
                    return new LoginDetail()
                    {
                        Result = LoginResult.ERROR,
                        ErrorText = "Forbidden"
                    };
                }

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