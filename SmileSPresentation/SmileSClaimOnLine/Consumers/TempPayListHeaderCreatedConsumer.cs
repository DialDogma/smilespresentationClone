using MassTransit;
using PayTransferAPI.Contract;
using Serilog;
using SmileSClaimOnLine.Controllers;
using SmileSClaimOnLine.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SmileSClaimOnLine.Consumers
{
    public class TempPayListHeaderCreatedConsumer : IConsumer<TempPayListHeaderCreated>
    {
        private const string _controllerName = nameof(TempPayListHeaderCreatedConsumer);

        public Task Consume(ConsumeContext<TempPayListHeaderCreated> context)
        {
            try
            {
                var message = context.Message;
                Guid? PayListHeaderId = message.PayListHeaderId;
                Log.Information("{_controllerName} - Consume [message: {@message}]", _controllerName, message);

                if (message.IsSuccess.Value == false)
                {
                    ClaimOnLineDBContext dataBase = new ClaimOnLineDBContext();
                    //Select TmpClaimOnLineTransfer
                    var col = dataBase.usp_TmpClaimOnLineTransfer_Select(PayListHeaderId).FirstOrDefault();

                    int? userId = col.CreateByUserId;
                    int? claimOnLineId = col.ClaimOnLineId;
                    double? transferAmountTotal = col.Amount;
                    int claimOnLineStatusId = 6; //ไม่สำเร็จ
                    Log.Information("{_controllerName} - Consume claimOnLineId = {claimOnLineId} [message: {@message}]", _controllerName, message);

                    Log.Debug($"{_controllerName} Consume - บันทึก ประวัติการทำรายการ สถานะ: ไม่สำเร็จ claimOnLineId = {claimOnLineId}");
                    new PayListResultConsumer().ClaimOnLinePayAuto_Transaction_Insert(1, 6, userId, null, (decimal)transferAmountTotal, null, null, claimOnLineId, PayListHeaderId, null);

                    if (col.IsSuccessTempPayList == true)
                    {
                        Log.Debug($"{_controllerName} Consume - บันทึก ClaimOnLineTransfer claimOnLineId = {claimOnLineId}");
                        new PayListResultConsumer().ClaimOnLineTransfer_PayAuto_Insert(claimOnLineId, null, transferAmountTotal, userId, null, col.SMSPhoneNo, "", PayListHeaderId, claimOnLineStatusId, col.TransferFromMenu, null, false);
                    }
                    else
                    {
                        Log.Debug($"{_controllerName} Consume - start update ClaimOnLineStatusId = 6 [ClaimOnLineId = {claimOnLineId}] ");
                        var o = dataBase.ClaimOnLine.Where(x => x.ClaimOnLineId == claimOnLineId).FirstOrDefault();
                        o.ClaimOnLineStatusId = 6;
                        dataBase.SaveChanges();
                        Log.Debug($"{_controllerName} Consume - update ClaimOnLineStatusId = 6 Success [ClaimOnLineId = {claimOnLineId}] ");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{_controllerName} - Consume Error: {Message}", _controllerName, ex.Message);
            }
            return Task.CompletedTask;
        }
    }
}