using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace DentiCare.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Aktualne hasło jest wymagane.")]
        [DataType(DataType.Password)]
        [Display(Name = "Aktualne hasło")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Nowe hasło jest wymagane.")]
        [StringLength(100, ErrorMessage = "Hasło musi mieć przynajmniej 6 znaków.", MinimumLength = 6)]
        [DataType(DataType.Password, ErrorMessage = "Hasło musi mieć jedną wielką literę, jedną małą literę, jedną cyfrę i jeden znak niealfanumeryczny.")]
        [Display(Name = "Nowe hasło")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdzenie hasła")]
        [Compare("NewPassword", ErrorMessage = "Hasło i jego potwierdzenie nie są identyczne.")]
        public string ConfirmPassword { get; set; }
    }
    
    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}