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
    public partial class Task
    {
        public int Task_ID { get; set; }
        public string Text { get; set; }
        public string Due_Time { get; set; }
        public int Todo_List_ID { get; set; }
    
        public virtual TodoList TodoList { get; set; }
    }
    
}
