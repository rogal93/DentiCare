using DentiCare.Repository.Entities;
using System.Linq;

namespace DentiCare.Repository.Abstract
{
    /// <summary>
    /// Interfejs, który umożliwia dodawanie, edycję i usuwanie adresów zamieszkania użytkowników aplikacji.
    /// </summary>
    public interface IAddressesRepository
    {
        /// <summary>
        /// Kolekcja adresów.
        /// </summary>
        IQueryable<Address> Addresses { get; }

        /// <summary>
        /// Metoda dodająca adres zamieszkania.
        /// </summary>
        /// <param name="address">Adres zamieszkania do dodania.</param>
        void CreateAddress(Address address);

        /// <summary>
        /// Metoda edytująca adres zamieszkania.
        /// </summary>
        /// <param name="address">Edytowany adres zamieszkania.</param>
        void EditAddress(Address address);

        /// <summary>
        /// Metoda usuwająca adres zamieszkania.
        /// </summary>
        /// <param name="id">ID adresu zamieszkania do usunięcia.</param>
        void DeleteAddress(int id);
    }
}
