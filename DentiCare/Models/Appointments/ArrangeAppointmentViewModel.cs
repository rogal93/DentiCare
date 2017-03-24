using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DentiCare.Models.Appointments
{
    public class ArrangeAppointmentViewModel
    {
        [Display(Name = "lek. stom. ")]
        public Dictionary<int, string> NamesOfDoctors { get; set; }

        public List<UserNameWithIDs> Patients { get; set; }

        public List<FreeTerm> FreeTerms { get; set; }
    }
}