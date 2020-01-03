using System.Collections.Generic;
using System.Web.Mvc;

namespace PMIS.ViewModels
{
    public class AppointmentViewModel
    {
        public int PhysId { get; set; }
        public IEnumerable<SelectListItem> PhysicianListItems { get; set; }
    }
}