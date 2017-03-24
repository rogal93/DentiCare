using DentiCare.Repository.Entities;
using System.Linq;

namespace DentiCare.Repository.Abstract
{
    /// <summary>
    /// Interfejs, który umożliwia dodanie, edycje i usuwanie wizyt stomatologicznych.
    /// </summary>
    public interface IAppointmentsRepository
    {
        /// <summary>
        /// Kolekcja wizyt stomatologicznych.
        /// </summary>
        IQueryable<Appointment> Appointments { get; }

        /// <summary>
        /// Metoda dodająca wizytę stomatologiczną.
        /// </summary>
        /// <param name="appointment">Wizyta stomatologiczna do dodania.</param>
        void CreateAppointment(Appointment appointment);

        /// <summary>
        /// Metoda edytująca wizytę stomatologiczną.
        /// </summary>
        /// <param name="appointment">Edytowana wizyta stomatologiczna.</param>
        void EditAppointment(Appointment appointment);

        /// <summary>
        /// Metoda usuwająca wizytę stomatologiczną.
        /// </summary>
        /// <param name="id">Id wizyty stomatologicznej do usunięcia.</param>
        void DeleteAppointment(int id);
    }
}
