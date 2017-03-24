using DentiCare.Repository.Entities;
using System.Linq;

namespace DentiCare.Repository.Abstract
{
    /// <summary>
    /// Interfejs, który umożliwia dodanie, edycje i usuwanie grafików stomatologów.
    /// </summary>
    public interface ISchedulesRepository
    {
        /// <summary>
        /// Kolekcja grafików stomatologów.
        /// </summary>
        IQueryable<Schedule> Schedules { get; }

        /// <summary>
        /// Metoda dodająca grafik dzienny stomatologa.
        /// </summary>
        /// <param name="schedule">Grafik dzienny.</param>
        void CreateSchedule(Schedule schedule);

        /// <summary>
        /// Metoda edytująca grafik dzienny stomatologa.
        /// </summary>
        /// <param name="schedule">Grafik dzienny.</param>
        void EditSchedule(Schedule schedule);

        /// <summary>
        /// Metoda usuwająca grafik dzienny.
        /// </summary>
        /// <param name="id">ID grafiku dziennego.</param>
        void DeleteSchedule(int id);
    }
}
