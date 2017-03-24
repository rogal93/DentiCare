using System;

namespace DentiCare.Models.Schedules
{
    public class DoctorInfo
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Specialization { get; set; }
        public DateTime GraduationDate { get; set; }
        public string University { get; set; }
        public string AboutMyself { get; set; }
        public string Photo { get; set; }
    }
}