using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentiCare.Repository.Entities
{
    /// <summary>
    /// Dokument medyczny.
    /// </summary>
    [Table("MedicalDocuments")]
    public class MedicalDocument
    {
        /// <summary>
        /// ID dokumentu medycznego.
        /// </summary>
        [Key]
        public int id { get; set; }
        
        /// <summary>
        /// Opis dokumentu medycznego.
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// Data zapisania dokumentu medycznego.
        /// </summary>
        public DateTime upload_date { get; set; }

        /// <summary>
        /// Ścieżka do pliku dokumentu medycznego.
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// ID pacjenta, które dotyczy dokument.
        /// </summary>
        [ForeignKey("Patient")]
        public int patient_id { get; set; }

        /// <summary>
        /// Pacjent, którego dotyczy dokument.
        /// </summary>
        public virtual UserInformation Patient { get; set; }
    }
}