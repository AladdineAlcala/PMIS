using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PMIS.Model;
using PMIS.ViewModels;

namespace PMIS.ServiceLayer
{
    public class PrescriptionServices:IDisposable, IPrescriptionServices
    {
        private readonly PMISEntities _pmisEntities;

        public PrescriptionServices(PMISEntities pmisEntities)
        {
            _pmisEntities = pmisEntities;
        }

        public async Task<IEnumerable<PrescriptionViewModel>> GetAllPrescriptions()
        {
            return await _pmisEntities.Prescriptions.Select(t => new PrescriptionViewModel()
                        {
                            Id=(int) t.PresId,
                            PrescriptionCatid = (int) t.CatId,
                            PrescriptionDetails = t.PrescriptionDetails,
                            PrescriptionCatDetails = t.PresCat.CatDesc,
                            PrescUnit =   t.Unit
                        }).ToListAsync();
        }

        public async Task<Prescription> GetPrescriptionByIdAsync(int id)
        {
            return await _pmisEntities.Prescriptions.FirstOrDefaultAsync(t => t.PresId == id);
        }
        public void InsertPrescription(Prescription prescription)
        {
            _pmisEntities.Prescriptions.Add(prescription);
        }

        public void UpdatePrescription(Prescription prescription)
        {
            _pmisEntities.Prescriptions.Attach(prescription);

            _pmisEntities.Entry(prescription).State = EntityState.Modified;
        }

        public void RemovePrescription(Prescription prescription)
        {
            _pmisEntities.Prescriptions.Remove(prescription);
        }

        public IEnumerable<SelectListItem> GetCategoryListItems()
        {
            return _pmisEntities.PresCats.AsEnumerable().Select(t => new SelectListItem()
            {
                Text = t.CatDesc,
                Value = t.CatId.ToString()
            });
        }

        public IEnumerable<SelectListItem> GetPrescriptionListItems(int? catId)
        {
            List<Prescription> prescriptionlist = new List<Prescription>();
            prescriptionlist = catId != null ? _pmisEntities.Prescriptions.Where(t => t.CatId == catId).ToList() : _pmisEntities.Prescriptions.ToList();
            return prescriptionlist.AsEnumerable().Select(t => new SelectListItem()
            {
                Text = t.PrescriptionDetails,
                Value = t.PresId.ToString()

            });
        }

        public IEnumerable<DocPrescriptionViewModel> GetDocPrescriptionByRecNo(int recNo)
        {
            return _pmisEntities.DocPrescriptionRecords.Select(t => new DocPrescriptionViewModel()
            {
                PrescNo = (int)t.No,
                RecNo = (int)t.RecordNo,
                PrescId = (int)t.PresId,
                PrescriptionDetails = t.Prescription.PrescriptionDetails,
                Sig = t.Sig,
                DispInst = t.Disp

            }).Where(t => t.RecNo == recNo).ToList();
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _pmisEntities?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Prescription() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

      






        #endregion


    }
}