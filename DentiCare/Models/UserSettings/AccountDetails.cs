using System.ComponentModel.DataAnnotations;

namespace DentiCare.Models.UserSettings
{
    public class AccountDetails
    {
        public string ID { get; set; }

        [Display(Name = "Adres mailowy")]
        public string Email { get; set; }

        [Display(Name = "Telefon")]
        public string PhoneNumber { get; set; }
    }
}