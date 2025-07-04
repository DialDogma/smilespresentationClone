using SmileSUnderwrite.Helper;
using SmileSUnderwrite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SmileSUnderwrite.DatacenterOrganizeService;
using OfficeOpenXml;
using System.Web;
using System.IO;

namespace SmileSUnderwrite.Controllers
{
    [Authorization]
    public class ImportController : Controller
    {
        #region dbContext

        private UnderwriteDBContext _context;
        private SSSPAEntities _db;

        public ImportController()

        {
            _context = new UnderwriteDBContext();
            _db = new SSSPAEntities();
        }

        string CheckNull(object value)
        {
            if (value != null)
            {
                return value.ToString();
            }
            else
            {
                return "";
            }            
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion dbContext

        // GET: Import
        public ActionResult Index()
        {
            return View();
        }

        #region "Import"

        [HttpGet]
        public ActionResult ImportQueue()
        {
            ViewBag.QueueGroup = _context.usp_QueueGroup_Select().ToList();
            ViewBag.PaYear = _db.vw_CodeGroup_PAYear.OrderByDescending(x => new { x.Code}).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult ImportQueue(FormCollection form, HttpPostedFileBase file)
        {

            int? queueType = Convert.ToInt32(form["ddlQueueType"]);

            int? queueGroup = Convert.ToInt32(form["ddlQueueGroup"]);

            int? year = Convert.ToInt32(form["ddlYear"]);

            int userID;
            int appcount = 0;

            //userID = 228;

            userID = Convert.ToInt32(Session["User_ID"]);

            var lst = new List<QueueImportRow>();


            if (file != null && file.ContentLength > 0)
            {

                if (Path.GetExtension(file.FileName) != ".xlsx")
                {
                    ViewBag.QueueGroup = _context.usp_QueueGroup_Select().ToList();
                    ViewBag.PaYear = _db.vw_CodeGroup_PAYear.OrderByDescending(x => new { x.Code }).ToList();
                    ViewBag.Message = "กรุณาเลือก Excel file";
                    return View();
                }
                else if (queueGroup == null || queueGroup == -1)
                {
                    ViewBag.QueueGroup = _context.usp_QueueGroup_Select().ToList();
                    ViewBag.PaYear = _db.vw_CodeGroup_PAYear.OrderByDescending(x => new { x.Code }).ToList();
                    ViewBag.Message = "กรุณาเลือกกลุ่มคิว";
                    return View();
                }
                else if (queueType == null || queueType == -1)
                {
                    ViewBag.QueueGroup = _context.usp_QueueGroup_Select().ToList();
                    ViewBag.PaYear = _db.vw_CodeGroup_PAYear.OrderByDescending(x => new { x.Code }).ToList();
                    ViewBag.Message = "กรุณาเลือกประเภทคิว";
                    return View();
                }
                else if (year == null || year == -1)
                {
                    ViewBag.QueueGroup = _context.usp_QueueGroup_Select().ToList();
                    ViewBag.PaYear = _db.vw_CodeGroup_PAYear.OrderByDescending(x => new { x.Code }).ToList();
                    ViewBag.Message = "กรุณาเลือกปีการศึกษา";
                    return View();

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
                                var item = new QueueImportRow()
                                {
                                    CreatedByEmpCode = ws.Cells[ri, 1].Value.ToString(),
                                    AssignToEmpCode = ws.Cells[ri, 2].Value.ToString(),
                                    RefCode = ws.Cells[ri, 3].Value.ToString(),
                                    Detail01 = CheckNull(ws.Cells[ri, 4].Value),
                                    Detail02 = CheckNull(ws.Cells[ri, 5].Value),
                                    Detail03 = CheckNull(ws.Cells[ri, 6].Value),
                                    Detail04 = CheckNull(ws.Cells[ri, 7].Value),
                                    Detail05 = CheckNull(ws.Cells[ri, 8].Value),
                                    Detail06 = CheckNull(ws.Cells[ri, 9].Value),
                                    Detail07 = CheckNull(ws.Cells[ri, 10].Value),
                                    Detail08 = CheckNull(ws.Cells[ri, 11].Value),
                                    Detail09 = CheckNull(ws.Cells[ri, 12].Value),
                                    Detail10 = CheckNull(ws.Cells[ri, 13].Value)

                                };

                                // Loop Dupicate RefCode
                                foreach (var itm in lst)
                                {
                                    if (itm.RefCode == ws.Cells[ri, 3].Value.ToString())
                                    {
                                        appcount += 1;
                                    }
                                }


                                if (appcount > 0)
                                {
                                    ViewBag.QueueGroup = _context.usp_QueueGroup_Select().ToList();
                                    ViewBag.PaYear = _db.vw_CodeGroup_PAYear.OrderByDescending(x => new { x.Code }).ToList();
                                    ViewBag.Message = "RefCode ซ้ำ รบกวนตรวจสอบ";

                                    return View();
                                }
                                else
                                {
                                    lst.Add(item);
                                }
                            }

                            //For each หารหัสโรงเรียน App ซ้ำ
                            //foreach (var itm in lst)
                            //{

                            //}

                        }
                        catch (Exception)
                        {
                            ViewBag.QueueGroup = _context.usp_QueueGroup_Select().ToList();
                            ViewBag.PaYear = _db.vw_CodeGroup_PAYear.OrderByDescending(x => new { x.Code }).ToList();
                            ViewBag.Message = "รูปแบบไฟล์ไม่ถูกต้อง";

                            return View();
                        }
                    }
                }

            }
            else if (queueGroup == null || queueGroup == -1)
            {
                ViewBag.QueueGroup = _context.usp_QueueGroup_Select().ToList();
                ViewBag.PaYear = _db.vw_CodeGroup_PAYear.OrderByDescending(x => new { x.Code }).ToList();
                ViewBag.Message = "กรุณาเลือกกลุ่มคิว";
                return View();
            }
            else
            {
                ViewBag.QueueGroup = _context.usp_QueueGroup_Select().ToList();
                ViewBag.PaYear = _db.vw_CodeGroup_PAYear.OrderByDescending(x => new { x.Code }).ToList();
                ViewBag.Message = "กรุณาเลือกไฟล์ที่ต้องการ Import";
                return View();
            }

            var header =  _context.usp_ImportQueueHeader_Insert(queueType,year, userID).FirstOrDefault();

            //Save Detail
            foreach (var item in lst)
            {
                var queueDetail = new ImportQueueDetail()
                {
                    ImportQueueHeaderId = header,
                    CreatedByEmpCode = item.CreatedByEmpCode,
                    AssignToEmpCode = item.AssignToEmpCode,
                    RefCode = item.RefCode,
                    Detail01 = item.Detail01,
                    Detail02 = item.Detail02,
                    Detail03 = item.Detail03,
                    Detail04 = item.Detail04,
                    Detail05 = item.Detail05,
                    Detail06 = item.Detail06,
                    Detail07 = item.Detail07,
                    Detail08 = item.Detail08,
                    Detail09 = item.Detail09,
                    Detail10 = item.Detail10
                };

                _context.ImportQueueDetails.Add(queueDetail);
                _context.SaveChanges();
            }

            return RedirectToAction("ImportQueueSubmit", new { id = header });
        }


        public ActionResult ImportQueueSubmit(int id)
        {

            ViewBag.ImportQueueHeaderID = id;

            ViewBag.IsResult = _context.usp_ImportQueueValidate_Select(id).Single().IsResult;

            return View();
        }

        [HttpPost]
        public JsonResult ConfirmImportQueue(int importQueueHeaderId)
        {
            try
            {
                int userID;

                //userID = 228;

                userID = Convert.ToInt32(Session["User_ID"]);

                var isResult = _context.usp_ImportQueueSubmit(importQueueHeaderId, userID).Single().IsResult;

                return Json(isResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public JsonResult GetImportQueueDetail(int? draw = null, int? pageSize = null, int? pageStart = null, string sortField = null, string orderType = null, string search = null,
            int? importQueueHeaderId = null)
        {

            var result = _context.usp_ImportQueueOverView_Select(importQueueHeaderId,pageStart,pageSize,sortField,orderType,search).ToList();



            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

       //[HttpPost]
        public ActionResult ExportExcel(int? queueType)
        {
            int QueueTypeId = Convert.ToInt32(queueType);


                var result = _context.usp_ImportQueueTemplate_Select(QueueTypeId).ToList();

                ExcelManager.ExportToExcel(this.HttpContext, result, "Template", "Template");

                return View();
        }

        public ActionResult ExportToExcelByImportQueueHeaderID(int ImportQueueHeaderID)
        {


            var result = _context.usp_ImportQueueOverViewResult_Select(ImportQueueHeaderID).ToList();

            ExcelManager.ExportToExcel(this.HttpContext, result, "Report_ImportQuueDetail", "Report_ImportQuueDetail");


            return View();
        }


        public ActionResult GetQueueTypesByQueueGroup(int queueGroupId)
        {

            var result = _context.usp_QueueTypeV1_Select(queueGroupId).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        #endregion "Import"
    }
}