using System.ComponentModel.DataAnnotations;

namespace DentiCare.Models.Admin
{
    public class UserModel
    {
        public string ID { get; set; }

        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Display(Name = "Rola")]
        public string Role { get; set; }
    }
}