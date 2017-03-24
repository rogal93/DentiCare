using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentiCare.Repository.Entities
{
    /// <summary>
    /// Zdjęcie profilowe użytkownika.
    /// </summary>
    [Table("Photos")]
    public class Photo
    {
        /// <summary>
        /// ID zdjęcia profilowego.
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// Adres url zdjęcia profilowego.
        /// </summary>
        public string url { get; set; }
    }
}