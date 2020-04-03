using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMIS.ViewModels
{
    public class PatientPhysicianDistinctViewModel
    {
        public string PatientId { get; set; }
        public int Phyid { get; set; }
        public string PhyscianName { get; set; }

    }
}