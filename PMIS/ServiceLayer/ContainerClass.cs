
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMIS.Model;
using PMIS.ViewModels;

namespace PMIS.ServiceLayer
{
    public static class ContainerClass
    {
        public static List<PrintDocPrescriptionViewModel> DocPrescription=new List<PrintDocPrescriptionViewModel>();
        public static List<PatientDetailsViewModel> PatientDetails=new List<PatientDetailsViewModel>();
        public static List<PrintMedicalRecordViewModel> MedicalRecord=new List<PrintMedicalRecordViewModel>();
    }
}