//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace PartyServer.DB
{
    public partial class Member
    {
        public Member()
        {
            this.Comment = new HashSet<Comment>();
            this.Like = new HashSet<Like>();
            this.Wedding = new HashSet<Wedding>();
            this.Wedding1 = new HashSet<Wedding>();
        }
    
        public int Member_ID { get; set; }
        public string Email { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Token_ID { get; set; }
        public Nullable<int> Wedding_ID { get; set; }
        public Nullable<int> Permission_ID { get; set; }
        public Nullable<int> RSVP_ID { get; set; }
        public Nullable<bool> Is_Blocked { get; set; }
        public Nullable<int> FacebookUserID { get; set; }
    
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<Like> Like { get; set; }
        public virtual Permission Permission { get; set; }
        public virtual RSVP RSVP { get; set; }
        public virtual ICollection<Wedding> Wedding { get; set; }
        public virtual ICollection<Wedding> Wedding1 { get; set; }
    }
    
}
