using DentiCare.Models.Appointments;
using DentiCare.Models.UserSettings;
using System.Collections.Generic;

namespace DentiCare.Models.Patients
{
    public class AllPatientsViewModel
    {
        public List<UserNameWithIDs> Patients { get; set; }

        public DisplayDetailsViewModel Details { get; set; }
    }
}