using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using StandardDevelopment.Models;

namespace StandardDevelopment.Controllers
{
    public class DatatablesController:Controller
    {
        #region Context

        private readonly StandardDevelopmentEntities _context;

        public DatatablesController()

        {
            _context = new StandardDevelopmentEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Context

        // GET: Datatables
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DatatablesBasic()
        {
            return View();
        }

        public ActionResult DatatablesRenderRowHilight()
        {
            return View();
        }

        public ActionResult DatatablesAddEdit_HilightRowSelected()
        {
            return View();
        }

        public ActionResult DatatablesCheckBoxList()
        {
            return View();
        }

        public ActionResult DatatablesExportExcel()
        {
            return View();
        }

        public ActionResult Excel()
        {
            return View();
        }

        public void ExportExcel()
        {
            var result = _context.usp_Person_Select(null,null,999999,null,null).ToList();
            ExportToExcel(HttpContext,result,"sheet1","testdata");
            //return RedirectToAction("DatatablesExportExcel");
        }

        #region "Method"

        [HttpPost]
        public JsonResult GetPersonDetail(int? draw = null,int? indexStart = null,int? pageSize = null,
           string sortField = null,string orderType = null,string search = null)
        {
            //get result from db
            var result = _context.usp_Person_Select(search,indexStart,pageSize,sortField,orderType).ToList();

            //result[0].IsAllowDelete
            //put result to datatable
            var dt = new Dictionary<string,object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            return Json(dt,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPersonDetailAll(int? draw = null,int? indexStart = null,int? pageSize = null,
            string sortField = null,string orderType = null,string search = null)
        {
            //get result from db
            var result = _context.usp_Person_Select(search,indexStart,pageSize,sortField,orderType).ToList();

            string[] arrayList = new string[result.Count];
            int i = 0;
            foreach(var List in result)
            {
                arrayList[i] = (List.Person_Id.ToString());
                i++;
            }
            return Json(arrayList,JsonRequestBehavior.AllowGet);
        }

        public JsonResult Assign(int[] ListNo = null)
        {
            //get result from db

            return Json(ListNo,JsonRequestBehavior.AllowGet);
        }

        public static void ExportToExcel<T>(HttpContextBase actionContext,List<T> lst,string SheetName,string FileName)
        {
            //Convert List To Datatable
            var dt = ToDataTable(lst);

            var wb = new ClosedXML.Excel.XLWorkbook();
            wb.AddWorksheet(dt,SheetName);

            actionContext.Response.Clear();
            actionContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            actionContext.Response.AddHeader("content-disposition","attachment;filename=" + FileName + ".xlsx");

            using(MemoryStream tmpMemoryStream = new MemoryStream())
            {
                wb.SaveAs(tmpMemoryStream);
                tmpMemoryStream.WriteTo(actionContext.Response.OutputStream);
                tmpMemoryStream.Close();
            }
            actionContext.Response.End();
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach(PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name,type);
            }
            foreach(T item in items)
            {
                var values = new object[Props.Length];
                for(int i = 0;i < Props.Length;i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item,null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        #endregion "Method"
    }
}