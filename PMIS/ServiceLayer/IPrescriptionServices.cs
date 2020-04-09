using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using PMIS.Model;
using PMIS.ViewModels;

namespace PMIS.ServiceLayer
{
    public interface IPrescriptionServices
    {
        Task<IEnumerable<PrescriptionViewModel>> GetAllPrescriptions();
        Task<Prescription> GetPrescriptionByIdAsync(int id);
        void InsertPrescription(Prescription prescription);
        void UpdatePrescription(Prescription prescription);
        void RemovePrescription(Prescription prescription);
        IEnumerable<SelectListItem> GetCategoryListItems();
        IEnumerable<SelectListItem> GetPrescriptionListItems(int? catId);
    }
}