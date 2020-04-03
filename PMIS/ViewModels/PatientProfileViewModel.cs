using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using PMIS.Model;

namespace PMIS.ViewModels
{
    public class PatientProfileViewModel
    {
        public string PatientId { get; set; }
        public PatientDetailsViewModel PatientDetails { get; set; }
        public List<PhysicianDetailsViewModel> Listofdoctors { get; set; }
        public Appointment Lastvisited { get; set; }

       
    }
}