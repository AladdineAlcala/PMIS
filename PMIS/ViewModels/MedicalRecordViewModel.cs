using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMIS.ViewModels
{
    public class MedicalRecordViewModel
    {
        public int RecordNo { get; set; }
        public string PatientId { get; set; }
        public int PhyId { get; set; }
        public DateTime RecordedDate { get; set; }
        public string Subject { get; set; }
        [AllowHtml]
        public string Desciption { get; set; }
     

    }
}