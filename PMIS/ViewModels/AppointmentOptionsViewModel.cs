using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PMIS.Model;

namespace PMIS.ViewModels
{

    public enum AppointOptions
    {
        Cancel=1,
        Remove,
        Replace,
        Serve
    }

    public class AppointmentOptionsViewModel
    {
        public int AppNo { get; set; }
        [Required(ErrorMessage = "Please enter appointment option.")]
        public AppointOptions AppOptions  { get; set; }
        public string RepIdNo { get; set; }
        public MedicalRecord PatientMedicalRecord { get; set; }
    }
}