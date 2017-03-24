using System.ComponentModel.DataAnnotations;

namespace DentiCare.Models.UserSettings
{
    public class DisplayDetailsViewModel
    {
        [Display(Name = "Dane użytkownika")]
        public AccountDetails AccountDetails { get; set; }

        [Display(Name = "Dane personalne")]
        public PersonalDetails PersonalDetails { get; set; }

        [Display(Name = "Dane adresowe")]
        public AddressDetails AddressDetails { get; set; }

        [Display(Name = "Dane stomatologa")]
        public MedicalDetails MedicalDetails { get; set; }

        public string Photo { get; set; }
    }
}