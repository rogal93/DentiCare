using DentiCare.Repository.Abstract;
using DentiCare.Repository.Entities;
using System.Data.Entity;
using System.Linq;

namespace DentiCare.Repository.Concrete
{
    public class PhotosRepository : IPhotosRepository
    {
        private readonly DentiCareDBContext _context = new DentiCareDBContext();

        public IQueryable<Photo> Photos
        {
            get { return _context.Photos; }
        }

        public void AddPhoto(Photo photo)
        {
            if (photo != null)
            {
                _context.Entry(photo).State = EntityState.Added;
                _context.Photos.Add(photo);
                _context.SaveChanges();
            }
        }

        public void ChangePhoto(Photo photo)
        {
            if (photo != null)
            {
                var photoToUpdate = _context.Photos.FirstOrDefault(p => p.id == photo.id);

                photoToUpdate.url = photo.url;

                _context.Photos.Attach(photoToUpdate);
                _context.Entry(photoToUpdate).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void DeletePhoto(int id)
        {
            var photoToRemove = _context.Photos.FirstOrDefault(p => p.id == id);

            if (photoToRemove != null)
            {
                _context.Entry(photoToRemove).State = EntityState.Deleted;
                _context.Photos.Remove(photoToRemove);
                _context.SaveChanges();
            }
        }
    }
}