using DentiCare.Repository.Abstract;
using DentiCare.Repository.Entities;
using System.Data.Entity;
using System.Linq;

namespace DentiCare.Repository.Concrete
{
    public class UserInformationsRepository : IUserInformationsRepository
    {
        private readonly DentiCareDBContext _context = new DentiCareDBContext();

        public IQueryable<UserInformation> UserInformations
        {
            get { return _context.UserInformations; }
        }

        public void CreateUserInformation(UserInformation userInfo)
        {
            if (userInfo != null)
            {
                _context.Entry(userInfo).State = EntityState.Added;
                _context.UserInformations.Add(userInfo);
                _context.SaveChanges();
            }
        }
        
        public void EditUserInformation(UserInformation userInfo)
        {
            if (userInfo != null)
            {
                var infoToUpdate = _context.UserInformations.FirstOrDefault(p => p.id == userInfo.id);

                infoToUpdate.first_name = userInfo.first_name;
                infoToUpdate.last_name = userInfo.last_name;
                infoToUpdate.register_date = userInfo.register_date;
                infoToUpdate.gender = userInfo.gender;
                infoToUpdate.photo_id = userInfo.photo_id;

                _context.UserInformations.Attach(infoToUpdate);
                _context.Entry(infoToUpdate).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void DeleteUserInformation(int id)
        {
            var userInfo = _context.UserInformations.FirstOrDefault(p => p.id == id);

            if (userInfo != null)
            {
                _context.Entry(userInfo).State = EntityState.Deleted;
                _context.UserInformations.Remove(userInfo);
                _context.SaveChanges();
            }
        }
    }
}