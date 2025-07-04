using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSClaimPayBack.ViewModels
{
    public class FundPayDTO
    {
    }

    public class ViewDetailTransactionDTO
    {
        public string TransferDate { get; set; }
        public string SubGroupTransactionStatusName { get; set; }
        public int SubGroupTransactionStatusId { get; set; }
        public string CreateByUser { get; set; }
        public string Amount { get; set; }
        public string Detail { get; set; }
        public string Description { get; set; }
        public int Round { get; set; }
    }

    public class ValidateHospitalDetailDTO
    {
        public bool Valid { get; set; } = true;
        public string Message { get; set; }
        public string HospitalName { get; set; }
    }   
    public class FormRePayTransfersDTO
    {
        public string dataPublish { get; set; }
        public string transactionId { get; set; }
        public string encodeDateTime { get; set; }
        
    }

}