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
        public string PhyId { get; set; }
        public DateTime RecordedDate { get; set; }
        //public string Subject { get; set; }
        //public string Desciption { get; set; }
        //public int ApppointmentNo { get; set; }
    }
}