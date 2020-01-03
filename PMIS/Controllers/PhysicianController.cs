using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PMIS.Model;
using PMIS.ServiceLayer;
using PMIS.ViewModels;

namespace PMIS.Controllers
{
    public class PhysicianController : Controller
    {
        private readonly PMISEntities _pmisEntities;
        private readonly PhycisianServices _phycisianservices;
        public PhysicianController()
        {
            _pmisEntities=new PMISEntities();
            _phycisianservices = new PhycisianServices();
        }
        // GET: Physician
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllPhysician()
        {
            return Json(new {data = _phycisianservices.GetAllPhysician()},JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CreatePhysician()
        {
            return PartialView("_CreatePhysician");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePhysician(PhysicianDetailsViewModel physicianDetails)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreatePhysician");
            }
            Physician physician = new Physician()
            {
                Phys_Fullname = physicianDetails.PhysName,
                Phys_Abr = physicianDetails.PhysAbr

            };

            _pmisEntities.Physicians.Add(physician);
            _pmisEntities.SaveChanges();

            return Json(new {success=true},JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult RemovePhysician(int id)
        {
            var physician = _pmisEntities.Physicians.Find(id);


            _pmisEntities.Physicians.Remove(physician);
            _pmisEntities.SaveChanges();

            return Json(new {success=true}, JsonRequestBehavior.AllowGet);
        }

      
        protected override void Dispose(bool disposing)
        {
            _pmisEntities.Dispose();
        }
    }
}