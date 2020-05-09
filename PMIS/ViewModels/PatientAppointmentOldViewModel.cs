using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMIS.ViewModels
{
    public class PatientAppointmentOldViewModel
    {
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        [Required(ErrorMessage = "Doctor In-charge required..")]
        public string PhysId { get; set; }
        public string PhyName { get; set; }
        [Required(ErrorMessage ="Appointment date required..")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString="{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime AppointDate { get; set; }
        public IEnumerable<SelectListItem> PhysicianListItems { get; set; }
       
    }
}