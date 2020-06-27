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
        private readonly IPatientRecordServices _patientRecordServices;
        private readonly IPatientServices _patientServices;
        private readonly IUnitOfWork _unitOfWork;
        public PrescriptionController(IPrescriptionServices prescriptionServices, IUnitOfWork unitOfWork, IPatientRecordServices patientRecordServices, IPatientServices patientServices)
        {
            _prescriptionServices = prescriptionServices;
            _unitOfWork = unitOfWork;
            _patientRecordServices = patientRecordServices;
            _patientServices = patientServices;
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

        [HttpGet]
        public async Task<ActionResult> PrintPrescript(int medRecNo)
        {
            //var reportOption = new ReportOptionViewModel()
            //{
            //   recordNo = medRecNo
            //};

            var medicalRecordList = await _patientRecordServices.GetAllRecordsAsync();
            ContainerClass.MedicalRecord = (from m in medicalRecordList where m.RecordNo==medRecNo
                                            select new PrintMedicalRecordViewModel()
                                            {
                                                RecordNo = (int)m.RecordNo,
                                                PatientId = m.Pat_Id,
                                                PhyId = m.Phys_id,
                                                RecordedDate = (DateTime) m.RecordDate
                                               

                                            }).ToList();

           var docprescription = _prescriptionServices.GetDocPrescriptionByRecNo(medRecNo).ToList();
            ContainerClass.DocPrescription = (from p in docprescription
                                               select new PrintDocPrescriptionViewModel()
                                                {
                                                    PrescNo = p.PrescNo,
                                                    RecNo = p.RecNo,
                                                    PrescriptionDetails = p.PrescriptionDetails,
                                                    Sig = p.Sig,
                                                    DispInst = p.DispInst

                                                }).ToList();

           var patientList = await _patientServices.GetAllPatients();
            ContainerClass.PatientDetails = patientList.Where(t => t.PatientId == ContainerClass.MedicalRecord[0].PatientId).ToList();

            return View("~/Views/Shared/ReportContainer.cshtml");
        }
    }
} 