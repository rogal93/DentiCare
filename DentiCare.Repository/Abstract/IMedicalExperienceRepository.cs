using DentiCare.Repository.Entities;
using System.Linq;

namespace DentiCare.Repository.Abstract
{
    /// <summary>
    /// Interfejs, który umożliwia dodawanie, edycję i usuwanie informacji o stomatologu.
    /// </summary>
    public interface IMedicalExperienceRepository
    {
        /// <summary>
        /// Kolekcja danych o stomatologach.
        /// </summary>
        IQueryable<MedicalExperience> MedicalExperience { get; }

        /// <summary>
        /// Metoda dodająca dane stomatologa.
        /// </summary>
        /// <param name="exp">Dane stomatologa do dodania.</param>
        void CreateMedicalExperience(MedicalExperience exp);

        /// <summary>
        /// Metoda edytująca dane stomatologa.
        /// </summary>
        /// <param name="exp">Dane stomatologa do edycji.</param>
        void EditMedicalExperience(MedicalExperience exp);

        /// <summary>
        /// Metoda usuwająca dane stomatologa.
        /// </summary>
        /// <param name="id">ID danych stomatologa do usunięcia.</param>
        void DeleteMedicalExperience(int id);
    }
}
