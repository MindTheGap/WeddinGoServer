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
    public partial class Photo
    {
        public Photo()
        {
            this.Like = new HashSet<Like>();
            this.Like1 = new HashSet<Like>();
            this.Wedding = new HashSet<Wedding>();
        }
    
        public int Photo_ID { get; set; }
        public string Image_Path { get; set; }
    
        public virtual ICollection<Like> Like { get; set; }
        public virtual ICollection<Like> Like1 { get; set; }
        public virtual ICollection<Wedding> Wedding { get; set; }
    }
    
}
