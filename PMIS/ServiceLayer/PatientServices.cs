using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PMIS.Model;
using PMIS.ViewModels;

namespace PMIS.ServiceLayer
{
    public class PatientServices: IPatientServices,IDisposable
    {
        private readonly PMISEntities _pmisEntities;

        //public PatientServices()
        //{
        //    _pmisEntities=new PMISEntities();
        //}

        public PatientServices(PMISEntities pmisEntities)
        {
            this._pmisEntities = pmisEntities;
        }


        public Dictionary<string, string> GetGenderDictionary()
        {
            var genderdictionary=new Dictionary<string,string>()
            {
                {"Female","Female" },
                {"Male","Male"}
            };

            return genderdictionary;
        }

        public Dictionary<string, string> Generate_CivilStatus_Dictionary()
        {
            var civilstatdictionary=new Dictionary<string,string>()
            {
                {"Single","Single" },
                {"Married","Married" },
                {"Widowed","Widowed" },
                {"Divorced","Divorced" },
            };
            return civilstatdictionary;
        }

        public async Task<IEnumerable<PatientDetailsViewModel>>  GetAllPatients()
        {
            return await _pmisEntities.Patients.Select(t => new PatientDetailsViewModel()
            {
                PatientId   = t.Pat_Id,
                Firstname = t.Firstname,
                Lastname = t.Lastname,
                Middle = t.Middle,
                Gender = t.Gender,
                AddStreetBrgy = t.AddStreetBrgy,
                Municipality = t.Muncity,
                Province = t.Province,
                DateofBirth = (DateTime) t.DoB,
                ContactCell = t.ContactCell,
                ProfileImage = t.Image
            }).ToListAsync();
        }


        public IQueryable<PatientViewModel> GetPatientAutoComplete()
        {
            return _pmisEntities.Patients.Select(t => new PatientViewModel()
            {
                PatientId = t.Pat_Id,
                PatientName = t.Lastname + " " + t.Firstname
            }).OrderBy(t=>t.PatientName);
        }

        public void InsertPatient(Patient patient)
        {
            _pmisEntities.Patients.Add(patient);
          
        }

        public void RemovePatient(Patient patient)
        {
            _pmisEntities.Patients.Remove(patient);
        }


        public Patient GetPatientById(string id)
        {
            return _pmisEntities.Patients.FirstOrDefault(t=>t.Pat_Id==id);
        }


        public void UpdatePatient(Patient patient)
        {
            _pmisEntities.Patients.Attach(patient);

           _pmisEntities.Entry(patient).State=EntityState.Modified;
        }

        public async Task<PatientDetailsViewModel> GetPatientDetailsById(string patientid)
        {
            return await _pmisEntities.Patients.Select(t => new PatientDetailsViewModel()
            {
                PatientId = t.Pat_Id,
                Firstname = t.Firstname,
                Lastname = t.Lastname,
                Middle = t.Middle,
                Gender = t.Gender,
                AddStreetBrgy = t.AddStreetBrgy,
                Municipality = t.Muncity,
                Province = t.Province,
                DateofBirth = (DateTime) t.DoB,
                ContactCell = t.ContactCell,
                ContactTell = t.ContactPhone,
                Height =t.Height==null?t.Height : 0,
                Weight =t.Weight == null?t.Weight : 0,
                BloodType = t.BType,
                Occupation = t.Occupation,
                Company = t.Company,
                GuardianName = t.GuardianName,
                GuardianContact = t.GuardianContact,
                GuardianRelation = t.GuardianRelation,
                ProfileImage = t.Image
                

            }).FirstOrDefaultAsync(t => t.PatientId == patientid);
        }

        public Task<PatientDetailsViewModel> GetPatientDetailsByMedicalRecordNo(int medicalRecordNo)
        {

            return (from mr in _pmisEntities.MedicalRecords
                join t in _pmisEntities.Patients on mr.Pat_Id equals t.Pat_Id where mr.RecordNo==medicalRecordNo select new PatientDetailsViewModel()
                {

                    PatientId = t.Pat_Id,
                    Firstname = t.Firstname,
                    Lastname = t.Lastname,
                    Middle = t.Middle,
                    Gender = t.Gender,
                    AddStreetBrgy = t.AddStreetBrgy,
                    Municipality = t.Muncity,
                    Province = t.Province,
                    DateofBirth = (DateTime)t.DoB,
                    ContactCell = t.ContactCell,
                    ContactTell = t.ContactPhone,
                    Height = t.Height == null ? t.Height : 0,
                    Weight = t.Weight == null ? t.Weight : 0,
                    BloodType = t.BType,
                    Occupation = t.Occupation,
                    Company = t.Company,
                    GuardianName = t.GuardianName,
                    GuardianContact = t.GuardianContact,
                    GuardianRelation = t.GuardianRelation,
                    ProfileImage = t.Image

                }).FirstOrDefaultAsync();
        }



        public List<PhysicianDetailsViewModel> GetDoctorsByPatient(string id)
        {

            return (from mr in _pmisEntities.MedicalRecords
                    join p in _pmisEntities.Users on mr.Phys_id equals p.Id
                    where mr.Pat_Id == id
                    group new { mr, p } by new { mr.Pat_Id, mr.Phys_id, p.Abr }
                into mrp
                    select new PhysicianDetailsViewModel()
                    {
                        PhysId = mrp.Key.Phys_id,
                        PhysName = mrp.Key.Abr

                    }).ToList();

        }


        public async Task<int> CountAllPatients()
        {
            return await _pmisEntities.Patients.CountAsync();
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _pmisEntities?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~PatientServices() {
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