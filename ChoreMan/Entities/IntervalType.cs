//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChoreMan.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class IntervalType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public IntervalType()
        {
            this.RotationIntervals = new HashSet<RotationInterval>();
        }
    
        public int IntervalTypeId { get; set; }
        public string IntervalType1 { get; set; }
    
        public virtual RotationInterval RotationInterval { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RotationInterval> RotationIntervals { get; set; }
    }
}
