//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileParameter
{
    using System;
    using System.Collections.Generic;
    
    public partial class Parameter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Parameter()
        {
            this.ParameterValues = new HashSet<ParameterValue>();
        }
    
        public int id { get; set; }
        public string ShortName { get; set; }
        public Nullable<int> ClassId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ReferenceParameters { get; set; }
    
        public virtual Class Class { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParameterValue> ParameterValues { get; set; }
    }
}
