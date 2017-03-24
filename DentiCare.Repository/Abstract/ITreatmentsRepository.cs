using DentiCare.Repository.Entities;
using System.Linq;

namespace DentiCare.Repository.Abstract
{
    /// <summary>
    /// Interfejs, który umożliwia dodanie, edycje i usuwanie zabiegów stomatologicznych.
    /// </summary>
    public interface ITreatmentsRepository
    {
        /// <summary>
        /// Kolekcja zabiegów stomatologicznych.
        /// </summary>
        IQueryable<Treatment> Treatments { get; }

        /// <summary>
        /// Kolekcja kategorii zabiegów.
        /// </summary>
        IQueryable<TreatmentCategory> TreatmentCategories { get; }

        /// <summary>
        /// Metoda dodająca zabieg stomatologiczny.
        /// </summary>
        /// <param name="treatment">Zabieg stomatologiczny do dodania.</param>
        void CreateTreatment(Treatment treatment);

        /// <summary>
        /// Metoda edytująca zabieg stomatologiczny.
        /// </summary>
        /// <param name="treatment">Edytowany zabieg stomatologiczny.</param>
        void EditTreatment(Treatment treatment);

        /// <summary>
        /// Metoda usuwająca zabieg stomatologiczny.
        /// </summary>
        /// <param name="id">ID zabiegu do usunięcia.</param>
        void DeleteTreatment(int id);

        /// <summary>
        /// Metoda dodająca kategorię zabiegów.
        /// </summary>
        /// <param name="category">Nazwa kategorii.</param>
        void CreateCategory(TreatmentCategory category);

        /// <summary>
        /// Metoda usuwająca kategorię zabiegów.
        /// </summary>
        /// <param name="category">Nazwa kategorii.</param>
        void DeleteCategory(TreatmentCategory category);
    }
}
