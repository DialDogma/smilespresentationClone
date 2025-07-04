using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSPACommunity.ViewModels
{
    public class CreateReasonDetail
    {
        public int approveCause { get; set; }
        public string comment { get; set; }
        public int id { get; set; }
    }

    public class GetDataSaveEditInsuredDto
    {
        public string PersonnelApplicationId { get; set; }
        public string PersonnelApplicationCode { get; set; }
        public string PolicyNo { get; set; }
        public string PersonnelApplicationName { get; set; }
        public Nullable<int> PersonnelApplicationStatusId { get; set; }
        public string PersonnelApplicationStatusName { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string Product { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string Branch { get; set; }
        public string StartCoverDate { get; set; }
        public string EndCoverDate { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public Nullable<System.DateTime> CancelDate { get; set; }
        public Nullable<int> PAYear { get; set; }
        public string RefApplicationCode { get; set; }
        public Nullable<int> RefSchoolOrganizeId { get; set; }
        public Nullable<int> TotalCount { get; set; }
    }
}