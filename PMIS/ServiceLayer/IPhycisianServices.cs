using System.Collections.Generic;
using System.Web.Mvc;
using PMIS.Model;
using PMIS.ViewModels;

namespace PMIS.ServiceLayer
{
    public interface IPhycisianServices
    {
        Physician GetPhysician_By_Id(int id);
        IEnumerable<PhysicianDetailsViewModel> GetAllPhysician();
        IEnumerable<SelectListItem> GetPhysicianListItems();
        void AddPhysician(Physician physician);
        void RemovePhysician(Physician physician);
        void UpdatePhysician(Physician physician);

    }
}