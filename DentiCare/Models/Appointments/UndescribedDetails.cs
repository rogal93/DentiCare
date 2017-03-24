using System;
using System.ComponentModel.DataAnnotations;

namespace DentiCare.Models.Appointments
{
    public class UndescribedDetails
    {
        public int AppointmentID { get; set; }

        [Display(Name = "Komentarz")]
        [Required(ErrorMessage = "Komentarz jest wymagany.")]
        public string Comment { get; set; }

        [Display(Name = "Koszt")]
        [Required(ErrorMessage = "Koszt jest wymagany.")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "Zły format kosztu.")]
        public double Price { get; set; }
    }
}