using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using SmileSCustomerBase.DataCenterPersonService;
using SmileSCustomerBase.Helper;
using SmileSCustomerBase.Models;
using SmileSCustomerBase.SSSDocumentService;
using SmileSCustomerBase.DatacenterEmployeeService;

namespace SmileSCustomerBase.Controllers
{
    [Authorization]
    public class AuditController:Controller
    {
        #region Context

        private CustomerBaseDBContext _context;

        public AuditController()

        {
            _context = new CustomerBaseDBContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Context

        #region Assign

        public ActionResult AuditAssign()
        {
            ViewBag.AuditCountCreated = _context.usp_AuditCountCreated_Select().SingleOrDefault();

            var obj = _context.usp_AuditCountAssign_Select().ToList();

            ViewBag.AuditAll = obj.Where(x => x.IsAssign == false).Select(x => x.All).Single();
            ViewBag.AuditUnCheck = obj.Where(x => x.IsAssign == true).Select(x => x.UnCheck).Single();
            ViewBag.AuditCheck = obj.Where(x => x.IsAssign == true).Select(x => x.Check).Single();

            ViewBag.AuditSummary = _context.usp_AuditCreatedSummary_Select().FirstOrDefault();

            return View();
        }

        public JsonResult GettableAuditQueueDetail(int? draw = null,int? pageSize = null,int? pageStart = null,string sortField = null,string orderType = null,string search = null)
        {
            int userID;

            userID = Convert.ToInt32(Session["User_ID"]);

            //var result = _context.usp_AuditMonitor_Select(pageStart,pageSize,sortField,orderType,search).ToList();

            var result = _context.usp_AuditMonitor_SelectV2(null,pageStart,pageSize,sortField,orderType,search).ToList();

            var dt = new Dictionary<string,object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GettableAuditHistory(int? draw = null,int? pageSize = null,int? pageStart = null,string sortField = null,string orderType = null,string search = null)
        {
            int userID;

            userID = Convert.ToInt32(Session["User_ID"]);

            //var result = _context.usp_AuditMonitor_Select(pageStart,pageSize,sortField,orderType,search).ToList();

            var result = _context.usp_AuditCreatedLog_Select(pageStart,pageSize,sortField,orderType,search).ToList();

            var dt = new Dictionary<string,object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt,JsonRequestBehavior.AllowGet);
        }

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
        public JsonResult SetRandomAudit(int percent)
        {
            int userID;

            userID = Convert.ToInt32(Session["User_ID"]);

            //userID = 1;
            try
            {
                _context.usp_AuditCreated_Insert(percent,userID).SingleOrDefault();

                return Json(true,JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public JsonResult SetAuditAssignList(int amountQueue,int assignTo)
        {
            int userID;

            userID = Convert.ToInt32(Session["User_ID"]);

            //userID = 1;
            try
            {
                _context.usp_AuditAssignList(amountQueue,assignTo).SingleOrDefault();

                return Json(true,JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public ActionResult ExportHistory(int? auditCreateId)
        {
            var result = _context.usp_ReportAppAuditCheckedDetail_Select(auditCreateId).Select(x => new
            {
                คร้งที่ = x.AuditCreatedId,
                AppId = x.ApplicationCode,
                เจ้าของงาน = x.CheckByName,
                รหัสเจ้าของงาน = x.CheckByEmployeeCode,
                สาขา = x.CheckByBranch,
                ประเภทคิว = x.QueueType,
                ผลการตรวจสอบ = x.AuditCheckStatus,
                ครั้งทื่สุ่มตรวจ = x.AuditCreatedId,
                c_คำนำหน้าชื่อ = x.IsFalseCustTitle,
                c_ชื่อสกุล = x.IsFalseCustName,
                c_วันเกิด = x.IsFalseCustBirthDate,
                c_เลขบัตรประชาชนPassport = x.IsFalseCustZcard_Passport,
                p_คำนำหน้าชื่อ = x.IsFalsePayerTitle,
                p_ชื่อสกุล = x.IsFalsePayerName,
                p_วันเกิด = x.IsFalsePayerBirthDate,
                p_เลขบัตรประชาชนPassport = x.IsFalsePayerZcard_Passport,
                ผู้ตรวจสอบ = x.AuditByName,
                วันที่ตรวจ = x.AuditCloseDate,
                วันที่สุ่ม = x.AuditCreatedDate
            }).ToList();
            var result2 = _context.usp_ReportAppAuditChecked_Select(auditCreateId).Select(x => new
            {
                ครั้งที่ = x.AuditCreatedId,
                รหัสAudit = x.AuditByUserId,
                ชื่อAudit = x.PersonName,
                คิวงานทั้งหมดจากการสุ่ม = x.AppTotal,
                คิวงานที่สุ่ม_เปอร์เซ็นต์ = x.AuditPercent,
                คิวงานที่ได้รับ = x.AppAudit,
                รอดำเนินการ = x.Pending,
                ตรวจสอบแล้ว = x.Checked,
                ถูกต้อง = x.True,
                ไม่ถูกต้อง = x.False
            }).ToList();

            var dt1 = GlobalFunction.ToDataTable(result);
            var dt2 = GlobalFunction.ToDataTable(result2);

            ExcelManager.ExportToExcel(this.HttpContext,"Report_HistoryAssign",dt1,"Report1",dt2,"Report2");

            return null;
        }

        #endregion Assign

        #region QueueManager

        public ActionResult AuditQueueManager()
        {
            //Audit by user
            var user = Convert.ToInt32(Session["User_ID"]);

            var totalResult = _context.usp_AuditMonitor_SelectV2(user,null,null,null,null,null).FirstOrDefault();
            ViewBag.TotalQueue = totalResult.All;
            ViewBag.WaitQueue = totalResult.UnCheck;
            ViewBag.DoneQueue = totalResult.Check;

            return View();
        }

        public JsonResult GetDataTableQueueList(int? auditByUserId,bool? auditIsCheck,
            int? indexStart,int? pageSize,string sortField = null,string orderType = null
            ,string searchDetail = null,string searchProduct = null,
            string searchApplication = null,string searchCustName = null,
            string searchPayerName = null,int? draw = null)
        {
            //Get session user id
            auditByUserId = Convert.ToInt32(Session["User_ID"]);
            var result = _context.usp_AuditList_Select(auditByUserId,auditIsCheck,indexStart,
                pageSize,sortField,orderType,searchDetail,searchProduct,searchApplication,searchCustName,
                searchPayerName).ToList();

            var dt = new Dictionary<string,object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetExportExcel()
        {
            //Get session user id
            int auditByUserId = Convert.ToInt32(Session["User_ID"]);
            var result = _context.usp_AuditList_Select(auditByUserId,null,null,9999,null,null,
                null,null,null,null,null).Select(x => new
                {
                    เลขสติ๊กเกอร์ = x.ApplicationCode,
                    แผน = x.Product,
                    สถานะ = x.Status,
                    ชื่อผู้เอาประกัน = x.CustName,
                    ชื่อผู้ชำระเบี้ย = x.PayerName,
                    ผลการตรวจสอบ = x.AuditCheckStatus,
                    รหัสเจ้าของงาน = x.CheckByCode,
                    ชื่อเจ้าของงาน = x.CheckByName
                }).ToList();

            var dt1 = GlobalFunction.ToDataTable(result);
            ExcelManager.ExportToExcel(this.HttpContext,"Audit_CustomerBase",dt1,"Audit_CustomerBase");

            return View("AuditQueueManager");
        }

        #endregion QueueManager

        #region Detail

        public ActionResult AuditDetail(int cleanCustomerId)
        {
            //call app detail
            ViewBag.QueueId = cleanCustomerId;

            //call queue detail

            var appDetail = _context.usp_AuditAppDetail_Select(cleanCustomerId).FirstOrDefault();

            if(appDetail != null)
            {
                //app info
                ViewBag.VerifyBy = appDetail.CheckByName;
                ViewBag.CleanCustomerId = appDetail.CleanCustomerId;
                ViewBag.StickerNumber = appDetail.ApplicationCode;
                ViewBag.AppId = appDetail.ApplicationCode;
                ViewBag.Plan = appDetail.Product;
                ViewBag.Status = appDetail.Status;
                ViewBag.PhoneNum = appDetail.CustPhoneNo;
                ViewBag.PayerPhoneNum = appDetail.PayerPhoneNo;
                ViewBag.PlanId = appDetail.ProductCode;

                //customer info edited
                ViewBag.CustomerTitle = appDetail.To_CustTitleCode;
                ViewBag.Fname = appDetail.To_CustFirstName;
                ViewBag.Lname = appDetail.To_CustLastName;
                if(appDetail.To_CustBirthDate != null)
                {
                    ViewBag.BirthDay = appDetail.To_CustBirthDate?.ToString("dd/MM/yyyy");
                }
                //nation of customer
                ViewBag.CusNation = appDetail.To_CustNationalityId;
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
                ViewBag.PayerNation = appDetail.To_PayerNationalityId;
                //status type
                ViewBag.StatusTypeCustomer = appDetail.CustCheckStatusId;
                ViewBag.StatusTypePayer = appDetail.PayerCheckStatusId;
                ViewBag.Remark = appDetail.Remark;

                if(appDetail.AuditCloseDate == null)
                {
                    ViewBag.IsAudit = "False";
                }
                else
                {
                    ViewBag.IsAudit = "True";
                    //call isfalse Customer
                    ViewBag.IsFalseCustitle = appDetail.IsFalseCustTitle;
                    ViewBag.IsFalseCusName = appDetail.IsFalseCustName;
                    ViewBag.IsFalseCusBirthDate = appDetail.IsFalseCustBirthDate;
                    ViewBag.IsFalseCusZCard_Passport = appDetail.IsFalseCustZcard_Passport;
                    //call isfalse payer
                    ViewBag.IsFalsePayertitle = appDetail.IsFalsePayerTitle;
                    ViewBag.IsFalsePayerName = appDetail.IsFalsePayertName;
                    ViewBag.IsFalsePayerBirthDate = appDetail.IsFalsePayerBirthDate;
                    ViewBag.IsFalsePayerZCard_Passport = appDetail.IsFalsePayerZcard_Passport;
                }

                ViewBag.AuditResult = appDetail.AuditCheckStatusId;
                ViewBag.AuditRemark = appDetail.AuditRemark;

                //get status type customer
                ViewBag.StatusType = _context.usp_CheckStatus_SelectV2(null,null).ToList();

                //get status type payer
                ViewBag.StatusTypePayerList = _context.usp_CheckStatus_SelectV2(null,null).ToList();

                //get title
                using(var db = new PersonServiceClient())
                {
                    var title = db.GetTitle(null,null).ToList();

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
            return View();
        }

        [HttpPost]
        public ActionResult AuditDetail(FormCollection form)
        {
            //customer form
            var cusTitle = form["select_title"];
            var cusFname = form["txtFname"];
            var cusLname = form["txtLname"];
            DateTime? CusBirthDate = null;
            if(form["CusBirthDate"] != "")
            {
                CusBirthDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(form["CusBirthDate"]).ToString());
            }
            int CusNationalityId = Convert.ToInt32(form["rd_Nation"]);
            if(CusNationalityId == 0)
            {
                CusNationalityId = 1;
            }
            var CusPassPort = form["txtPassport"];
            var CusZcardId = "";
            if(form["txtIdCard"] != null)
            {
                CusZcardId = form["txtIdCard"].Replace(@"-","");
            }

            //payer form
            var PayerTitleCode = form["select_Payertitle"];
            var PayerFirstName = form["txtPayerFname"];
            var PayerLastName = form["txtPayerLname"];
            DateTime? PayerBirthDate = null;
            //Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(form["PayerBirthDate"]).ToString());

            var dd = form["PayerBirthDate"];

            if(dd != "")
            {
                PayerBirthDate = Convert.ToDateTime(GlobalFunction.ConvertDatePickerToDate_BE(form["PayerBirthDate"]).ToString());
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

            //audit form
            var cleanCusId = Convert.ToInt32(form["hd_CleanCustomerId"]);
            var auditCheckstatus = Convert.ToInt32(form["rd_verify"]);
            var auditRemark = form["txtAuditRemark"];
            var user = Convert.ToInt32(Session["User_ID"]);
            //if cus is false
            bool falseCusTitle = form["chk_CusTitle"] == "1";
            bool falseCusName = form["chk_CusName"] == "1";
            bool falseCusBday = form["chk_CusBday"] == "1";
            bool falseCusZCardPassport = form["chk_CusZCard"] == "1";
            //if payer is false
            bool falsePayerTitle = form["chk_PayerTitle"] == "1";
            bool falsePayerName = form["chk_PayerName"] == "1";
            bool falsePayerBday = form["chk_PayerBday"] == "1";
            bool falsePayerZCardPassport = form["chk_PayerZCard"] == "1";
            var x = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
            try
            {
                var result = _context.usp_Audit_Update(cleanCusId,cusTitle,cusFname,cusLname,CusBirthDate,
                    CusNationalityId,CusPassPort,CusZcardId,PayerTitleCode,PayerFirstName,PayerLastName,
                    PayerBirthDate,PayerNationalityId,PayerPassport,PayerZcardId,auditCheckstatus,falseCusTitle,
                    falseCusName,falseCusBday,falseCusZCardPassport,falsePayerTitle,falsePayerName,falsePayerBday,
                    falsePayerZCardPassport,auditRemark,user);

                x.Clients.All.RefreshQueueManagerResult("คิวงานที่" + cleanCusId + "ได้ถูกอัพเดทแล้ว");

                return RedirectToAction("CloseTab","CustomerBase");
            }
            catch(Exception e)
            {
                return RedirectToAction("InternalServerError","Error",new { exception = e });
            }
        }

        #endregion Detail

        #region ReportCloseQueue

        public ActionResult ReportCloseQueue()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReportCloseQueue(FormCollection form)
        {
            var selectDate = form["dtpDateFrom"];
            var selectTime = form["time_in"];
            var user = Convert.ToInt32(Session["User_ID"]);
            var inputDateTime = GlobalFunction.ConvertDatePickerToDate_BE(selectDate,selectTime);

            try
            {
                var result = _context.usp_Wage_Insert(inputDateTime,user);
                return RedirectToAction("ReportCloseQueue");
            }
            catch(Exception e)
            {
                return RedirectToAction("InternalServerError","Error",new { Exception = e });
            }
        }

        public JsonResult GetReportCloseQueue(string draw,string search,int? indexStart,int? pageSize,string sortField,
            string orderType)
        {
            var result = _context.usp_Wage_Select(indexStart,pageSize,sortField,orderType,search).ToList();
            var dt = new Dictionary<string,object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt,JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportExcelCloseQueue(int wageId)
        {
            var result = _context.usp_WageHistory_Select(wageId).ToList();
            var dt1 = GlobalFunction.ToDataTable(result);
            ExcelManager.ExportToExcel(this.HttpContext,"Wage_History",dt1,"Wage_History");
            return null;
        }

        #endregion ReportCloseQueue
    }
}