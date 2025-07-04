using System;

namespace SmileSDataCenterV2.Contracts
{
    public class CustomerContactUpdate
    {
        public Guid PersonId { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
    }
}
