using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PMIS.Model;
using PMIS.ViewModels;
using Microsoft.Owin.Security;
using PMIS.ServiceLayer;

namespace PMIS.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private readonly IUnitOfWork _unitofwork;
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        // GET: Account
        public AccountController(IUnitOfWork unitofwork)
        {;
            _unitofwork = unitofwork;
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;

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

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }




        [HttpGet]
        [AllowAnonymous]
        public ActionResult LogIn(string returnUrl)
        {
            ViewBag.ReturnUrl = "";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogIn(LoginViewModel model,string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, shouldLockout: true);

            switch (result)
            {
                case SignInStatus.Success:

                    //FormsAuthentication.SetAuthCookie(model.Username, true);

                    return RedirectToAction("LoginRoute", new {returnUrl=returnUrl});

                case SignInStatus.LockedOut:
                 

                case SignInStatus.RequiresVerification:
                 

                case SignInStatus.Failure:

                default:

                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }

        }

        [ChildActionOnly]
        public ActionResult UserLogIn()
        {
            var userId = User.Identity.GetUserId();

            var users=new UsersViewModel();
            var userlog = (from v in users.listofUsers().Where(x => x.userId == userId)
                select new UsersViewModel()
                {
                    userId = v.userId,
                    username = v.username
                }).FirstOrDefault();

            return PartialView("_LogUserPartialView", userlog);
        }


        public ActionResult LoginRoute(string returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl))
            {

                if (User.IsInRole("doctor"))
                {

                    //ViewBag.phyId = _userPhysicianService.GetPhysicianId(User.Identity.GetUserId());

                    var url = Url.Action("Index", "DocAppointment", new {area = "Doctor"});

                    return RedirectToLocal(url);
                }

                else
                {
                    var url = Url.Action("Index", "Home");

                    return RedirectToLocal(url);
                }
            }
            else
            {
                if (User.IsInRole("doctor"))
                {
                    var url = Url.Action("Index", "DocAppointment", new {area = "Doctor"});

                    return RedirectToLocal(url);
                }
                else
                {
                    return RedirectToLocal(returnUrl);
                }
            }
           
           

        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("LogIn", "Account");
        }



        #region Users

        public ActionResult UsersIndex()
        {

            return View();
        }


        [HttpGet]
        public ActionResult RegisterUser()
        {
            return PartialView("_NewUser");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterUser(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user=new ApplicationUser {UserName =model.UserName,Email = model.Email};
                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return Json(new {success=true}, JsonRequestBehavior.AllowGet);
                }
            }

            return PartialView("_NewUser", model);
        }

        public ActionResult GetAllUsers()
        {
            UsersViewModel users=new UsersViewModel();
            var listofusers=new List<UsersViewModel>();

            try
            {
                listofusers = users.listofUsers().ToList();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Json(new {data = listofusers}, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult AddRoleUser(string id)
        {
            UserinRoleViewModel user = new UserinRoleViewModel();


            var userRole = UserManager.GetRoles(id);
            var userinrole = user.GetUsersinRole().FirstOrDefault(u => u.userId == id);
            string roleName = userRole.Count() == 0 ? "user" : userRole[0];
            var roleList = RoleManager.Roles.ToList();

            userinrole.selectListRoles = roleList.Select(s => new SelectListItem()
            {
                Value = s.Id.ToString(),
                Text = s.Name,
                Selected = s.Name == roleName ? true : false
            });

            //userinrole.selectListPhysicians = _appointmentServices.GetAllDoctors();

            return PartialView("_AddRoleUser", userinrole);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddRoleUser(UserinRoleViewModel model)
        {
            bool success = false;
            if (!ModelState.IsValid)
            {
                return PartialView("_AddRoleUser", model);
            }

            if (model.selecteduserRole != null)
            {

               
                var currentUser = UserManager.FindById(model.userId);
                var rolename = RoleManager.Roles.First(r => r.Id == model.selecteduserRole).Name;
               

                //if (rolename == "doctor")
                //{
                //    User_Physician userPhysician=new User_Physician
                //    {
                //        Id=model.userId,
                //        Phys_id = model.selectedPhyId
                //    };


                //    _userPhysicianService.InsertPhysicianUser(userPhysician);
                   
                //}

                var roleresult = await UserManager.AddToRoleAsync(currentUser.Id, rolename);

                if (roleresult.Succeeded)
                {
                    _unitofwork.Commit();
                    success = true;
                }

            }


            return Json(new {success= success }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public async Task<ActionResult> RemoveUser(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            var logins = user.Logins;
            var rolesforUser = await UserManager.GetRolesAsync(id);

            using (var transaction = _context.Database.BeginTransaction())
            {
                //IdentityResult result = IdentityResult.Success;

                foreach (var login in logins)
                {
                    await UserManager.RemoveLoginAsync(login.UserId,
                        new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                }

                if (rolesforUser.Any())
                {
                    foreach (var item in rolesforUser.ToList())
                    {
                        var result = await UserManager.RemoveFromRoleAsync(user.Id, item);
                    }
                }

                await UserManager.DeleteAsync(user);
                transaction.Commit();
            }


            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult UserProfile(string userId)
        {
            var userProfile = new UserProfileViewModel { UserId = userId };

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
                ApplicationUser user = await UserManager.FindByIdAsync(userprofileviewmodel.userId);
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
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
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


                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);

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

        public ActionResult GetInfoLogInUser(string userId)
        {
            ApplicationUser user = UserManager.FindById(userId);
            ApplicationUserViewModel appuser = null;
            if (user != null)
            {
                appuser = new ApplicationUserViewModel()
                {
                    UserId = userId,
                    Lastname = user.Lastname,
                    Firstname = user.Firstname,
                    Abr = user.Abr
                };


            }

            return PartialView("_GetInfoLogInUser",appuser);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }

                if (_roleManager != null)
                {
                    _roleManager.Dispose();
                    _roleManager = null;
                }

            }

            base.Dispose(disposing);
        }


        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";




        public ActionResult UnauthorizedAccess()
        {
            return View("UnauthorizedAccess");
        }


        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

   

    }
}