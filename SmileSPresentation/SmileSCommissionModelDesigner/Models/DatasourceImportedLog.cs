//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSCommissionModelDesigner.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DatasourceImportedLog
    {
        public int Id { get; set; }
        public Nullable<int> Period_Id { get; set; }
        public Nullable<int> Datasource_Id { get; set; }
        public string ImportedBy_Id { get; set; }
        public Nullable<int> Status_Id { get; set; }
        public Nullable<int> ImportedType_Id { get; set; }
        public Nullable<System.DateTime> ImportedDateTime { get; set; }
        public Nullable<bool> IsEdit { get; set; }
        public string EditBy_Id { get; set; }
        public Nullable<System.DateTime> EditDate { get; set; }
        public string EditCause_Id { get; set; }
        public string EditRemark { get; set; }
    }
}
