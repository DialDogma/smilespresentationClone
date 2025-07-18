﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSIncident.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class DataCenterV1Entities : DbContext
    {
        public DataCenterV1Entities()
            : base("name=DataCenterV1Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        public virtual ObjectResult<usp_Employee_SelectV2_Result> usp_Employee_SelectV2(Nullable<int> employeeId, string employeeCode, Nullable<int> teamId, Nullable<int> positionId, Nullable<int> departmentId, Nullable<int> employeeStatusId, Nullable<int> branchId, Nullable<bool> isActive, Nullable<int> indexStart, Nullable<int> pageSize, string sortField, string orderType, string search)
        {
            var employeeIdParameter = employeeId.HasValue ?
                new ObjectParameter("EmployeeId", employeeId) :
                new ObjectParameter("EmployeeId", typeof(int));
    
            var employeeCodeParameter = employeeCode != null ?
                new ObjectParameter("EmployeeCode", employeeCode) :
                new ObjectParameter("EmployeeCode", typeof(string));
    
            var teamIdParameter = teamId.HasValue ?
                new ObjectParameter("TeamId", teamId) :
                new ObjectParameter("TeamId", typeof(int));
    
            var positionIdParameter = positionId.HasValue ?
                new ObjectParameter("PositionId", positionId) :
                new ObjectParameter("PositionId", typeof(int));
    
            var departmentIdParameter = departmentId.HasValue ?
                new ObjectParameter("DepartmentId", departmentId) :
                new ObjectParameter("DepartmentId", typeof(int));
    
            var employeeStatusIdParameter = employeeStatusId.HasValue ?
                new ObjectParameter("EmployeeStatusId", employeeStatusId) :
                new ObjectParameter("EmployeeStatusId", typeof(int));
    
            var branchIdParameter = branchId.HasValue ?
                new ObjectParameter("BranchId", branchId) :
                new ObjectParameter("BranchId", typeof(int));
    
            var isActiveParameter = isActive.HasValue ?
                new ObjectParameter("IsActive", isActive) :
                new ObjectParameter("IsActive", typeof(bool));
    
            var indexStartParameter = indexStart.HasValue ?
                new ObjectParameter("IndexStart", indexStart) :
                new ObjectParameter("IndexStart", typeof(int));
    
            var pageSizeParameter = pageSize.HasValue ?
                new ObjectParameter("PageSize", pageSize) :
                new ObjectParameter("PageSize", typeof(int));
    
            var sortFieldParameter = sortField != null ?
                new ObjectParameter("SortField", sortField) :
                new ObjectParameter("SortField", typeof(string));
    
            var orderTypeParameter = orderType != null ?
                new ObjectParameter("OrderType", orderType) :
                new ObjectParameter("OrderType", typeof(string));
    
            var searchParameter = search != null ?
                new ObjectParameter("Search", search) :
                new ObjectParameter("Search", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Employee_SelectV2_Result>("usp_Employee_SelectV2", employeeIdParameter, employeeCodeParameter, teamIdParameter, positionIdParameter, departmentIdParameter, employeeStatusIdParameter, branchIdParameter, isActiveParameter, indexStartParameter, pageSizeParameter, sortFieldParameter, orderTypeParameter, searchParameter);
        }
    }
}
