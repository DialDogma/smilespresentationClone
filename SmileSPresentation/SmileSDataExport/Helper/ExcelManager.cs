using SmileSDataExport.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace SmileSDataCenterV2.Helper
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
        /// <param name="actionContext">ส่ง this.HttpContext</param>
        /// <param name="lst">list to export</param>
        /// <param name="SheetName">Sheet name</param>
        /// <param name="FileName">File Name</param>
        public static void ExportToExcel<T>(HttpContextBase actionContext, List<T> lst, string SheetName, string FileName)
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
        /// <param name="actionContext"></param>
        /// <param name="fileName"></param>
        /// <param name="lst1"></param>
        /// <param name="sheetName1"></param>
        /// <param name="lst2"></param>
        /// <param name="sheetName2"></param>
        /// <param name="lst3"></param>
        /// <param name="sheetName3"></param>
        /// <param name="lst4"></param>
        /// <param name="sheetName4"></param>
        public static void ExportToExcel(HttpContextBase actionContext,
                                                                        string fileName,
                                                                        DataTable dt1, string sheetName1 = "Sheet1",
                                                                        DataTable dt2 = null, string sheetName2 = "Sheet2",
                                                                        DataTable dt3 = null, string sheetName3 = "Sheet3",
                                                                        DataTable dt4 = null, string sheetName4 = "Sheet4")
        {
            //Convert List To Datatable
            try
            {
                var wb = new ClosedXML.Excel.XLWorkbook();
                //var dt1 = GlobalFunction.ToDataTable(lst1);
                wb.AddWorksheet(dt1, sheetName1);

                if (dt2 != null)
                {
                    //var dt2 = GlobalFunction.ToDataTable(lst2);
                    wb.AddWorksheet(dt2, sheetName2);
                }

                if (dt3 != null)
                {
                    // var dt3 = GlobalFunction.ToDataTable(lst3);
                    wb.AddWorksheet(dt3, sheetName3);
                }

                if (dt4 != null)
                {
                    //var dt4 = GlobalFunction.ToDataTable(lst4);
                    wb.AddWorksheet(dt4, sheetName4);
                }
                actionContext.Response.Clear();
                actionContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                actionContext.Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xlsx");

                using (MemoryStream tmpMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(tmpMemoryStream);
                    tmpMemoryStream.WriteTo(actionContext.Response.OutputStream);
                    tmpMemoryStream.Close();
                }
                actionContext.Response.End();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

    }
}