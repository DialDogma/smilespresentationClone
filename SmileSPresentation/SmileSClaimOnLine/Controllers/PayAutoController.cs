using MassTransit;
using Newtonsoft.Json;
using PayTransferAPI.Contract;
using Serilog;
using SmileSClaimOnLine.Helper;
using SmileSClaimOnLine.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmileSClaimOnLine.Controllers
{
    public class PayAutoController : Controller
    {
        private const string _controllerName = nameof(PayAutoController);
        private ClaimOnLineDBContext _context;

        public PayAutoController()
        {
            _context = new ClaimOnLineDBContext();
        }

        public string GenerateCode(string transactionCodeControlTypeDetail)
        {
            var Code = new ObjectParameter("Result", typeof(string));
            _context.usp_GenerateCode(transactionCodeControlTypeDetail, 6, Code);
            return Code.Value.ToString();
        }

        [HttpPost]
        public ActionResult SaveClaimOnLinePayAuto_Transaction(int _type, int payAutoTypeId, int? userId = null, DateTime? paymentDate = null, string transRefNo = null, string payTransferResponse = null, int? claimOnlineId = null, Guid? payListHeaderId = null)
        {
            try
            {
                Serilog.Log.Debug("{_controllerName} Start SaveClaimOnLinePayAuto_Transaction [_type = {_type}, payAutoTypeId = {payAutoTypeId}, userId = {userId}, paymentDate = {paymentDate}, transRefNo = {transRefNo}, payTransferResponse = {payTransferResponse}, claimOnlineId = {claimOnlineId}, payListHeaderId = {payListHeaderId}, Code = {Code}]", _controllerName, _type, payAutoTypeId, userId, paymentDate, transRefNo, payTransferResponse, claimOnlineId, payListHeaderId);
                userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

                //_type
                //1: insert ClaimOnLinePayAuto_Transaction
                //2: insert TmpClaimOnLinePayAuto_Transaction

                string[] rs = new string[3]; 

                //GenerateCode
                var Code = GenerateCode("CT");

                Serilog.Log.Debug($"{_controllerName} - SaveClaimOnLinePayAuto_Transaction [Store procedure]: usp_ClaimOnLinePayAuto_Transaction_Insert [type = {_type}, code = {Code}, payAutoTypeId = {payAutoTypeId}, createByUserId = {userId}, paymentDate = {paymentDate}, transRefNo = {transRefNo}, payTransferResponse = {payTransferResponse}, claimOnlineId = {claimOnlineId}, payListHeaderId = {payListHeaderId}]");
                var result = _context.usp_ClaimOnLinePayAuto_Transaction_Insert(_type, Code, payAutoTypeId, userId, paymentDate,null,  transRefNo, payTransferResponse, claimOnlineId, payListHeaderId, null).FirstOrDefault();
                Serilog.Log.Debug("{_controllerName} - SaveClaimOnLinePayAuto_Transaction [Store procedure]: usp_ClaimOnLinePayAuto_Transaction_Insert [Result]: {@result}", _controllerName, result);

                var Message = result.Msg;
                rs[0] = result.IsResult.Value.ToString();
                rs[1] = result.Result;
                rs[2] = Message;

                if (result.IsResult.Value)
                {
                    Serilog.Log.Information("{_controllerName} - SaveClaimOnLinePayAuto_Transaction Successful", _controllerName);
                    return Json(ResponseResult.Success<string>(message: result.Msg), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Serilog.Log.Information("{_controllerName} - SaveClaimOnLinePayAuto_Transaction failed", _controllerName);
                    return Json(ResponseResult.Failure<string>(message: result.Msg), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "{_controllerName} - SaveClaimOnLinePayAuto_Transaction Error: {Message}", _controllerName, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [Authorization(Roles = "Developer")]
        public ActionResult PublishPayTransferCreate(int? claimOnLineId, int? payeeBankId, string payeeAccountNo, string payeeAccountName, double? transferAmountTotal, Guid payListHeaderId, string claimOnLineCode)
        {
            Log.Debug("{_controllerName} Start PublishPayTransferCreate [claimOnLineId = {claimOnLineId}, payeeBankId = {payeeBankId}, payeeAccountNo = {payeeAccountNo}, payeeAccountName = {payeeAccountName}, transferAmountTotal = {transferAmountTotal}, payListHeaderId = {payListHeaderId}, claimOnLineCode = {claimOnLineCode}]", _controllerName, claimOnLineId, payeeBankId, payeeAccountNo, payeeAccountName, transferAmountTotal, payListHeaderId, claimOnLineCode);
            try
            {
                DateTime d_dateNow = DateTime.Now;
                System.Globalization.CultureInfo enGB = new System.Globalization.CultureInfo("en-GB");
                var userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                var tempListDetail = new List<TempPayListDetailCreate>();
                var Detail = new TempPayListDetailCreate
                {
                    PayListDetailId = Guid.NewGuid(),
                    PayListDetailCode = claimOnLineCode,
                    PayListHeaderId = payListHeaderId,
                    RefDetail01 = "",
                    RefDetail02 = "",
                    Amount = (decimal?)transferAmountTotal,
                    RefDate = d_dateNow
                };
                tempListDetail.Add(Detail);

                var msg = new TempPayListHeaderCreate
                {
                    PayListHeaderId = payListHeaderId,
                    PayListHeaderCode = null,
                    SendingBankId = 7,
                    SendingBankAccountNo = Properties.Settings.Default.PayAutoBankAccNo,
                    SendingName = Properties.Settings.Default.PayAutoBankAccName,
                    ReceivingBankId = payeeBankId,
                    ReceivingBankAccountNo = payeeAccountNo,
                    ReceivingName = payeeAccountName,
                    TempPayListStatusId = 3,
                    Amount = (decimal?)transferAmountTotal,
                    PayListHeaderSourceTypeId = 2,
                    CreatedDate = d_dateNow,
                    CreatedByUserId = userId,
                    UpdatedDate = d_dateNow,
                    UpdatedByUserId = userId,
                    IsSentSMS = false,
                    PhoneNo = null,
                    TempPayListDetailCreate = tempListDetail
                };

                Log.Debug("{_controllerName} - PublishPayTransferCreate [message: {message}]", _controllerName, JsonConvert.SerializeObject(msg));
                MvcApplication.Bus.Publish(msg);

                Log.Information("{_controllerName} - PublishPayTransferCreate Successful", _controllerName);
                return Json(ResponseResult.Success<string>(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - PublishPayTransferCreate Error: {Message}", _controllerName, ex.Message);
                return Json(ResponseResult.Failure<string>(message: ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        public void UpdateClaimOnLineStatusId(int? claimOnLineId, int ClaimOnLineStatusId)
        {
            ClaimOnLineDBContext dataBase = new ClaimOnLineDBContext();
            //update status
            var d = (from x in dataBase.ClaimOnLine
                     where x.ClaimOnLineId == claimOnLineId
                     select x).First();
            d.ClaimOnLineStatusId = ClaimOnLineStatusId;
            dataBase.SaveChanges();
        }

        public ActionResult GetClaimOnLinePayAuto_Transaction(int? draw = null, int? indexStart = null, int? pageSize = null, string sortField = null, string orderType = null, int? claimOnlineId = null)
        {
            string _sortField = sortField == "" ? null : sortField;
            Log.Debug("{_controllerName} Start GetClaimOnLinePayAuto_Transaction [sortField = {sortField}, orderType = {orderType}, pageStart = {pageStart}]", _controllerName, sortField, orderType, claimOnlineId);
            Log.Debug($"{_controllerName} - GetClaimOnLinePayAuto_Transaction Call Store procedure: usp_ClaimOnLinePayAuto_Transaction_Select [ClaimOnLineId = {claimOnlineId}, indexStart = {indexStart}, pageSize = {pageSize}, sortField = {_sortField}, orderType = {orderType}, searchDetail = {null}]");

            var result = _context.usp_ClaimOnLinePayAuto_Transaction_Select(claimOnlineId, indexStart, pageSize, _sortField, orderType, null).ToList();
            Log.Debug("{_controllerName} usp_ClaimOnLinePayAuto_Transaction_Select [Result]: {@result}", _controllerName, result);

            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"recordsFiltered", result.Count() != 0 ? result.FirstOrDefault()?.TotalCount : result.Count()},
                {"data", result.ToList()}
            };

            Log.Information("{_controllerName} - GetClaimOnLinePayAuto_Transaction Successful", _controllerName);
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateCustomerAccount_ClaimOnLineTransfer(int ClaimOnLineTransferId, string toAccountNo, string toAccountName, int toBankId)
        {
            Log.Debug("{_controllerName} Start UpdateCustomerAccount_ClaimOnLineTransfer [ClaimOnLineTransferId = {ClaimOnLineTransferId}, toAccountNo = {toAccountNo}, toAccountName = {toAccountName}, toBankId = {toBankId}]", _controllerName, ClaimOnLineTransferId, toAccountNo, toAccountName, toBankId);
            try
            {
                ClaimOnLineDBContext dataBase = new ClaimOnLineDBContext();
                int? userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;
                string _toAccountNo = toAccountNo.Trim();
                string _toAccountName = toAccountName.Trim();
                var c = dataBase.ClaimOnLineTransfer.Where(x => x.ClaimOnLineTransferId == ClaimOnLineTransferId).FirstOrDefault();
                c.ToAccountNo = _toAccountNo;
                c.ToAccountName = _toAccountName;
                c.ToBankId = toBankId;
                c.UpdateDate = DateTime.Now;
                c.UpdateByUserId = userId;
                var d = dataBase.TmpClaimOnLineTransfer.Where(x => x.ClaimOnLineTransferId == ClaimOnLineTransferId).FirstOrDefault();
                d.ToAccountNo = _toAccountNo;
                d.ToAccountName = _toAccountName;
                d.ToBankId = toBankId;
                d.UpdateDate = DateTime.Now;
                d.UpdateByUserId = userId;
                dataBase.SaveChanges();
                Log.Information("{_controllerName} - UpdateCustomerAccount_ClaimOnLineTransfer Successful", _controllerName);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - UpdateCustomerAccount_ClaimOnLineTransfer Error: {Message}", _controllerName, ex.Message);
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult RetryToTransfer(int ClaimOnLineTransferId, string ClaimOnLineCode, int ClaimOnlineId)
        {
            var PayListHeaderId = Guid.NewGuid();
            int? userId = GlobalFunction.GetLoginDetail(this.HttpContext).User_ID;

            var result = _context.usp_RetryToTransfer_Update(ClaimOnLineTransferId, PayListHeaderId, userId, ClaimOnlineId).FirstOrDefault();

            if (result.IsResult == true)
            {
                var c = _context.TmpClaimOnLineTransfer.Where(x => x.ClaimOnLineTransferId == ClaimOnLineTransferId).FirstOrDefault();
                return PublishPayTransferCreate(c.ClaimOnLineId,c.ToBankId, c.ToAccountNo, c.ToAccountName, c.Amount, PayListHeaderId, ClaimOnLineCode);
            }
            else
            {
                return Json(ResponseResult.Failure<string>(message: result.Msg), JsonRequestBehavior.AllowGet);
            }
        }
    }
}