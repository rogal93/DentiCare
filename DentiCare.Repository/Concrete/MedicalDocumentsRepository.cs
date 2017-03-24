using DentiCare.Repository.Abstract;
using DentiCare.Repository.Entities;
using System.Data.Entity;
using System.Linq;

namespace DentiCare.Repository.Concrete
{
    public class MedicalDocumentsRepository : IMedicalDocumentsRepository
    {
        private readonly DentiCareDBContext _context = new DentiCareDBContext();

        public IQueryable<MedicalDocument> Documents
        {
            get { return _context.Documents; }
        }

        public void CreateMedicalDocument(MedicalDocument doc)
        {
            if (doc != null)
            {
                _context.Entry(doc).State = EntityState.Added;
                _context.Documents.Add(doc);
                _context.SaveChanges();
            }
        }
        
        public void DeleteMedicalExperience(int id)
        {
            var doc = _context.Documents.FirstOrDefault(p => p.id == id);

            if (doc != null)
            {
                _context.Entry(doc).State = EntityState.Deleted;
                _context.Documents.Remove(doc);
                _context.SaveChanges();
            }
        }
    }
}