using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmileSClaimPayBack.Models
{
    public class ClaimPayBackMonitorModel
    {
        public string ClaimGroupCode
        {
            get;
            set;
        }
        public int? ItemCount
        {
            get;
            set;
        }
       
        public Decimal? Amount
        {
            get;
            set;
        }
        public string ClaimGroupType
        {
            get;
            set;
        }
        public DateTime? CreatedDate
        {
            get;
            set;
        }
        public string InsuranceCompany
        {
            get;
            set;
        }
        public int? TotalCount
        {
            get;
            set;
        }
        public string ProductGroup
        {
            get;
            set;
        }
        public string BranchDetail
        {
            get;
            set;
        }
        public string HospitalName
        {
            get;
            set;
        }
        public int? ClaimGroupTypeId
        {
            get;
            set;
        }
    }
}