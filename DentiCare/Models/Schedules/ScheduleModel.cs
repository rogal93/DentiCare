using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DentiCare.Models.Schedules
{
    public class ScheduleModel
    {
        public int ID { get; set; }
        public bool DayOff { get; set; }
        public DateTime Date { get; set; }

        [Display(Name = "Od")]
        public int HourFrom { get; set; }

        [Display(Name = "Do")]
        public int HourTo { get; set; }

        public Dictionary<int, string> HoursFrom { get; set; }
        public Dictionary<int, string> HoursTo { get; set; }
    }
}