using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PMIS.Model;
using PMIS.ViewModels;

namespace PMIS.ServiceLayer
{
    public class PhycisianServices
    {
        public PMISEntities pmisEntities = null;
        public PhycisianServices()
        {
            pmisEntities=new PMISEntities();
        }

        public IEnumerable<PhysicianDetailsViewModel> GetAllPhysician()
        {
            return pmisEntities.Physicians.Select(t => new PhysicianDetailsViewModel()
            {
                PhysId = t.Phys_id,
                PhysName = t.Phys_Fullname,
                PhysAbr = t.Phys_Abr
            });
        }

        public IEnumerable<SelectListItem> GetPhysicianListItems()
        {
            var physicianlistItems = pmisEntities.Physicians.AsEnumerable().Select(t => new SelectListItem()
            {
                Value = t.Phys_id.ToString(),
                Text ="Dr. " + t.Phys_Fullname
                
            }).ToList();

            return new SelectList(physicianlistItems, "Value", "Text");
        }

    }
}