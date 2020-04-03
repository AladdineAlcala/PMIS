using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMIS.Model;

namespace PMIS.ViewModels
{
    public class PatientChartViewModel
    {
        public int recordNo { get; set; }
        public MedicalRecord MedicalRecord { get; set; }
        public Medication Medication { get; set; }
    }
}