using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMIS.ViewModels
{
    public class PrescriptionViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter category")]
        public int PrescriptionCatid { get; set; }

        public string PrescriptionCatDetails { get; set; }
        [Required(ErrorMessage = "Please enter category")]
        public string PrescriptionDetails { get; set; }
        [Required(ErrorMessage = "Please enter category")]
        public string PrescUnit { get; set; }
        public IEnumerable<SelectListItem> PresCategorySelectList { get; set;}
    }
}