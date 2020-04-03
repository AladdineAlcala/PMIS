using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PMIS.Model;
using PMIS.ViewModels;

namespace PMIS.ServiceLayer
{
    public class AppointmentServices:IAppointmentServices, IDisposable
    {
        private PMISEntities _pmisEntities = null;
        private readonly IPhycisianServices _phycisianService;
        private readonly IPatientServices _patientservice;

        //public AppointmentServices()
        //{
        //    _pmisEntities = new PMISEntities();
        //    _phycisianService = new PhycisianServices();
        //    _patientservice = new PatientServices();
        //}

        public AppointmentServices(PMISEntities pmisEntities,IPhycisianServices phycisianServices,IPatientServices patientServices)
        {
            this._pmisEntities = pmisEntities;
            this._phycisianService = phycisianServices;
            this._patientservice = patientServices;
        }


        public async Task<IEnumerable<AppointmentScheduleViewModel>>  GetAllAppointmentList()
        {

            return await _pmisEntities.Appointments.Select(t => new AppointmentScheduleViewModel()
            {
                No = t.No,
                PatientNo = t.Pat_Id,
                PatientName = t.Patient.Lastname + " ," + t.Patient.Firstname,
                PhyId = (int)t.Phys_id,
                PhyName = t.Physician.Phys_Fullname,
                AppointDate = (DateTime)t.AppointDate,
                Stat =(bool) t.Status?"Served":"Pending",
                Iscancelled = (bool) t.IsCancelled
                

            }).ToListAsync();
        }

        public IEnumerable<SelectListItem> GetAllDoctors()
        {
            return _phycisianService.GetPhysicianListItems();
        }

        public Dictionary<string,string> GetAllGender()
        {
            return _patientservice.GetGenderDictionary();
        }

        public void InsertAppointment(Appointment appointment)
        {
            _pmisEntities.Appointments.Add(appointment);
         
        }

        public bool CheckAppointment(string patId, int phyId, DateTime dateAppoint)
        {
           
            return _pmisEntities.Appointments.Any(t => t.Pat_Id == patId && t.Phys_id == phyId &&
                                                     t.AppointDate == dateAppoint);
        }

        public Appointment GetAppointment(int appId)
        {
            return _pmisEntities.Appointments.Find(appId);
        }

        
     

        public void RemoveAppointment(Appointment appointment)
        {
            _pmisEntities.Appointments.Remove(appointment);
        }

    

        public void ModifyAppointment(Appointment appointment)
        {
            _pmisEntities.Appointments.Attach(appointment);
            _pmisEntities.Entry(appointment).State = EntityState.Modified;
        }

        public Appointment LastVisited(string id)
        {
            var list = _pmisEntities.Appointments.Where(t => t.Pat_Id == id).OrderBy(t => t.AppointDate);

            Appointment lastvisit = null;

            if (list.Any())
            {
                lastvisit = new Appointment();

                lastvisit = list.ToList().Last();
            }

            return lastvisit;
        }

        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _pmisEntities?.Dispose();
                }
            }
            this._disposed = true;
        }

        public IEnumerable<AppointmentScheduleViewModel> GetAllAppointment()
        {
            return _pmisEntities.Appointments.Select(t => new AppointmentScheduleViewModel()
            {
                No = t.No,
                PatientNo = t.Pat_Id,
                PatientName = t.Patient.Lastname + " ," + t.Patient.Firstname,
                PhyId = (int)t.Phys_id,
                PhyName = t.Physician.Phys_Fullname,
                AppointDate = (DateTime)t.AppointDate,
                Stat = (bool)t.Status ? "Served" : "Pending",
                Iscancelled = (bool)t.IsCancelled


            }).ToList();
        }
    }
}