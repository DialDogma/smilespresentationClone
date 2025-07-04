using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmileSBillPayment.DataCenterAddressService;
using SmileSBillPayment.Helper;
using SmileSBillPayment.Models;

namespace SmileSBillPayment.Controllers
{
    [Authorization]
    public class RefundPaymentController : Controller
    {
        #region Context

        private readonly SSSCashReceiveEntities _context;

        public RefundPaymentController()
        {
            _context = new SSSCashReceiveEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Context

        #region View

        [Authorization(Roles = "Billpayment_Refund,Developer")]
        public ActionResult RequestRefund()
        {
            using (var client = new AddressServiceClient())
            {
                var provinceList = client.GetProvince(null).ToList();
                ViewBag.provinceList = provinceList;
            }

            var organizeList = _context.usp_Organize_Select(null, null, 999, null, null, null).ToList();
            ViewBag.organizeList = organizeList;

            var refundCauseList = _context.usp_RefundCause_Select(null, null, 999, null, null, null).ToList();
            ViewBag.refundCauseList = refundCauseList;

            return View();
        }

        [Authorization(Roles = "Billpayment_Refund,Developer")]
        public ActionResult ApproveRefund()
        {
            var refundType = _context.usp_RefundApproveStatus_Select(null, null, 99, null, null, null).ToList();
            ViewBag.refundType = refundType;
            var rejectTypeList = _context.usp_RefundUnApproveCause_Select(null, null, 99, null, null, null).ToList();
            ViewBag.rejectTypeList = rejectTypeList;

            return View();
        }

        [Authorization(Roles = "Billpayment_Refund,Developer")]
        public ActionResult RefundReport()
        {
            return View();
        }

        [Authorization(Roles = "Billpayment_Premium,Developer")]
        public ActionResult TransferFund()
        {
            var transferCauseList = _context.usp_TransferCause_Select(null, null, 99, null, null, null).ToList();
            ViewBag.transferCauseList = transferCauseList;

            return View();
        }

        #endregion View

        #region api

        #region datatable

        [HttpGet]
        public ActionResult GetRefundMonitor(int? refundRequestId, string refundRequestCode, string refundTypeIdList, string refundApproveStatusIdList
            , int draw, int? indexStart, int? pageSize, string sortField, string orderType, string search)
        {
            try
            {
                if (refundApproveStatusIdList == "-1")
                {
                    refundApproveStatusIdList = null;
                }

                var userId = GlobalFunction.GetLoginDetail(HttpContext).User_ID;

                var result = _context.usp_RefundRequest_Select(refundRequestId, refundRequestCode, refundTypeIdList, refundApproveStatusIdList, userId,
                    indexStart, pageSize, sortField, orderType, search).ToList();

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
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpGet]
        public ActionResult GetDocument(int refundRequestId, int draw, int? indexStart, int? pageSize, string sortField, string orderType, string search)
        {
            try
            {
                var result = _context.usp_RefundRequestDocument_Select(refundRequestId, indexStart, pageSize, sortField, orderType, search).ToList();
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
                throw new Exception(e.Message, e.InnerException);
            }
        }

        #endregion datatable

        #region function

        [HttpGet]
        public ActionResult GetApplicationDetail(string applicationCode)
        {
            var result = _context.usp_DebtAppDetail_Select(applicationCode, null, 1, null, null, null).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetRefundDetail(int refundRequestId, string refundRequestCode)
        {
            try
            {
                var userId = Helper.GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _context.usp_RefundRequest_Select(refundRequestId, refundRequestCode, null, null, userId, null, null, null,
                    null, null).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpPost]
        public ActionResult RequestRefund_Insert(int reserveAccountId, string applicationCode)
        {
            var createdByUserId = Helper.GlobalFunction.GetLoginDetail(HttpContext).User_ID;
            var result = _context.usp_RefundRequest_Insert(reserveAccountId, applicationCode, createdByUserId).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RequestRefund_Update(int? refundRequestId, int? refundTypeId, string chequeName, string chequeAddress1, string subDistrict_ID,
            int? refundCauseId, int? bankId, string bankAccount, string bankAccountName, double amount, string requestRemark)
        {
            try
            {
                if (bankId == -1)
                {
                    bankId = null;
                }

                if (refundTypeId == 4)
                {
                    subDistrict_ID = null;
                }

                var updateByUserId = Helper.GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _context.usp_RefundRequest_Update(refundRequestId,
                                                               updateByUserId,
                                                               refundTypeId,
                                                               chequeName,
                                                               chequeAddress1,
                                                               subDistrict_ID,
                                                               refundCauseId,
                                                               bankId,
                                                               bankAccount,
                                                               bankAccountName,
                                                               amount,
                                                               requestRemark).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpPost]
        public ActionResult ApproveRefund(int refundRequestId, int refundApproveStatusId, int? refundUnApproveCauseId, string approveRemark)
        {
            try
            {
                var approveByUserId = Helper.GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _context.usp_RefundRequestApprove_Update(refundRequestId, refundApproveStatusId, refundUnApproveCauseId, approveRemark,
                    approveByUserId).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public void ExportExcelReport(string dateFrom, string dateTo)
        {
            try
            {
                var dtFromConvert = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", new CultureInfo("en-Us"));
                var dtToConvert = DateTime.ParseExact(dateTo, "dd-MM-yyyy", new CultureInfo("en-Us"));

                var result = _context.usp_RefundRequestForReport_Select(dtFromConvert, dtToConvert).ToList();

                GlobalFunction.ExportDatatableToExcel(HttpContext, result, "sheet1", "RefundReport_" + DateTime.Now);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpGet]
        public ActionResult GetDistrict(int? provinceId)
        {
            try
            {
                using (var client = new AddressServiceClient())
                {
                    var districtList = client.GetDistrict(provinceId).ToList();
                    return Json(districtList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpGet]
        public ActionResult GetSubDistrict(int? districtId)
        {
            try
            {
                using (var client = new AddressServiceClient())
                {
                    var subDistrictList = client.GetSubDistrict(districtId).ToList();
                    return Json(subDistrictList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpGet]
        public ActionResult GetPostCode(int subDistrictId)
        {
            try
            {
                using (var client = new AddressServiceClient())
                {
                    var postCode = client.GetZipCodeBySubDistrictID(subDistrictId).ZipCode;
                    return Json(postCode, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpGet]
        public ActionResult GetAccountSearch(string search)
        {
            try
            {
                var result = _context.usp_ReserveAccountForAutoComplete_Select(null, null, null, search, null, 99, null, null, null).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        [HttpPost]
        public ActionResult ReserveAccountTransfer(int reserveAccountIdFrom, int? reserveAccountIdTo, double transferAmount,
            int transferCauseId, string transferRemark)
        {
            try
            {
                var createdByUserId = Helper.GlobalFunction.GetLoginDetail(HttpContext).User_ID;
                var result = _context.usp_ReserveAccountTransfer_Update(reserveAccountIdFrom, reserveAccountIdTo, transferAmount,
                    transferCauseId, transferRemark, createdByUserId).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        #endregion function

        #endregion api
    }
}