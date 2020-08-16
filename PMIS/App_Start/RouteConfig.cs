using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PMIS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Account", action = "LogIn", id = UrlParameter.Optional },
               namespaces: new[] { "PMIS.Controllers" }
           );

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new[] { "PMIS.Controllers" }
            //);

            routes.MapRoute("PatientRecordById",
               "{controller}/{action}/{patientid}/{phyid}",
               new {controller= "PatientRecord", action= "MedicalHistory", patientid= "", phyid="" }
            );

            //routes.MapRoute("DoctorAppointRecord",
            //    "Doctor/{controller}/{action}/{phyid}",
            //    new { action = "Index",phyid = "" },
            //    new { controller = "DocAppointment" },
            //    new[] { "PMIS.Areas.Doctor.Controllers" });

            routes.MapRoute("DoctorMedicalRecord",
                "Doctor/{controller}/{action}/{id}/{phyid}",
                new {action = "MedicalHistory", id ="", phyid =""}, 
                new {controller = "PatientMedicalRecord"},
                new[] {"PMIS.Areas.Doctor.Controllers"});


        }
    }
}
