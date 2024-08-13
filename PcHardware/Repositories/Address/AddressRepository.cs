using PcHardware.Services;

namespace PcHardware.Repositories.Address
{
    public class AddressRepository : IAddressRepository
    {
        private readonly MyDbContext dbContext;
        public AddressRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        void IAddressRepository.AddAddress(Models.Address address)
        {
            dbContext.Addresses.Add(address);
            dbContext.SaveChanges();
        }

        void IAddressRepository.EditAddress(Models.Address address)
        {
            dbContext.Addresses.Update(address);
            dbContext.SaveChanges();
        }

        Models.Address IAddressRepository.GetAddressById(int Id)
        {
            return dbContext.Addresses.FirstOrDefault(a => a.Id == Id);
        }

        List<Models.Address> IAddressRepository.GetAddresses()
        {
            return dbContext.Addresses.ToList();
        }

        List<Models.Address> IAddressRepository.GetUserAddresses(string Id)
        {
            return dbContext.Addresses.Where(a => a.UserId == Id).ToList();
        }

        void IAddressRepository.RemoveAddress(int Id)
        {
            var address = dbContext.Addresses.Where(a => a.Id == Id).FirstOrDefault();
            dbContext.Addresses.Remove(address);
            dbContext.SaveChanges();
        }
    }
}
