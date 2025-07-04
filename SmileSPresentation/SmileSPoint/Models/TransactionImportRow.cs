using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSPoint.Models
{
    public class TransactionImportRow
    {
        public int AccountId { get; set; }
        public int TransactionTypeId { get; set; }
        public double Point { get; set; }
        public double BalanceBefore { get; set; }
        public double BalanceAfter { get; set; }
        public double Deductible { get; set; }
        public double NotDeductible { get; set; }
        public string Remark { get; set; }
    }
}