using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DentiCare.Models.Schedules
{
    public class DoctorScheduleViewModel
    {
        [Display(Name = "lek. stom. ")]
        public Dictionary<int, string> NamesOfDoctors { get; set; }
        public List<ScheduleModel> Schedules { get; set; }
    }
}