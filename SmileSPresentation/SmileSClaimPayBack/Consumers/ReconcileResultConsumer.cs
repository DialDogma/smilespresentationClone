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
    public class ReconcileResultConsumer : IConsumer<PayTransferAPI.Contract.ReconcileResult>
    {
        private readonly ClaimPayBackEntities _context;
        private readonly string _consumeName = nameof(ReconcileResultConsumer);
        public ReconcileResultConsumer()
        {
            _context = new ClaimPayBackEntities();
        }

        /// <summary>
        /// ReconcileResultConsumer สำหรับ Update status IsReconcile
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<ReconcileResult> context)
        {
            Log.Information("{_consumerName} consume is start.", _consumeName);
            if (context.Message.Reconciles.Count() > 0)
            {
                Log.Information("{_consumerName} consume start count data = {countData}", _consumeName, context.Message.Reconciles.Count());
                foreach (var reconcile in context.Message.Reconciles)
                {
                    var getSubGroup = _context.ClaimPayBackSubGroup.FirstOrDefault(f => f.PayListHeaderId == reconcile.PayListHeaderId && f.IsActive == true);
                    if (getSubGroup != null && reconcile.IsReconcile == true)
                    {
                        // ตรวจสอบยอดเงิน ต้นทางและปลายทาง
                        if (reconcile.TransferAmount != getSubGroup.Amount)
                        {
                            Log.Information("{_consumerName} No update data amount in sender {Amount}, reconcile result {TransferAmount} or no data.", _consumeName, getSubGroup.Amount, reconcile.TransferAmount);
                        }
                        else
                        {
                            getSubGroup.IsReconcile = (bool)reconcile.IsReconcile;
                            getSubGroup.UpdatedDate = DateTime.Now;
                            getSubGroup.UpdatedByUserId = 1;
                            _context.ClaimPayBackSubGroup.AddOrUpdate(getSubGroup);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                 
                Log.Information("{_consumerName} consume is done.", _consumeName);
            }
            else
            {
                Log.Information("{_consumerName} consume is no data from reconcile result.", _consumeName);
            }
        }
    }
}