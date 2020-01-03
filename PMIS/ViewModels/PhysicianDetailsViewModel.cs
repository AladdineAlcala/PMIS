using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PMIS.ViewModels
{
    public class PhysicianDetailsViewModel
    {
        public int? PhysId { get; set; }
        [Required(ErrorMessage = "Please enter physican.")]
        public string PhysName { get; set; }
        [Required(ErrorMessage = "Please enter physician abbrv..")]
        public string PhysAbr { get; set; }

    }
}