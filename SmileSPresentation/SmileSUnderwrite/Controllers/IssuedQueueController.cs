using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSUnderwrite.DatacenterEmployeeService;
using SmileSUnderwrite.DatacenterOrganizeService;
using SmileSUnderwrite.Helper;
using SmileSUnderwrite.Models;
using SmileSUnderwrite.SmileSPAService;

namespace SmileSUnderwrite.Controllers
{
    [Authorization]
    public class IssuedQueueController:Controller
    {
        #region dbContext

        private UnderwriteDBContext _context;

        public IssuedQueueController()
        {
            _context = new UnderwriteDBContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion dbContext

        #region view

        // GET: IssuedQueue
        public ActionResult Index()
        {
            ViewBag.QueueType = _context.usp_QueueType_Select();

            var user = GlobalFunction.GetLoginDetail(HttpContext);
            var roleList = user.Roles.Split(',');

            //check role
            if(roleList.Contains("UnderwritePA_Manager") || roleList.Contains("Developer"))
            {
                using(var person = new EmployeeServiceClient())
                {
                    var empList = person.GetEmployeeSelectResults(null,null,null,null,
                        100,0,null,null,3,9,null,null,null).ToList();

                    empList.Insert(0,new usp_Employee_Select1_Result { UserId = -1,EmployeeCode = "---ทั้งหมด---" });

                    ViewBag.Emp = empList;
                }
            }
            else
            {
                using(var person = new EmployeeServiceClient())
                {
                    ViewBag.Emp = person.GetEmployeeSelectResults(user.UserName,null,null,null,
                        100,0,null,null,3,9,null,null,null).ToList();
                }
            }

            return View();
        }

        public ActionResult IssuedQueuePAUnderwriteCall(int queueId)
        {
            //insert doc
            _context.usp_DocumentByQueue_Insert(queueId,"26");
            //insert underwrite by queue id
            var underwrite = _context.usp_Underwrite_Insert(queueId,null).FirstOrDefault();
            ViewBag.QueueId = queueId;
            //get call detail id
            var underwriteDetail = _context.usp_Underwrite_Select(underwrite).FirstOrDefault();
            ViewBag.UnderwiteId = underwrite;
            //call other insurance
            using(var datacenter = new OrganizeServiceClient())
            {
                ViewBag.OtherInsurance = datacenter.GetOtherInsurance(null);
            }
            //get question id
            var queueDetail = _context.usp_Queue_Select(queueId,null,null,null,null,null,null,null,null,null,"","","",null).FirstOrDefault();
            ViewBag.QuestionId = queueDetail.QuestionId;
            ViewBag.QueueCreated = queueDetail.CreatedDate;
            ViewBag.QueueTypeDetail = queueDetail.QueueTypeDetail;
            var AppDetail = _context.usp_ApplicationDetail_Select(queueDetail.ReferenceCode).FirstOrDefault();
            ViewBag.AppDetail = AppDetail;
            var db = new ApplicationServiceClient();
            var appResult = db.GetApplicationDetailForPAUnderwrite(queueDetail.ReferenceCode);
            ViewBag.SchoolId = appResult.School_id;
            //call ref id
            ViewBag.AppId = appResult.App_id;
            //check if underwriteDetail is not null
            if(underwriteDetail != null)
            {
                //check if question id
                if(underwriteDetail.QuestionId == 2)
                {
                    var resultQ2 = _context.usp_UdwQ2_Select(underwrite).FirstOrDefault();

                    if(resultQ2 != null)
                    {
                        ViewBag.ResultQ2 = resultQ2;

                        if(resultQ2.A9 != null)
                        {
                            ViewBag.DatePayment = resultQ2.A9.Value.ToString("dd/MM/yyyy");
                        }
                    }
                }
                else if(underwriteDetail.QuestionId == 3)
                {
                    var resultQ3 = _context.usp_UdwQ3_Select(underwrite).FirstOrDefault();
                    if(resultQ3 != null)
                    {
                        ViewBag.ResultQ3 = resultQ3;
                    }
                }
                else if(underwriteDetail.QuestionId == 4)
                {
                    var resultQ4 = _context.usp_UdwQ4_Select(underwrite).FirstOrDefault();
                    if(resultQ4 != null)
                    {
                        ViewBag.ResultQ4 = resultQ4;
                        if(resultQ4.A31 != null)
                        {
                            ViewBag.DatePayment2 = resultQ4.A31.Value.ToString("dd/MM/yyyy");
                        }
                    }
                }
            }
            //check if have call
            var callSelect = _context.usp_Call_Select(underwrite,true).FirstOrDefault();
            if(callSelect != null)
            {
                ViewBag.CallDetail = callSelect;
            }

            //get document
            var docLink = _context.usp_Document_Select(null,queueId,null,"26",null,null,null,null,null).FirstOrDefault();
            ViewBag.ScanUrl = docLink.UrlLinkScan;
            ViewBag.OpenUrl = docLink.UrlLinkOpen;
            ViewBag.DocCount = docLink.FileCount;

            return View();
        }

        [HttpPost]
        public ActionResult IssuedQueuePAUnderwriteCallPost(FormCollection form)
        {
            //remark form
            var underwiteId = Convert.ToInt32(form["hd_underwriteId"]);
            var user = Convert.ToInt32(Session["User_ID"]);

            //check form
            bool isCheatValue = (form["chk_IsCheat"] == "on");
            var cheatRemark = form["txtRemarkCheck"];
            var cheatStatus = 2;
            if(isCheatValue)
            {
                cheatStatus = 4;
            }

            //insert issued sp
            var result = _context.usp_UnderwriteCorrupt_Update(underwiteId,cheatStatus,cheatRemark,user).FirstOrDefault();

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        #endregion view

        #region api

        /// <summary>
        ///
        /// </summary>
        /// <param name="queueTypeId"></param>
        /// <param name="empCode"></param>
        /// <param name="draw"></param>
        /// <param name="indexStart"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="orderType"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public ActionResult IssuedQueue_DT(int? queueTypeId,int? userId,int? draw,int? indexStart,int? pageSize,
            string sortField,string orderType,string search)
        {
            if(userId == -1)
            {
                userId = null;
            }
            if(queueTypeId == -1)
            {
                queueTypeId = null;
            }

            var result = _context.usp_QueueIssues_Select(userId,queueTypeId,indexStart,pageSize,sortField,orderType,search).ToList();

            var dt = new Dictionary<string,object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult RefreshDocCount(int queueId)
        {
            try
            {
                var docLink = _context.usp_Document_Select(null,queueId,null,"26",null,null,null,null,null).FirstOrDefault();

                return Json(docLink.FileCount,JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion api
    }
}