using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SmileSBankDirectDebit.Helper;
using SmileSBankDirectDebit.Models;
using SmileSBankDirectDebit.SmileEmailService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SmileSBankDirectDebit.Controllers
{
    [Helper.Authorization]
    public class BankDirectDebitController : Controller
    {
        #region Context

        private readonly BankDirectDebitDBContext _context;
        private static readonly CultureInfo _dtEn = new CultureInfo("en-US");

        public BankDirectDebitController()

        {
            _context = new BankDirectDebitDBContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Context

        #region Search

        [HttpGet]
        public ActionResult Search()
        {
            ViewBag.BankId = _context.usp_Bank_Select(null, null).Where(x => x.BankId != 2).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Search(FormCollection form)
        {
            return View();
        }

        public JsonResult GetDDebitMonitor(int? bankId, string AccountNo = null, string AccountName = null, int? draw = null, int? indexStart = null, int? pageSize = null,
            string sortField = null, string orderType = null, string search = null)
        {
            //var result = _context.usp_UnderWriteHistoryBySchoolId_Select(schoolId,
            //    indexStart, pageSize, sortField, orderType, search).ToList();
            var result = _context.usp_BankDirectDebitMonitor_Select_V2(bankId, AccountNo, AccountName, indexStart, pageSize, sortField, orderType, search).ToList();
            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDDebitDetail(string AccountNo = null, string bankId = null, int? draw = null, int? indexStart = null, int? pageSize = null,
            string sortField = null, string orderType = null, string search = null)
        {
            //var result = _context.usp_UnderWriteHistoryBySchoolId_Select(schoolId,
            //    indexStart, pageSize, sortField, orderType, search).ToList();
            if (bankId == "")
            {
                bankId = null;
            }

            var result = _context.usp_ApplicationDetailfromDataCenter_Select(bankOrganizeCode: bankId, bankOrganizeId: null, accountNo: AccountNo, indexStart: indexStart, pageSize: pageSize, sortField: sortField, orderType: orderType, searchDetail: search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBankDirectDeibitDatailByAccountNo(string AccountNo, int BankId)
        {
            var result = _context.usp_BankDirectDebitMonitor_Select_V2(BankId, AccountNo, null, 0, 1, null, null, null).SingleOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBankConsentDatailByAccountNo(int BankDirectDebitId, int? draw = null, int? indexStart = null, int? pageSize = null,
            string sortField = null, string orderType = null, string search = null)
        {
            // Start with the base query
            var query = _context.usp_BankDirectDebitLog_Select(bankId: null, accountNo: null, bankDirectDebitId: BankDirectDebitId, indexStart: null, pageSize: null, sortField: null, orderType: null);

            // Apply sorting if the sortField parameter is provided
            //query = query.OrderByDescending(x => x.CreatedDate);

            // Execute the query and retrieve the sorted data
            var result = query.ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count()},
                {"recordsFiltered", result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        #endregion Search

        #region Print

        public ActionResult Deliverynote(int delivernoteID)
        {
            ViewBag.BankName = "ธนาคารกรุงไทยอาคารถนนศรีอยุธยา";
            ViewBag.BankAddress = "513 อาคารสาขาถนนศรีอยุธยา ชั้น 6 (งานหนังสือยินยอม) ถนนศรีอยุธยา แขวงถนนพญาไท เขตราชเทวี กรุงเทพฯ 10400";
            ViewBag.BankTel = "02-203-2024-2025";
            ViewBag.DeliveryDate = DateTime.Now.ToString("dd/MM/yyyy");
            //ViewBag.Branch = "สำนักงานใหญ่";
            //ViewBag.CountAccount = "8";
            ViewBag.DeliverNoteID = Convert.ToString(delivernoteID);

            var result = _context.usp_DataCenterV1Address_Select(delivernoteID).Single();

            ViewBag.Company = result.Company;
            ViewBag.Address = result.AddressFullDetail;
            ViewBag.Tel = result.PhoneNo;

            _context.usp_BankDirectDebitPrintLog_Insert(delivernoteID, GlobalFunction.GetLoginDetail(HttpContext).User_ID);

            return View();
        }

        public ActionResult AccountDetailGroup(int BankDirectDebitID)
        {
            ViewBag.BankDirectDebitCode = Convert.ToString(BankDirectDebitID);
            ViewBag.AccountCount = _context.usp_BankDirectDebitDetailV2_Select(BankDirectDebitID).Count();

            return View();
        }

        //public ActionResult DeliverynoteAddress()
        //{
        //    return View();
        //}

        public ActionResult GetDeliveryNote(int delivernoteID)
        {
            var result = _context.usp_BankDirectDebitDetailV2_Select(delivernoteID).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Print

        #region ImportData

        [HttpGet]
        [Helper.Authorization(Roles = "Developer,BankDirectDebit_Premium")]
        public ActionResult ImportData()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportData(FormCollection form, HttpPostedFileBase uploadFile)
        {
            //declare
            int totalRecords, changeRecords, deleteRecords;
            //_context = new BankDirectDebitDBContext();
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var backupPath = GlobalFunction.CreateBackupFile(uploadFile);
            //get result from file upload
            var mainResult = GetFileImport(uploadFile);
            var headerResult = mainResult.Item1;
            var detailResult = mainResult.Item2;
            var footerResult = mainResult.Item3;
            //convert to datetime
            DateTime dateInput = DateTime.ParseExact(headerResult[0].ProcessDate, "ddMMyy", _dtEn);
            //check if not null
            if (footerResult[0].TotalRecords != "")
            {
                totalRecords = Convert.ToInt32(footerResult[0].TotalRecords);
            }
            else
            {
                totalRecords = 0;
            }
            if (footerResult[0].ChangeRecords != "")
            {
                changeRecords = Convert.ToInt32(footerResult[0].ChangeRecords);
            }
            else
            {
                changeRecords = 0;
            }
            if (footerResult[0].DeleteRecords != "")
            {
                deleteRecords = Convert.ToInt32(footerResult[0].DeleteRecords);
            }
            else
            {
                deleteRecords = 0;
            }
            try
            {
                //save to entity
                var resultHeaderId = _context.usp_DDRStandardFormatHeader_Insert(backupPath,
                    Convert.ToInt32(footerResult[0].TotalRows), headerResult[0].HeaderText, footerResult[0].FooterText,
                    headerResult[0].FirstSequenceNo, headerResult[0].BankCompanyCode, headerResult[0].CompanyName,
                    dateInput, headerResult[0].ProcessDate, headerResult[0].Blank, footerResult[0].TotalRows, totalRecords,
                    changeRecords, deleteRecords, footerResult[0].Blank, true, user).FirstOrDefault();

                //get header id
                int? headerId = resultHeaderId.DDRStandardFormatHeaderId;
                var detail = new List<DDRStandardFormatDetail>();
                //loop detail to model
                foreach (var itm in detailResult)
                {
                    DateTime dateOutput = DateTime.ParseExact(itm.ApplyDate, "dd/MM/yyyy", new CultureInfo("en-Us"));
                    detail.Add(new DDRStandardFormatDetail
                    {
                        DDRStandardFormatHeaderId = headerId,
                        DetailText = itm.DetailText,
                        SequenceNo = itm.RunningNumber,
                        BankCode = itm.BankCode,
                        Action = itm.Action,
                        ApplyDate = dateOutput,
                        ApplyDateText = itm.ApplyDateText,
                        Ref1 = itm.Ref1,
                        Ref2 = itm.Ref2,
                        Register = itm.Register,
                        BranchCode = itm.BranchCode,
                        AccountNo = itm.AccountNo,
                        AccountName = itm.AccountName,
                        StatusCode = Convert.ToInt32(itm.Status),
                        Blank = itm.Blank,
                        CreateByUserId = user,
                        CreateDate = DateTime.Now,
                        UpdateByUserId = user,
                        UpdateDate = DateTime.Now,
                        IsActive = true
                    });
                }
                //save list model to entity
                _context.DDRStandardFormatDetails.AddRange(detail);
                _context.SaveChanges();

                //update direct debit
                _context.usp_BankDirectDebit_Update(headerId);
                _context.usp_BankDirectDebitLog_Insert(groupId: headerId, bankDirectDebitLogType: 6, createdByUserId: user);

                TempData["Success"] = "อัพโหลดไฟล์สำเร็จ";

                return View();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
                //return RedirectToAction("InternalServerError", "Error", new { exception = "ไม่สามารถอัพโหลดข้อมูลได้!! " + e });
            }
        }

        [HttpPost]
        public ActionResult ImportCheckApproved(HttpPostedFileBase file)
        {
            try
            {
                var user = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

                if (file == null)
                {
                    return Json(new { IsResult = false, Msg = "กรุณาเลือกไฟล์ก่อนอัพโหลด" }, JsonRequestBehavior.AllowGet);
                }

                if (file.ContentLength <= 0)
                {
                    return Json(new { IsResult = false, Msg = "กรุณาเลือกไฟล์ก่อนอัพโหลด" }, JsonRequestBehavior.AllowGet);
                }

                if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return Json(new { IsResult = false, Msg = "ประเภทของไฟล์ไม่ถูกต้อง" }, JsonRequestBehavior.AllowGet);
                }

                var excelList = new List<DocImport>();

                var fileDoc = file.FileName;

                Stream fileContent = file.InputStream;
                int? column = null;
                int? row = null;

                using (ExcelPackage excelPackage = new ExcelPackage(fileContent))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.First();
                    var rowCount = worksheet.Dimension.Rows;
                    var colCount = worksheet.Dimension.Columns;
                    try
                    {
                        // check formath accNo
                        for (int i = 2; i <= rowCount; i++)
                        {
                            row = i;
                            for (int j = 1; j <= colCount; j++)
                            {
                                column = j;
                                if (!Regex.IsMatch(worksheet.Cells[i, j].Value.ToString().Replace("-", "").Trim(), "^[0-9]*$"))
                                {
                                    var columnErrName = (worksheet.Cells[1, column.Value].Value != null) ? worksheet.Cells[1, column.Value].Value.ToString() : $"Column Name : {column.Value}";
                                    return Json(new { IsResult = false, Msg = $"กรุณาตรวจสอบ file : {fileDoc} Column : {columnErrName} Row : {row}" }, JsonRequestBehavior.AllowGet);
                                }
                            }

                            if (worksheet.Cells[i, 1].Value == null)
                            {
                                var columnErrName = (worksheet.Cells[1, 1].Value != null) ? worksheet.Cells[1, 1].Value.ToString() : $"Column Name : ";
                                return Json(new { IsResult = false, Msg = $"กรุณาตรวจสอบ file : {fileDoc} Column : {columnErrName} Row : {row} มี ค่า Blanks" }, JsonRequestBehavior.AllowGet);
                            }

                            excelList.Add(new DocImport
                            {
                                AccNo = worksheet.Cells[i, 1].Value.ToString().Replace("-", "").Trim(),
                            });
                        }
                    }
                    catch (Exception)
                    {
                        var columnErrName = (worksheet.Cells[1, column.Value].Value != null) ? worksheet.Cells[1, column.Value].Value.ToString() : $"Column Name = Null;Error Column :{column.Value}";
                        var MsgErr = $"Error Message : กรุณาตรวจสอบ file:{fileDoc} column:{columnErrName} row:{row}";

                        //return RedirectToAction("BadReques", "Error", new { ErrMsg = Msg });
                        return Json(new { IsResult = false, Msg = MsgErr }, JsonRequestBehavior.AllowGet);
                    }
                }

                ViewBag.excelData = excelList;

                try
                {
                    //clear data table tem
                    _context.usp_AccountValidateImport_Delete();

                    foreach (var item in excelList)
                    {
                        var accNo = item.AccNo;

                        var insertData = _context.usp_AccountValidateImport_Insert(accNo, user).FirstOrDefault();
                    }
                }
                catch (Exception e)
                {
                    return Json(new { IsResult = false, Msg = $"Error Message : {e.Message}", JsonRequestBehavior.AllowGet });
                }

                return Json(new { IsResult = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { IsResult = false, Msg = $"Error Message : {e.Message}", JsonRequestBehavior.AllowGet });
            }
        }

        public ActionResult Preview(int? draw,
                                        int? indexStart = 0,
                                        int? pageSize = null,
                                        string sortField = null,
                                        string orderType = null)
        {
            try
            {
                var result = _context.usp_AccountValidateImport_Select(indexStart, pageSize, sortField, orderType).ToList();

                var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", 0},
                {"recordsFiltered", 0},
                {"data", null},
                {"errorCountField", 0} ,
                {"errMsg",e.Message }
            };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ExportPreview()
        {
            var result = _context.usp_AccountValidateImport_Select(0, null, null, null).ToList();

            // data empty
            if (result.Count == 0)
            {
                return Json(new { IsResult = false, Msg = $"ไม่พบข้อมูลนำเข้า" }, JsonRequestBehavior.AllowGet);
            }

            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Detail");
                {
                    var Roadside = _context.usp_AccountValidateImport_Select(0, 9999, null, null).Select(g => new
                    {
                        AccountNo = g.BankAccountNo,
                        ชื้อสกุล = g.AccountName,
                        สถานะรับรองบัญชี = g.BankDirectDebitStatus
                    }).ToList();
                    workSheet.Cells.LoadFromCollection(Roadside, true);
                    //edit name headerCells
                    workSheet.Cells["A1"].Value = "Account No.";
                    workSheet.Cells["B1"].Value = "ชื้อ-สกุล";
                }
                var rowCount = workSheet.Dimension.Rows;
                var colCount = workSheet.Dimension.Columns;

                // Select only the header cells
                var headerCells = workSheet.Cells[1, 1, 1, workSheet.Dimension.Columns];

                // Set their text to bold.
                headerCells.Style.Font.Bold = true;
                // Set their background color
                headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerCells.Style.Fill.BackgroundColor.SetColor(Color.Yellow);

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
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ExportReport(string bankDirectDebitStatus, string startDate, string endDate, bool isCheck, int? branchId, bool byuser)
        {
            try
            {
                var user = GlobalFunction.GetLoginDetail(HttpContext);
                //check session expired
                if (user.UserName is null && user.User_ID == 0)
                {
                    return Json(new { IsResult = false, Msg = "Your session is expired", JsonRequestBehavior.AllowGet });
                }

                int? cBankDirectDebitStatus = Convert.ToInt16(bankDirectDebitStatus);
                int? cbranchId = Convert.ToInt16(branchId);
                DateTime? cStartDate = null;
                DateTime? cEndDate = null;

                switch (cBankDirectDebitStatus)
                {
                    //ทั้งหมด
                    case -1:
                    //รับรอง
                    case 1:
                        //Covert string startDate to DateTime
                        if (startDate != "")
                        {
                            cStartDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", new CultureInfo("en-Us"));
                            cEndDate = cStartDate;
                        }
                        else
                        {
                            cStartDate = null;
                            cEndDate = null;
                        }
                        break;
                    //ไม่รับรอง
                    case 2:
                        if (!isCheck)
                        {
                            //Covert string startDate to DateTime
                            if (startDate != "" && endDate != "")
                            {
                                cStartDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", new CultureInfo("en-Us"));
                                cEndDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", new CultureInfo("en-Us"));
                            }
                            else
                            {
                                cStartDate = null;
                                cEndDate = null;
                            }
                        }
                        break;
                    //อื่นๆ
                    default:
                        return Json(new { IsResult = false, Msg = $"BankDirectDebitStatus not found" }, JsonRequestBehavior.AllowGet);
                }

                MemoryStream stream = new MemoryStream();
                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    ExcelWorksheet workSheet = package.Workbook.Worksheets.Add("สรุป");
                    {
                        var result = _context.usp_BankDirectDebitAccountReportSummary_Select(bankDirectDebitStatusId: cBankDirectDebitStatus == -1 ? null : cBankDirectDebitStatus,
                                                                                             startCoverDateFrom: cStartDate,
                                                                                             startCoverDateTo: cEndDate,
                                                                                             branchId: cbranchId == -1 ? null : branchId,
                                                                                             employeeCode: byuser == true ? user.EmployeeCode : null).ToList();
                        if (result.Count == 0)
                            return Json(new { IsResult = false, Msg = $"ไม่พบข้อมูล" }, JsonRequestBehavior.AllowGet);

                        workSheet.Cells.LoadFromCollection(result, true);
                    }
                    // Select only the header cells
                    var headerCells = workSheet.Cells[1, 1, 1, workSheet.Dimension.Columns];

                    // Set their text to bold.
                    headerCells.Style.Font.Bold = true;
                    // Set their background color
                    headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(Color.Yellow);

                    // Apply the auto-filter to all the columns
                    var allCells = workSheet.Cells[workSheet.Dimension.Address];
                    allCells.AutoFilter = true;
                    // Auto-fit all the columns
                    allCells.AutoFitColumns();

                    ExcelWorksheet workSheet2 = package.Workbook.Worksheets.Add("Detail");
                    {
                        var result2 = _context.usp_BankDirectDebitAccountReport_Select(bankDirectDebitStatusId: cBankDirectDebitStatus == -1 ? null : cBankDirectDebitStatus,
                                                                                      startCoverDateFrom: cStartDate,
                                                                                      startCoverDateTo: cEndDate,
                                                                                      branchId: (cbranchId == -1 ? null : branchId),
                                                                                      employeeCode: (byuser == true ? user.EmployeeCode : null))
                                                                                      .Select(g => new
                                                                                      {
                                                                                          App_ID = g.App_id,
                                                                                          ประเภทเบี้ย = g.ProductTypeDetail,
                                                                                          แผน = g.Product,
                                                                                          BankName = g.BankName,
                                                                                          bankAccountNo = g.BankAccountNo,
                                                                                          AccountName = g.AccountName,
                                                                                          ชื่อผู้เอาประกัน = g.CustomerName,
                                                                                          BankDirectDebitStatus = g.BankDirectDebitStatusDisplay,
                                                                                          StatusDisplayCauseText = g.StatusDisplayCauseText,
                                                                                          สถานะการส่งเอกสาร = g.DeliverStatus,
                                                                                          วันที่ดำเนินการ = (g.CreatedDate != null ? g.CreatedDate.Value.AddYears(543).ToString("dd/MM/yyyy HH:mm", new CultureInfo("en-Us")) : ""),
                                                                                          BankAccountApplyDate = (g.BankAccountApplyDate != null ? g.BankAccountApplyDate.Value.AddYears(543).ToString("dd/MM/yyyy HH:mm", new CultureInfo("en-Us")) : ""),
                                                                                          StartCoverYear = g.StartCoverYear,
                                                                                          StartCoverMonth = g.StartCoverMonth,
                                                                                          PayerBranch = g.PayerBranch,
                                                                                          PayerStudyArea = g.PayerStudyArea,
                                                                                          PayerBuildingName = g.PayerBuildingName,
                                                                                          PayerTumbol = g.PayerTumbol,
                                                                                          PayerAmphoe = g.PayerAmphoe,
                                                                                          PayerProvince = g.PayerProvince,
                                                                                          PayerPhoneNumber = g.PayerPhoneNumber,
                                                                                          EmployeeCode = g.EmployeeCode,
                                                                                          EmployeeName = g.EmployeeName,
                                                                                          เลขที่ใบขอ = g.BankDirectDebitHeaderCode
                                                                                      }).ToList();
                        if (result2.Count == 0)
                            return Json(new { IsResult = false, Msg = $"ไม่พบข้อมูล" }, JsonRequestBehavior.AllowGet);

                        workSheet2.Cells.LoadFromCollection(result2, true);
                    }
                    // Select only the header cells
                    var headerCells2 = workSheet2.Cells[1, 1, 1, workSheet2.Dimension.Columns];

                    // Set their text to bold.
                    headerCells2.Style.Font.Bold = true;
                    // Set their background color
                    headerCells2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells2.Style.Fill.BackgroundColor.SetColor(Color.Yellow);

                    // Apply the auto-filter to all the columns
                    var allCells2 = workSheet2.Cells[workSheet2.Dimension.Address];
                    allCells2.AutoFilter = true;
                    // Auto-fit all the columns
                    allCells2.AutoFitColumns();

                    package.Save();

                    stream.Position = 0;
                    //get new GUID
                    var dataSessionKey = Guid.NewGuid().ToString();
                    //tempData GUID
                    TempData["keyReport"] = dataSessionKey;
                    //Session Data
                    Session[dataSessionKey] = package.GetAsByteArray();
                }

                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { IsResult = false, Msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Download(string reportName)
        {
            var dataKey = TempData["keyReport"].ToString();

            if (Session[dataKey] != null)
            {
                byte[] data = Session[dataKey] as byte[];
                string excelName = $"{reportName}-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        //convert uploadfile to list object
        public static Tuple<List<HeaderObject>, List<DetailObject>, List<FooterObject>> GetFileImport(HttpPostedFileBase uploadFile)
        {
            //declare
            var headerList = new List<HeaderObject>();
            var detailList = new List<DetailObject>();
            var footerList = new List<FooterObject>();
            string header, footer;

            //check upload file
            if (uploadFile != null && uploadFile.ContentLength > 0)
            {
                //check extension
                if (Path.GetExtension(uploadFile.FileName) == ".txt")
                {
                    //read file and store
                    string result = new StreamReader(uploadFile.InputStream, Encoding.Default, true).ReadToEnd();

                    //split string in result and put to list
                    List<string> list = new List<string>(Regex.Split(result, "\\n"));

                    //cut header
                    header = list[0];
                    list.RemoveAt(0);
                    //get header detail from header string
                    headerList.Add(new HeaderObject
                    {
                        HeaderText = header,
                        FirstSequenceNo = header.Substring(1, 6),
                        BankCompanyCode = header.Substring(7, 3),
                        CompanyName = header.Substring(10, 25).Trim(),
                        ProcessDate = header.Substring(35, 6),
                        Blank = header.Substring(41, 100)
                    });

                    //cut footer
                    footer = list[list.Count - 1];
                    list.RemoveAt(list.Count - 1);
                    //get footer detail from footer string
                    footerList.Add(new FooterObject
                    {
                        FooterText = footer,
                        BankCode = footer.Substring(7, 3),
                        TotalRows = footer.Substring(1, 6).Trim(),
                        TotalRecords = footer.Substring(10, 7).Trim(),
                        ChangeRecords = footer.Substring(17, 7).Trim(),
                        DeleteRecords = footer.Substring(24, 7).Trim(),
                        Blank = footer.Substring(31, 110)
                    });

                    //loop item in list
                    foreach (var itm in list)
                    {
                        //convert to datetime
                        DateTime dateInput = DateTime.ParseExact(itm.Substring(11, 6), "ddMMyy", _dtEn);
                        var dateOutput = dateInput.ToString("dd/MM/yyyy", _dtEn);
                        //add detail list to model
                        detailList.Add(new DetailObject
                        {
                            DetailText = itm,
                            RunningNumber = itm.Substring(1, 6),
                            BankCode = itm.Substring(7, 3),
                            Action = itm.Substring(10, 1),
                            ApplyDate = dateOutput,
                            ApplyDateText = itm.Substring(11, 6),
                            Ref1 = itm.Substring(17, 20).Trim(),
                            Ref2 = itm.Substring(37, 20).Trim(),
                            Register = itm.Substring(57, 3),
                            BranchCode = itm.Substring(60, 6).Trim(),
                            AccountNo = itm.Substring(66, 16).Trim(),
                            AccountName = itm.Substring(82, 50).Trim(),
                            Status = itm.Substring(132, 1),
                            Blank = itm.Substring(133, 10)
                        });
                    }
                }
            }

            return System.Tuple.Create(headerList, detailList, footerList);
        }

        //stored backup file
        //not use
        public string StoreBackupFile(HttpPostedFileBase file)
        {
            try
            {
                //store backup file
                var originalFileName = Path.GetFileName(file.FileName);
                var fileName = (DateTime.Now.ToShortDateString()).Replace("/", "-") + "_" + originalFileName;
                var path = "D:\\" + fileName;
                //var path = Path.Combine(Server.MapPath("~/BackUp/"), fileName);
                //save file
                file.SaveAs(path);
                return path;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        //get header and footer detail
        public JsonResult GetFileHeaderFooterDetail(HttpPostedFileBase uploadFile)
        {
            //get result from file upload
            var result = GetFileImport(uploadFile);
            //get header and footer from tuple
            var resultHeader = result.Item1;
            var resultFooter = result.Item3;

            DateTime dateInput = DateTime.ParseExact(resultHeader[0].ProcessDate, "ddMMyy", _dtEn);
            var dateOutput = dateInput.ToString("dd/MM/yyyy", _dtEn);
            //get bank code,company,processdate,totalrecords
            var detail = new
            {
                //TODO:edit fix bank
                BankCompanyCode = "ธนาคารกรุงไทย",
                resultHeader[0].CompanyName,
                ProcessDate = dateOutput,
                resultFooter[0].TotalRows
            };

            return Json(detail, JsonRequestBehavior.AllowGet);
        }

        //call datatable
        [HttpPost]
        public JsonResult GetDatatableImportData(HttpPostedFileBase uploadFile)
        {
            var tuple = GetFileImport(uploadFile);
            //get detail from tuple
            var detailList = tuple.Item2;

            //sum item in list
            int sum = detailList.Select(x => x).Count();
            //put to datatable object
            var dt = new Dictionary<string, object>
            {
                {"draw", 0 },
                {"recordsTotal", detailList.Count() != 0 ? sum : detailList.Count()},
                {"recordsFiltered", detailList.Count() != 0 ? sum : detailList.Count()},
                {"data", detailList.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public class HeaderObject
        {
            public string HeaderText { get; set; }
            public string FirstSequenceNo { get; set; }
            public string BankCompanyCode { get; set; }
            public string CompanyName { get; set; }
            public string ProcessDate { get; set; }
            public string Blank { get; set; }
        }

        public class DetailObject
        {
            public string DetailText { get; set; }
            public string RunningNumber { get; set; }
            public string BankCode { get; set; }
            public string Action { get; set; }
            public string ApplyDate { get; set; }
            public string ApplyDateText { get; set; }
            public string Ref1 { get; set; }
            public string Ref2 { get; set; }
            public string Register { get; set; }
            public string BranchCode { get; set; }
            public string AccountNo { get; set; }
            public string AccountName { get; set; }
            public string Status { get; set; }
            public string Blank { get; set; }
        }

        public class FooterObject
        {
            public string FooterText { get; set; }
            public string TotalRows { get; set; }
            public string BankCode { get; set; }
            public string TotalRecords { get; set; }
            public string ChangeRecords { get; set; }
            public string DeleteRecords { get; set; }
            public string Blank { get; set; }
        }

        #endregion ImportData

        #region ImportExcel

        [HttpGet]
        [Helper.Authorization(Roles = "Developer,BankDirectDebit_Premium")]
        public ActionResult ImportExcel()
        {
            return View();
        }

        public ActionResult ImportExcelNoValidate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportExcel(FormCollection form, HttpPostedFileBase uploadFile)
        {
            try
            {
                //backup file
                var backupPath = GlobalFunction.CreateBackupFile(uploadFile);
                //store user
                var user = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                //get result from file upload
                var mainResult = GetExcelImportDetail(uploadFile);
                var detailResult = mainResult.Item2;
                //add header
                var headerResult = new List<KTBReportHeader>();
                //call bank detail from bank code
                var bankDetail = _context.usp_Bank_Select(null, detailResult[0].BankCode).FirstOrDefault();
                headerResult.Add(new KTBReportHeader()
                {
                    BankId = bankDetail.TempCode.ToString(),
                    CreateByUserId = user,
                    CreateDate = DateTime.Now,
                    FileBackupPath = backupPath,
                    IsActive = true,
                    TotalRowCount = detailResult.Count(),
                    UpdateDate = DateTime.Now,
                    UpdateByUserId = user,
                });
                //save list model to entity
                _context.KTBReportHeaders.Add(headerResult[0]);
                _context.SaveChanges();

                int headerId = headerResult[0].KTBReportHeaderId;

                var detail = new List<KTBReportDetail>();
                foreach (var itm in detailResult)
                {
                    detail.Add(new KTBReportDetail()
                    {
                        KTBReportHeaderId = headerId,
                        BankDirectDebitHeaderCode = "",
                        OrderNo = itm.OrderNo,
                        DeliverDate = itm.DeliverDate,
                        BankCode = itm.BankCode,
                        AccountNo = itm.AccountNo,
                        AccountName = itm.AccountName,
                        Ref1 = itm.Ref1,
                        Ref2 = itm.Ref2,
                        KTBReportStatusId = itm.KTBReportStatusId,
                        CauseText = itm.CauseText,
                        IsActive = true,
                        CreateByUserId = user,
                        CreateDate = DateTime.Now,
                        UpdateByUserId = user,
                        UpdateDate = DateTime.Now
                    });
                }
                //save list model to entity
                _context.KTBReportDetails.AddRange(detail);
                _context.SaveChanges();

                //wait for Update store
                _context.usp_BankDirectDebit_KTBReport_Update(headerId);
                _context.usp_BankDirectDebitLog_Insert(groupId: headerId, bankDirectDebitLogType: 4, createdByUserId: user);

                //send success message
                TempData["Success"] = "อัพโหลดไฟล์สำเร็จ";
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("InternalServerError", "Error", new { exception = "ไม่สามารถอัพโหลดข้อมูลได้!! " + e });
            }
        }

        //function get tuple (multi return list)
        public Tuple<List<KTBReportHeader>, List<KTBReportDetail>> GetExcelImportDetail(HttpPostedFileBase uploadFile)
        {
            List<KTBReportDetail> detailList = new List<KTBReportDetail>();
            List<KTBReportHeader> headerList = new List<KTBReportHeader>();
            //check upload file
            if (uploadFile != null && uploadFile.ContentLength > 0)
            {
                // var f = new FileInfo(uploadFile.InputStream); ,"5153"

                //check extension
                if (Path.GetExtension(uploadFile.FileName) == ".xlsx")
                {
                    //read excel
                    using (var package = new ExcelPackage(uploadFile.InputStream))
                    {
                        var tbl = new System.Data.DataTable();
                        var ws = package.Workbook.Worksheets.First();

                        var hasHeader = true;  // adjust accordingly
                                               // add DataColumns to DataTable
                        var countColumns = 0;
                        foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                        {
                            if (!string.IsNullOrEmpty(firstRowCell.Text))
                            {
                                countColumns++;
                                tbl.Columns.Add(hasHeader ? firstRowCell.Text : String.Format("Column {0}", firstRowCell.Start.Column));
                            }
                        }

                        // add DataRows to DataTable
                        int startRow = hasHeader ? 2 : 1;
                        for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                        {
                            var wsRow = ws.Cells[rowNum, 1, rowNum, countColumns];

                            var firstCell = ((object[,])wsRow.Value)[0, 0];

                            if (firstCell is null) break;
                            if (!int.TryParse(firstCell.ToString(), out var result)) break;

                            DataRow row = tbl.NewRow();
                            foreach (var cell in wsRow)
                                row[cell.Start.Column - 1] = cell.Text;
                            tbl.Rows.Add(row);
                        }
                        //tbl.Rows.RemoveAt(tbl.Rows.Count - 1);

                        foreach (DataRow row in tbl.Rows)
                        {
                            int statusId;
                            if (row["สถานะ"].ToString() == "รับรอง")
                            {
                                statusId = 2;
                            }
                            else if (row["สถานะ"].ToString() == "ไม่รับรอง")
                            {
                                statusId = 3;
                            }
                            else if (row["สถานะ"].ToString() == "ยกเลิก")
                            {
                                statusId = 4;
                            }
                            else { statusId = 1; }

                            detailList.Add(new KTBReportDetail()
                            {
                                OrderNo = Convert.ToInt32(row["ลำดับ"]),
                                AccountName = row["ชื่อบัญชี"].ToString(),
                                AccountNo = row["เลขที่บัญชี"].ToString(),
                                BankCode = row["รหัสธนาคาร"].ToString(),
                                DeliverDate = DateTime.ParseExact(row["วันที่นำส่ง"].ToString(), "dd/MM/yyyy", new CultureInfo("th-TH")),
                                Ref1 = row["Ref No.1/Tax Id"].ToString(),
                                Ref2 = row["Ref No.2/  Service Name"].ToString(),
                                KTBReportStatusId = statusId,
                                CauseText = row["สาเหตุ"].ToString()
                            });
                        }
                    }
                }
            }
            return System.Tuple.Create(headerList, detailList);
        }

        //get excel table for preview
        [HttpPost]
        public ActionResult GetExcelPreviewTable(HttpPostedFileBase uploadFile)
        {
            //call function and assign to datatable
            var result = GetExcelImportDetail(uploadFile);
            //var headerResult = result.Item1;
            var detailResult = result.Item2;

            //sum item in list
            int sum = detailResult.Select(x => x).Count();

            //put to datatable object
            var dt = new Dictionary<string, object>
            {
                {"draw", 0 },
                {"recordsTotal", detailResult.Count() != 0 ? sum : detailResult.Count()},
                {"recordsFiltered", detailResult.Count() != 0 ? sum : detailResult.Count()},
                {"data", detailResult.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetHeaderKTBDetail(HttpPostedFileBase uploadFile)
        {
            var result = GetExcelImportDetail(uploadFile);
            var detail = result.Item2;
            var bankDetail = _context.usp_Bank_Select(null, detail[0].BankCode).FirstOrDefault();
            var headerDetail = new
            {
                BankCompanyCode = bankDetail.OrganizeDetail,
                ProcessDate = detail[0].DeliverDate,
                TotalRow = detail.Count()
            };

            return Json(headerDetail, JsonRequestBehavior.AllowGet);
        }

        #endregion ImportExcel

        #region AddDirectDebitHeader

        public ActionResult AddDirectDebitHeader()
        {
            ViewBag.BankId = _context.usp_Bank_Select(3, null).ToList();

            var BranchID = GlobalFunction.GetLoginDetail(this.HttpContext).Branch_ID;
            //var BranchCode = _context.usp_Branch_Select(BranchID).FirstOrDefault();

            var objBranch = _context.usp_Branch_Select(BranchID).FirstOrDefault();
            ViewBag.BranchCode = objBranch.BranchCode;
            ViewBag.Branch_Name = objBranch.BranchDetail;
            ViewBag.BranchId = BranchID;

            return View();
        }

        public JsonResult GetDatatable(int? draw = null, int? IndexStart = null, int? pageSize = null, string sortField = null, string orderType = null, string search = null, string BankId = null, int? PayerBranchId = null, int? BankDirectDebitStatusId = null)
        {
            if (BankDirectDebitStatusId == 0)
            {
                BankDirectDebitStatusId = null;
            }

            var result = _context.usp_BankDirectDebitAccountNo_SelectV2(BankId, PayerBranchId, BankDirectDebitStatusId,
                IndexStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDatatableALL(int? draw = null, int? IndexStart = null, int? pageSize = null, string sortField = null, string orderType = null, string search = null, string BankId = null, int? PayerBranchId = null, int? BankDirectDebitStatusId = null)
        {
            if (BankDirectDebitStatusId == 0)
            {
                BankDirectDebitStatusId = null;
            }

            var result = _context.usp_BankDirectDebitAccountNo_SelectV2(BankId, PayerBranchId, BankDirectDebitStatusId,
                IndexStart, pageSize, sortField, orderType, search).ToList();
            string[] arrayList = new string[result.Count];
            int i = 0;
            foreach (var List in result)
            {
                arrayList[i] = (List.AccountNo);
                i++;
            }
            return Json(arrayList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDatatableDetail(int? draw = null, int? IndexStart = null, int? pageSize = null, string sortField = null, string orderType = null, string search = null, string BankId = null, string AccountNo = null)
        {
            var result = _context.usp_BankDirectDebitAccountNoDetail_Select(BankId, AccountNo, IndexStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDatatableHead(int? draw = null, int? IndexStart = null, int? pageSize = null, string sortField = null, string orderType = null, string search = null, string BankId = null, string AccountNo = null)
        {
            var BankDirectDebit = _context.usp_BankDirectDebitAccountNoDetail_Select(BankId, AccountNo, IndexStart, pageSize, sortField, orderType, search).FirstOrDefault();
            string[] result = new string[5];
            result[0] = BankDirectDebit.AccountNo;
            result[1] = BankDirectDebit.AccountName;
            result[2] = BankDirectDebit.PayerName;
            result[3] = BankDirectDebit.PayerPhoneNumber;
            result[4] = BankDirectDebit.AgentName;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InsertGroup(string BankId = null, string[] AccountNoList = null)
        {
            var userid = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            string[] result = new string[2];

            result[0] = "false";
            result[1] = "";
            var index = 0;
            string AccountList = "";
            try
            {
                foreach (var List in AccountNoList)
                {
                    AccountList = AccountList + ',' + List;
                    index += 1;
                }

                if (AccountList != null)
                {
                    AccountList = AccountList.Substring(1, AccountList.Length - 1);
                    var InserDirect = _context.usp_BankDirectDebitDetail_Insert(BankId, AccountList, userid).FirstOrDefault();
                    result[1] = InserDirect.Result;
                }
                else
                {
                    result[0] = "false";
                }

                result[0] = "true";
            }
            catch (Exception e)
            {
                result[0] = "false";
                Console.WriteLine(e);
                throw;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion AddDirectDebitHeader

        #region ManageDirectDebit

        public ActionResult ManageDirectDebit()
        {
            ViewBag.BranchList = _context.usp_Branch_Select(null).ToList();
            var user = GlobalFunction.GetLoginDetail(HttpContext).UserName;
            var department = GlobalFunction.GetLoginDetail(HttpContext).Department_ID;
            if (department != 7 && user != "00174" && user != "03774" && user != "01230")
            {
                ViewBag.BranchUser = GlobalFunction.GetLoginDetail(HttpContext).Branch_ID;
            }
            return View();
        }

        [HttpPost]
        public ActionResult ManageDirectDebit(FormCollection form)
        {
            //submit form delete item in list
            var deleteId = form["hd_DeleteItemId"];
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            try
            {
                //add user session
                var result = _context.usp_BankDirectDebitHeader_Delete(deleteId, user).FirstOrDefault();

                if (result.IsResult == false)
                {
                    return RedirectToAction("InternalServerError", "Error", new { exception = "ไม่สามารถลบข้อมูลได้!! " + result.Msg });
                }
                return RedirectToAction("ManageDirectDebit", "BankDirectDebit");
            }
            catch (Exception e)
            {
                return RedirectToAction("InternalServerError", "Error", new { exception = "ไม่สามารถลบข้อมูลได้!! " + e });
            }
        }

        [HttpPost]
        public JsonResult GetHeaderDirectDebit(string dateFrom, string dateTo, int? branchId,
            int? draw = null, int? indexStart = null, int? pageSize = null,
            string sortField = null, string orderType = null, string search = null)
        {
            var datet = DateTime.ParseExact(dateTo, "dd/MM/yyyy", new CultureInfo("en-Us"));
            var datef = DateTime.ParseExact(dateFrom, "dd/MM/yyyy", new CultureInfo("en-Us"));

            //get result from db
            var result = _context.usp_BankDirectDebitHeader_NotIsConfirm_Select(null, branchId, datef, datet, indexStart, pageSize, sortField,
                orderType, search).ToList();

            //result[0].IsAllowDelete
            //put result to datatable
            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetHeaderDetail(int bankCoverId)
        {
            //get result from db
            var result = _context.usp_BankDirectDebitDetailV2_Select(bankCoverId).ToList();
            //put result to datatable
            var totalRow = result.Count();
            var dt = new Dictionary<string, object>
            {
                {"draw", 0 },
                {"recordsTotal", result.Count() != 0 ? totalRow : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? totalRow : result.Count()},
                {"data", result.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        #endregion ManageDirectDebit

        #region "Report"

        public ActionResult BankDirectDebitReport()
        {
            return View();
        }

        #endregion "Report"

        #region ExcelForExport

        [Helper.Authorization(Roles = "Developer,BankDirectDebit_Premium")]
        public ActionResult ExcelForExport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ExcelForExport(FormCollection form)
        {
            //Get list of path file
            var pathList = form["hd_pathList"].Split(',').ToList();
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            var pathFile = Path.GetDirectoryName(pathList[0].ToString());
            var pathzip = Path.GetDirectoryName(Path.GetDirectoryName(pathList[0].ToString()));

            var fileName = form["hd_fileName"] + ".zip";

            var fullPath = Path.GetFullPath(pathzip + "\\" + fileName);
            //int unicode = 92;
            //char character = (char)unicode;

            //string fullPath = "D:" + character.ToString() + "KDR18080037.zip";

            //string ccEmail = "sujimanus.jirapraphoosak@ktb.co.th,mana.purksasri@ktb.co.th,nijjaree.s-na_nakornpanom@ktb.co.th,pornsivarai.thipmanee@ktb.co.th";

            try
            {
                var result = GlobalFunction.AddToZip(pathFile, pathzip + "\\" + fileName);

                Thread.Sleep(5000);

                if (result != "")
                {
                    return RedirectToAction("InternalServerError", "Error", new { Exception = result });
                }

                using (var client = new EmailServiceClient())
                {
                    string emailResult;
                    if (Properties.Settings.Default.SendMailUAT == true)
                    {
                        //uat
                        emailResult = client.SendEmail(Properties.Settings.Default.PremiumEmail, Properties.Settings.Default.PremiumEmailPassword,
                          "programmer.isc@gmail.com", "programmer.isc@siamsmile.co.th", "",
                          "บริษัทสยามสไมล์ฯ นำส่งหนังสือยินยอมให้หักบัญชีเงินฝาก ประจำวันที่ " + DateTime.Today.ToShortDateString() + " " + fileName, "", fullPath);
                    }
                    else
                    {
                        //production
                        emailResult = client.SendEmail(Properties.Settings.Default.PremiumEmail, Properties.Settings.Default.PremiumEmailPassword,
                            Properties.Settings.Default.SendingMail, Properties.Settings.Default.CCMail, "",
                            "บริษัทสยามสไมล์ฯ นำส่งหนังสือยินยอมให้หักบัญชีเงินฝาก ประจำวันที่ " + DateTime.Today.ToString("dd/MM/yyyy") + " " + fileName, "", fullPath);
                    }

                    if (emailResult == "success")
                    {
                        _context.usp_BankDirectDebitHeaderGroup_Update(form["hd_fileName"], 3, user);
                        TempData["Success"] = "ส่งข้อมูลสำเร็จ";
                    }
                    else
                    {
                        _context.usp_BankDirectDebitHeaderGroup_Update(form["hd_fileName"], 5, user);
                        TempData["Fail"] = "ส่งข้อมูลไม่สำเร็จ กรุณาติดต่อผู้ดูแลระบบ!!";
                    }
                }
            }
            catch (Exception e)
            {
                TempData["Fail"] = "ส่งข้อมูลไม่สำเร็จ กรุณาติดต่อผู้ดูแลระบบ!!";
            }

            return View();
        }

        public JsonResult GenerateExcel2()
        {
            var pathList = new List<string>();
            //store user
            var user = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
            try
            {
                //WARN:fix bank id
                var resultHeader = _context.usp_BankDirectDebitHeaderGroup_Insert("1000", user).FirstOrDefault().Result;

                var headerList = _context.usp_BankDirectDebitHeaderGroupDetail_Select(Convert.ToInt32(resultHeader), null, 99999, null, null, null)
                    .ToList()
                    .Select(e => new
                    {
                        โค๊ด = e.BankDirectDebitHeaderGroupCode,
                        เลขที่ใบคุมเอกสาร = e.BankDirectDebitHeaderCode,
                        วันที่ทำรายการ = e.CreatedDate,
                        ชื่อสาขาที่นำส่ง = e.Branch,
                        ธนาคาร = e.Bank,
                        เลขที่บัญชี = e.AccountNo,
                        ชื่อบัญชี = e.AccountName,
                        RefNo1_taxId = e.Ref1
                    }).ToList();

                var groupHeaderList = headerList
                    .GroupBy(u => u.เลขที่ใบคุมเอกสาร)
                    .Select(grp => grp.ToList())
                    .ToList();

                var headerGroupCode = headerList[0].โค๊ด;

                foreach (var itm in groupHeaderList)
                {
                    //export to excel and return path to list
                    var excelPath = GlobalFunction.ExportToExcel(itm, "sheet1", itm[0].ชื่อสาขาที่นำส่ง + "-" + itm[0].เลขที่ใบคุมเอกสาร, headerGroupCode);
                    pathList.Add(excelPath);
                }

                var resultCOUNT = headerList.Count();

                var resultData = new { headerGroupCode, pathList, resultCOUNT };
                //return list path
                return Json(resultData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion ExcelForExport

        #region ReportForPremium

        [HttpGet]
        public ActionResult ReportForPremium()
        {
            //Get login detail
            var user = GlobalFunction.GetLoginDetail(HttpContext);
            //Get Role
            var roles = OAuth2Helper.GetRoles();

            if (roles.Contains("Developer") || roles.Contains("BankDirectDebit_Premium"))
            {
                ViewBag.BranchList = _context.usp_Branch_Select(null).ToList();
            }
            else
            {
                ViewBag.BranchList = _context.usp_Branch_Select(user.Branch_ID).ToList();
            }

            ViewBag.BankList = _context.usp_Bank_Select(null, null).ToList().OrderBy(x => x.BankId);
            ViewBag.SendStatus = _context.usp_DeliverStatus_Select(null).ToList();
            ViewBag.CheckStatus = _context.usp_BankDirectDebitStatusDisplay_Select().ToList();

            ViewBag.BranchUser = user.Branch_ID;

            return View();
        }

        [HttpPost]
        public ActionResult ReportForPremium(FormCollection form)
        {
            var datet = DateTime.ParseExact(form["DateTo"], "dd/MM/yyyy", new CultureInfo("en-Us"));
            var datef = DateTime.ParseExact(form["DateFrom"], "dd/MM/yyyy", new CultureInfo("en-Us"));
            int? branchId = null;

            if (!string.IsNullOrWhiteSpace(form["ddlBranch"]))
            {
                branchId = Convert.ToInt32(form["ddlBranch"]);
            }

            var bankId = form["ddlBank"];
            int? docStatus = null;
            if (form["ddlDocStatus"] != "")
            {
                docStatus = Convert.ToInt32(form["ddlDocStatus"]);
            }
            int? statusDisplay = null;
            if (form["ddlStatus"] != "")
            {
                statusDisplay = Convert.ToInt32(form["ddlStatus"]);
            }

            try
            {
                var result = _context.usp_ReportBankDirectDebitDeliver(datef, datet, branchId, bankId, docStatus, statusDisplay,
              0, 99999, null, null, null).Select(e => new
              {
                  เลขที่ใบคุม = e.BankDirectDebitHeaderCode,
                  วันที่ทำรายการ = e.CreateDate,
                  ชื่อสาขานำส่ง = e.Branch,
                  ธนาคาร = e.Bank,
                  เลขที่บัญชี = e.AccountNo,
                  ชื่อบัญชี = e.AccountName,
                  Reference_TaxID = e.Ref1,
                  ช่องทางการRegister = e.Register,
                  สถานะการส่งเอกสาร = e.DeliverStatus,
                  วันที่ตรวจสอบเอกสาร = e.ApplyDate,
                  สถานะการตรวจสอบ = e.StatusDisplay,
                  หมายเหตุ = e.CauseText
              }).ToList();

                GlobalFunction.ExportDatatableToExcel(this.HttpContext, result, "sheet1", "รายงานผลนำส่งเอกสารหนังสือยินยอมหักบัญชีธนาคาร_" + DateTime.Now.ToShortDateString().Replace("/", "-"));
                return RedirectToAction("ReportForPremium");
            }
            catch (Exception e)
            {
                return RedirectToAction("InternalServerError", "Error", new { Exception = e });
            }
        }

        public JsonResult GetReportForPremium(string dFrom, string dTo, int? branchId, string bankId, int? statusDisplay
            , int? docStatus, int? draw, int? indexStart, int? pageSize = null,
            string sortField = null, string orderType = null, string search = null)
        {
            var datet = DateTime.ParseExact(dTo, "dd/MM/yyyy", new CultureInfo("en-Us"));
            var datef = DateTime.ParseExact(dFrom, "dd/MM/yyyy", new CultureInfo("en-Us"));

            //get result from db
            var result = _context.usp_ReportBankDirectDebitDeliver(datef, datet, branchId, bankId, docStatus,
                statusDisplay, indexStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReportApprovedByAppID()
        {
            ViewBag.CheckStatus = _context.usp_BankDirectDebitStatusDisplay_Select().ToList();
            return View();
        }

        public ActionResult ReportFollowAccountBranch()
        {
            //Get login detail
            var user = GlobalFunction.GetLoginDetail(HttpContext);
            //Get Role
            var roles = OAuth2Helper.GetRoles();

            if (roles.Contains("Developer") || roles.Contains("BankDirectDebit_Premium"))
            {
                ViewBag.BranchList = _context.usp_Branch_Select(null).ToList();
            }
            else
            {
                ViewBag.BranchList = _context.usp_Branch_Select(user.Branch_ID).ToList();
            }

            return View();
        }

        [Helper.Authorization()]
        public ActionResult ReportFollowAccountAgent()
        {
            //Get login detail
            var user = GlobalFunction.GetLoginDetail(HttpContext);
            //Get Role
            var roles = OAuth2Helper.GetRoles();

            if (roles.Contains("Developer") || roles.Contains("BankDirectDebit_Premium"))
            {
                ViewBag.BranchList = _context.usp_Branch_Select(null).ToList();
            }
            else
            {
                ViewBag.BranchList = _context.usp_Branch_Select(user.Branch_ID).ToList();
            }

            return View();
        }

        #endregion ReportForPremium

        #region ReportImportData

        [Helper.Authorization(Roles = "Developer,BankDirectDebit_Premium")]
        public ActionResult ReportImportData()
        {
            ViewBag.BankList = _context.usp_Bank_Select(null, null).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult ReportImportData(FormCollection form)
        {
            var headerId = Convert.ToInt32(form["hd_headerId"]);

            var pathFile = _context.usp_KTBReportHeader_FilePath_Select(headerId).FirstOrDefault();
            if (pathFile != null)
            {
                try
                {
                    var fileName = Path.GetFileName(pathFile);
                    //get some path of string
                    pathFile = String.Join(@"\", pathFile.Split('\\').Reverse().Take(4).Reverse()).Replace("\\", "/");
                    var fullPath = Properties.Settings.Default.PathFtpFile + pathFile;

                    var file = new WebClient().DownloadData(fullPath);

                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
                catch (Exception e)
                {
                    return RedirectToAction("InternalServerError", "Error", new { Exception = e });
                }
            }
            else
            {
                return RedirectToAction("InternalServerError", "Error", new { Exception = "ไม่สามารถดาวน์โหลดไฟล์ได้" });
            }
        }

        public ActionResult GetReportImportData(string dFrom, string dTo, string bankId
            , int? draw, int? indexStart, int? pageSize = null,
            string sortField = null, string orderType = null, string search = null)
        {
            var datet = DateTime.ParseExact(dTo, "dd/MM/yyyy", new CultureInfo("en-Us"));
            var datef = DateTime.ParseExact(dFrom, "dd/MM/yyyy", new CultureInfo("en-Us"));

            if (bankId == "")
            {
                bankId = null;
            }

            var result = _context.usp_KTBReportHeader_Select(bankId, datef, datet, indexStart, pageSize, sortField,
                orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        #endregion ReportImportData

        #region ReportForExport

        [Helper.Authorization(Roles = "Developer,BankDirectDebit_Premium")]
        public ActionResult ReportForExport()
        {
            return View();
        }

        public ActionResult GetReportExportData(string dFrom, string dTo, string bankId
    , int? draw, int? indexStart, int? pageSize = null,
    string sortField = null, string orderType = null, string search = null)
        {
            var datet = DateTime.ParseExact(dTo, "dd/MM/yyyy", new CultureInfo("en-Us"));
            var datef = DateTime.ParseExact(dFrom, "dd/MM/yyyy", new CultureInfo("en-Us"));

            if (bankId == "") { bankId = null; }

            var result = _context.usp_KTBReportExcelForExport_Select(bankId, datef, datet, indexStart, pageSize, sortField,
                orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        #endregion ReportForExport

        #region "AddBankDirectDebitHeaderOld"

        public ActionResult AddBankDirectDebitHeader(int? bankDirectDebitHeaderId)
        {
            if (bankDirectDebitHeaderId != null)
            {
                var result = _context.usp_BankDirectDebitDetailV2_Select(bankDirectDebitHeaderId).FirstOrDefault();
                if (result != null)
                {
                    ViewBag.ddrNum = result.HeaderCode;
                    ViewBag.bankDirectDebitHeaderId = result.BankDirectDebitHeaderId;
                }
            }

            return View();
        }

        public ActionResult GenBankDirectDebitHeader()
        {
            var bankId = "1000";

            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            try
            {
                var result = _context.usp_BankDirectDebitHeader_Insert(bankId, userId).SingleOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
                //throw;
            }
        }

        public ActionResult DeleteBankDirectDebitDetail(int bankDirectDebitDetail)
        {
            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            try
            {
                var result = _context.usp_BankDirectDebitDetail_Delete(bankDirectDebitDetail, userId);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
                //throw;
            }
        }

        public ActionResult GetAccountName(string AccountNo)
        {
            if (AccountNo != null)
            {
                AccountNo = AccountNo.Replace("-", "");
            }

            int c;

            c = _context.usp_AccountName_Select(AccountNo).Count();

            var result = "";

            if (c == 1)
            {
                result = _context.usp_AccountName_Select(AccountNo).SingleOrDefault();
            }
            else
            {
                result = "";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetIsConfirmBankDirectDebitHeader(string bankDirectDebitHeaderId)
        {
            var result = _context.usp_BankDirectDebitHeader_NotIsConfirm_Select(bankDirectDebitHeaderId, null, null, null, 0, 9999, null, null, null).SingleOrDefault().IsConfirm;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConfirmDirectDebitHeader(int bankDirectDebitHeaderId)
        {
            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            try
            {
                var result = _context.usp_BankDirectDebitHeader_Confirm_Update(bankDirectDebitHeaderId, userId);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
                //throw;
            }
        }

        public ActionResult GetDirectDebitDetail(int bankDirectDebitHeaderId)
        {
            var result = _context.usp_BankDirectDebitDetailV2_Select(bankDirectDebitHeaderId).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", 1 },
                {"recordsTotal", result.Count() } ,
                {"recordsFiltered", result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InsertBankDirectDebitDetail(int bankDirectDebitHeaderId, string accountNo, string accountName)
        {
            var bankId = "1000";
            var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            if (accountNo != null)
            {
                accountNo = accountNo.Replace("-", "");
            }

            try
            {
                var result = _context.usp_BankDirectDebitDetailV2_Insert(bankDirectDebitHeaderId, bankId, accountNo, accountName, userId).FirstOrDefault();

                if (result.IsResult == false)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
                //throw;
            }
        }

        #endregion "AddBankDirectDebitHeaderOld"

        #region ReportImportFile

        [Helper.Authorization(Roles = "Developer,BankDirectDebit_Premium")]
        public ActionResult ReportImportFile()
        {
            ViewBag.BankList = _context.usp_Bank_Select(null, null).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult ReportImportFile(FormCollection form)
        {
            var DDRStandardFormatHeaderId = Convert.ToInt32(form["hd_pathFile"]);

            var pathFile = _context.usp_DDRStandardFormatHeader_FilePath_Select(DDRStandardFormatHeaderId).FirstOrDefault();

            if (pathFile != null)
            {
                try
                {
                    var fileName = Path.GetFileName(pathFile);
                    //get some path of string
                    pathFile = String.Join(@"\", pathFile.Split('\\').Reverse().Take(4).Reverse()).Replace("\\", "/");
                    var fullPath = Properties.Settings.Default.PathFtpFile + pathFile;

                    var file = new WebClient().DownloadData(fullPath);

                    return File(file, "text/plain", fileName);
                }
                catch (Exception e)
                {
                    return RedirectToAction("InternalServerError", "Error", new { Exception = e });
                }
            }
            else
            {
                return RedirectToAction("InternalServerError", "Error", new { Exception = "ไม่สามารถดาวน์โหลดไฟล์ได้" });
            }
        }

        public ActionResult GetReportImportFile(string dFrom, string dTo, string bankId
           , int? draw, int? indexStart, int? pageSize = null,
           string sortField = null, string orderType = null, string search = null)
        {
            var datet = DateTime.ParseExact(dTo, "dd/MM/yyyy", new CultureInfo("en-Us"));
            var datef = DateTime.ParseExact(dFrom, "dd/MM/yyyy", new CultureInfo("en-Us"));

            if (bankId == "")
            {
                bankId = null;
            }

            var result = _context.usp_DDRStandardFormatHeader_Select(bankId, datef, datet, indexStart, pageSize, sortField,
                orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        #endregion ReportImportFile

        #region Monitor

        /// <summary>
        /// DocumentMonitor INDEX
        /// </summary>
        /// <returns></returns>
        public ActionResult DocumentMonitor()
        {
            //Get login detail
            var user = GlobalFunction.GetLoginDetail(HttpContext);

            //GET BRANCH ID USER
            ViewBag.BranchUser = user.Branch_ID;

            //GET ROLES
            var roles = OAuth2Helper.GetRoles();

            //Dev AND Premium
            if (roles.Contains("Developer") || roles.Contains("BankDirectDebit_Premium"))
            {
                ViewBag.BranchList = _context.usp_Branch_Select(null).ToList();
            }
            else
            {
                ViewBag.BranchList = _context.usp_Branch_Select(user.Branch_ID).ToList();
            }

            ViewBag.CheckStatus = _context.usp_TrackAccountStatus_Select().ToList();

            return View();
        }

        public ActionResult CheckApproved()
        {
            return View();
        }

        /// <summary>
        /// TrackAccountMonitor
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public JsonResult TrackAccountMonitor(int? branchId)
        {
            //CHECK -1 == SELECT ALL >> SET VALUE = NULL
            if (branchId == -1) branchId = null;
            //CALL STORE PROCEDURE
            var result = _context.usp_TrackAccountMonitor_Select(branchId).ToList();

            //RETURN RESULT
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// TrackAccountList For Datatables
        /// </summary>
        /// <param name="payerBranchId"></param>
        /// <param name="bankDirectDebitStatusDisplayId"></param>
        /// <param name="search"></param>
        /// <param name="indexStart"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TrackAccountList(int? payerBranchId, int? bankDirectDebitStatusDisplayId, string search, int indexStart = 0, int pageSize = 10, string sortField = "AccountNo", string orderType = "ASC")
        {
            //CHECK -1 == SELECT ALL >> SET VALUE = NULL
            if (bankDirectDebitStatusDisplayId == -1) bankDirectDebitStatusDisplayId = null;
            //CHECK -1 == SELECT ALL >> SET VALUE = NULL
            if (payerBranchId == -1) payerBranchId = null;
            //CALL STORE PROCEDURE
            var result = _context.usp_TrackAccountList_Select(payerBranchId
                                                             , bankDirectDebitStatusDisplayId
                                                             , indexStart
                                                             , pageSize
                                                             , sortField
                                                             , orderType
                                                             , search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            //RETURN
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  TrackAccountSelect For Datatables in Modal
        /// </summary>
        /// <param name="payerBranchId"></param>
        /// <param name="accountNo"></param>
        /// <param name="search"></param>
        /// <param name="indexStart"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TrackAccountSelect(int? payerBranchId, string accountNo, string search = "", int indexStart = 0, int pageSize = 10, string sortField = "AppID", string orderType = "ASC")
        {
            //CHECK -1 == SELECT ALL >> SET VALUE = NULL
            if (payerBranchId == -1) payerBranchId = null;
            //CALL STORE PROCEDURE
            var result = _context.usp_TrackAccount_Select(payerBranchId
                                                             , accountNo
                                                             , indexStart
                                                             , pageSize
                                                             , sortField
                                                             , orderType
                                                             , search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            //RETURN
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// EXPORT EXCEL TRACK ACCOUNT DETAIL
        /// </summary>
        /// <param name="payerBranchId"></param>
        /// <param name="bankDirectDebitStatusDisplayId"></param>
        public void ExportToExcel_TrackAccountDetail(int? payerBranchId, int? bankDirectDebitStatusDisplayId)
        {
            //CHECK -1 == SELECT ALL >> SET VALUE = NULL
            if (bankDirectDebitStatusDisplayId == -1) bankDirectDebitStatusDisplayId = null;
            //CHECK -1 == SELECT ALL >> SET VALUE = NULL
            if (payerBranchId == -1) payerBranchId = null;
            //SET DATE TIEM TO VARIABLE
            var d = DateTime.Now;
            //CALL STORE PROCEDURE
            var lst = _context.usp_TrackAccountDetail_Select(payerBranchId, bankDirectDebitStatusDisplayId).Select(o => new
            {
                เลขบัญชี = o.AccountNo,
                ชื่อบัญชี = o.AccountName,
                AppID = o.ApplicationCode,
                ชื่อ_สกุล_ผู้เอาประกัน = o.CustName,
                ชื่อ_สกุล_ผู้ชำระเบี้ย = o.PayerName,
                สาขา = o.PayerBranch,
                เบอร์ผู้ชำระเบี้ย = o.PayerMobile,
                สถานะApp = o.AppStatus,
                ที่อยู่ = o.PayerBuildingName,
                ตำบล = o.PayerSubDistrict,
                อำเภอ = o.PayerDistrict,
                จังหวัด = o.PayerProvince,
                พื้นที่_ผอ = o.PayerStudyArea,
                รหัสตัวแทน = o.SaleCode,
                สถานะงานKTB = o.BankDirectDebitStatusDisplay,
                วันที่ดำเนินการนำส่งKTB = o.UpdatedDate,
                วันที่KTBตรวจสอบ = o.KTBApplyDate,
            }).ToList();
            //CALL FUNCTION EXPORT TO EXCEL
            GlobalFunction.ExportToExcel2(HttpContext, lst, "Sheet1", "AccountDetail_" + d);
        }

        #endregion Monitor
    }
}