using System.ComponentModel.DataAnnotations;

namespace DentiCare.Models.Admin
{
    public class TreatmentDetailsModel
    {
        public int TreatmentID { get; set; }

        [Display(Name = "Nazwa zabiegu")]
        public string Name { get; set; }

        [Display(Name = "Cena")]
        public double Price { get; set; }

        [Display(Name = "Kategoria")]
        public string Category { get; set; }
    }
}