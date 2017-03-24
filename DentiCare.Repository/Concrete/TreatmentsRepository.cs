using DentiCare.Repository.Abstract;
using DentiCare.Repository.Entities;
using System.Data.Entity;
using System.Linq;

namespace DentiCare.Repository.Concrete
{
    public class TreatmentsRepository : ITreatmentsRepository
    {
        private readonly DentiCareDBContext _context = new DentiCareDBContext();

        public IQueryable<Treatment> Treatments
        {
            get { return _context.Treatments; }
        }

        public IQueryable<TreatmentCategory> TreatmentCategories
        {
            get { return _context.TreatmentCategories; }
        }

        #region Treatments methods

        public void CreateTreatment(Treatment treatment)
        {
            if (treatment != null)
            {
                _context.Entry(treatment).State = EntityState.Added;
                _context.Treatments.Add(treatment);
                _context.SaveChanges();
            }
        }

        public void EditTreatment(Treatment treatment)
        {
            if (treatment != null)
            {
                var treatmentToUpdate = _context.Treatments.FirstOrDefault(p => p.id == treatment.id);

                treatmentToUpdate.name = treatment.name;
                treatmentToUpdate.price = treatment.price;
                treatmentToUpdate.category_id = treatment.category_id;

                _context.Treatments.Attach(treatmentToUpdate);
                _context.Entry(treatmentToUpdate).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void DeleteTreatment(int id)
        {
            var treatmentToRemove = _context.Treatments.FirstOrDefault(p => p.id == id);

            if (treatmentToRemove != null)
            {
                _context.Entry(treatmentToRemove).State = EntityState.Deleted;
                _context.Treatments.Remove(treatmentToRemove);
                _context.SaveChanges();
            }
        }

        #endregion

        #region Categories methods

        public void CreateCategory(TreatmentCategory category)
        {
            if (category != null)
            {
                _context.Entry(category).State = EntityState.Added;
                _context.TreatmentCategories.Add(category);
                _context.SaveChanges();
            }
        }

        public void DeleteCategory(TreatmentCategory category)
        {
            if (category != null)
            {
                _context.Entry(category).State = EntityState.Deleted;
                _context.TreatmentCategories.Remove(category);
                _context.SaveChanges();
            }
        }

        #endregion
    }
}