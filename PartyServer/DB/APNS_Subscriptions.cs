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
    public partial class APNS_Subscriptions
    {
        public int id { get; set; }
        public string DeviceToken { get; set; }
        public string DeviceID { get; set; }
        public string NetworkID { get; set; }
        public string Application { get; set; }
        public Nullable<System.DateTime> AddedOn { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<bool> Dev { get; set; }
        public int BadgeCount { get; set; }
    }
    
}
