using System.Collections.Generic;
using System.Threading.Tasks;
using PMIS.Model;
using PMIS.ViewModels;

namespace PMIS.ServiceLayer
{
    public interface IPatientRecordServices
    {
       void AddMedicalRecord(MedicalRecord record);
       void AddMedication(Medication medrecord);
       void UpdateMedication(Medication medication);
       Task<IEnumerable<MedicalRecord>> GetAllRecordsAsync();
       IEnumerable<PatientPhysicianDistinctViewModel> Get_Distinct_PhysicianByPatient();
       Medication GetMedication(int recordNo);
       Task <MedicalRecord> GetMedicalRecord(long recordNo);
      IEnumerable<MedicalRecord> GetAllRecords(string patId,int phyid);
       
    }
}