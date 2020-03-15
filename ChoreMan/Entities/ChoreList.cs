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
    
    public partial class ChoreList
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ChoreList()
        {
            this.Chores = new HashSet<Chore>();
            this.RotationIntervals = new HashSet<RotationInterval>();
            this.ChoreUsers = new HashSet<ChoreUser>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
        public int ChoreListTypeId { get; set; }
    
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Chore> Chores { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RotationInterval> RotationIntervals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChoreUser> ChoreUsers { get; set; }
        public virtual ChoreListType ChoreListType { get; set; }
    }
}