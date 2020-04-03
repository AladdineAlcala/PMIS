using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMIS.Model;

namespace PMIS.ServiceLayer
{
    public interface IUserPhysicianService
    {
        string GetPhysicianUserId(int userid);
        int GetPhysicianId(string loginId);
        void InsertPhysicianUser(User_Physician userPhysician);
    }
}
