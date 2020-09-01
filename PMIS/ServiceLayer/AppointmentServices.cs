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
        private readonly IUserPhysicianService _userPhysicianService;
        private readonly IPatientServices _patientServices;

        //public AppointmentServices()
        //{
        //    _pmisEntities = new PMISEntities();
        //    _phycisianService = new PhycisianServices();
        //    _patientservice = new PatientServices();
        //}

        public AppointmentServices(PMISEntities pmisEntities, IUserPhysicianService userPhysicianService, IPatientServices patientServices1)
        {
            this._pmisEntities = pmisEntities;
            _userPhysicianService = userPhysicianService;
            _patientServices = patientServices1;
        }


        public async Task<IEnumerable<AppointmentScheduleViewModel>>  GetAllAppointmentList()
        {

            return await _pmisEntities.Appointments.Select(t => new AppointmentScheduleViewModel()
            {
                No = t.No,
                PatientNo = t.Pat_Id,
                PatientName = t.Patient.Lastname + " ," + t.Patient.Firstname,
                PhyId =t.User.Id,
                PhyName =t.User.Abr,
                AppointDate = (DateTime)t.AppointDate,
                Stat =(bool) t.Status?"Served":"Pending",
                Iscancelled = (bool) t.IsCancelled,
                BlStat = (bool)t.Status


            }).ToListAsync();
        }

        public async Task<IEnumerable<AppointmentScheduleViewModel>> GetAllAppointmentList(string id, DateTime appointmentDate)
        {
            return await _pmisEntities.Appointments.Where(t=>t.Phys_id==id && t.AppointDate.Value==appointmentDate.Date) .Select(t => new AppointmentScheduleViewModel()
            {
                No = t.No,
                PatientNo = t.Pat_Id,
                PatientName = t.Patient.Lastname + " ," + t.Patient.Firstname,
                PhyId = t.User.Id,
                PhyName = t.User.Abr,
                AppointDate = (DateTime)t.AppointDate,
                Stat = (bool)t.Status ? "Served" : "Pending",
                Iscancelled = (bool)t.IsCancelled,
                BlStat = (bool)t.Status

            }).ToListAsync();
        }

        public async Task<IEnumerable<AppointmentScheduleViewModel>> GetAllAppointmentByPatient(string patientId)
        {
            return await _pmisEntities.Appointments.Where(t =>t.Pat_Id==patientId).Select(t => new AppointmentScheduleViewModel()
            {
                No = t.No,
                PatientNo = t.Pat_Id,
                PatientName = t.Patient.Lastname + " ," + t.Patient.Firstname,
                PhyId = t.User.Id,
                PhyName = t.User.Abr,
                AppointDate = (DateTime)t.AppointDate,
                Stat = (bool)t.Status ? "Served" : "Pending",
                Iscancelled = (bool)t.IsCancelled


            }).OrderByDescending(t=>t.AppointDate).ToListAsync();
        }


        public IEnumerable<SelectListItem> GetAllDoctors()
        {
            return _userPhysicianService.GetPhysicianListItems();
        }

        public Dictionary<string,string> GetAllGender()
        {
            return _patientServices.GetGenderDictionary();
        }

        public void InsertAppointment(Appointment appointment)
        {
            _pmisEntities.Appointments.Add(appointment);
         
        }

        public bool CheckAppointment(string patId, string phyId, DateTime dateAppoint)
        {
           
            return _pmisEntities.Appointments.Any(t => t.Pat_Id == patId && t.Phys_id == phyId &&
                                                     t.AppointDate == dateAppoint);
        }

        public async Task<Appointment> GetAppointment(int appId)
        {
            return await _pmisEntities.Appointments.FindAsync(appId);
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

        public IEnumerable<AppointmentScheduleViewModel> GetAllAppointment()
        {
            return _pmisEntities.Appointments.Select(t => new AppointmentScheduleViewModel()
            {
                No = t.No,
                PatientNo = t.Pat_Id,
                PatientName = t.Patient.Lastname + " ," + t.Patient.Firstname,
                PhyId = t.User.Id,
                PhyName = t.User.Abr,
                AppointDate = (DateTime)t.AppointDate,
                Stat = (bool)t.Status ? "Served" : "Pending",
                Iscancelled = (bool)t.IsCancelled,
                BlStat = (bool) t.Status


            }).ToList();
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

      
    }
}