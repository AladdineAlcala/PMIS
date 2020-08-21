using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMIS.ViewModels
{
    public class PrintMedicalRecordViewModel
    {
        public int RecordNo { get; set; }
        public string PatientId { get; set; }
        public string PatFullname { get; set; }
        public string PatAddress { get; set; }
        public int Age { get; set; }
        public Char Gender { get; set; }
        public string PhyId { get; set; }
        public DateTime RecordedDate { get; set; }
        
    }
    }