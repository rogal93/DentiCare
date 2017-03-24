using System.Collections.Generic;

namespace DentiCare.Models.Appointments
{
    public class UndescribedViewModel
    {
        public List<AppointmentModel> Undescribed { get; set; }

        public UndescribedDetails Details { get; set; }
    }
}