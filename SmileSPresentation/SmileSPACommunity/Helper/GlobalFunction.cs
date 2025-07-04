using Newtonsoft.Json;
using RestSharp;
using SmileSPACommunity.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web;

namespace SmileSPACommunity.Helper
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

        public static LoginDetail GetLoginDetail(HttpContextBase actionContext)
        {
            var result = Authorization.GetLoginDetail(actionContext);
            return result;
        }

        public static ResponseResult CheckSmilePDPAExist(string ApplicationID)
        {
            string url = string.Format(Properties.Settings.Default.PDPAURL + "Home/smilepdpaexist?applicationcode={0}", ApplicationID);
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            var queryResult = client.Execute(request);
            var result = JsonConvert.DeserializeObject<ResponseResult>(queryResult.Content);

            return result;
        }

        public static List<PDPAResult> GetSmilePDPA(string ApplicationID)
        {
            string url = string.Format(Properties.Settings.Default.PDPAURL + "Home/smilepdpadata?applicationcode={0}", ApplicationID);
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            var queryResult = client.Execute(request);
            var result = JsonConvert.DeserializeObject<List<PDPAResult>>(queryResult.Content);

            return result;
        }

    }
}