﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileClaimV2.Models
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
    
    
        public virtual ObjectResult<usp_ChiefComplain_Select_Result> usp_ChiefComplain_Select(Nullable<int> chiefComplainID, Nullable<bool> isActive)
        {
            var chiefComplainIDParameter = chiefComplainID.HasValue ?
                new ObjectParameter("ChiefComplainID", chiefComplainID) :
                new ObjectParameter("ChiefComplainID", typeof(int));
    
            var isActiveParameter = isActive.HasValue ?
                new ObjectParameter("IsActive", isActive) :
                new ObjectParameter("IsActive", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_ChiefComplain_Select_Result>("usp_ChiefComplain_Select", chiefComplainIDParameter, isActiveParameter);
        }
    
        public virtual ObjectResult<usp_ClaimAdmitType_Select_Result> usp_ClaimAdmitType_Select(Nullable<int> productGroupID, Nullable<int> claimTypeID, Nullable<bool> isActive)
        {
            var productGroupIDParameter = productGroupID.HasValue ?
                new ObjectParameter("ProductGroupID", productGroupID) :
                new ObjectParameter("ProductGroupID", typeof(int));
    
            var claimTypeIDParameter = claimTypeID.HasValue ?
                new ObjectParameter("ClaimTypeID", claimTypeID) :
                new ObjectParameter("ClaimTypeID", typeof(int));
    
            var isActiveParameter = isActive.HasValue ?
                new ObjectParameter("IsActive", isActive) :
                new ObjectParameter("IsActive", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_ClaimAdmitType_Select_Result>("usp_ClaimAdmitType_Select", productGroupIDParameter, claimTypeIDParameter, isActiveParameter);
        }
    
        public virtual ObjectResult<usp_ClaimType_Select_Result> usp_ClaimType_Select(Nullable<int> productGroupID, Nullable<bool> isActive)
        {
            var productGroupIDParameter = productGroupID.HasValue ?
                new ObjectParameter("ProductGroupID", productGroupID) :
                new ObjectParameter("ProductGroupID", typeof(int));
    
            var isActiveParameter = isActive.HasValue ?
                new ObjectParameter("IsActive", isActive) :
                new ObjectParameter("IsActive", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_ClaimType_Select_Result>("usp_ClaimType_Select", productGroupIDParameter, isActiveParameter);
        }
    
        public virtual ObjectResult<usp_Province2_Select_Result> usp_Province2_Select(Nullable<int> province_ID)
        {
            var province_IDParameter = province_ID.HasValue ?
                new ObjectParameter("Province_ID", province_ID) :
                new ObjectParameter("Province_ID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Province2_Select_Result>("usp_Province2_Select", province_IDParameter);
        }
    
        public virtual ObjectResult<usp_Branch_Select_Result> usp_Branch_Select(Nullable<int> branchID)
        {
            var branchIDParameter = branchID.HasValue ?
                new ObjectParameter("branchID", branchID) :
                new ObjectParameter("branchID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Branch_Select_Result>("usp_Branch_Select", branchIDParameter);
        }
    }
}
