using System;
using System.ComponentModel.DataAnnotations;

namespace DentiCare.Models.Appointments
{
    public class AppointmentModel
    {
        public int ID { get; set; }

        [Display(Name = "Stomatolog")]
        public string DoctorFullName { get; set; }

        [Display(Name = "Pacjent")]
        public string PatientFullName { get; set; }

        [Display(Name = "Data wizyty")]
        public DateTime Date { get; set; }

        [Display(Name = "Komentarz")]
        public string Comment { get; set; }

        [Display(Name = "Koszt")]
        public double Price { get; set; }
    }
}