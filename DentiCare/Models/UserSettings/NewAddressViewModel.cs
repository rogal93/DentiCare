using System.ComponentModel.DataAnnotations;

namespace DentiCare.Models.UserSettings
{
    public class NewAddressViewModel
    {
        [RegularExpression(@"([0-9]{3})\-?[-]?([0-9]{3})[-]\-?([0-9]{3})", ErrorMessage = "Format numeru to 000-000-000.")]
        [Required(ErrorMessage = "Nr telefonu jest wymagany.")]
        [Display(Name = "Nr telefonu")]
        public string PhoneNumber { get; set; }
        

        [MaxLength(30, ErrorMessage = "Nazwa miasta nie może składać się z powyżej 30 liter.")]
        [Required(ErrorMessage = "Miejscowość jest wymagana.")]
        [Display(Name = "Miejscowość")]
        public string City { get; set; }

        [MaxLength(30, ErrorMessage = "Nazwa ulicy nie może składać się z powyżej 30 liter.")]
        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [MaxLength(4)]
        [Required(ErrorMessage = "Nr domu jest wymagany.")]
        [Display(Name = "Nr domu")]
        public string HouseNumber { get; set; }

        [MaxLength(4)]
        [Display(Name = "Nr mieszkania")]
        public string ApartamentNumber { get; set; }


        [RegularExpression(@"([0-9]{2})\-?[-]?([0-9]{3})", ErrorMessage = "Zły format danych")]
        [Required(ErrorMessage = "Kod pocztowy jest wymagany.")]
        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }

        [MaxLength(30, ErrorMessage = "Nazwa miasta nie może składać się z powyżej 30 liter.")]
        [Required(ErrorMessage = "Miejscowość poczty jest wymagana.")]
        [Display(Name = "Miejscowość poczty")]
        public string PostalCity { get; set; }
    }
}