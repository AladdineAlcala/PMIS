using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PMIS.ServiceLayer;

namespace PMIS.Areas.Doctor.Controllers
{
    [Authorize]
    public class DocAppointmentController : Controller
    {
        private readonly IAppointmentServices _appointmentServices;
     
        private readonly IUnitOfWork _unitOfWork;

        public DocAppointmentController(IAppointmentServices appointmentServices, IUnitOfWork unitOfWork)
        {
            _appointmentServices = appointmentServices;
            _unitOfWork = unitOfWork;

            //ViewBag.controller = "docappointment";
        }

        // GET: Doctor/DocAppointment
        public ActionResult Index()
        {
            //ViewBag.phyId = _userPhysicianService.GetPhysicianId(User.Identity.GetUserId());

            ViewBag.phyId = User.Identity.GetUserId();
            return View();
        }

        [HttpGet]
        public ActionResult GetAppointmentByDoctor(string id, DateTime appointmentDate)
        {
            var appointSchedulebydoctor = _appointmentServices.GetAllAppointment()
                .Where(t => t.PhyId == id && t.AppointDate.Date == appointmentDate.Date).ToList();

            return PartialView("_appointListPartialView", appointSchedulebydoctor);
        }

        [HttpPost]
        public ActionResult ServeAppointment(int apptId)
        {
            var appointment = _appointmentServices.GetAppointment(apptId);

            if (appointment!=null)
            {
                appointment.Status = true;
                _appointmentServices.ModifyAppointment(appointment);

                _unitOfWork.Commit();


                var _url = Url.Action("GetAppointmentByDoctor", "DocAppointment", new { area="Doctor", id = appointment.Phys_id, appointmentDate = appointment.AppointDate });

                return Json(new { success = true, url=_url}, JsonRequestBehavior.AllowGet);


            }

            return Json(new { success =false}, JsonRequestBehavior.AllowGet);
        }
    }
}