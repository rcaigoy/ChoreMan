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
    
    public partial class RotationInterval
    {
        public int Id { get; set; }
        public int ChoreListId { get; set; }
        public System.DateTime StartDate { get; set; }
        public int IntervalTypeId { get; set; }
        public int IntervalNumber { get; set; }
        public bool IsActive { get; set; }
    
        public virtual ChoreList ChoreList { get; set; }
    }
}
