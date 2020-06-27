using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMIS.ViewModels
{
    public class DashBoardViewModel
    {
        public int appointmentCount { get; set; }
        public int loggedPhysician { get; set; }
        public int registeredPatients { get; set; }
        public int medicineCount { get; set; }
    }
}