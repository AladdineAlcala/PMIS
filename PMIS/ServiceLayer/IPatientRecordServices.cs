using System.Collections.Generic;
using System.Threading.Tasks;
using PMIS.Model;
using PMIS.ViewModels;

namespace PMIS.ServiceLayer
{
    public interface IPatientRecordServices
    {
       void AddMedicalRecord(MedicalRecord record);
        void RemoveMedicalRecord(MedicalRecord record);
       void AddMedication(Medication medrecord);
       void UpdateMedication(Medication medication);
       void RemoveMedication(Medication medication);
       Task<IEnumerable<MedicalRecord>> GetAllRecordsAsync();
       IEnumerable<PatientPhysicianDistinctViewModel> Get_Distinct_PhysicianByPatient();
       Medication GetMedication(int recordNo);
       Task<Medication> GetMedicationBymedNo(int medNo);
       Task <MedicalRecord> GetMedicalRecord(long recordNo);
       IEnumerable<MedicalRecord> GetAllRecords(string patId,string phyid);
       
    }
}