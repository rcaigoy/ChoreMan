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
    
    public partial class ChoreUser
    {
        public int Id { get; set; }
        public int ChoreListId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; }
        public bool IsPendingCancel { get; set; }
    
        public virtual ChoreList ChoreList { get; set; }
    }
}
