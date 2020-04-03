using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
        public PatientMedicalRecordController(IPatientRecordServices patientrecordservices, IPatientServices patientservices, IUserPhysicianService userPhysicianService, IUnitOfWork unitOfWork)
        {
            _patientrecordservices = patientrecordservices;
            _patientservices = patientservices;
            _unitOfWork = unitOfWork;
            _userPhysicianService = userPhysicianService;

        }


        // GET: Doctor/PatientMedicalRecord
        public ActionResult Index()
        {
            ViewBag.loginPhyId = _userPhysicianService.GetPhysicianId(HttpContext.User.Identity.GetUserId());
            return View();
        }

        [HttpGet]
        public ActionResult MedicalHistory(string id,int phyid)
        {
         

            var patientRecordDetails = new PatientMedicalRecordDetailsViewModel
            {
                PatientId = id,
                PhyId = phyid,
                Patient = _patientservices.GetPatientById(id)
            };
         
            return View(patientRecordDetails);
        }

        [HttpGet]
        public ActionResult MedicalHistoryListByPatient(string id,int phyid,int? page)
        {
            int pageIndex = page ?? 1;
            int dataCount = 3;
            string patId = id.Trim();

            ViewBag.patId = id;
            ViewBag.phyId = phyid;


            var medicalRecord = _patientrecordservices.GetAllRecords(patId, phyid);

            var patientMedicalHistory = medicalRecord.OrderByDescending(t => t.RecordNo).ToList().ToPagedList(pageIndex, dataCount);

            return PartialView("_RecordListPartialView", (PagedList<MedicalRecord>) patientMedicalHistory);
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

    }
}