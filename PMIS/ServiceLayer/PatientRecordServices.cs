using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using PMIS.Model;
using PMIS.ViewModels;

namespace PMIS.ServiceLayer
{
    public class PatientRecordServices : IPatientRecordServices,IDisposable
    {
        private readonly PMISEntities _pmisEntities;
        public PatientRecordServices(PMISEntities pmisEntities)
        {
            this._pmisEntities = pmisEntities;
          
        }

        public void AddMedicalRecord(MedicalRecord record)
        {
            _pmisEntities.MedicalRecords.Add(record);
        }

    
        public void AddMedication(Medication medrecord)
        {
            _pmisEntities.Medications.Add(medrecord);
        }

        public void UpdateMedication(Medication medication)
        {
            _pmisEntities.Medications.Attach(medication);
            _pmisEntities.Entry(medication).State=EntityState.Modified;
        }


        public async Task<IEnumerable<MedicalRecord>> GetAllRecordsAsync()
        {
            return await _pmisEntities.MedicalRecords.ToListAsync();
        }

        public async Task<MedicalRecord> GetMedicalRecord(long recordNo)
        {
            return await  _pmisEntities.MedicalRecords.FindAsync(recordNo);
        }


        public Medication GetMedication(int recordNo)
        {
            return _pmisEntities.Medications.FirstOrDefault(t => t.RecordNo == recordNo);
        }



        public IEnumerable<PatientPhysicianDistinctViewModel> Get_Distinct_PhysicianByPatient()
        {
            return (_pmisEntities.MedicalRecords.Select(m => new PatientPhysicianDistinctViewModel()
            {
                PatientId = m.Pat_Id,
                Phyid = (int) m.Phys_id,
                PhyscianName = m.Physician.Phys_Fullname
            })).Distinct().ToList();

        }


        public IEnumerable<MedicalRecord> GetAllRecords(string patId, int phyid)
        {
            return _pmisEntities.MedicalRecords.Where(t => t.Pat_Id == patId && t.Phys_id == phyid);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _pmisEntities.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~PatientRecordServices() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }









        #endregion


    }

    
}