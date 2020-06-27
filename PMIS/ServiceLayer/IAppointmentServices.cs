using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using PMIS.Model;
using PMIS.ViewModels;

namespace PMIS.ServiceLayer
{
    public interface IAppointmentServices
    {
        Task<IEnumerable<AppointmentScheduleViewModel>>GetAllAppointmentList();
        Task<IEnumerable<AppointmentScheduleViewModel>> GetAllAppointmentList(string id, DateTime appointmentDate);
        IEnumerable<SelectListItem> GetAllDoctors();
        Dictionary<string,string> GetAllGender();
        Task<Appointment> GetAppointment(int appId);
        Appointment LastVisited(string id);
        void InsertAppointment(Appointment appointment);
        void ModifyAppointment(Appointment appointment);
        void RemoveAppointment(Appointment appointment);
        IEnumerable<AppointmentScheduleViewModel> GetAllAppointment();
        bool CheckAppointment(string patId, string phyId, DateTime dateAppoint);
        Task<IEnumerable<AppointmentScheduleViewModel>> GetAllAppointmentByPatient(string patientId);

    }
}