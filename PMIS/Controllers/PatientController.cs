using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PMIS.HelperClass;
using PMIS.Model;
using PMIS.ServiceLayer;
using PMIS.ViewModels;

namespace PMIS.Controllers
{
    public class PatientController : Controller
    {
        private readonly PatientServices patientservice;
        private readonly PMISEntities pmisEntities;
        public PatientController()
        {
            patientservice = new PatientServices();
            pmisEntities=new PMISEntities();
        }



        // GET: Patient
        public ActionResult Index()
        {

            return View();
        }


        [HttpGet]
        public JsonResult GetAllPatients()
        {
         
          var patientlist=patientservice.GetAllPatients().ToList();

            return Json(new {data=patientlist},JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPatientAutoComplete(string query)
        {

            var patients = patientservice.GetPatientAutoComplete().Where(t => t.PatientName.Contains(query))
                .OrderBy(t => t.PatientName).Take(10).ToList();

            return Json(patients,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CreatePatientProfile()
        {

            var patientModel = new PatientDetailsViewModel
            {
                GenderDictionary = patientservice.GetGenderDictionary()
             
            };


            return View(patientModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePatientProfile(PatientDetailsViewModel patientDetails)
        {
            if (!ModelState.IsValid)
            {
                patientDetails.GenderDictionary = patientservice.GetGenderDictionary();

                return View(patientDetails);
            }

            DateTime dateofBirth=new DateTime(patientDetails.yearint,patientDetails.monthint,patientDetails.dateint);

            var patient = new Patient()
            {
                Pat_Id = Utilities.GeneratePatientId(),
                Firstname = patientDetails.Firstname,
                Lastname = patientDetails.Lastname,
                Middle = patientDetails.Middle,
                DoB = dateofBirth,
                DateRegister = DateTime.Now,
                AddStreetBrgy = patientDetails.AddStreetBrgy,
                Muncity = patientDetails.Municipality,
                Province = patientDetails.Province,
                Gender = patientDetails.Gender,
                ContactCell = patientDetails.ContactCell

            };

            pmisEntities.Patients.Add(patient);
            pmisEntities.SaveChanges();

            //var url = @Url.Action("Index", "Patient");
            return Json(new {success=true}, JsonRequestBehavior.AllowGet);
        }


        //remove patient
        [HttpPost]
        public ActionResult RemovePatientProfile(string patientId)
        {

            var patient = pmisEntities.Patients.Find(patientId);

            pmisEntities.Patients.Remove(patient);
            pmisEntities.SaveChanges();

            return Json(new {success=true}, JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
          pmisEntities.Dispose();
        }
    }
}