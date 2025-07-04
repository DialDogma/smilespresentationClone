//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSCriticalIllness.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ApplicationTransaction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ApplicationTransaction()
        {
            this.ApplicationTransactionDetail = new HashSet<ApplicationTransactionDetail>();
        }
    
        public int ApplicationTransactionId { get; set; }
        public string ApplicationTransactionCode { get; set; }
        public Nullable<int> ApplicationId { get; set; }
        public Nullable<int> TransactionTypeId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedByUserId { get; set; }
        public Nullable<int> TmpId { get; set; }
    
        public virtual Application Application { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApplicationTransactionDetail> ApplicationTransactionDetail { get; set; }
    }
}
