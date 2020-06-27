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
        Task<IEnumerable<PatientDetailsViewModel>> GetAllPatients();
        Task<PatientDetailsViewModel> GetPatientDetailsById(string patientid);
        Task<PatientDetailsViewModel> GetPatientDetailsByMedicalRecordNo(int medicalRecordNo);
        IQueryable<PatientViewModel> GetPatientAutoComplete();
        List<PhysicianDetailsViewModel> GetDoctorsByPatient(string id);
        Task<int> CountAllPatients();
        void InsertPatient(Patient patient);
        void RemovePatient(Patient patient);
        Patient GetPatientById(string id);
        void UpdatePatient(Patient patient);
    }
}