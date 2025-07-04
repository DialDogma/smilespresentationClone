using System;
using System.Collections.Generic;
using System.Linq;
using SmileSSurvey.Models;
using SmileSSurvey.Helper;
using System.Text;
using System.Web.Mvc;
using System.Web.Http;
using RouteAttribute = System.Web.Mvc.RouteAttribute;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Web.Http.Cors;
using System.Globalization;

namespace SmileSSurvey.Controllers

{
   [AllowCrossSite]
    public class ReportFileSurveyController : Controller 
    {
        private readonly SurveyEntities _context;
        public ReportFileSurveyController() => _context = new SurveyEntities();

        
     
        public ActionResult SmileCRMReportExcel(string dateFrom, string dateTo, int? branchId = null, string serviceTypeName = null, int? createdByEmployeeId = null, int? indexStart = 0, int? pageSize = 10, string sortField = null, string orderType = null, string searchDetail = null , string rptemplate=null)
        {


            //dateFrom= "04-10-2566 "   dateTo= "04-10-2566 "  branchId =null  serviceTypeName=null indexStart=1   pageSize=10  sortField=null   orderType="false" searchDetail=null
            //indexStart = 1;
            //pageSize = 10;
            //sortField = null;
            try
            {
                if (serviceTypeName == "null") serviceTypeName = null;
                //var a = Convert.ToDateTime(period);
                //var b = a.ToString("dd/MM/yyyy");
                //var period_c = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(b, null));

                DateTime? c_dateFrom = null;
                DateTime? c_dateTo = null;
                if (dateFrom != null && dateFrom != "")
                {
                    c_dateFrom = GlobalFunction.ConvertDatePickerToDate_BE(dateFrom);
                }
                if (dateTo != null && dateTo != "")
                {
                    c_dateTo = GlobalFunction.ConvertDatePickerToDate_BE(dateTo);
                }
     



                pageSize = 999999;
               var result = _context.usp_SmileCRMSurveyResult_Select(c_dateFrom, c_dateTo, branchId, serviceTypeName, createdByEmployeeId, indexStart, pageSize, sortField, orderType, searchDetail).ToList();

                // var result = _context.usp_QueueMotorAuditChairmanCloseReport_Select(a).ToList();

                // data empty
                if (result.Count == 0)
                {
                    return HttpNotFound();

                }




                var stream = new MemoryStream();
                if (rptemplate == "admin")
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets.Add("Detail");
                        {


                            var dataExcelAdmin = _context.usp_SmileCRMSurveyResult_Select(c_dateFrom, c_dateTo, branchId, serviceTypeName, createdByEmployeeId, indexStart, pageSize, sortField, orderType, searchDetail).Select(o => new
                            {
                                วันที่ส่ง_SMS = (o.CreatedDate != null ? o.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("th-TH")) : ""),

                                สาขา = (o.BranchName),
                                รหัสผู้ให้บริการ = (o.EmployeeCode),
                                ชื่อสกุล_ผู้ให้บริการ = (o.PersonName),
                                ตำแหน่ง = (o.EmployeePosition),
                                วันที่ประเมินความพึงพอใจ = (o.SubmitDate != null ? o.SubmitDate.Value.ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("th-TH")): ""),

                                เบอร์โทรศัพท์ลูกค้า = (o.PhoneNo),
                                ประเภทการให้บริการ = (o.ServiceTypeName),
                                รายละเอียดประเภทอื่นๆ = (o.OtherServiceTypeName),
                                หมายเหตุจากเจ้าหน้าที่ = (o.SmileCRMRemark),
                                ข้อที่1_บริการด้วยรอยยิ้ม_และสุภาพ = (o.AnswerDetail1),
                                ข้อที่2_บริการสะดวก_รวดเร็ว = (o.AnswerDetail2),
                                ข้อที่3_ได้รับข้อมูลถูกต้อง_ครบถ้วน = (o.AnswerDetail3),
                                ผลรวม = (o.AnswerDetailTotal),
                                ข้อเสนอแนะเพิ่มเติม = (o.AnswerMore)
                            }).ToList();
                            workSheet.Cells.LoadFromCollection(dataExcelAdmin, true);
                           
                            
                        }
                        var rowCount = workSheet.Dimension.Rows;
                        var colCount = workSheet.Dimension.Columns;

                        // Select only the header cells
                        var headerCells = workSheet.Cells[1, 1, 1, workSheet.Dimension.Columns];

                        // Set their text to bold.
                        headerCells.Style.Font.Bold = true;
                        // Set their background color
                        headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        headerCells.Style.Fill.BackgroundColor.SetColor(Color.Orchid);

                        // Apply the auto-filter to all the columns
                        var allCells = workSheet.Cells[workSheet.Dimension.Address];
                        allCells.AutoFilter = true;
                        // Auto-fit all the columns
                        allCells.AutoFitColumns();






                        package.Save();

                        stream.Position = 0;
                        //get new GUID
                        var dataSessionKey = Guid.NewGuid().ToString();
                        //tempData GUID

                        TempData["keyReport"] = dataSessionKey;
                        //Session Data
                        Session[dataSessionKey] = package.GetAsByteArray();
                        byte[] data = Session[dataSessionKey] as byte[];

                        string excelName = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                        return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);

                        //return Json(dt, GlobalObject.carmelSetting());
                        //return Json("", JsonRequestBehavior.AllowGet);
                    }
                }
                else 

                    using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Detail");
                    {
                        
                       
                     
                             var excelDataBranch = _context.usp_SmileCRMSurveyResult_Select(c_dateFrom, c_dateTo, branchId, serviceTypeName, createdByEmployeeId, indexStart, pageSize, sortField, orderType, searchDetail).Select(o => new
                            {
                                วันที่ส่ง_SMS = (o.CreatedDate != null ? o.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("th-TH")) : ""),
                                สาขา = (o.BranchName),
                                รหัสผู้ให้บริการ = (o.EmployeeCode),
                                ชื่อสกุล_ผู้ให้บริการ = (o.PersonName),
                                ตำแหน่ง = (o.EmployeePosition),
                                 วันที่ประเมินความพึงพอใจ = (o.SubmitDate != null ? o.SubmitDate.Value.ToString("dd/MM/yyyy HH:mm:ss" ,new CultureInfo("th-TH")) : ""),
                                 ประเภทการให้บริการ = (o.ServiceTypeName),
                                รายละเอียดประเภทอื่นๆ = (o.OtherServiceTypeName),
                                หมายเหตุจากเจ้าหน้าที่ = (o.SmileCRMRemark),
                                 ข้อที่1_บริการด้วยรอยยิ้ม_และสุภาพ = (o.AnswerDetail1),
                                 ข้อที่2_บริการสะดวก_รวดเร็ว = (o.AnswerDetail2),
                                 ข้อที่3_ได้รับข้อมูลถูกต้อง_ครบถ้วน= (o.AnswerDetail3),
                                ผลรวม = (o.AnswerDetailTotal),
                                ข้อเสนอแนะเพิ่มเติม = (o.AnswerMore)
                            }).ToList();
                            workSheet.Cells.LoadFromCollection(excelDataBranch, true);

                     
                    }
                    var rowCount = workSheet.Dimension.Rows;
                    var colCount = workSheet.Dimension.Columns;

                    // Select only the header cells
                    var headerCells = workSheet.Cells[1, 1, 1, workSheet.Dimension.Columns];

                    // Set their text to bold.
                    headerCells.Style.Font.Bold = true;
                    // Set their background color
                    headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(Color.Orchid);

                    // Apply the auto-filter to all the columns
                    var allCells = workSheet.Cells[workSheet.Dimension.Address];
                    allCells.AutoFilter = true;
                    // Auto-fit all the columns
                    allCells.AutoFitColumns();

                   




                    package.Save();

                    stream.Position = 0;
                    //get new GUID
                    var dataSessionKey = Guid.NewGuid().ToString();
                    //tempData GUID

                    TempData["keyReport"] = dataSessionKey;
                    //Session Data
                    Session[dataSessionKey] = package.GetAsByteArray();
                    byte[] data = Session[dataSessionKey] as byte[];
                   
                    string excelName = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
                                            
                    return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);

                    //return Json(dt, GlobalObject.carmelSetting());
                    //return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}