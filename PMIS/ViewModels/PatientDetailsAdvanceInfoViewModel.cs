using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PMIS.ViewModels
{
    public class PatientDetailsAdvanceInfoViewModel
    {
        public string PatientId { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?[\d]{3}\)?[\s-]?[\d]{3}[\s-]?[\d]{4}$", ErrorMessage = "Entered phone format is not valid.")]
        public string ContactTell { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public string BloodType { get; set; }
        public string GuardianName { get; set; }
        public string GuardianContact { get; set; }
        public string GuardianRelation { get; set; }
        public string Occupation { get; set; }
        public string Company { get; set; }
        public string CivilStat { get; set; }
        public Dictionary<string, string> CivilStatDictionary { get; set; }
    }
}