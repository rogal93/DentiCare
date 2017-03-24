using System.Collections.Generic;

namespace DentiCare.Models.Schedules
{
    public class CurrentScheduleViewModel
    {
        public List<ScheduleModel> Schedules { get; set; }

        public DayState DayState { get; set; }

        public HoursAtWork HoursAtWork { get; set; }
    }
}