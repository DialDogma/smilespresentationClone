using OfficeOpenXml;
using SmileSPoint.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSPoint.Helper;

namespace SmileSPoint.Controllers
{
    [Authorization]
    public class DeductController : Controller
    {
        private SmilePointEntities _context;

        public DeductController()
        {
            _context = new SmilePointEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Deduct
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetDatatable(int draw, int pageSize, int pageStart, string sortField, string orderType, string search)
        {
            var list = _context.usp_DeductImport_Datatable(pageStart, pageSize, sortField, orderType, search).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadDeduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadDeduct(FormCollection form, HttpPostedFileBase file)
        {
            var lst = new List<TransactionImportRow>();

            if (file != null && file.ContentLength > 0)

                if (Path.GetExtension(file.FileName) != ".xlsx")
                {
                    ViewBag.Message = "กรุณาเลือก Excel file";
                }
                else
                {
                    //Import Excel To List
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var cs = package.Workbook.Worksheets;
                        var ws = cs.First();
                        var nr = ws.Dimension.End.Row;

                        try
                        {
                            for (var ri = 2; ri <= nr; ri++)
                            {
                                if (ws.Cells[ri, 1].Value != null)
                                {
                                    var item = new TransactionImportRow()
                                    {
                                        AccountId = Convert.ToInt32(ws.Cells[ri, 1].Value.ToString()),
                                        TransactionTypeId = Convert.ToInt32(ws.Cells[ri, 2].Value.ToString()),
                                        Point = Convert.ToDouble(ws.Cells[ri, 3].Value.ToString()),
                                        Remark = ws.Cells[ri, 4].Value.ToString(),
                                    };
                                    lst.Add(item);
                                }
                            }
                        }
                        catch (Exception)
                        {
                            ViewBag.ErrorText = "รูปแบบไฟล์ไม่ถูกต้อง";
                            return View();
                        }
                    }
                }

            //Add New PreImportHeader
            var header = new DeductImportHeader()
            {
                CreatedBy = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID,
                CreatedDate = System.DateTime.Now,
                SubmitStatus = 0
            };

            _context.DeductImportHeaders.Add(header);
            _context.SaveChanges();

            //Add PreImportDetail
            foreach (var item in lst)
            {
                var detail = new DeductImportDetail()
                {
                    Amount = item.Point,
                    PointAccountId = item.AccountId,
                    TransactionTypeId = item.TransactionTypeId,
                    DeductImportHeaderId = header.DeductImportHeaderId,
                    Remark = item.Remark
                };

                _context.DeductImportDetails.Add(detail);
                _context.SaveChanges();
            }

            return RedirectToAction("UploadPreSubmit", new { id = header.DeductImportHeaderId });
        }

        public ActionResult UploadPreSubmit(int id)
        {
            //Validate Account
            var validateResult = _context.usp_DeductImportValidate(id).ToList();
            //TODO: Validate TransactionId

            ViewBag.DeductImportHeaderId = id;

            if (validateResult.Count > 0)
            {
                ViewBag.ErrorList = validateResult;
            }
            else
            {
                var result = _context.usp_DeductImportOverview(id).ToList();
                ViewBag.List = result;
                ViewBag.ItemCount = result.Sum(item => item.ItemCount);
                ViewBag.ItemSum = result.Sum(item => item.ItemSum);
            }

            return View();
        }

        [HttpPost]
        public ActionResult UploadSubmit(FormCollection form)
        {
            var headerId = Convert.ToInt32(form["DeductImportHeaderId"]);
            var lst = _context.DeductImportDetails.Where(x => x.DeductImportHeaderId == headerId).ToList();

            //Loop For Submit
            foreach (var item in lst)
            {
                var createdByID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                _context.usp_DeductCreateTransaction(item.DeductImportDetailId, createdByID);
            }

            //Update Header
            _context.usp_DeductUpdateHeader(headerId);

            return RedirectToAction("UploadSubmit", new { id = headerId });
        }

        public ActionResult UploadSubmit(int id)
        {
            ViewBag.Result = "Import เรียบร้อยแล้ว ";
            ViewBag.DeductImportHeaderId = id;
            return View();
        }

        public ActionResult DownloadResult(int id)
        {
            //download excel
            //var lst = _context.usp_DeductImportResult(id).ToList();
            var lst = from u in _context.usp_DeductImportResult(id)
                      select new
                      {
                          TransactionId = u.ImportedTransactionId,
                          Account = u.PointAccountId,
                          Reference = u.ReferenceCode,
                          AccountName = u.PointAccountName,
                          TransactionName = u.TransactionTypeName,
                          แต้มก่อนหัก = u.BalanceBefore,
                          แต้มส่งหัก = u.Amount,
                          หักได้ = u.Deductible,
                          หักไม่ได้ = u.NotDeductible,
                          แต้มหลังส่งหัก = u.BalanceAfter,
                          หมายเหตุ = u.Remark
                      };
            var sheetName = "Result" + id.ToString();

            ExcelManager.ExportToExcel(this.HttpContext, lst.ToList(), sheetName, sheetName);
            return RedirectToAction("UploadSubmit", new { id });
        }

        public FileResult DownloadDeductExampleFile()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "ExampleExcelFiles/";
            string fileName = "Import_Deduct_Example.xlsx";
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + fileName);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public ActionResult DownloadTransactionType()
        {
            var lst = _context.usp_Transaction_GetTypeList().ToList();
            ExcelManager.ExportToExcel(this.HttpContext, lst, "TransactionType", "TransactionType");
            return View();
        }
    }
}