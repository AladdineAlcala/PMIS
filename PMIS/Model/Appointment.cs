//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PMIS.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Appointment
    {
        public int No { get; set; }
        public Nullable<System.DateTime> AppointDate { get; set; }
        public string Pat_Id { get; set; }
        public Nullable<int> Phys_id { get; set; }
        public Nullable<int> PriorNo { get; set; }
        public Nullable<bool> Status { get; set; }
    
        public virtual Patient Patient { get; set; }
        public virtual Physician Physician { get; set; }
    }
}
