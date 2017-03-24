using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DentiCare.Models.SendMails
{
    public class MailFromPatientViewModel
    {
        public Dictionary<int, string> NamesOfDoctors { get; set; }

        [Display(Name = "Tytuł")]
        [Required]
        public string Title { get; set; }

        [Display(Name = "Treść")]
        [Required]
        public string Body { get; set; }

        [Display(Name = "lek. stom. ")]
        public int Receiver { get; set; }
    }
}