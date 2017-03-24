using DentiCare.Repository.Abstract;
using DentiCare.Repository.Entities;
using System.Data.Entity;
using System.Linq;

namespace DentiCare.Repository.Concrete
{
    public class AppointmentsRepository : IAppointmentsRepository
    {
        private readonly DentiCareDBContext _context = new DentiCareDBContext();

        public IQueryable<Appointment> Appointments
        {
            get { return _context.Appointments; }
        }

        public void CreateAppointment(Appointment appointment)
        {
            if (appointment != null)
            {
                _context.Entry(appointment).State = EntityState.Added;
                _context.Appointments.Add(appointment);
                _context.SaveChanges();
            }
        }

        public void EditAppointment(Appointment appointment)
        {
            var appointmentToUpdate = _context.Appointments.FirstOrDefault(p => p.id == appointment.id);

            if (appointment != null)
            {
                appointmentToUpdate.comment = appointment.comment;
                appointmentToUpdate.price = appointment.price;

                _context.Appointments.Attach(appointmentToUpdate);
                _context.Entry(appointmentToUpdate).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void DeleteAppointment(int id)
        {
            var appointmentToRemove = _context.Appointments.FirstOrDefault(p => p.id == id);

            if (appointmentToRemove != null)
            {
                _context.Entry(appointmentToRemove).State = EntityState.Deleted;
                _context.Appointments.Remove(appointmentToRemove);
                _context.SaveChanges();
            }
        }
    }
}