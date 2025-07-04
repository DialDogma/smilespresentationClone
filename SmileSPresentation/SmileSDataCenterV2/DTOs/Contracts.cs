using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSDataCenterV2.DTOs
{
    public class Contracts
    {
        public class OrderCreated
        {
            public string OrderId { get; set; }
            public Guid CustomerId { get; set; }
        }
    }
}