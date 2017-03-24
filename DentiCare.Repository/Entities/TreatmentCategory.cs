using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentiCare.Repository.Entities
{
    /// <summary>
    /// Kategoria zabiegu stomatologicznego.
    /// </summary>
    [Table("TreatmentsCategories")]
    public class TreatmentCategory
    {
        /// <summary>
        /// ID kategorii.
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// Nazwa kategorii.
        /// </summary>
        public string name { get; set; }
    }
}