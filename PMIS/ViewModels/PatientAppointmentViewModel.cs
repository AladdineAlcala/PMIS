using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMIS.ViewModels
{
    public class PatientAppointmentViewModel
    {
        public DateTime DateSchedule { get; set; }
        public string Physician { get; set; }
        public string Status { get; set; }

    }
}