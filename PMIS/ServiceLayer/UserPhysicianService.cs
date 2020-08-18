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
    public class UserPhysicianService : IUserPhysicianService,IDisposable
    {
        private readonly PMISEntities _pmisEntities = null;
       
        public UserPhysicianService(PMISEntities pmisEntities)
        {
            this._pmisEntities = pmisEntities;
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
        // ~UserPhysicianService() {
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

        public User GetUserPhysician_By_Id(string id)
        {
            return _pmisEntities.Users.FirstOrDefault(t => t.Id == id);
        }

        public IEnumerable<PhysicianDetailsViewModel> GetAllPhysician()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SelectListItem> GetPhysicianListItems()
        {
            UsersViewModel users = new UsersViewModel();

            var physicianlist = (from v in users.listofUsers().Where(t => t.roles.Contains("doctor"))
                select new UsersViewModel()
                {
                    userId = v.userId,
                    abr = v.abr,
                    roles = v.roles,
                    has_superadminRights = v.has_superadminRights
                }).ToList();

            return new SelectList(physicianlist.AsEnumerable().Select(t=>new SelectListItem()
                {
                    Value = t.userId,
                    Text = t.abr
                }).ToList(), "Value", "Text"
               );

        }









        #endregion
    }
}