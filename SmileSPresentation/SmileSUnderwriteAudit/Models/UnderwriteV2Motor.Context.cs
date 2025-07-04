﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSUnderwriteAudit.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class UnderwriteBranchV2MotorEntities : DbContext
    {
        public UnderwriteBranchV2MotorEntities()
            : base("name=UnderwriteBranchV2MotorEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        public virtual ObjectResult<usp_QueueMotorFullDetailByQueueId_Select_Result> usp_QueueMotorFullDetailByQueueId_Select(Nullable<int> queueId, string applicationCode)
        {
            var queueIdParameter = queueId.HasValue ?
                new ObjectParameter("QueueId", queueId) :
                new ObjectParameter("QueueId", typeof(int));
    
            var applicationCodeParameter = applicationCode != null ?
                new ObjectParameter("ApplicationCode", applicationCode) :
                new ObjectParameter("ApplicationCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_QueueMotorFullDetailByQueueId_Select_Result>("usp_QueueMotorFullDetailByQueueId_Select", queueIdParameter, applicationCodeParameter);
        }
    
        public virtual ObjectResult<usp_UnderwriteTypeMotor_Select_Result> usp_UnderwriteTypeMotor_Select(Nullable<int> underwriteTypeId)
        {
            var underwriteTypeIdParameter = underwriteTypeId.HasValue ?
                new ObjectParameter("UnderwriteTypeId", underwriteTypeId) :
                new ObjectParameter("UnderwriteTypeId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_UnderwriteTypeMotor_Select_Result>("usp_UnderwriteTypeMotor_Select", underwriteTypeIdParameter);
        }
    
        public virtual ObjectResult<usp_UnderwritePaymentTypeMotor_Select_Result> usp_UnderwritePaymentTypeMotor_Select(Nullable<int> underwritePaymentTypeId)
        {
            var underwritePaymentTypeIdParameter = underwritePaymentTypeId.HasValue ?
                new ObjectParameter("UnderwritePaymentTypeId", underwritePaymentTypeId) :
                new ObjectParameter("UnderwritePaymentTypeId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_UnderwritePaymentTypeMotor_Select_Result>("usp_UnderwritePaymentTypeMotor_Select", underwritePaymentTypeIdParameter);
        }
    
        public virtual ObjectResult<usp_CallStatusMotor_Select_Result> usp_CallStatusMotor_Select(Nullable<int> callStatusId)
        {
            var callStatusIdParameter = callStatusId.HasValue ?
                new ObjectParameter("CallStatusId", callStatusId) :
                new ObjectParameter("CallStatusId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_CallStatusMotor_Select_Result>("usp_CallStatusMotor_Select", callStatusIdParameter);
        }
    
        public virtual ObjectResult<usp_QueueMotorPaymentHistory_Select_Result> usp_QueueMotorPaymentHistory_Select(Nullable<int> queueId, Nullable<int> indexStart, Nullable<int> pageSize, string sortField, string orderType)
        {
            var queueIdParameter = queueId.HasValue ?
                new ObjectParameter("QueueId", queueId) :
                new ObjectParameter("QueueId", typeof(int));
    
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
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_QueueMotorPaymentHistory_Select_Result>("usp_QueueMotorPaymentHistory_Select", queueIdParameter, indexStartParameter, pageSizeParameter, sortFieldParameter, orderTypeParameter);
        }
    
        public virtual ObjectResult<usp_QueueMotorUnderwriteHistory_Select_Result> usp_QueueMotorUnderwriteHistory_Select(Nullable<int> queueId, Nullable<int> indexStart, Nullable<int> pageSize, string sortField, string orderType)
        {
            var queueIdParameter = queueId.HasValue ?
                new ObjectParameter("QueueId", queueId) :
                new ObjectParameter("QueueId", typeof(int));
    
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
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_QueueMotorUnderwriteHistory_Select_Result>("usp_QueueMotorUnderwriteHistory_Select", queueIdParameter, indexStartParameter, pageSizeParameter, sortFieldParameter, orderTypeParameter);
        }
    }
}
