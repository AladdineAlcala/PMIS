using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PMIS.Model;
using PMIS.ServiceLayer;
using PMIS.ViewModels;

namespace PMIS.Controllers
{
    [Authorize]
    public class PatientRecordController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IPatientRecordServices _patientrecordservices;
        private readonly IPatientServices _patientservices;

       

        // GET: PatientRecord
        public PatientRecordController(IUnitOfWork unitofwork, IPatientRecordServices patientrecordservices, IPatientServices patientservices)
        {
            _unitofwork = unitofwork;
            _patientrecordservices = patientrecordservices;
            _patientservices = patientservices;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddRecord(string patientId,string physicianId)
        {
            var newrecord = new MedicalRecordViewModel()
            {
                PatientId = patientId,
                PhyId = physicianId
            };

            return PartialView("_AddPatientRecordPartialView",newrecord);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRecordAsync(MedicalRecordViewModel medicalrecord)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_AddPatientRecordPartialView", medicalrecord);
            }


            var newmedicalrecord = new MedicalRecord()
            {
                Pat_Id = medicalrecord.PatientId.Trim(),
                Phys_id = medicalrecord.PhyId,
                RecordDate = DateTime.Now,
                ActivityName = medicalrecord.Subject,
                RecordDetails = medicalrecord.Desciption,
                AppointmentNo = medicalrecord.ApppointmentNo
            };

             _patientrecordservices.AddMedicalRecord(newmedicalrecord);
             _unitofwork.Commit();

            return Json(new {success=true},JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public async Task<ActionResult> RemovePatientRecord(int recordId)
        {

           var patientMedicalRecord= await _patientrecordservices.GetMedicalRecord(recordId);

            if (patientMedicalRecord != null)
            {
                _patientrecordservices.RemoveMedicalRecord(patientMedicalRecord);
                _unitofwork.Commit();
                return Json(new { success=true}, JsonRequestBehavior.AllowGet);
            }


            return Json(new {success=false}, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult MedicalHistory(string patientid, string phyid)
        {

            var patient_record_details = new PatientMedicalRecordDetailsViewModel
            {
                PatientId = patientid,
                PhyId = phyid,
                Patient = _patientservices.GetPatientById(patientid)
            };

            return View(patient_record_details);
        }


        [HttpGet]
        public ActionResult MedicalHistoryListByPatient(string id, string phyid, int? page)
        {
            int pageIndex = page ?? 1;
            int dataCount = 3;
            string patId = id.Trim();

            ViewBag.patId = id;
            ViewBag.phyId = phyid;


            var medicalRecord = _patientrecordservices.GetAllRecords(patId, phyid);

            var patientMedicalHistory = medicalRecord.OrderByDescending(t => t.RecordNo).ToList().ToPagedList(pageIndex, dataCount);

            return PartialView("_RecordListPartialView", (PagedList<MedicalRecord>)patientMedicalHistory);
        }


        [HttpGet]
        public async Task<ActionResult> Load_MedicalRecordPartialView(string patientid, string phyid,int? page)
        {
            int pageIndex = page ?? 1;
            int dataCount = 3;

            var medicalRecord = await _patientrecordservices.GetAllRecordsAsync();
            var patientmedicalhistory = medicalRecord.Where(t => t.Pat_Id == patientid.Trim() && t.Phys_id == phyid)
                .OrderByDescending(t => t.RecordNo).ToPagedList(pageIndex, dataCount);

            return PartialView("_RecordListPartialView",(PagedList<MedicalRecord>)patientmedicalhistory);
        }


        [HttpGet]
        public ActionResult GetAllPhysicianforPatient(string patientId)
        {
            var physicianIncharge = _patientrecordservices.Get_Distinct_PhysicianByPatient().Where(t=>t.PatientId.Trim()==patientId);
            return PartialView("_GetAllPhysicianforPatientPartialView",physicianIncharge.ToList());
        }


        [HttpGet]
        public async Task<ActionResult> GetRecordChart(int recordNo)
        {

            var medicalRecord = await _patientrecordservices.GetMedicalRecord(recordNo);

            return PartialView("_GetRecord", medicalRecord);
        }
        
        [HttpGet]
        public ActionResult CreateMedication(int recNo)
        {

            var medication = _patientrecordservices.GetMedication(recNo);

            var medicationModel = new MedicationViewModel()
            {
               
                RecordNo = recNo,
                RecordedDate = DateTime.Now,
                Medication =medication!=null?medication.MedicationDesc:String.Empty
            };

            if (medication != null)
            {
                medicationModel.operation = 1;
                medicationModel.Id = (int) medication.MidNo;
            }
            else
            {
                medicationModel.operation = 0;
            }
            //medicationModel.operation = medication != null ? 1 : 0;

            return PartialView("_CreateMedication", medicationModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMedication(MedicationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreateMedication", viewModel);
            }


            Medication medication=new Medication
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

            _unitofwork.Commit();

            return Json(new {success=true}, JsonRequestBehavior.AllowGet);
        }
    }
}