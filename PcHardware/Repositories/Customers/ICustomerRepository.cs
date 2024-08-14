using PcHardware.Models;

namespace PcHardware.Repositories.Customers
{
    public interface ICustomerRepository
    {
        List<ApplicationUser> GetCustomers();
        List<Models.Order> GetCustomerOrders(string Id);
        ApplicationUser GetCustomer(string Id);
    }
}
