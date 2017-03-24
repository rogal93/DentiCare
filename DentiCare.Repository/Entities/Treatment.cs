using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentiCare.Repository.Entities
{
    /// <summary>
    /// Rodzaj zabiegu stomatologicznego.
    /// </summary>
    [Table("Treatments")]
    public class Treatment
    {
        /// <summary>
        /// ID zabiegu.
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// Nazwa zabiegu stomatologicznego.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Cena zabiegu stomatologicznego.
        /// </summary>
        public double price { get; set; }

        /// <summary>
        /// Id kategorii zabiegu.
        /// </summary>
        [ForeignKey("Category")]
        public int category_id { get; set; }

        /// <summary>
        /// Kategoria zabiegu.
        /// </summary>
        public virtual TreatmentCategory Category { get; set; }
    }
}