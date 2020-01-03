using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMIS.ViewModels
{
    public class ScheduleViewModel
    {
        public int s_id { get; set; }
        public DateTime DateofSched { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
        public string TimeOption { get; set; }
        public string Status { get; set; }
        public int PhysId { get; set; }
    }
}