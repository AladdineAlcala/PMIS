﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMIS.ViewModels
{
    public class AppointmentScheduleViewModel
    {
        public int No { get; set; }
        public int PriorNo { get; set; }
        public string PatientNo { get; set; }
        public string PatientName { get; set; }
        public int PhyId { get; set; }
        public string PhyName { get; set; }

    }
}