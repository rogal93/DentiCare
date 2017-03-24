using System.Collections.Generic;

namespace DentiCare.Models.Patients
{
    public class PatientDocumentsViewModel
    {
        public List<DocumentModel> Documents { get; set; }
        public DocumentModel NewDocument { get; set; }

        public DownloadModel Download { get; set; }
    }
}