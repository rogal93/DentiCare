using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentiCare.Repository.Entities
{
    /// <summary>
    /// Adres zamieszkania użytkownika aplikacji.
    /// </summary>
    [Table("Addresses")]
    public class Address
    {
        /// <summary>
        /// ID adresu zamieszkania.
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// Kod pocztowy.
        /// </summary>
        public string postal_code { get; set; }

        /// <summary>
        /// Miasto.
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// Ulica.
        /// </summary>
        public string street { get; set; }

        /// <summary>
        /// Numer domu.
        /// </summary>
        public string house_number { get; set; }

        /// <summary>
        /// Numer mieszkania.
        /// </summary>
        public string apartment_number { get; set; }

        /// <summary>
        /// Miasto adresu pocztowego.
        /// </summary>
        public string postal_city { get; set; }
    }
}