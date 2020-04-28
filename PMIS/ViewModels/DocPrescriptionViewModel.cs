using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMIS.ViewModels
{
    public class DocPrescriptionViewModel
    {
        public int PrescNo { get; set; }
        public int RecNo { get; set; }
        public int CatId { get; set; }
        public IEnumerable<SelectListItem> PrescriptionCatListItem { get; set; }
        public int PrescId { get; set; }
        public IEnumerable<SelectListItem> RecipeListItem { get; set; }
        public string PrescriptionDetails { get; set; }
        public string Sig { get; set; }
        public string DispInst { get; set; }

    }
}