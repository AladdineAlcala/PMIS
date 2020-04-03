using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PMIS.Model;
using PMIS.ViewModels;

namespace PMIS.ServiceLayer
{
    public class PhycisianServices : IPhycisianServices,IDisposable
    {
        private PMISEntities _pmisEntities = null;

        public PhycisianServices(PMISEntities pmisEntities)
        {
            this._pmisEntities = pmisEntities;
        }

        public Physician GetPhysician_By_Id(int id)
        {
            return _pmisEntities.Physicians.FirstOrDefault(t => t.Phys_id == id);
        }


        public void AddPhysician(Physician physician)
        {
            _pmisEntities.Physicians.Add(physician);
        }

        public void RemovePhysician(Physician physician)
        {
            _pmisEntities.Physicians.Remove(physician);
        }


        public void UpdatePhysician(Physician physician)
        {
            _pmisEntities.Physicians.Attach(physician);
            _pmisEntities.Entry(physician).State = EntityState.Modified;
        }

        public IEnumerable<PhysicianDetailsViewModel> GetAllPhysician()
        {
            return _pmisEntities.Physicians.Select(t => new PhysicianDetailsViewModel()
            {
                PhysId = t.Phys_id,
                PhysName = t.Phys_Fullname,
                PhysAbr = t.Phys_Abr
            });
        }

        public IEnumerable<SelectListItem> GetPhysicianListItems()
        {
            var physicianlistItems = _pmisEntities.Physicians.AsEnumerable().Select(t => new SelectListItem()
            {
                Value = t.Phys_id.ToString(),
                Text ="Dr. " + t.Phys_Fullname
                
            }).ToList();

            return new SelectList(physicianlistItems, "Value", "Text");
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
        // ~PatientServices() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }



        #endregion
    }
}