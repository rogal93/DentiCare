using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DentiCare.Models.Schedules
{
    public class HoursAtWork
    {
        public int ScheduleID { get; set; }

        [Display(Name = "Od")]
        public int HourFrom { get; set; }

        [Display(Name = "Do")]
        public int HourTo { get; set; }

        public Dictionary<int, string> HoursFrom { get; set; }
        public Dictionary<int, string> HoursTo { get; set; }
    }
}