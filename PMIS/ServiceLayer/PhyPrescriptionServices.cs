﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using PMIS.Model;
using PMIS.ViewModels;

namespace PMIS.ServiceLayer
{
    public class PhyPrescriptionServices :IPhyPrescriptionServices, IDisposable
    {
        private readonly PMISEntities _pmisEntities;
        public PhyPrescriptionServices(PMISEntities pmisEntities)
        {
            this._pmisEntities = pmisEntities;
        }

        public async Task<DocPrescriptionRecord> FindDocPrescriptionByIdAsync(int id)
        {
            return await _pmisEntities.DocPrescriptionRecords.FirstOrDefaultAsync(t => t.No == id);
        }
        public void InsertDocPrescription(DocPrescriptionRecord docPrescription)
        {
            _pmisEntities.DocPrescriptionRecords.Add(docPrescription);
        }

        public void RemoveDocPrescription(DocPrescriptionRecord docPrescription)
        {
            _pmisEntities.DocPrescriptionRecords.Remove(docPrescription);
        }

        public void UpdateDocPrescription(DocPrescriptionRecord docPrescription)
        {
            _pmisEntities.DocPrescriptionRecords.Attach(docPrescription);
            _pmisEntities.Entry(docPrescription).State = EntityState.Modified;
        }

        public DocPrescriptionRecord FindDocPrescriptionById(int id)
        {
            return _pmisEntities.DocPrescriptionRecords.FirstOrDefault(t => t.No == id);
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
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~PhyPrescriptionServices() {
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