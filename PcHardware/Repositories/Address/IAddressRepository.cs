namespace PcHardware.Repositories.Address
{
    public interface IAddressRepository
    {
        List<Models.Address> GetAddresses();
        Models.Address GetAddressById(int Id);
        List<Models.Address> GetUserAddresses(string Id);
        void AddAddress(Models.Address address);
        void RemoveAddress(int Id);
        void EditAddress(Models.Address address);
    }
}
