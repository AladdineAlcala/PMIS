using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using PMIS.Model;

namespace PMIS.ServiceLayer
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly PMISEntities _pmisEntities;

        //public UnitOfWork()
        //{
        //    _pmisEntities=new PMISEntities();
        //}
        public UnitOfWork(PMISEntities pmisEntities)
        {
            this._pmisEntities = pmisEntities;
        }

        public void Commit()
        {
           //var list = _pmisEntities.ChangeTracker.Entries().ToList();
            _pmisEntities.SaveChanges();
        }

       
        public void Dispose()
        {
           _pmisEntities.Dispose();
        }
    }
}