using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMIS.ViewModels
{
    public class UserProfileInfoViewModel
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string userAbr { get; set; }
        public string userRole { get; set; }
        public string profileImage { get; set; }
    }
}