using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSClaimOnLine.DTOs
{
    public class GetPayListHeaderResponseDto
    {
        public data data { get; set; }
        public bool isSuccess { get; set; }
        public DateTime serverDateTime { get; set; }
    }

    public class data
    {
        public string payListHeaderId { get; set; }
        public int sendingBankId { get; set; }
        public string sendingBankAccountNo { get; set; }
        public string sendingBankName { get; set; }
        public string sendingName { get; set; }
        public int receivingBankId { get; set; }
        public string receivingBankAccountNo { get; set; }
        public string receivingBankName { get; set; }
        public string receivingName { get; set; }
        public string bankTranferNo { get; set; }
        public string bankStatusCode { get; set; }
        public string bankDescription { get; set; }
        public int payListStatusId { get; set; }
        public string payListStatusName { get; set; }
        public decimal amount { get; set; }
        public bool isTransfer { get; set; }
        public int payListHeaderSourceTypeId { get; set; }
        public string payListHeaderSourceTypeName { get; set; }
        public bool isSentSMS { get; set; }
        public DateTime? bankTransferDate { get; set; }
    }


}