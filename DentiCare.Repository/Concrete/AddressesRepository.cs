using DentiCare.Repository.Abstract;
using DentiCare.Repository.Entities;
using System.Data.Entity;
using System.Linq;

namespace DentiCare.Repository.Concrete
{
    public class AddressesRepository : IAddressesRepository
    {
        private readonly DentiCareDBContext _context = new DentiCareDBContext();

        public IQueryable<Address> Addresses
        {
            get { return _context.Addresses; }
        }

        public void CreateAddress(Address address)
        {
            if (address != null)
            {
                _context.Entry(address).State = EntityState.Added;
                _context.Addresses.Add(address);
                _context.SaveChanges();
            }
        }

        public void EditAddress(Address address)
        {
            if (address != null)
            {
                var addressToUpdate = _context.Addresses.FirstOrDefault(p => p.id == address.id);

                addressToUpdate.city = address.city;
                addressToUpdate.postal_code = address.postal_code;
                addressToUpdate.street = address.street;
                addressToUpdate.house_number = address.house_number;
                addressToUpdate.apartment_number = address.apartment_number;
                addressToUpdate.postal_city = address.postal_city;
                
                _context.Addresses.Attach(addressToUpdate);
                _context.Entry(addressToUpdate).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void DeleteAddress(int id)
        {
            var addressToRemove = _context.Addresses.FirstOrDefault(p => p.id == id);

            if (addressToRemove != null)
            {
                _context.Entry(addressToRemove).State = EntityState.Deleted;
                _context.Addresses.Remove(addressToRemove);
                _context.SaveChanges();
            }
        }
    }
}