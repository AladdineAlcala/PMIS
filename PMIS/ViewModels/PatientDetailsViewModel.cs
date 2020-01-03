using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace PMIS.ViewModels
{
    public class PatientDetailsViewModel
    {
        public string PatientId { get; set; }
        [Required(ErrorMessage ="Please enter firstname.")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Please enter lastname.")]
        public string Lastname { get; set; }
        public string Middle { get; set; }
        [Required(ErrorMessage = "Please enter gender.")]
        public string Gender { get; set; }
        public string CivilStat { get; set; }
        public DateTime DateofBirth { get; set; }
        public string Occupation { get; set; }
        public string Company { get; set; }
        public string AddStreetBrgy { get; set; }
        [Required(ErrorMessage = "Please enter Municipaly/City.")]
        public string Municipality { get; set; }
        [Required(ErrorMessage = "Please enter Province.")]
        public string Province { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(09|\+639)\d{9}$", ErrorMessage = "Entered phone format is not valid.")]
        public string ContactCell { get; set; }
        public string ContactTell { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string BloodType { get; set; }
        public string GuardianName { get; set; }
        public string GuardianContact { get; set; }
        public string GuardianRelation { get; set; }
        public DateTime DateRegister { get; set; }

        [Required(ErrorMessage = "Please enter Month.")]
        public int monthint { get; set; }
        [Required(ErrorMessage = "Please enter date.")]
        public int dateint { get; set; }
        [Required(ErrorMessage = "Please enter year.")]
        public int yearint { get; set; }
        public Dictionary<string, string> GenderDictionary { get; set; }
        public Dictionary<string,string> CivilStatDictionary { get; set; }
    
    }
}