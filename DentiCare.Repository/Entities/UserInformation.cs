using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DentiCare.Repository.Entities
{
    /// <summary>
    /// informacje o użytkowniku.
    /// </summary>
    [Table("UserInformations")]
    public class UserInformation
    {
        /// <summary>
        /// Id informacji o użytkowniku.
        /// </summary>
        [Key]
        public int id { get; set; }
        
        /// <summary>
        /// Imię użytkownika.
        /// </summary>
        public string first_name { get; set; }

        /// <summary>
        /// Nazwisko użytkownika.
        /// </summary>
        public string last_name { get; set; }

        /// <summary>
        /// ID adresu zamieszkania.
        /// </summary>
        [ForeignKey("Address")]
        public int? address_id { get; set; }

        /// <summary>
        /// Data rejestracji użytkownika.
        /// </summary>
        public DateTime register_date { get; set; }

        /// <summary>
        /// Płeć użytkownika.
        /// </summary>
        public string gender { get; set; }

        /// <summary>
        /// ID zdjęcia profilowego użytkownika.
        /// </summary>
        [ForeignKey("Photo")]
        public int? photo_id { get; set; }
        
        /// <summary>
        /// Adres użytkownika.
        /// </summary>
        public virtual Address Address { get; set; }

        /// <summary>
        /// Zdjęcie profilowe użytkownika.
        /// </summary>
        public virtual Photo Photo { get; set; }
    }
}