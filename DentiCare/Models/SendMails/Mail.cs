using System.ComponentModel.DataAnnotations;

namespace DentiCare.Models.SendMails
{
    public class Mail
    {
        [Display(Name = "Tytuł")]
        [Required]
        public string Title { get; set; }

        [Display(Name = "Treść")]
        [Required]
        public string Body { get; set; }

        public int Receiver { get; set; }
    }
}