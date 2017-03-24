using System.Collections.Generic;

namespace DentiCare.Models.Appointments
{
    public class CancelAppointmentViewModel
    {
        public List<UserNameWithIDs> Patients { get; set; }

        public List<AppointmentModel> PatientAppointments { get; set; }
    }
}