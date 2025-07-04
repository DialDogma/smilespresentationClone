using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSPettyCash.DataCenterAddressService;
using SmileSPettyCash.Helper;
using SmileSPettyCash.Models;

namespace SmileSPettyCash.Controllers
{
    [Authorization(Roles = "Developer,PettyCash_ACC_01,PettyCash_ACC_02")]
    public class FinanceCheckerController:Controller
    {
        #region dbCon

        private readonly PettyCashEntities _context;
        private readonly AddressServiceClient _DCService;
        private static readonly CultureInfo _dtEn = new CultureInfo("en-US");
        private static readonly CultureInfo _dtTh = new CultureInfo("th-TH");

        public FinanceCheckerController()

        {
            _context = new PettyCashEntities();
            _DCService = new AddressServiceClient();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion dbCon

        #region View

        public ActionResult CheckTransactionManage()
        {
            return View();
        }

        public ActionResult PettyCashTransactionDetail(int hPettyCashId)
        {
            ViewBag.hPettyCashId = hPettyCashId;
            ViewBag.NotPass = _context.usp_UnPassCause_Select(null,null,null,null,null,null).ToList();

            return View();
        }

        //หน้าจอเช็ครายการตั้งเบิก
        public ActionResult CheckWithdrawalManage()
        {
            var branchList = _DCService.GetBranch(null).ToList();
            ViewBag.branchList = branchList;

            return View();
        }

        //หน้าจอรายละเอียดตั้งเบิก
        public ActionResult CheckWithdrawalDetail(int? withDrawalId)
        {
            var detailList = _context.usp_RequisitionHeader_Select(withDrawalId,null,null,null,99,null,null,null).FirstOrDefault();

            ViewBag.headerId = withDrawalId;
            ViewBag.branch = detailList.Branch;
            ViewBag.rqhCode = detailList.RequisitionHeaderCode;
            ViewBag.amount = detailList.Amount;

            ViewBag.NotPass = _context.usp_UnPassCause_Select(null,null,null,null,null,null).ToList();

            return View();
        }

        //หน้าจอแจ้งการโอนเงิน
        public ActionResult TransferAlert()
        {
            var bankList = _context.usp_Organize_Select(5,null,999,null,null,null).ToList();
            ViewBag.bankList = bankList;
            var branchList = _DCService.GetBranch(null).ToList();
            ViewBag.branchList = branchList;
            return View();
        }

        public ActionResult EditWithdrawalAmount()
        {
            var oldWithdrawAmount = _context.usp_ProgramConfig_PettyCash_WithdrawAmount_Select().FirstOrDefault();

            ViewBag.oldWithdrawAmount = oldWithdrawAmount.Value;

            return View();
        }

        #endregion View

        #region api

        [HttpGet]
        public JsonResult GetChecker_DT(int? draw = null,int? indexStart = null,int? pageSize = null,
            string sortField = null,string orderType = null,string search = null)
        {
            var result = _context.usp_PettyCashXPettyCashDaily_Select(null,2,indexStart,pageSize,sortField,
                orderType,search).ToList();

            var dt = new Dictionary<string,object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCheckerDetail_DT(int pcDailyId,int? draw = null,int? indexStart = null,int? pageSize = null,
            string sortField = null,string orderType = null,string search = null)
        {
            var result = _context.usp_PettyCashXPettyCashDailyDetail_Select(pcDailyId,indexStart,pageSize,sortField,
                orderType,search).ToList();

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
        public JsonResult GetPctDetail(int pctTranId)
        {
            var result = _context.usp_PettyCashTransactionById_Select(pctTranId).FirstOrDefault();

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetRequistitionDetail(int requisitionDetailId)
        {
            var result = _context.usp_RequisitionDetailById_Select(requisitionDetailId).FirstOrDefault();

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        //set transaction status 4-pass 5-not pass
        [HttpPost]
        public JsonResult SetTransactionStatus(int pctId,int pctStatusId,int? unPassCauseId,string unPassRemark = null)
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            //var user = 1;

            var result = _context.usp_PettyCashTransactionVerify_Update(pctId,pctStatusId,unPassCauseId,unPassRemark,user).FirstOrDefault();

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RequisitionDetailUpdateStatus(int requisitionDetailId,int requisitionDetailStatusId,int? unPassCauseId,string unPassRemark = null)
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            var result = _context.usp_RequisitionDetail_Update(requisitionDetailId,requisitionDetailStatusId,unPassCauseId,unPassRemark,user).FirstOrDefault();

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        //close day status set to 3
        [HttpPost]
        public JsonResult CloseHeaderTransaction(int hpctId)
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            //var user = 1;
            var result = _context.usp_PettyCashXPettyCashDaily_Update(hpctId,3,user).FirstOrDefault();

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        //check withdraw
        [HttpPost]
        public JsonResult GetWithdrawalCheckerByBranch(int? branchId,int requisitionHeaderStatusId,
            int? draw = null,int? indexStart = null,int? pageSize = null,
            string sortField = null,string orderType = null,string search = null)
        {
            if(branchId == 1)
            {
                branchId = null;
            }

            var result = _context.usp_RequisitionHeader_Select(null,branchId,requisitionHeaderStatusId,
                indexStart,pageSize,sortField,orderType,search).ToList();

            var dt = new Dictionary<string,object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt,JsonRequestBehavior.AllowGet);
        }

        //get withdraw detail
        [HttpPost]
        public JsonResult GetWithdrawDetail(int? headerId,
                                            int? organizeId,
                                            int? draw,
                                            int? indexStart = 0,
                                            int? pageSize = 10,
                                            string sortField = null,
                                            string orderType = null,
                                            string search = null)
        {
            var result = _context.usp_RequisitionDetail_Select(headerId,
                                                               organizeId,
                                                               indexStart,
                                                               pageSize,
                                                               sortField,
                                                               orderType,
                                                               search).ToList();

            var dt = new Dictionary<string,object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt,JsonRequestBehavior.AllowGet);
        }

        //get transfer datatable
        [HttpPost]
        public JsonResult GetTransfer_DT(int? branchId,bool? isScanDoc,bool? isTransfer,int? draw = null,int? indexStart = null,
            int? pageSize = null,string sortField = null,string orderType = null,string search = null)
        {
            if(branchId == 1)
            {
                branchId = null;
            }

            var result = _context.usp_Transfer_Select(branchId,isScanDoc,isTransfer,indexStart,pageSize,null,null,search).ToList();

            var dt = new Dictionary<string,object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault().TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            return Json(dt,JsonRequestBehavior.AllowGet);
        }

        //get tr detail
        [HttpGet]
        public JsonResult GetTransfer(int transferId,int? branchId)
        {
            if(branchId == 1)
            {
                branchId = null;
            }
            var result = _context.usp_Transfer_ById_Select(transferId).FirstOrDefault();

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// UPDATE REQUISITION HEADER
        /// </summary>
        /// <param name="requisitionHeaderId">headerId</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateRequisitionHeader(int? requisitionHeaderId)
        {
            var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            var result = _context.usp_RequisitionHeader_Approve_Update(requisitionHeaderId,user).FirstOrDefault();

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        //update transfer
        [HttpPost]
        public JsonResult UpdateTransfer(int transferId,int bankId,string transferDate,string bankAccNo,string bankAccName)
        {
            DateTime dateConvert = DateTime.ParseExact(transferDate,"dd/MM/yyyy HH:mm:ss",_dtTh);

            var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            var result = _context.usp_Transfer_Update(transferId,user,dateConvert,bankId,bankAccNo,bankAccName).FirstOrDefault();

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        //get transfer detail
        [HttpGet]
        public JsonResult GetTransferDetail_DT(int transferId,int? draw = null,int? indexStart = null,
            int? pageSize = null,string sortField = null,string orderType = null,string search = null)
        {
            var result = _context.usp_Payment_Select(transferId,null,indexStart,pageSize,sortField,orderType,search).ToList();

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
        public ActionResult GetTransferDetailById(int transferId)
        {
            var result = _context.usp_Transfer_ById_Select(transferId).FirstOrDefault();

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDocCount(string docCode)
        {
            var result = _context.usp_CountDocument(docCode).FirstOrDefault();
            if(result == null)
            {
                result = 0;
            }
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult WithdrawalAmount_Update(string amount)
        {
            try
            {
                var convertAmount = Convert.ToDouble(amount);
                var user = GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _context.usp_ProgramConfig_PettyCash_WithdrawAmount_Update(convertAmount,user);

                return Json(true,JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(false,JsonRequestBehavior.AllowGet);
            }
        }

        #endregion api
    }
}