using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SmileUnderwriteBranch.Helper;

namespace SmileUnderwriteBranch.Helper
{
    public class ExcelManager
    {
        public ExcelManager()
        {
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

        public static DataTable ToDataTable<T>(List<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            var Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in Props)
            {
                //Defining type of data column gives proper data table
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                if (type != null) dataTable.Columns.Add(prop.Name, type);
            }
            foreach (var item in items)
            {
                var values = new object[Props.Length];
                for (var i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}