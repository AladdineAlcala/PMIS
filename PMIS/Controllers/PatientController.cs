using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PMIS.HelperClass;
using PMIS.Model;
using PMIS.ServiceLayer;
using PMIS.ViewModels;

namespace PMIS.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        private readonly IPatientServices _patientServices;
        private readonly IAppointmentServices _appointmentServices;
        private readonly IUnitOfWork _unitofwork;
        public PatientController(IPatientServices patientServices,IAppointmentServices appointmentServices, IUnitOfWork unitofwork)
        {
            this._patientServices = patientServices;
            this._unitofwork = unitofwork;
            this._appointmentServices = appointmentServices;
        }

        // GET: Patient
        public ActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAllPatientsAsync()
        {
            var patientlist = await _patientServices.GetAllPatients();

            return Json(new { data = patientlist }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPatientAutoComplete(string query)
        {

            var patients = _patientServices.GetPatientAutoComplete().Where(t => t.PatientName.Contains(query))
                .OrderBy(t => t.PatientName).Take(10).ToList();

            return Json(patients, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CreatePatientProfile()
        {

            var patientModel = new PatientDetailsViewModel
            {
                GenderDictionary = _patientServices.GetGenderDictionary()

            };


            return View(patientModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePatientProfile(PatientDetailsViewModel patientDetails)
        {
            if (!ModelState.IsValid)
            {
                patientDetails.GenderDictionary = _patientServices.GetGenderDictionary();

                return View(patientDetails);
            }

            DateTime dateofBirth = new DateTime(patientDetails.yearint, patientDetails.monthint, patientDetails.dateint);

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

            _patientServices.InsertPatient(patient);
            _unitofwork.Commit();

            //var url = @Url.Action("Index", "Patient");
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemovePatientProfile(string patientId)
        {
            var success = false;
            var patient = _patientServices.GetPatientById(patientId);

            if (patient != null)
            {
                _patientServices.RemovePatient(patient);
                _unitofwork.Commit();

                success = true;
            }

            return Json(new { success = success }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditPatient()
        {
            var patient = _patientServices.GetPatientById(RouteData.Values["id"].ToString());
     
            return View(patient);
        }

        [ChildActionOnly]
        public PartialViewResult GetBasicInfo(Patient patient)
        {
           
            var patientdetail=new PatientDetailsBasicInfoViewModel()
            {
                PatientId = patient.Pat_Id,
                Firstname = patient.Firstname,
                Lastname = patient.Lastname,
                Middle = patient.Middle,
                Gender = patient.Gender,
                AddStreetBrgy = patient.AddStreetBrgy,
                Municipality = patient.Muncity,
                Province = patient.Province,
                ContactCell = patient.ContactCell,
                monthint = patient.DoB?.Month ?? DateTime.Now.Month,
                dateint = patient.DoB?.Day ?? DateTime.Now.Day,
                yearint = patient.DoB?.Year ?? DateTime.Now.Year,

                GenderDictionary = _patientServices.GetGenderDictionary()
            };

            return PartialView("_EditBasicInfoPartialView", patientdetail);
        }

        [ChildActionOnly]
        public PartialViewResult GetAdvanceInfo(Patient patient)
        {
            var patientadvnceinfo = new PatientDetailsAdvanceInfoViewModel()
            {
                PatientId = patient.Pat_Id,
                CivilStat = patient.CivilStat,
                CivilStatDictionary = _patientServices.Generate_CivilStatus_Dictionary()
            };

            return PartialView("_EditAdvanceInfoPartialView", patientadvnceinfo);
        }



        [ActionName("EditBasic")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPatient(PatientDetailsBasicInfoViewModel patientBasic)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_EditBasicInfoPartialView", patientBasic);
            }


            var patientbasicinfo = _patientServices.GetPatientById(patientBasic.PatientId);

                patientbasicinfo.Firstname = patientBasic.Firstname;
                patientbasicinfo.Lastname = patientBasic.Lastname;
                patientbasicinfo.Middle = patientBasic.Middle;
                patientbasicinfo.DoB = new DateTime(patientBasic.yearint, patientBasic.monthint, patientBasic.dateint);
                patientbasicinfo.AddStreetBrgy = patientBasic.AddStreetBrgy;
                patientbasicinfo.Muncity = patientBasic.Municipality;
                patientbasicinfo.Province = patientBasic.Province;
                patientbasicinfo.Gender = patientBasic.Gender;
                patientbasicinfo.ContactCell = patientBasic.ContactCell;


            _patientServices.UpdatePatient(patientbasicinfo);
            _unitofwork.Commit();


            return Json(new {success = true}, JsonRequestBehavior.AllowGet);
        }


        [ActionName("EditAdvance")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPatient(PatientDetailsAdvanceInfoViewModel patientAdvance)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_EditAdvanceInfoPartialView", patientAdvance);
            }


            var patientdetailsAdvanceinfo = _patientServices.GetPatientById(patientAdvance.PatientId);

            patientdetailsAdvanceinfo.CivilStat = patientAdvance.CivilStat;
            patientdetailsAdvanceinfo.ContactPhone = patientAdvance.ContactTell;
            patientdetailsAdvanceinfo.Height = patientAdvance.Height;
            patientdetailsAdvanceinfo.Weight = patientAdvance.Weight;
            patientdetailsAdvanceinfo.Occupation = patientAdvance.Occupation;
            patientdetailsAdvanceinfo.Company = patientAdvance.Company;
            patientdetailsAdvanceinfo.BType = patientAdvance.BloodType;
            patientdetailsAdvanceinfo.GuardianName = patientAdvance.GuardianName;
            patientdetailsAdvanceinfo.GuardianContact = patientAdvance.GuardianContact;
            patientdetailsAdvanceinfo.GuardianRelation = patientAdvance.GuardianRelation;

            _patientServices.UpdatePatient(patientdetailsAdvanceinfo);
            _unitofwork.Commit();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewProfile(string id)
        {
            var patientprofile = new PatientProfileViewModel()
            {
                PatientId = id,
                PatientDetails = _patientServices.GetPatientDetailsById(id),
                Listofdoctors = _patientServices.GetDoctorsByPatient(id),
                Lastvisited = _appointmentServices.LastVisited(id)
            };

        
            return View(patientprofile);
        }


        [HttpGet]
        public ActionResult ProfileImageModal()
        {
           
            return PartialView("ProfileImageModal");
        }

        [HttpPost]
        public ActionResult SaveProfileImage(ProfileImageViewModel viewModel)
        {
            string randomFileName = null;

            if (viewModel != null)
            {
                var t = viewModel.base64Image.Substring(23);
                byte[] bytes = Convert.FromBase64String(t);

                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    var image = Image.FromStream(ms);

                     randomFileName = Guid.NewGuid().ToString().Substring(0, 4) + ".jpg";
                     var fullPath = Path.Combine(Server.MapPath("~/Content/Images/"), randomFileName);
                     image.Save(fullPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }

                //save image filename to database
                

            }

           // System.IO.File.WriteAllText(Server.MapPath("~/Content/Images/" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".txt"), base64Image);

            return Json(new {success = true,randomFileName}, JsonRequestBehavior.AllowGet);
        }
    }
}