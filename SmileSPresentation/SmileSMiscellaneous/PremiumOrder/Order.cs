using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSMiscellaneous.PremiumOrder
{
    public class Order
    {
        public OrderDetail data { get; set; } = null;
    }

    public class OrderDetail
    {
        public string igCode { get; set; }
        public int? personId { get; set; }
        public int? cardTypeId { get; set; }
        public string cardDetail { get; set; }
        public int? titleId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phoneNumber { get; set; }
        public int? paymentMethodTypeId { get; set; }

        public int? statusId { get; set; }
        public string bankId { get; set; }
        public string bankAccountName { get; set; }
        public string bankAccountNo { get; set; }
        public string refPremium { get; set; }
        public string billNoId { get; set; }
        public List<OrderSubDetail> orderSubDetails { get; set; }
    }

    public class OrderSubDetail
    {
        public string inCode { get; set; }
        public int? productId { get; set; }
        public int? productTypeId { get; set; }
        public double? premium { get; set; }
        public double? premiumNet { get; set; }
        public int? periodTypeId { get; set; }
        public DateTime? dcrDate { get; set; }
        public DateTime? birthDate { get; set; }
        public int? genderId { get; set; }
    }
}