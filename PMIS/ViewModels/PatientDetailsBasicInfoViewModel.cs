using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PMIS.ViewModels
{
    public class PatientDetailsBasicInfoViewModel
    {
        public string PatientId { get; set; }
        [Required(ErrorMessage = "Please enter firstname.")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Please enter lastname.")]
        public string Lastname { get; set; }
        public string Middle { get; set; }
        [Required(ErrorMessage = "Please enter gender.")]
        public string Gender { get; set; }
        public DateTime DateofBirth { get; set; }
     
        public string AddStreetBrgy { get; set; }
        [Required(ErrorMessage = "Please enter Municipaly/City.")]
        public string Municipality { get; set; }
        [Required(ErrorMessage = "Please enter Province.")]
        public string Province { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(09|\+639)\d{9}$", ErrorMessage = "Entered phone format is not valid.")]
        public string ContactCell { get; set; }

        [Required(ErrorMessage = "Please enter Month.")]
        public int monthint { get; set; }
        [Required(ErrorMessage = "Please enter date.")]
        public int dateint { get; set; }
        [Required(ErrorMessage = "Please enter year.")]
        public int yearint { get; set; }
        public Dictionary<string, string> GenderDictionary { get; set; }
        
    }
}