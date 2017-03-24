using System;
using System.ComponentModel.DataAnnotations;

namespace DentiCare.Models.UserSettings
{
    public class PersonalDetails
    {
        public int PersonalID { get; set; }

        [Display(Name = "Imię")]
        [Required(ErrorMessage = "Imię jest wymagane.")]
        [MaxLength(30, ErrorMessage = "Imię nie może być dłuższe niż 30 liter.")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [MaxLength(30, ErrorMessage = "Nazwisko nie może być dłuższe niż 30 liter.")]
        public string LastName { get; set; }

        [Display(Name = "Data rejestracji")]
        public DateTime RegisterDate { get; set; }

        [Display(Name = "Płeć")]
        [Required(ErrorMessage = "Płeć jest wymagana.")]
        public string Gender { get; set; }
    }
}