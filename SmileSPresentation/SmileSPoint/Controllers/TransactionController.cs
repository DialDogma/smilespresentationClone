using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using SmileSPoint.Models;
using SmileSPoint.Helper;
using System.Globalization;

namespace SmileSPoint.Controllers
{
    public class TransactionController : Controller
    {
        private SmilePointEntities _context;

        public TransactionController()
        {
            _context = new SmilePointEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Transaction
        public ActionResult Index()
        {
            return View();
        }

        #region UpLoadAddPoint

        [HttpPost]
        public ActionResult UploadAddPoint(FormCollection form, HttpPostedFileBase file)
        {
            var transactionTypeId = 1;
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
                                        AccountId = Convert.ToInt32(ws.Cells[ri, 1].Value.ToString().Trim()),
                                        Point = Convert.ToDouble(ws.Cells[ri, 2].Value.ToString().Trim()),
                                        Remark = ws.Cells[ri, 3].Value.ToString().Trim(),
                                        TransactionTypeId = transactionTypeId
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
            var header = new PreImportHeader()
            {
                CreatedBy = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID,
                CreatedDate = System.DateTime.Now,
                SubmitStatus = 0
            };

            _context.PreImportHeaders.Add(header);
            _context.SaveChanges();

            //Add PreImportDetail
            foreach (var item in lst)
            {
                var detail = new PreImportDetail()
                {
                    Point = item.Point,
                    PointAccountId = item.AccountId,
                    TransactionTypeId = item.TransactionTypeId,
                    PreImportHeaderId = header.PreImportHeaderId,
                    Remark = item.Remark
                };

                _context.PreImportDetails.Add(detail);
                _context.SaveChanges();
            }

            return RedirectToAction("UploadPreSubmit", new { id = header.PreImportHeaderId });
        }

        [HttpGet]
        public ActionResult UploadAddPoint()
        {
            return View();
        }

        public FileResult DownloadAddPointExampleFile()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "ExampleExcelFiles/";
            string fileName = "Import_Addpoint_Example.xlsx";
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + fileName);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        #endregion UpLoadAddPoint

        #region UploadSubmit

        public ActionResult UploadPreSubmit(int id)
        {
            //TODO: Validate Account
            var validateResult = _context.usp_PreImportValidate(id).ToList();

            ViewBag.PreImportHeaderId = id;

            if (validateResult.Count > 0)
            {
                ViewBag.ErrorList = validateResult;
            }
            else
            {
                var result = _context.usp_PreImportOverview(id).ToList();
                ViewBag.List = result;
                ViewBag.ItemCount = result.Sum(item => item.ItemCount);
                ViewBag.ItemSum = result.Sum(item => item.ItemSum);
            }

            return View();
        }

        [HttpPost]
        public ActionResult UploadSubmit(FormCollection form)
        {
            var id = Convert.ToInt32(form["PreImportHeaderId"]);
            var lst = _context.PreImportDetails.Where(x => x.PreImportHeaderId == id).ToList();

            //Loop For Submit
            foreach (var item in lst)
            {
                var createdByID = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                var import = _context.usp_CreateTransaction(item.PointAccountId, item.TransactionTypeId, item.Point, item.Remark, createdByID).FirstOrDefault();
                item.ImportedTransactionId = import.TransactionId;
                _context.SaveChanges();
            }

            var importHeader = _context.PreImportHeaders.Where(x => x.PreImportHeaderId == id).FirstOrDefault();
            importHeader.SubmitStatus = 1;
            _context.SaveChanges();

            ViewBag.Result = "Import เรียบร้อยแล้ว ";

            return View();
        }

        #endregion UploadSubmit

        public JsonResult GetTransactionType(int transactionGroupId)
        {
            var result = _context.TransactionTypes.Where(X => X.TransactionGroupId == transactionGroupId);

            return Json(result.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDatatable(int draw, int pageSize, int pageStart, string sortField, string orderType, string search, int accountId)
        {
            var list = _context.SP_Transaction_Datatable(pageStart, pageSize, sortField, orderType, search, accountId).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search()
        {
            return View();
        }

        public JsonResult SearchDatatable(int draw, int pageSize, int pageStart, string sortField, string orderType, string search, string dFrom, string dTo)
        {
            DateTime? dateFrom = null;
            DateTime? dateTo = null;

            dateFrom = GlobalFunction.ConvertDatePickerToDate_BE(dFrom);
            dateTo = GlobalFunction.ConvertDatePickerToDate_BE(dTo);

            // ถึงวันที่ต้อง + 1 วัน
            if (dateTo.HasValue)
            {
                dateTo = dateTo.Value.AddDays(1);
            }

            var list = _context.SP_Transaction_Datatable_Search(pageStart, pageSize, sortField, orderType, search, dateFrom, dateTo).ToList();

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault().TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail(int id)
        {
            var detail = _context.usp_Transaction_GetDetail(id).FirstOrDefault();
            ViewBag.AmountBefore = detail.AmountBalance + detail.AmountDecrease - detail.AmountIncrease;
            return View(detail);
        }

        #region Cancel Transaction

        public ActionResult Cancel(int id)
        {
            var detail = _context.usp_Transaction_GetDetail(id).FirstOrDefault();
            ViewBag.AmountBefore = detail.AmountBalance + detail.AmountDecrease - detail.AmountIncrease;
            return View(detail);
        }

        [HttpPost]
        public ActionResult Cancel(FormCollection form)
        {
            var PointAccountId = Convert.ToInt32(form["PointAccountId"]);
            var transactionId = Convert.ToInt32(form["TransactionId"]);
            var Remark = form["Remark"];
            var transaction = _context.Transactions.Where(x => x.TransactionId == transactionId).FirstOrDefault();

            _context.usp_CancelTransaction(transactionId, Remark, GlobalFunction.GetLoginDetail(this.HttpContext).User_ID);

            return RedirectToAction("CancelComplete", new { id = PointAccountId });
        }

        public ActionResult CancelComplete(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        //Controller (Action)
        public ActionResult TestJqueryValid(string value)
        {
            var result = true;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Cancel Transaction
    }
}