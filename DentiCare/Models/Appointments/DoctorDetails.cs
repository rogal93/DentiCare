using System;
using System.ComponentModel.DataAnnotations;

namespace DentiCare.Models.Appointments
{
    public class DoctorDetails
    {
        public int DoctorID { get; set; }

        public string Photo { get; set; }

        [Display(Name = "Ukończony uniwersytet")]
        public string University { get; set; }

        [Display(Name = "Data ukończenia")]
        public DateTime GraduationDate { get; set; }

        [Display(Name = "Specjalizacja")]
        public string Specialization { get; set; }

        [Display(Name = "O mnie")]
        public string AboutMyself { get; set; }
    }
}