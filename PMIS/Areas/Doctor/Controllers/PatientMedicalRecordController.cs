using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PMIS.Model;
using PMIS.ServiceLayer;
using PMIS.ViewModels;
using PagedList;

namespace PMIS.Areas.Doctor.Controllers
{
    [Authorize]
    public class PatientMedicalRecordController : Controller
    {
        private readonly IPatientRecordServices _patientrecordservices;
        private readonly IPatientServices _patientservices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserPhysicianService _userPhysicianService;
        private readonly IPrescriptionServices _prescriptionServices;
        public PatientMedicalRecordController(IPatientRecordServices patientrecordservices, IPatientServices patientservices, IUserPhysicianService userPhysicianService, IUnitOfWork unitOfWork, IPrescriptionServices prescriptionServices)
        {
            _patientrecordservices = patientrecordservices;
            _patientservices = patientservices;
            _unitOfWork = unitOfWork;
            _prescriptionServices = prescriptionServices;
            _userPhysicianService = userPhysicianService;

        }
        // GET: Doctor/PatientMedicalRecord
        public ActionResult Index()
        {
            ViewBag.loginPhyId = _userPhysicianService.GetPhysicianId(HttpContext.User.Identity.GetUserId());
            return View();
        }

        [HttpGet]
        public ActionResult MedicalHistory(string id,int phyid,int? page)
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
        public ActionResult MedicalPrescription(DocPrescriptionViewModel docPrescription)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_MedicalPrescription", docPrescription);
            }


            return Json(new {success=true}, JsonRequestBehavior.AllowGet);
        }


    }
}