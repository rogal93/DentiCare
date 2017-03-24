using DentiCare.Repository.Entities;
using System.Linq;

namespace DentiCare.Repository.Abstract
{
    /// <summary>
    /// Interfejs umożliwiający dodawanie, edytowanie i usuwanie zdjęć użytkowników.
    /// </summary>
    public interface IPhotosRepository
    {
        /// <summary>
        /// Kolekcja zdjęć.
        /// </summary>
        IQueryable<Photo> Photos { get; }

        /// <summary>
        /// Metoda dodająca zdjęcie.
        /// </summary>
        /// <param name="photo">Zdjęcie.</param>
        void AddPhoto(Photo photo);

        /// <summary>
        /// Metoda podmieniająca zdjęcie.
        /// </summary>
        /// <param name="photo">Nowe zdjęcie.</param>
        void ChangePhoto(Photo photo);

        /// <summary>
        /// Metoda usuwająca zdjęcie użytkownika.
        /// </summary>
        /// <param name="id">ID zdjęcia do usunięcia.</param>
        void DeletePhoto(int id);
    }
}