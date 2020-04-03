using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PMIS.Model;
using PMIS.ViewModels;

namespace PMIS.ServiceLayer
{
    public interface IPatientServices
    {
        Dictionary<string, string> GetGenderDictionary();
        Dictionary<string, string> Generate_CivilStatus_Dictionary();
        Task<IEnumerable<PatientDetailsViewModel>>  GetAllPatients();
        PatientDetailsViewModel GetPatientDetailsById(string patientid);
        IQueryable<PatientViewModel> GetPatientAutoComplete();
        List<PhysicianDetailsViewModel> GetDoctorsByPatient(string id);
        void InsertPatient(Patient patient);
        void RemovePatient(Patient patient);
        Patient GetPatientById(string id);
        void UpdatePatient(Patient patient);
    }
}