using System;
using System.Collections.Generic;
using System.Text;

namespace PolicyApiContract
{
    // ****************************************************************************************
    // คำอธิบาย:
    // คลาสนี้ถูกสร้างขึ้นมาเพื่อใช้เป็น Message Contract ในฝั่ง Consumer (.NET Framework)
    // โดยอิงตามโครงสร้างของระบบต้นทาง (Publisher) ซึ่งเขียนด้วย .NET 6
    //
    // ชื่อ property และโครงสร้าง ต้องตรงกับต้นทางเพื่อให้สามารถ Deserialize ได้ถูกต้อง
    // หากระบบต้นทางมีการเปลี่ยนแปลงโครงสร้าง message (เช่น เพิ่ม/ลบ/แก้ชื่อ field)
    // จำเป็นต้องอัปเดตคลาสนี้ให้สอดคล้องกันทันที เพื่อป้องกันการ Deserialize ผิดพลาด
    // ****************************************************************************************
    public class PolicyPremiumDebtCreate
    {
        public Guid DebtGroupId { get; set; }
        public string DebtGroupCode { get; set; }

        public DateTime? Period { get; set; }

        public int? SourceTypeId { get; set; }
        public int? PaymentMethodTypeId { get; set; }
        public int? ItemCount { get; set; }

        public decimal? TotalAmount { get; set; }

        public DateTime? PayablePeriodFrom { get; set; }
        public DateTime? PayablePeriodTo { get; set; }
        public bool? IsSendSMSPaySlip { get; set; }
        public bool? IsSendSMSBilling { get; set; }
        public int? DebtGroupStatusId { get; set; }
        public int? DebtGroupReferTypeId { get; set; }
        public List<PremiumDebtHeader> PremiumDebtHeaders { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedByUserId { get; set; }
    }

    /// <summary>
    /// เป็นส่วนหนึ่งของ message contract เดียวกัน
    /// </summary>
    public class PremiumDebtHeader
    {
        public Guid DebtHeaderId { get; set; }
        public Guid? DebtGroupId { get; set; }
        public string B { get; set; }
        public string PayerName { get; set; }
        public string PhoneNo { get; set; }
        public decimal? PremiumDebt { get; set; }

        public int? BankId { get; set; }

        public string BankAccountName { get; set; }

        public string BankAccountNo { get; set; }

        public string Ref1 { get; set; }

        public string Ref2 { get; set; }

        public string Ref3 { get; set; }

        public int? PaymentStatusId { get; set; }
        public int? PaymentChannelId { get; set; }
        public Guid? RefSummaryDetailId { get; set; }
        public DateTime? TransactionDatetime { get; set; }
        public int? ItemCount { get; set; }
        public List<PremiumDebtDetail> PremiumDebtDetails { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedByUserId { get; set; }
    }

    /// <summary>
    /// เป็นส่วนหนึ่งของ message contract เดียวกัน
    /// </summary>
    public class PremiumDebtDetail
    {
        public Guid DebtDetailId { get; set; }
        public Guid? DebtHeaderId { get; set; }
        public string IN { get; set; }
        public string ApplicationCode { get; set; }
        public string ProductGroupName { get; set; }
        public string ProductName { get; set; }

        public decimal? Premium { get; set; }

        public int? PeriodTypeId { get; set; }

        public DateTime? PeriodFrom { get; set; }

        public DateTime? PeriodTo { get; set; }

        public decimal? PremiumSum { get; set; }

        public decimal? Discount { get; set; }

        public decimal? TotalAmount { get; set; }
        public string CustName { get; set; }

        public int? InsuranceId { get; set; }
        public int? ReceiveTypeId { get; set; }

        public string Detail1 { get; set; }

        public string Detail2 { get; set; }

        public string Detail3 { get; set; }

        public string Remark { get; set; }
        public bool? IsActive { get; set; }
        public int? ProductId { get; set; }
    }
}