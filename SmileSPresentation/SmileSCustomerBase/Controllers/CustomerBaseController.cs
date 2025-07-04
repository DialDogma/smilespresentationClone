using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Ajax.Utilities;
using SmileSCustomerBase.DataCenterPersonService;
using SmileSCustomerBase.Helper;
using SmileSCustomerBase.Models;
using SmileSCustomerBase.SSSDocumentService;
using SmileSCustomerBase.EmployeeService;
using Microsoft.AspNet.SignalR;

namespace SmileSCustomerBase.Controllers
{
    [Authorization]
    public class CustomerBaseController:Controller
    {
        #region dbContext

        private CustomerBaseDBContext _context;

        public CustomerBaseController()

        {
            _context = new CustomerBaseDBContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion dbContext

        // GET: CustomerBase
        public ActionResult QueueManager()
        {
            //int userId;

            //userId = 44;

            //usp_QueueMonitor_Select_Result obj = new usp_QueueMonitor_Select_Result();

            //obj = _context.usp_QueueMonitor_Select(null,null,userId,null,null).FirstOrDefault();

            //ViewBag.QueueAll = obj.ALL;
            //ViewBag.WaitQueue = obj.รอดำเนินการ + obj.คิวงานใหม่;
            //ViewBag.ClosedQueue = obj.ปิดคิวงาน;
            //@ViewBag.NewQueue = obj.คิวงานใหม่;

            return View();
        }

        //[HttpGet]
        public JsonResult GetSummary()
        {
            int userId;

            userId = Convert.ToInt32(Session["User_ID"]);

            //usp_ obj = new usp_QueueMonitor_Select_Result();

            usp_QueueMonitorByUserId_SelectV2_Result obj = new usp_QueueMonitorByUserId_SelectV2_Result();

            obj = _context.usp_QueueMonitorByUserId_SelectV2(null,userId).Single();

            return Json(obj,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSumAssign()
        {
            var result = _context.usp_QueueTypeMonitor_Select(3).FirstOrDefault();

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        public ActionResult Assign()
        {
            //var em = new EmployeeServiceClient();

            //ViewBag.Assign = em.GetEmployeeSelectResults(null, null, null, null, 5000, 0, true, null, null, null, null, null, null).Select(x => new { x.UserId , x.FirstName,x.EmployeeCode } ).ToList();

            //var obj = new usp_QueueTypeMonitor_Select_Result();

            //get queue type
            ViewBag.QueueType = _context.usp_QueueType_Select().ToList();
            //get result
            var result = _context.usp_QueueTypeMonitorAll_Select(null).FirstOrDefault();
            //get queueAll
            ViewBag.QueueAll = result.ALL;
            ViewBag.QueueUnAssign = _context.usp_UnAssignQueue_Select(null).FirstOrDefault();

            ViewBag.WaitQueue = result.รอดำเนินการ;
            ViewBag.ClosedQueue = result.ปิดคิวงาน;
            ViewBag.NewQueue = result.คิวงานใหม่;

            return View();
        }

        //[HttpPost]
        //[HttpGet]
        public ActionResult GetEmployee(string q)
        {
            using(var client = new EmployeeServiceClient())
            {
                var result = client.GetEmployeeSearch(q).ToList();

                return Json(result,JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetUserIdByEmployeeCode(string empCode)
        {
            using(var client = new EmployeeServiceClient())
            {
                var result = client.GetUserIDByEmpCode(empCode);

                return Json(result,JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AssignQueue(int amountQueue,int assignUser,int? queueType)
        {
            var userLogin = Convert.ToInt32(Session["User_ID"]);
            if(queueType == -1)
            {
                queueType = null;
            }
            try
            {
                if(assignUser != 0)
                {
                    _context.usp_AssignQueueList(amountQueue,queueType,2,assignUser,userLogin);

                    return Json(true,JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false,JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception e)
            {
                return Json(false,JsonRequestBehavior.AllowGet);
            }
        }

        #region "API QueueManager"

        public JsonResult GetDatatableQueueDetail(int? draw = null,int? pageSize = null,int? pageStart = null,string sortField = null,string orderType = null,string search = null,string appID = null,string custName = null,string payerName = null)
        {
            int userID;

            userID = Convert.ToInt32(Session["User_ID"]);

            var result = _context.usp_Queue_Select(null,null,null,1,userID,null,null,null,null
                ,pageStart,pageSize,sortField,orderType,search,appID,null,null,custName,payerName).ToList();

            var dt = new Dictionary<string,object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDatatableQueue(int? queueType,int? draw = null,int? pageSize = null,int? pageStart = null,string sortField = null,string orderType = null,string search = null)
        {
            if(queueType == -1)
            {
                queueType = null;
            }
            var result = _context.usp_QueueMonitor_Select(queueType,null,null,null,null,pageStart,pageSize,sortField,orderType,search).ToList();

            var dt = new Dictionary<string,object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataForChartResult(int? queueType)
        {
            if(queueType == -1)
            {
                queueType = null;
            }
            var result = _context.usp_QueueTypeMonitorAll_Select(queueType).FirstOrDefault();

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUnAssignQueueResult(int? queueType)
        {
            if(queueType == -1)
            {
                queueType = null;
            }
            var result = _context.usp_UnAssignQueue_Select(queueType).FirstOrDefault();

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        #endregion "API QueueManager"

        #region Detail

        [HttpGet]
        public ActionResult Detail(int queueId)
        {
            //call app detail
            ViewBag.QueueId = queueId;
            var appDetail = _context.usp_QueueCleanCustomer_Select(queueId).FirstOrDefault();
            if(appDetail != null)
            {
                //app info
                ViewBag.CleanCustomerId = appDetail.CleanCustomerId;
                ViewBag.StickerNumber = appDetail.ApplicationCode;
                ViewBag.AppId = appDetail.ApplicationCode;
                ViewBag.Plan = appDetail.Product;
                ViewBag.Status = appDetail.Status;
                ViewBag.PhoneNum = appDetail.CustPhoneNo;
                ViewBag.PayerPhoneNum = appDetail.PayerPhoneNo;
                ViewBag.PlanId = appDetail.ProductCode;
                ViewBag.queueType = appDetail.QueueTypeId;

                //customer info edited
                ViewBag.CustomerTitle = appDetail.To_CustTitleCode;
                ViewBag.Fname = appDetail.To_CustFirstName;
                ViewBag.Lname = appDetail.To_CustLastName;
                if(appDetail.To_CustBirthDate != null)
                {
                    ViewBag.BirthDay = appDetail.To_CustBirthDate?.ToString("dd/MM/yyyy");
                }
                //nation of customer
                ViewBag.IdCard = appDetail.To_CustZcardId;
                ViewBag.Passport = appDetail.To_CustPassPort;
                //Payer info edited
                ViewBag.PayerTitle = appDetail.To_PayerTitleCode;
                ViewBag.PayerFname = appDetail.To_PayerFirstName;
                ViewBag.PayerLname = appDetail.To_PayerLastName;
                if(appDetail.To_PayerBirthDate != null)
                {
                    ViewBag.PayerBirthDate = appDetail.To_PayerBirthDate?.ToString("dd/MM/yyyy");
                }
                //nation of Payer
                ViewBag.PayerIdCard = appDetail.To_PayerZcardId;
                ViewBag.PayerPassport = appDetail.To_PayerPassport;

                //status type
                ViewBag.StatusTypeCustomer = appDetail.CustCheckStatusId;
                ViewBag.StatusTypePayer = appDetail.PayerCheckStatusId;
                ViewBag.Remark = appDetail.Remark;
            }

            //get status type customer
            ViewBag.StatusType = _context.usp_CheckStatus_SelectV2(appDetail.QueueTypeId,2).ToList();

            //get status type payer
            ViewBag.StatusTypePayerList = _context.usp_CheckStatus_SelectV2(appDetail.QueueTypeId,3).ToList();

            //get title
            using(var db = new PersonServiceClient())
            {
                var title = db.GetTitle(null,2).ToList();

                ViewBag.PersonTitle = title;
            }
            using(var docClient = new DocumentServiceClient())
            {
                if(appDetail != null)
                {
                    var docLink = docClient.GetDocumentV2(appDetail.ApplicationCode);

                    if(docLink != null)
                    {
                        ViewBag.DocLink = docLink.ToList();
                    }

                    if(docLink == null)
                    {
                        ViewBag.FirstDoc = null;
                    }
                    else
                    {
                        var FirstDoc = docLink;
                        ViewBag.FirstDoc = "http://49.231.178.252:81/sssdocfiles/pdfviewer/web/viewer.html?file=" + HttpUtility.UrlEncode(FirstDoc.FirstOrDefault().PathFullDoc);
                    }
                }
            }

            var sssdoc = _context.usp_SSSDocLink_Select(appDetail.ApplicationCode).SingleOrDefault();

            if(sssdoc == null)
            {
                ViewBag.SSSDoc = null;
            }
            else
            {
                ViewBag.SSSDoc = sssdoc.UrlLinkScanSSSDoc;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Detail(FormCollection form)
        {
            //app info
            int cleanCustomerId = Convert.ToInt32(form["hd_CleanCustomerId"]);
            var queueId = form["hd_queueId"];
            //Customer info
            var CustTitleCode = form["select_title"];
            if(CustTitleCode == null)
            {
                CustTitleCode = form["hd_CustTitle"];
            }
            var CustFirstName = form["txtFname"];
            var CustLastName = form["txtLname"];
            DateTime? CustBirthDate = null;
            if(form["CusBirthDate"] != "")
            {
                CustBirthDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(form["CusBirthDate"]).ToString());
            }
            int CustNationalityId = Convert.ToInt32(form["rd_Nation"]);

            if(CustNationalityId == 0)
            {
                CustNationalityId = 1;
            }

            var CustPassPort = form["txtPassport"];

            var cZcardID = form["txtIdCard"];

            if(cZcardID != null)
            {
                cZcardID = form["txtIdCard"].Replace(@"-","");
            }

            var CustZcardId = cZcardID;
            int CustCheckStatusId = Convert.ToInt32(form["hd_CustStatusType"]);
            //int hidddenCustCheckStatusId = Convert.ToInt32(form["hd_CustStatusType"]);
            //Payer info
            var PayerTitleCode = form["select_Payertitle"];
            if(PayerTitleCode == null)
            {
                PayerTitleCode = form["hd_PayerTitle"];
            }
            var PayerFirstName = form["txtPayerFname"];
            var PayerLastName = form["txtPayerLname"];
            DateTime? PayerBirthDate = null;
            //Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(form["PayerBirthDate"]).ToString());

            var dd = form["PayerBirthDate"];

            if(dd != "")
            {
                PayerBirthDate = GlobalFunction.ConvertDatePickerToDate_BE(dd);
            }
            int PayerNationalityId = Convert.ToInt32(form["rd_PayerNation"]);

            if(PayerNationalityId == 0)
            {
                PayerNationalityId = 1;
            }
            var PayerPassport = form["txtPayerPassport"];

            var PayerZcardId = "";
            var pZcardId = form["txtPayerIdCard"];

            if(pZcardId != null)
            {
                PayerZcardId = form["txtPayerIdCard"].Replace(@"-","");
            }

            int PayerCheckStatusId = Convert.ToInt32(form["hd_PayerStatusType"]);
            //int hiddenPayerCheckStatusId = Convert.ToInt32(form["hd_PayerStatusType"]);
            var Remark = form["txtRemark"];
            var CreatedByUserId = Convert.ToInt32(Session["User_ID"]);
            //check if status is eqal to hidden
            //if(hidddenCustCheckStatusId == 3 || hidddenCustCheckStatusId == 4)
            //{
            //    CustTitleCode = form["hd_CustTitle"];
            //    CustCheckStatusId = hidddenCustCheckStatusId;
            //}
            //if(hiddenPayerCheckStatusId == 3 || hiddenPayerCheckStatusId == 4)
            //{
            //    PayerTitleCode = form["hd_PayerTitle"];
            //    PayerCheckStatusId = hiddenPayerCheckStatusId;
            //}
            try
            {
                if(CustCheckStatusId == 0 || PayerCheckStatusId == 0)
                {
                    return RedirectToAction("InternalServerError","Error");
                }
                var result = _context.usp_CleanCustomer_Update(cleanCustomerId,queueId,CustTitleCode,
                    CustFirstName,CustLastName,CustBirthDate,CustNationalityId,CustPassPort,
                    CustZcardId,CustCheckStatusId,PayerTitleCode,PayerFirstName,PayerLastName,
                    PayerBirthDate,PayerNationalityId,PayerPassport,PayerZcardId,PayerCheckStatusId,
                    Remark,CreatedByUserId).FirstOrDefault();
                //return RedirectToAction("QueueManager","CustomerBase");

                //if ((CustCheckStatusId == 3 || CustCheckStatusId == 4) && (PayerCheckStatusId == 3 || PayerCheckStatusId == 4))
                //{
                var x = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
                x.Clients.All.RefreshQueueManagerResult("คิวงานที่" + form["hd_queueId"] + "ได้ถูกอัพเดทแล้ว");
                //}
                return View("CloseTab");
            }
            catch(Exception e)
            {
                return RedirectToAction("InternalServerError","Error",new { exception = e });
            }
        }

        #endregion Detail

        public ActionResult CloseTab()
        {
            return View();
        }

        #region Report

        public ActionResult SummaryQueueReport()
        {
            var totalCleanCustomer = _context.usp_QueueTypeMonitor_Select(null).Sum(x => x.ALL);
            ViewBag.TotalCleanCustomer = totalCleanCustomer;
            var totalPersonInProcess = _context.usp_QueueMonitor_Select(null,null,null,null,null,null,100000,null,null,null).ToList().Count;
            ViewBag.DateComplete = totalCleanCustomer / totalPersonInProcess;
            return View();
        }

        public JsonResult CallResultBranchReport(int? queueTypeId,int? branchId,int? assignToUserId,
               DateTime? createdDateFrom,DateTime? createdDateTo,int? indexStart,
               int? pageSize,string sortField,string orderType,string search,int? draw = null)
        {
            var result = _context.usp_QueueMonitor_Select(queueTypeId,branchId,assignToUserId,
                createdDateFrom,createdDateTo,indexStart,pageSize,sortField,orderType,search).ToList();

            var dt = new Dictionary<string,object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt,JsonRequestBehavior.AllowGet);
        }

        public JsonResult CallQueueTypeMonitor(int? queueTypeId)
        {
            var result = _context.usp_QueueTypeMonitor_Select(queueTypeId).ToList();
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ExportExcelReport(FormCollection form)
        {
            int? queueTypeId = null;
            int? branchId = null; ;
            var queueTypeIdvar = form["select_queueType"];
            var branchIdvar = form["select_branch"];
            if(!queueTypeIdvar.IsNullOrWhiteSpace())
            {
                queueTypeId = Convert.ToInt32(queueTypeIdvar);
            }

            if(!branchIdvar.IsNullOrWhiteSpace())
            {
                branchId = Convert.ToInt32(branchIdvar);
            }

            try
            {
                var result = _context.usp_QueueMonitor_Select(queueTypeId,branchId,null,null,null,0,9999999,null,null,null).Select(
               x => new
               {
                   ประเภทคิว = x.QueueType,
                   สาขา = x.Branch,
                   เจ้าของงาน = x.AssignToName,
                   คิวงานทั้งหมด = x.ALL,
                   รอดำเนินนการ = x.รอดำเนินการ,
                   คิวงานที่ปิดแล้ว = x.ปิดคิวงาน,
                   วันที่ดำเนินการล่าสุด = x.LastUpdate
               }).ToList();
                var result2 = _context.usp_QueueMonitorDetail_Select(queueTypeId,branchId,null).Select(
                    x => new
                    {
                        ประเภทคิว = x.QueueType,
                        สาขา = x.Branch,
                        รหัสเจ้าของงาน = x.AssignToCode,
                        เจ้าของงาน = x.AssignToName,
                        คิวงานทั้งหมด = x.ALL,
                        ยังไม่ได้ตรวจ_ผู้เอาประกัน = x.Cยังไม่ได้ตรวจ,
                        ผ่านมีเอกสารอ้างอิงในระบบ_ผู้เอาประกัน = x.Cผ่าน_มีเอกสารอ้างอิงในระบบ,
                        ผ่านเพิ่มเอกสารอ้างอิงในระบบ_ผู้เอาประกัน = x.Cผ่าน_เพิ่มเอกสารอ้างอิง,
                        ไม่ผ่านไม่มีเอกสารอ้างอิงในระบบ_ผู้เอาประกัน = x.Cไม่ผ่าน_ไม่มีเอกสารอ้างอิง,
                        ไม่ผ่านเอกสารอ้างอิงไม่ชัดเจน_ผู้เอาประกัน = x.Cไม่ผ่าน_เอกสารอ้างอิงไม่ชัดเจน,
                        ยังไม่ได้ตรวจ_ผู้ชำระเบี้ย = x.Pยังไม่ได้ตรวจ,
                        ผ่านมีเอกสารอ้างอิงในระบบ_ผู้ชำระเบี้ย = x.Pผ่าน_มีเอกสารอ้างอิงในระบบ,
                        ผ่านเพิ่มเอกสารอ้างอิงในระบบ_ผู้ชำระเบี้ย = x.Pผ่าน_เพิ่มเอกสารอ้างอิง,
                        ไม่ผ่านไม่มีเอกสารอ้างอิงในระบบ_ผู้ชำระเบี้ย = x.Pไม่ผ่าน_ไม่มีเอกสารอ้างอิง,
                        ไม่ผ่านเอกสารอ้างอิงไม่ชัดเจน_ผู้ชำระเบี้ย = x.Pไม่ผ่าน_เอกสารอ้างอิงไม่ชัดเจน,
                        วันที่ดำเนินการล่าสุด = x.LastUpdate
                    }).ToList();

                var dt1 = GlobalFunction.ToDataTable(result);
                var dt2 = GlobalFunction.ToDataTable(result2);

                ExcelManager.ExportToExcel(this.HttpContext,"Report_CustomerBase",dt1,"Report_CustomerBase1",dt2,"Report_CustomerBase2");
                return View("SummaryQueueReport");
            }
            catch(Exception e)
            {
                return RedirectToAction("InternalServerError","Error",new { exception = e.Message });
            }
        }

        #endregion Report

        #region ReturnQueue

        [HttpGet]
        public ActionResult ReturnQueue()
        {
            //get status type customer
            ViewBag.StatusType = _context.usp_CheckStatus_SelectV2(3,2).ToList();

            //get status type payer
            ViewBag.StatusTypePayerList = _context.usp_CheckStatus_SelectV2(3,3).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult ReturnQueue(FormCollection form)
        {
            //get status type customer
            ViewBag.StatusType = _context.usp_CheckStatus_SelectV2(3,2).ToList();

            //get status type payer
            ViewBag.StatusTypePayerList = _context.usp_CheckStatus_SelectV2(3,3).ToList();
            try
            {
                //get value
                var appId = form["hd_AppId"];
                bool? isCus = Convert.ToBoolean(form["chk_cusCheck"]);
                bool? isPayer = Convert.ToBoolean(form["chk_payerCheck"]);
                var cusCheckStatus = Convert.ToInt32(form["select_CusCheckStatus"]);
                var payerCheckStatus = Convert.ToInt32(form["select_PayerCheckStatus"]);
                //call sp
                var result = _context.usp_UndoQueue_Update(appId,isCus,cusCheckStatus,isPayer,payerCheckStatus).FirstOrDefault();
                if(result.Result == "Success")
                {
                    TempData["Success"] = "อัพโหลดไฟล์สำเร็จ";
                }
                else
                {
                    TempData["Fail"] = "" + result.Result;
                }

                return View();
            }
            catch(Exception e)
            {
                return RedirectToAction("InternalServerError","Error",new { exception = "ไม่สามารถแก้ไขรายการได้!! " + e });
            }
        }

        [HttpPost]
        public ActionResult GetAppDetail(string appId)
        {
            var appDetail = _context.usp_QueueCleanCustomer_ByApplicationCode_Select(appId).FirstOrDefault();
            if(appDetail == null)
            {
                return Json(false,JsonRequestBehavior.AllowGet);
            }
            //get value
            var cusName = appDetail.CustFirstName + " " + appDetail.CustLastName;
            var payName = appDetail.PayerFirstName + " " + appDetail.PayerLastName;
            var cusCheck = appDetail.CustCheckStatusId;
            var paycheck = appDetail.PayerCheckStatusId;

            var detailResult = new { cusName,payName,cusCheck,paycheck };

            return Json(detailResult,JsonRequestBehavior.AllowGet);
        }

        #endregion ReturnQueue
    }
}