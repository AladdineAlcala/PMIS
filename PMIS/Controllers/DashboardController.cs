using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PMIS.Hubs;
using PMIS.Model;
using PMIS.ServiceLayer;
using PMIS.ViewModels;

namespace PMIS.Controllers
{
    public class DashboardController : Controller
    {

        private readonly IAppointmentServices _appointmentServices;
        private readonly IPatientServices _patientServices;
        private readonly IPrescriptionServices _prescriptionServices;
        private AppointHub appointHub;
        private readonly ApplicationDbContext _context = new ApplicationDbContext();


        public DashboardController(IAppointmentServices appointmentServices, IPatientServices patientServices, IPrescriptionServices prescriptionServices)
        {
            _appointmentServices = appointmentServices;
            _patientServices = patientServices;
            _prescriptionServices = prescriptionServices;
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetAppointments()
        {
            var dashboard = new DashBoardViewModel();
            var appointments = await _appointmentServices.GetAllAppointmentList();

            int count = 0;
            foreach (var t in appointments)
            {
                if (t.AppointDate.Date == DateTime.Today.Date) count++;
            }
            int appointmentCount = count>0? count:0;

            dashboard.appointmentCount = appointmentCount;



            return PartialView("_GetAppointments", dashboard);
        }

        [HttpGet]
        public ActionResult GetLoggedPhysician()
        {
            appointHub = new AppointHub();

            var loggedUsers= appointHub.GetAllLoggedUsers();

            //var userlist = loggedUsers.Select(t => t.UserId).ToList();

            var dashboard = new DashBoardViewModel {loggedPhysician = loggedUsers.Count};

            return PartialView("_GetLoggedPhysician", dashboard);
        }





        [HttpGet]
        public async Task<ActionResult> GetRegisteredPatients()
        {
            int patientCount = await _patientServices.CountAllPatients();

            var dashboard = new DashBoardViewModel { registeredPatients = patientCount> 0?patientCount:0};
            return PartialView("_GetRegisteredPatients",dashboard);
        }


        public async Task<ActionResult> GetMedicines()
        {
            int prescription = await _prescriptionServices.PrescriptionCount();
            var dashboard = new DashBoardViewModel { medicineCount = prescription > 0 ? prescription : 0 };
            return PartialView("_GetMedicines", dashboard);
        }
    }
}
