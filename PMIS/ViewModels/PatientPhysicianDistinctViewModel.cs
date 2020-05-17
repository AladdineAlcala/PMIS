using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMIS.ViewModels
{
    public class PatientPhysicianDistinctViewModel
    {
        public string PatientId { get; set; }
        public string Phyid { get; set; }
        public string Arb { get; set; }
        public string phyLastname { get; set; }
        public string phyFirstname { get; set; }

    }
}