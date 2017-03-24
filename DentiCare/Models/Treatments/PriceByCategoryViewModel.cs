using System.Collections.Generic;

namespace DentiCare.Models.Treatments
{
    public class PriceByCategoryViewModel
    {
        public string CategoryName { get; set; }
        public List<TreatmentPrice> Treatments { get; set; }
    }
}