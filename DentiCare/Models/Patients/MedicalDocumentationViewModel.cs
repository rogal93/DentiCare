using DentiCare.Models.Appointments;
using System.Collections.Generic;

namespace DentiCare.Models.Patients
{
    public class MedicalDocumentationViewModel
    {
        public List<UserNameWithIDs> Patients { get; set; }

        public PatientDocumentsViewModel PatientDocuments { get; set; }
    }
}