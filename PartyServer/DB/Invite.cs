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
    public partial class Invite
    {
        public int Wedding_ID { get; set; }
        public int Invited_Member_ID { get; set; }
        public Nullable<int> Status { get; set; }
    
        public virtual Wedding Wedding { get; set; }
    }
    
}
