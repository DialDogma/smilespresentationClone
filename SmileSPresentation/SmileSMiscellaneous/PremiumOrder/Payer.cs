using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSMiscellaneous.PremiumOrder
{
    public class Payer
    {
        public PayerDetail data { get; set; } = null;
    }

    public class PayerDetail
    {
        public int? personTypeId { get; set; }
        public int? personId { get; set; }
        public string cardDetail { get; set; }
        public int? cardTypeId { get; set; }
        public int? titleId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phoneNumber { get; set; }

    }
}