using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PMIS.Model;
using PMIS.ServiceLayer;
using PMIS.ViewModels;

namespace PMIS.Controllers
{
    public class PrescriptionController : Controller
    {
        private readonly IPrescriptionServices _prescriptionServices;
        private readonly IUnitOfWork _unitOfWork;
        public PrescriptionController(IPrescriptionServices prescriptionServices, IUnitOfWork unitOfWork)
        {
            _prescriptionServices = prescriptionServices;
            _unitOfWork = unitOfWork;
        }
        // GET: Prescription
        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetAllPrescriptionsAsync()
        {
            var prescriptionlist = await _prescriptionServices.GetAllPrescriptions();
            return Json(new {data=prescriptionlist},JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CreatePrescription()
        {
            var prescription=new PrescriptionViewModel
            {
                PresCategorySelectList = _prescriptionServices.GetCategoryListItems()
            };

            return PartialView("_CreatePrescription",prescription);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePrescription(PrescriptionViewModel prescriptionviewmodel)
        {
            if (!ModelState.IsValid)
            {
              return PartialView("_CreatePrescription", prescriptionviewmodel);
            }

            var prescription=new Prescription()
            {
                CatId = prescriptionviewmodel.PrescriptionCatid,
                PrescriptionDetails = prescriptionviewmodel.PrescriptionDetails,
                Unit = prescriptionviewmodel.PrescUnit
            };
            _prescriptionServices.InsertPrescription(prescription);
            _unitOfWork.Commit();

            return Json(new {success=true},JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> RemovePrescription(int id)
        {
            var prescription =await _prescriptionServices.GetPrescriptionByIdAsync(id);
            if (prescription != null)
            {
                _prescriptionServices.RemovePrescription(prescription);
                _unitOfWork.Commit();

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new {success = false}, JsonRequestBehavior.AllowGet);
        }
    }
} 