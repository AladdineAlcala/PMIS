using System;
using System.Threading.Tasks;

namespace PMIS.ServiceLayer
{
    public interface IUnitOfWork:IDisposable
    {
        void Commit();
    }
}