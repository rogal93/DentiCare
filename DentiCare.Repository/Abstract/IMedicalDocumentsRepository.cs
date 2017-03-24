using DentiCare.Repository.Entities;
using System.Linq;

namespace DentiCare.Repository.Abstract
{
    /// <summary>
    /// Interfejs, który umożliwia dodawanie i usuwanie dokumentów medycznych.
    /// </summary>
    public interface IMedicalDocumentsRepository
    {
        /// <summary>
        /// Kolekcja dokumentów medycznych.
        /// </summary>
        IQueryable<MedicalDocument> Documents { get; }

        /// <summary>
        /// Metoda dodająca dokument medyczny.
        /// </summary>
        /// <param name="doc">Dokument medyczny do dodania.</param>
        void CreateMedicalDocument(MedicalDocument doc);
        
        /// <summary>
        /// Metoda usuwająca dokument medyczny.
        /// </summary>
        /// <param name="id">ID dokumentu medycznego.</param>
        void DeleteMedicalExperience(int id);
    }
}
