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
    [Authorize]
    public class PhysicianController : Controller
    {
        
        private readonly IPhycisianServices _phycisianservices;
        private readonly IUnitOfWork _unitOfWork;
        public PhysicianController(IPhycisianServices physervices,IUnitOfWork iunitofwork)
        {
            this._phycisianservices = physervices;
            this._unitOfWork = iunitofwork;
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

            _phycisianservices.AddPhysician(physician);
            _unitOfWork.Commit();

            return Json(new {success=true},JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult RemovePhysician(int id)
        {
            bool success = false;

            var physician = _phycisianservices.GetPhysician_By_Id(id);
            if (physician != null)
            {
                _phycisianservices.RemovePhysician(physician);
                _unitOfWork.Commit();
                success = true;
            };

            return Json(new {success=success}, JsonRequestBehavior.AllowGet);
        }

      
       
    }
}