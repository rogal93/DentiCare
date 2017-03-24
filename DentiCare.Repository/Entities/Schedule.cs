using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentiCare.Repository.Entities
{
    /// <summary>
    /// Grafik dzienny.
    /// </summary>
    [Table("Schedules")]
    public class Schedule
    {
        /// <summary>
        /// Id grafiku dnia dla określonego stomatologa.
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// Id stomatologa, którego dotyczy grafik.
        /// </summary>
        [ForeignKey("Doctor")]
        public int doctor_id { get; set; }

        /// <summary>
        /// Wartość wskazująca na to, czy jest to dzień wolny dla stomatologa.
        /// </summary>
        public bool day_off { get; set; }

        /// <summary>
        /// Dzień grafiku.
        /// </summary>
        public DateTime date { get; set; }

        /// <summary>
        /// Godzina rozpoczęcia pracy danego dnia.
        /// </summary>
        public int hour_from { get; set; }

        /// <summary>
        /// Godzina zakończenia pracy danego dnia.
        /// </summary>
        public int hour_to { get; set; }

        /// <summary>
        /// Doktor, którego dotyczy grafik.
        /// </summary>
        public virtual UserInformation Doctor { get; set; }
    }
}