//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DcrWebAPI
{
    using System;
    using System.Collections.Generic;
    
    public partial class GraphEvent
    {
        public int DCRGraphId { get; set; }
        public int DCREventId { get; set; }
    
        public virtual DCRGraph DCRGraph { get; set; }
        public virtual DCREvent DCREvent { get; set; }
    }
}
