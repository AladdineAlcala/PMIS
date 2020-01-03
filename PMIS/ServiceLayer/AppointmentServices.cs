using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMIS.Model;
using PMIS.ViewModels;

namespace PMIS.ServiceLayer
{
    public class AppointmentServices
    {
        private PMISEntities pmisEntities = null;
        public AppointmentServices()
        {
            pmisEntities=new PMISEntities();
        }

        public IEnumerable<AppointmentScheduleViewModel> AppointScheduleList()
        {
            var appointmentSchedule = (from a in pmisEntities.Appointments
                select new AppointmentScheduleViewModel()
                {
                    No = a.No,
                    PatientNo = a.Pat_Id,
                    PatientName = a.Patient.Lastname + " ," + a.Patient.Firstname,
                    PhyId =(int) a.Phys_id,
                    PhyName = a.Physician.Phys_Fullname


                }).ToList();

            return appointmentSchedule;
        }
    }
}