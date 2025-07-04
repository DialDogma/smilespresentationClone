using MassTransit;
using PolicyApiContract;
using System;
using System.Threading.Tasks;
using SmileSDataCenterV2.Models;

namespace SmileSDataCenterV2.Consumer
{
    public class PolicyPremiumDebtCreateConsumer : IConsumer<PolicyPremiumDebtCreate>
    {
        private readonly DataCenterV1Entities1 _contextSP;

        public PolicyPremiumDebtCreateConsumer()
        {
            _contextSP = new DataCenterV1Entities1();
        }

        public async Task Consume(ConsumeContext<PolicyPremiumDebtCreate> context)
        {
            //var msg = context.Message;
            //Console.WriteLine($"✅ Got DebtGroupId: {msg.DebtGroupId}");

            //rabbit policy-api-policy-premium-debt-create
            var msgGroup = context.Message;
            var msgHeader = context.Message.PremiumDebtHeaders;

            //Excute SP Group
            _contextSP.usp_TempPremiumDebtGroup_Consume_Upsert(msgGroup.DebtGroupId, msgGroup.DebtGroupCode, msgGroup.Period, msgGroup.SourceTypeId, msgGroup.PaymentMethodTypeId, msgGroup.ItemCount, msgGroup.TotalAmount, msgGroup.PayablePeriodFrom, msgGroup.PayablePeriodTo, msgGroup.IsSendSMSPaySlip, msgGroup.IsSendSMSBilling, msgGroup.DebtGroupStatusId, msgGroup.DebtGroupReferTypeId, msgGroup.IsActive, msgGroup.CreatedDate, msgGroup.CreatedByUserId, msgGroup.UpdatedDate, msgGroup.UpdatedByUserId);
            //Loop Header
            foreach (var h in msgHeader)
            {
                //Excute SP Header
                _contextSP.usp_TempPremiumDebtHeader_Consume_Upsert(h.DebtHeaderId,h.DebtGroupId,h.B,h.PayerName,h.PhoneNo,h.PremiumDebt,h.BankId,h.BankAccountName,h.BankAccountNo,h.Ref1,h.Ref2,h.Ref3,h.PaymentStatusId,h.PaymentChannelId,h.RefSummaryDetailId,h.TransactionDatetime,h.ItemCount,h.IsActive,h.UpdatedDate,h.UpdatedByUserId);
                //Loop Detail
                foreach (var d in h.PremiumDebtDetails)
                {
                    //Excute SP Detail
                    _contextSP.usp_TempPremiumDebtDetail_Consume_Upsert(d.DebtDetailId,d.DebtHeaderId,d.IN,d.ApplicationCode,d.ProductGroupName,d.ProductName,d.Premium,d.PeriodTypeId,d.PeriodFrom,d.PeriodTo,d.PremiumSum,d.Discount,d.TotalAmount,d.CustName,d.InsuranceId,d.ReceiveTypeId,d.Detail1,d.Detail2,d.Detail3,d.Remark,d.IsActive,d.ProductId);
                }
            }

            await Task.CompletedTask;
        }
    }
}