using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMIS.ViewModels
{
    public class UsersConnectionViewModel
    {
        public string UserId { get; set; }
        public HashSet<string> ConnectionId { get; set; }

    }
}