﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web;

namespace SmileSLogin.Helper
{
    public class GlobalFunction
    {
        public static Token GetLoginDetail(HttpContextBase actionContext)
        {
            var t = new TokenManager();

            return t.GetToken(actionContext);
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
    }
}