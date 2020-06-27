using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMIS.ViewModels
{
    public class PrintDocPrescriptionViewModel
    {
        public int PrescNo { get; set; }
        public int RecNo { get; set; }
        public string PrescriptionDetails { get; set; }
        public string Sig { get; set; }
        public string DispInst { get; set; }
    }
}