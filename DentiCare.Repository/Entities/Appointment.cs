using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentiCare.Repository.Entities
{
    /// <summary>
    /// Umówiona wizyta na zabieg stomatologiczny.
    /// </summary>
    [Table("Appointments")]
    public class Appointment
    {
        /// <summary>
        /// ID wizyty stomatologicznej.
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// ID pacjenta obecnego na wizycie stomatologicznej.
        /// </summary>
        [ForeignKey("Patient")]
        public int patient_id { get; set; }

        /// <summary>
        /// ID lekarza obecnego na wizycie stomatologicznej.
        /// </summary>
        [ForeignKey("Doctor")]
        public int doctor_id { get; set; }

        /// <summary>
        /// Data wizyty stomatologicznej.
        /// </summary>
        public DateTime date { get; set; }

        /// <summary>
        /// Komentarz do wizyty stomatologicznej.
        /// </summary>
        public string comment { get; set; }

        /// <summary>
        /// Koszt wizyty stomatologicznej.
        /// </summary>
        public double price { get; set; }

        /// <summary>
        /// Pacjent obecny na wizycie stomatologicznej.
        /// </summary>
        public virtual UserInformation Patient { get; set; }

        /// <summary>
        /// Lekarz obecny na wizycie stomatologicznej.
        /// </summary>
        public virtual UserInformation Doctor { get; set; }
    }
}