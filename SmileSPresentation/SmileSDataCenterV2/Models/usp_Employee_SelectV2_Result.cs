//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSDataCenterV2.Models
{
    using System;
    
    public partial class usp_Employee_SelectV2_Result
    {
        public int UserId { get; set; }
        public int Employee_ID { get; set; }
        public Nullable<int> Person_ID { get; set; }
        public string EmployeeCode { get; set; }
        public Nullable<int> EmployeeTeam_ID { get; set; }
        public Nullable<int> EmployeePosition_ID { get; set; }
        public Nullable<int> Department_ID { get; set; }
        public Nullable<int> SiamSmileCompany_ID { get; set; }
        public Nullable<int> EmployeeStatus_ID { get; set; }
        public Nullable<System.DateTime> StartworkDate { get; set; }
        public Nullable<System.DateTime> TurnOverDate { get; set; }
        public Nullable<int> CreatedBy_ID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> Title_ID { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string EmployeeTeamDetail { get; set; }
        public int Branch_ID { get; set; }
        public string BranchDetail { get; set; }
        public string DepartmentDetail { get; set; }
        public string EmployeeStatusDetail { get; set; }
        public string Mobile { get; set; }
        public Nullable<int> TotalCount { get; set; }
    }
}
