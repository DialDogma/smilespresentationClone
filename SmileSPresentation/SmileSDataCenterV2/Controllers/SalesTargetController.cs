using SmileSDataCenterV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using System.Data;
using System.IO;
using SmileSDataCenterV2.Helper;
using System.Data.Entity.Core.Objects;
using System.Text.RegularExpressions;
using System.Globalization;

namespace SmileSDataCenterV2.Controllers
{
    public class SalesTargetController : Controller
    {
        // GET: Academy
        private DataStagingWHEntities _db;


        public SalesTargetController()
        {
            _db = new DataStagingWHEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }


        // GET: SalesTarget
        public ActionResult Index()
        {
            return View();
        }

        [Authorization(Roles = "Developer,DataCenter_SaleTarget_Supervisor")]
        public ActionResult SalesTargetManagement()
        {
            ViewBag.ProductType = _db.usp_ProductType_Select(null,0,9999,null,null,null).ToList();

            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.userID = userID;

            return View();
        }

        [Authorization(Roles = "Developer,DataCenter_SaleTarget_Operation,DataCenter_SaleTarget_Supervisor")]
        public ActionResult SalesTargetReport()
        {
            ViewBag.ProductType = _db.usp_ProductType_Select(null, 0, 9999, null, null, null).ToList();
            ViewBag.Branch = _db.usp_Branch_Select(null, 0, 9999, null, null, null).Where(x=>x.BranchId!=1).ToList();

            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            ViewBag.userID = userID;

            return View();
        }

        public ActionResult SearchTargetDetail()
        {
            try
            {
                ViewBag.ProductType = _db.usp_ProductType_Select(null, 0, 9999, null, null, null).ToList();
                ViewBag.Branch = _db.usp_Branch_Select(null, 0, 9999, null, null, null).ToList();

                var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                ViewBag.userID = userID;
                int productTypeId = 1;
                int branchId = -1;
                var period = new DateTime(2021, 10, 01).Date;

                var data = _db.usp_TargetDetail_Select(period, productTypeId, branchId, 0,9999,null,null,null).ToList();
                ViewBag.TargetDetail = data;

                return View("SalesTargetReport");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        [HttpPost]
        public ActionResult UploadTemplateFile(HttpPostedFileBase file , string dcr, int? productTypeId)
        {
            DateTime d_applyDate;
            d_applyDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dcr));
            var lstArr = new string[3]; //Array of result
            ObjectParameter TempCode = new ObjectParameter("Result", typeof(string)); 
            try
            {
                // 1 Create DataTable .
                DataTable dt = new DataTable();
                //Checking file content length and Extension must be .xlsx  
                if (file != null && file.ContentLength > 0 && System.IO.Path.GetExtension(file.FileName).ToLower() == ".xlsx")
                {
                    // 2. Generate Code.
                    _db.usp_GenerateCode("Tmp", 4, TempCode);

                    string path = Path.Combine(Server.MapPath("~/TempExcel"), Path.GetFileName(file.FileName));
                    //Saving the file  
                    file.SaveAs(path);
                    //Started reading the Excel file.  
                    using (XLWorkbook workbook = new XLWorkbook(path))
                    {
                        IXLWorksheet worksheet = workbook.Worksheet(1);
                        bool FirstRow = true;
                        //Range for reading the cells based on the last cell used.  
                        string readRange = "1:1";
                        foreach (IXLRow row in worksheet.RowsUsed())
                        {
                            //If Reading the First Row (used) then add them as column name  
                            if (FirstRow)
                            {
                                //Checking the Last cellused for column generation in datatable  
                                readRange = string.Format("{0}:{1}", 1, row.LastCellUsed().Address.ColumnNumber);
                                foreach (IXLCell cell in row.Cells(readRange))
                                {
                                    dt.Columns.Add(cell.Value.ToString());
                                }
                                FirstRow = false;
                            }
                            else
                            {
                                //Adding a Row in datatable  
                                dt.Rows.Add();
                                int cellIndex = 0;
                                string[] cellData = new string[3];
                                //Updating the values of datatable  
                                foreach (IXLCell cell in row.Cells(readRange))
                                {
                                    dt.Rows[dt.Rows.Count - 1][cellIndex] = cell.Value.ToString();
                                    cellData[cellIndex] = cell.Value.ToString();
                                    cellIndex++;
                                }
                              
                         
                                Regex regex = new Regex(@"^\d+$");
                                //สาขา
                                int? branchId = null;
                                if (regex.IsMatch(cellData[0]))// check isNumeric
                                {
                                    branchId = int.Parse(cellData[0]);
                                }

                                //เป้าสาขา
                                //int? targetValue = cellData[2] != "" ? int.Parse(cellData[2]) : 0;
                                int? targetValue = null;
                                string excelValue = null;
                                if (cellData[2] != "")
                                {
                                    excelValue = cellData[2];
                                    int num;
                                    if (int.TryParse(cellData[2], NumberStyles.AllowThousands,
                                                     CultureInfo.InvariantCulture, out num))
                                    {
                                        targetValue = num;
                                    }
                                }
                                // 2. insert into tempTarget
                                _db.usp_TmpImportTargetDetail_Insert(TempCode.Value.ToString(), d_applyDate, productTypeId, branchId, targetValue, excelValue);
                            }
                        }

                        //If no data in Excel file  
                        if (FirstRow)
                        {
                            //ViewBag.Message = "Empty Excel File!";
                            lstArr[0] = "False";
                            lstArr[1] = "";
                            lstArr[2] = "ไม่มีข้อมูลในไฟล์";
                            return Json(lstArr, JsonRequestBehavior.AllowGet);
                        }

                    }
                }
                else
                {
                    //If file extension of the uploaded file is different then .xlsx  
                    //ViewBag.Message = "Please select file with .xlsx extension!";
                    lstArr[0] = "False";
                    lstArr[1] = "";
                    lstArr[2] = "กรุณาเลือกไฟล์นามสกุล .xlsx";

                    return Json(lstArr, JsonRequestBehavior.AllowGet);
                }

                //var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                //ViewBag.userID = userID;
                //ViewBag.ProductType = _db.usp_ProductType_Select(null, 0, 9999, null, null, null).ToList();
                //ViewBag.TempCode = TempCode.Value.ToString();
                //ViewBag.Message = "Upload Successfull!!!";

                //return View("SalesTargetManagement"); //retJsonResult;

                //Validate
                var resultValidate = _db.usp_TmpImportTargetDetail_Validate(TempCode.Value.ToString()).Single();
                if (resultValidate.IsResult == true) //ตรวจสอบแล้ว
                {
                    if (resultValidate.Result == "1")//ไม่มีหมายเหตุ
                    {
                        //ตรวจสอบเรียบร้อย และไม่มีหมายเหตุ
                        lstArr[0] = "True";
                    }
                    else//มีหมายเหตุ
                    {
                        //ตรวจสอบเรียบร้อย และมีหมายเหตุ
                        lstArr[0] = "False";
                    }
                    lstArr[1] = TempCode.Value.ToString();
                    lstArr[2] = "";
                }
                else //เกิดข้อผิดพลาดขณะตรวจสอบ
                {
                    lstArr[0] = "False";
                    lstArr[1] = "";
                    lstArr[2] = resultValidate.Msg.ToString();
                }


                
                return Json(lstArr, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                //return View("SalesTargetManagement");
                lstArr[0] = "False";
                lstArr[1] = "";
                lstArr[2] = "ขั้นตอนการตรวจสอบมีปัญหา กรุณาอัปโหลดอีกครั้ง";
                return Json(lstArr, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPreviewTemplateFile(string tempCode, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            try
            {
                // 1. Previews
                var result = _db.usp_TmpImportTargetDetail_Preview(tempCode, pageStart, pageSize, sortField, orderType, search).ToList();

                var dt = new Dictionary<string, object>
                {
                    {"draw", draw },
                    {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                    {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                    {"data", result.ToList()}
                };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
            catch 
            {
                var dt = new Dictionary<string, object>
                {
                    {"draw", draw },
                    {"recordsTotal", 0},
                    {"recordsFiltered", 0},
                    {"data", null}
                };
                return Json(dt, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ExportTemplateFile() 
        {
            try
            {
                DataTable dt = getDataTemplate();
                //Name of File  
                string fileName = "Template.xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    //Add DataTable in worksheet  
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        //Return xlsx Excel File  
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private DataTable getDataTemplate()
        {
            try
            {
                //Get Data From Store Procedure.
                var templateSelect = _db.usp_TemplateTarget_Select(0, 9999, null, null, null).ToList();

                //Creating DataTable.  
                DataTable dt = new DataTable();

                //Setiing Table Name  
                dt.TableName = "TemplateData";

                //Add Columns  
                dt.Columns.Add("BranchID", typeof(string));
                dt.Columns.Add("ชื่อสาขา", typeof(string));
                dt.Columns.Add("เป้าสาขา", typeof(string));

                //Add Rows in DataTable  
                foreach (var item in templateSelect)
                {
                    dt.Rows.Add(item.BranchId, item.ชื่อสาขา, item.เป้าสาขา);
                }

                dt.AcceptChanges();
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }


        //Validate data
        public ActionResult FindTargetDetailData(string dcr, int? productTypeId, int? branchId)
        {
            var lstArr = new string[3]; //Array of result
            DateTime d_applyDate;
            d_applyDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dcr));

            var result = _db.usp_TargetDetail_Select(d_applyDate, productTypeId, branchId, 0, 99999, "BranchId", "ASC", null).ToList();

            if (result.Count == 0)
            {
                lstArr[0] = "False";
                lstArr[1] = "";
                lstArr[2] = "ไม่พบข้อมูล";
            }
            else
            {
                lstArr[0] = "True";
                lstArr[1] = "";
                lstArr[2] = "พบ "+ result.Count.ToString() + "รายการ";
            }
            
            return Json(lstArr, JsonRequestBehavior.AllowGet);
        }

       //Get Data
        public JsonResult GetTargetDetail(string dcr, int? productTypeId, int? branchId, int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null)
        {
            try
            {
                DateTime d_applyDate;
                d_applyDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dcr));
                // 1. Previews
                var result = _db.usp_TargetDetail_Select(d_applyDate, productTypeId, branchId, pageStart, pageSize, sortField, orderType, search).ToList();
                var dt = new Dictionary<string, object>
                {
                    {"draw", draw },
                    {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                    {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                    {"data", result.ToList()}
                };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dt = new Dictionary<string, object>
                {
                    {"draw", draw },
                    {"recordsTotal", 0},
                    {"recordsFiltered", 0},
                    {"data", null}
                };
                return Json(dt, JsonRequestBehavior.AllowGet);
            }
        }

        //Export Report
        public void ExportReportFile(string dcr, int? productTypeId = null, int? branchId = null)
        {
            DateTime d_applyDate;
            d_applyDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(dcr));
            try
            {
                var resultTargetDetail = _db.usp_TargetDetail_Select(d_applyDate, productTypeId, branchId, 0, 99999, "BranchDetail", "ASC", null).ToList();
                if (resultTargetDetail.Count > 0)//ไม่พบข้อมูล
                {
                    var result = resultTargetDetail.Select(s => new
                                                                   {
                                                                       DCRรายงานเป้าสาขา = s.DCRPriod,
                                                                       BranchID = s.BranchId,
                                                                       ชื่อสาขา = s.BranchDetail,
                                                                       ผลิตภัณฑ์ = s.ProductTypeDetail,
                                                                       เป้าสาขา = s.Value
                                                                   }).ToList();
                    var dt = GlobalFunction.ToDataTable(result);
                    ExcelManager.ExportToExcel(this.HttpContext, "รายงานเป้าสาขา", dt);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return null;
            }
        }

        //Save 
        public ActionResult SaveTargetManagement(string tempCode)
        {
            var lstArr = new string[3];
            var userID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            
            try
            {
                var result = _db.usp_TargetDetail_Insert(tempCode, userID).Single();

                lstArr[0] = result.IsResult.Value.ToString();
                lstArr[1] = result.Result.ToString();
                lstArr[2] = result.Msg.ToString();

                return Json(lstArr, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                lstArr[0] = "False";
                lstArr[1] = "";
                lstArr[2] = "เกิดข้อผิดพลาด กรุณาบันทึกอีกครั้ง";

                return Json(lstArr, JsonRequestBehavior.AllowGet);
            }
        }

        
    }
}