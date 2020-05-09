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
        Task<IEnumerable<AppointmentScheduleViewModel>>  GetAllAppointmentList();
        IEnumerable<SelectListItem> GetAllDoctors();
        Dictionary<string,string> GetAllGender();
        Appointment GetAppointment(int appId);
        Appointment LastVisited(string id);
        void InsertAppointment(Appointment appointment);
        void ModifyAppointment(Appointment appointment);
        void RemoveAppointment(Appointment appointment);
        IEnumerable<AppointmentScheduleViewModel> GetAllAppointment();
        bool CheckAppointment(string patId, string phyId, DateTime dateAppoint);
    }
}