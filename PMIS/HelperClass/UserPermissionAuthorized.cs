using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PMIS.HelperClass
{
    public class UserPermissionAuthorized:AuthorizeAttribute
    {
        private readonly UserPermisionLevelEnum[] _allowedPermisionLevelEnums;

        private IList<UserPermisionLevelEnum> _approvedPermissionLevelList =new List<UserPermisionLevelEnum>();


        public UserPermissionAuthorized(params UserPermisionLevelEnum[] allowedPermisionLevelEnums)
        {
            _allowedPermisionLevelEnums = allowedPermisionLevelEnums;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            bool isAuthorized = false;

             _approvedPermissionLevelList = GetLoggedUserPermission();

            foreach (UserPermisionLevelEnum permisionLevel in _allowedPermisionLevelEnums)
            {
                if (_approvedPermissionLevelList.Any(t => t == permisionLevel) == true)
                {
                    isAuthorized = true;
                }
            }


            return isAuthorized;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
           

            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    var urlHepler=new UrlHelper(filterContext.RequestContext);
                    filterContext.HttpContext.Response.StatusCode = 403;

                    filterContext.Result = new JsonResult()
                    {

                        Data = new
                        {
                            Error = "You Are Not Authorized to Access this Operation",
                            LogOnUrl = _approvedPermissionLevelList.Any(t=>t== UserPermisionLevelEnum.doctor)==true? urlHepler.Action("UnauthorizedAccess", "Account",new {area="Doctor"}): urlHepler.Action("UnauthorizedAccess", "Account")

                            //LogOnUrl= urlHepler.Action("UnauthorizedAccess", "Shared")
                        },JsonRequestBehavior =JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "UnauthorizedAccess" }));
                }
            }

            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

        private IList<UserPermisionLevelEnum> GetLoggedUserPermission()
        {

            var roles = ((ClaimsIdentity) HttpContext.Current.User.Identity).Claims
                .Where(t => t.Type == ClaimTypes.Role).Select(t => t.Value).ToList();


            IList<UserPermisionLevelEnum> loggedUsersRoleEnums = roles.Select(role => Enum.Parse(typeof(UserPermisionLevelEnum), role))
                .Cast<UserPermisionLevelEnum>().ToList();

            return loggedUsersRoleEnums;
        }
    }



    public enum UserPermisionLevelEnum
    {
        admin,
        user,
        doctor
    }
}