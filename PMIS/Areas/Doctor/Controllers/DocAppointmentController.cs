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
        private readonly IUserPhysicianService _userPhysicianService;
        public DocAppointmentController(IAppointmentServices appointmentServices, IUserPhysicianService userPhysicianService)
        {
            _appointmentServices = appointmentServices;
            _userPhysicianService = userPhysicianService;

            //ViewBag.controller = "docappointment";
        }

        // GET: Doctor/DocAppointment
        public ActionResult Index()
        {
            ViewBag.phyId = _userPhysicianService.GetPhysicianId(User.Identity.GetUserId());

            return View();
        }

        [HttpGet]
        public ActionResult GetAppointmentByDoctor(int id, DateTime appointmentDate)
        {
            var appointSchedulebydoctor = _appointmentServices.GetAllAppointment()
                .Where(t => t.PhyId == id && t.AppointDate.Date == appointmentDate.Date).ToList();

            return PartialView("_appointListPartialView", appointSchedulebydoctor);
        }
    }
}