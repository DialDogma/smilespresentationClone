//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSCommission.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DesignerParameter
    {
        public int Id { get; set; }
        public Nullable<int> DesignerModel_Id { get; set; }
        public Nullable<int> ParameterIndex { get; set; }
        public string ParameterName { get; set; }
        public string ParameterFormula { get; set; }
        public Nullable<int> ParameterDataType { get; set; }
    
        public virtual DesignerModel DesignerModel { get; set; }
    }
}
