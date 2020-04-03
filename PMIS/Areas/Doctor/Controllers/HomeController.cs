using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PMIS.ServiceLayer;

namespace PMIS.Areas.Doctor.Controllers
{
    public class HomeController : Controller
    {
        //private readonly IAppointmentServices _appointmentServices;
        //public HomeController(IAppointmentServices appointmentServices)
        //{
        //    this._appointmentServices = appointmentServices;
        //}
        // GET: Doctor/Home


        public ActionResult Index()
        {

            return View();
        }

        //[HttpGet]
        //public ActionResult GetAppointmentByDoctor(int id,DateTime appointmentDate)
        //{
        //    var appointSchedulebydoctor = _appointmentServices.GetAllAppointment()
        //        .Where(t => t.PhyId == id && t.AppointDate.Date == appointmentDate.Date).ToList();

        //    return PartialView("_appointListPartialView", appointSchedulebydoctor);
        //}
    }
}