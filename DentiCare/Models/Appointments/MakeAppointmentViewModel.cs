using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DentiCare.Models.Appointments
{
    public class MakeAppointmentViewModel
    {
        [Display(Name = "lek. stom.")]
        public Dictionary<int, string> DoctorsIDsNames { get; set; }

        public DoctorDetails DoctorDetails { get; set; }

        public List<FreeTerm> FreeTerms { get; set; }
    }
}