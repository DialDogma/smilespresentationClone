using MassTransit;
using PayTransferAPI.Contract;
using SmileSClaimPayBack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Serilog;

namespace SmileSClaimPayBack.Consumers
{
    public class TempPayListHeaderCreatedConsumer : IConsumer<PayTransferAPI.Contract.TempPayListHeaderCreated>
    {
        private readonly ClaimPayBackEntities _context;
        public string _consumerName = nameof(TempPayListHeaderCreatedConsumer);
        public TempPayListHeaderCreatedConsumer()
        {
            _context = new ClaimPayBackEntities();
        }

        /// <summary>
        /// step 2 udpate status การสร้างรายการ
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<TempPayListHeaderCreated> context)
        {
            try
            {
                Log.Information("{_consumerName} TempPayListHeaderCreated is start with detail {detail}.", _consumerName, context.Message);
                var getSubGroupTransaction = _context.ClaimPayBackSubGroup.Where(w => w.PayListHeaderId == context.Message.PayListHeaderId).FirstOrDefault();
                if (getSubGroupTransaction != null)
                {
                    Log.Information("{_consumerName} get ClaimPayBackSubGroup is not null detail {detail}", _consumerName, getSubGroupTransaction.ClaimPayBackSubGroupId + "/" + context.Message.PayListHeaderId);

                    var checkRoundTransaction = _context.ClaimPayBackSubGroupTransaction
                        .Where(w => w.ClaimPayBackSubGroupId == getSubGroupTransaction.ClaimPayBackSubGroupId && w.PayListHeaderId == context.Message.PayListHeaderId)
                        .OrderByDescending(o => o.ClaimPayBackSubGroupTransactionId)
                        .FirstOrDefault();
                    if (checkRoundTransaction != null)
                    {
                        Log.Information("{_consumerName}  Receive from payment transfer PayListHeaderId: {PayListHeaderId}, contract name : {contractName}", _consumerName, context.Message.PayListHeaderId, "PayTransferAPI.Contract.TempPayListHeaderCreated");

                        // สร้างข้อมูล
                        var createSubGroupTransaction = new ClaimPayBackSubGroupTransaction
                        {
                            ClaimPayBackSubGroupId = getSubGroupTransaction.ClaimPayBackSubGroupId,
                            ClaimPayBackSubGroupTransactionStatusId = context.Message.IsSuccess == true ? 3 : 4,
                            RefCode = null,
                            TransRefNo = null,
                            StatusBank = null,
                            DescriptionEN = context.Message.Message,
                            DescriptionTH = context.Message.Message,
                            TransferDate = null,
                            PayResultStatusId = 0,
                            PayResultDescription = null,
                            TransType = null,
                            RoundNumber = checkRoundTransaction.RoundNumber,
                            PayListHeaderId = context.Message.PayListHeaderId,
                            IsActive = true,
                            CreatedByUserId = 1,
                            CreatedDate = DateTime.Now,
                            UpdatedByUserId = 1,
                            UpdatedDate = DateTime.Now,
                            ReceiverAccountDetail = checkRoundTransaction.ReceiverAccountDetail
                        };
                        _context.ClaimPayBackSubGroupTransaction.Add(createSubGroupTransaction);
                        await _context.SaveChangesAsync();

                        Log.Information("{_controllerName} Create ClaimPayBackSubGroupTransaction is done.", _consumerName);
                    }
                }

                Log.Information("{_consumerName} TempPayListHeaderCreated is done. with PayListHeaderId {PayListHeaderId}", _consumerName, context.Message.PayListHeaderId);
            }
            catch (Exception ex)
            {
                Log.Error("{_controllerName} Create ClaimPayBackSubGroupTransaction is error {messageError}.", _consumerName, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }
        }
    }
}