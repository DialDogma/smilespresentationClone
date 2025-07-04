using System;

namespace SmileSPACommunity.Models
{
    public class PDPAResult
    {
        public string PDPAId { get; set; }
        public int ProjectId { get; set; }
        public string ApplicatonCode { get; set; }
        public string PDPATypeId { get; set; }
        public string PDPATypeName { get; set; }
        public string CustomerCode { get; set; }
        public string IdentitycardNo { get; set; }
        public bool IsAllowPersonalData { get; set; }
        public bool IsAllowSensitivePersonalData { get; set; }
        public bool IsAllowSell { get; set; }
        public bool IsAllowInsurancCompany { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public object CreatedByUserId { get; set; }
        public object UpdatedDate { get; set; }
        public object UpdatedByUserId { get; set; }
    }
}