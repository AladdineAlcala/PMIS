using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMIS.Model;

namespace PMIS.ViewModels
{
    public class ApplicationUserViewModel
    {
        public string UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Middle { get; set; }
        public string Abr { get; set; }
    }
}