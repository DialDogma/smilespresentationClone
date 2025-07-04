using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using SmileSCustomerBase.DataCenterPersonService;
using SmileSCustomerBase.Helper;
using SmileSCustomerBase.Models;
using SmileSCustomerBase.SSSDocumentService;

namespace SmileSCustomerBase.Controllers
{
    [Authorization]
    public class CustomerBaseP2Controller:Controller
    {
        #region dbContext

        private CustomerBaseDBContext _context;

        public CustomerBaseP2Controller()

        {
            _context = new CustomerBaseDBContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion dbContext

        #region QueueManagerMO

        public ActionResult QueueManagerMO()
        {
            //get user detail
            var userBranch = GlobalFunction.GetLoginDetail(HttpContext).Branch_ID;
            ViewBag.UserBranch = userBranch;

            return View();
        }

        [HttpPost]
        public ActionResult QueueManagerMO(FormCollection form)
        {
            return View();
        }

        public ActionResult GetQueueManagerMO(string appId,string cusName,string payerName,int? payerBranchId,
            int? draw = null,int? pageSize = null,int? pageStart = null,string sortField = null,
            string orderType = null,string search = null)
        {
            //mock up
            //payerBranchId = 60;
            appId = appId.Trim();

            //get list of queue
            var result = _context.usp_CleanCustomerTrackAccountList_SelectV2(payerBranchId,appId,cusName,payerName,
                pageStart,pageSize,sortField,orderType,search).ToList();

            var dt = new Dictionary<string,object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetQueueMonitorMO()
        {
            var userBranch = GlobalFunction.GetLoginDetail(HttpContext).Branch_ID;
            //mock up
            //var userBranch = 60;
            var branchResult = _context.usp_CleanCustomerTrackAccountMonitor_SelectV2(userBranch,0,null,null,null,null).FirstOrDefault();
            return Json(branchResult,JsonRequestBehavior.AllowGet);
        }

        #endregion QueueManagerMO

        #region DetailMO

        public ActionResult DetailMO(int queueId)
        {
            //call app detail
            ViewBag.QueueId = queueId;
            var appDetail = _context.usp_CleanCustomerTrackAccount_SelectV2(queueId).FirstOrDefault();
            if(appDetail != null)
            {
                //app info
                ViewBag.CleanCustomerId = appDetail.CleanCustomerTrackAccountId;
                ViewBag.StickerNumber = appDetail.ApplicationCode;
                ViewBag.AppId = appDetail.ApplicationCode;
                ViewBag.Plan = appDetail.Product;
                ViewBag.Status = appDetail.AppStatus;
                ViewBag.PhoneNum = appDetail.CustPhoneNo;
                ViewBag.PayerPhoneNum = appDetail.PayerPhoneNo;
                ViewBag.PlanId = appDetail.Product;
                //ViewBag.queueType = appDetail.QueueTypeId;

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
                ViewBag.CusNationId = appDetail.To_CustNationalityId;
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
                ViewBag.PayerNationId = appDetail.To_PayerNationalityId;

                //status type
                ViewBag.StatusTypeCustomer = appDetail.CustCheckStatusGroupId;
                ViewBag.StatusTypePayer = appDetail.PayerCheckStatusGroupId;
                //track status type
                ViewBag.CusTrackStatus = appDetail.TrackAccountStatusId;
                ViewBag.IsShowTrackAccount = appDetail.IsShowTrackAccount;
                ViewBag.Remark = appDetail.Remark;

                //track status type
                ViewBag.CusTrackStatus = appDetail.TrackAccountStatusId;
                ViewBag.IsShowTrackAccount = appDetail.IsShowTrackAccount;

                //check if disclosure
                var isDisclosure = appDetail.IsDisclosure;
                if(isDisclosure == true)
                {
                    var disclosureList = _context.usp_CustomerDisclosure_Select(appDetail.ApplicationCode).ToList();
                    ViewBag.Disclosure = disclosureList;
                }
            }

            //get status type customer
            ViewBag.StatusType = _context.usp_StatusGroup_Select(null).ToList();

            //get status type payer
            ViewBag.StatusTypePayerList = _context.usp_StatusGroup_Select(null).ToList();

            //get track account
            ViewBag.TrackStatus = _context.usp_TrackAccountStatus_Select(null).ToList();

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
        public ActionResult DetailMO(FormCollection form)
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
                CustNationalityId = Convert.ToInt32(form["hd_cusNation"]);
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
                PayerNationalityId = Convert.ToInt32(form["hd_PayerNation"]);
            }
            var PayerPassport = form["txtPayerPassport"];

            var PayerZcardId = "";
            var pZcardId = form["txtPayerIdCard"];

            if(pZcardId != null)
            {
                PayerZcardId = form["txtPayerIdCard"].Replace(@"-","");
            }

            int? PayerCheckStatusId = Convert.ToInt32(form["hd_PayerStatusType"]);
            //int hiddenPayerCheckStatusId = Convert.ToInt32(form["hd_PayerStatusType"]);
            int TrackStatusId = Convert.ToInt32(form["select_KtbResult"]);
            var Remark = form["txtRemark"];
            var createdByUserId = Convert.ToInt32(Session["User_ID"]);

            var disclosureIdList = form["hd_ArrCompany"].Split(',').ToList();
            var undisclosureIdList = form["hd_UnChkArrCompany"].Split(',').ToList();
            //put data to sp

            try
            {
                foreach(var itm in disclosureIdList)
                {
                    if(itm != "")
                    {
                        _context.usp_CustomerDisclosure_Update(Convert.ToInt32(itm),true,createdByUserId);
                    }
                }
                foreach(var itm in undisclosureIdList)
                {
                    if(itm != "")
                    {
                        _context.usp_CustomerDisclosure_Update(Convert.ToInt32(itm),false,createdByUserId);
                    }
                }

                var resultClean = _context.usp_CleanCustomerTrackAccount_UpdateV2(cleanCustomerId,CustTitleCode,CustFirstName,
               CustLastName,CustBirthDate,CustNationalityId,CustPassPort,CustZcardId,null,CustCheckStatusId,
               PayerTitleCode,PayerFirstName,PayerLastName,PayerBirthDate,PayerNationalityId,PayerPassport,
               PayerZcardId,null,PayerCheckStatusId,TrackStatusId,Remark,createdByUserId).FirstOrDefault();

                if((bool)resultClean.IsResult)
                {
                    var x = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
                    x.Clients.All.RefreshQueueManagerResult("คิวงานที่" + cleanCustomerId + "ได้ถูกอัพเดทแล้ว");
                    //}
                    return RedirectToAction("CloseTab","CustomerBase");
                }
                else
                {
                    var x = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
                    x.Clients.All.RefreshQueueManagerResult("คิวงานที่" + cleanCustomerId + "ไม่สามารถอัพเดทได้!");
                    //}
                    return RedirectToAction("CloseTab","CustomerBase");
                }
            }
            catch(Exception e)
            {
                return RedirectToAction("InternalServerError","Error",new { exception = e });
            }
        }

        #endregion DetailMO

        #region DetailNoClean

        public ActionResult DetailNoClean(int queueId)
        {
            //call app detail
            ViewBag.QueueId = queueId;
            var appDetail = _context.usp_CleanCustomerTrackAccount_SelectV2(queueId).FirstOrDefault();

            if(appDetail != null)
            {
                ViewBag.CleanCustomerId = appDetail.CleanCustomerTrackAccountId;
                ViewBag.StickerNumber = appDetail.ApplicationCode;
                ViewBag.AppId = appDetail.ApplicationCode;
                ViewBag.Plan = appDetail.Product;
                ViewBag.Status = appDetail.AppStatus;
                ViewBag.PhoneNum = appDetail.CustPhoneNo;
                ViewBag.PayerPhoneNum = appDetail.PayerPhoneNo;
                ViewBag.PlanId = appDetail.Product;
                //customer info edited
                ViewBag.Fname = appDetail.CustName;
                //Payer info edited
                ViewBag.PayerFname = appDetail.PayerName;
                ViewBag.Remark = appDetail.Remark;

                //track status type
                ViewBag.CusTrackStatus = appDetail.TrackAccountStatusId;
                ViewBag.IsShowTrackAccount = appDetail.IsShowTrackAccount;

                //check if disclosure
                var isDisclosure = appDetail.IsDisclosure;
                if(isDisclosure == true)
                {
                    var disclosureList = _context.usp_CustomerDisclosure_Select(appDetail.ApplicationCode).ToList();
                    ViewBag.Disclosure = disclosureList;
                }
            }

            //get track account
            ViewBag.TrackStatus = _context.usp_TrackAccountStatus_Select(null).ToList();

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
        public ActionResult DetailNoClean(FormCollection form)
        {
            //app info
            int cleanCustomerId = Convert.ToInt32(form["hd_CleanCustomerId"]);

            //update data
            int TrackStatusId = Convert.ToInt32(form["select_KtbResult"]);
            var Remark = form["txtRemark"];
            var createdByUserId = Convert.ToInt32(Session["User_ID"]);

            var disclosureIdList = form["hd_ArrCompany"].Split(',').ToList();
            var undisclosureIdList = form["hd_UnChkArrCompany"].Split(',').ToList();
            try
            {
                foreach(var itm in disclosureIdList)
                {
                    if(itm != "")
                    {
                        _context.usp_CustomerDisclosure_Update(Convert.ToInt32(itm),true,createdByUserId);
                    }
                }
                foreach(var itm in undisclosureIdList)
                {
                    if(itm != "")
                    {
                        _context.usp_CustomerDisclosure_Update(Convert.ToInt32(itm),false,createdByUserId);
                    }
                }

                var result = _context.usp_CleanCustomerTrackAccount_UpdateV2(cleanCustomerId,null,null,
                 null,null,null,null,null,null,null,
                 null,null,null,null,null,null,
                 null,null,null,TrackStatusId,Remark,createdByUserId).FirstOrDefault();

                if((bool)result.IsResult)
                {
                    var x = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
                    x.Clients.All.RefreshQueueManagerResult("คิวงานที่" + cleanCustomerId + "ได้ถูกอัพเดทแล้ว");
                    //}
                    return RedirectToAction("CloseTab","CustomerBase");
                }
                else
                {
                    var x = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
                    x.Clients.All.RefreshQueueManagerResult("คิวงานที่" + cleanCustomerId + "ไม่สามารถอัพเดทได้!");
                    //}
                    return RedirectToAction("CloseTab","CustomerBase");
                }
            }
            catch(Exception e)
            {
                return RedirectToAction("InternalServerError","Error",new { exception = e });
            }
        }

        #endregion DetailNoClean

        #region ReportP2

        public ActionResult ReportBranch()
        {
            //call branch
            ViewBag.BranchList = _context.usp_Branch_Select(null,null,99999,null,null,null).ToList();
            //var user = GlobalFunction.GetLoginDetail(HttpContext).UserName;
            var department = GlobalFunction.GetLoginDetail(HttpContext).Department_ID;
            if(department != 7)
            {
                ViewBag.BranchUser = GlobalFunction.GetLoginDetail(HttpContext).Branch_ID;
            }
            return View();
        }

        [HttpPost]
        public ActionResult ReportBranch(FormCollection form)
        {
            int? branchUser = null;
            if(form["ddlBranch"] != "")
            {
                branchUser = Convert.ToInt32(form["ddlBranch"]);
            }
            if(branchUser == 0)
            {
                branchUser = Convert.ToInt32(form["hd_Branch"]);
            }
            try
            {
                this._context.Database.CommandTimeout = 180;
                var result = _context.usp_ReportCleanCustomerTrackAccount_Select(branchUser,null,999999,null,null,null).ToList();

                var dt1 = GlobalFunction.ToDataTable(result);
                ExcelManager.ExportToExcel(this.HttpContext,"Report_Branch_CustomerBase",dt1,"Report_Branch_CustomerBase");

                return RedirectToAction("ReportBranch");
            }
            catch(Exception e)
            {
                return RedirectToAction("InternalServerError","Error",new { exception = e.Message });
            }
        }

        #endregion ReportP2

        #region ReportExcel

        public ActionResult ReportExcel()
        {
            //call branch
            ViewBag.BranchList = _context.usp_Branch_Select(null,null,99999,null,null,null).ToList();
            return View();
        }

        public JsonResult DownloadSummaryReport()
        {
            try
            {
                var result = _context.usp_Report_QueueClosed_Summary_select().ToList();
                var dt = GlobalFunction.ToDataTable(result);
                ExcelManager.ExportToExcel(HttpContext,"QueueCloseSummaryReport",dt,"sheet1");

                return Json(true,JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(false,JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DownloadBranchReport(int branchId)
        {
            try
            {
                var result = _context.usp_Report_QueueClosed_Detail_select(branchId).ToList();
                var dt = GlobalFunction.ToDataTable(result);
                ExcelManager.ExportToExcel(HttpContext,"QueueCloseSummaryReport",dt,"sheet1");

                return Json(true,JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(false,JsonRequestBehavior.AllowGet);
            }

            return Json(true,JsonRequestBehavior.AllowGet);
        }

        #endregion ReportExcel
    }
}