using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using PMIS.Model;

namespace PMIS.ServiceLayer
{
   public interface IPhyPrescriptionServices
    {
        void InsertDocPrescription(DocPrescriptionRecord docPrescription);
        Task<DocPrescriptionRecord> FindDocPrescriptionByIdAsync(int id);
        DocPrescriptionRecord FindDocPrescriptionById(int id);
        void RemoveDocPrescription(DocPrescriptionRecord docPrescription);
        void UpdateDocPrescription(DocPrescriptionRecord docPrescription);
      
 
    }
}
