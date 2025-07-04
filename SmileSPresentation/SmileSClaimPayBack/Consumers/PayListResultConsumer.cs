using MassTransit;
using PayTransferAPI.Contract;
using SmileSClaimPayBack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Serilog;
using System.Data.Entity.Migrations;

namespace SmileSClaimPayBack.Consumers
{
    public class PayListResultConsumer : IConsumer<PayTransferAPI.Contract.PayListResult>
    {
        private readonly ClaimPayBackEntities _context;
        public string _consumerName = nameof(PayListResultConsumer);
        public PayListResultConsumer()
        {
            _context = new ClaimPayBackEntities();
        }

        /// <summary>
        /// step 3 udpate status การโอนเงิน
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Consume(ConsumeContext<PayListResult> context)
        {
            Log.Information("{_consumerName} PayListResultConsumer detail {detail} is start.", _consumerName, context.Message);
            try
            {
                var getClaimPayBackSubGroup = _context.ClaimPayBackSubGroup.Where(w => w.PayListHeaderId == context.Message.RefCode).FirstOrDefault();
                if (getClaimPayBackSubGroup != null)
                {
                    Log.Information("{_consumerName} get ClaimPayBackSubGroup is not null detail {detail}", _consumerName, getClaimPayBackSubGroup.ClaimPayBackSubGroupId + "/" + context.Message);

                    var checkRoundTransaction = _context.ClaimPayBackSubGroupTransaction
                        .Where(w => w.ClaimPayBackSubGroupId == getClaimPayBackSubGroup.ClaimPayBackSubGroupId && w.PayListHeaderId == context.Message.RefCode)
                        .OrderByDescending(o => o.ClaimPayBackSubGroupTransactionId)
                        .FirstOrDefault();
                    if (checkRoundTransaction != null)
                    {
                        Log.Information("{_consumerName}  Receive from payment transfer PayListHeaderId: {PayListHeaderId}, contract name : {contractName}", _consumerName, context.Message.RefCode, "PayTransferAPI.Contract.PayListResult");

                        if (checkRoundTransaction.ClaimPayBackSubGroupTransactionStatusId == 6)
                        {
                            Log.Information("{_consumerName} Create claimPayBackSubGroupTransaction duplicate data in transaction detail {detail}", _consumerName, context.Message);
                        }
                        else
                        {
                            // สร้างข้อมูลใน sub transaction 
                            var createSubGroupTransaction = new ClaimPayBackSubGroupTransaction
                            {
                                ClaimPayBackSubGroupId = getClaimPayBackSubGroup.ClaimPayBackSubGroupId,
                                ClaimPayBackSubGroupTransactionStatusId = context.Message.IsSucceed == true ? 5 : 6,
                                RefCode = context.Message.RefCode,
                                TransRefNo = context.Message.TransRefNo,
                                StatusBank = context.Message.StatusBank,
                                DescriptionEN = context.Message.Description,
                                DescriptionTH = context.Message.DescriptionTh,
                                TransferDate = context.Message.TransferDate,
                                PayResultStatusId = (int)context.Message.PayResultStatusId,
                                PayResultDescription = context.Message.PayResultDescription,
                                TransType = context.Message.TransType,
                                RoundNumber = checkRoundTransaction.RoundNumber,
                                PayListHeaderId = context.Message.RefCode,
                                IsActive = true,
                                CreatedByUserId = 1,
                                CreatedDate = DateTime.Now,
                                UpdatedByUserId = 1,
                                UpdatedDate = DateTime.Now,
                                ReceiverAccountDetail = checkRoundTransaction.ReceiverAccountDetail
                            };
                            _context.ClaimPayBackSubGroupTransaction.Add(createSubGroupTransaction);
                            await _context.SaveChangesAsync();

                            Log.Information("{_consumerName} PayListResultConsumer create ClaimPayBackSubGroupTransaction {PayListHeaderId} is done.", _consumerName, context.Message.RefCode);

                            // update status 
                            if (context.Message.IsSucceed)
                            {
                                var getSubGroup = _context.ClaimPayBackSubGroup.Where(w => w.ClaimPayBackSubGroupId == getClaimPayBackSubGroup.ClaimPayBackSubGroupId).FirstOrDefault();
                                getSubGroup.BillingTransferDate = createSubGroupTransaction.TransferDate;
                                getSubGroup.IsPayTransfer = true;
                                getSubGroup.UpdatedByUserId = 1;
                                getSubGroup.UpdatedDate = DateTime.Now;
                                //getSubGroup.ReferenceNo = createSubGroupTransaction.TransRefNo;
                                _context.ClaimPayBackSubGroup.AddOrUpdate(getSubGroup);
                                await _context.SaveChangesAsync();

                                Log.Information("{_consumerName} PayListResultConsumer update data  ClaimPayBackSubGroup {ClaimPayBackSubGroupId} IsPayTransfer == true is done.", _consumerName, getSubGroup.ClaimPayBackSubGroupId);

                                int countSubGroupTotal = _context.ClaimPayBackSubGroup.Where(w => w.ClaimPayBackTransferId == getClaimPayBackSubGroup.ClaimPayBackTransferId && w.IsActive == true).Count();
                                var countSubGroupTotalPay = _context.ClaimPayBackSubGroup.Where(w => w.ClaimPayBackTransferId == getClaimPayBackSubGroup.ClaimPayBackTransferId && w.IsActive == true && w.IsPayTransfer == true).ToList();

                                //update status and
                                var getTransfer = _context.ClaimPayBackTransfer.FirstOrDefault(f => f.ClaimPayBackTransferId == getClaimPayBackSubGroup.ClaimPayBackTransferId && f.IsActive == 1);
                                getTransfer.UpdatedDate = DateTime.Now;
                                getTransfer.UpdatedByUserId = 1;
                                getTransfer.TransferDate = (countSubGroupTotal == countSubGroupTotalPay.Count() ? (DateTime?)createSubGroupTransaction.TransferDate : null);
                                getTransfer.TransferAmount = (countSubGroupTotal == countSubGroupTotalPay.Count() ? countSubGroupTotalPay.Sum(s => s.Amount) : getClaimPayBackSubGroup.Amount);
                                getTransfer.ClaimPayBackTransferStatusId = (countSubGroupTotal == countSubGroupTotalPay.Count() ? 3 : 4);
                                _context.ClaimPayBackTransfer.AddOrUpdate(getTransfer);
                                await _context.SaveChangesAsync();

                                Log.Information("{_consumerName} PayListResultConsumer update data  ClaimPayBackTransfer ClaimPayBackTransferId:{ClaimPayBackTransferId} is done.", _consumerName, getClaimPayBackSubGroup.ClaimPayBackTransferId);

                                // update report detail สำหรับออกรายงานรายละเอียดการโอนเงิน
                                var getClaimPayBackDetails = _context.ClaimPayBackDetail.Where(f => f.ClaimPayBackSubGroupId == getClaimPayBackSubGroup.ClaimPayBackSubGroupId).ToList();
                                if (getClaimPayBackDetails.Count() > 0)
                                {
                                    foreach (var getClaimGroupCode in getClaimPayBackDetails)
                                    {
                                        var getDetailReports = _context.ClaimPayBackDetailReport.Where(f => f.ClaimGroupCode == getClaimGroupCode.ClaimGroupCode.Trim()).ToList();
                                        foreach (var getDetailReport in getDetailReports)
                                        {
                                            getDetailReport.PaymentDate = context.Message.TransferDate;
                                            getDetailReport.UpdatedByUserId = 1;
                                            getDetailReport.UpdatedDate = DateTime.Now;
                                            _context.ClaimPayBackDetailReport.AddOrUpdate(getDetailReport);
                                            await _context.SaveChangesAsync();
                                        }
                                    }
                                }

                                // update status จ่ายแล้วในตาราง
                                var getClaimPayBacks = _context.ClaimPayBack.Where(w => w.ClaimPayBackTransferId == getClaimPayBackSubGroup.ClaimPayBackTransferId).ToList();
                                foreach (var getClaimPayBack in getClaimPayBacks)
                                {
                                    getClaimPayBack.ClaimPayBackStatusId = 4;
                                    getClaimPayBack.UpdatedByUserId = 1;
                                    getClaimPayBack.UpdatedDate = DateTime.Now;
                                    _context.ClaimPayBack.AddOrUpdate(getClaimPayBack);
                                    await _context.SaveChangesAsync();
                                }

                            }
                        }

                        Log.Information("{_consumerName}  Receive from payment transfer PayListHeaderId: {PayListHeaderId}, contract name : {contractName} is done.", _consumerName, context.Message.RefCode, "PayTransferAPI.Contract.PayListResult");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("{_consumerName} Create PayListResultConsumer is error {messageError}.", _consumerName, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }

            throw new NotImplementedException();
        }
    }
}