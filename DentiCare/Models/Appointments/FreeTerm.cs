using System;

namespace DentiCare.Models.Appointments
{
    public class FreeTerm
    {
        public int ScheduleID { get; set; }
        public DateTime Date { get; set; }
        public DayOfWeek DayOfWeek { get; set; }

        public int Hour { get; set; }
    }
}