using DentiCare.Repository.Abstract;
using DentiCare.Repository.Entities;
using System.Data.Entity;
using System.Linq;

namespace DentiCare.Repository.Concrete
{
    public class MedicalExperienceRepository : IMedicalExperienceRepository
    {
        private readonly DentiCareDBContext _context = new DentiCareDBContext();

        public IQueryable<MedicalExperience> MedicalExperience
        {
            get { return _context.MedicalExperience; }
        }

        public void CreateMedicalExperience(MedicalExperience exp)
        {
            if (exp != null)
            {
                _context.Entry(MedicalExperience).State = EntityState.Added;
                _context.MedicalExperience.Add(exp);
                _context.SaveChanges();
            }
        }

        public void EditMedicalExperience(MedicalExperience exp)
        {
            if (exp != null)
            {
                var expToUpdate = _context.MedicalExperience.FirstOrDefault(x => x.id == exp.id);

                expToUpdate.university = exp.university;
                expToUpdate.graduation_date = exp.graduation_date;
                expToUpdate.specialization = exp.specialization;
                expToUpdate.about_myself = exp.about_myself;

                _context.MedicalExperience.Attach(expToUpdate);
                _context.Entry(expToUpdate).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void DeleteMedicalExperience(int id)
        {
            var exp = _context.MedicalExperience.FirstOrDefault(p => p.id == id);

            if (exp != null)
            {
                _context.Entry(exp).State = EntityState.Deleted;
                _context.MedicalExperience.Remove(exp);
                _context.SaveChanges();
            }
        }
    }
}