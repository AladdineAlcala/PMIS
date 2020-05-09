using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIS.Model;
using PMIS.ViewModels;
using System.Web.Mvc;

namespace PMIS.ServiceLayer
{
    public interface IUserPhysicianService
    {
        User GetUserPhysician_By_Id(int id);
        IEnumerable<PhysicianDetailsViewModel> GetAllPhysician();
        IEnumerable<SelectListItem> GetPhysicianListItems();

    }
}
