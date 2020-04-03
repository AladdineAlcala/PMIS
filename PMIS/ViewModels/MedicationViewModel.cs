using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMIS.ViewModels
{
    public class MedicationViewModel
    {
        public int Id { get; set; }
        public DateTime RecordedDate { get; set; }
        [AllowHtml]
        public string Medication { get; set; }
        public int RecordNo { get; set; }
        public int operation { get; set; }
    }
}