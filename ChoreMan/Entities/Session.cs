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
    
    public partial class Session
    {
        public int SessionId { get; set; }
        public int UserId { get; set; }
        public string BearerToken { get; set; }
        public string RefreshToken { get; set; }
        public System.DateTime ExpirationDate { get; set; }
    
        public virtual User User { get; set; }
    }
}
