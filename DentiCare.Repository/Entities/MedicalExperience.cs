using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentiCare.Repository.Entities
{
    /// <summary>
    /// Informacje o stomatologu.
    /// </summary>
    [Table("MedicalExperience")]
    public class MedicalExperience
    {
        /// <summary>
        /// ID danych stomatologa.
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// Ukończona uczelnia.
        /// </summary>
        public string university { get; set; }

        /// <summary>
        /// Data ukończenia uczelni.
        /// </summary>
        public DateTime graduation_date { get; set; }

        /// <summary>
        /// Specjalizacja stomatologiczna.
        /// </summary>
        public string specialization { get; set; }

        /// <summary>
        /// Kilka słów o sobie. 
        /// </summary>
        public string about_myself { get; set; }
    }
}