using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PMIS.ViewModels;
using Microsoft.Owin.Security;
using PMIS.Model;
using PMIS;

namespace PMIS.Areas.Doctor.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        // GET: Doctor/Account
        private ApplicationDbContext _context = new ApplicationDbContext();

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


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

        [HttpGet]
        public ActionResult UserProfile(string userId)
        {

            var userProfile = new UserProfileViewModel { UserId = userId};

            return View(userProfile);
        }

       [HttpGet]
        public ActionResult GetUserProfileInfo(string userId)
        {
            var userprofile = (_context.Users.Where(t => t.Id == userId)
                .Select(user => new UserProfileInfoViewModel()
                {
                    userId = user.Id,
                    userName = user.UserName,
                    lastName = user.Lastname,
                    firstName = user.Firstname,
                    userAbr = user.Abr

                    //userRole = string.Join(",", _userManager.GetRoles(userId))
                })).FirstOrDefault();



            return PartialView("_GetUserProfileInfo", userprofile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateUserProfileInfo(UserProfileInfoViewModel userprofileviewmodel)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_GetUserProfileInfo", userprofileviewmodel);
            }

         

            try
            {
                ApplicationUser user =await UserManager.FindByIdAsync(userprofileviewmodel.userId);
                user.UserName = userprofileviewmodel.userName;
                user.Firstname = userprofileviewmodel.firstName;
                user.Lastname = userprofileviewmodel.lastName;
                user.Abr = userprofileviewmodel.userAbr;

                var result = await UserManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return Json(new {success=false}, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ChangeUserPassword(string _userId)
        {
            var userSecurity = new UserSecurityViewModel { userId = _userId };
            return PartialView("_ChangeUserPassword", userSecurity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateUserPasswordAsync(UserSecurityViewModel userSecurityViewModel)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_ChangeUserPassword", userSecurityViewModel);
            }

            var userId = userSecurityViewModel.userId;
            
            if (await UserManager.CheckPasswordAsync(UserManager.FindById(userId), userSecurityViewModel.oldPass))
            {
                if (userSecurityViewModel.newPass == userSecurityViewModel.confirmnewPass)
                {
                    UserManager.RemovePassword(userId);
                    UserManager.AddPassword(userId, userSecurityViewModel.newPass);

                    
                    return Json(new { success = true}, JsonRequestBehavior.AllowGet);

                }
                else
                {

                    return Json(new { success = false, errmsg = "invalid password confirmation" },
                        JsonRequestBehavior.AllowGet);

                }
            }
            else
            {

                return Json(new { success = false, errmsg = "Current password invalid" },
                    JsonRequestBehavior.AllowGet);

            }


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