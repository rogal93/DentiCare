using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DentiCare.Models.Appointments
{
    public class TodayVisitsViewModel
    {
        [Display(Name = "lek. stom.")]
        public Dictionary<int, string> DoctorsIDsNames { get; set; }

        public List<AppointmentModel> Appointments { get; set; }
    }
}