using DentiCare.Repository.Abstract;
using DentiCare.Repository.Entities;
using System.Data.Entity;
using System.Linq;

namespace DentiCare.Repository.Concrete
{
    public class SchedulesRepository : ISchedulesRepository
    {
        private readonly DentiCareDBContext _context = new DentiCareDBContext();

        public IQueryable<Schedule> Schedules
        {
            get { return _context.Schedules; }
        }

        public void CreateSchedule(Schedule schedule)
        {
            if (schedule != null)
            {
                _context.Entry(schedule).State = EntityState.Added;
                _context.Schedules.Add(schedule);
                _context.SaveChanges();
            }
        }

        public void EditSchedule(Schedule schedule)
        {
            if (schedule != null)
            {
                var toUpdate = _context.Schedules.FirstOrDefault(p => p.id == schedule.id);

                toUpdate.hour_from = schedule.hour_from;
                toUpdate.hour_to = schedule.hour_to;
                toUpdate.day_off = schedule.day_off;

                
                _context.Schedules.Attach(toUpdate);
                _context.Entry(toUpdate).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void DeleteSchedule(int id)
        {
            var schedule = _context.Schedules.FirstOrDefault(p => p.id == id);

            if (schedule != null)
            {
                _context.Entry(schedule).State = EntityState.Deleted;
                _context.Schedules.Remove(schedule);
                _context.SaveChanges();
            }
        }
    }
}