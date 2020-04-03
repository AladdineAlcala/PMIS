using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PMIS.ViewModels;
using Microsoft.Owin.Security;

namespace PMIS.Areas.Doctor.Controllers
{
    public class AccountController : Controller
    {
        // GET: Doctor/Account


        [ChildActionOnly]
        public ActionResult UserLogIn()
        {
            var userId = User.Identity.GetUserId();

            var users = new UsersViewModel();
            var userlog = (from v in users.listofUsers().Where(x => x.userId == userId)
                select new UsersViewModel()
                {
                    userId = v.userId,
                    username = v.username,
                    roles = v.roles
                }).FirstOrDefault();

            return PartialView("_LoginDocsPartialView", userlog);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();

            return RedirectToAction("LogIn", "Account",new {area=""});
        }


        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}