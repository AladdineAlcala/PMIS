using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMIS.ViewModels
{
    public class PatientAppointmentNewViewModel
    {
        public string PatientId { get; set; }
        [Required(ErrorMessage = "Please enter firstname.")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Please enter lastname.")]
        public string Lastname { get; set; }
        public string Middle { get; set; }
        [Required(ErrorMessage = "Please enter gender.")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Please enter Month.")]
        public int monthint { get; set; }
        [Required(ErrorMessage = "Please enter date.")]
        public int dateint { get; set; }
        [Required(ErrorMessage = "Please enter year.")]
        public int yearint { get; set; }
        public string AddStreetBrgy { get; set; }
        [Required(ErrorMessage = "Please enter Municipaly/City.")]
        public string Municipality { get; set; }
        [Required(ErrorMessage = "Please enter Province.")]
        public string Province { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(09|\+639)\d{9}$", ErrorMessage = "Entered phone format is not valid.")]
        public string ContactCell { get; set; }
        public Dictionary<string, string> GenderDictionary { get; set; }
        [Required(ErrorMessage = "Doctor In-charge required..")]
        public string PhysId { get; set; }
        public string PhyName { get; set; }
        [Required(ErrorMessage = "Appointment date required..")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime AppointDate { get; set; }
        public IEnumerable<SelectListItem> PhysicianListItems { get; set; }
    }
}