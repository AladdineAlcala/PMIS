using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Services.Configuration;
using Microsoft.AspNet.Identity;
using PMIS.Model;
using PMIS.ServiceLayer;
using PMIS.ViewModels;
using PagedList;
using PMIS.HelperClass;
using System.Collections.Generic;

namespace PMIS.Areas.Doctor.Controllers
{
   [UserPermissionAuthorized(UserPermisionLevelEnum.doctor)]
    public class PatientMedicalRecordController : Controller
    {
        private readonly IPatientRecordServices _patientrecordservices;
        private readonly IPatientServices _patientservices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPrescriptionServices _prescriptionServices;
        private readonly IPhyPrescriptionServices _phyPrescriptionServices;
        public PatientMedicalRecordController(IPatientRecordServices patientrecordservices, IPatientServices patientservices, IUnitOfWork unitOfWork, IPrescriptionServices prescriptionServices, IPhyPrescriptionServices phyPrescriptionServices)
        {
            _patientrecordservices = patientrecordservices;
            _patientservices = patientservices;
            _unitOfWork = unitOfWork;
            _prescriptionServices = prescriptionServices;
            _phyPrescriptionServices = phyPrescriptionServices;
        }
        // GET: Doctor/PatientMedicalRecord
        public ActionResult Index()
        {
            ViewBag.loginPhyId =HttpContext.User.Identity.GetUserId();
            return View();
        }

        [HttpGet]
        public ActionResult MedicalHistory(string id,string phyid,int? page)
        {
            int pageIndex = page ?? 1;
            int dataCount = 3;
           

            ViewBag.patId = id;
            ViewBag.phyId = phyid;
           


            var patientRecordDetails = new PatientMedicalRecordDetailsViewModel
            {
                PatientId = id,
                PhyId = phyid,
                Patient = _patientservices.GetPatientById(id)
             };
            var medicalRecord = _patientrecordservices.GetAllRecords(id, phyid);

            patientRecordDetails.MedicalRecordList = medicalRecord.OrderByDescending(t => t.RecordNo).ToList()
                .ToPagedList(pageIndex, dataCount);
          
            if (Request.IsAjaxRequest())
            {
                return PartialView("_RecordListPartialView",(PagedList<MedicalRecord>) patientRecordDetails.MedicalRecordList);
            }
         
            return View(patientRecordDetails);
        }



        [HttpGet]
        public ActionResult New_MedicalRecord(string id)
        {
            //var userid = System.Web.HttpContext.Current.User.Identity.GetUserId();


            var newrecord = new MedicalRecordViewModel()
            {
                PatientId = id,
                //PhyId =
            };


            return PartialView("_AddPatientRecordPartialView",newrecord);
        }


        [HttpGet]
        public ActionResult CreateMedication(int recNo)
        {

            var medication = _patientrecordservices.GetMedication(recNo);

            var medicationModel = new MedicationViewModel()
            {

                RecordNo = recNo,
                RecordedDate = DateTime.Now,
                Medication = medication != null ? medication.MedicationDesc : String.Empty
            };

            if (medication != null)
            {
                medicationModel.operation = 1;
                medicationModel.Id = (int)medication.MidNo;
            }
            else
            {
                medicationModel.operation = 0;
            }

            //medicationModel.operation = medication != null ? 1 : 0;

            return PartialView("_CreateMedication", medicationModel);
        }

        [HttpGet]
        public async Task<ActionResult> GetRecordChart(int recordNo)
        {

            var medicalRecord = await _patientrecordservices.GetMedicalRecord(recordNo);

            return PartialView("_GetRecord", medicalRecord);
        }

        [HttpGet]
        public ActionResult GetMedicationView(int recNo)
        {
            var patientMedication = _patientrecordservices.GetMedication(recNo);
            
            return PartialView("_GetMedicationView", patientMedication);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMedication(MedicationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreateMedication", viewModel);
            }


            Medication medication = new Medication
            {
                MidNo = viewModel.Id,
                RecordNo = viewModel.RecordNo,
                RecordedDate = DateTime.Now,
                MedicationDesc = viewModel.Medication
            };

            if (viewModel.operation == 0)
            {
                _patientrecordservices.AddMedication(medication);
            }
            else
            {
                _patientrecordservices.UpdateMedication(medication);
            }

            _unitOfWork.Commit();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ModifyRecord(int recNo)
        {

            return PartialView("_ModifyPatientRecord");
        }


        public async Task<ActionResult> RemoveMedicationAsync(int medNo)
        {

            var medication = await _patientrecordservices.GetMedicationBymedNo(medNo);

            if (medication == null) return Json(new { succces = false }, JsonRequestBehavior.AllowGet);


            _patientrecordservices.RemoveMedication(medication);

            _unitOfWork.Commit();

            return Json(new {success = true}, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult MedicalPrescription(int recNo)
        {
            var docPrescription=new DocPrescriptionViewModel
            {   

                RecNo = recNo,
                PrescriptionCatListItem = _prescriptionServices.GetCategoryListItems(),
                RecipeListItem = _prescriptionServices.GetPrescriptionListItems(null)
            };


            return PartialView("_MedicalPrescription", docPrescription);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("AddNewDoctorsPrescription")]
        public ActionResult MedicalPrescription(DocPrescriptionViewModel docPrescriptionViewModel)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_MedicalPrescription", docPrescriptionViewModel);
            }

            var docprescription=new DocPrescriptionRecord()
            {
                Date =DateTime.Now,
                RecordNo = docPrescriptionViewModel.RecNo,
                PresId = docPrescriptionViewModel.PrescId,
                Sig = docPrescriptionViewModel.Sig,
                Disp = docPrescriptionViewModel.DispInst
            };

            _phyPrescriptionServices.InsertDocPrescription(docprescription);
            _unitOfWork.Commit();

            return Json(new {success=true}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ModifyMedicalPrescription(int prescNo)
        {
            var docprescription =_phyPrescriptionServices.FindDocPrescriptionById(prescNo);

            DocPrescriptionViewModel docPrescriptionViewModel=null;
            if (docprescription != null)
            {
                docPrescriptionViewModel = new DocPrescriptionViewModel()
                {
                    PrescNo = prescNo,
                    RecNo = (int) docprescription.RecordNo,
                    CatId = (int) docprescription.Prescription.CatId,
                    PrescId = (int) docprescription.PresId,
                    Sig = docprescription.Sig,
                    DispInst = docprescription.Disp,
                    PrescriptionCatListItem= _prescriptionServices.GetCategoryListItems(),
                    RecipeListItem = _prescriptionServices.GetPrescriptionListItems((int)docprescription.Prescription.CatId)
                };

             
            }

            return PartialView("_ModifyMedicalPrescription", docPrescriptionViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateMedicalPrescription(DocPrescriptionViewModel docPrescriptionViewModel)
        {
            if (!ModelState.IsValid) return PartialView("_ModifyMedicalPrescription", docPrescriptionViewModel);

            var docprescription = new DocPrescriptionRecord()
            {
                Date = DateTime.Now,
                No = docPrescriptionViewModel.PrescNo,
                RecordNo = docPrescriptionViewModel.RecNo,
                PresId = docPrescriptionViewModel.PrescId,
                Sig = docPrescriptionViewModel.Sig,
                Disp = docPrescriptionViewModel.DispInst
            };

            _phyPrescriptionServices.UpdateDocPrescription(docprescription);
            _unitOfWork.Commit();

            return Json(new {success=true}, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ActionName("RemoveDoctorsPrescription")]
        public async Task<ActionResult> RemoveMedicalPrescription(int prescNo)
        {
            var docprescription =await _phyPrescriptionServices.FindDocPrescriptionByIdAsync(prescNo);


            if (docprescription == null) return Json(new {succces = false}, JsonRequestBehavior.AllowGet);

            _phyPrescriptionServices.RemoveDocPrescription(docprescription);
            _unitOfWork.Commit();

            return Json(new { succces = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetMedPrescription(int recordNo)
        {
            var docprescriptionList = _prescriptionServices.GetDocPrescriptionByRecNo(recordNo);

            return PartialView("_GetMedPrescription",docprescriptionList.ToList());
        }

        [HttpGet]
        public ActionResult ModifyMedication(int recNo)
        {
            var medication = _patientrecordservices.GetMedication(recNo);

            var medicationModel = new MedicationViewModel()
            {

                RecordNo = recNo,
                RecordedDate = DateTime.Now,
                Medication = medication != null ? medication.MedicationDesc : String.Empty
            };


            if (medication != null)
            {
                medicationModel.operation = 1;
                medicationModel.Id = (int)medication.MidNo;
            }
            else
            {
                medicationModel.operation = 0;
            }

            return PartialView("_ModifyMedication", medicationModel);
        }


        public ActionResult UpdateMedication(MedicationViewModel viewModel)
        {

            if (!ModelState.IsValid)
            {
                return PartialView("_CreateMedication", viewModel);
            }


            Medication medication = new Medication
            {
                MidNo = viewModel.Id,
                RecordNo = viewModel.RecordNo,
                RecordedDate = DateTime.Now,
                MedicationDesc = viewModel.Medication
            };

            _patientrecordservices.UpdateMedication(medication);

            _unitOfWork.Commit();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPrescriptionCategorySelect(int id)
        {
            List<SelectListItem> prescriptionlist = new List<SelectListItem>();

            prescriptionlist = _prescriptionServices.GetPrescriptionListItems(id).ToList();

            return Json(prescriptionlist,JsonRequestBehavior.AllowGet);
        }
    }
}