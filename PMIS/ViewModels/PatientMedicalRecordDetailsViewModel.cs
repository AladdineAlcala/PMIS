using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using PMIS.Model;

namespace PMIS.ViewModels
{
    public class PatientMedicalRecordDetailsViewModel
    {

        public int AppointmentId { get; set; }
        public string PatientId { get; set; }
        public string PhyId { get; set; }
        public Patient Patient { get; set; }
        public IPagedList<MedicalRecord> MedicalRecordList { get; set; }
    }
}