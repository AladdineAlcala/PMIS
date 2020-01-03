using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PMIS.Model;
using PMIS.ViewModels;

namespace PMIS.ServiceLayer
{
    public class PatientServices
    {
        private PMISEntities pmisEntities=null;

        public PatientServices()
        {
            pmisEntities=new PMISEntities();
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

        public IQueryable<PatientDetailsViewModel> GetAllPatients()
        {
            return pmisEntities.Patients.Select(t => new PatientDetailsViewModel()
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
                ContactCell = t.ContactCell
            });
        }


        public IQueryable<PatientViewModel> GetPatientAutoComplete()
        {
            return pmisEntities.Patients.Select(t => new PatientViewModel()
            {
                PatientId = t.Pat_Id,
                PatientName = t.Lastname + " " + t.Firstname
            }).OrderBy(t=>t.PatientName);
        }

        
    }
}