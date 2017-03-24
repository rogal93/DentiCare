using System.ComponentModel.DataAnnotations;

namespace DentiCare.Models.UserSettings
{
    public class AddressDetails
    {
        [Display(Name = "AddressID")]
        public int AddressID { get; set; }

        [Display(Name = "Miejscowość")]
        [Required(ErrorMessage = "Miejscowość jest wymagane.")]
        [MaxLength(50, ErrorMessage = "Nazwa miejscowości nie może składać się z powyżej 50 liter.")]
        public string City { get; set; }

        [Display(Name = "Ulica")]
        [MaxLength(50, ErrorMessage = "Nazwa ulicy nie może składać się z powyżej 50 liter.")]
        public string Street { get; set; }

        [Display(Name = "Nr domu")]
        [Required(ErrorMessage = "Nr domu jest wymagany.")]
        [MaxLength(6)]
        public string HouseNumber { get; set; }

        [Display(Name = "Nr mieszkania")]
        [MaxLength(6)]
        public string ApartmentNumber { get; set; }

        [Display(Name = "Kod pocztowy")]
        [Required(ErrorMessage = "Kod pocztowy jest wymagany.")]
        [RegularExpression(@"\d{2}-\d{3}", ErrorMessage = "Zły format danych")]
        public string PostalCode { get; set; }

        [Display(Name = "Miejscowość poczty")]
        [MaxLength(50, ErrorMessage = "Nazwa miejscowości nie może składać się z powyżej 50 liter.")]
        public string PostalCity { get; set; }
    }
}