using System.Collections.Generic;

namespace DentiCare.Models.Admin
{
    public class DisplayTreatmentsViewModel
    {
        public List<TreatmentModel> TreatmentViews { get; set; }
        public TreatmentDetailsModel Details { get; set; }

        public TreatmentDetailsModel Empty { get; set; }
    }
}