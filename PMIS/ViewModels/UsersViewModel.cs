using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using PMIS.Model;

namespace PMIS.ViewModels
{
    public class UsersViewModel
    {
        public string userId { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string roles { get; set; }
        public bool has_superadminRights { get; set; }

        public IEnumerable<UsersViewModel> listofUsers()
        {
            //List<UsersViewModel> listofuser=new List<UsersViewModel>();

            var context = new ApplicationDbContext();


            var listofuser = (from user in context.Users
                select new
                {
                    userId = user.Id,
                    username = user.UserName,
                    email = user.Email,
                    Rolenames = (from userRole in user.Roles
                        join role in context.Roles on userRole.RoleId
                        equals role.Id
                        select role.Name).ToList()
                }).ToList().Select(p => new UsersViewModel()
            {
                userId = p.userId,
                username = p.username,
                email = p.email,
                roles = string.Join(",", p.Rolenames),
                has_superadminRights = p.Rolenames.Contains("superadmin")
            }).ToList();



            return listofuser;
        }




    }

    public class UserinRoleViewModel
    {
        public string userId { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public List<IdentityRole> userRole { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> selectListRoles { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> selectListPhysicians { get; set; }
        [Required(ErrorMessage = "Select a role")]
        public string selecteduserRole { get; set; }
        public int selectedPhyId { get; set; }

        public IEnumerable<UserinRoleViewModel> GetUsersinRole()
        {

            var context = new ApplicationDbContext();

            var listofuser = (from user in context.Users
                select new
                {
                    userId = user.Id,
                    username = user.UserName,
                    email = user.Email,
                    Rolenames = (from userRole in user.Roles
                        join role in context.Roles on userRole.RoleId
                        equals role.Id
                        select role).ToList()
                }).ToList().Select(p => new UserinRoleViewModel()
            {
                userId = p.userId,
                username = p.username,
                email = p.email,
                userRole = p.Rolenames

            }).ToList();


            return listofuser.ToList();
        }


      
    }
}