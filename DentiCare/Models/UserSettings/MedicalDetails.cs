using System;
using System.ComponentModel.DataAnnotations;

namespace DentiCare.Models.UserSettings
{
    public class MedicalDetails
	{
        public int MedicalID { get; set; }

        [Display(Name = "Ukończona uczelnia")]
        [Required(ErrorMessage = "Nazwa uczelni jest wymagana.")]
        public string University { get; set; }

        [Display(Name = "Data ukończenia")]
        public DateTime GraduationDate { get; set; }

        [Display(Name = "Specjalizacja")]
        public string Specialization { get; set; }

        [Display(Name = "O mnie")]
        [Required(ErrorMessage = "Kilka słów o sobie jest wymagane.")]
        public string AboutMyself { get; set; }
	}
}