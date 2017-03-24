using System.ComponentModel.DataAnnotations;

namespace DentiCare.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Adres mailowy jest wymagany")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Display(Name = "Zapamiętaj mnie")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        #region User fields

        [Required(ErrorMessage = "Adres mailowy jest wymagany.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [StringLength(100, ErrorMessage = "Hasło musi mieć przynajmniej 6 znaków.", MinimumLength = 6)]
        [DataType(DataType.Password, ErrorMessage = "Hasło musi mieć jedną wielką literę, jedną małą literę, jedną cyfrę i jeden znak niealfanumeryczny.")]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password, ErrorMessage = "Hasło musi mieć jedną wielką literę, jedną małą literę, jedną cyfrę i jeden znak niealfanumeryczny.")]
        [Display(Name = "Potwierdź")]
        [Compare("Password", ErrorMessage = "Hasło i jego potwierdzenie nie są identyczne.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Numer telefonu jest wymagany.")]
        [RegularExpression(@"([0-9]{3})\-?[-]?([0-9]{3})[-]\-?([0-9]{3})", ErrorMessage = "Format numeru to 000-000-000.")]
        [Display(Name = "Nr telefonu")]
        public string PhoneNumber { get; set; }

        #endregion

        #region UserInformation fields
        
        [Required(ErrorMessage = "Imię jest wymagane.")]
        [MaxLength(30, ErrorMessage = "Imię nie może być dłuższe niż 30 liter.")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [MaxLength(30, ErrorMessage = "Nazwisko nie może być dłuższe niż 30 liter.")]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Płeć jest wymagana.")]
        [Display(Name = "Płeć")]
        public string Gender { get; set; }

        #endregion

        #region Address fields

        [Required(ErrorMessage = "Miejscowość jest wymagana.")]
        [MaxLength(30, ErrorMessage = "Nazwa miasta nie może składać się z powyżej 30 liter.")]
        [Display(Name = "Miejscowość")]
        public string City { get; set; }

        [Required(ErrorMessage = "Kod pocztowy jest wymagany.")]
        [RegularExpression(@"([0-9]{2})\-?[-]?([0-9]{3})", ErrorMessage = "Zły format danych")]
        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Miejscowość poczty jest wymagana.")]
        [MaxLength(30, ErrorMessage = "Nazwa miasta nie może składać się z powyżej 30 liter.")]
        [Display(Name = "Miejscowość poczty")]
        public string PostalCity { get; set; }

        [MaxLength(30, ErrorMessage = "Nazwa ulicy nie może składać się z powyżej 30 liter.")]
        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Nr domu jest wymagany.")]
        [MaxLength(4)]
        [Display(Name = "Nr domu")]
        public string HouseNumber { get; set; }

        [MaxLength(4)]
        [Display(Name = "Nr mieszkania")]
        public string ApartmentNumber { get; set; }

        #endregion
    }
    
    public class ForgotViewModel
    {
        [Required(ErrorMessage = "Adres mailowy jest wymagany.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Adres mailowy jest wymagany.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [StringLength(100, ErrorMessage = "Hasło musi mieć przynajmniej 6 znaków.", MinimumLength = 6)]
        [DataType(DataType.Password, ErrorMessage = "Hasło musi mieć jedną wielką literę, jedną małą literę, jedną cyfrę i jeden znak niealfanumeryczny.")]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password, ErrorMessage = "Hasło musi mieć jedną wielką literę, jedną małą literę, jedną cyfrę i jeden znak niealfanumeryczny.")]
        [Display(Name = "Potwierdź")]
        [Compare("Password", ErrorMessage = "Hasło i jego potwierdzenie nie są identyczne.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Adres mailowy jest wymagany.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
