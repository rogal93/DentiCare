using DentiCare.Repository.Entities;
using System.Linq;

namespace DentiCare.Repository.Abstract
{
    /// <summary>
    /// Interfejs, który umożliwia dodanie, edycje i usuwanie danych użytkowników.
    /// </summary>
    public interface IUserInformationsRepository
    {
        /// <summary>
        /// Kolekcja danych użytkowników.
        /// </summary>
        IQueryable<UserInformation> UserInformations { get; }

        /// <summary>
        /// Metoda dodająca dane użytkownika.
        /// </summary>
        /// <param name="userInfo">Dane użytkownika.</param>
        void CreateUserInformation(UserInformation userInfo);

        /// <summary>
        /// Metoda edytująca dane użytkownika.
        /// </summary>
        /// <param name="userInfo">Dane uzytkownika.</param>
        void EditUserInformation(UserInformation userInfo);

        /// <summary>
        /// Metoda usuwająca dane użytkownika.
        /// </summary>
        /// <param name="id">ID danych użytkownika.</param>
        void DeleteUserInformation(int id);
    }
}
