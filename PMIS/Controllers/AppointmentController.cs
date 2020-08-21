using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Configuration;
using Microsoft.AspNet.SignalR;
using PMIS.HelperClass;
using PMIS.Hubs;
using PMIS.Model;
using PMIS.ServiceLayer;
using PMIS.ViewModels;

namespace PMIS.Controllers
{
    [System.Web.Mvc.Authorize]
    public class AppointmentController : Controller
    {
       
        private readonly IAppointmentServices _appointmentServices;
        private readonly IPatientRecordServices _patientRecordServices;
        private readonly IPatientServices _patientServices;
        private readonly IUnitOfWork _unitofwork;
        private AppointHub appointHub;
        //private readonly IUserPhysicianService _userPhysicianService;


        public AppointmentController(IAppointmentServices appointmentServices,IPatientServices patientServices,IUnitOfWork unitofwork, IUserPhysicianService userPhysicianService, IPatientRecordServices patientRecordServices)
        {
            _appointmentServices = appointmentServices;
            _patientServices = patientServices;
            _unitofwork = unitofwork;
            //_userPhysicianService = userPhysicianService;
            _patientRecordServices = patientRecordServices;
        }


        // GET: Appointment
        public ActionResult Index()
        {
            var appointment = new AppointmentViewModel()
            {
               PhysicianListItems =_appointmentServices.GetAllDoctors()
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
                    //AppointDate = DateTime.Now,
                    PhysicianListItems = _appointmentServices.GetAllDoctors()
                };

            return PartialView("_AppointOldPatientPartialView", patientAppointmentOld);
        }

        [HttpGet]
        public ActionResult LoadAppointNewPatientPartialView()
        {
            var patientAppointNew = new PatientAppointmentNewViewModel()
            {
                GenderDictionary =_appointmentServices.GetAllGender(),
                AppointDate = DateTime.Now,
                PhysicianListItems = _appointmentServices.GetAllDoctors()
            };

            return PartialView("_AppointNewPatientPartialView", patientAppointNew);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAppointmentOld(PatientAppointmentOldViewModel appointmentold)
        {
            bool success = false;



            if (!ModelState.IsValid)
            {

                var patientAppointment =
                    new PatientAppointmentOldViewModel
                    {
                        PhysicianListItems = _appointmentServices.GetAllDoctors()
                    };

                return PartialView("_CreateAppointment", patientAppointment);

            }

            //var context = GlobalHost.ConnectionManager.GetHubContext<AppointHub>();

            var appointment = new Appointment()
            {
                AppointDate = appointmentold.AppointDate,
                Pat_Id = appointmentold.PatientId,
                Phys_id = appointmentold.PhysId,
                PriorNo = 1,
                Status = false,
                IsCancelled = false

            };
           

            var hasAppointmentExist =
                _appointmentServices.CheckAppointment(appointmentold.PatientId,appointmentold.PhysId,appointmentold.AppointDate);



            if (!hasAppointmentExist)
            {
                success = true;

                _appointmentServices.InsertAppointment(appointment);

                appointHub = new AppointHub();

           
           
                _unitofwork.Commit();

                var appointSchedulebydoctor = await _appointmentServices.GetAllAppointmentList();

                if(appointment.Phys_id != null)

                appointHub.SendAppointment(appointment.Phys_id, appointSchedulebydoctor.Where(t => t.PhyId == appointment.Phys_id && t.AppointDate == appointment.AppointDate).ToList());


            }
           

            var url = Url.Action("GetAppointment_By_Doctor", "Appointment",new {id=appointment.Phys_id,appdate=appointment.AppointDate});


            return Json(new { success = success, url=url}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAppointmentNew(PatientAppointmentNewViewModel newPatientAppointment)
        {
            if (!ModelState.IsValid)
            {
                var patientAppointNew = new PatientAppointmentNewViewModel()
                {
                    GenderDictionary = _appointmentServices.GetAllGender(),
                    PhysicianListItems = _appointmentServices.GetAllDoctors()
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

            var appointment = new Appointment()
            {
                AppointDate = newPatientAppointment.AppointDate,
                Pat_Id = patientId,
                Phys_id = newPatientAppointment.Phys_Id,
                PriorNo = 1,
                Status = false,
                IsCancelled = false
                

            };
            _patientServices.InsertPatient(basicPatientAppointInfo);
            _appointmentServices.InsertAppointment(appointment);
            _unitofwork.Commit();
            
            var url = Url.Action("GetAppointment_By_Doctor", "Appointment", new { id= appointment.Phys_id, appdate = appointment.AppointDate });

            return Json(new { success = true, url = url }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public async Task<ActionResult> GetAppointment_By_Doctor(string id, DateTime appdate)
        {
            var appointSchedulebydoctor = await _appointmentServices.GetAllAppointmentList();

            return PartialView("_appointListPartialView",appointSchedulebydoctor.Where(t => t.PhyId == id && t.AppointDate == appdate));
        }

        [HttpGet]
        public async Task<JsonResult> GetAppointCount(string id, DateTime appdate)
        {

            int count = 0;

            if (id != null)
            {
                var appointment = await _appointmentServices.GetAllAppointmentList();

                count = appointment.Where(t => t.PhyId == id && t.AppointDate.Date==appdate.Date).ToList().Count;
            }

            return Json(count, JsonRequestBehavior.AllowGet);
        }

        

        [HttpGet]
        public ActionResult AppointmentOptions(int appno)
        {
            var appointoptionsviewmodel = new AppointmentOptionsViewModel()
            {
                AppNo = appno
            };

            return PartialView("_AppointmentOptions", appointoptionsviewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AppointmentOptions(AppointmentOptionsViewModel appointmentoption)
        {
            bool success = false;

            if (!ModelState.IsValid)
            {
                return PartialView("_AppointmentOptions", appointmentoption);
            }

            var appointment =await _appointmentServices.GetAppointment(appointmentoption.AppNo);
        
            string phyId = appointment.Phys_id;
            DateTime appdate = (DateTime)appointment.AppointDate;


            switch (appointmentoption.AppOptions)
            {

                case AppointOptions.Cancel:

                    appointment.IsCancelled = true;
                    _appointmentServices.ModifyAppointment(appointment);
                    success = true;
                    break;

                case AppointOptions.Remove:
                        _appointmentServices.RemoveAppointment(appointment);
                        success = true;
                        break;

                case AppointOptions.Replace:
                    if (appointment.Pat_Id != appointmentoption.RepIdNo)
                    {
                        //get medical record
                        var medicalRecord = _patientRecordServices.GetMedicalRecordByAppointment(appointment.No);

                        if (medicalRecord != null)
                        {

                            //remove medical record
                            _patientRecordServices.RemoveMedicalRecord(medicalRecord);
                        }
                        
                       
                        //update appointment
                         appointment.Pat_Id = appointmentoption.RepIdNo;
                        _appointmentServices.ModifyAppointment(appointment);

                        success = true;
                    }

                    break;

                case AppointOptions.Serve:

                    appointment.Status = true;
                    _appointmentServices.ModifyAppointment(appointment);

                    success = true;
                    break;


            }

            if (success == true)
            {
                appointHub = new AppointHub();

                //var id = _userPhysicianService.GetPhysicianUserId(phyId);

                _unitofwork.Commit();

                var appointSchedulebydoctor = await _appointmentServices.GetAllAppointmentList();

                appointHub.SendAppointment(phyId, appointSchedulebydoctor.Where(t => t.PhyId == phyId && t.AppointDate == appdate).ToList());


            }


            var url = Url.Action("GetAppointment_By_Doctor", "Appointment", new { id = phyId, appdate = appdate });

            return Json(new { success = success, url = url }, JsonRequestBehavior.AllowGet);
        }

     


        //[HttpGet]
        //public ActionResult GetAppointmentByPatient(string patientId)
        //{
        //    var appointmentList =  _appointmentServices.GetAllAppointmentByPatient(patientId);

        //    return Json(new {data=appointmentList }, JsonRequestBehavior.AllowGet);
        //}

    }
}

