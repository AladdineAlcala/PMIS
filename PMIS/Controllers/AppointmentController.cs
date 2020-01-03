using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Configuration;
using PMIS.HelperClass;
using PMIS.Model;
using PMIS.ServiceLayer;
using PMIS.ViewModels;

namespace PMIS.Controllers
{
    public class AppointmentController : Controller
    {
        private PMISEntities _pmisEntities = null;
        private readonly PhycisianServices _phycisianService;
        private readonly AppointmentServices _appointmentServices;
        private readonly PatientServices _patientservice;

        public AppointmentController()
        {
            _pmisEntities=new PMISEntities();
            _phycisianService=new PhycisianServices();
            _appointmentServices=new AppointmentServices();
            _patientservice=new PatientServices();
        }

        // GET: Appointment
        public ActionResult Index()
        {
            var appointment = new AppointmentViewModel()
            {
                PhysicianListItems = _phycisianService.GetPhysicianListItems()
            };
            return View(appointment);
        }

        [HttpGet]
        public ActionResult CreateAppointment()
        {
            return PartialView("_CreateAppointment");
        }

        [HttpGet]
        public ActionResult LoadAppointOldPatientPartialView()
        {
            var patientAppointmentOld =
                new PatientAppointmentOldViewModel
                {
                    AppointDate = DateTime.Now,
                    PhysicianListItems = _phycisianService.GetPhysicianListItems()
                };

            return PartialView("_AppointOldPatientPartialView", patientAppointmentOld);
        }


        [HttpGet]
        public ActionResult LoadAppointNewPatientPartialView()
        {
            var patientAppointNew=new PatientAppointmentNewViewModel()
            {
                GenderDictionary = _patientservice.GetGenderDictionary(),
                AppointDate = DateTime.Now,
                PhysicianListItems = _phycisianService.GetPhysicianListItems()
            };
           
            return PartialView("_AppointNewPatientPartialView", patientAppointNew);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAppointmentOld(PatientAppointmentOldViewModel appointmentold)
        {
            if (!ModelState.IsValid)
            {

                var patientAppointment =
                    new PatientAppointmentOldViewModel
                    {
                        AppointDate = DateTime.Now,
                        PhysicianListItems = _phycisianService.GetPhysicianListItems()
                    };

                return PartialView("_CreateAppointment", patientAppointment);

            }

            Appointment appointment = new Appointment()
            {
                AppointDate = appointmentold.AppointDate,
                Pat_Id = appointmentold.PatientId,
                Phys_id = appointmentold.PhysId,
                PriorNo = 1,
                Status = false

            };

            _pmisEntities.Appointments.Add(appointment);
            _pmisEntities.SaveChanges();

            var url = Url.Action("GetAppointList", "Appointment");

            return Json(new {success=true,url=url}, JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAppointmentNew(PatientAppointmentNewViewModel newPatientAppointment)
        {
            if (!ModelState.IsValid)
            {
                var patientAppointNew = new PatientAppointmentNewViewModel()
                {
                    GenderDictionary = _patientservice.GetGenderDictionary(),
                    AppointDate = DateTime.Now,
                    PhysicianListItems = _phycisianService.GetPhysicianListItems()
                };

                return PartialView("_AppointNewPatientPartialView", patientAppointNew);
            }


            var patientId = Utilities.GeneratePatientId();
            var basicPatientAppointInfo = new Patient()
            {
                Pat_Id = patientId,
                Firstname = newPatientAppointment.Firstname,
                Lastname = newPatientAppointment.Lastname,
                Middle = newPatientAppointment.Middle,
                DoB = new DateTime(newPatientAppointment.yearint, newPatientAppointment.monthint, newPatientAppointment.dateint),
                AddStreetBrgy = newPatientAppointment.AddStreetBrgy,
                DateRegister = DateTime.Now,
                Muncity = newPatientAppointment.Municipality,
                Province = newPatientAppointment.Province,
                Gender = newPatientAppointment.Gender,
                ContactCell = newPatientAppointment.ContactCell
            };
            _pmisEntities.Patients.Add(basicPatientAppointInfo);

            Appointment appointment = new Appointment()
            {
                AppointDate = newPatientAppointment.AppointDate,
                Pat_Id = patientId,
                Phys_id = newPatientAppointment.PhysId,
                PriorNo = 1,
                Status = false

            };

            _pmisEntities.Patients.Add(basicPatientAppointInfo);
            _pmisEntities.Appointments.Add(appointment);

            _pmisEntities.SaveChanges();

            var url = Url.Action("GetAppointList", "Appointment");

            return Json(new {success=true,url=url}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAppointList()
        {
            var appointSchedule = _appointmentServices.AppointScheduleList();

            return PartialView("_appointListPartialView", appointSchedule);

        }

        //[HttpGet]
        //public JsonResult GetAppointCount() //change to async task
        //{
        //   

        //    return Json(appointcount, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public JsonResult GetAppointCount(int? id)
        {

            int count = 0;

            if (id != null)
            {
                var appointcount = _pmisEntities.Appointments.ToList();

                count = appointcount.Where(t => t.Phys_id == id).ToList().Count;
            }


            return Json(count, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAppointment_By_Doctor(int id)
        {
            var appointSchedulebydoctor = _appointmentServices.AppointScheduleList().Where(t=>t.PhyId==id).ToList();


            return PartialView("_appointListPartialView", appointSchedulebydoctor);
        }
        protected override void Dispose(bool disposing)
        {
           _pmisEntities.Dispose();
        }
    }
}